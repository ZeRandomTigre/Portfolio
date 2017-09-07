#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
using namespace std;

#include "FileUtilities.h"

/*
* Partially completed program
* The program should copy a text file.
*
*/

bool Copy(char filenamein[], char filenameout[]);
bool Check(char filenamein[], char filenameout[]);
string line = "colour (1,2,3)";
string text = "";
int stuff;
FileUtilities fileutilities;

int main(int argc, char **argv) {


	if (argc !=3) {
		cerr << "Usage: " << argv[0] << " <input filename> <output filename>" << endl;
		int keypress; cin >> keypress;
		return -1;
	}

	//fileutilities.textFileCopy(argv[1], argv[2]);
	
	if ( fileutilities.textFileCopy(argv[1], argv[2]) )
		cout << "Copy completed" << endl;
	else
		cout << "Copy failed!" << endl;

	/****************************************
	if ( fileutilities.textFileCheck(argv[1], argv[2]) )
		cout << "Check completed" << endl;
	else
		cout << "Check failed!" << endl;
	****************************************/

	system("PAUSE");
}