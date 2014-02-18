using Microsoft.Phone.Controls;
using BatteryAPI = Windows.Phone.Devices.Power;

namespace Battery
{
    public partial class MainPage : PhoneApplicationPage
    {

        private readonly BatteryAPI.Battery _battery;

        public MainPage()
        {
            InitializeComponent();

            _battery = BatteryAPI.Battery.GetDefault();
            _battery.RemainingChargePercentChanged += OnRemainingChargePercentChanged;

            UpdateUI();
        }

        private void OnRemainingChargePercentChanged(object sender, object e)
        {
            BatteryChargeTextBlock.Dispatcher.BeginInvoke(delegate()
            {
                UpdateUI();
            });
        }

        private void UpdateUI()
        {
            BatteryChargeTextBlock.Text = _battery.RemainingChargePercent + "% ( осталось - " +
                _battery.RemainingDischargeTime.Hours + "ч." + _battery.RemainingDischargeTime.Minutes + ")";
        }
    }
}