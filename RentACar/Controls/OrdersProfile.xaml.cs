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

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for OrdersProfile.xaml
    /// </summary>
    public partial class OrdersProfile : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public OrdersProfile()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT ReturnDateCar,LicensePlate,OrderNumber,CarReturnTime FROM ReturnCar";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            ReturnCarsDataGrid.ItemsSource = registeredUsersTable.DefaultView;

            SqlConnection connection1 = new SqlConnection(connectionString);
            string query1 = $"SELECT Id, PaymentType,PaymentMethod,Advance,PaymentDate,DebtAmount,IsPaymentCompleted FROM PaymentInfo";
            SqlCommand command1 = new SqlCommand(query1, connection1);
            SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
            DataTable registeredUsersTable1 = new DataTable();
            adapter1.Fill(registeredUsersTable1);
            CarPaymentsPaymentsInfoDataGrid.ItemsSource = registeredUsersTable1.DefaultView;

            SqlConnection connection2 = new SqlConnection(connectionString);
            string query2 = "SELECT DateOfDamage, CarConditions,DamageDescription,AmountForDamage FROM CarCondition";
            SqlCommand command2 = new SqlCommand(query2, connection2);
            SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
            DataTable registeredUsersTable2 = new DataTable();
            adapter2.Fill(registeredUsersTable2);
            CarConditionInfoDataGrid.ItemsSource = registeredUsersTable2.DefaultView;

        }

        private void btnRemoveReturnCar_click(object sender, RoutedEventArgs e)
        {
            if (ReturnCarsDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this operations?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = ReturnCarsDataGrid.SelectedItem as DataRowView;

                    string query = "DELETE FROM ReturnCar WHERE ReturnDateCar=@ReturnDateCar";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReturnDateCar", selectedRow["ReturnDateCar"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)ReturnCarsDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);
                }
            }
        }
        private void textChangedSearch(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Получаем текст для поиска
                string searchText = txtSearch.Text;
                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM ReturnCar WHERE OrderNumber LIKE @txtSearch";
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
                ReturnCarsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void btn_ReturnTheCar_click(object sender, RoutedEventArgs e)
        {
            bool isPaymentMade = true;
            bool isCarCondition = true;
            ReturnDetailsForm returnDetailsForm = new ReturnDetailsForm();

            if (CarPaymentsPaymentsInfoDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("To return the car, you need to make a payment.");
                return;
            }

            if (CarConditionInfoDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("To return a car, you must add conditions for the state of the car!");
                return;
            }

            if (isPaymentMade && isCarCondition)
            {
                btnReturnTheCar.IsEnabled = false;
                returnDetailsForm.Show();
            }
        }

        private void btnRemovePaymentForRent_click(object sender, RoutedEventArgs e)
        {
            if (CarPaymentsPaymentsInfoDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this operations?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = CarPaymentsPaymentsInfoDataGrid.SelectedItem as DataRowView;

                    string query = "DELETE FROM PaymentInfo WHERE DebtAmount=@DebtAmount";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DebtAmount", selectedRow["DebtAmount"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)CarPaymentsPaymentsInfoDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);
                }
            }
        }

        private void textChangedSearchPayments(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Получаем текст для поиска
                string searchText = txtSearchPayments.Text;
                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM PaymentInfo WHERE DebtAmount LIKE @txtSearchPayments";
                // Создаем объект DataTable и заполняем его данными из базы данных
                DataTable dataTable = new DataTable();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметры к запросу для безопасного использования параметров
                    command.Parameters.AddWithValue("@txtSearchPayments", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                // Устанавливаем свойство ItemsSource элемента DataGrid на DataTable
                CarPaymentsPaymentsInfoDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
        private void btn_PayOffTheRent_click(object sender, RoutedEventArgs e)
        {
            bool isCondition = true;
            CarPaymentForm carPaymentForm = new CarPaymentForm();
            if (CarConditionInfoDataGrid.Items.Count <= 0)
            {
                MessageBox.Show("For payment, you must add the conditions of the state of the car");
                return;
            }
            if (isCondition)
            {
                btnCarCondition.IsEnabled = false;
                carPaymentForm.Show();
            }
        }

        private void btnRemoveCarCondition_click(object sender, RoutedEventArgs e)
        {
            if (CarConditionInfoDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this operations?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataRowView selectedRow = CarConditionInfoDataGrid.SelectedItem as DataRowView;

                    string query = "DELETE FROM CarCondition WHERE AmountForDamage=@AmountForDamage";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AmountForDamage", selectedRow["AmountForDamage"]);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    DataView dataView = (DataView)CarConditionInfoDataGrid.ItemsSource;
                    DataTable dataTable = dataView.Table;
                    dataTable.Rows.Remove(selectedRow.Row);
                }
            }
        }

        private void textChangedSearchCarCondition(object sender, TextChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Получаем текст для поиска
                string searchText = txtSearchCarCondtion.Text;
                // Создаем запрос на получение данных из таблицы RegisteredUsers
                string query = "SELECT * FROM CarCondition WHERE AmountForDamage LIKE @txtSearchCarCondtion";
                // Создаем объект DataTable и заполняем его данными из базы данных
                DataTable dataTable = new DataTable();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметры к запросу для безопасного использования параметров
                    command.Parameters.AddWithValue("@txtSearchCarCondtion", "%" + searchText + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                // Устанавливаем свойство ItemsSource элемента DataGrid на DataTable
                CarConditionInfoDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void btn_CarCondition_click(object sender, RoutedEventArgs e)
        {
            CarCondition carCondition = new CarCondition();
            carCondition.Show();
        }
    }
}
