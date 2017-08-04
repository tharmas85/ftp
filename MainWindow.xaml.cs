using System.Windows;

namespace BajadaDeArchivos
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBajarTRN_Click(object sender, RoutedEventArgs e)
        {
            Logica.bajarTRN();
        }

        private void btnBajarRPT_Click(object sender, RoutedEventArgs e)
        {
            Logica.bajarRPT();
        }

        private void btnBajarBancos_Click(object sender, RoutedEventArgs e)
        {
            Logica.bajarBancos();
        }
    }
}
