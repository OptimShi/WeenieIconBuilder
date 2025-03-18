using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeenieIconBuilder.Db;

namespace WeenieIconBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoadIcons_Click(object sender, RoutedEventArgs e)
        {
            // Init our DB
            DbManager db;
            db = new DbManager();
            var connected = false;
            try
            {
                connected = db.Connect();
            }
            catch (Exception)
            {
                // Somethign went wrong.

            }

            if (connected)
            {
                // Generate Icons
               // db.LoadAllWeenies();
            }
        }
    }
}