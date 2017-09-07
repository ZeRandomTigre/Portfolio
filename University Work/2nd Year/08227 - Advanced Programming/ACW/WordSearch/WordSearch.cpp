#include "WordSearch.h"
#include <algorithm>
#include <iterator>
#include <iostream>
#include <iomanip>
#include <cmath>
#include <sstream>

#include "PuzzleNode.h"

WordSearch::WordSearch() : NUMBER_OF_RUNS(500000), PUZZLE_NAME("wordsearch_grid.txt"), DICTIONARY_NAME("dictionary.txt"), simplePuzzleArrayLength(0), 
												simpleDictionaryArrayLength(0), NUMBER_OF_WORDS_MATCHED(0), NUMBER_OF_GRID_CELLS_VISTED(0), 
												NUMBER_OF_DICTIONARY_ENTRIES_VISITED(0), TIME_TO_POPULATE_GIRD_STRUCTURE(0), TIME_TO_SOLVE_PUZZLE(0)
{
}

WordSearch::~WordSearch()
{
}

bool WordSearch::ReadAdvancedPuzzle()
{
	std::ifstream fileIn("wordsearch_grid.txt");

	// copy puzzle to string vector
	if (!fileIn.is_open())
	{
		return false;		
	}
	else
	{
		std::string line;

		while (std::getline(fileIn, line))
		{
			// remove white spaces from line & modify line length
			line.erase(remove(line.begin(), line.end(), ' '), line.end());

			advancedPuzzleGrid.push_back(line);
		}

		fileIn.close();
	}


	std::string vectorLine;
	PuzzleNode *node;

	for (unsigned int Y = 0; Y < advancedPuzzleGrid.size(); ++Y)
	{
		vectorLine = advancedPuzzleGrid[Y];

		for (unsigned int X = 0; X < vectorLine.length(); ++X)
		{
			node = new PuzzleNode(vectorLine[X], X, Y);
			advancedPuzzleArray[Y][X] = node;
		}
	}

	for (unsigned int Y = 0; Y < advancedPuzzleGrid.size(); ++Y)
	{
		vectorLine = advancedPuzzleGrid[Y];

		for (unsigned int X = 0; X < vectorLine.length(); ++X)
		{
			node = advancedPuzzleArray[Y][X];

			// right pointer
			if (advancedPuzzleArray[Y][X + 1] != NULL)
			{
				node->m_right = advancedPuzzleArray[Y][X + 1];
			}
			//left pointer
			if (advancedPuzzleArray[Y][X - 1] != NULL)
			{
				node->m_left = advancedPuzzleArray[Y][X - 1];
			}
			// up pointer
			if (advancedPuzzleArray[Y - 1][X] != NULL)
			{
				node->m_up = advancedPuzzleArray[Y - 1][X];
			}
			// down pointer
			if (advancedPuzzleArray[Y + 1][X] != NULL)
			{
				node->m_down = advancedPuzzleArray[Y + 1][X];
			}
			// left up pointer
			if (advancedPuzzleArray[Y - 1][X - 1] != NULL)
			{
				node->m_leftUp = advancedPuzzleArray[Y - 1][X - 1];
			}
			// left down pointer
			if (advancedPuzzleArray[Y + 1][X - 1] != NULL)
			{
				node->m_leftDown = advancedPuzzleArray[Y + 1][X - 1];
			}
			// right up pointer
			if (advancedPuzzleArray[Y - 1][X + 1] != NULL)
			{
				node->m_rightUp = advancedPuzzleArray[Y - 1][X + 1];
			}
			// right down pointer
			if (advancedPuzzleArray[Y + 1][X + 1] != NULL)
			{
				node->m_rightDown = advancedPuzzleArray[Y + 1][X + 1];
			}
		}
	}

	std::cout << std::endl << "ReadAdvancedPuzzle() has been read correctly" << std::endl;
	return true;
}

bool WordSearch::ReadSimplePuzzle()
{
	std::ifstream fileIn("wordsearch_grid.txt");

	// copy puzzle to 2d array
	if (fileIn.is_open())
	{
		int row = 0;
		std::string line;

		while (std::getline(fileIn, line))
		{
			// remove white spaces from line & modify line length
			line.erase(remove(line.begin(), line.end(), ' '), line.end());

			// add line chars to array
			for (unsigned int i = 0; i < line.length(); i++)
			{
				simplePuzzleArray[row][i] = line[i];
				++simplePuzzleArrayLength;				
			}
			++row;
		}

		fileIn.close();
	}

	else
	{
		std::cout << "File Does Not exist" << std::endl;
		return false;
	}

	// check if file copied correctly
	char inputCharacter;
	int row = 0;
	int column = 0;
	std::ifstream checkFileIn("wordsearch_grid.txt");
	if (checkFileIn.is_open())
	{
		while (checkFileIn.get(inputCharacter))
		{
			if (inputCharacter == '\n')
			{
				++row;
				column = 0;
			}
			else if (inputCharacter != ' ')
			{
				if (simplePuzzleArray[row][column] != inputCharacter)
				{
					std::cout << "puzzle has NOT been read correctly" << std::endl;
					return false;
				}
				++column;
			}
		}

		checkFileIn.close();
	}

	std::cout << "puzzle has been read correctly" << std::endl;
	return true;
}

bool WordSearch::ReadAdvancedDictionary()
{
	std::cout << std::endl << "ReadAdvancedDictionary() has NOT been implemented" << std::endl;
	return true;
}

bool WordSearch::ReadSimpleDictionary()
{
	// copy dictionary to array
	std::ifstream fileIn("dictionary.txt");
	if (fileIn.is_open())
	{
		std::string line;
		int count = 0;

		while (std::getline(fileIn, line))
		{
			simpleDictionaryArray[count] = line;
			++simpleDictionaryArrayLength;
			++count;
		}
	}

	// check if copied correctly
	std::ifstream checkFileIn("dictionary.txt");
	if (checkFileIn.is_open())
	{
		std::string line;
		int count = 0;
		while (std::getline(checkFileIn, line))
		{
			if (simpleDictionaryArray[count] != line)
			{
				std::cout << "simple dictionary has NOT been read correctly" << std::endl;
				return false;
			}

			++count;
		}

		std::cout << "simple dictionary has been read correctly" << std::endl;

		checkFileIn.close();

		return true;
	}

	return true;
}

bool WordSearch::SolveSimplePuzzleWithSimpleDictionary() {
	double timeTakenInSeconds;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);

	for (int n = 0; n < NUMBER_OF_RUNS; ++n) {
		// Add your solving code here!

		int gridSize = sqrt(simplePuzzleArrayLength);

		NUMBER_OF_WORDS_MATCHED = 0;
		std::stringstream wordsMatchedInGrid;
		std::string *wordPtr;

		for (int row = 0; row < gridSize; ++row)
		{
			for (int column = 0; column < gridSize; ++column)
			{				
				int gridY = row;
				int gridX = column;
				
				bool wordFound = false;

				// loop through dictionary
				for (int i = 0; i < simpleDictionaryArrayLength; ++i)
				{
					wordPtr = &simpleDictionaryArray[i];

					if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[0])
					{
						// check along right direction
						for (unsigned int right = 1; right < wordPtr->length(); ++right)
						{
							++gridX;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[right])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID+NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();							
						}

						// check along left direction
						for (unsigned int left = 1; left < wordPtr->length(); ++left)
						{
							--gridX;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[left])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along down direction
						for (unsigned int down = 1; down < wordPtr->length(); ++down)
						{
							++gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[down])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along up direction
						for (unsigned int up = 1; up < wordPtr->length(); ++up)
						{
							--gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[up])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along diagonally right down
						for (unsigned int up = 1; up < wordPtr->length(); ++up)
						{
							++gridX;
							++gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[up])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along diagonally right up
						for (unsigned int up = 1; up < wordPtr->length(); ++up)
						{
							++gridX;
							--gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[up])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along diagonally left down
						for (unsigned int up = 1; up < wordPtr->length(); ++up)
						{
							--gridX;
							++gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[up])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}

						// check along diagonally left up
						for (unsigned int up = 1; up < wordPtr->length(); ++up)
						{
							--gridX;
							--gridY;
							if (simplePuzzleArray[gridY][gridX] == (*wordPtr)[up])
							{
								wordFound = true;
							}
							else
							{
								wordFound = false;
								break;
							}
						}

						gridY = row;
						gridX = column;

						if (wordFound == true)
						{
							++NUMBER_OF_WORDS_MATCHED;
							wordsMatchedInGrid.clear();
							wordsMatchedInGrid.str(std::string());
							wordsMatchedInGrid << gridX - 1 << " " << gridY - 1 << " " << (*wordPtr);
							*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = wordsMatchedInGrid.str();
						}
					}					
				}
			}
		}				
	}

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);


	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	std::cout << std::fixed << std::setprecision(10) << "SolveSimplePuzzleWithSimpleDictionary() - " << timeTakenInSeconds << " seconds" << std::endl;
	return true;
}

bool WordSearch::SolveSimplePuzzleWithAdvancedDictionary() {
	std::cout << std::endl << "SolveSimplePuzzleWithAdvancedDictionary() has NOT been implemented" << std::endl;
	return false;
}

bool WordSearch::SolveAdvancedPuzzleWithSimpleDictionary() {
	double timeTakenInSeconds;
	QueryPerformanceFrequency(&frequency);
	QueryPerformanceCounter(&start);


	for (int n = 0; n < NUMBER_OF_RUNS; ++n)
	{
		NUMBER_OF_WORDS_MATCHED = 0;
		Direction direction;

		PuzzleNode *currentNode;
		PuzzleNode *checkNode;
		std::string *testWord;

		for (unsigned int i = 0; i < advancedPuzzleGrid.size(); ++i)
		{			
			currentNode = advancedPuzzleArray[i][0];

			while (currentNode != NULL)
			{
				for (int j = 0; j < simpleDictionaryArrayLength; ++j)
				{
					testWord = &simpleDictionaryArray[j];
					checkNode = currentNode;

					if ((*checkNode->GetLetter()) == (*testWord)[0])
					{
						direction = right;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = left;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = down;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = up;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = rightDown;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = rightUp;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = leftDown;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);

						direction = leftUp;
						SearchAdvancedPuzzleSimpleDictionary(checkNode, testWord, direction);
					}
				}

				currentNode = currentNode->m_right;
			}
		}
	}

	QueryPerformanceCounter(&end);
	timeTakenInSeconds = (end.QuadPart - start.QuadPart) / (double)(frequency.QuadPart*NUMBER_OF_RUNS);


	////////////////////////////////////////////////
	// This output should be to your results file //
	////////////////////////////////////////////////
	std::cout << std::fixed << std::setprecision(10) << "SolveAdvancedPuzzleWithSimpleDictionary() - " << timeTakenInSeconds << " seconds" << std::endl;
	return true;
}

bool WordSearch::SolveAdvancedPuzzleWithAdvancedDictionary() {
	std::cout << std::endl << "SolveAdvancedPuzzleWithAdvancedDictionary() has NOT been implemented" << std::endl;
	return false;
}

void WordSearch::WriteResults(std::string fileName)
{
	std::ofstream fileOut(fileName);

	if (fileOut.is_open())
	{
		fileOut << "NUMBER_OF_WORDS_MATCHED " << NUMBER_OF_WORDS_MATCHED << std::endl;
		fileOut << std::endl;
		fileOut << "WORDS_MATCHED_IN_GRID " << std::endl;
		for (int i = 0; i < 100; i = i + 3)
		{
			if (!WORDS_MATCH_IN_GRID[i].empty())
			{
				fileOut << WORDS_MATCH_IN_GRID[i] << WORDS_MATCH_IN_GRID[i+1] << WORDS_MATCH_IN_GRID[i+2];
				fileOut << std::endl;
			}
		}
		fileOut << std::endl;
		fileOut << "NUMBER_OF_GRID_CELLS_VISTED " << NUMBER_OF_GRID_CELLS_VISTED << std::endl;
		fileOut << std::endl;
		fileOut << "NUMBER_OF_DICTIONARY_ENTRIES_VISITED " << NUMBER_OF_DICTIONARY_ENTRIES_VISITED << std::endl;
		fileOut << std::endl;
		fileOut << "TIME_TO_POPULATE_GRID_STRUCTURE " << TIME_TO_POPULATE_GIRD_STRUCTURE << std::endl;
		fileOut << std::endl;
		fileOut << "TIME_TO_SOLVE_PUZZLE " << TIME_TO_SOLVE_PUZZLE << std::endl;

		fileOut.close();
	}
	else
	{
		std::cout << "Write Results Error" << std::endl;
	}
}

void WordSearch::SearchAdvancedPuzzleSimpleDictionary(PuzzleNode *node, std::string *testWordPtr, const Direction &direction)
{
	PuzzleNode *ptr = node;
	bool wordFound = false;

	for (int i = 1; (unsigned int)i < testWordPtr->length(); ++i)
	{
		if (ptr == NULL)
		{
			wordFound = false;
			break;
		}

		if (direction == right)
			ptr = node->m_right;
		else if (direction == left)
			ptr = node->m_left;
		else if (direction == up)
			ptr = node->m_up;
		else if (direction == down)
			ptr = node->m_down;
		else if (direction == rightDown)
			ptr = node->m_rightDown;
		else if (direction == rightUp)
			ptr = node->m_rightUp;
		else if (direction == leftDown)
			ptr = node->m_leftDown;
		else if (direction == leftUp)
			ptr = node->m_leftUp;
		
		if ((*node->GetLetter()) == (*testWordPtr)[i - 1])
		{
			wordFound = true;
			node = ptr;
		}
		else
		{
			wordFound = false;
			break;
		}
	}

	if (wordFound == true)
		UpdateResults(node, testWordPtr);
}

void WordSearch::UpdateResults(PuzzleNode *node, std::string *testWord)
{
	++NUMBER_OF_WORDS_MATCHED;
	*(WORDS_MATCH_IN_GRID + NUMBER_OF_WORDS_MATCHED) = (*node->GetXCoord());
	*(WORDS_MATCH_IN_GRID + (NUMBER_OF_WORDS_MATCHED + 1)) = (*node->GetYCoord());
	*(WORDS_MATCH_IN_GRID + (NUMBER_OF_WORDS_MATCHED + 2)) = (*testWord);
}

