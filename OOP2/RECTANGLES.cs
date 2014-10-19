using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class RECTANGLES : SHAPES
{
    public RECTANGLES(Point first, Point second)
    {
        First = first;
        Second = second;
    }

    public override void Draw(Graphics e)
    {
        e.DrawRectangle(new Pen(Color.Black), First.X, First.Y, Second.X - First.X, Second.Y - First.Y);
    }

    private Point First;
    private Point Second;

}

