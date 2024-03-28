using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = new SqlConnection(@"Data Source=3205EC06; Initial Catalog=Task; Integrated Security=True");

        public MainWindow()
        {
            InitializeComponent();

        }
        List<string> taskList = new List<string>();
        List<string> newTask = new List<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string text = task.Text;

            taskList.Add(text);
            listBox.Items.Add(text);

            UpdateList();
            task.Text = "";
        }
        private void UpdateList()
        {
            listBox.Items.Clear();
            foreach (string task in taskList)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                TextBlock textBlock = new TextBlock { Text = task, Margin = new Thickness(5) };
                Button delete = new Button 
                {
      
                    Content = "✖",
                    Tag = task
                };
                Button update = new Button
                {
                    Content = "✔",
                    Tag = task
                };
                update.Click += MoveToNewList;
                delete.Click += Delete_Task;
                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(delete);
                stackPanel.Children.Add(update);
                listBox.Items.Add(stackPanel);
               
            }
        }
        private void Delete_Task(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            string task = button.Tag.ToString();
            taskList.Remove(task);
            UpdateList();

        }
        private void MoveToNewList(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            string task = button.Tag.ToString();
            newTask.Add(task);
            UpdateList();
            UpdateMoveToNewList();
            taskList.Remove(task);
            UpdateList();
        }

        private void UpdateMoveToNewList()
        {

            newlistBox.Items.Clear();
            foreach (string task in newTask)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                TextBlock textBlock = new TextBlock { Text = task, Margin = new Thickness(5) };
                
                stackPanel.Children.Add(textBlock);
                newlistBox.Items.Add(stackPanel);
            }
        }
        private void СreateList()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select Name,id from Task", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string line = (string)reader[0];
            reader.Close();
        }

    }
}
