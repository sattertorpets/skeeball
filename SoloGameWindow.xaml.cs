using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
// for Bindings Class
using SkeeBall.Models;
using System.Windows.Controls;
using System.Media;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for SoloGameWindow.xaml
    /// </summary>
    public partial class SoloGameWindow : Window
    {
        public BaseGame Game { get; set; }

        public SoloGameWindow(string gameName)
        {
            InitializeComponent();
            NewGame(gameName);
            if (gameName == "Multi")  //TODO: replace this with databinding to Game.HasMultiplier
            {
                cnvsMultiplier.Visibility = Visibility.Hidden;
                colHighScore.Width = new GridLength(0, GridUnitType.Star);
                colMultiTarget.Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                cnvsMultiplier.Visibility = Visibility.Visible;
                colHighScore.Width = new GridLength(1, GridUnitType.Star);
                colMultiTarget.Width = new GridLength(0, GridUnitType.Star);
            }
            if (gameName == "ThreeTen" || gameName == "FiveTen")
            {
                colMultiplier.Width = new GridLength(0, GridUnitType.Star);
                colScoreTarget.Width = new GridLength(1, GridUnitType.Star);
                if (gameName == "ThreeTen")
                    targetScoreBox.Text = "310";
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                keyList.Items.Add(e.Key + " @ " + e.Timestamp);
                keyList.ScrollIntoView(keyList.Items.GetItemAt(keyList.Items.Count - 1));
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
            Game.ActivePlayer.Score += Game.ActivePlayer.ThisThrow * Game.ActivePlayer.Multiplier;
            if (Game.ActivePlayer.ThisThrow < 30)
            {
                Game.ActivePlayer.Multiplier = 1;
                Game.ActivePlayer.MultiTarget = 30;
            }
            if (Game.ActivePlayer.MultiTarget == Game.ActivePlayer.ThisThrow && Game.GameName == "Multi")
            {
                Game.ActivePlayer.Multiplier++;
                Game.IncrementMultiTarget();
            }
            Game.ActivePlayer.LastThrow = Game.ActivePlayer.ThisThrow;

            if (Game.GameName == "ThreeTen" || Game.GameName == "FiveTen")       //If it is a game that scores balls played
            {

                if (Game.GameName == "ThreeTen" && Game.ActivePlayer.Score == 310 || Game.GameName == "FiveTen" && Game.ActivePlayer.Score == 510)  //If the score was hit exactly
                {
                    if (Game.GameOverGolf(Game.ActivePlayer.BallsPlayed, this))       //Call the game over function for this game type
                        NewGame(Game.GameName);
                    else
                        this.Close();
                }
                else if (Game.GameName == "ThreeTen" && Game.ActivePlayer.Score > 310 || Game.GameName == "FiveTen" && Game.ActivePlayer.Score > 510)  //If the player went over the target score
                {
                    Game.ActivePlayer.Score -= Game.ActivePlayer.LastThrow; //Don't score the throw.
                }
            }
            else if (Game.ActivePlayer.BallsPlayed == 9)    //For normal games, only end if the last ball is thrown
            {
                if (Game.GameOver(Game.ActivePlayer.Score, this))
                    NewGame(Game.GameName);
                else
                    this.Close();
            }
        }

        private void NewGame(string gameName)
        {
            Game = new BaseGame(gameName);
            this.DataContext = Game;        //This tells it where to look for dependency properties
        }
    }
}
