#include "PuzzleNode.h"
#include <string>


PuzzleNode::PuzzleNode(const char &letter, const int &Xcoord, const int &Ycoord) : m_letter(letter), m_XCoord(Xcoord), m_YCoord(Ycoord), m_right(0), m_left(0), m_up(0), m_down(0), m_rightDown(0), m_rightUp(0), m_leftDown(0), m_leftUp(0)
{
}


PuzzleNode::~PuzzleNode(void)
{
}

char *PuzzleNode::GetLetter()
{
	return &m_letter;
}

void PuzzleNode::SetLetter(const char &letter)
{
	m_letter = letter;
}

int *PuzzleNode::GetXCoord()
{
	return &m_XCoord;
}

int *PuzzleNode::GetYCoord()
{
	return &m_YCoord;
}

void PuzzleNode::SetXCoord(const int &Xcoord)
{
	m_XCoord = Xcoord;
}

void PuzzleNode::SetYCoord(const int &Ycoord)
{
	m_YCoord = Ycoord;
}