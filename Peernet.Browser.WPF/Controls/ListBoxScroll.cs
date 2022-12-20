using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    public class ListBoxScroll : ListBox
    {
        public ListBoxScroll()
            :base()
        {
            SelectionChanged += new SelectionChangedEventHandler(ListBoxScroll_SelectionChanged);
        }

        void ListBoxScroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollIntoView(SelectedItem);
        }
    }
}
