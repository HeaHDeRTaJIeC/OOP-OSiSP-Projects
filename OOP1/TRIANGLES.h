#include "../SHAPES/SHAPES.h"

#ifndef TRIANGLE_H
#define TRIANGLE_H

class _declspec(dllexport) Triangles : public Shapes
{
public:
   Triangles(int = 60,  int = 10, int = 40 , int = 40, int = 80, int = 40);
   ~Triangles();
   virtual void Draw(HDC);
private:
   POINT *tri;
};

#endif