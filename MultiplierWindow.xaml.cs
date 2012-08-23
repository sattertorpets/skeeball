using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
// for Bindings Class
using SkeeBall.Models;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for MultiplierWindow.xaml
    /// </summary>
    public partial class MultiplierWindow : Window
    {
        private bool _count100s;
        private string _gameName;
        int BallTimestamp = 0;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        public BaseGame Game { get; set; }

        public MultiplierWindow(string gameName)
        {
            InitializeComponent();
            _gameName = gameName;
            Game = new BaseGame(_gameName);
            Game.ActivePlayer.ThisThrow = -1;
            Game.ActivePlayer.LastThrow = -1;
            this.DataContext = Game;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            bool validKey = true;
            if (BallTimestamp == 0)     //Set the comparison timestamps in the past for the first ball
            { 
                BallTimestamp = e.Timestamp - 3000;
                Game.Last10 = e.Timestamp - 3000;
            }
            if (e.IsRepeat) return;
            
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
                        if (IsDuplicateBall(e.Timestamp, 300)) return;
                        Game.ActivePlayer.LastThrow = 10;
                        wplayer.URL = @"D:\Nick\Dropbox\Projects\Skee Ball\SkeeBallskeeball\Sounds\10-Pew.mp3";
                        wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole20:
                        if (IsDuplicateBall(e.Timestamp, 300)) return;
                        Game.ActivePlayer.LastThrow = 20;
                        wplayer.URL = @"D:\Nick\Dropbox\Projects\Skee Ball\SkeeBallskeeball\Sounds\20-Shotgun.mp3";
                        wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole30:
                        if (IsDuplicateBall(e.Timestamp, 300)) return;
                        Game.ActivePlayer.LastThrow = 30;
                        wplayer.URL = @"D:\Nick\Dropbox\Projects\Skee Ball\SkeeBallskeeball\Sounds\30-Gun.mp3";
                        wplayer.controls.play();
                        break;
                    case Util.KeyName.Hole40:
                        if (IsDuplicateBall(e.Timestamp, 300)) return;
                        Game.ActivePlayer.LastThrow = 40;
                        break;
                    case Util.KeyName.Hole50:
                        if (IsDuplicateBall(e.Timestamp, 300)) return;
                        Game.ActivePlayer.LastThrow = 50;
                        break;
                    case Util.KeyName.HoleL100: // Left 100
                    case Util.KeyName.HoleR100: // Right 100
                        if (IsDuplicateBall(e.Timestamp, 300)) return;    
                        if (_count100s)
                        {
                            Game.ActivePlayer.LastThrow = 100;  //disabled for Classic, enabled for Modern
                        }
                        else
                        {
                            Game.ActivePlayer.LastThrow = 10;
                        }
                        break;
                    case Util.KeyName.Gutter:
                        if (IsDuplicateBall(e.Timestamp, 200) && Game.ActivePlayer.LastThrow == 0) return;  //Switch bounce protection
                        if (e.Timestamp > (Game.Last10 + 2600))   //If its been more than 2.6s since the 10 switch was tripped, this was a gutter
                        {
                            Game.ActivePlayer.LastThrow = 0;
                        }
                        else return;        //If it wasn't, then ignore it
                        break;
                    case Util.KeyName.Select: //Select Button
                        NewGame();
                        break;
                    case Util.KeyName.Back: //Exit the game
                        this.Close();
                        break;
                    default:
                        validKey = false;
                        break;
                }
                if (validKey)     //If a valid key was pressed
                {
                    BallTimestamp = e.Timestamp;
                    Game.ActivePlayer.BallsPlayed += 1;
                    Game.ActivePlayer.Score += Game.ActivePlayer.LastThrow*Game.ActivePlayer.Multiplier;
                    if (_gameName == "310" || _gameName == "510")       //If it is a game that scores balls played
                    {
                        if (Game.ActivePlayer.Score == Int16.Parse(_gameName))  //If the score was hit exactly
                        {
                            GameOverGolf(Game.ActivePlayer.BallsPlayed);           //Call the game over function for this game type
                        }
                        else if (Game.ActivePlayer.Score > Int16.Parse(_gameName))  //If the player went over the target score
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

        private void NewGame()
        {
            Game.ActivePlayer.BallsPlayed = 0;
            Game.ActivePlayer.Score = 0;
        }

        private void GameOver(int score)
        {
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

                if (_count100s) Util.WriteScores(Game.HighScores, "ModernNames", "ModernScores");
                else Util.WriteScores(Game.HighScores, "ClassicNames", "ClassicScores");
            }
            NewGame();
        }
        private void GameOverGolf(int balls)
        {
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

                if (_gameName == "310") Util.WriteScores(Game.HighScores, "Game310Names", "Game310Scores");
                if (_gameName == "510") Util.WriteScores(Game.HighScores, "Game510Names", "Game510Scores");
            }
            NewGame();
        }
    }
}
