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
using System.Collections.ObjectModel;

namespace SkeeBall
{
    /// <summary>
    /// Interaction logic for PlayerList.xaml
    /// </summary>

    public partial class PlayerList : Window
    {
        public ObservableCollection<string> PlayerNames { get; set; }
        public string SelectedPlayer { get; set; }

        private const string createNew = "CREATE NEW";

        public PlayerList()
        {
            InitializeComponent();
            LoadPlayerNames();
            nameList.DataContext = this;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedPlayer = ((Button)sender).Content.ToString();
            if (SelectedPlayer == createNew)
            {
                NewPlayer newPlayerWindow = new NewPlayer();
                newPlayerWindow.Owner = this;
                newPlayerWindow.ShowDialog();
                if (newPlayerWindow.EnteredName != "")
                {
                    SelectedPlayer = newPlayerWindow.EnteredName;
                    PlayerNames.Add(newPlayerWindow.EnteredName);
                }
            }

            PlayerNames.Remove(SelectedPlayer);
            PlayerNames.Insert(0, SelectedPlayer);

            SavePlayerNames();
            this.Close();

        }

        private void LoadPlayerNames()
        {
            PlayerNames = new ObservableCollection<string>(Util.LoadList<string>("players.xml"));
            PlayerNames.Add(createNew);
        }

        private void SavePlayerNames()
        {
            PlayerNames.Remove(createNew);
            Util.WriteList<string>("players.xml", PlayerNames.ToList());
        }
    }
}
