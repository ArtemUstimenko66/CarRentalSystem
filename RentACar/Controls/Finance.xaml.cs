using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using LiveCharts.Definitions.Charts;
using System.Globalization;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Base;
using System.Data;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for Finance.xaml
    /// </summary>
    public partial class Finance : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Finance()
        {
            InitializeComponent();
            CalculateDailyProfit();
            CalculateDailyLoss();
            CalculateProfitAndLoss();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CalculateDailyProfit()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(TotalAmount) FROM Orders WHERE RentalDate = @rentalDate";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rentalDate", DateTime.Today);

                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        card1.Number = $"{result:C}";
                    }
                    else
                    {
                        card1.Number = "No profit per day";
                    }
                }
            }
        }
        private void CalculateDailyLoss()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(DailyContent) FROM CarData";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        int totalExpenses = Convert.ToInt32(result);
                        card2.Number = $"{totalExpenses:C}";
                    }
                    else
                    {
                        card2.Number = "No losses per day";
                    }
                }
            }
        }

        private void CalculateProfitAndLoss()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(TotalAmount) FROM Orders WHERE RentalDate = @rentalDate";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rentalDate", DateTime.Today);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        double totalRevenue = Convert.ToDouble(result);
                        string query1 = "SELECT SUM(DailyContent) FROM CarData";
                        using (SqlCommand command2 = new SqlCommand(query1, connection))
                        {
                            result = command2.ExecuteScalar();
                            if (result != DBNull.Value)
                            {
                                double totalExpenses = Convert.ToDouble(result);
                                double dailyProfit = totalRevenue - totalExpenses;
                                card3.Number = $"{dailyProfit:C}";

                                // Update the TotalProfit field in the Orders table
                                string query3 = "UPDATE Orders SET TotalProfit = @totalProfit WHERE RentalDate = @rentalDate";
                                using (SqlCommand command3 = new SqlCommand(query3, connection))
                                {
                                    command3.Parameters.AddWithValue("@totalProfit", dailyProfit);
                                    command3.Parameters.AddWithValue("@rentalDate", DateTime.Today);
                                    command3.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                card3.Number = "No data on total expenses per day";
                            }
                        }
                    }
                }
            }
        }

        private void btn_showGraphic(object sender, RoutedEventArgs e)
        {
            DateTime startDate = datePickerFirst.SelectedDate.Value;
            DateTime endDate = datePickerLast.SelectedDate.Value;
            string sqlQuery = "SELECT SUM(DailyProfit) as DailyProfit, Date FROM Profit WHERE Date BETWEEN @StartDate AND @EndDate GROUP BY Date";

          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Добавляем параметры для запроса
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                  
                    connection.Open();

                    // Выполняем запрос и получаем результат в виде набора данных
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Создаем объект для хранения данных графика
                        var chartValues = new ChartValues<double>();

                        // Создаем объект для хранения дат
                        var dates = new ChartValues<DateTime>();

                        // Читаем данные из набора данных и добавляем их в объекты для хранения данных графика и дат
                        while (reader.Read())
                        {
                            chartValues.Add(reader.GetInt32(0));
                            dates.Add(reader.GetDateTime(1));
                        }

                        // Получаем выбранный пользователем месяц
                        // Получаем выбранный пользователем месяц
                        string selectedMonth = startDate.ToString("MMMM yyyy");
                        CultureInfo culture = new CultureInfo("en-US"); // Используем культуру en-US для доллара
                        CultureInfo.CurrentCulture = culture;
                        CultureInfo.CurrentUICulture = culture;
                        // Используем библиотеку LiveCharts для отображения полученных данных на графике
                        cartesianChart.Series = new SeriesCollection
{
    new LineSeries
    {
        Title = "TotalProfit",
        Values = chartValues,
        LabelPoint = point => $"Profit: {point.Y.ToString("C2")}\nDate: {dates[(int)point.X].ToString("dd.MM.yyyy")}"
    }
};
                        // Выводим выбранный пользователем месяц под графиком
                        selectedMonthTextBlock.Text = $"{selectedMonth}";
                        
                    }
                   

                }
            }
        }
    }
}



