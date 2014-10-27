using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace OOP2
{
    public partial class SuperOOP2Form : Form
    {
        Graphics PlaceToDraw;
        SHAPES Shape;

        private List<Type> ShapesTypes;
        private List<SHAPES> list;
        private Random rand = new Random();

        public SuperOOP2Form()
        {
            InitializeComponent();

            PlaceToDraw = pictureBox1.CreateGraphics();
            AddMenu();
            list = new List<SHAPES>();
            pictureBox1.MouseClick += new MouseEventHandler(MouseClickShape);
        }

        private void AddMenu()
        {
            var directory = new DirectoryInfo("PROJECT_DLL");
            ShapesTypes = directory.GetFiles("*.dll").
                Select(x => Assembly.LoadFile(x.FullName).GetTypes().
                    Where(y => y.IsSubclassOf(typeof(SHAPES))))
                    .Aggregate(new List<Type>(), (acc, x) => { acc.AddRange(x); return acc; });
            foreach (var type in ShapesTypes)
            {
                var MenuItem = new ToolStripButton
                {
                    Name = type.Name,
                    Text = type.Name,
                    Tag = type,
                };
                MenuItem.Click += ShapeItemClick;
                shapesToolStripMenuItem.DropDownItems.Add(MenuItem);
            }

        }


        private void ShapeItemClick(object sender, EventArgs e)
        {
            Shape = (SHAPES)Activator.CreateInstance((Type)((Control)sender).Tag);
            list.Add(Shape);
        }

        private void clearFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void paintListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var Item in list)
            {
                Item.Draw(PlaceToDraw);
            }
        }

        private void MouseClickShape(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Shape.Add(e.Location, PlaceToDraw);
            else
                if (e.Button == MouseButtons.Right)
                    list.Add(Shape);
        }
    }
}
