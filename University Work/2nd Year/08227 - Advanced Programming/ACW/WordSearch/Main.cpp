// #######################################################################################
// ##                                                                                   ##
// ##  This main.cpp file WILL BE REPLACED during the marking procedure.                ##
// ##  Therefore all of your implementation code MUST be in your WordSearch class,      ##
// ##  or any other new classes that you create and use inside of the WordSearch class. ##
// ##                                                                                   ##
// #######################################################################################

#include <iostream>
#include <exception>
#include "WordSearch.h"

using namespace std;

void main()
{
	WordSearch puzzle;

	// Load the word search puzzle and dictionary into all of the data structures
	try {
		if (!puzzle.ReadSimplePuzzle()) throw exception("Cannot read simple word search");
		if (!puzzle.ReadSimpleDictionary()) throw exception("Cannot read simple dictionary");
		if (!puzzle.ReadAdvancedPuzzle()) throw exception("Cannot read advanced word search");
		if (!puzzle.ReadAdvancedDictionary()) throw exception("Cannot read advanced dictionary");

		// Run the simple puzzle grid data structure with the simple dictionary data structure - execute a number of time and then calculate the average time to execute
		if (puzzle.SolveSimplePuzzleWithSimpleDictionary())
			puzzle.WriteResults("results_simple_puzzle_simple_dictionary.txt");

		// Run the simple puzzle grid data structure with the advanced dictionary data structure - execute a number of time and then calculate the average time to execute
		if (puzzle.SolveSimplePuzzleWithAdvancedDictionary())
			puzzle.WriteResults("results_simple_puzzle_advanced_dictionary.txt");

		// Run the advanced puzzle grid data structure with the simple dictionary data structure - execute a number of time and then calculate the average time to execute
		if (puzzle.SolveAdvancedPuzzleWithSimpleDictionary())
			puzzle.WriteResults("results_advanced_puzzle_simple_dictionary.txt");

		// Run the advanced puzzle grid data structure with the advanced dictionary data structure - execute a number of time and then calculate the average time to execute
		if (puzzle.SolveAdvancedPuzzleWithAdvancedDictionary())
			puzzle.WriteResults("results_advanced_puzzle_advanced_dictionary.txt");
	}
	catch (exception& e) {
		cout << endl << "EXCEPTION: " << e.what() << endl;
	}

	system("pause");
}