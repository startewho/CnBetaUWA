//-----------------------------------------------------------------------
// <copyright file="ImageGenerator.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------



using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using MyToolkit.Html;
using ImageLib.Controls;


namespace MyToolkit.Controls.Html.Generators
{
    /// <summary>Generates the UI element for an image (img) HTML tag.</summary>
    public class CacheImageGenerator : SingleControlGenerator
    {
        /// <summary>Creates a single UI element for the given HTML node and HTML view.</summary>
        /// <param name="node">The node.</param>
        /// <param name="htmlView">The text block.</param>
        /// <returns>The UI element.</returns>
        public override DependencyObject CreateControl(HtmlNode node, IHtmlView htmlView)
        {
            try
            {
                var imageUri = node.Attributes["src"];

                var height = 0;
                if (node.Attributes.ContainsKey("height"))
                    int.TryParse(node.Attributes["height"], out height);

                var width = 0;
                if (node.Attributes.ContainsKey("width"))
                    int.TryParse(node.Attributes["width"], out width);

                if (height == 1 && width == 1)
                    return null;

                var image = new ImageView {UriSource = new Uri(imageUri)};

                
                var imageBlock = new ImageBlock
                {
                  
                    UserHeight = height,
                    UserWidth = width,
                    Image = image,
                };

               image.LoadingCompleted += delegate { imageBlock.Update(htmlView.ActualWidth); };

                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Margin = new Thickness(0, htmlView.ParagraphMargin, 0, htmlView.ParagraphMargin);

                if (width > 0)
                    image.Width = width;
                if (height > 0)
                    image.Height = height;

                htmlView.SizeDependentControls.Add(imageBlock);
                return new ContentPresenter { Content = image };
            }
            catch
            {
                return null;
            }
        }

        internal class ImageBlock : ISizeDependentControl
        {
            public ImageView Image { get; set; }

            public int UserWidth { get; set; }

            public int UserHeight { get; set; }

        
            public void Update(double actualWidth)
            {
                 Image.Width = actualWidth;
                    //if (Image.Width < width)
                    //    width = Source.PixelWidth;

                    //if (UserWidth < width && UserWidth != 0)
                    //    width = UserWidth;

                    //Image.Width = width;
                    //Image.Height = Source.PixelHeight * width / Source.PixelWidth;
                }
            }
        }
    
}

