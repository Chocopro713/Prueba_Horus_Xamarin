using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace horus_prueba
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        /*
        static readonly Assembly CurrentAssembly =
          typeof(ImageResourceExtension).GetType().Assembly;

        public const string Assets = nameof(Assets);

        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Source))
                return null;

            // Do your translation lookup here, using whatever method you require            
            var source = $"{CurrentAssembly.GetName().Name}.{Assets}.{Source}";

            var imageSource = ImageSource.FromResource(source, CurrentAssembly);

            return imageSource;
        }
        */

        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
