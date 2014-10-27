using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

public class POLYLINES : SHAPES
{
    public POLYLINES()
    {
        PointList = new List<Point>();
        Number = 0;
    }

    public override void Add(Point current, Graphics e)
    {
        if (Number == 0)
        {
            PointList.Add(current);
            ++Number;
        }
        else
        {
            PointList.Add(current);
            Draw(e);
        }
    }

    public override void Draw(Graphics e)
    {
        Point[] PointArray = new Point[PointList.Count];
        PointArray = PointList.ToArray();
        for (int i = 0; i < PointList.Count; i++)
            e.DrawLines(new Pen(Color.Black), PointArray);
    }

    private List<Point> PointList;
    private int Number;
}
