#include <iostream>

using namespace std;

#include "Fraction.h"

template <class T>
class Calculator
{
public:
	Calculator(const T &x, const T &y) : m_x(x), m_y(y) {}
	~Calculator(void) { }
	T Add(void) { return m_x + m_y; }
	T Sub(void) { return m_x - m_y; }
	T Mult(void){ return m_x * m_y; }
	T Div(void) { return m_x / m_y; }
private:
	T m_x;
	T m_y;
};

int main(int argc, char **argv) {
	/*******************************************

	// Create  a calculator for integers
	Calculator<int> calc1(5, 2);
	// should give 10
	int z1 = calc1.Mult();

	cout << z1 << endl;

	// Create a calculator for doubles
	Calculator<double> calc2(5.0, 2.5);
	// should give 12.5
	double z2 = calc2.Mult();

	cout << z2 << endl;

	//Create a calculator for floats
	Calculator<float> calc3(1.2, 1.5);
	// should give 2.7
	float z3 = calc3.Add();

	cout << z3 << endl;

	*******************************************/

	Fraction frac1(1, 2); // 1/2
	Fraction frac2(1, 3); // 1/3

	Calculator<Fraction> fractionCalculator(frac1, frac2);
	// should give 5/6
	Fraction frac3 = fractionCalculator.Add();

	frac3.OutputStream(cout);
	cout << frac3 << endl;

	system("PAUSE");
}



