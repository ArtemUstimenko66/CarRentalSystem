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
using System.Collections.ObjectModel;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for Cars.xaml
    /// </summary>
    public partial class Cars : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private byte[] imageData;
        public ObservableCollection<Car> Carss { get; set; }
        public Cars()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT Id,Color,Mark,Model,Year,FuelType,LicensePlate,NumberOfSeats,Mileage,Availability,Photo,RateDay,Transmission,DailyContent,CarClass FROM CarData";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            CarsDataGrid.ItemsSource = registeredUsersTable.DefaultView;
        }

        private void btn_addCar_click(object sender, RoutedEventArgs e)
        {
            AddCarForm addCarForm = new AddCarForm();
            addCarForm.ShowDialog();
        }

        private void textChangedSearch(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();

                // Получаем текст для поиска
                string searchText = txtSearch.Text;

                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM CarData WHERE Mark LIKE @txtSearch OR Model LIKE @txtSearch OR Year LIKE @txtSearch OR Mileage LIKE @txtSearch";

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

                CarsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void btnUpdate_click(object sender, RoutedEventArgs e)
        {
            CarProfile carProfile = new CarProfile();
           
            DataRowView rowView = (DataRowView)CarsDataGrid.SelectedItem;
            if (rowView != null)
            {
                string color = rowView["Color"].ToString();
                string mark = rowView["Mark"].ToString();
                string model = rowView["Model"].ToString();
                string year = rowView["Year"].ToString();
                string fueltype = rowView["FuelType"].ToString();
                string licenseplate = rowView["LicensePlate"].ToString();
                string numberofseats = rowView["NumberOfSeats"].ToString();
                string mileage = rowView["Mileage"].ToString();
                string availability = rowView["Availability"].ToString();
                string transmission = rowView["Transmission"].ToString();
                string rateperday = rowView["RateDay"].ToString();
                string carclass = rowView["CarClass"].ToString();
                byte[] photoBytes = (byte[])rowView["Photo"];
                if (photoBytes != null && photoBytes.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(photoBytes))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        carProfile.myImage.Background = new ImageBrush(bitmap);
                    }
                }
                UpdateCarForm updateUserForm = new UpdateCarForm(color, mark, model, year, fueltype, licenseplate, numberofseats, mileage, rowView, availability,photoBytes,transmission,rateperday,carclass);
                updateUserForm.ShowDialog();
            }
        }

        private void btnRemove_click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem != null) // проверяем, что выбрана запись
            {
                if (MessageBox.Show("Are you sure you want to delete this car?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = CarsDataGrid.SelectedItem as DataRowView; // получаем выбранную запись

                    // удаляем запись из базы данных

                    string query = "DELETE FROM CarData WHERE Model=@Model";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Mark", selectedRow["Mark"]);
                        command.Parameters.AddWithValue("@Model", selectedRow["Model"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)CarsDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);

                }
            }
        }

        private void btnView_click(object sender, RoutedEventArgs e)
        {
            CarProfile carProfile = new CarProfile();
            AdminRentalCars adminRental = Window.GetWindow(this) as AdminRentalCars;
            adminRental.MainContent.Content = carProfile;

            DataRowView dataRowView = (DataRowView)CarsDataGrid.SelectedItem;
            if (dataRowView != null)
            {
                int id = (int)dataRowView["id"];
                string query = $"SELECT Mark,Model,Color,FuelType,Photo,Mileage,Year,CarClass FROM CarData WHERE ID = {id}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand selectCommand = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        carProfile.txtMark.Text = "Mark:" + reader.GetString(0);
                        carProfile.txtModel.Text = "Model:" + reader.GetString(1);
                        carProfile.txtColor.Text = "Color:" + " " + reader.GetString(2);
                        carProfile.txtFuelType.Text = "FuelType:" + reader.GetString(3);
                        carProfile.txtMileage.Text = "Mileage:" + reader.GetDouble(5);
                        carProfile.txtYear.Text = "Year:" + reader.GetInt32(6);
                        carProfile.txtCarClass.Text = "CarClass:" + reader.GetString(7);
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
                                carProfile.myImage.Background = new ImageBrush(bitmap);
                            }
                        }
                    }
                }
            }
        }
    }
}
