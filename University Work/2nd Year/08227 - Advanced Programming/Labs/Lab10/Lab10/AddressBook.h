#pragma once
#include "Person.h"

using namespace std;

class AddressBook
{
public:
	AddressBook();
	~AddressBook();

	void addPerson(string name, int age);

	void OutputStreamDatabase(ostream& stream);
	void InputStreamPerson(istream& stream);

private:
	int m_currentNumberOfContacts = 0;

	Person people[10];
};

