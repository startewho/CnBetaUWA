
using System;


// ReSharper disable once CheckNamespace
namespace HtmlToXaml
{
    public  static class ObjectExtensions
    {

        public static string ToLower(this Object obj)
        {
            return (string) obj.ToString().ToLower();
        }
    }
}
