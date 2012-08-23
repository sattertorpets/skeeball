using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
// for Bindings Class
using SkeeBall.Models;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for SoloGameWindow.xaml
    /// </summary>
    public partial class SoloGameWindow : Window
    {
        private bool _count100s;
        private string _gameName;
        int BallTimestamp = 0;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        public BaseGame Game { get; set; }

        public SoloGameWindow(string gameName, bool count100s)
        {
            InitializeComponent();
            _gameName = gameName;
            _count100s = count100s;
            NewGame(_gameName);
            if (_gameName == "Multi")
            {
                lblHighestScore.Visibility = Visibility.Hidden;
                txtHighScore.Visibility = Visibility.Hidden;

                txtMultiplier.Visibility = Visibility.Visible;
                txtMultiTarget.Visibility = Visibility.Visible;
                lblMultiTarget.Visibility = Visibility.Visible;
            }
            else
            {
                lblHighestScore.Visibility = Visibility.Visible;
                txtHighScore.Visibility = Visibility.Visible;

                txtMultiplier.Visibility = Visibility.Hidden;
                txtMultiTarget.Visibility = Visibility.Hidden;
                lblMultiTarget.Visibility = Visibility.Hidden;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Game.GameIsOver && !(Util.KeyMap[e.Key] == Util.KeyName.Back || Util.KeyMap[e.Key] == Util.KeyName.Select)) return;
            // If the game has ended ignore everything except navigation keys
            if (BallTimestamp == 0)     //Set the comparison timestamps in the past for the first ball
            {
                BallTimestamp = e.Timestamp - 3000;
                Game.Last10 = e.Timestamp - 3000;
            }
            //If its a repeat key, or is a switch bounce, ignore it and return
            if (e.IsRepeat || (e.Key == Game.LastKey && BallTimestamp + 100 > e.Timestamp))
                return;
            Game.LastKey = e.Key;
            keyList.Items.Add(e.Key + " @ " + e.Timestamp);
            keyList.ScrollIntoView(keyList.Items.GetItemAt(keyList.Items.Count - 1));
            bool validKey = true;
            // Enum Hole10, Hole20, Hole30, Hole40, Hole50, HoleL100, HoleR100, Gutter, UpKey, DownKey, SelectKey, BackKey;

            /* 
            A = 10, B = 20, C = 30, D = 40, E = 50, F = L100, G = R100, L = Gutter, K = BackButton
             * I = UpArrow, J = DownArrow, H = Select = SpaceBar
            */

            try
            {
                switch (Util.KeyMap[e.Key])
                {
                    case Util.KeyName.Hole10:
                        Game.Last10 = e.Timestamp;
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        Game.ActivePlayer.ThisThrow = 10;
                        wplayer.URL = Path.GetFullPath(@"Sounds\Sound10.mp3");
                        //wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole20:
                        Game.Last10 = e.Timestamp;
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        Game.ActivePlayer.ThisThrow = 20;
                        wplayer.URL = Path.GetFullPath(@"Sounds\Sound20.mp3");
                        //wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole30:
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        Game.ActivePlayer.ThisThrow = 30;
                        wplayer.URL = Path.GetFullPath(@"Sounds\Sound30.mp3");
                        //wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole40:
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        Game.ActivePlayer.ThisThrow = 40;
                        wplayer.URL = Path.GetFullPath(@"Sounds\Sound40.mp3");
                        //wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole50:
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        Game.ActivePlayer.ThisThrow = 50;
                        wplayer.URL = Path.GetFullPath(@"Sounds\Sound50.mp3");
                        //wplayer.controls.play();
                        break;
                    case Util.KeyName.HoleL100: // Left 100
                    case Util.KeyName.HoleR100: // Right 100
                        if (IsDuplicateBall(e.Timestamp, 450)) { BallTimestamp = e.Timestamp; return; }
                        if (_count100s)
                        {
                            Game.ActivePlayer.ThisThrow = 100;  //disabled for Classic, enabled for Modern
                        }
                        else
                        {
                            Game.ActivePlayer.ThisThrow = 10;
                        }
                        break;
                    case Util.KeyName.Gutter:
                        //if (IsDuplicateBall(e.Timestamp, 200) && Game.ActivePlayer.LastThrow == 0) return;  //Switch bounce protection
                        if (e.Timestamp > (Game.Last10 + 2600))   //If its been more than 2.6s since the 10 switch was tripped, this was a gutter
                        {
                            Game.ActivePlayer.ThisThrow = 0;
                        }
                        else return;        //If it wasn't, then ignore it
                        break;
                    case Util.KeyName.Select: //Select Button
                        NewGame(_gameName);
                        return;
                    case Util.KeyName.Back: //Exit the game
                        this.Close();
                        return;
                    default:
                        validKey = false;
                        break;
                }
                BallTimestamp = e.Timestamp;
                if (validKey)     //If a valid key was pressed
                {
                    Game.ActivePlayer.BallsPlayed += 1;
                    Game.ActivePlayer.Score += Game.ActivePlayer.ThisThrow * Game.ActivePlayer.Multiplier;
                    if (Game.ActivePlayer.ThisThrow < 30)
                    {
                        Game.ActivePlayer.Multiplier = 1;
                        Game.ActivePlayer.MultiTarget = 30;
                    }
                    if (Game.ActivePlayer.MultiTarget == Game.ActivePlayer.ThisThrow && _gameName == "Multi")
                    {
                        Game.ActivePlayer.Multiplier++;
                        Game.ActivePlayer.MultiTarget = IncrementMultiTarget(Game.ActivePlayer.MultiTarget);
                    }
                    Game.ActivePlayer.LastThrow = Game.ActivePlayer.ThisThrow;
                    
                    if (_gameName == "ThreeTen" || _gameName == "FiveTen")       //If it is a game that scores balls played
                    {

                        if (_gameName == "ThreeTen" && Game.ActivePlayer.Score == 310 || _gameName == "FiveTen" && Game.ActivePlayer.Score == 510)  //If the score was hit exactly
                        {
                            GameOverGolf(Game.ActivePlayer.BallsPlayed);           //Call the game over function for this game type
                        }
                        else if (_gameName == "ThreeTen" && Game.ActivePlayer.Score > 310 || _gameName == "FiveTen" && Game.ActivePlayer.Score > 510)  //If the player went over the target score
                        {
                            Game.ActivePlayer.Score -= Game.ActivePlayer.LastThrow; //Don't score the throw.
                        }
                    }
                    else if (Game.ActivePlayer.BallsPlayed == 9)    //For normal games, only end if the last ball is thrown
                    {
                        GameOver(Game.ActivePlayer.Score);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                //Don't care.
            }


        }

        private bool IsDuplicateBall(int time, int timeout)
        {
            if ((time - BallTimestamp) <= timeout)
            {
                Console.WriteLine("Double switch detected");
                return true;
            }
            return false;
        }
        
        private int IncrementMultiTarget(int target)
        {
            target += 10;
            if (target == 60) target = 100;
            return target;
        }
        
        private int BallThrown(KeyEventArgs e)
        {
            return 0;
        }

        private void NewGame(string gameName)
        {
            Game = new BaseGame(gameName);
            Game.ActivePlayer.ThisThrow = -1;
            Game.ActivePlayer.LastThrow = -1;
            if (gameName == "Multi") Game.ActivePlayer.MultiTarget = 30;
            this.DataContext = Game;        //This tells it where to look for dependency properties
        }

        private void GameOver(int score)
        {
            Game.GameIsOver = true;
            int numHighScores = Game.HighScores.Count - 1;
            if (Game.HighScores.ElementAt(numHighScores).Value < score)  //if higher than the last item in the high score list
            {
                Game.HighScores.RemoveAt(numHighScores);  //remove last score
                Score newHighScore = new Score();
                PlayerList playerListWindow = new PlayerList();
                playerListWindow.Owner = this;
                playerListWindow.ShowDialog();
                newHighScore.Name = playerListWindow.SelectedPlayer;
                newHighScore.Value = score;
                Game.HighScores.Add(newHighScore);     //add new score at the bottom of the list
                Game.HighScores.ReverseBubbleSort();
                
                Util.WriteScores(Game.HighScores, _gameName+"Names", _gameName+"Scores");
            }
        }
        private void GameOverGolf(int balls)
        {
            Game.GameIsOver = true;
            int numHighScores = Game.HighScores.Count - 1;
            if (Game.HighScores.ElementAt(numHighScores).Value > balls)  //if smaller than the last item in the high score list
            {
                Game.HighScores.RemoveAt(numHighScores);  //remove last score
                Score newHighScore = new Score();
                PlayerList playerListWindow = new PlayerList();
                playerListWindow.Owner = this;
                playerListWindow.ShowDialog();
                newHighScore.Name = playerListWindow.SelectedPlayer;
                newHighScore.Value = balls;
                Game.HighScores.Add(newHighScore);     //add new score at the bottom of the list
                Game.HighScores.ReverseBubbleGolfSort();

                Util.WriteScores(Game.HighScores, _gameName + "Names", _gameName + "Scores");
            }
        }
    }
}
