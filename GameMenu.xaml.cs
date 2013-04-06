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
using System.Collections.ObjectModel;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class GameMenu : Window
    {

        public GameMenu()
        {
            InitializeComponent();
            
            LEDWiz ledwiz = new LEDWiz(IntPtr.Zero);
            ledwiz.StartupLighting();
            ledwiz.SBA(255,255,255, 128 | 32 | 16,2);
            byte[] brightness = { 48, 48, 48, 129, 48, 48, 48, 48, 48, 48, 48, 48, 48, 20, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48 }; ;
            for (int i = 0; i < 32; i++)
			{
                if (i > 23)
                    brightness[i] = 49;
                else
                    brightness[i] = 129;
			}
            ledwiz.PBA(brightness);
            //maybe set an attract mode for lighting here
            //should set the button lighting
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = null;
            try 
	        {	        
		        Button test = (Button)sender;  //need to cast this to button to get its name or content for the switch
                
                switch (test.Content.ToString())
                {
                    case "Classic":
                        game = new SoloGameWindow("Classic");
                        break;
                    case "Skee Ball":
                        game = new SoloGameWindow("Modern");
                        break;
                    case "Multi Ball":
                    case "310":
                    case "510":
                    case "Cricket":
                        //game = new CricketWindow("Cricket");
                        break;
                    case "Tic Tac Three":
                    case "Tic Tac Skee":
                    default:
                        game = new SoloGameWindow("Classic");
                        break;
                }

                game.Owner = this;
                game.ShowDialog();
	        }
	        catch (Exception)
	        {
                System.Windows.Forms.MessageBox.Show("Something that wasn't a button called the btn_Click function");
		        throw;
	        }
        }

        private void btn_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Classic Skeeball, 9 balls, no 100s.  In all menus, blue button is Select/New Game and red button is Back";
        }

        private void btnClassic_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Classic");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnClassic_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Classic Skeeball, 9 balls, no 100s.  In all menus, blue button is Select/New Game and red button is Back";
        }

        private void btnCricket_Click(object sender, RoutedEventArgs e)
        {
            CricketWindow game = new CricketWindow("Cricket");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnCricket_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Solo or Multiplayer Cricket, similar to darts.  In game, press up to add a player, down to remove a player.  Hit 2 50s in a row to get a 100.";
        }

        private void btnModern_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Modern");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnModern_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Modern Skeeball, 9 balls, with 100s.";
        }

        private void btn310_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Get to exactly 310 in the fewest throws possible.";
        }

        private void btnMultiBall_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Build up a multiplier for huge scores.  In game, hit the multiplier target to increase your multiplier.  Multiplier resets on throws less than 30.";
        }

        private void btn310_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("ThreeTen");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnMultiBall_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("Multi");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btn510_Click(object sender, RoutedEventArgs e)
        {
            SoloGameWindow game = new SoloGameWindow("FiveTen");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btn510_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Get to exactly 510 in the fewest throws possible.";
        }

        private void btnTicTac3_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "1st throw determines the row, 2nd throw determines the column.  Only 30, 40 and 50 score.  (2P Only)";
        }

        private void btnTicTac3_Click(object sender, RoutedEventArgs e)
        {
            TicTac3 game = new TicTac3("TicTac3");
            game.Owner = this;
            game.ShowDialog();
        }

        private void btnTicTacSkee_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGameDescript.Text = "Get 4 in a row to win.  1st throw determines the row, 2nd throw determines the column.  10 through 50 scores.  (2P Only)";
        }

        private void btnTicTacSkee_Click(object sender, RoutedEventArgs e)
        {
            TicTacSkee game = new TicTacSkee("TicTacSkee");
            game.Owner = this;
            game.ShowDialog();
        }

    }
}
