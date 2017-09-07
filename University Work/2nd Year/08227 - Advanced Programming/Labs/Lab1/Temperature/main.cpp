#include <iostream>
using namespace std;

int main(int argc, char **argv) {
	cout << "This program will convert a temperature in fahrenheit to celsius" << endl;

	float fahrenheit;
	float celsius;

	cout << "Enter temperature in fahrenheit: ";
	cin >> fahrenheit;

	celsius = 5.0 / 9.0 * (fahrenheit - 32.0);

	cout << "temeprature in celsius: " << celsius << endl;

	system("PAUSE");
	return 0;
}