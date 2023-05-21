
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
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
namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Users()
        {
            
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Id, Users,Phone,Address,Gender,DateofBirth,Email,Password,Photo FROM RegisteredUsers";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            UsersDataGrid.ItemsSource = registeredUsersTable.DefaultView;
        }

        private void btnRemove_click(object sender, RoutedEventArgs e)
        {

            if (UsersDataGrid.SelectedItem != null) // проверяем, что выбрана запись
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = UsersDataGrid.SelectedItem as DataRowView; // получаем выбранную запись

                    // удаляем запись из базы данных

                    string query = "DELETE FROM RegisteredUsers WHERE Users=@Users";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Users", selectedRow["Users"]);
                        command.Parameters.AddWithValue("@Email", selectedRow["Email"]);
                        command.Parameters.AddWithValue("@Password", selectedRow["Password"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)UsersDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);

                }
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
                string query = "SELECT * FROM RegisteredUsers WHERE Users LIKE @txtSearch OR Email LIKE @txtSearch";

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

                UsersDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void btn_addUser_click(object sender, RoutedEventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.ShowDialog();

        }

        private void btnUpdate_click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = (DataRowView)UsersDataGrid.SelectedItem;
            
            if (rowView != null)
            {
                string userLogin = rowView["Users"].ToString();
                string email = rowView["Email"].ToString();
                string phone = rowView["Phone"].ToString();
                string address = rowView["Address"].ToString();
                string gender = rowView["Gender"].ToString();
                string dateofbirth = rowView["DateofBirth"].ToString();
                byte[] photoBytes = (byte[])rowView["Photo"];
               
                UpdateUserForm updateUserForm = new UpdateUserForm(userLogin, email, phone, address, gender, dateofbirth, rowView,photoBytes);
                updateUserForm.ShowDialog();
            }

        }

        private void btnView_click(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = new UserProfile();
            AdminRentalCars adminRental = Window.GetWindow(this) as AdminRentalCars;
            adminRental.MainContent.Content = userProfile;
            DataRowView dataRowView = (DataRowView)UsersDataGrid.SelectedItem;
          
            if (dataRowView != null)
            {
               int CurrentUserId = (int)dataRowView["Id"];
                string query = $"SELECT Id, Users, Email, Phone, Address, Photo FROM RegisteredUsers WHERE Id = {CurrentUserId}";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile.txtUser.Text = reader.GetString(1);
                        userProfile.txtEmail.Text = reader.GetString(2);
                        userProfile.txtPhone.Text = reader.GetString(3);
                        userProfile.txtLocation.Text = reader.GetString(4);
                        try
                        {
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
                                    userProfile.myImage.Background = new ImageBrush(bitmap);
                                }
                            }
                        }
                        catch (Exception)
                        {}
                    }
                }
                        
                query = $"SELECT Advance,UserId, Users, OrderNumber, RentalDate, ReturnDate, Days, RatePerDay, TotalAmount, Model, Mark, Year, Photo FROM Orders WHERE UserId = {CurrentUserId}";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    DataTable ordersTable = new DataTable();
                    ordersTable.Load(reader);
                    userProfile.UserProfileInfoOrderDataGrid.ItemsSource = ordersTable.DefaultView;
                    userProfile.UserProfileInfoDepositsDataGrid.ItemsSource = ordersTable.DefaultView;
                }
                string query1 = $"SELECT DocumentLink,DocumentName FROM DocumentInfo WHERE UserId = {CurrentUserId}";
                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand1 = new SqlCommand(query1, connection1);
                    connection1.Open();
                    SqlDataReader reader1 = selectCommand1.ExecuteReader();
                    DataTable ordersTable1 = new DataTable();
                    ordersTable1.Load(reader1);
                    userProfile.UserProfileInfoDocumentsDataGrid.ItemsSource = ordersTable1.DefaultView;
                }
            }
        }
    }
}

