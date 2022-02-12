using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    internal class CreateRowCollection
    {
        public static RowDefinitionCollection CreateCollection()
        {
            var rowDefcollection = new Grid().RowDefinitions;
            var rowDefborder = new RowDefinition();
            rowDefborder.Height = new GridLength(20);
            rowDefcollection.Add (rowDefborder);
            rowDefcollection.Add(rowDefborder);
            return rowDefcollection;
        }
    }
}
