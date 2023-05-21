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
using System.Diagnostics;
using System.Security.Principal;
using System.Data.SqlTypes;
using System.Data.Common;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public UserProfile()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT OrderNumber, Users, RentalDate, ReturnDate, Days, RatePerDay, TotalAmount, Model, Mark, Year, Photo FROM Orders";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            UserProfileInfoOrderDataGrid.ItemsSource = registeredUsersTable.DefaultView;

            SqlConnection connection1 = new SqlConnection(connectionString);
            string query1 = "SELECT Advance,Users,RentalDate FROM Orders";
            SqlCommand command1 = new SqlCommand(query1, connection1);
            SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
            DataTable registeredUsersTable1 = new DataTable();
            adapter1.Fill(registeredUsersTable1);
            UserProfileInfoDepositsDataGrid.ItemsSource = registeredUsersTable1.DefaultView;

            SqlConnection connection2 = new SqlConnection(connectionString);
            string query2 = $"SELECT UserId, DocumentLink,DocumentName FROM DocumentInfo";
            SqlCommand command2 = new SqlCommand(query2, connection2);
            SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
            DataTable documentTable2 = new DataTable();
            adapter2.Fill(documentTable2);
            UserProfileInfoDocumentsDataGrid.ItemsSource = documentTable2.DefaultView;
        }

        private void btnRemove_click(object sender, RoutedEventArgs e)
        {
            if (UserProfileInfoOrderDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = UserProfileInfoOrderDataGrid.SelectedItem as DataRowView;
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
                    DataView dataView = (DataView)UserProfileInfoOrderDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);
                }
            }
        }

        private void btnDepositRemove_click(object sender, RoutedEventArgs e)
        {
            if (UserProfileInfoDepositsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = UserProfileInfoDepositsDataGrid.SelectedItem as DataRowView;
                    string query = "DELETE FROM Orders WHERE Users=@Users";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Users", selectedRow["Users"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)UserProfileInfoDepositsDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);

                }
            }
        }

        private void btnUpdate_click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = (DataRowView)UserProfileInfoOrderDataGrid.SelectedItem;
            if (rowView != null)
            {
                string users = rowView["Users"].ToString();
                DateTime rentalDate = (DateTime)rowView["RentalDate"];
                DateTime returnlDate = (DateTime)rowView["ReturnDate"];
                string days = rowView["Days"].ToString();
                string totalAmount = rowView["TotalAmount"].ToString();
                string rentPerDay = rowView["RatePerDay"].ToString();
                UpdateOrderForm updateOrderForm = new UpdateOrderForm(users, rentalDate, returnlDate, days, totalAmount, rentPerDay, rowView);
                updateOrderForm.ShowDialog();
            }
        }

        private void btnView_click(object sender, RoutedEventArgs e)
        {
            OrdersProfile ordersProfile = new OrdersProfile();
            AdminRentalCars adminRental = Window.GetWindow(this) as AdminRentalCars;
            adminRental.MainContent.Content = ordersProfile;

            DataRowView dataRowView = (DataRowView)UserProfileInfoOrderDataGrid.SelectedItem;
            if (dataRowView != null)
            {
                int Userid = (int)dataRowView["UserId"];
                string query = $"SELECT RentalDate,ReturnDate,Model,Mark,Photo,OrderNumber FROM Orders WHERE UserId = '{Userid}'";

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
            }
        }

        private void btnRemoveDocument_click(object sender, RoutedEventArgs e)
        {
            if (UserProfileInfoDocumentsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this operations?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = UserProfileInfoDocumentsDataGrid.SelectedItem as DataRowView;

                    string query = "DELETE FROM DocumentInfo WHERE DocumentName=@DocumentName";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DocumentName", selectedRow["DocumentName"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)UserProfileInfoDocumentsDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);
                }
            }
        }

        private void btnViewDocument_click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную строку в DataGrid
            DataRowView selectedRow = (DataRowView)UserProfileInfoDocumentsDataGrid.SelectedItem;

            // Получаем значение поля "DocumentLink" выделенной строки
            byte[] imageData = (byte[])selectedRow["DocumentLink"];

            // Создаем временный файл для изображения
            string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "F:\\c#\\RentACar\\RentACar\\Images\\mersedes.png");
            File.WriteAllBytes(tempFilePath, imageData);

            // Открываем изображение в стандартном приложении просмотра изображений Windows
            Process.Start(tempFilePath);
        }
        private void btn_AddDocument_click(object sender, RoutedEventArgs e)
        {
            DocumentForm form = new DocumentForm();
            form.ShowDialog();
        }

        private void textChangedSearch(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Получаем текст для поиска
                string searchText = txtSearch.Text;
                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM DocumentInfo WHERE DocumentName LIKE @txtSearch";
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
                UserProfileInfoDocumentsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }
}
