using System.Windows;
using Task8.ViewModels;

namespace Task8.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}
