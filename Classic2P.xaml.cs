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
        int LastTimeStamp = 0;
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
            LastTimeStamp = e.Timestamp;
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
                scoreBox.Text = String.Format("{0:000}", Score);
                ballBox.Text = Convert.ToString(BallsPlayed);
                if (BallsPlayed == 9)
                {
                    GameOver();
                    GameMenu wdwSelectGame = new GameMenu();
                    wdwSelectGame.Owner = Window.GetWindow(this);
                    wdwSelectGame.Height *= 1.5;
                    wdwSelectGame.Width *= 1.5;
                    wdwSelectGame.ShowDialog();
                    wdwSelectGame.HorizontalAlignment = HorizontalAlignment.Center;
                    wdwSelectGame.VerticalAlignment = VerticalAlignment.Center;

                }
                keyList.Items.Add(e.Key + " @ " + e.Timestamp);
                keyList.ScrollIntoView(keyList.Items.GetItemAt(keyList.Items.Count - 1));
            }

        }

        private void NewGame()
        {
            BallsPlayed = 0;
            Score = 0;
            scoreBox.Text = String.Format("{0:000}", Score);
            ballBox.Text = Convert.ToString(BallsPlayed);
        }

        private void GameOver()
        {
            if (Model.HighestScore < Score)  //if new high score list entry
            {
                Model.HighScores.RemoveAt(Model.HighScores.Count - 1);
                Model.HighScores.Add(Score);
            }
            else
            {
                MessageBox.Show("GAME OVER");
            }
            Util.WriteScores(Model.HighScores, "ClassicNames", "ClassicScores");
            NewGame();

        }


    }
}
