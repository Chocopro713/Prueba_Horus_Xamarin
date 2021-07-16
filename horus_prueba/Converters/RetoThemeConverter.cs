namespace horus_prueba.Converters
{
	using System;
	using System.Globalization;
	using Xamarin.Forms;

	public class RetoThemeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool valor = (bool)value;
			return valor ? "Themes/CompleteTheme" : "Themes/IncompleteTheme";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Empty;
		}
	}
}
