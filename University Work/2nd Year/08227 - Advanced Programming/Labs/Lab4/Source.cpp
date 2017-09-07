#include <iostream>
using namespace std;

void exercise1() {
	int a = 10;
	int b = 20;
	int *p = &a;

	cout << "a= " << a << endl;
	cout << "b= " << b << endl;

	// Add your code here
	*p = 100;

	cout << "a= " << a << endl;
	cout << "b= " << b << endl;
}

void exercise2() {
	int a = 10;
	int b = 20;
	int c = 30;
	int *p = &b;

	cout << "a= " << a << endl;
	cout << "b= " << b << endl;
	cout << "c= " << c << endl;

	*p = 100;
	//p++;
	//*p = 200;

	cout << "a= " << a << endl;
	cout << "b= " << b << endl;
	cout << "c= " << c << endl;
}

void exercise3() {
	unsigned int a = 0x00ff00ff;
	unsigned int *p = (unsigned int *)a;

	// does not work because Windows operating system does not allow code to access memory outside of its footprint

	*p = 999;
}

void exercise4() {
	double x = 3.14;

	cout << "x= " << x << endl;

	// Add code here
	double *q, **p; // doubley

	q = &x;
	p = &q;

	**p = 50.0;

	cout << "x= " << x << endl;
}

void pointerChainTest() 
{
	//void *s;
	//s = 100;
	//void *p = &s, *q = p, *r = q;	
}

void main(int, char**) {

	//exercise1();
	//exercise2();
	//exercise3();
	exercise4();
	//pointerChainTest();

	system("PAUSE");
}
