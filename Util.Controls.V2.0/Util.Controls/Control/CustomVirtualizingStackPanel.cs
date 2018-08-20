using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Util.Controls
{
    public class CustomVirtualizingStackPanel: VirtualizingStackPanel
    {
        public void BringIntoView(int index)
        {
            this.BringIndexIntoView(index);
        }
    }
}
