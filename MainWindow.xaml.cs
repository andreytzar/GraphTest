using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphTest
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

        private void BTNGenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            int numbcolumns = 40;
            Random random = new Random();
            DateTime curdate = DateTime.Now.AddDays(-numbcolumns);
            GraphData[] data = new GraphData[numbcolumns];
            for (int i = 0; i < numbcolumns; i++)
            {
                data[i] = new($"{curdate.ToString("d")}", random.Next(0, 100), System.Drawing.Color.FromArgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255)));
                curdate=curdate.AddDays(1);
            }
            using var bmp = HelperGraph.GraphColumns(800, 450, data);
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            GraphImage.Source = imageSource;    
        }

        private void BTNGenerateGraphLinear_Click(object sender, RoutedEventArgs e)
        {
            int numbcolumns = 40;
            Random random = new Random();
            DateTime curdate = DateTime.Now.AddDays(-numbcolumns);
            GraphData[] data = new GraphData[numbcolumns];
            for (int i = 0; i < numbcolumns; i++)
            {
                data[i] = new($"{curdate.ToString("d")}", random.Next(0, 100), System.Drawing.Color.FromArgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255)));
                curdate = curdate.AddDays(1);
            }
            using var bmp = HelperGraph.LineGraphic (800, 450, data);
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            GraphImage.Source = imageSource;
        }
    }
}
