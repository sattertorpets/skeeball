using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SkeeBall.Models;
using System.Collections.Specialized;

namespace SkeeBall
{
    class Util
    {
        public static ObservableCollection<Score> LoadScores(string names, string scores)
        {
            ObservableCollection<Score> highScores = new ObservableCollection<Score>();
            StringCollection namesCollection = (StringCollection)Properties.Settings.Default[names];
            StringCollection scoresCollection = (StringCollection)Properties.Settings.Default[scores];
            for (int i = 0; i < namesCollection.Count; i++)
            {
                Score score = new Score();
                score.Name = namesCollection[i];
                score.Value = int.Parse(scoresCollection[i]);
                highScores.Add(score);
            }
            return highScores;
        }

        public static void WriteScores(ObservableCollection<Score> highScores, string names, string scores)
        {
            Properties.Settings.Default[names] = new StringCollection();
            Properties.Settings.Default[scores] = new StringCollection();
            foreach (Score score in highScores)
            {
                ((StringCollection)Properties.Settings.Default[names]).Add(score.Name);
                ((StringCollection)Properties.Settings.Default[scores]).Add(score.Value.ToString());
            }
            Properties.Settings.Default.Save();
        }


    }
}
