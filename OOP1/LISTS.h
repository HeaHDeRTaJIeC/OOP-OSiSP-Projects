#include "../SHAPES/SHAPES.h"

#ifndef LISTS_H
#define LISTS_H

struct Node
{
	Shapes *ShapeObjPtr;
	Node *next;
};

class _declspec(dllexport) Lists 
{
public:
	Lists();
	void AddNode(Shapes *);
	void DrawList(HDC);
private:
	Node *head;
};

#endif