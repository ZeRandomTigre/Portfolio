#pragma once
class FileUtilities
{
public:
	FileUtilities();
	~FileUtilities();
	bool textFileCopy(char filenamein[], char filenameout[]);
	bool textFileCheck(char filenamein[], char filenameout[]);
	bool ObjectParser(char filenamein[], char filenameout[]);
};

