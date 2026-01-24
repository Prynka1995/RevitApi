using Task10.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Task10.ViewModels
{
    internal class TreeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeType type)
            {
                switch (type)
                {
                    case TreeType.FirTree:
                        return "Ель";
                    case TreeType.OrangeTree:
                        return "Апельсиновое дерево";
                    case TreeType.Willowtree:
                        return "Ива";
                    default:
                        return value != null ? value.ToString() : string.Empty;
                }
            }

            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                switch (str)
                {
                    case "Ель":
                        return TreeType.FirTree;
                    case "Стул":
                        return TreeType.OrangeTree;
                    case "Шкаф":
                        return TreeType.Willowtree;
                    default:
                        throw new ArgumentException(string.Format("Неизвестный тип деревьев: {0}", str));
                }
            }

            throw new ArgumentException("Некорректное значение для конвертации");
        }
    }
}
