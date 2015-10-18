using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


namespace CnBetaUWA.Controls
{
    [TemplatePart(Name = "BottomTicksCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "RightTicksCanvas", Type = typeof(Canvas))]
    public class LabelSlider : Slider
    {
        public double TickFrequency { get; set; }



        public DataTemplate TickTemplate
        {
            get { return (DataTemplate)GetValue(TickTemplateProperty); }
            set { SetValue(TickTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickTemplateProperty =
            DependencyProperty.Register("TickTemplate", typeof(DataTemplate), typeof(LabelSlider), null);


        public LabelSlider()
        {
            DefaultStyleKey = typeof(LabelSlider);
            SizeChanged += new SizeChangedEventHandler(LabelSlider_SizeChanged);
           //var text= ThumbToolTipValueConverter.Convert(Value, null, null, language: null);
        }

        private  Canvas bottomTicksCanvas;
        private  Canvas rightTicksCanvas;

        void LabelSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bottomTicksCanvas.Children.Clear();
            rightTicksCanvas.Children.Clear();
            var numberOfTicks = (int) ((Maximum - Minimum)/TickFrequency);
            //numberOfTicks++;
            for (var i = 0; i <= numberOfTicks; i++)
            {
                double x1 = 5 + ((i)*((this.ActualWidth - 10)/numberOfTicks));
                double y1 = 5 + ((i)*((this.ActualHeight - 10)/numberOfTicks));

                bottomTicksCanvas.Children.Add(CreateTick(new Point(x1, 0), new Point(x1, 5)));
                rightTicksCanvas.Children.Add(CreateTick(new Point(0, y1), new Point(5, y1)));
                bottomTicksCanvas.Children.Add(
                    CreateLabel((Math.Round(((Maximum - Minimum)/numberOfTicks)*(i+1), 0)).ToString(), 6.0, x1));
                rightTicksCanvas.Children.Add(
                    CreateLabel((Math.Round(((Maximum - Minimum)/numberOfTicks)*(i+1), 0)).ToString(), y1, 6.0));
            }

            switch (Orientation)
            {
                case Orientation.Vertical:
                    rightTicksCanvas.Visibility = Visibility.Visible;
                    break;
                case Orientation.Horizontal:
                    bottomTicksCanvas.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private TextBlock CreateLabel(string text, double top, double left)
        {
            TextBlock txt = new TextBlock();
            txt.Text = text;
            txt.FontSize = 12*Convert.ToInt32(text);
            //txt.SetValue(Canvas.TopProperty, top);
            //txt.SetValue(Canvas.LeftProperty, left - txt.ActualWidth / 2);
            txt.SetValue(Canvas.TopProperty, top - txt.ActualHeight/2);
            txt.SetValue(Canvas.LeftProperty, left);
            return txt;
        }

        private FrameworkElement CreateTick(Point start, Point end)
        {
            if (TickTemplate == null)
            {
                var ln = new Line
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1.0,
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = end.X,
                    Y2 = end.Y
                };
                return ln;
            }

            var cp = new ContentPresenter
            {
                Content = "a",
                ContentTemplate = TickTemplate
            };
            cp.SetValue(Canvas.TopProperty, start.Y);
            cp.SetValue(Canvas.LeftProperty, start.X - (cp.ActualWidth/2));
            return cp;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            bottomTicksCanvas = GetTemplateChild("BottomTicksCanvas") as Canvas;
            rightTicksCanvas = GetTemplateChild("RightTicksCanvas") as Canvas;
        }
    }
}

