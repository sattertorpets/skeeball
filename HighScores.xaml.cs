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

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for HighScores.xaml
    /// </summary>
    public partial class HighScoresWndw : Window
    {
        public bool MakeNewGame { get; set; }
        
        public HighScoresWndw()
        {
            InitializeComponent();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                switch (Util.KeyMap[e.Key])
                {
                    case Util.KeyName.Select: //New Game
                        MakeNewGame = true;
                        this.Close();
                        return;
                    case Util.KeyName.Back: //Exit the game
                        MakeNewGame = false;
                        this.Close();
                        return;
                    case Util.KeyName.Up:  //Global high scores (the default)
                        lstHighScores.SetBinding(ListView.ItemsSourceProperty, new Binding("HighScores"));
                        lblHeader.Content = "Global High Scores";
                        //turn on light for down, turn off light for up
                        break;
                    case Util.KeyName.Down: //Personal high scores
                        lstHighScores.SetBinding(ListView.ItemsSourceProperty, new Binding("PersonalHighScores"));
                        lblHeader.Content = "Player's High Scores";
                        //turn on light for up, turn off light for down
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
    }
}
