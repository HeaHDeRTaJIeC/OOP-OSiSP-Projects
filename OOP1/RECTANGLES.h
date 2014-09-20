#include "../SHAPES/SHAPES.h"

#ifndef RECTANGLE_H
#define RECTANGLE_H

class _declspec(dllexport) Rectangles : public Shapes
{
public:
   Rectangles(int = 0, int = 0, int = 0, int = 0);
   ~Rectangles();
   virtual void Draw(HDC);

private:
   POINT *Rect;
};

#endif