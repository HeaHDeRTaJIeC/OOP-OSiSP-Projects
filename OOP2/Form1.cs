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
        Point[] Points;

        private List<Type> ShapesTypes;
        private List<SHAPES> list;
        private Random rand = new Random();

        public SuperOOP2Form()
        {
            InitializeComponent();

            AddMenu();
            PlaceToDraw = pictureBox1.CreateGraphics();
            list = new List<SHAPES>();
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
            Point one = new Point();
            Point two = new Point();
            try
            {
                one.X = int.Parse(textBox1.Text);
                one.Y = int.Parse(textBox2.Text);
                two.X = int.Parse(textBox3.Text);
                two.Y = int.Parse(textBox4.Text);
            }
            catch
            {
                one.X = rand.Next(10, pictureBox1.Width - 10);
                one.Y = rand.Next(10, pictureBox1.Height - 10);
                two.X = rand.Next(10, pictureBox1.Width - 10);
                two.Y = rand.Next(10, pictureBox1.Height - 10);
            }

            Shape = (SHAPES)Activator.CreateInstance((Type)((ToolStripButton)sender).Tag, one, two);
            Shape.Draw(PlaceToDraw);
            list.Add(Shape);
        }

    }
}
