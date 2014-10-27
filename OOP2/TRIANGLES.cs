using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


public class TRIANGLES : SHAPES
{
    public TRIANGLES()
    {
        Number = 0;
    }

    public override void Add(Point current, Graphics e)
    {
        if (Number == 0)
        {
            First = current;
            ++Number;
        }
        else
            if (Number == 1)
            {
                Second = current;
                Draw(e);
                ++Number;
            }
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
    private int Number;
}

