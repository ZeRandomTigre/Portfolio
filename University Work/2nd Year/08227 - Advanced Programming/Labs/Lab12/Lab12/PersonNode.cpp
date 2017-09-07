#include "PersonNode.h"
using namespace std;

PersonNode::PersonNode(string name, int age) : m_name(name), m_age(age), m_next(0)
{
}

PersonNode::~PersonNode(void)
{
}

string PersonNode::GetName()
{
	return m_name;
}

void PersonNode::SetName(string name)
{
	m_name = name;
}

int PersonNode::GetAge()
{
	return m_age;
}

void PersonNode::SetAge(int age)
{
	m_age = age;
}