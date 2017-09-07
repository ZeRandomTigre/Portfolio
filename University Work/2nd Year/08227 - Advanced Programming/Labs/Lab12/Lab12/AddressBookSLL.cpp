#include "AddressBookSLL.h"

AddressBookSLL::AddressBookSLL(void) : m_head(0)
{
}

AddressBookSLL::~AddressBookSLL(void)
{
}

void AddressBookSLL::AddPerson(const string &name, int age)
{
	if (m_head == 0)
	{
		m_head = new PersonNode(name, age);
	}
	else if ((m_head != 0) && (m_head->m_next == 0))
	{
		m_head->m_next = new PersonNode(name, age);
	}
	else
	{
		PersonNode *p = 0;
		p = m_head->m_next;

		while (p->m_next != 0)
		{
			p = p->m_next;
		}

		p->m_next = new PersonNode(name, age);
	}
}

bool AddressBookSLL::FindPerson(const string &name)
{
	if (m_head == 0)
	{
		return false;
	}

	if (m_head->m_name == name)
	{
		return true;
	}


	PersonNode *p = 0;
	p = m_head;

	while (p->m_next != 0)
	{
		if (p->m_next->m_name == name)
		{
			return true;
		}

		p = p->m_next;
	}

	return false;
}

bool AddressBookSLL::DeletePerson(const string &name)
{
	if (m_head == 0)
	{
		return false;
	}

	PersonNode *del = m_head;

	if (m_head->m_name == name)
	{
		return true;
		m_head = del->m_next;
		delete del;
	}


	PersonNode *p = 0;
	p = m_head;

	while (p->m_next != 0)
	{
		if (p->m_next->m_name == name)
		{
			return true;
		}

		p = p->m_next;
	}

	return false;
}
