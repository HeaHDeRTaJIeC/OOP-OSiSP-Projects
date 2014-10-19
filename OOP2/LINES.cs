using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class LINES : SHAPES
{
    public LINES(Point first, Point second)
    {
        First = first;
        Second = second;
    }

    public override void Draw(Graphics e)
    {
        e.DrawLine(new Pen(Color.Black), First, Second);
    }

    private Point First;
    private Point Second;
}

