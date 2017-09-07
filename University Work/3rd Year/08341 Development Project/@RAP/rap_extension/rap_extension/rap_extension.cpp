// extensionTest.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <string>
#include <unordered_map>
#include <thread>
#include <mutex>
#include <atomic>
#include <fstream>
#include <math.h>
#include <vector>
#include <ctime>

#define PI 3.14159265

using namespace std;

struct Data
{
	bool ready = false;
	string params = "";
	string result = "";

	vector<string> position, velocity, direction, selection, ammo, paramsVector;
	string target = "", shooter = "", bullet = "", surface = "";
	int radius = 0;
	bool direct = false;
};

struct TankProjectileData
{
	vector<double> projectileVelocity3D; // projectile velocity in 3D vector, feet / second
	vector<double> projectilePosition3D; // position of projectile
	vector<double> projectileVelocityReflected3D;
	vector<double> projectileVelocityNormal3D;
	vector<double> projectileVelocityReflectedNormal3D;	
	vector<double> direction; // vector perpendicular to armour impacted

	string tankType; // classname of target vehicles
	string projectileType; // classname of projectile

	double projectileVelocity = 0; // velocity of projectile, m/s
	double projectileVelocityReflected = 0;
	double projectileWeight = 4.8; // weight of projectile, kg
	double projectileDiameter = 0.022; // diameter of projectile, m
	double penetrationDepth = 0; // thickness of armour penetrated, m
	double angleOfImpact = 0; // angle of incidence between projectile and armour normal

	double projectileMagnitude = 0;
	double directionMagnitude = 0;

	const double K_CONST = 9000000000; // 8973010000 rolled homogenous armour constant
	const double N_CONST = 1.4; // misc constant
	
	double tankArmourThickness = 0.6; // thickness of armour, in m (temp)
	double LOSTankArmourThickness = 0;

	// Damage variables

	bool penetration = false;
	double damageToTank = 0;
};

unordered_map<long int, Data> tickets;
mutex mtx;

atomic<bool> worker_working(false);
long int id = 0; // global ticket id
long int cur_id = 0; // current ticket id

double LOSThickness(const double &armourThickness, const double &angle);
double vectorAngle(const vector<double> &vector1, const vector<double> &vector2);
double vectorDotProduct(const vector<double> &vector1, const vector<double> &vector2);
double vectorMagnitude(const vector<double> &vector);

void vectorReflect(const vector<double> &incidenceVector, const vector<double> &normalVector, vector<double> &reflectedVector);
void NormaliseVector(vector<double> &vector);
void PrintValue(const double &value, const string &identifier);

extern "C"
{
	__declspec (dllexport) void __stdcall RVExtension(char *output, int outputSize, const char *function);
}

double LOSThickness(const double &armourThickness, const double &angle) 
{
	double result = 0;

	// convert degress to radians
	double angleRads = angle * (PI / 180);

	// armour los thickness = armour thickness / cos angle
	result = armourThickness / (cos(angleRads));

	return result;
}

void vectorReflect(const vector<double> &incidenceVector, const vector<double> &normalVector, vector<double> &reflectedVector)
{
	// first reflect incidence vector

	// r = -2 * ( v dot n ) * n + v
	// r = reflected vector
	// v = incidence vector
	// n = normal vector

	double tempDotProduct = vectorDotProduct(incidenceVector, normalVector);

	reflectedVector[0] = -2 * tempDotProduct * normalVector[0] + incidenceVector[0];
	reflectedVector[1] = -2 * tempDotProduct * normalVector[1] + incidenceVector[1];
	reflectedVector[2] = -2 * tempDotProduct * normalVector[2] + incidenceVector[2];
}

double vectorMagnitude(const vector<double> &vector)
{
	// using pythagors sqrt of (x^2 + y^2 + z^2)
	double X2 = vector[0] * vector[0];
	double Y2 = vector[1] * vector[1];
	double Z2 = vector[2] * vector[2];

	double temp1 = X2 + Y2 + Z2;

	double result = sqrt(temp1);

	return result;
}

double vectorAngle(const vector<double> &vector1,const vector<double> &vector2)
{
	// dot product
	double tempDotProduct1 = vectorDotProduct(vector1, vector2);

	// calculate angle and convert to degrees
	double result = acos(tempDotProduct1) * (180 / PI);	

	// 180 - angle
	// because direction and projectile vector are opposites
	result = 180 - result;
	return result;
}

double vectorDotProduct(const vector<double> &vector1,const vector<double> &vector2)
{
	double X1X2 = vector1[0] * vector2[0];
	double Y1Y2 = vector1[1] * vector2[1];
	double Z1Z2 = vector1[2] * vector2[2];
	
	double dotProduct = X1X2 + Y1Y2 + Z1Z2;

	return dotProduct;
}

void NormaliseVector(vector<double> &vector)
{
	// calculate length of vector
	double tempLength = vectorMagnitude(vector);

	vector[0] = vector[0] / tempLength;
	vector[1] = vector[1] / tempLength;
	vector[2] = vector[2] / tempLength;
}

void ParseStringVector(vector<string> &vector, string delimiter, string line, string token, size_t pos)
{
	// erase brackets
	line.erase(line.find("["), 1);
	line.erase(line.find("]"), 1);

	// seperate input by delimiter and add to parsed string array
	while ((pos = line.find(delimiter)) != string::npos) {
		token = line.substr(0, pos);
		line.erase(0, token.length() + delimiter.length());
		vector.push_back(token);
	}

	// add final part of input to parsed string array
	vector.push_back(line);
}

void ParseInputString(Data &data)
{
	size_t delimiterPos = 0;
	int lineNo = 0;
	ofstream fileOut;
	string line = "", token = "", delimiter = ":";

	// clean up messy string (remove quotations marks)
	while ((delimiterPos = data.params.find('\"')) != string::npos)
	{
		data.params.erase(delimiterPos, 1);
	}

	// add to string array
	while ((delimiterPos = data.params.find('|')) != string::npos)
	{
		line = data.params.substr(0, delimiterPos);
		data.paramsVector.push_back(line);
		data.params.erase(0, delimiterPos + 1);

		// get token (target, shooter etc..) & erase it
		if ((delimiterPos = line.find(delimiter)) != string::npos) {
			token = line.substr(0, delimiterPos);
			line.erase(0, delimiter.length() + delimiterPos);
		}
		if (token == "Target") {
			data.target = line;
		}
		else if (token == "Shooter") {
			data.shooter = line;
		}
		else if (token == "Bullet") {
			data.bullet = line;
		}
		else if (token == "Surface") {
			data.surface = line;
		}
		else if (token == "Radius") {
			data.radius = atoi(line.c_str());
		}
		else if (token == "Direct") {
			if (line == "true")
				data.direct = true;
			else
				data.direct = false;
		}
		else if (token == "Position") {
			ParseStringVector(data.position, ",", line, token, delimiterPos);
		}
		else if (token == "Velocity") {
			ParseStringVector(data.velocity, ",", line, token, delimiterPos);
		}
		else if (token == "Direction") {
			ParseStringVector(data.direction, ",", line, token, delimiterPos);
		}
		else if (token == "Selection") {
			ParseStringVector(data.selection, ",", line, token, delimiterPos);
		}
		else if (token == "Ammo") {
			ParseStringVector(data.ammo, ",", line, token, delimiterPos);
		}
		lineNo++;
	}

}

void DeMarreEquation(TankProjectileData &data)
{
	double temp1 = (data.projectileWeight) * (pow(data.projectileVelocity, 2)); // w * v^2
	double temp2 = (data.K_CONST) * (pow(data.projectileDiameter, 3)); // k * d^3
	double temp3 = (temp1) / (temp2); // (w * v^2) / (k * d^3)
	double temp4 = pow(temp3, 1 / data.N_CONST);
	data.penetrationDepth = data.projectileDiameter * temp4;
}

void DataConversion(Data &data, TankProjectileData &tankData)
{
	// projectile type
	tankData.projectileType = data.ammo[4];

	// tank type
	tankData.tankType = data.target;

	// direction
	for (unsigned int i = 0; i < data.direction.size(); i++)
	{
		tankData.direction.push_back(stof(data.direction[i]));
	}

	// projectile velocity
	for (unsigned int i = 0; i < data.velocity.size(); i++)
	{
		tankData.projectileVelocity3D.push_back(stof(data.velocity[i]));
	}

	// projectile position
	for (unsigned int i = 0; i < data.position.size(); i++)
	{
		tankData.projectilePosition3D.push_back(stof(data.position[i]));
	}

	// get magnitude of velocity 3D
	tankData.projectileVelocity = pow(tankData.projectileVelocity3D[1], 2) + pow(tankData.projectileVelocity3D[2], 2) + pow(tankData.projectileVelocity3D[3], 2);
	tankData.projectileVelocity = sqrt(tankData.projectileVelocity);

	// get normalised vectors
	tankData.projectileVelocityNormal3D = tankData.projectileVelocity3D;
	NormaliseVector(tankData.projectileVelocityNormal3D);

	// get angle between velocity and armour angle
	tankData.angleOfImpact = vectorAngle(tankData.projectileVelocityNormal3D, tankData.direction);

	// get magnitudes
	tankData.projectileMagnitude = vectorMagnitude(tankData.projectileVelocityNormal3D);
	tankData.directionMagnitude = vectorMagnitude(tankData.direction);

	// get LOS armour thickness
	tankData.LOSTankArmourThickness = LOSThickness(tankData.tankArmourThickness, tankData.angleOfImpact);

	// set up vectors
	tankData.projectileVelocityReflected3D = tankData.projectileVelocity3D;
	tankData.projectileVelocityReflectedNormal3D = tankData.projectileVelocity3D;
}

void HandleDamage(TankProjectileData &data)
{
	// if penetration has occured then handle damage
	if (data.penetrationDepth > data.LOSTankArmourThickness)
	{
		data.penetration = true;

		// calculate amount of damage
		// difference between penetration and tank thickness, divided by tank thickness
		// damage values range from 0 - 1
		double tempDamage;
		tempDamage = (data.penetrationDepth - data.LOSTankArmourThickness) / data.LOSTankArmourThickness;
		data.damageToTank = tempDamage;
	}

	if (data.penetrationDepth < data.LOSTankArmourThickness)
	{
		data.penetration = false;
		vectorReflect(data.projectileVelocityNormal3D, data.direction, data.projectileVelocityReflectedNormal3D);

		double startKE = (data.projectileWeight * pow(data.projectileVelocity, 2)) / 2;

		double angleTemp = sin(data.angleOfImpact * (PI / 180));

		double endKE = pow(angleTemp, 2) * startKE;

		double percentLossKE = endKE / startKE;

		double temp1 = endKE / (0.5 * data.projectileWeight);
		double newProjectileVelocity = sqrt(temp1);	

		data.projectileVelocityReflected = newProjectileVelocity;

		data.projectileVelocityReflected3D[0] = data.projectileVelocityReflectedNormal3D[0] * newProjectileVelocity;
		data.projectileVelocityReflected3D[1] = data.projectileVelocityReflectedNormal3D[1] * newProjectileVelocity;
		data.projectileVelocityReflected3D[2] = data.projectileVelocityReflectedNormal3D[2] * newProjectileVelocity;
	}
}

void PrintDataStruct(const Data &data)
{
	ofstream fileOut;
	fileOut.open("debug_datastruct.txt", ios_base::app);
	char s[50];
	std::time_t  t = std::time(NULL);
	errno_t e = ctime_s(s, 50, &t);

	fileOut << "Extension Input Data, Ticket ID [" << cur_id << "], Date: " << s;
	fileOut << "bool ready: " << data.ready << endl;
	fileOut << "string params: " << data.params << endl;
	fileOut << "string result: " << data.result << endl;
	fileOut << "string target: " << data.target << endl;
	fileOut << "string shooter: " << data.shooter << endl;
	fileOut << "string bullet: " << data.bullet << endl;
	fileOut << "string surface: " << data.surface << endl;

	fileOut << "string[] position: ";
	for (unsigned int i = 0; i < data.position.size(); i++)
	{
		fileOut << data.position[i] << ", ";
	}
	fileOut << endl;

	fileOut << "string[] velocity: ";
	for (unsigned int i = 0; i < data.velocity.size(); i++)
	{
		fileOut << data.velocity[i] << ", ";
	}
	fileOut << endl;

	fileOut << "string[] direction: ";
	for (unsigned int i = 0; i < data.direction.size(); i++)
	{
		fileOut << data.direction[i] << ", ";
	}
	fileOut << endl;

	fileOut << "string[] selection: ";
	for (unsigned int i = 0; i < data.selection.size(); i++)
	{
		fileOut << data.selection[i] << ", ";
	}
	fileOut << endl;

	fileOut << "string[] ammo: ";
	for (unsigned int i = 0; i < data.ammo.size(); i++)
	{
		fileOut << data.ammo[i] << ", ";
	}
	fileOut << endl;

	fileOut << "string[] paramsVector: ";
	for (unsigned int i = 0; i < data.paramsVector.size(); i++)
	{
		fileOut << data.paramsVector[i] << ", ";
	}
	fileOut << endl;

	fileOut << "int radius: " << data.radius << endl;
	fileOut << "bool direct: " << data.direct << endl;
	fileOut << endl;

	fileOut.close();
}

void PrintValue(const double &value, const string &identifier)
{
	ofstream fileOut;
	fileOut.open("debug_value.txt", ios_base::app);
	fileOut << identifier << ": " << value << endl;
	fileOut.close();
}

void PrintTankProjectileDataStruct(const TankProjectileData &data)
{
	ofstream fileOut;
	fileOut.open("debug_tankprojectiledatastruct.txt", ios_base::app);
	char s[50];
	std::time_t  t = std::time(NULL);
	errno_t e = ctime_s(s, 50, &t);
	
	fileOut << "Tank Projectile Data, Ticket ID [" << cur_id << "], Date: " << s;
	fileOut << "Tank Type: " + data.tankType << endl;
	fileOut << "Projectile Type: " + data.projectileType << endl;
	fileOut << "Projectile Velocity: " + to_string(data.projectileVelocity) << endl;
	fileOut << "Projectile Velocity Reflected: " + to_string(data.projectileVelocityReflected) << endl;

	fileOut << "Projectile Velocity 3D:";
	for (unsigned int i = 0; i < data.projectileVelocity3D.size(); i++)
	{
		fileOut << " " << data.projectileVelocity3D[i];
	}
	fileOut << endl;

	fileOut << "Projectile Velocity Normalised 3D:";
	for (unsigned int i = 0; i < data.projectileVelocityNormal3D.size(); i++)
	{
		fileOut << " " << data.projectileVelocityNormal3D[i];
	}
	fileOut << endl;

	fileOut << "Projectile Velocity Reflected 3D:";
	for (unsigned int i = 0; i < data.projectileVelocityReflected3D.size(); i++)
	{
		fileOut << " " << data.projectileVelocityReflected3D[i];
	}
	fileOut << endl;

	fileOut << "Projectile Velocity Reflected Normalised 3D:";
	for (unsigned int i = 0; i < data.projectileVelocityReflectedNormal3D.size(); i++)
	{
		fileOut << " " << data.projectileVelocityReflectedNormal3D[i];
	}
	fileOut << endl;

	fileOut << "Projectile Position 3D:";
	for (unsigned int i = 0; i < data.projectilePosition3D.size(); i++)
	{
		fileOut << " " << data.projectilePosition3D[i];
	}
	fileOut << endl;

	fileOut << "Direction:";
	for (unsigned int i = 0; i < data.direction.size(); i++)
	{
		fileOut << " " << data.direction[i];
	}
	fileOut << endl;

	fileOut << "Projectile Weight: " << data.projectileWeight << endl;
	fileOut << "Projectile Diameter: " << data.projectileDiameter << endl;
	fileOut << "Penetration Depth: " << data.penetrationDepth << endl;
	fileOut << "Angle of Impact: " << data.angleOfImpact << endl;
	fileOut << "Tank Armour Thickness: " << data.tankArmourThickness << endl;
	fileOut << "Tank LOS Armour Thickness: " << data.LOSTankArmourThickness << endl;
	fileOut << "Projectile Magnitude: " << data.projectileMagnitude << endl;
	fileOut << "Direction Magnitude: " << data.directionMagnitude << endl;
	fileOut << "K CONSTANT: " << data.N_CONST << endl;
	fileOut << "n CONSTANT: " << data.K_CONST << endl;
	fileOut << "Penetration: " << data.penetration << endl;
	fileOut << "Damage: " << data.damageToTank << endl;
	fileOut << endl;

	fileOut.close();
}

string ProcessOutput(const TankProjectileData &data)
{
	string output = "";

	for (int i = 0; i < data.projectileVelocityReflected3D.size(); i++)
	{
		output = output + to_string(data.projectileVelocityReflected3D[i]) + ",";
	}

	output = output + to_string(data.damageToTank);

	output = output + "," + to_string(data.penetration);

	return output;
}

void Debugger(const TankProjectileData &tankData, const Data &data)
{
	PrintDataStruct(data);
	PrintTankProjectileDataStruct(tankData);
}

void Worker()
{
	while (worker_working = id > cur_id) // next ticket exists?
	{
		mtx.lock();
		Data ticket = tickets[++cur_id]; // copy ticket
		mtx.unlock();

		ParseInputString(ticket);		

		TankProjectileData tankProjectileData;

		DataConversion(ticket, tankProjectileData);
		DeMarreEquation(tankProjectileData);		

		HandleDamage(tankProjectileData);		

		thread debugger(Debugger,tankProjectileData, ticket);
		debugger.detach();

		string output = ProcessOutput(tankProjectileData); // process input
		ticket.result = output; // prepare result
		ticket.ready = true; // notify about result

		mtx.lock();
		tickets[cur_id] = ticket; // copy back the result
		mtx.unlock();
	}
}

void __stdcall RVExtension(char *output, int outputSize, const char *function)
{
	if (!strncmp(function, "r:", 2)) // detect checking for result
	{
		long int num = atol(&function[2]); // ticket number or 0

		if (tickets.find(num) != tickets.end()) // ticket exists
		{
			mtx.lock();
			if (tickets[num].ready) // result is ready
			{
				strncpy_s(output, outputSize, tickets[num].result.c_str(), _TRUNCATE); // result
				tickets.erase(num); // get rid of the read ticket
				mtx.unlock();
				return;
			}
			mtx.unlock();

			strncpy_s(output, outputSize, "WAIT", _TRUNCATE); // result is not ready
			return;
		}
		strncpy_s(output, outputSize, "EMPTY", _TRUNCATE); // no such ticket
	}
	else if (!strncmp(function, "s:", 2)) // detect ticket submission
	{
		Data data;
		data.params = string(&function[2]); // extract params		

		mtx.lock();
		tickets.insert(pair<long int, Data>(++id, data)); // add ticket to the queue
		mtx.unlock();

		if (!worker_working) // if worker thread is finished, start another one
		{
			worker_working = true;
			thread worker(Worker);
			worker.detach(); // start parallel process
		}
		strncpy_s(output, outputSize, to_string(id).c_str(), _TRUNCATE); // ticket number
	}
	else
	{
		strncpy_s(output, outputSize, "INVALID COMMAND", _TRUNCATE); // other input
	}
}