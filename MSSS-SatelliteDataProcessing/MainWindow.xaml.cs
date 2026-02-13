using Galileo6;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MSSS_SatelliteDataProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinkedList<double> SensorA { get; } = new LinkedList<double>();
        LinkedList<double> SensorB { get; } = new LinkedList<double>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void LoadData()
        {
            ReadData data = new ReadData();

            double sigma = double.Parse(txtSigma.Text);
            double mu = double.Parse(txtMean.Text);

            for (int i = 0; i <= 400; i++)
            {
                SensorA.AddLast(data.SensorA(mu, sigma));
                SensorB.AddLast(data.SensorB(mu, sigma));
            }
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();

            for(int i = 0; i < SensorA.Count && i< SensorB.Count; i++)
            {
                lstSensorA.Items.Add(SensorA.ElementAt(i));
                lstSensorB.Items.Add(SensorB.ElementAt(i));

                lvSensorData.Items.Add(new
                {
                    SensorA = SensorA.ElementAt(i),
                    SensorB = SensorB.ElementAt(i)
                });
            }

        }
    }
}