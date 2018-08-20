using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Util.Controls
{
    /// <summary>
    /// 文本垂直TextBlock
    /// </summary>
    public partial class VerticalTextBlock : TextBlock
    {
        static VerticalTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalTextBlock),new FrameworkPropertyMetadata(typeof(VerticalTextBlock)));
        }
    }
}
