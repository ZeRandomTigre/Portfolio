#pragma once
#include <iostream>

using namespace std;

class Fraction
{
public:
	Fraction(int numerator, int denominator);
	~Fraction();
	const int getNumerator();
	const int getDenominator();
	void OutputStream(ostream& stream);

	Fraction operator+ (const Fraction &rhs) const;

	friend ostream& operator<<(ostream& os, Fraction& frac);
	
private:
	int m_numerator, m_denominator;
};

