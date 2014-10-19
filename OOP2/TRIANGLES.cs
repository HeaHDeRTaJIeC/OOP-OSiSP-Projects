using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class TRIANGLES : SHAPES
{
    public TRIANGLES(Point first, Point second)
    {
        First = first;
        Second = second;
    }
    public override void Draw(Graphics e)
    {
        Point one = new Point();
        Point two = new Point();
        one.X = (First.X + Second.X) / 2;
        one.Y = First.Y;
        two.X = First.X;
        two.Y = Second.Y;
        e.DrawLine(new Pen(Color.Black), one, two);
        two.X = Second.X;
        e.DrawLine(new Pen(Color.Black), one, two);
        one.X = First.X;
        one.Y = Second.Y;
        e.DrawLine(new Pen(Color.Black), one, two);
    }

    private Point First;
    private Point Second;
}

