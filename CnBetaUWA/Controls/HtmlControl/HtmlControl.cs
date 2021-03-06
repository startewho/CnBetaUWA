﻿using System;
using System.Collections.Generic;
using Windows.UI;
using MyToolkit.Controls.Html;
using MyToolkit.Utilities;
using MyToolkit.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;


namespace MyToolkit.Controls
{
    public class HtmlControl : ScrollableItemsControl, IHtmlView
    {
        private bool _isLoaded = false;

        private ContentPresenter _headerPresenter;
        private ContentPresenter _footerPresenter;

        private Action<object, object> Update;
        private readonly IDictionary<string, IControlGenerator> _generators;

        /// <summary>Initializes a new instance of the <see cref="ScrollableHtmlView"/> class. </summary>
        public HtmlControl()
        {
            _generators = HtmlControlHelper.GetDefaultGenerators(this);

            Update = UpdateFrameWork;

            InnerMargin = new Thickness(12, 0, 12, 0);

            FontSize = (double)Resources["ControlContentThemeFontSize"];
            Foreground = (Brush)Resources["ApplicationForegroundThemeBrush"];
            Margin = new Thickness(0);

            SizeChanged += OnSizeChanged;
            SizeDependentControls = new List<ISizeDependentControl>();
            Unloaded += HtmlControl_Unloaded;
            DependencyPropertyChangedEvent.Register(this, FontSizeProperty, Update);
            DependencyPropertyChangedEvent.Register(this, FontFamilyProperty, Update);
            DependencyPropertyChangedEvent.Register(this, ForegroundProperty, Update);
            DependencyPropertyChangedEvent.Register(this, BackgroundProperty, Update);
            DependencyPropertyChangedEvent.Register(this, InnerMaxWidthProperty, Update);
        }


        private void UpdateFrameWork(object o1, object o2)
        {
            this.Generate();
        }
        private void HtmlControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DependencyPropertyChangedEvent.Deregister(this, FontSizeProperty, Update);
            DependencyPropertyChangedEvent.Deregister(this, FontFamilyProperty, Update);
            DependencyPropertyChangedEvent.Deregister(this, ForegroundProperty, Update);
            DependencyPropertyChangedEvent.Deregister(this, BackgroundProperty, Update);
            DependencyPropertyChangedEvent.Deregister(this, InnerMaxWidthProperty, Update);
        }

        /// <summary>Gets the list of HTML element generators. </summary>
        public IDictionary<string, IControlGenerator> Generators
        {
            get { return _generators; }
        }

        /// <summary>Gets the list of size dependent controls. </summary>
        public List<ISizeDependentControl> SizeDependentControls { get; private set; }

        /// <summary>Occurs when the HTML content has been loaded. </summary>
        public event EventHandler<EventArgs> HtmlLoaded;

        public static readonly DependencyProperty HtmlProperty =
        DependencyProperty.Register("Html", typeof(string), typeof(HtmlControl), new PropertyMetadata(default(string),
        (obj, e) => ((HtmlControl)obj).Generate()));

        /// <summary>Gets or sets the HTML content to display. </summary>
        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }


        public static readonly DependencyProperty ParagraphMarginProperty =
        DependencyProperty.Register("ParagraphMargin", typeof(int), typeof(HtmlControl), new PropertyMetadata(6));

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(HtmlControl), new PropertyMetadata(null,BackgroundColorChanged));

        private static void BackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlControl = d as HtmlControl;
            var backgroundcolor =(Color)e.NewValue;
            if (backgroundcolor == default(Color)) return;
            if (htmlControl != null) htmlControl.Background = new SolidColorBrush(backgroundcolor);
        }


        /// <summary>Gets or sets the margin for paragraphs (added at the bottom of the element). </summary>
        public int ParagraphMargin
        {
            get { return (int)GetValue(ParagraphMarginProperty); }
            set { SetValue(ParagraphMarginProperty, value); }
        }

        public static readonly DependencyProperty HtmlBaseUriProperty =
        DependencyProperty.Register("HtmlBaseUri", typeof(Uri), typeof(HtmlControl), new PropertyMetadata(default(Uri)));

        /// <summary>Gets or sets the base URI which is used to resolve relative URIs. </summary>
        public Uri HtmlBaseUri
        {
            get { return (Uri)GetValue(HtmlBaseUriProperty); }
            set { SetValue(HtmlBaseUriProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HtmlControl), new PropertyMetadata(default(DataTemplate),
        (d, e) => ((HtmlControl)d).UpdateHeader()));

        /// <summary>Gets or sets the header template. </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty ShowHeaderProperty =
        DependencyProperty.Register("ShowHeader", typeof(bool), typeof(HtmlControl),
        new PropertyMetadata(true, (d, e) => ((HtmlControl)d).UpdateShowHeader()));

        /// <summary>Gets or sets a value indicating whether the header should be shown. </summary>
        public bool ShowHeader
        {
            get { return (bool)GetValue(ShowHeaderProperty); }
            set { SetValue(ShowHeaderProperty, value); }
        }

        public static readonly DependencyProperty FooterTemplateProperty =
        DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(HtmlControl),
        new PropertyMetadata(default(DataTemplate), (d, e) => ((HtmlControl)d).UpdateFooter()));

        /// <summary>Gets or sets the footer template. </summary>
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty InnerMaxWidthProperty = DependencyProperty.Register(
        "InnerMaxWidth", typeof(double), typeof(HtmlControl), new PropertyMetadata(default(double)));

        /// <summary>Gets or sets the maximum width of the elements in the scroll viewer.</summary>
        public double InnerMaxWidth
        {
            get { return (double)GetValue(InnerMaxWidthProperty); }
            set { SetValue(InnerMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ShowFooterProperty =
        DependencyProperty.Register("ShowFooter", typeof(bool), typeof(HtmlControl),
        new PropertyMetadata(true, (d, e) => ((HtmlControl)d).UpdateShowFooter()));

        /// <summary>Gets or sets a value indicating whether the footer should be shown. </summary>
        public bool ShowFooter
        {
            get { return (bool)GetValue(ShowFooterProperty); }
            set { SetValue(ShowFooterProperty, value); }
        }

        /// <summary>Gets the generator for the tag name or creates a new one.</summary>
        /// <typeparam name="TGenerator">The type of the generator.</typeparam>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns>The generator.</returns>
        public TGenerator GetGenerator<TGenerator>(string tagName)

        where TGenerator : IControlGenerator, new()
        {
            if (Generators.ContainsKey(tagName))
                return (TGenerator)Generators[tagName];
            Generators[tagName] = new TGenerator();
            return (TGenerator)Generators[tagName];
        }


        /// <summary>Attaches a binding to a FrameworkElement, using the provided binding object.</summary>
        protected override void OnApplyTemplate()

        {
            base.OnApplyTemplate();

            _isLoaded = true;

            UpdateHeader();
            UpdateFooter();
        }

        internal void UpdateHeader()
        {
            if (!_isLoaded)
                return;

            if (HeaderTemplate == null)
            {
                if (_headerPresenter != null)
                {
                    if (Items != null && Items.Contains(_headerPresenter))
                        Items.Remove(_headerPresenter);

                    _headerPresenter = null;
                }
            }
            else
            {
                if (_headerPresenter == null)
                {
                    _headerPresenter = new ContentPresenter();
                    _headerPresenter.Content = this.FindParentDataContext();
                    _headerPresenter.Visibility = ShowHeader ? Visibility.Visible : Visibility.Collapsed;
                }

                if (Items != null && !Items.Contains(_headerPresenter))
                    Items.Insert(0, _headerPresenter);

                _headerPresenter.ContentTemplate = HeaderTemplate;
            }
        }

        internal void UpdateFooter()
        {
            if (!_isLoaded)
                return;

            if (FooterTemplate == null)
            {
                if (_footerPresenter != null)
                {
                    if (Items != null && Items.Contains(_footerPresenter))
                        Items.Remove(_footerPresenter);
                    _footerPresenter = null;
                }
            }
            else
            {
                if (_footerPresenter == null)
                {
                    _footerPresenter = new ContentPresenter();
                    _footerPresenter.Content = this.FindParentDataContext();
                    _footerPresenter.Visibility = ShowFooter ? Visibility.Visible : Visibility.Collapsed;
                }

                if (Items != null && !Items.Contains(_footerPresenter))
                    Items.Add(_footerPresenter);

                _footerPresenter.ContentTemplate = FooterTemplate;
            }
        }

        private void UpdateShowHeader()
        {
            if (_headerPresenter != null)
                _headerPresenter.Visibility = ShowHeader && HeaderTemplate != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateShowFooter()
        {
            if (_footerPresenter != null)
                _footerPresenter.Visibility = ShowFooter && FooterTemplate != null ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void OnHtmlLoaded()
        {
            var copy = HtmlLoaded;
            if (copy != null)
                copy(this, new EventArgs());
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            foreach (var control in SizeDependentControls)
                control.Update(ActualWidth);
        }
    }
}
