using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for PagerControl.xaml
    /// </summary>
    public partial class PagerControl : UserControl
    {
        public event EventHandler PageSizeChanged;
        public event EventHandler PageIndexChanged;

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int),
                typeof(PagerControl), new PropertyMetadata(OnPageIndexChangedCallBack));

        public static readonly DependencyProperty PageSizeProperty =
                    DependencyProperty.Register("PageSize", typeof(int),
                typeof(PagerControl), new PropertyMetadata(OnPageSizeChangedCallBack));
        
        public static readonly DependencyProperty PagesCountProperty =
                    DependencyProperty.Register("PagesCount", typeof(int),
                typeof(PagerControl), null);
        
        public static readonly DependencyProperty FirstPageCommandProperty =
                    DependencyProperty.Register("FirstPageCommand", typeof(ICommand),
                typeof(PagerControl), null);

        public static readonly DependencyProperty LastPageCommandProperty =
                    DependencyProperty.Register("LastPageCommand", typeof(ICommand),
                typeof(PagerControl), null);

        public static readonly DependencyProperty PreviousPageCommandProperty =
                    DependencyProperty.Register("PreviousPageCommand", typeof(ICommand),
                typeof(PagerControl), null);

        public static readonly DependencyProperty NextPageCommandProperty =
                    DependencyProperty.Register("NextPageCommand", typeof(ICommand),
                typeof(PagerControl), null);

        public static readonly DependencyProperty RefreshPageCommandProperty =
                    DependencyProperty.Register("RefreshPageCommand", typeof(ICommand),
                typeof(PagerControl), null);

        public PagerControl()
        {
            InitializeComponent();
        }

        public ICommand FirstPageCommand
        {
            get => (ICommand)GetValue(FirstPageCommandProperty);
            set => SetValue(FirstPageCommandProperty, value);
        }

        public ICommand NextPageCommand
        {
            get => (ICommand)GetValue(NextPageCommandProperty);
            set => SetValue(NextPageCommandProperty, value);
        }

        public ICommand LastPageCommand
        {
            get => (ICommand)GetValue(LastPageCommandProperty);
            set => SetValue(LastPageCommandProperty, value);
        }
        
        public ICommand PreviousPageCommand
        {
            get => (ICommand)GetValue(PreviousPageCommandProperty);
            set => SetValue(PreviousPageCommandProperty, value);
        }

        public ICommand RefreshPageCommand
        {
            get => (ICommand)GetValue(RefreshPageCommandProperty);
            set => SetValue(RefreshPageCommandProperty, value);
        }

        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public int PagesCount
        {
            get => (int)GetValue(PagesCountProperty);
            set => SetValue(PagesCountProperty, value);
        }

        private static void OnPageSizeChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PagerControl pc = sender as PagerControl;
            if (pc != null)
            {
                pc?.PageSizeChanged?.Invoke(sender, new());
            }
        }
        
        private static void OnPageIndexChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PagerControl pc = sender as PagerControl;
            if (pc != null)
            {
                pc?.PageIndexChanged?.Invoke(sender, new());
            }
        }
    }
}