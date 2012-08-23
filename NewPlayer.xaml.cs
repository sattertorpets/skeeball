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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewPlayer : Window
    {
        public string EnteredName { get; set; }
        private char[] nameChars = new char[38];
        private char endChar = (char)187;  // '>>'
        private Dictionary<char, int> charToInt = new Dictionary<char, int>();

        public NewPlayer()
        {
            InitializeComponent();
            
            //Create character list dictionaries
            for (int i = 0; i < 26; i++)
            {   //Add all uppercase letters
                nameChars[i] = (char)(i + 65);
            }
            for (int i = 26; i < 36; i++)
            {   //Add 0-9
                nameChars[i] = (char)(i + 22);
            }
            nameChars[36] = endChar;  //Add >>
            nameChars[37] = ' ';  //Add space

            int charIndex = 0;
            foreach (char i in nameChars)
            {
                charIndex++;
                charToInt.Add(i, charIndex);
            }
            // A-Z is 65-90, 0-9 is 48-57, 'e' is 101
        }

        private void Label_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Label thisLabel = sender as Label;
                char currentChar = thisLabel.Content.ToString()[0];
                int newIndex;
                int currentLabel = Convert.ToInt16(thisLabel.Tag as string);
                int nextLabel = currentLabel + 1;
                int prevLabel = currentLabel - 1;

                ObservableCollection<string> PlayerNames = new ObservableCollection<string>();
                PlayerNames = Util.LoadNames();

                switch (Util.KeyMap[e.Key])
                {
                    case Util.KeyName.Up:
                        newIndex = charToInt[currentChar] - 2;    //subtract 2 because Dictionary starts at 1, and array at 0
                        if (newIndex < 0) { newIndex = nameChars.Length - 1; }
                        thisLabel.Content = nameChars[newIndex];
                        break;
                    //END with font size 11 will fit
                    case Util.KeyName.Down:
                        newIndex = charToInt[currentChar];
                        if (newIndex == nameChars.Length) { newIndex = 0; }
                        thisLabel.Content = nameChars[newIndex];
                        break;
                    case Util.KeyName.Select:
                        //If in the last cell or if >> is selected, save the name.
                        if (currentLabel == LetterContainer.Children.Count || thisLabel.Content.ToString()[0].Equals(endChar))    
                        {
                            foreach (Label label in LetterContainer.Children)
                            {
                                EnteredName += label.Content.ToString();
                            }
                            EnteredName = EnteredName.Remove(currentLabel - 1);   //remove >> and trailing spaces
                            EnteredName.Trim();
                            if (PlayerNames.Contains(EnteredName, StringComparer.OrdinalIgnoreCase))  //if this name Exists give a warning, otherwise return the name
                            {
                                EnteredName = "";
                                lblInstructions.Text = "That name already exists, please enter a new name.";
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            foreach (Label label in LetterContainer.Children)
                            {
                                if (Convert.ToInt16(label.Tag as string) == nextLabel)
                                {
                                    label.Focus();
                                    break;  //exit for loop
                                }
                            }
                        }
                        break;
                    case Util.KeyName.Back:
                        if (currentLabel == 1)
                        {
                            EnteredName = "";
                            this.Close();    //Exit this screen without entering a name
                        }
                        else
                        {
                            foreach (Label label in LetterContainer.Children)
                            {
                                if (Convert.ToInt16(label.Tag as string) == prevLabel)
                                {
                                    label.Focus();
                                    break;  //exit for loop
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (KeyNotFoundException)
            {

            }
        }
    }
}
