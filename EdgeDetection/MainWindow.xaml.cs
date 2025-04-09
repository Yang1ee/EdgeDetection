using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using EdgeDetection.ImageProcessing;
using EdgeDetection.GeneralHelper;

namespace EdgeDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _imagePath = "";
        public MainWindow()
        {
            InitializeComponent();
            Logger.Info("Edge detection started.");
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        private void LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.bmp;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                _imagePath = openFileDialog.FileName;
                ImagePathTextBox.Text = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                InputImagePreview.Source = bitmap;
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_imagePath)) return;

            //convert to greyscale
            var imageData = ImageHandler.LoadImageAsGrayscaleArray(_imagePath);

            var selected = ((System.Windows.Controls.ComboBoxItem)OperatorComboBox.SelectedItem)?.Content.ToString();
            var edgeOperator = OperatorFactory.GetOperator(selected.ToLower());

            var processor = new EdgeDetectionProcessor(edgeOperator);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = processor.ProcessImage(imageData);
            stopwatch.Stop();
            string logEntry = $"[{DateTime.Now}] Processed '{Path.GetFileName(_imagePath)}" +
                $"' using {selected} operator in {stopwatch.ElapsedMilliseconds} ms";
            Logger.Info(logEntry);

            using var bmp = ImageHandler.ConvertByteArrayToBitmap(result);
            bmp.Save("edge_output.bmp");

            using MemoryStream ms = new();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            OutputImagePreview.Source = bitmapImage;
        }


    }
}