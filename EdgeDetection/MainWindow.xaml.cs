using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using EdgeDetection.ImageProcessing;
using EdgeDetection.GeneralHelper;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace EdgeDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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
            openFileDialog.Filter = "Image files (*.bmp;*.jpeg;*.jpg;*.png)|*.bmp;*.jpeg;*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;

                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    InputImagePreview.Source = bitmap;
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to load image");
                    MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImagePathTextBox.Text)) return;

            try
            {
                //convert to grey scale image
                var imageData = ImageHandler.LoadImageAsGrayscaleArray(ImagePathTextBox.Text);
                var edgeOperator = GetSelectedOperator();

                OutputImagePreview.Source = ProcessImage(imageData,edgeOperator);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during image processing: {ex.Message}");
                Logger.Error("Processing error", ex);
            }

        }
        private IEdgeDetectionOperator GetSelectedOperator()
        {
            var selected = ((ComboBoxItem)OperatorComboBox.SelectedItem)?.Content?.ToString()?.ToLower();

            if (string.IsNullOrEmpty(selected))
            {
                Logger.Error("Invalide edge detection operator");
                MessageBox.Show("Please select an edge detection operator.");
                return null;
            }

            return  OperatorFactory.GetOperator(selected);
        }

        private BitmapImage ProcessImage(byte[,] imageData, IEdgeDetectionOperator edgeOperator)
        {
            var processor = new EdgeDetectionProcessor(edgeOperator);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = processor.ProcessImage(imageData);
            stopwatch.Stop();
            string logEntry = $"Processed '{Path.GetFileName(ImagePathTextBox.Text)}" +
                $"' using {edgeOperator} operator in {stopwatch.ElapsedMilliseconds} ms";
            Logger.Info(logEntry);

            
            Bitmap bmp = ImageHandler.ConvertByteArrayToBitmap(result);
            bmp.Save("edge_output.bmp");

            return ImageHandler.ConvertToBitmapImage(bmp); ;
        }



    }
}