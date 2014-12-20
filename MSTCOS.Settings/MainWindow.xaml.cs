using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MSTCOS.Settings
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SettingData data;
            try
            {
                data = Base.StorageManager.LoadData<SettingData>(@"Config.mstcos");
            }
            catch
            {
                data = new SettingData();
            }
            ReadData(data);
        }

        void ReadData(SettingData data)
        {
            if (data.AmbientOn)
            {
                AmbientOpen.IsChecked = true;
            }
            else AmbientClose.IsChecked = true;

            if (data.ResolutionX == 1280&&data.ResolutionY==720)
            {
                resolution1280_720.IsChecked = true;
            }
            else if (data.ResolutionX == 1024 && data.ResolutionY == 768)
            {
                resolution1024_768.IsChecked = true;
            }
            else if (data.ResolutionX == 800 && data.ResolutionY == 600)
            {
                resolution800_600.IsChecked = true;
            }

            IPBox.Text = data.IP;
            PortBox.Text = data.Port;
        }

        SettingData GenerateData()
        {
            SettingData data = new SettingData();
            if (resolution1280_720.IsChecked.HasValue && resolution1280_720.IsChecked.Value == true)
            {
                data.ResolutionX = 1280;
                data.ResolutionY = 720;
            }
            else if (resolution1024_768.IsChecked.HasValue && resolution1024_768.IsChecked.Value == true)
            {
                data.ResolutionX = 1024;
                data.ResolutionY = 768;
            }
            else if (resolution800_600.IsChecked.HasValue && resolution800_600.IsChecked.Value == true)
            {
                data.ResolutionX = 800;
                data.ResolutionY = 600;
            }
            else
            {
                data.ResolutionX = 1280;
                data.ResolutionY = 720;
            }

            if (AmbientOpen.IsChecked.HasValue && AmbientOpen.IsChecked.Value == true)
            {
                data.AmbientOn = true;
            }
            else data.AmbientOn = false;

            data.IP = IPBox.Text;
            data.Port = PortBox.Text;

            return data;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            SettingData data = new SettingData();
            ReadData(data);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingData d = GenerateData();
                Base.StorageManager.SaveData(d, @"Config.mstcos");
                Close();
            }
            catch
            {
                MessageBox.Show("设定未正确保存");
            }
        }

    }
}
