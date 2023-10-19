using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public static string mode { get; set; } = "Dark";
        public static string modeColor { get; set; } = "Orange";

        private float _currentSliderPosition;

        public float currentSliderPosition
        {
            get { return _currentSliderPosition; }
            set
            {
                if (_currentSliderPosition != value)
                {
                    _currentSliderPosition = value;
                    OnPropertyChanged("currentSliderPosition");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            //this.Title = "File: " + item.Header;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //this.Title = "File: " + item.Header;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                float currentPositionInSeconds = mediaPlayer.Position.Minutes * 60 + mediaPlayer.Position.Seconds;
                float fullDurationInSeconds = mediaPlayer.NaturalDuration.TimeSpan.Minutes * 60 + mediaPlayer.NaturalDuration.TimeSpan.Seconds;
                _currentSliderPosition = (currentPositionInSeconds / fullDurationInSeconds) * 100;
                lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                progSlider.Value = _currentSliderPosition;
            }
            else lblStatus.Content = "No file selected...";
        }
        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/RobertDoxa",
                UseShellExecute = true
            });
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
    }
}
