
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


int main(int argc, char **argv) {
	if (argc !=3) {
		cerr << "Usage: " << argv[0] << " <input filename> <output filename>" << endl;
		int keypress; cin >> keypress;
		return -1;
	}

	if (Copy(argv[1], argv[2]))
		cout << "Copy completed" << endl;
	else
		cout << "Copy failed!" << endl;

	system("pause");

	return 0;
}


bool Copy(char filenamein[], char filenameout[])
{
	ifstream fin(filenamein);
	if(fin.is_open())
	{
		ofstream fout(filenameout);
	
		char c;
		while(fin.get(c))
		{
			fout.put(c);
		}

		fout.close();
		fin.close();

		return true;
	}
	return false;
}
