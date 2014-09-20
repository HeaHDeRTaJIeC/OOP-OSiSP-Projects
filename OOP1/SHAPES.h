// SHAPES.h
#include "windows.h"

#ifndef SHAPES_H
#define SHAPES_H

class _declspec(dllexport) Shapes
{
public:
	virtual void Draw(HDC) = 0;
};

#endif