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
    bool FirstPlayerBlocked = false;
    bool SecondPlayerBlocked = false;
    int CardsChosen = 0;
    CardClass[] Cards = new CardClass[150];
    CardClass[] CardsOnTable = new CardClass[4];
    List<CardClass> FirstPlayerGameCards = new List<CardClass>();
    List<CardClass> SecondPlayerGameCards = new List<CardClass>();
    List<CardClass> FirstPlayerOutCards = new List<CardClass>();
    List<CardClass> SecondPlayerOutCards = new List<CardClass>();

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
    }

    Players ChangeTurn(Players Item)
    {
        foreach (var card in FirstPlayerGameCards)
            card.setVisible(false);
        foreach (var card in SecondPlayerGameCards)
            card.setVisible(false);
        skip1.Visible = false;
        skip2.Visible = false;

        if (Item == Players.First)
        {
            MessageBox.Show("Player 2 turn");
            foreach (var card in SecondPlayerGameCards)
                card.setVisible(true);
            skip2.Visible = true;
            Item = Players.Second;
        }
        else
        {
            MessageBox.Show("Player 1 turn");
            foreach (var card in FirstPlayerGameCards)
                card.setVisible(true);
            skip1.Visible = true;
            Item = Players.First;
            
        }
        return Item;
    }

    void MoveCurrentCard(CardClass Item)
    {
        if ((Turn == Players.First && Item.PlayerOwner == CardOwner.First) ||
            (Turn == Players.Second && Item.PlayerOwner == CardOwner.Second))
        {
            if ((Item.PlayerOwner == CardOwner.First && !FirstPlayerBlocked) ||
                (Item.PlayerOwner == CardOwner.Second && !SecondPlayerBlocked))
            {
                int TableIndex;
                if (Item.PlayerOwner == CardOwner.First)
                    if (CardsOnTable[1] == null)
                        TableIndex = 1;
                    else
                    {
                        TableIndex = 0;
                        FirstPlayerBlocked = true;
                    }
                else
                    if (CardsOnTable[2] == null)
                        TableIndex = 2;
                    else
                    {
                        TableIndex = 3;
                        SecondPlayerBlocked = true;
                    }
                ++CardsChosen;
                CardsOnTable[TableIndex] = Item;
                Item.setImagePosition(244 + TableIndex * 128, 268);
                Item.ImageBox.BringToFront();
                Item.Status = CardStatus.Table;
                Turn = ChangeTurn(Turn);
                if (CardsChosen > 1)
                    CompareCards();
            }
        }
    }

    void GetNewCard(CardClass Item)
    {
        if ((Turn == Players.First && Item.PlayerOwner == CardOwner.First) ||
            (Turn == Players.Second && Item.PlayerOwner == CardOwner.Second))
        {
            if ((Item.PlayerOwner == CardOwner.First && !FirstPlayerBlocked) ||
                (Item.PlayerOwner == CardOwner.Second && !SecondPlayerBlocked))
            {
                int index;
                if (Item.PlayerOwner == CardOwner.First)
                    index = FirstPlayerGameCards.IndexOf(Item);
                else
                    index = SecondPlayerGameCards.IndexOf(Item);

                if (Item.PlayerOwner == CardOwner.Second)
                {
                    Cards[Counter].setImagePosition(182 + index * 128, 32);
                    Cards[Counter].PlayerOwner = CardOwner.Second;
                    SecondPlayerGameCards[index] = Cards[Counter];
                }
                else
                {
                    Cards[Counter].setImagePosition(182 + index * 128, 504);
                    Cards[Counter].PlayerOwner = CardOwner.First;
                    FirstPlayerGameCards[index] = Cards[Counter];
                }

                Cards[Counter].setVisible(true);
                Controls.Add(Cards[Counter].ImageBox);
                Cards[Counter].ImageBox.BringToFront();
                Cards[Counter].Status = CardStatus.Player;
                ++Counter;
            }
        }
    }

    void CompareCards()
    {
        switch (CardsChosen)
        {
            case 2:
                {
                    if (CardsOnTable[1].Nation == CardsOnTable[2].Nation ||
                        CardsOnTable[1].Value == CardsOnTable[2].Value) //random                    
                    {
                        MessageBox.Show("Random cards");//Start random on 3 cards
                        StartNewRound();
                    }
                    else 
                        if (CardsOnTable[1].Value > CardsOnTable[2].Value)
                            if (Turn == Players.Second && CheckOtherPlayerCards(CardsOnTable[2].Nation, Players.Second) == true)
                                skip2.Enabled = true;
                            else
                            {
                                MessageBox.Show("First win");
                                RoundWinner = Players.First;
                                StartNewRound();
                            }
                        else
                            if (Turn == Players.First && CheckOtherPlayerCards(CardsOnTable[1].Nation, Players.First))
                                skip1.Enabled = true;
                            else
                            {
                                MessageBox.Show("Second win");
                                RoundWinner = Players.Second;
                                StartNewRound();
                            }
                    break;
                }
            case 3:
                {
                    if (CompareCardsOnTable() == Players.First)
                        if (Turn == Players.Second && skip2.Enabled == false && 
                            CheckOtherPlayerCards(CardsOnTable[2].Nation, Turn) == true)
                            skip2.Enabled = true;
                        else
                        {
                            MessageBox.Show("First win");
                            RoundWinner = Players.First;
                            StartNewRound();
                        }
                    else
                        if (CompareCardsOnTable() == Players.Second)
                            if (Turn == Players.First && skip1.Enabled == false &&
                                CheckOtherPlayerCards(CardsOnTable[1].Nation, Turn) == true)
                                skip1.Enabled = true;
                            else
                            {
                                MessageBox.Show("Second win");
                                RoundWinner = Players.Second;
                                StartNewRound();
                            }
                    break;
                }
            case 4:
                {
                    if (CompareCardsOnTable() == Players.First)
                    {
                        MessageBox.Show("First win");
                        RoundWinner = Players.First;
                        StartNewRound();
                    }
                    else
                        if (CompareCardsOnTable() == Players.Second)
                        {
                            MessageBox.Show("Second win");
                            RoundWinner = Players.Second;
                            StartNewRound();
                        }
                        else
                        {
                            MessageBox.Show("Random cards");
                            StartNewRound();
                        }
                    break;
                }
        }
    }

    bool CheckOtherPlayerCards(string nation, Players turn)
    {
        bool Result = false;
        if (turn == Players.First)
            foreach (var card in FirstPlayerGameCards)
            {
                if (nation == card.Nation)
                    Result = true;
            }
        else
            foreach (var card in SecondPlayerGameCards)
            {
                if (nation == card.Nation)
                    Result = true;
            }
           
        return Result;
    }

    Players CompareCardsOnTable()
    {
        Players Result;
        int Player1 = CardsOnTable[1].Value;
        int Player2 = CardsOnTable[2].Value;

        if (CardsOnTable[0] != null)
            Player1 += CardsOnTable[0].Value;
        if (CardsOnTable[3] != null)
            Player2 += CardsOnTable[3].Value;

        if (Player1 > Player2)
            Result = Players.First;
        else
            if (Player2 > Player1)
                Result = Players.Second;
            else
                Result = Players.None;

        return Result;
    }

    void StartNewRound()
    {
        for (int i = 0; i < 4; i++)
            if (CardsOnTable[i] != null)
            {
                CardsOnTable[i].setVisible(false);
                if (RoundWinner == Players.First)
                {
                    FirstPlayerOutCards.Add(CardsOnTable[i]);
                    CardsOnTable[i].setImagePosition(830, 504);
                }
                else
                {
                    SecondPlayerOutCards.Add(CardsOnTable[i]);
                    CardsOnTable[i].setImagePosition(830, 32);
                }
                CardsOnTable[i] = null;
            }
        CardsChosen = 0;
        FirstPlayerBlocked = false;
        SecondPlayerBlocked = false;
        skip1.Enabled = false;
        skip2.Enabled = false;
        if (RoundWinner != Turn)
            Turn = ChangeTurn(Turn);
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
                FirstPlayerGameCards.Add(CardItem);
            else
                SecondPlayerGameCards.Add(CardItem);
            ++Counter;
        }
        int Count = 0;
        foreach (CardClass Item in FirstPlayerGameCards)
        {
            Item.setImagePosition(182 + Count * 128, 504);
            Item.setVisible(true);
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
            Item.setVisible(true);
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
        Turn = Players.First;
    }

    private void skip1_Click(object sender, EventArgs e)
    {
        Turn = ChangeTurn(Turn);
        CompareCards();
    }

    private void skip2_Click(object sender, EventArgs e)
    {
        Turn = ChangeTurn(Turn);
    }
}

