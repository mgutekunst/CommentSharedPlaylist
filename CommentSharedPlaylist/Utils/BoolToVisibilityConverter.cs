using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CommentSharedPlaylist.Utils
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        private static BoolToVisibilityConverter _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new BoolToVisibilityConverter();
            }
            return _converter;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doInvert = System.Convert.ToBoolean(parameter ?? false);
            var isTrue = System.Convert.ToBoolean(value);
            return ((isTrue && !doInvert) || (!isTrue && doInvert)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var retValue = ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
            if (parameter != null)
            {
                if (System.Convert.ToBoolean(parameter ?? false))
                {
                    retValue = !retValue;
                }
            }
            return retValue;
        }

        #region IMultiValueConverter implementation
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool inputValue = true;
            foreach(object o in values)
            {
                var isTrue = System.Convert.ToBoolean(o);
                inputValue &= isTrue;
            }

            return Convert(inputValue, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
