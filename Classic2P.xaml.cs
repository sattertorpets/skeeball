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
using System.Windows.Navigation;
using System.Windows.Shapes;
// for Bindings Class
using SkeeBall.Models;
using System.Collections.Specialized;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Classic2P.xaml
    /// </summary>
    public partial class Classic2P : Window
    {
        int BallTimestamp = 0;
        public Classic Model { get; set; }

        public Classic2P()
        {
            InitializeComponent();
            Model = new Classic();
            Model.HighScores = Util.LoadScores("ClassicNames", "ClassicScores");
            Model.HighestScore = Model.HighScores.First<Score>().Value;
            this.DataContext = Model;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;
            if ((e.Timestamp - BallTimestamp) <= 1000)
            {
                Console.WriteLine("Double switch detected");
                return;
            }
            bool NoKey = false;
            Enum Hole10, Hole20, Hole30, Hole40, Hole50, HoleL100, HoleR100, Gutter, UpKey, DownKey, SelectKey, BackKey;

            switch (e.Key)
            {
                case Key.A:
                    Model.Machine1.Score += 10;
                    break;
                case Key.B:
                    Model.Machine1.Score += 20;
                    break;
                case Key.C:
                    Model.Machine1.Score += 30;
                    break;
                case Key.D:
                    Model.Machine1.Score += 40;
                    break;
                case Key.E:
                    Model.Machine1.Score += 50;
                    break;
                case Key.F:
                    Model.Machine1.Score += 100;
                    break;
                case Key.G:
                    Model.Machine1.BallsPlayed += 1;
                    break;
                case Key.H:
                    NewGame();
                    break;
                default:
                    NoKey = true;
                    break;
            }
            if (!NoKey)     //If a valid key was pressed
            {
                BallTimestamp = e.Timestamp;
                if (Model.Machine1.BallsPlayed == 9)
                {
                    GameOver(Model.Machine1.Score);
                }
                keyList.Items.Add(e.Key + " @ " + e.Timestamp);
                keyList.ScrollIntoView(keyList.Items.GetItemAt(keyList.Items.Count - 1));
            }

        }

        private void NewGame()
        {
            Model.Machine1.BallsPlayed = 0;
            Model.Machine1.Score = 0;
        }

        private void GameOver(int score)
        {
            int numHighScores = Model.HighScores.Count - 1;
            int newRank = numHighScores + 1;
            if (Model.HighScores.ElementAt(numHighScores).Value < score)  //if higher than the last item in the high score list
            {
                for (int i = 0; i < numHighScores; i++)
                {
                    if (score > Model.HighScores.ElementAt(i).Value & i < newRank)
                    {
                        newRank = i;    //this is the index of the new high score
                        break;  //exit for loop
                    }
                }
                Model.HighScores.RemoveAt(numHighScores);  //remove last score
                Score newHighScore = new Score();
                newHighScore.Name = "Player1";
                newHighScore.Value = score;
                Model.HighScores.Add(newHighScore);     //add new score at the bottom of the list
                
                //rebuild high score list from bottom up
                //for (int i = numHighScores; i >= newRank; i--)
                //{
                //    Model.HighScores.ElementAt(i).Value = Model.HighScores.ElementAt(i - 1).Value;
                //}
                Util.WriteScores(Model.HighScores, "ClassicNames", "ClassicScores");
                MessageBox.Show("New High Score");
            }
            else
            {
                MessageBox.Show("GAME OVER");
            }
            
            NewGame();

        }


    }
}
