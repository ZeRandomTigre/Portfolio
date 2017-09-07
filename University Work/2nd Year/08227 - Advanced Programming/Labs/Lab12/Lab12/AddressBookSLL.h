#pragma once

#include "PersonNode.h"

class AddressBookSLL
{
public:
	AddressBookSLL(void);
	~AddressBookSLL(void);

	void AddPerson(const string &name, int age);
	bool FindPerson(const string &name);
	bool DeletePerson(const string &name);

private:
	PersonNode *m_head;
};
