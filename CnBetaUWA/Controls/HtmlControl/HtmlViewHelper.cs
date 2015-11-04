//-----------------------------------------------------------------------
// <copyright file="HtmlTextBlockHelper.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using MyToolkit.Controls;
using MyToolkit.Controls.Html;
using MyToolkit.Controls.Html.Generators;
using HtmlToXaml;
namespace MyToolkit.Controls.Html
{
    /// <summary>Common HTML view helper methods.</summary>
    public static class HtmlViewHelper
    {
        /// <summary>Creates the default UI element generators.</summary>
        /// <param name="view">The view.</param>
        /// <returns>The generators.</returns>
        public static Dictionary<string, IControlGenerator> GetDefaultGenerators(FrameworkElement view)
        {
            var list = new Dictionary<string, IControlGenerator>();

            list.Add("p", new ParagraphGenerator());
            list.Add("div", new ParagraphGenerator());
            list.Add("blockquote", new ParagraphGenerator());
            list.Add("h1", new ParagraphGenerator { FontSize = 1.6 });
            list.Add("h2", new ParagraphGenerator { FontSize = 1.4 });
            list.Add("h3", new ParagraphGenerator { FontSize = 1.2 });

            list.Add("html", new HtmlGenerator());
            list.Add("strong", new StrongGenerator());
            list.Add("b", list["strong"]);
            list.Add("text", new TextGenerator());
            list.Add("em", new EmGenerator());
            list.Add("i", list["em"]);
            list.Add("a", new LinkGenerator());
            list.Add("img", new CacheImageGenerator());
            list.Add("ul", new UlGenerator());
            list.Add("script", new EmptyGenerator());

            return list;
        }

        internal async static void Generate(this IHtmlView htmlView)
        {
            var html = htmlView.Html;
            var itemsControl = (ItemsControl)htmlView;

            if (string.IsNullOrEmpty(html))
                itemsControl.Items.Clear();


            htmlView.SizeDependentControls.Clear();

            

            var scrollableHtmlView = htmlView as HtmlControl;

         
            if (scrollableHtmlView != null)
                scrollableHtmlView.UpdateHeader();

            if (string.IsNullOrEmpty(html))
            {
                if (scrollableHtmlView != null)
                    scrollableHtmlView.UpdateFooter();

                if (htmlView is HtmlControl)
                    ((HtmlControl)htmlView).OnHtmlLoaded();
             
                   // ((HtmlView)htmlView).OnHtmlLoaded();

                return;
            }

            XmlElement htmlElement=null;
            await Task.Run(() =>

            {
                htmlElement = HtmlParser.ParseHtml(html);
            });

            foreach (var element in htmlElement.ChildNodes)
            {
                var xamlstring = HtmlToXamlConverter.ConvertHtmlToXaml(element.GetXml(), false);
                var richtextblock = (RichTextBlock) XamlReader.Load(xamlstring);
            }
        }
    }
}
