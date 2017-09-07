#include "Fraction.h"


Fraction::Fraction(int numerator, int denominator)
{
	m_numerator = numerator;
	m_denominator = denominator;
}


Fraction::~Fraction()
{
}

const int Fraction::getNumerator()
{
	return m_numerator;
}

const int Fraction::getDenominator()
{
	return m_denominator;
}

Fraction Fraction::operator+(const Fraction &rhs) const
{
	Fraction temp(0, 0);
	Fraction lhs(*this);

	temp.m_numerator = (lhs.m_numerator * rhs.m_denominator) + (rhs.m_numerator * lhs.m_denominator);
	temp.m_denominator = lhs.m_denominator * rhs.m_denominator;;
	return temp;
}

ostream& operator<<(ostream& os, Fraction& frac)
{
	os << frac.getNumerator() << '/' << frac.getDenominator();
	return os;
}

void Fraction::OutputStream(ostream& stream)
{
	Fraction temp(*this);
	stream << temp.getNumerator() << "/" << temp.getDenominator() << endl;
}
