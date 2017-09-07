#pragma once
#include <string>
#include <ostream>

using namespace std;

class Person
{
public:
	Person();
	~Person();

	// Getter and setter functions
	void SetName(string name);
	string GetName();

	void SetAge(int age);
	int GetAge();

	// stream
	void OutputPerson(ostream& stream);
	void InputPerson(istream& stream);

private:
	string m_name;
	int m_age;
};

