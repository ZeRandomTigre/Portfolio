
#include <iostream>
using namespace std;
#include "AddressBookSLL.h"

/*
	This program will act as a Contacts Address Book using a Singly Linked Lists
*/

int main(int argc, char **argv) {

	AddressBookSLL book;

	book.AddPerson("Bob", 12);
	book.AddPerson("Jim", 15);
	book.AddPerson("Sean", 20);
	book.AddPerson("Alice", 20);

	if (book.FindPerson("fsdf"))
		cout << "Found" << endl;
	else
		cout << "Not Found" << endl;

	book.DeletePerson("Sean");
	system("PAUSE");
}



