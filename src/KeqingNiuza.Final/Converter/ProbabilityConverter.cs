using System;
using System.Globalization;
using System.Windows.Data;
using KeqingNiuza.Core.Wish;

namespace KeqingNiuza.Converter
{
    public class ProbabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WishStatistics statistic))
            {
                return "NAN";
            }
            double probability = 0;
            if (statistic.WishType == WishType.CharacterEvent || statistic.WishType == WishType.Permanent)
            {
                if (statistic.Guarantee <= 72 || statistic.Guarantee == 90)
                {
                    probability = 0.006;
                }
                else
                {
                    probability = 0.006 + 0.06 * (statistic.Guarantee - 72);
                }
            }
            if (statistic.WishType == WishType.WeaponEvent)
            {
                if (statistic.Guarantee <= 61 || statistic.Guarantee == 80)
                {
                    probability = 0.007;
                }
                else if (statistic.Guarantee >= 62 || statistic.Guarantee <= 72)
                {
                    probability = 0.007 + 0.07 * (statistic.Guarantee - 62);
                }
                else
                {
                    probability = 0.777 + 0.035 * (statistic.Guarantee - 72);
                }
            }
            if (probability < 1)
            {
                return probability.ToString("P3");
            }
            else
            {
                return "100%";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
