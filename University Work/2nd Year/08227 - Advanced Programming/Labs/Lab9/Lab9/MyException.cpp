#include "MyException.h"
#include <sstream>
using namespace std;

MyException::MyException() : m_exceptionMessage("My Exception"), m_line(0) {}

MyException::MyException(const string &message, int line, const string function, const string file) : m_line(line) {
	ostringstream outMessage;
	outMessage << message << ", at Line " << m_line << endl;
	outMessage << "The function: " << "'" << function << "'" << " has failed" << endl;
	outMessage << "At file location: " << "'" << file << "'" << endl;

	m_exceptionMessage = outMessage.str();
}

const char* MyException::what() const throw() {
	return m_exceptionMessage.c_str();
}

int MyException::line() const { return m_line; }
