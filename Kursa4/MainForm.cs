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


namespace _5Sem_Kursa4
{
    public partial class MainForm : Form
    {
        int Counter;
        int CardsChosen = 0;
        int Round;

        bool FirstPlayerBlocked = false;
        bool SecondPlayerBlocked = false;
        CardClass[] Cards = new CardClass[150];
        CardClass[] CardsOnTable = new CardClass[4];
        List<CardClass> FirstPlayerGameCards = new List<CardClass>();
        List<CardClass> SecondPlayerGameCards = new List<CardClass>();
        List<CardClass> FirstPlayerOutCards = new List<CardClass>();
        List<CardClass> SecondPlayerOutCards = new List<CardClass>();
        List<CardClass> RandomGameCards = new List<CardClass>();

        Players Turn = Players.First;
        Players RoundWinner;

        public enum Players
        {
            First,
            Second,
            None
        }

        public MainForm()
        {
            InitializeComponent();
            CardClass.SetLocalPath(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
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
                Cards[i].onCardClick += GetNewCard;
                Cards[i].onCardClick += MoveCurrentCard;
            }
            skip1.Enabled = false;
            skip2.Enabled = false;
            skip1.Visible = false;
            skip2.Visible = false;
            RoundLabel.Visible = false;
            TimeLabel.Visible = false;
            NumberOutCards1Label.Visible = false;
            NumberOutCards2Label.Visible = false;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FirstPlayerGameCards.Clear();
            SecondPlayerGameCards.Clear();
            FirstPlayerOutCards.Clear();
            SecondPlayerOutCards.Clear();
            CardClass CardItem;
            Counter = 0;
            Round = 0;
            for (int i = 0; i < 150; i++)
                Controls.Add(Cards[i].ImageBox);
            for (int i = 0; i < 10; i++)
            {
                CardItem = Cards[Counter];
                if (i % 2 == 0)
                    FirstPlayerGameCards.Add(CardItem);
                else
                    SecondPlayerGameCards.Add(CardItem);
                ++Counter;
            }
            int Count = 0;
            foreach (CardClass Item in FirstPlayerGameCards)
            {
                Item.setImagePosition(182 + Count * 128, 504);
                Item.setVisible(false);
                Controls.Add(Item.ImageBox);
                Item.ImageBox.BringToFront();
                Item.Status = CardStatus.Player;
                Item.PlayerOwner = CardOwner.First;
                ++Count;
            }

            Count = 0;
            foreach (CardClass Item in SecondPlayerGameCards)
            {
                Item.setImagePosition(182 + Count * 128, 32);
                Item.setVisible(false);
                Controls.Add(Item.ImageBox);
                Item.ImageBox.BringToFront();
                Item.Status = CardStatus.Player;
                Item.PlayerOwner = CardOwner.Second;
                ++Count;
            }

            skip1.Enabled = false;
            skip2.Enabled = false;
            skip2.Visible = false;
            skip1.Visible = true;
            Turn = Players.Second;
            Turn = ChangeTurn(Turn);
            RoundLabel.Visible = true;
            TimeLabel.Visible = true;
            NumberOutCards1Label.Visible = true;
            NumberOutCards2Label.Visible = true;
            RoundLabel.Text = "Round " + (Round + 1).ToString();
            TimeLabel.Text = "Time\n" + (Round * 5).ToString() + ":00";
            NumberOutCards1Label.Text = FirstPlayerOutCards.Count.ToString();
            NumberOutCards2Label.Text = SecondPlayerOutCards.Count.ToString();
        }

        private void skip1_Click(object sender, EventArgs e)
        {
            Turn = ChangeTurn(Turn);
            CompareCards();
        }

        private void skip2_Click(object sender, EventArgs e)
        {
            Turn = ChangeTurn(Turn);
            CompareCards();
        }
    }
}

