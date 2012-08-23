using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
using SkeeBall.Models;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for CricketWindow.xaml
    /// </summary>
    public partial class CricketWindow : Window
    {
        private string _gameName;
        int BallTimestamp = 0;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        public BaseGame Game { get; set; }

        public CricketWindow(string gameName)
        {
            InitializeComponent();
            _gameName = gameName;
            NewGame(_gameName, false);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
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
                        Game.ActivePlayer.ThisThrow = 100;  //disabled for Classic, enabled for Modern
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
                        NewGame(_gameName, Game.TwoPlayer);
                        return;
                    case Util.KeyName.Back: //Exit the game
                        this.Close();
                        return;
                    case Util.KeyName.Up:   //Add a player
                        Game.TwoPlayer = true;
                        NewGame(_gameName, true);
                        return;
                    case Util.KeyName.Down: //Remove a player
                        Game.TwoPlayer = false;
                        NewGame(_gameName, false);
                        return;
                    default:
                        validKey = false;
                        break;
                }
                BallTimestamp = e.Timestamp;
                if (validKey)     //If a valid key was pressed
                {
                    Game.ActivePlayer.BallsPlayed += 1;
                    Game.ActivePlayer.BallsLeft -= 1;
                    switch (Game.ActivePlayer.ThisThrow)
                    {
                        case 100:
                            if (Game.ActivePlayer.Num100s <= 3)
                                Game.ActivePlayer.Num100s++;
                            else if (Game.ActivePlayer.Num100s == 3 && Game.OtherPlayer.Num100s < 3)
                                Game.ActivePlayer.Score += 100;
                            break;
                        case 50:
                            if (Game.ActivePlayer.Multiplier == 1)  //temp code for machine without 100s
                            {
                                if (Game.ActivePlayer.Num100s < 3)
                                {
                                    Game.ActivePlayer.Num100s++;
                                    break;
                                }
                                else if (Game.OtherPlayer.Num100s < 3)
                                {
                                    Game.ActivePlayer.Score += 55;  //only add 55 because 100 would probably be too much
                                    break;
                                }
                                //otherwise, fall through and treat it as a normal 50
                            }
                            if (Game.ActivePlayer.Num50s < 3)
                                Game.ActivePlayer.Num50s++;
                            else if (Game.ActivePlayer.Num50s == 3 && Game.OtherPlayer.Num50s < 3)
                                Game.ActivePlayer.Score += 50;
                            break;
                        case 40:
                            if (Game.ActivePlayer.Num40s < 3)
                                Game.ActivePlayer.Num40s++;
                            else if (Game.ActivePlayer.Num40s == 3 && Game.OtherPlayer.Num40s < 3)
                                Game.ActivePlayer.Score += 40;
                            break;
                        case 30:
                            if (Game.ActivePlayer.Num30s < 3)
                                Game.ActivePlayer.Num30s++;
                            else if (Game.ActivePlayer.Num30s == 3 && Game.OtherPlayer.Num30s < 3)
                                Game.ActivePlayer.Score += 30;
                            break;
                        case 20:
                            if (Game.ActivePlayer.Num20s < 3)
                                Game.ActivePlayer.Num20s++;
                            else if (Game.ActivePlayer.Num20s == 3 && Game.OtherPlayer.Num20s < 3)
                                Game.ActivePlayer.Score += 20;
                            break;
                        case 10:
                            if (Game.ActivePlayer.Num10s < 3)
                                Game.ActivePlayer.Num10s++;
                            else if (Game.ActivePlayer.Num10s == 3 && Game.OtherPlayer.Num10s < 3)
                                Game.ActivePlayer.Score += 10;
                            break;
                        default:
                            break;
                    }
                    if (Game.ActivePlayer.ThisThrow == 50)
                        Game.ActivePlayer.Multiplier = 1;
                    else
                        Game.ActivePlayer.Multiplier = 0;

                    Game.ActivePlayer.LastThrow = Game.ActivePlayer.ThisThrow;
                    if (Game.ActivePlayer.BallsLeft == 0)
                    {
                        SwitchPlayers();
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

        private void NewGame(string gameName, bool twoPlayer)
        {
            Game = new BaseGame(gameName);
            Game.ActivePlayer.ThisThrow = -1;
            Game.ActivePlayer.LastThrow = -1;
            Game.TwoPlayer = twoPlayer;
            this.DataContext = Game;        //This tells it where to look for dependency properties
            NewRound();
        }

        private void NewRound()
        {
            if (Game.TwoPlayer)
            {
                //Check 2P game end conditions
                if (Game.Player1.TargetsComplete == 18 && Game.Player1.Score > Game.Player2.Score)
                    GameOverGolf(Game.Player1.BallsPlayed);
                else if (Game.Player2.TargetsComplete == 18 && Game.Player2.Score > Game.Player1.Score)
                    GameOverGolf(Game.Player2.BallsPlayed);
                else
                {
                    Game.Player1.BallsLeft = 3;
                    Game.Player2.BallsLeft = 3;
                    Game.ActivePlayer = Game.Player1;
                    Game.OtherPlayer = Game.Player2;
                }
            }
            else
            {
                //Check 1P game end conditions
                if (18 == Game.ActivePlayer.TargetsComplete)
                {
                    GameOverGolf(Game.ActivePlayer.BallsPlayed);
                }
                else
                    Game.ActivePlayer.BallsLeft = 3;
            }
        }

        private void SwitchPlayers()
        {
            if (Game.TwoPlayer)
            {
                if (Game.ActivePlayer.Equals(Game.Player2))
                {
                    NewRound();
                }
                else
                {
                    Game.ActivePlayer = Game.Player2;
                    Game.OtherPlayer = Game.Player1;
                }
            }
            else
                NewRound();
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
