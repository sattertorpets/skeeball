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
        int Score = 0;
        int BallsPlayed = 0;
        int LastTimeStamp = 0;
        public Bindings Model { get; set; }

        public Classic2P()
        {
            InitializeComponent();
            Model = new Bindings();
            this.DataContext = Model;
            LoadHighScores();
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
                    Score = Score + 10;
                    break;
                case Key.B:
                    Score = Score + 20;
                    break;
                case Key.C:
                    Score = Score + 30;
                    break;
                case Key.D:
                    Score = Score + 40;
                    break;
                case Key.E:
                    Score = Score + 50;
                    break;
                case Key.F:
                    Score = Score + 100;
                    break;
                case Key.G:
                    BallsPlayed += 1;
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
            if (Convert.ToInt16(HighScoreText.Text) < Score)
            {
                HighScoreText.Text = String.Format("{0:000}", Score);
                MessageBox.Show("Congratulations, you have a new High Score!");
            }
            else
            {
                MessageBox.Show("GAME OVER");
            }
            NewGame();
        }
        private void LoadHighScores()
        {
            //StringCollection Game310Data = Properties.Settings.Default.Game310Names;
            StringCollection Game310Scores = Properties.Settings.Default.Game310Scores;
            StringCollection SkeeBallScores = Properties.Settings.Default.SkeeBallScores;
            StringCollection ClassicScores = Properties.Settings.Default.ClassicScores;
            StringCollection FlashBallScores = Properties.Settings.Default.FlashBallScores;

            Int32Collection Game310IntScores = new Int32Collection();
            Int32Collection SkeeBallIntScores = new Int32Collection();
            Int32Collection FlashBallIntScores = new Int32Collection();
            Int32Collection ClassicIntScores = new Int32Collection();

            foreach (String score in Game310Scores)
	        {
               Game310IntScores.Add(Int32.Parse(score));
	        }
            //put into key/value pair

            List<List<string>> HSdata = new List<List<string>>();

            
                
          
        }

    }
}
