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
        
        private SqlConnection conn;
        private List<string> taskList;
        private List<string> newTask;
        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection(@"Data Source=3218EC08; Initial Catalog=ToDo; Integrated Security=True");
            taskList = new List<string>();
            newTask = new List<string>();
            GetTasksFromDatabase();
            GetCompletedTasksFromDatabase();

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    string text = task.Text;

        //    taskList.Add(text);
        //    listBox.Items.Add(text);

        //    UpdateList();
        //    task.Text = "";
        //}
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
        private void GetTasksFromDatabase()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Task", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string taskName = (string)reader["Name"];
                    taskList.Add(taskName);
                    UpdateList();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving tasks from the database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void AddTaskToDatabase(string taskName)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Task (Name) VALUES (@TaskName)", conn);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                cmd.ExecuteNonQuery();
                taskList.Add(taskName);
                UpdateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding task to the database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void GetCompletedTasksFromDatabase()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM NewTask", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string completedTaskName = (string)reader["Name"];
                    newTask.Add(completedTaskName);
                    UpdateMoveToNewList();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving completed tasks from the database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void AddCompletedTaskToDatabase(string taskName)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO NewTask (Name) VALUES (@TaskName)", conn);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                cmd.ExecuteNonQuery();
                newTask.Add(taskName);
                UpdateMoveToNewList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding completed task to the database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void DeleteTaskFromDatabase(string taskName)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Task WHERE Name = @TaskName", conn);
                cmd.Parameters.AddWithValue("@TaskName", taskName);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    taskList.Remove(taskName);
                    UpdateList();
                }
                else
                {
                    MessageBox.Show("Задача с указанным именем не найдена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while deleting task from the database: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = task.Text;
            AddTaskToDatabase(text);
            task.Text = "";
        }
        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string task = button.Tag.ToString();
            DeleteTaskFromDatabase(task);
        }

        private void MoveToNewList(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string task = button.Tag.ToString();
            DeleteTaskFromDatabase(task);
            AddCompletedTaskToDatabase(task);
            UpdateMoveToNewList();
        }
        
    }
}

       
    

    


 
