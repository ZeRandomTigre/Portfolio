#include <iostream>
#include <fstream>
#include <string>
#include <algorithm>
using namespace std;

#include "FileUtilities.h"

struct objects_t
{
	string object;
	int number;
} objects[100];

FileUtilities::FileUtilities()
{
}


FileUtilities::~FileUtilities()
{
}

bool FileUtilities::textFileCopy(char filenamein[], char filenameout[])
{
	ifstream fileIn(filenamein);
	if (fileIn.is_open())
	{
		ofstream fileOut(filenameout);

		/*
		char c;
		while (fileIn.get(c))
		{
			fileOut.put(c);
		}
		*/

		int count = 0;
		string newLine;
		size_t whiteCharacterPos;
		size_t commaCharacterPos;
		string numbers;
		char delimiters[] = "()";
		int totalNumber = 0;
		int runningNumber = 0;
		string objectNumbers[100];

		while (fileIn.good())
		{
			getline(fileIn, newLine);
			if (newLine[0] == '{')
			{
			}
			else if (newLine[0] == '}')
			{
			}
			else
			{
				whiteCharacterPos = newLine.find(" ");
				objects[count].object = newLine.substr(0, whiteCharacterPos);
				numbers = newLine.substr(whiteCharacterPos + 1);

				for (unsigned int i = 0; i < strlen(delimiters); i++)
				{
					numbers.erase(remove(numbers.begin(), numbers.end(), delimiters[i]), numbers.end());
				}				

				int numbersCount = 0;

				while ( (commaCharacterPos = numbers.find(",")) != string::npos)
				{
					objectNumbers[numbersCount] = numbers.substr(0, commaCharacterPos);
					numbers.erase(0, commaCharacterPos + 1);
					numbersCount++;
				}

				objectNumbers[numbersCount] = numbers;

				
				for (unsigned int i = 0; i < numbers.length(); i++)
				{
					runningNumber = stoi(objectNumbers[i]);
					totalNumber = totalNumber + runningNumber;
				}
				
				objects[count].number = totalNumber;
				
				

				count++;
			}
				
		}

		
		for (int i = 0; i < 100; ++i) {
			fileOut << objects[i].object << ' ' << objects[i].number << ' ';
		}
		fileOut.close();

		fileOut.close();
		fileIn.close();

		return true;
	}
	else
		return false;
}

bool FileUtilities::textFileCheck(char filenamein[], char filenameout[])
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

bool FileUtilities::ObjectParser(char filenamein[], char filenameout[])
{
	return true;
}
