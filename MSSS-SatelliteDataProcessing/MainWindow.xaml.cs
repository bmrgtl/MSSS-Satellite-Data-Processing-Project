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

            // iterates through linked list
            for (int i = 0; i < max - 1; i++)
            {
                min = i;

                // finds index of minimum value in the unsorted portion of the list
                for (int j = i+1; j < max; j++)
                {
                    // compares current minimum with the next element in the list
                    if (sensorData.ElementAt(j) < sensorData.ElementAt(min))
                    {
                        // updates minimum index if a smaller value is found
                        min = j;
                    }
                }

                // swaps the minimum value with the first element of the unsorted portion of the list
                LinkedListNode<double> currentMin = sensorData.Find(sensorData.ElementAt(min));
                // finds the node corresponding to the current minimum value and the current index i
                LinkedListNode<double> currentI = sensorData.Find(sensorData.ElementAt(i));

                // swaps the values of the current minimum node and the current index node
                var temp = currentMin.Value;
                // updates the value of the current minimum node to the value of the current index node
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
        }

        // 4.8 Insertion Sort
        private void InsertionSort(LinkedList<double> sensorData)
        {
            int max = NumberOfNodes(sensorData);

            for (int i = 0; i < max - 1; i++)
            {
                for(int j = i+1; j > 0; j--)
                {
                    if(sensorData.ElementAt(j-1) > sensorData.ElementAt(j))
                    {
                        LinkedListNode<double> current = sensorData.Find(sensorData.ElementAt(j));
                        LinkedListNode<double> previous = sensorData.Find(sensorData.ElementAt(j-1));
                        
                        var temp = current.Value;
                        current.Value = previous.Value;
                        previous.Value = temp;
                    }
                }
            }
        }

        // 4.9 Binary Search Iterative
        private int BinarySearchIterative(LinkedList<double> sensorData, double target,  int min, int max)
        {
            
            int closestIndex = min;

            while (min <= max - 1)
            {
                int mid = (min + max) / 2;

                // updates closest index if the current middle value is closer
                // to the target than the previously recorded closest value
                if (Math.Abs(sensorData.ElementAt(mid) - target) < 
                    Math.Abs(sensorData.ElementAt(closestIndex) - target))
                {
                    closestIndex = mid;
                }

                // binary search
                if (target == sensorData.ElementAt(mid))
                    return mid;
                else if (target < sensorData.ElementAt(mid))
                    max = mid - 1;
                else
                    min = mid + 1;
            }

            return closestIndex;
        }

        // 4.10 Binary Search Recursive
        private int BinarySearchRecursive(LinkedList<double> sensorData, double target, int min, int max)
        {
            if (min <= max - 1)
            {
                int mid = (min + max) / 2;
                    
                if (target == sensorData.ElementAt(mid))
                    return mid;
                else if (target < sensorData.ElementAt(mid))
                    return BinarySearchRecursive(sensorData, target, min, mid - 1);
                else
                    return BinarySearchRecursive(sensorData, target, mid + 1, max);
            }

            // nearest neighbor logic: compares the values at the minimum and maximum indices to the target value
            if (min >= NumberOfNodes(sensorData))
            {
                min = NumberOfNodes(sensorData) - 1;
            }

            if (max < 0)
            {
               return 0;
            }

            if (Math.Abs(sensorData.ElementAt(min) - target) <
                Math.Abs(sensorData.ElementAt(max) - target))
            {
                return min;
            }
            else
                return max;
        }


        private void btnSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(SensorA);
            DisplayListboxData(SensorA, lstSensorA);
        }

        private void btnInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(SensorA);
            DisplayListboxData(SensorA, lstSensorA);
        }

        private void btnBinarySearchIterative_Click(object sender, RoutedEventArgs e)
        {
            double target = double.Parse(txtTarget.Text);
            int index = BinarySearchIterative(SensorA, target, 0, NumberOfNodes(SensorA));
            lstSensorA.SelectedIndex = index;
            lstSensorA.ScrollIntoView(lstSensorA.SelectedItem);
            
        }

        private void btnBinarySearchRecursive_Click(object sender, RoutedEventArgs e)
        {
            double target = double.Parse(txtTarget.Text);
            int index = BinarySearchRecursive(SensorA, target, 0, NumberOfNodes(SensorA));
            lstSensorA.SelectedIndex = index;
            lstSensorA.ScrollIntoView(lstSensorA.SelectedItem);
        }


    }
}