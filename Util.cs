using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SkeeBall.Models;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows;
using System.Xml.Serialization;
using System.IO;

namespace SkeeBall
{
    public static class Util
    {
        public enum KeyName { Hole10, Hole20, Hole30, Hole40, Hole50, HoleL100, HoleR100, Gutter, Up, Down, Select, Back };
        public static Dictionary<Key, KeyName> KeyMap = new Dictionary<System.Windows.Input.Key, KeyName>()
        {
            {Key.A, KeyName.Hole10},
            {Key.S, KeyName.Hole20},
            {Key.Q, KeyName.Hole30},
            {Key.W, KeyName.Hole40},
            {Key.I, KeyName.Hole50},
            {Key.K, KeyName.HoleL100},
            {Key.J, KeyName.HoleR100},
            {Key.Space, KeyName.Select},
            {Key.V, KeyName.Back},
            {Key.L, KeyName.Gutter},
            {Key.Up, KeyName.Up},
            {Key.Down, KeyName.Down}
        };
        /* 
        The letters are the pins on the Keywiz where everything is wired
         * A = 10, B = 20, C = 30, D = 40, E = 50, F = L100, G = R100, L = Gutter, K = BackButton
         * The 3 keys below are wired to I, J, H, but send Up, Down, and Space keypresses.
         * I = UpArrow, J = DownArrow, H = Select = SpaceBar
        */

        public static List<T> LoadList<T>(string path)
        {
            List<T> result;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            try
            {
                using (FileStream fs = File.OpenRead(path))
                {

                    result = serializer.Deserialize(fs) as List<T>;
                }
            }
            catch (FileNotFoundException)
            {
                result = new List<T>();
            }

            return result;
        }

        public static void WriteList<T>(string path, List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(list.GetType());

            using (FileStream fs = File.OpenWrite(path))
            {
                serializer.Serialize(fs, list);
            }
        }
    }

    public static class ListExtension
    {
        public static void ReverseBubbleSort(this IList o)  //High scores are better
        {
            for (int i = o.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object o1 = o[j - 1];
                    object o2 = o[j];
                    if (((IComparable)o2).CompareTo(o1) > 0)
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);
                    }
                }
            }
        }
        public static void ReverseBubbleGolfSort(this IList o)  //Low scores are better
        {
            for (int i = o.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object o1 = o[j - 1];
                    object o2 = o[j];
                    if (((IComparable)o2).CompareTo(o1) < 0)    //If the bottom value is less than the top value
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);
                    }
                }
            }
        }
    }

    public class IntToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static IntToVisibilityConverter _converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;
            if ((int)value == 0)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value; // convert and return something (if needed)
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new IntToVisibilityConverter();
            return _converter;
        }
        // usage: <TextBlock Visibility="{Binding SomeInt, Converter={src:IntToVisibilityConverter}}" />
    }

    public class BoolToGridConverter : MarkupExtension, IValueConverter
    {
        private static BoolToGridConverter _converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "0*";
            if ((bool)value == true)
                return "1*";
            else
                return "0*";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value; // convert and return something (if needed)
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new BoolToGridConverter();
            return _converter;
        }
    }

    public class InvBoolToGridConverter : MarkupExtension, IValueConverter
    {
        private static InvBoolToGridConverter _converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "0*";
            if ((bool)value == true)
                return "0*";
            else
                return "1*";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value; // convert and return something (if needed)
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new InvBoolToGridConverter();
            return _converter;
        }
    }
}
