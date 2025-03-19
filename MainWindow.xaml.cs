using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using WeenieIconBuilder.Db;
using ACE.DatLoader;

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

            LoadDATs(WeenieIconBuilderSettings.Default.portal_dat_path);

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
                foreach(var weenie in db.Weenies)
                {
                    var w = weenie.Value;
                    IconData iconData = IconData.GenerateFromWeenie(w); ;
                    

                }
                // Generate Icons
               // db.LoadAllWeenies();
            }
        }

        public void LoadDATs(string filename)
        {
            if (!File.Exists(filename) && !Directory.Exists(filename)) return;

                // MainWindow.Status.WriteLine("Reading " + filename);

            ReadDATFile(filename);
        }

        public static void ReadDATFile(string filename)
        {
            var fi = new System.IO.FileInfo(filename);
            var di = fi.Attributes.HasFlag(FileAttributes.Directory) ? new DirectoryInfo(filename) : fi.Directory;

            var loadCell = false;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DatManager.Initialize(di.FullName, true, loadCell);
        }

    }
}