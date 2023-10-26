using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MPX_player
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        private static string mode { get; set; } = Settings.Default.themeMode;
        private static string modeColor { get; set; } = Settings.Default.themeColor;

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
            UpdateRadioButtonMode();
            UpdateRadioButtonColor();
            ThemeApply();
        }
        
        private void UpdateRadioButtonMode()
        {
            foreach (var radioButton in modeRadioButtons.Children.OfType<RadioButton>())
            {
                radioButton.IsChecked = (string)radioButton.Content == mode;
            }
        }
        private void UpdateRadioButtonColor()
        {
            foreach (var radioButton in colorRadioButtons.Children.OfType<RadioButton>())
            {
                radioButton.IsChecked = (string)radioButton.Content == modeColor;
            }
        }

        private void DarkModeClick(object sender, RoutedEventArgs e)
        {
            mode = "Dark";
            Settings.Default.themeMode = mode;
            Settings.Default.themeColor = modeColor;
            Settings.Default.Save();
            ThemeApply();
        }
        private void LightModeClick(object sender, RoutedEventArgs e)
        {
            mode = "Light";
            Settings.Default.themeMode = mode;
            Settings.Default.themeColor = modeColor;
            Settings.Default.Save();
            ThemeApply();
        }
        private void ColorRadioChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                modeColor = radioButton.Content.ToString();
                Settings.Default.themeMode = mode;
                Settings.Default.themeColor = modeColor;
                Settings.Default.Save();
                ThemeApply();
            }
        }
        public bool IsColorChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            string radioColor = radioButton.Content.ToString();
            
            if(modeColor == radioColor)
            {
                return true;
            }
            return false;
        }
        public static void ThemeApply()
        {
            foreach (Window window in Application.Current.Windows)
            {
                string completeMode = mode + "." + modeColor;
                if (window is MetroWindow metroWindow)
                {
                    ThemeManager.Current.ChangeTheme(metroWindow, completeMode);
                }
            }
        }
    }
}
