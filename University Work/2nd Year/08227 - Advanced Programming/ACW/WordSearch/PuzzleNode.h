#pragma once
#include <string>


class PuzzleNode
{
public:
	PuzzleNode(const char &letter, const int &Xcoord, const int &Ycoord);
	~PuzzleNode(void);

	char *PuzzleNode::GetLetter();
	void PuzzleNode::SetLetter(const char &letter);

	int *PuzzleNode::GetXCoord();
	int *PuzzleNode::GetYCoord();

	void PuzzleNode::SetXCoord(const int &Xcoord);
	void PuzzleNode::SetYCoord(const int &Ycoord);

private:
	char m_letter;
	int m_XCoord;
	int m_YCoord;

	PuzzleNode *m_right;
	PuzzleNode *m_left;
	PuzzleNode *m_up;
	PuzzleNode *m_down;
	PuzzleNode *m_rightUp;
	PuzzleNode *m_rightDown;
	PuzzleNode *m_leftUp;
	PuzzleNode *m_leftDown;

	friend class WordSearch;
};

