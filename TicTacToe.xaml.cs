using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SkeeBall.Models;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TicTacToe : Window
    {
        public BaseGame Game { get; set; }

        public TicTacToe()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (Util.KeyMap[e.Key])
                {
                    case Util.KeyName.Select: //Select Button
                        NewGame(Game.GameName);
                        return;
                    case Util.KeyName.Back: //Exit the game
                        this.Close();
                        return;
                    case Util.KeyName.Gutter:
                    case Util.KeyName.Hole10:
                    case Util.KeyName.Hole20:
                    case Util.KeyName.Hole30:
                    case Util.KeyName.Hole40:
                    case Util.KeyName.Hole50:
                    case Util.KeyName.HoleL100:
                    case Util.KeyName.HoleR100:
                        if (Game.IsOver)
                            return;     // If the game has ended ignore everything except navigation keys
                        else
                            if (Game.ScoringKeyHandler(e)) UpdateScores();    //If it was a valid scoring key
                        break;
                    default:
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                //Don't care.
            }
        }

        private void UpdateScores()
        {
            Game.ActivePlayer.BallsPlayed += 1;
            Game.ActivePlayer.BallsLeft -= 1;
            Game.ActivePlayer.BallHistory.Add(Game.ActivePlayer.ThisThrow);

            if (Game.ActivePlayer.BallsLeft == 1)   //First throw
            {
                
            }
            if (Game.ActivePlayer.BallsLeft == 0)   //Second throw
            {
                
            }
            Game.ActivePlayer.LastThrow = Game.ActivePlayer.ThisThrow;
        }

        private void NewGame(string gameName)
        {
            Game = new BaseGame(gameName);
            Game.TwoPlayer = true;
            this.DataContext = Game;        //This tells it where to look for dependency properties
        }
    }
}
