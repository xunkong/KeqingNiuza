using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace KeqingNiuza.Converter
{
    public class IntToNoteStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var i = (int)value;
            if (i > -40 && i < 49)
            {
                return IntToNoteString[i];
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static readonly Dictionary<int, string> IntToNoteString = new Dictionary<int, string>
        {
            {-39,"A0"},
            {-38,"A♯0"},
            {-37,"B0"},
            {-36,"C1"},
            {-35,"C♯1"},
            {-34,"D1"},
            {-33,"D♯1"},
            {-32,"E1"},
            {-31,"F1"},
            {-30,"F♯1"},
            {-29,"G1"},
            {-28,"G♯1"},
            {-27,"A1"},
            {-26,"A♯1"},
            {-25,"B1"},
            {-24,"C2"},
            {-23,"C♯2"},
            {-22,"D2"},
            {-21,"D♯2"},
            {-20,"E2"},
            {-19,"F2"},
            {-18,"F♯2"},
            {-17,"G2"},
            {-16,"G♯2"},
            {-15,"A2"},
            {-14,"A♯2"},
            {-13,"B2"},
            {-12,"C3"},
            {-11,"C♯3"},
            {-10,"D3"},
            {-9,"D♯3"},
            {-8,"E3"},
            {-7,"F3"},
            {-6,"F♯3"},
            {-5,"G3"},
            {-4,"G♯3"},
            {-3,"A3"},
            {-2,"A♯3"},
            {-1,"B3"},
            {0,"C4"},
            {1,"C♯4"},
            {2,"D4"},
            {3,"D♯4"},
            {4,"E4"},
            {5,"F4"},
            {6,"F♯4"},
            {7,"G4"},
            {8,"G♯4"},
            {9,"A4"},
            {10,"A♯4"},
            {11,"B4"},
            {12,"C5"},
            {13,"C♯5"},
            {14,"D5"},
            {15,"D♯5"},
            {16,"E5"},
            {17,"F5"},
            {18,"F♯5"},
            {19,"G5"},
            {20,"G♯5"},
            {21,"A5"},
            {22,"A♯5"},
            {23,"B5"},
            {24,"C6"},
            {25,"C♯6"},
            {26,"D6"},
            {27,"D♯6"},
            {28,"E6"},
            {29,"F6"},
            {30,"F♯6"},
            {31,"G6"},
            {32,"G♯6"},
            {33,"A6"},
            {34,"A♯6"},
            {35,"B6"},
            {36,"C7"},
            {37,"C♯7"},
            {38,"D7"},
            {39,"D♯7"},
            {40,"E7"},
            {41,"F7"},
            {42,"F♯7"},
            {43,"G7"},
            {44,"G♯7"},
            {45,"A7"},
            {46,"A♯7"},
            {47,"B7"},
            {48,"C8"},
        };
    }
}
