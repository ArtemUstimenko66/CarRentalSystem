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
using System.Windows.Shapes;
using System.Windows.Markup;
using RentACar.Controls;
using MaterialDesignThemes.Wpf;
using System.Runtime.ConstrainedExecution;
using System.Data.SqlTypes;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.IO;
using System.ComponentModel.Design;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Syncfusion.Windows.Shared;


namespace RentACar
{
    /// <summary>
    /// Interaction logic for UsersOrders.xaml
    /// </summary>
    public partial class UsersOrders : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Cars cars { get; set; }
        public Orders orders { get; set; }

        public List<Car> filteredCars = new List<Car>();
        public string SelectedCarClass { get; set; }
        public ObservableCollection<Car> Cars { get; set; }

        public List<Car> selectedCars = new List<Car>();
        public UsersOrders()
        {
            InitializeComponent();

            SqlConnection connection = new SqlConnection(connectionString);
            string query = $"SELECT Id,Users,Phone,Address,Gender,DateofBirth,Email,Password,Photo FROM RegisteredUsers";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable registeredUsersTable = new DataTable();
            adapter.Fill(registeredUsersTable);
            UsersDataGrid.ItemsSource = registeredUsersTable.DefaultView;
            orders = new Orders();
            cars = new Cars();
            Cars = new ObservableCollection<Car>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var commands = new SqlCommand("SELECT Mark, Model, Year, Photo, IsCheckedCar,IsAvailable,Availability,LicensePlate,RateDay,CarClass FROM CarData", conn);
                using (var reader = commands.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var car = new Car()
                        {
                            Mark = reader.GetString(0),
                            Model = reader.GetString(1),
                            Year = reader.GetInt32(2),
                            Photo = (byte[])reader["Photo"],
                            IsCheckedCar = reader.GetBoolean(4),
                            IsAvailable = reader.GetBoolean(5),
                            Availability = reader.GetString(6),
                            LicensePlate = reader.GetString(7),
                            RateDay = reader.GetInt32(8),
                            CarClass = reader.GetString(9)
                    };
                        Cars.Add(car);
                    }
                }
            }
            DataContext = this; 
        }
     

        private bool isChecked;
        private void CheckBox_Uncheked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            isChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           
            CheckBox checkBox = (CheckBox)sender;
            isChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;

            foreach (Car item in Cars)
            {
                if (item.IsCheckedCar)
                {
                    selectedCars.Add(item);
                }
            }

            if (selectedCars.Count > 0)
            {
                foreach (Car selectedCar in selectedCars)
                {
                    string carNumber = selectedCar.LicensePlate;
                    using (SqlConnection connectionsRate = new SqlConnection(connectionString))
                    {
                        connectionsRate.Open();
                        SqlCommand commandRateDay = new SqlCommand("SELECT RateDay FROM CarData WHERE LicensePlate = @LicensePlate", connectionsRate);
                        commandRateDay.Parameters.AddWithValue("@LicensePlate", carNumber);
                        object rateDay = commandRateDay.ExecuteScalar();
                        if (rateDay != null && !string.IsNullOrEmpty(rateDay.ToString()))
                        {
                            txtRateDate.Text = rateDay.ToString();
                        }
                    }
                }
            }
        }
        private void btn_SaveOrder_click(object sender, RoutedEventArgs e)
        {
            if (isChecked)
            {
                DataRowView selectedRow = (DataRowView)UsersDataGrid.SelectedItem;
                if (selectedRow != null)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string selectedUserId = selectedRow["Id"].ToString(); // получаем выбранный Id из таблицы RegisteredUsers
                        string selectedUserName = selectedRow["Users"].ToString(); // получаем имя пользователя
                        DateTime rentalDate = DateTime.Parse(txtRentalDate.Text);
                        DateTime returnDate = DateTime.Parse(txtReturnDate.Text);
                        string days = txtAmountOfDays.Text;
                        string totalAmount = txtTotalAmount.Text;
                        string rateDay = txtRateDate.Text;
                        string orderNumber = txtOrderNumber.Text;
                        string paymentType = cbPaymentType.SelectedItem.ToString();
                        string paymentMethod = cbPaymentMethod.SelectedItem.ToString();
                        string advance = txtAdvance.Text;
                        string debt = txtDebt.Text;

                        if (selectedCars.Count > 0)
                        {
                            foreach (Car selectedCar in selectedCars)
                            {
                                string mark = selectedCar.Mark;
                                string model = selectedCar.Model;
                                byte[] photo = selectedCar.Photo;
                                string insertSql = "INSERT INTO Orders (UserId, RentalDate, ReturnDate, Days, Mark, Model, Users, RatePerDay, TotalAmount, Photo, OrderNumber, PaymentType, PaymentMethod, Advance, Debt) VALUES (@userId, @rentalDate, @returnDate, @days, @mark, @model, @selectedUserName, @rateDay, @totalAmount, @photo, @orderNumber, @paymentType, @paymentMethod, @advance, @debt)";
                                SqlCommand insertCommand = new SqlCommand(insertSql, conn);
                                insertCommand.Parameters.AddWithValue("@userId", selectedUserId); // добавляем параметр UserId и присваиваем ему значение из переменной selectedUserId
                                insertCommand.Parameters.AddWithValue("@rentalDate", rentalDate);
                                insertCommand.Parameters.AddWithValue("@returnDate", returnDate);
                                insertCommand.Parameters.AddWithValue("@days", days);
                                insertCommand.Parameters.AddWithValue("@mark", mark);
                                insertCommand.Parameters.AddWithValue("@model", model);
                                insertCommand.Parameters.AddWithValue("@selectedUserName", selectedUserName);
                                insertCommand.Parameters.AddWithValue("@rateDay", rateDay);
                                insertCommand.Parameters.AddWithValue("@totalAmount", totalAmount);
                                insertCommand.Parameters.AddWithValue("@photo", photo);
                                insertCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                                insertCommand.Parameters.AddWithValue("@paymentType", paymentType);
                                insertCommand.Parameters.AddWithValue("@paymentMethod", paymentMethod);
                                insertCommand.Parameters.AddWithValue("@advance", advance);
                                insertCommand.Parameters.AddWithValue("@debt", debt);
                                int rowsAffected = insertCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // добавляем прибыль за день в базу данных
                                    string addProfitSql = "IF EXISTS(SELECT 1 FROM Profit WHERE Date = @date) " +
                                    "UPDATE Profit SET DailyProfit = DailyProfit + @dailyProfit WHERE Date = @date " +
                                    "ELSE " +
                                    "INSERT INTO Profit (Date, DailyProfit) VALUES (@date, @dailyProfit)";
                                    SqlCommand addProfitCommand = new SqlCommand(addProfitSql, conn);
                                    addProfitCommand.Parameters.AddWithValue("@date", rentalDate.Date);
                                    addProfitCommand.Parameters.AddWithValue("@dailyProfit", totalAmount);
                                    addProfitCommand.ExecuteNonQuery();

                                    Car selectedCarProfit = selectedCars.FirstOrDefault(c => c.Model == model);
                                    if (selectedCarProfit != null)
                                    {
                                        // получаем текущий CarProfit для выбранной машины
                                        string getCarProfitSql = $"SELECT CarProfit FROM CarData WHERE Model='{model}'";
                                        SqlCommand getCarProfitCommand = new SqlCommand(getCarProfitSql, conn);
                                        object result = getCarProfitCommand.ExecuteScalar();
                                        if (result != null && result != DBNull.Value)
                                        {
                                            int currentCarProfit = (int)result;

                                            // добавляем totalAmount заказа к текущему CarProfit
                                            int newCarProfit = currentCarProfit + int.Parse(totalAmount);

                                            // обновляем CarProfit в базе данных
                                            string updateCarProfitSql = $"UPDATE CarData SET CarProfit={newCarProfit} WHERE Model='{model}'";
                                            SqlCommand updateCarProfitCommand = new SqlCommand(updateCarProfitSql, conn);
                                            updateCarProfitCommand.ExecuteNonQuery();
                                        }
                                    }
                                    string updateSql = $"UPDATE CarData SET Availability='OnRent' WHERE Model='{model}'";
                                    SqlCommand updateCommand = new SqlCommand(updateSql, conn);
                                    updateCommand.ExecuteNonQuery();
                                    MessageBox.Show("Order added successfully!");
                                }
                                else
                                {
                                    MessageBox.Show("Please select a car!");
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a user!");
                }
            }
        }
        private void txtReturnDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateInterval();
        }

        private void txtRentalDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateInterval();
        }
        private void UpdateInterval()
        {
            DateTime startDate, endDate;
            if (DateTime.TryParse(txtRentalDate.Text, out startDate) && DateTime.TryParse(txtReturnDate.Text, out endDate))
            {
                TimeSpan interval = endDate - startDate;
                txtAmountOfDays.Text = interval.TotalDays.ToString();
            }
            else
            {
                txtAmountOfDays.Text = "";
            }
        }

        private void txtAmountOfDaysCng(object sender, TextChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void txtRateDayCng(object sender, TextChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            if (int.TryParse(txtAmountOfDays.Text, out int rentalDays) && int.TryParse(txtRateDate.Text, out int ratePerDay))
            {
                int totalAmount = rentalDays * ratePerDay;
                txtTotalAmount.Text = totalAmount.ToString();
            }
        }

        private void CalculateDebt()
        {
            double totalAmount = double.Parse(txtTotalAmount.Text);
            double advance = double.Parse(txtAdvance.Text);
            double debt = totalAmount - advance;
            txtDebt.Text = debt.ToString();
        }
        private void txtcngAdvance(object sender, TextChangedEventArgs e)
        {
            CalculateDebt();
        }
     

        private void CheckBox_CheckedEconomy(object sender, RoutedEventArgs e)
        {
            string carClass = cbc.Tag.ToString();
            filteredCars.Clear();
            foreach (Car car in Cars)
            {
                if (car.CarClass == null) continue; // пропускаем машины с null CarClass
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        private void CheckBox_UnchekedEconomy(object sender, RoutedEventArgs e)
        {
            string carClass = cbc.Tag.ToString();
            List<Car> filteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        //----------------------------------------------------------------------
        private void CheckBox_CheckedSUV(object sender, RoutedEventArgs e)
        {
            string carClass = cbc1.Tag.ToString();
            filteredCars.Clear();
            foreach (Car car in Cars)
            {
                if (car.CarClass == null) continue; // пропускаем машины с null CarClass
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }

        private void CheckBox_UnchekedSUV(object sender, RoutedEventArgs e)
        {
            string carClass = cbc1.Tag.ToString();
            List<Car> filteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        //----------------------------------------------------------------------
        private void CheckBox_CheckedBusiness(object sender, RoutedEventArgs e)
        {
            string carClass = cbc2.Tag.ToString();
            filteredCars.Clear();
            foreach (Car car in Cars)
            {
                if (car.CarClass == null) continue; // пропускаем машины с null CarClass
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }

        private void CheckBox_UnchekedBusiness(object sender, RoutedEventArgs e)
        {
            string carClass = cbc2.Tag.ToString();
            List<Car> filteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        //----------------------------------------------------------------------
        private void CheckBox_CheckedPremium(object sender, RoutedEventArgs e)
        {
            string carClass = cbc3.Tag.ToString();
            filteredCars.Clear();
            foreach (Car car in Cars)
            {
                if (car.CarClass == null) continue; 
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }

        private void CheckBox_UnchekedPremium(object sender, RoutedEventArgs e)
        {
            string carClass = cbc3.Tag.ToString();
            List<Car> filteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        //----------------------------------------------------------------------
        private void CheckBox_CheckedMinivan(object sender, RoutedEventArgs e)
        {
            string carClass = cbc4.Tag.ToString();
            filteredCars.Clear();
            foreach (Car car in Cars)
            {
                if (car.CarClass == null) continue;
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }

        private void CheckBox_UnchekedMinivan(object sender, RoutedEventArgs e)
        {
            string carClass = cbc4.Tag.ToString();
            List<Car> filteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.CarClass == carClass)
                {
                    filteredCars.Add(car);
                }
            }
            myItemsControl.ItemsSource = filteredCars;
        }
        //----------------------------------------------------------------------
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

        private void btn_close_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbPaymentType.Items.Add("Prepaid Expense");
            cbPaymentMethod.Items.Add("Cash");
            cbPaymentMethod.Items.Add("Bank");
        }
    }
}

 



