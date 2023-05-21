using MaterialDesignThemes.Wpf;
using RentACar.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

namespace RentACar
{
    /// <summary>
    /// Interaction logic for ReturnDetailsForm.xaml
    /// </summary>
    public partial class ReturnDetailsForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        OrdersProfile profile {get; set; }
        public List<Car> selectedCars = new List<Car>();
        public ObservableCollection<Car> Cars { get; set; }
        public ReturnDetailsForm()
        {
            InitializeComponent();
            profile = new OrdersProfile(); 
            Cars = new ObservableCollection<Car>();
        }

        private void Border_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool isMax = false;
        private void Border_mouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMax)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 780;
                    isMax = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    isMax = false;
                }
            }
        }

        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn_AddCarForm_click(object sender, RoutedEventArgs e)
        {
            string lp = txtLicensePlate.Text;
            string time = cbTime.SelectedItem.ToString();
            DateTime returnDate = DateTime.Parse(txtReturnDate.Text);
            string orderNumber = txtOrderNumber.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM Orders WHERE OrderNumber = @orderNumber";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                int orderCount = (int)selectCommand.ExecuteScalar();
                if (orderCount == 0)
                {
                    MessageBox.Show("Order with specified number not found.");
                }
                else
                {
                    string updateQuery = "INSERT INTO ReturnCar (ReturnDateCar,LicensePlate,OrderNumber,CarReturnTime) VALUES (@returnDatecar,@licensePlate,@OrderNumber,@carReturnTime)";
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@returnDateCar", returnDate);
                    command.Parameters.AddWithValue("@licensePlate", lp);
                    command.Parameters.AddWithValue("@orderNumber", orderNumber);
                    command.Parameters.AddWithValue("@carReturnTime", time);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string updateSql = $"UPDATE CarData SET Availability='Avalaible' WHERE LicensePlate='{lp}'";
                        SqlCommand updateCommand = new SqlCommand(updateSql, connection);
                        updateCommand.ExecuteNonQuery();
                        MessageBox.Show("Return date has been updated.");
                        string selectSql = "SELECT * FROM CarData WHERE Availability = 'Avalaible'";
                        SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        profile.ReturnCarsDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                    else
                    {
                        MessageBox.Show("Failed to update return date.");
                    }
                }
            }
        }

                private void Window_Loaded(object sender, RoutedEventArgs e)
                {
            cbTime.Items.Add("9:00");
            cbTime.Items.Add("10:00");
            cbTime.Items.Add("11:00"); 
            cbTime.Items.Add("12:00");
            cbTime.Items.Add("13:00");
            cbTime.Items.Add("14:00");
            cbTime.Items.Add("15:00");
            cbTime.Items.Add("16:00");
            cbTime.Items.Add("17:00");
            cbTime.Items.Add("18:00");
            cbTime.Items.Add("19:00");
        }
    }
}
