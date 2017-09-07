#pragma once
#include <exception>
#include <string>
using namespace std;

class MyException : public exception
{
public:
	MyException();
	MyException(const string &message, int line, const string function, const string file);

	virtual const char* what() const throw();
	int line() const;

private:
	string m_exceptionMessage;
	const int m_line;

	const string m_function;
	const string m_file;
};

