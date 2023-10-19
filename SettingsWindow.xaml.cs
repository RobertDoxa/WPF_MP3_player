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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
            UpdateRadioButtonMode();
            UpdateRadioButtonColor();
        }

        private void UpdateRadioButtonMode()
        {
            string currentMode = MainWindow.modeColor;

            foreach (var radioButton in modeRadioButtons.Children.OfType<RadioButton>())
            {
                radioButton.IsChecked = (string)radioButton.Content == currentMode;
            }
        }
        private void UpdateRadioButtonColor()
        {
            string currentModeColor = MainWindow.modeColor;

            foreach (var radioButton in colorRadioButtons.Children.OfType<RadioButton>())
            {
                radioButton.IsChecked = (string)radioButton.Content == currentModeColor;
            }
        }

        private void DarkModeClick(object sender, RoutedEventArgs e)
        {
            MainWindow.mode = "Dark";
            ThemeApply();
        }
        private void LightModeClick(object sender, RoutedEventArgs e)
        {
            MainWindow.mode = "Light";
            ThemeApply();
        }
        private void ColorRadioChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                MainWindow.modeColor = radioButton.Content.ToString();
                ThemeApply();
            }
        }
        public bool IsColorChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            string radioColor = radioButton.Content.ToString();
            
            if(MainWindow.modeColor == radioColor)
            {
                return true;
            }
            return false;
        }
        private void ThemeApply()
        {
            foreach (Window window in Application.Current.Windows)
            {
                string completeMode = MainWindow.mode + "." + MainWindow.modeColor;
                if (window is MetroWindow metroWindow)
                {
                    ThemeManager.Current.ChangeTheme(metroWindow, completeMode);
                }
            }
        }
    }
}
