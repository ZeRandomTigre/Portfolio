#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <Windows.h>

#include "PuzzleNode.h"

class WordSearch
{
public:
	WordSearch();
	~WordSearch();

	bool ReadSimplePuzzle();
	bool ReadSimpleDictionary();
	bool ReadAdvancedPuzzle();
	bool ReadAdvancedDictionary();

	bool SolveSimplePuzzleWithSimpleDictionary();
	bool SolveSimplePuzzleWithAdvancedDictionary();
	bool SolveAdvancedPuzzleWithSimpleDictionary();
	bool SolveAdvancedPuzzleWithAdvancedDictionary();

	void WriteResults(std::string fileName);
	void UpdateResults(PuzzleNode *node, std::string *testWord);

private:
	LARGE_INTEGER start, end, frequency;
	const int NUMBER_OF_RUNS;
	const std::string PUZZLE_NAME;
	const std::string DICTIONARY_NAME;

	char simplePuzzleArray[100][100];
	int simplePuzzleArrayLength;
	std::string simpleDictionaryArray[100];
	int simpleDictionaryArrayLength;

	std::vector<std::string> advancedPuzzleGrid;
	PuzzleNode *advancedPuzzleArray[100][100];
		
	int NUMBER_OF_WORDS_MATCHED;
	std::string WORDS_MATCH_IN_GRID[100];
	int NUMBER_OF_GRID_CELLS_VISTED;
	int NUMBER_OF_DICTIONARY_ENTRIES_VISITED;
	float TIME_TO_POPULATE_GIRD_STRUCTURE;
	float TIME_TO_SOLVE_PUZZLE;

	enum Direction { right, left, down, up, rightDown, rightUp, leftDown, leftUp };

	void WordSearch::SearchAdvancedPuzzleSimpleDictionary(PuzzleNode *node, std::string *testWordPtr, const Direction &direction);
};

