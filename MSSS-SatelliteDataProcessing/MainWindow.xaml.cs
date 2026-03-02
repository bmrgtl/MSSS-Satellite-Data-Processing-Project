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
using System.Diagnostics;

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

        // Check if data is sorted
        bool isSensorASorted = false;
        bool isSensorBSorted = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // 4.2 Method to load data for Sensor A and B using Galileo6
        private void LoadData()
        {
            // clear existing data in linked lists before loading new data
            SensorA.Clear();
            SensorB.Clear();

            // creates new instance of ReadData class from Galileo6 to generate data for SensorA and SensorB
            ReadData data = new ReadData();

            double sigma = txtSigma.Value ?? 10;
            double mu = txtMean.Value ?? 50;
            

            // generates 400 data points for Sensor A and B using Galileo6
            // and adds them to their respective linked list
            for (int i = 0; i < 400; i++)
            {
                SensorA.AddLast(data.SensorA(mu, sigma));
                SensorB.AddLast(data.SensorB(mu, sigma));
            }
        }
        // 4.13 Sigma Range (10-20; Default 10)
        private void ValidateSigma()
        {
            if (txtSigma.Value == null || txtSigma.Value < 10 || txtSigma.Value > 20)
            {
                MessageBox.Show("Standard Deviation must be between 10 and 20. Default Value 10 is used.");
                txtSigma.Value = 10; // default value if input is out of range
            }

        }

        // 4.13 Mean Range (35-75; Default 50)
        private void ValidateMean()
        {
            if (txtMean.Value == null || txtMean.Value < 35 || txtMean.Value > 75)
            {
                MessageBox.Show("Mean must be between 35 and 75. Default Value 50 is used.");
                txtMean.Value = 50; // default value if input is out of range
            }
        }

        // 4.3 Method to display Sensor A and B data in ListBox and ListView
        private void ShowAllSensorData()
        {
            // clear existing items before displaying new data
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
            ValidateSigma();
            ValidateMean();

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
        private bool SelectionSort(LinkedList<double> sensorData)
        {
            int min = 0;
            int max = NumberOfNodes(sensorData);

            // iterates through linked list
            for (int i = 0; i < max - 1; i++)
            {
                min = i;

                // finds index of minimum value in the unsorted portion of the list
                for (int j = i + 1; j < max; j++)
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
            return true;
        }

        // 4.8 Insertion Sort
        private bool InsertionSort(LinkedList<double> sensorData)
        {
            int max = NumberOfNodes(sensorData);

            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (sensorData.ElementAt(j - 1) > sensorData.ElementAt(j))
                    {
                        LinkedListNode<double> current = sensorData.Find(sensorData.ElementAt(j));
                        LinkedListNode<double> previous = sensorData.Find(sensorData.ElementAt(j - 1));

                        var temp = current.Value;
                        current.Value = previous.Value;
                        previous.Value = temp;
                    }
                }
            }

            return true;
        }

        // 4.9 Binary Search Iterative
        private int BinarySearchIterative(LinkedList<double> sensorData, double target, int min, int max)
        {
            while (min <= max - 1)
            {
                int mid = (min + max) / 2;

                // binary search
                if (target == Math.Floor(sensorData.ElementAt(mid)))
                    return mid;
                else if (target < Math.Floor(sensorData.ElementAt(mid)))
                    max = mid - 1;
                else
                    min = mid + 1;
            }

            return -1;
        }

        // 4.10 Binary Search Recursive
        private int BinarySearchRecursive(LinkedList<double> sensorData, double target, int min, int max)
        {
            if (min <= max - 1)
            {
                int mid = (min + max) / 2;

                if (target == Math.Floor(sensorData.ElementAt(mid)))
                    return mid;
                else if (target < Math.Floor(sensorData.ElementAt(mid)))
                    return BinarySearchRecursive(sensorData, target, min, mid - 1);
                else
                    return BinarySearchRecursive(sensorData, target, mid + 1, max);
            }

            return -1; 
        }
        
        //4.11 Highlight all occurrences of target
        private void HighlightAllOccurrences(LinkedList<double> SensorData, ListBox lstSensor, int index, double target)
        {
            lstSensor.SelectedItems.Clear(); // Clear previous selections
            int firstOccurrence = index;
            int lastOccurrence = index + 1;

            // find the first value
            while (firstOccurrence >=0 && Math.Floor(SensorData.ElementAt(firstOccurrence)) == target)
            {
                lstSensor.SelectedItems.Add(lstSensor.Items.GetItemAt(firstOccurrence));
                firstOccurrence--;
            }

            // find the last value
            while (lastOccurrence < SensorData.Count && Math.Floor(SensorData.ElementAt(lastOccurrence)) == target)
            {
                lstSensor.SelectedItems.Add(lstSensor.Items.GetItemAt(lastOccurrence));
                lastOccurrence++;
            }

            lstSensor.ScrollIntoView(lstSensor.Items[lastOccurrence-1]);
        }

        // 4.11.1 Method for Sensor A Binary Search Iterative
        private void btnBinarySearchIterativeA_Click(object sender, RoutedEventArgs e)
        {

            // 4.11.1 checks if Sensor A data is sorted before performing binary search
            if (isSensorASorted == true)
            {
                // Input validation: ensures target value entered is a valid number
                if (!double.TryParse(txtTargetA.Text, out double target))
                {
                    MessageBox.Show("Please enter a valid number for the target value.");
                    return;
                }

                // 4.11.1 Measure ticks taken for binary search iterative method
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // start Binary Search
                int index = BinarySearchIterative(SensorA, target, 0, NumberOfNodes(SensorA));
                stopwatch.Stop();

                // 4.11.1 Display ticks taken for binary search iterative method in the corresponding textbox
                txtBinaryIterativeA.Text = stopwatch.ElapsedTicks.ToString() + " Ticks";

                if(index == -1)
                {
                    MessageBox.Show("Value Not Found.");
                }

                else
                    // 4.11.1 Highlight all values that are equal to the target (decimals excluded).
                    HighlightAllOccurrences(SensorA, lstSensorA, index, target);
                                
            }

            else
            {
                MessageBox.Show("Please sort the data before performing binary search.");
            }

        }

        //4.11.2 Method for Sensor A Binary Search Recursive
        private void btnBinarySearchRecursiveA_Click(object sender, RoutedEventArgs e)
        {
            // 4.11.2 checks if Sensor A data is sorted before performing binary search
            if (isSensorASorted == true)
            {
                // input validation: ensures target value entered is a valid number
                if (!double.TryParse(txtTargetA.Text, out double target))
                {
                    MessageBox.Show("Please enter a valid number for the target value.");
                    return;
                }

                // 4.11.2 Measure ticks taken for binary search recursive method
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                int index = BinarySearchRecursive(SensorA, target, 0, NumberOfNodes(SensorA));
                stopwatch.Stop();

                // 4.11.2 Display ticks taken for binary search iterative method in the corresponding textbox
                txtBinaryRecursiveA.Text = stopwatch.ElapsedTicks.ToString() + " Ticks";

                if (index == -1)
                {
                    MessageBox.Show("Value Not Found.");
                }

                else
                    // 4.11.2 Highlight all values that are equal to the target (decimals excluded).
                    HighlightAllOccurrences(SensorA, lstSensorA, index, target);
            }

            else
            {
                MessageBox.Show("Please sort the data before performing binary search.");
            }
        }

        //4.11.3 Method for Sensor B Binary Search Iterative
        private void btnBinarySearchIterativeB_Click(object sender, RoutedEventArgs e)
        {
            // 4.11.3 checks if Sensor B data is sorted before performing binary search
            if (isSensorBSorted == true)
            {
                // input validation: ensures target value entered is a valid number
                if (!double.TryParse(txtTargetB.Text, out double target))
                {
                    MessageBox.Show("Please enter a valid number for the target value.");
                    return;
                }

                // 4.11.3 Measure ticks taken for binary search iterative method
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                int index = BinarySearchIterative(SensorB, target, 0, NumberOfNodes(SensorB));
                stopwatch.Stop();

                // 4.11.3 Display ticks taken for binary search iterative method in the corresponding textbox
                txtBinaryIterativeB.Text = stopwatch.ElapsedTicks.ToString() + " Ticks";

                if (index == -1)
                {
                    MessageBox.Show("Value Not Found.");
                }

                else
                    // 4.11.3 Highlight all values that are equal to the target (decimals excluded).
                    HighlightAllOccurrences(SensorB, lstSensorB, index, target);
            }
            else
            {
                MessageBox.Show("Please sort the data before performing binary search.");
            }
        }

        //4.11.4 Method for Sensor B Binary Search Recursive
        private void btnBinarySearchRecursiveB_Click(object sender, RoutedEventArgs e)
        {
            // 4.11.4 checks if Sensor B data is sorted before performing binary search
            if (isSensorBSorted == true)
            {
                // input validation: ensures target value entered is a valid number
                if (!double.TryParse(txtTargetB.Text, out double target))
                {
                    MessageBox.Show("Please enter a valid number for the target value.");
                    return;
                }

                // 4.11.4 Measure ticks taken for binary search recursive method
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                int index = BinarySearchRecursive(SensorB, target, 0, NumberOfNodes(SensorB));
                stopwatch.Stop();


                // 4.11.4 Display ticks taken for binary search iterative method in the corresponding textbox
                txtBinaryRecursiveB.Text = stopwatch.ElapsedTicks.ToString() + " Ticks";

                if (index == -1)
                {
                    MessageBox.Show("Value Not Found.");
                }

                else
                    // 4.11.4 Highlight all values that are equal to the target (decimals excluded).
                    HighlightAllOccurrences(SensorB, lstSensorB, index, target);
            }
            else
            {
                MessageBox.Show("Please sort the data before performing binary search.");
            }
        }

        // 4.12.1 Method for Sensor A Selection Sort
        private void btnSelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            // 4.12.1 Measures time taken to sort Sensor A data using selection sort method
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            SelectionSort(SensorA);
            stopwatch.Stop();

            // 4.12.1 Displays time taken to sort Sensor A data using selection sort method in the corresponding textbox
            txtSelectionSortA.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            // 4.12.1 Displays sorted Sensor A data in the corresponding listbox
            DisplayListboxData(SensorA, lstSensorA);
            ShowAllSensorData();
            isSensorASorted = true;

        }

        //4.12.2 Method for Sensor A Insertion Sort
        private void btnInsertionSortA_Click(object sender, RoutedEventArgs e)
        {
            // 4.12.2 Measures time taken to sort Sensor A data using insertion sort method
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            InsertionSort(SensorA);
            stopwatch.Stop();

            // 4.12.2 Displays time taken to sort Sensor A data using insertion sort method in the corresponding textbox
            txtInsertionSortA.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            // 4.12.2 Displays sorted Sensor A data in the corresponding listbox
            DisplayListboxData(SensorA, lstSensorA);
            ShowAllSensorData();
            isSensorASorted = true;
           
        }

        //4.12.3 Method for Sensor B Selection Sort
        private void btnSelectionSortB_Click(object sender, RoutedEventArgs e)
        {
            // 4.12.3 Measures time taken to sort Sensor B data using selection sort method
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            SelectionSort(SensorB);
            stopwatch.Stop();

            // 4.12.3 Displays time taken to sort Sensor B data using selection sort method in the corresponding textbox
            txtSelectionSortB.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            // 4.12.3 Displays sorted Sensor B data in the corresponding listbox
            DisplayListboxData(SensorB, lstSensorB);
            ShowAllSensorData();
            isSensorBSorted = true;
            
        }

        //4.12.4 Method for Sensor B Insertion Sort
        private void btnInsertionSortB_Click(object sender, RoutedEventArgs e)
        {
            // 4.12.4 Measures time taken to sort Sensor B data using insertion sort method
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            InsertionSort(SensorB);
            stopwatch.Stop();

            // 4.12.4 Displays time taken to sort Sensor B data using insertion sort method in the corresponding textbox
            txtInsertionSortB.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";

            //4.12.4 Displays sorted Sensor B data in the corresponding listbox
            DisplayListboxData(SensorB, lstSensorB);
            ShowAllSensorData();
            isSensorBSorted = true;
            
        }

        // 4.14 Allow numeric input only for search targets
        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regular expression to allow only numeric input (including decimal point)
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}