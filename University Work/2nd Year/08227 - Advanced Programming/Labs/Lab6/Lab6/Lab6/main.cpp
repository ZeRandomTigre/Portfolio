#include <iostream>
#include <string>
using namespace std;

void functionA(int i, int j) 
{
	if (i / 4 == 4)
	{
		i = 100;
	}
}

void functionB(int i, int j) 
{
	if (i * j == 8)
	{
		i = 50;
	}
	else
	{
		j = 60;
	}
}

void functionC(int i, int j)
{
	if (i < j)
	{
		j = j * 2;
	}
	else if (i % 2)
	{
		i = i * 2;
	}
	else
	{
		i++;
		j++;
	}
}

void functionD(int i, int j)
{
	if (i == 0 && j == 0)
	{
		i = 1;
		j = 2;
	}
	else if (i == 0 && j != 0)
	{
		i = 5;
		j = 10;
	}
	else if (j == 0 && i != 0)
	{
		i = 10;
		j = 5;
	}
	else
	{
		i = 4;
		j = 4;
	}
}



void main(int argc, char** argv) 
{
	bool P = true; 
	bool Q = false; 
	bool R = false; 
	string s = "a"; 
	string t = "b"; 
	int i = 10; 
	int j = 0;

	if (s != t)
	{
		cout << "True" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (s < t)
	{
		cout << "True" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	cout << i * j << endl;

	if (i > j)
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (i < j + 2)
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (!P == Q)
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (R == (P&&Q))
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (Q && (P || R))
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (Q || (P && !R))
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}

	if (P && !Q && !R || (P == !Q))
	{
		cout << "true" << endl;
	}
	else
	{
		cout << "false" << endl;
	}


	system("PAUSE"); // Waits for a keypress
}