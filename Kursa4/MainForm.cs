using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using MainClass;


public partial class MainForm : Form
{
    int Counter;
    CardClass[] Cards = new CardClass[150];
    List<CardClass> Player1Cards = new List<CardClass>();
    List<CardClass> Player2Cards = new List<CardClass>();

    public MainForm()
    {
        InitializeComponent();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        StreamReader str = new StreamReader(CardClass.LocalPath + "info.txt");
        string line;
        for (int i = 0; i < 150; i++)
        {
            line = str.ReadLine();
            string[] data = line.Split(';');
            int value = 0;
            string name = data[0];
            try
            {
                value = int.Parse(data[1]);
            }
            catch
            {
                MessageBox.Show("Error parse");
            }
            string nation = data[2];
            Cards[i] = new CardClass(name, nation, i + 1, value, false);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        
        /*Cards[0] = new CardClass("Messi", "Argentina", 1, 90, true);
        Cards[1] = new CardClass("Schweinsteiger", "Portugal", 2, 89, true);
        Cards[2] = new CardClass("Havi", "Spain", 3, 84, true);
        Cards[3] = new CardClass("Iniesta", "Spain", 4, 83, true);
        Cards[4] = new CardClass("C. Ronaldo", "Spain", 5, 91, true);
        for (int i = 0; i < 5; i++)
        {
            Cards[i].setImagePosition(182 + i * 128, 504);
            Cards[i].setVisible(true);
            Controls.Add(Cards[i].ImageBox);
        }*/
        //200, 365
    }

    private void button2_Click(object sender, EventArgs e)
    {

    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CardClass CardItem;
        Counter = 0;
        for (int i = 0; i < 150; i++)
            Controls.Add(Cards[i].ImageBox);
        for (int i = 0; i < 10; i++)
        {
            CardItem = Cards[Counter];
            if (i % 2 == 0)
                Player1Cards.Add(CardItem);
            else
                Player2Cards.Add(CardItem);
            ++Counter;
        }
        int Count = 0;
        foreach (CardClass Item in Player1Cards)
        {
            Item.setImagePosition(182 + Count * 128, 504);
            Item.setVisible(true);
            Controls.Add(Item.ImageBox);
            Item.ImageBox.BringToFront();
            ++Count;
        }

        Count = 0;
        foreach (CardClass Item in Player2Cards)
        {
            Item.setImagePosition(182 + Count * 128, 32);
            Item.setVisible(true);
            Controls.Add(Item.ImageBox);
            Item.ImageBox.BringToFront();
            ++Count;
        }

    }
}

