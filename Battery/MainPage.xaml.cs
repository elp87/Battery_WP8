using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using BatteryAPI = Windows.Phone.Devices.Power;

namespace Battery
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Fields
        private readonly BatteryAPI.Battery _battery;
        private Rectangle _chargeLevelRectangle;
        #endregion

        #region Constructors
        public MainPage()
        {
            InitializeComponent();

            _battery = BatteryAPI.Battery.GetDefault();
            _battery.RemainingChargePercentChanged += OnRemainingChargePercentChanged;

            UpdateInfo();

        } 
        #endregion

        #region Event Handlers
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            DrawBattery();
        }

        private void OnRemainingChargePercentChanged(object sender, object e)
        {
            BatteryChargeTextBlock.Dispatcher.BeginInvoke(delegate()
            {
                UpdateInfo();
                UpdateBatteryPic();
            });
        } 
        #endregion

        #region Methods
        private void DrawBattery()
        {
            Brush brush = BatteryChargeTextBlock.Foreground;
            Rectangle _batMainRectangle = StandardRectangleCreator();
            DrawGrid.Children.Add(_batMainRectangle);
            _batMainRectangle.Margin = new Thickness(0, 0, 0, 100);
            _batMainRectangle.StrokeThickness = 3;
            _batMainRectangle.Width = LayoutRoot.ActualWidth * 0.3;
            _batMainRectangle.Height = LayoutRoot.ActualHeight * 0.4;


            Rectangle _batTopRectangle = StandardRectangleCreator();
            DrawGrid.Children.Add(_batTopRectangle);
            _batTopRectangle.Margin = new Thickness(0, 0, 0, 97 + (LayoutRoot.ActualHeight * 0.4));
            _batTopRectangle.StrokeThickness = 3;
            _batTopRectangle.Width = LayoutRoot.ActualWidth * 0.1;
            _batTopRectangle.Height = 15;

            _chargeLevelRectangle = StandardRectangleCreator();
            DrawGrid.Children.Add(_chargeLevelRectangle);
            _chargeLevelRectangle.Fill = new SolidColorBrush(CalcColor());
            _chargeLevelRectangle.Margin = new Thickness(0, 0, 0, 102);
            _chargeLevelRectangle.Width = LayoutRoot.ActualWidth * 0.3 - 4;
            _chargeLevelRectangle.Height = ((LayoutRoot.ActualHeight * 0.4) - 4) * _battery.RemainingChargePercent / 100;
        }

        private void UpdateInfo()
        {
            BatteryChargeTextBlock.Text = _battery.RemainingChargePercent + "% (осталось - " +
                _battery.RemainingDischargeTime.Hours + "ч." + _battery.RemainingDischargeTime.Minutes + "м.)";
        }

        private void UpdateBatteryPic()
        {
            _chargeLevelRectangle.Height = ((LayoutRoot.ActualHeight * 0.4) - 4) * _battery.RemainingChargePercent / 100;
            _chargeLevelRectangle.Fill = new SolidColorBrush(CalcColor());
        }

        private Color CalcColor()
        {
            Color color = new Color();
            int charge = _battery.RemainingChargePercent;
            color.A = 255;
            color.B = 0;
            if (charge > 50)
            {
                color.R = (byte)(255 - (255 / 50 * (charge - 50)));
                color.G = 255;
            }
            else
            {
                color.R = 255;
                color.G = (byte)(0 + (255 / 50 * charge));
            }
            return color;
        }

        private Rectangle StandardRectangleCreator()
        {
            Rectangle rect = new Rectangle();
            rect.HorizontalAlignment = HorizontalAlignment.Center;
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Stroke = new SolidColorBrush((Color)Application.Current.Resources["PhoneForegroundColor"]);
            return rect;
        }
        #endregion

        
    }        
}