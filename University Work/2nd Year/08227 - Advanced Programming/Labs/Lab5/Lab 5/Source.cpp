#include <iostream>
using namespace std;

//void swap(int &lhs, int &rhs)
//void swap(int lhs, int rhs)
void swap(int *lhs, int *rhs)
{
	int *temp = lhs;
	lhs = rhs;
	rhs = temp;
}

int& clamp(int& value, int low, int high)
{
	if (value < low)
		return low;
	if (value > high)
		return high;
	return value;
}

void main(int, char**) {

	int a = 10;
	int b = 20;

	cout << "a=" << a << ", b=" << b << endl;

	//swap(a, b);
	clamp(a, a, b);
	
	cout << "a=" << a << ", b=" << b << endl;

	system("PAUSE");
}

