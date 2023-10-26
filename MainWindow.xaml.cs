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
using System.Windows.Controls.Primitives;
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

namespace MPX_player
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

            volSlider.MouseWheel += volSlider_Scroll;
            SettingsWindow.ThemeApply();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private MediaPlayer mediaPlayer = new MediaPlayer();

        private float _currentVolume = 100;
        private float _currentSliderPosition;
        private float _currentPositionMillis;
        private float _fullDurationMillis;
        private bool isDraggingSlider = false;
        private bool isMediaPlaying = false;
        private bool isAdjustingVolume = false;

        public float currentVolume
        {
            get { return _currentVolume; }
            set
            {
                if (_currentVolume != value)
                {
                    _currentVolume = value;
                    OnPropertyChanged("currentVolume");
                }
            }
        }
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
        }

        private void UpdateVolume(float volume)
        {
            mediaPlayer.Volume = volume / 100;
            volSlider.Value = volume;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                _currentPositionMillis = (float)mediaPlayer.Position.TotalMilliseconds;
                _fullDurationMillis = (float)mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;

                if (!isDraggingSlider)
                {
                    // Update the slider position only if it's not being manually dragged.
                    _currentSliderPosition = (_currentPositionMillis / _fullDurationMillis) * 100;
                    progSlider.Value = _currentSliderPosition;
                }

                lblStatus.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));

                UpdateVolume((float)volSlider.Value);
            }
            //else lblStatus.Content = "No file selected...";
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
            isMediaPlaying = true;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            isMediaPlaying = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            isMediaPlaying = false;
        }

        private void progSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Handle the slider's ValueChanged event.
            // Update the current position of the media player based on the slider value.
            if (mediaPlayer.Source != null && !progSlider.IsMouseCaptured && !isDraggingSlider)
            {
                TimeSpan newPosition = TimeSpan.FromMilliseconds((progSlider.Value / 100) * _fullDurationMillis);
                mediaPlayer.Position = newPosition;
            }
        }

        private void progSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Handle the MouseLeftButtonDown event to indicate that the user started dragging the slider.
            isDraggingSlider = true;
        }
        private void progSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Handle the PreviewMouseDown event (quick click).
            isDraggingSlider = true;
        }

        private void progSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Handle the MouseLeftButtonUp event to indicate that the user stopped dragging the slider.
            isDraggingSlider = false;
        }

        private void progSlider_DragStarted(object sender,  DragStartedEventArgs e)
        {
            isDraggingSlider = true;
        }

        private void progSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDraggingSlider = false;
            TimeSpan newPosition = TimeSpan.FromMilliseconds((progSlider.Value / 100) * _fullDurationMillis);
            mediaPlayer.Position = newPosition;
        }
        
        private void volSlider_Scroll(object sender, MouseWheelEventArgs e) 
        {
            isAdjustingVolume = true;

            _currentVolume += (e.Delta > 0) ? (_currentVolume < 100 ? 0.75f : 0) : (_currentVolume > 0 ? -0.75f : 0);

            UpdateVolume(_currentVolume);

            isAdjustingVolume = false;
        }
    }
}
