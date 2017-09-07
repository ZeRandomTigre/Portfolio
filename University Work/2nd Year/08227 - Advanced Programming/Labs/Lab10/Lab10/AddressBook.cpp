#include "AddressBook.h"
#include "Person.h"

using namespace std;

AddressBook::AddressBook()
{
}


AddressBook::~AddressBook()
{
}

void AddressBook::addPerson(string name, int age)
{
	people[m_currentNumberOfContacts].SetName(name);
	people[m_currentNumberOfContacts].SetAge(age);

	++m_currentNumberOfContacts;
}

void AddressBook::OutputStreamDatabase(ostream& stream)
{
	for (int i = 0; i < 10; ++i)
	{
		stream << people[i].GetName() << ' ' << people[i].GetAge() << endl;
	}
}

void AddressBook::InputStreamPerson(istream& stream)
{
	string name;
	int age;

	stream >> name >> age;

	people[m_currentNumberOfContacts].SetName(name);
	people[m_currentNumberOfContacts].SetAge(age);

	++m_currentNumberOfContacts;
}
