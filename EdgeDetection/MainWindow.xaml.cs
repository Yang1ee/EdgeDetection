using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using EdgeDetection.ImageProcessing;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using EdgeDetection.GeneralHelper;

namespace EdgeDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _imagePath = "";
        private const string LogFilePath = "system_log.txt";
        public MainWindow()
        {
            InitializeComponent();
            Logger.Info("Edge detection started.");
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
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
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imageInput.Source = bitmap;
            }
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_imagePath)) return;

            var imageData = LoadImageAsGrayscaleArray(_imagePath);
            var selected = ((System.Windows.Controls.ComboBoxItem)comboBoxEdgeDetectionSelection.SelectedItem)?.Content.ToString();
            var edgeOperator = OperatorFactory.GetOperator(selected);

            var processor = new EdgeDetectionProcessor(edgeOperator);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = processor.ProcessImage(imageData);
            stopwatch.Stop();
            string logEntry = $"[{DateTime.Now}] Processed '{Path.GetFileName(_imagePath)}" +
                $"' using {selected} operator in {stopwatch.ElapsedMilliseconds} ms";
            Logger.Info(logEntry);

            using var bmp = ConvertByteArrayToBitmap(result);
            bmp.Save("edge_output.bmp");

            using MemoryStream ms = new();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            imageOutput.Source = bitmapImage;
        }

        private byte[,] LoadImageAsGrayscaleArray(string path)
        {
            using Bitmap bmp = new(path);
            int width = bmp.Width;
            int height = bmp.Height;
            byte[,] imageData = new byte[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    byte gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    imageData[x, y] = gray;
                }

            return imageData;
        }

        private Bitmap ConvertByteArrayToBitmap(byte[,] imageData)
        {
            int width = imageData.GetLength(0);
            int height = imageData.GetLength(1);
            Bitmap bmp = new(width, height);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    byte value = imageData[x, y];
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(value, value, value));
                }

            return bmp;
        }
    }
}