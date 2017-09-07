#include "Person.h"
#include "MyException.h"
using namespace std;

// constructor
Person::Person()
{
	// intialsie data members
	SetName("");
	SetAge(0);
}

// deconstructor
Person::~Person()
{
}


// getters and setters
void Person::SetName(string name)
{
	m_name = name;
}

string Person::GetName()
{
	return m_name;
}

void Person::SetAge(int age)
{
	if (age < 0 || age > 150)
		throw MyException("Invalid age in Person class", __LINE__, __FUNCTION__, __FILE__ );
	else
		m_age = age;
}

int Person::GetAge()
{
	return m_age;
}

void Person::OutputPerson(ostream& stream)
{
	stream << GetName() << ' ' << GetAge() << endl;
}

void Person::InputPerson(istream& stream)
{
	string name;
	int age;

	stream >> name >> age;

	SetName(name);
	SetAge(age);
}
