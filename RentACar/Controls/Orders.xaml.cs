using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.IO;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public int AmountForDamage;

        public Orders()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Id,UserId,OrderNumber,Users,RentalDate,ReturnDate,Days,RatePerDay,TotalAmount,Model,Mark,Year,Photo FROM Orders";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            OrdersDataGrid.ItemsSource = registeredUsersTable.DefaultView;
        }

        private void btnUpdate_click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = (DataRowView)OrdersDataGrid.SelectedItem;
            if (rowView != null)
            {
                string users = rowView["Users"].ToString();
                DateTime rentalDate = (DateTime)rowView["RentalDate"];
                DateTime returnlDate = (DateTime)rowView["ReturnDate"];
                string days = rowView["Days"].ToString();
                string totalAmount = rowView["TotalAmount"].ToString();
                string rentPerDay = rowView["RatePerDay"].ToString();
                UpdateOrderForm updateOrderForm = new UpdateOrderForm(users,rentalDate,returnlDate,days,totalAmount,rentPerDay,rowView);
                updateOrderForm.ShowDialog();
            }
        }

        private void btnRemove_click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null) 
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = OrdersDataGrid.SelectedItem as DataRowView; 
                    string query = "DELETE FROM Orders WHERE Users=@Users";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Users", selectedRow["Users"]);
                        command.Parameters.AddWithValue("@Mark", selectedRow["Mark"]);
                        command.Parameters.AddWithValue("@Model", selectedRow["Model"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)OrdersDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);

                }
            }
        }

        private void btnView_click(object sender, RoutedEventArgs e)
        {
            OrdersProfile ordersProfile = new OrdersProfile();
            AdminRentalCars adminRental = Window.GetWindow(this) as AdminRentalCars;
            adminRental.MainContent.Content = ordersProfile;

            DataRowView dataRowView = (DataRowView)OrdersDataGrid.SelectedItem;
            if (dataRowView != null)
            {
                int Userid = (int)dataRowView["UserId"];
                int OrderId = (int)dataRowView["Id"];
                string query = $"SELECT RentalDate,ReturnDate,Model,Mark,Photo,OrderNumber,Id FROM Orders WHERE UserId = '{Userid}'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        ordersProfile.txtRentalDate.Text = "Rental Date:" + " " + reader.GetDateTime(0);
                        ordersProfile.txtReturnDate.Text = "Return Date:" + reader.GetDateTime(1);
                        ordersProfile.txtMark.Text = "Mark:" + reader.GetString(2);
                        ordersProfile.txtModel.Text = "Model:" + reader.GetString(3);
                        ordersProfile.txtOrderNumber.Text = "Order Number" + reader.GetString(5);
                        byte[] photoBytes = (byte[])reader["Photo"];
                        if (photoBytes != null && photoBytes.Length > 0)
                        {
                            using (MemoryStream stream = new MemoryStream(photoBytes))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = stream;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                ordersProfile.myImage.Background = new ImageBrush(bitmap);
                            }
                        }
                    }
                }
               /* query = $"SELECT DateOfDamage,CarConditions,DamageDescription,AmountForDamage,OrderId FROM CarCondition WHERE OrderId = {OrderId}";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    DataTable ordersTable = new DataTable();
                    ordersTable.Load(reader);
                    ordersProfile.CarConditionInfoDataGrid.ItemsSource = ordersTable.DefaultView;  
                }*/
            }
        }


        private void textChangedSearch(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем подключение
                connection.Open();

                // Получаем текст для поиска
                string searchText = txtSearch.Text;

                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM Orders WHERE Users LIKE @txtSearch OR Mark LIKE @txtSearch OR Model LIKE @txtSearch";

                // Создаем объект DataTable и заполняем его данными из базы данных
                DataTable dataTable = new DataTable();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметры к запросу для безопасного использования параметров
                    command.Parameters.AddWithValue("@txtSearch", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }

                // Устанавливаем свойство ItemsSource элемента DataGrid на DataTable

                OrdersDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void btn_AddOrder_click(object sender, RoutedEventArgs e)
        {
            UsersOrders usersOrders = new UsersOrders();
            usersOrders.Show();
        }
    }
}

