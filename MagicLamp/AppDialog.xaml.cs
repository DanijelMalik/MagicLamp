using MagicLamp.ViewModels;
using Microsoft.Win32;

namespace MagicLamp
{
    /// <summary>
    /// Interaction logic for AppDialog.xaml
    /// </summary>
    public partial class AppDialog
    {
        public AppDialog()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
        }

        internal MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var dialogResult = dialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                ViewModel.LoadFromFile(dialog.FileName);
            }
        }

        private void CreateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}