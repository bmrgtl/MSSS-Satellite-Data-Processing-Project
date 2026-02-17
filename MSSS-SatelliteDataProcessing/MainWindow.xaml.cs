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
        // 4.1 Global LinkedList<double> that stores data for Sensor A and B
        LinkedList<double> SensorA { get; } = new LinkedList<double>();
        LinkedList<double> SensorB { get; } = new LinkedList<double>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // 4.2 Method to load data for Sensor A and B using Galileo6
        private void LoadData()
        {
            // creates new instance of ReadData class from Galileo6 to generate data for SensorA and SensorB
            ReadData data = new ReadData();

            // parses standard deviation and mean from textboxes to double variables
            double sigma = double.Parse(txtSigma.Text);
            double mu = double.Parse(txtMean.Text);

            // generates 400 data points for Sensor A and B using Galileo6
            // and adds them to their respective linked list
            for (int i = 0; i <= 400; i++)
            {
                SensorA.AddLast(data.SensorA(mu, sigma));
                SensorB.AddLast(data.SensorB(mu, sigma));
            }
        }

        // 4.3 Method to display Sensor A and B data in ListBox and ListView
        private void ShowAllSensorData()
        {
            // clear existing items before displaying new data
            lstSensorA.Items.Clear();
            lstSensorB.Items.Clear();
            lvSensorData.Items.Clear();

            // populates ListView with data from Sensor A and B linked lists
            for (int i = 0; i < SensorA.Count && i < SensorB.Count; i++)
            {
                lvSensorData.Items.Add(new
                {
                    SensorA = SensorA.ElementAt(i),
                    SensorB = SensorB.ElementAt(i)
                });
            }
        }

        // 4.4 On click event handler that loads and shows sensor data
        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
            DisplayListboxData(SensorA, lstSensorA);
            DisplayListboxData(SensorB, lstSensorB);
        }

        // 4.5 Number of Nodes: returns number of nodes in the linked list
        private int NumberOfNodes(LinkedList<double> sensorData)
        {
            return sensorData.Count;
        }

        // 4.6 DisplayListboxData: shows data from linked list in the specified listbox
        private void DisplayListboxData(LinkedList<double> sensorData, ListBox listBox)
        {
            listBox.Items.Clear();
            foreach (var data in sensorData)
            {
                listBox.Items.Add(data);
            }
        }

        // 4.7 Selection Sort
        private void SelectionSort(LinkedList<double> sensorData)
        {
            int min = 0;
            int max = NumberOfNodes(sensorData);

            for (int i = 0; i < max - 1; i++)
            {
                min = i;

                for (int j = i+1; j < max; j++)
                {
                    if (sensorData.ElementAt(j) < sensorData.ElementAt(min))
                    {
                        min = j;
                    }
                }

                LinkedListNode<double> currentMin = sensorData.Find(sensorData.ElementAt(min));
                LinkedListNode<double> currentI = sensorData.Find(sensorData.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            } // Placeholder for selection sort implementation
        }

        private void btnSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(SensorA);
            DisplayListboxData(SensorA, lstSensorA);
        }
        // 
    }
}