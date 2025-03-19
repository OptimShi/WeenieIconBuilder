using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using WeenieIconBuilder.Db;
using ACE.DatLoader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

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
            string iconDir = WeenieIconBuilderSettings.Default.image_path;

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
                MessageBox.Show("There was an error connecting to your database.");
            }

            if (connected)
            {
                foreach(var weenie in db.Weenies)
                {
                    var w = weenie.Value;
                    var wcid = weenie.Key;
                    string filename = Path.Combine(iconDir, wcid + ".png");
                    if (!File.Exists(filename))
                    {
                        IconData iconData = IconData.GenerateFromWeenie(w); ;

                        Bitmap icon = IconBuilder.BuildIcon(iconData);
                        icon.Save(filename, ImageFormat.Png);
                    }
                }
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