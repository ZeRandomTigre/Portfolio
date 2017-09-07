#include <iostream>

#include "Person.h"
#include "AddressBook.h"
#include "MyException.h"

using namespace std;

/*
	This program will act as a Contacts Address Book
*/

int main(int argc, char **argv) {
	
	AddressBook addressbook;

	addressbook.addPerson("Sean", 20);

	addressbook.InputStreamPerson(cin);

	addressbook.OutputStreamDatabase(cout);



	/*Person person("Sean Phillips", 20);

	person.OutputPerson(cout);

	Person person2("Alice Rawling", 20);
	try
	{
		person2.InputPerson(cin);
	}
	catch (exception& e)
	{
		cout << e.what() << endl;
	}
	person2.OutputPerson(cout);*/

	system("PAUSE");
}



