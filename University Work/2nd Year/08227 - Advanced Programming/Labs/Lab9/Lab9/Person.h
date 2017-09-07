#pragma once
#include <string>
#include <ostream>

class Person
{
public:
	Person(std::string name, int age);
	~Person();

	// Getter and setter functions
	void SetName(std::string name);
	std::string GetName();

	void SetAge(int age);
	int GetAge();

	// stream
	void OutputPerson(std::ostream& stream);
	void InputPerson(std::istream& stream);

private:
	std::string m_name;
	int m_age;
};

