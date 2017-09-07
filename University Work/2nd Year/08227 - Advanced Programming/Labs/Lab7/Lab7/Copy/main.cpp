
#include <iostream>
#include <fstream>
#include <string>
using namespace std;

/*
* Partially completed program
* The program should copy a text file.
*
*/

bool Copy(char filenamein[], char filenameout[]);
bool Check(char filenamein[], char filenameout[]);



int main(int argc, char **argv) {
	if (argc !=3) {
		cerr << "Usage: " << argv[0] << " <input filename> <output filename>" << endl;
		int keypress; cin >> keypress;
		return -1;
	}

	//Copy(argv[1], argv[2]);

	Check(argv[1], argv[2]);

	system("PAUSE");
}


bool Copy(char filenamein[], char filenameout[])
{
	ifstream fileIn(filenamein);
	if (fileIn.is_open())
	{
		ofstream fileOut(filenameout);

		char character;
		while (fileIn.get(character))
		{
			fileOut.put(character);
		}

		fileIn.close();
		fileOut.close();

		return true;
	}
}

bool Check(char filenamein[], char filenameout[])
{
	ifstream fileIn(filenamein);
	ifstream fileOut(filenameout);

	char inputCharacter;
	char outptCharacter;

	while (fileIn.get(inputCharacter))
	{
		fileOut.get(outptCharacter);
		if (inputCharacter != outptCharacter)
		{
			cout << "Error: Input & Ouput files are not the same" << endl;
			return false;
		}
	}

	return true;
}
