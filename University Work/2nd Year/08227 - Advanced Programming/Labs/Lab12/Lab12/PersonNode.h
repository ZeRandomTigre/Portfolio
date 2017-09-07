#pragma once

#include <string>
using namespace std;

class PersonNode
{
public:
	PersonNode(string name, int age);
	~PersonNode(void);

	string GetName();
	void SetName(string name);

	int GetAge();
	void SetAge(int age);

private:
	string m_name;
	int m_age;
	PersonNode *m_next;

	friend class AddressBookSLL;
};
