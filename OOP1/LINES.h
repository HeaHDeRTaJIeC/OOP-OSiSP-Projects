#include "..\SHAPES\SHAPES.h"

#ifndef LINES_H
#define LINES_H

class Lines : public Shapes
{
public:
   Lines(int = 0, int = 0, int = 0, int = 0);
   ~Lines();
   virtual void Draw(HDC);
private:
   POINT *lin;
};

#endif