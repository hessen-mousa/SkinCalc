using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace SkinCalc.ViewLogic
{
    public class ByteArrayToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is byte[]))
            {
                return null; // Handle null or non-byte array input
            }

            byte[] imageData = (byte[])value;

            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Improve performance
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                // Handle potential exceptions during image loading (optional)
                return null; // Or provide a default image
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not implemented for this converter.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

}
