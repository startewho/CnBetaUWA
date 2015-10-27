using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace CnBetaUWA.Behaviors
{
    public  class ListViewIndexNumberBehavior: DependencyObject, IBehavior

    {
        public void Attach(DependencyObject associatedObject)
        {
            throw new NotImplementedException();
        }

        public void Detach()
        {
            throw new NotImplementedException();
        }

        public DependencyObject AssociatedObject { get; }
    }

   
}
