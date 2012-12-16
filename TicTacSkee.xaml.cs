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
using System.Windows.Media.Animation;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for TicTacSkee.xaml
    /// </summary>
    public partial class TicTacSkee : Window
    {
        public BaseGame Game { get; set; }
        int[] ScoreMap = new int[51];
        public int h = 90;
        public int w = 90;

        public TicTacSkee(string gameName)
        {
            InitializeComponent();
            ScoreMap[10] = 0;
            ScoreMap[40] = 1;
            ScoreMap[50] = 2;
            ScoreMap[30] = 3;
            ScoreMap[20] = 4;
            NewGame(gameName);
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
                            if (Game.ScoringKeyHandler(e))
                            {
                                UpdateScores();    //If it was a valid scoring key
                            }
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
            //Game.ActivePlayer.BallHistory.Add(Game.ActivePlayer.ThisThrow);
            int row, col = -1;
            if (Game.ActivePlayer.BallsLeft == 1)   //First throw
            {
                Game.ActivePlayer.Throw1 = Game.ActivePlayer.ThisThrow;
            }
            if (Game.ActivePlayer.BallsLeft == 0)   //Second throw
            {

                Game.ActivePlayer.Throw2 = Game.ActivePlayer.ThisThrow;
                if (Game.ActivePlayer.Throw1 >= 10 && Game.ActivePlayer.Throw2 >= 10)
                {
                    row = ScoreMap[Game.ActivePlayer.Throw1];
                    col = ScoreMap[Game.ActivePlayer.Throw2];
                    if (Game.TTT5Grid[row, col] == 0)
                    {
                        Game.TTT5Grid[row, col] = Game.ActvPlyrNum;
                        updateImages(Game.ActvPlyrNum, row, col);
                    }
                }
                SwitchPlayers();
            }
            else
                Game.ActivePlayer.LastThrow = Game.ActivePlayer.ThisThrow;
        }

        private void NewGame(string gameName)
        {
            Game = new BaseGame(gameName);
            Game.TwoPlayer = true;
            Game.ActivePlayer.BallsLeft = 2;
            board.Children.Clear();
            lblDraw.Visibility = Visibility.Hidden;
            this.DataContext = Game;        //This tells it where to look for dependency properties
        }

        private void updateImages(int player, int row, int col)
        {
            Image img = new Image();
            Uri xUri = new Uri(@"pack://application:,,,/Skeeball;component/Images/TTT-X.png");
            Uri oUri = new Uri(@"pack://application:,,,/Skeeball;component/Images/TTT-O.png");

            if (player == 1)
                img.Source = new BitmapImage(xUri);
            else
                img.Source = new BitmapImage(oUri);

            img.Width = w;    
            img.Height = h;
            img.Name = "img" + row + col;
            Canvas.SetLeft(img, w * col);
            Canvas.SetTop(img, h * row);
            board.Children.Add(img);
        }

        private void SwitchPlayers()
        {
            if (GameIsOver())
            {
                LabelBlink("lblP" + Game.ActvPlyrNum.ToString());
                //Inform players somehow
                return;
            }
            else if (GameIsDraw(5))
            {
                lblDraw.Visibility = Visibility.Visible;
                return;
            }
            Game.OtherPlayer.Throw1 = -1;
            Game.OtherPlayer.Throw2 = -1;
            Game.OtherPlayer.BallsLeft = 2;
            Game.SwitchPlayer();
        }

        private bool GameIsDraw(int gridSize)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (Game.GameName == "TicTac3" && Game.TTT3Grid[i, j] == 0 || Game.GameName == "TicTacSkee" && Game.TTT5Grid[i, j] == 0)
                        return false;
                }
            }
            return true;
        }

        private bool CheckDiagDown(int startRow, int startCol, int[,] grid, int pNum)
        {
            return (grid[startRow, startCol] == pNum &&
                    grid[startRow + 1, startCol + 1] == pNum &&
                    grid[startRow + 2, startCol + 2] == pNum &&
                    grid[startRow + 3, startCol + 3] == pNum);
        }
        private bool CheckDiagUp(int startRow, int startCol, int[,] grid, int pNum)
        {
            return (grid[startRow, startCol] == pNum &&
                    grid[startRow - 1, startCol - 1] == pNum &&
                    grid[startRow - 2, startCol - 2] == pNum &&
                    grid[startRow - 3, startCol - 3] == pNum);
        }
        private bool CheckDown(int startRow, int startCol, int[,] grid, int pNum)
        {
            return (grid[startRow, startCol] == pNum &&
                    grid[startRow + 1, startCol] == pNum &&
                    grid[startRow + 2, startCol] == pNum &&
                    grid[startRow + 3, startCol] == pNum);
        }
        private bool CheckRight(int startRow, int startCol, int[,] grid, int pNum)
        {
            return (grid[startRow, startCol] == pNum &&
                    grid[startRow, startCol + 1] == pNum &&
                    grid[startRow, startCol + 2] == pNum &&
                    grid[startRow, startCol + 3] == pNum);
        }
        private bool CheckDiagDown(int startRow, int startCol, int[,] grid)
        {
            int dim = grid.GetLength(0);

            return true;
        }

        private bool GameIsOver()
        {
            int pNum = Game.ActvPlyrNum;
            int[,] grid = Game.TTT5Grid;
            bool win = false;
            for (int row = 0; row < 2; row++)
                for (int col = 0; col < 2; col++)
                    win |= CheckDiagDown(row, col, grid, pNum);
            for (int row = 3; row < 5; row++)
                for (int col = 3; col < 5; col++)
                    win |= CheckDiagUp(row, col, grid, pNum);
            for (int row = 0; row < 5; row++)
                for (int col = 0; col < 2; col++)
                    win |= CheckRight(row, col, grid, pNum);
            for (int row = 0; row < 2; row++)
                for (int col = 0; col < 5; col++)
                    win |= CheckDown(row, col, grid, pNum);

            return win;
        }

        private void LabelBlink(string labelName)
        {
            Storyboard sbdBlink = (Storyboard)FindResource("sbdLabelBlink");
            Storyboard.SetTargetName(sbdBlink, labelName);
            sbdBlink.Begin(this);
        }
    }
}
