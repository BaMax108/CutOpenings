using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace CutOpeningsPlugin.Inserting.Views
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        /// <summary></summary>
        public string SelectedLink { get; private set; }
        /// <summary></summary>
        public string WallOffsetA { get; private set; }
        /// <summary></summary>
        public string WallOffsetB { get; private set; }
        /// <summary></summary>
        public string FloorOffsetA { get; private set; }
        /// <summary></summary>
        public string FloorOffsetB { get; private set; }

        /// <summary></summary>
        public UserWindow(List<string> _links)
        {
            InitializeComponent();

            Settings(_links);
        }

        private void Settings(List<string> _links)
        {
            ImgFloor.Source = DecodeImage(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Addins\2021\CGN_AddIns\IMG", "floorOffset.png"));
            ImgWAll.Source = DecodeImage(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Addins\2021\CGN_AddIns\IMG", "wallOffset.png"));

            if (_links.Count > 0)
            {
                cbLinks.ItemsSource = _links;
            }
            btnStart.IsEnabled = false;
        }

        private BitmapImage DecodeImage(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(path);
                bitmapImage.DecodePixelWidth = 150;
                bitmapImage.EndInit();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return bitmapImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbLinks.SelectedItem == null) return;
            SelectedLink = cbLinks.Text;
            this.Close();
        }

        private void CbLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLinks.Text != null) btnStart.IsEnabled = true;
        }
        
        private void TboxFloorOffset_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (Int32.TryParse(tb.Text, out int result))
            {
                tb.Text = result.ToString();

                switch (tb.Name)
                {
                    case "TboxWallOffsetA": WallOffsetA = result.ToString(); break;
                    case "TboxWallOffsetB": WallOffsetB = result.ToString(); break;
                    case "TboxFloorOffsetA": FloorOffsetA = result.ToString(); break;
                    case "TboxFloorOffsetB": FloorOffsetB = result.ToString(); break;
                }
            }
            else 
            {
                tb.Text = "0";
            }
        }
    }
}
