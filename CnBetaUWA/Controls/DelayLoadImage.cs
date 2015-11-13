using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Q42.WinRT.Data;

namespace CnBetaUWA.Controls
{

        [TemplatePart(Name = PART_DEFAULTIMAGE_NAME, Type = typeof(Image))]
        [TemplatePart(Name = PART_ACTUALIMAGE_NAME, Type = typeof(Image))]
        [TemplateVisualState(Name = STATE_DEFAULT_NAME, GroupName = GROUP_COMMONSTATES_NAME)]
        [TemplateVisualState(Name = STATE_ACTUAL_NAME, GroupName = GROUP_COMMONSTATES_NAME)]
        public class DelayLoadImage : Windows.UI.Xaml.Controls.Control
        {
            private const string PART_DEFAULTIMAGE_NAME = "PART_DefaultImage";
            private const string PART_ACTUALIMAGE_NAME = "PART_ActualImage";

            private const string STATE_DEFAULT_NAME = "STATE_Default";
            private const string STATE_ACTUAL_NAME = "STATE_Actual";

            private const string GROUP_COMMONSTATES_NAME = "CommonStates";

            private BitmapImage _image;

            static DelayLoadImage()
            {
                StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(DelayLoadImage), new PropertyMetadata(default(Stretch),StretchChanged));
                DefaultImageSourceProperty = DependencyProperty.Register("DefaultImageSource", typeof(ImageSource), typeof(DelayLoadImage), new PropertyMetadata(default(ImageSource),DefaultImageSourceChanged));
                ActualImageSourceProperty = DependencyProperty.Register("ActualImageSource", typeof(string), typeof(DelayLoadImage), new PropertyMetadata(default(string),ActualImageSourceChanged));
            }

            public DelayLoadImage()
            {
                DefaultStyleKey = typeof(DelayLoadImage);

                _image = new BitmapImage {CreateOptions = BitmapCreateOptions.None};
                _image.ImageOpened += image_ImageOpened;
            }

            private bool imageLoaded;

            public void image_ImageOpened(object sender, RoutedEventArgs e)
            {
                imageLoaded = true;

                if (actualImage != null)
                {
                    actualImage.Source = _image;
                    VisualStateManager.GoToState(this, STATE_ACTUAL_NAME, false);
                }
            }

            public static readonly DependencyProperty StretchProperty;

            public Stretch Stretch
            {
                get { return (Stretch)GetValue(StretchProperty); }
                set { SetValue(StretchProperty, value); }
            }

            private static void StretchChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                DelayLoadImage instance = o as DelayLoadImage;
                if (instance != null)
                {
                    if (instance.defaultImage != null)
                        instance.defaultImage.Stretch = (Stretch)e.NewValue;
                    if (instance.actualImage != null)
                        instance.actualImage.Stretch = (Stretch)e.NewValue;
                }
            }

            public static readonly DependencyProperty DefaultImageSourceProperty;

            public ImageSource DefaultImageSource
            {
                get { return (ImageSource)GetValue(DefaultImageSourceProperty); }
                set { SetValue(DefaultImageSourceProperty, value); }
            }

            //private static ImageSourceConverter converter = new ImageSourceConverter();

            private static void DefaultImageSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                DelayLoadImage instance = o as DelayLoadImage;
                if (instance?.defaultImage != null)
                {
                    instance.defaultImage.Source = (ImageSource)e.NewValue;
                }
            }

            public static readonly DependencyProperty ActualImageSourceProperty;

            public string ActualImageSource
            {
                get { return (string)GetValue(ActualImageSourceProperty); }
                set { SetValue(ActualImageSourceProperty, value); }
            }

            private async static void ActualImageSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                DelayLoadImage instance = o as DelayLoadImage;

                if (instance == null) return;
                instance.imageLoaded = false;

                var newCacheUri = e.NewValue;
                if (newCacheUri == null) return;

                var cacheUri =await WebDataCache.GetLocalUriAsync(new Uri(newCacheUri.ToString(), UriKind.RelativeOrAbsolute));
                instance._image.UriSource = cacheUri;
                VisualStateManager.GoToState(instance, STATE_DEFAULT_NAME, false);


                //这里引入Q42的缓存
            }

            internal Image actualImage;
            internal Image defaultImage;

            protected override void OnApplyTemplate()
            {
                actualImage = (Image)GetTemplateChild(PART_ACTUALIMAGE_NAME);
                defaultImage = (Image)GetTemplateChild(PART_DEFAULTIMAGE_NAME);

                defaultImage.Source = DefaultImageSource;
                defaultImage.Stretch = Stretch;

                actualImage.Source = _image;
                actualImage.Stretch = Stretch;

                if (imageLoaded)
                {
                    VisualStateManager.GoToState(this, STATE_ACTUAL_NAME, false);
                }

                base.OnApplyTemplate();
            }


        private static void SetSourceOnObject(object imgControl, ImageSource imageSource, bool throwEx = true)
        {

            try
            {
                if (imgControl is Image)
                {
                    ((Image)imgControl).Source = imageSource;
                }
                else
                {
                    if (imgControl is ImageBrush)
                    {
                        ((ImageBrush)imgControl).ImageSource = imageSource;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (throwEx)
                {
                    throw ex;
                }
            }

        }

    }



}
