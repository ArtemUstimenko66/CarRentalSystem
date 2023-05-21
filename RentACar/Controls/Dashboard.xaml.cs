using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Data.SqlClient;
using LiveCharts.Defaults;
using System.Security.Cryptography.X509Certificates;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Definitions.Charts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ObservableCollection<Car> Cars { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            CountCars();
           CalculateProfitCars();

            //------------------------
            int usercount = 0;
            Users usersForm = new Users();
            usercount = usersForm.UsersDataGrid.Items.Count;
            card1.Number = usercount.ToString();
            //---------------
            int carcount = 0;
            Cars cars = new Cars();
            carcount = cars.CarsDataGrid.Items.Count;
            card2.Number = carcount.ToString();
            //---------------
            int orderscount = 0;
            Orders orders = new Orders();
            orderscount = orders.OrdersDataGrid.Items.Count;
            card3.Number = orderscount.ToString();

            Cars = new ObservableCollection<Car>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var commands = new SqlCommand("SELECT Mark, Model, Year, Photo FROM Orders", conn);
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
                        };
                        Cars.Add(car);
                    }
                }
            }
            DataContext = this;
        }

        private void CountCars()
        {
            List<Tuple<string, int>> carRentals = new List<Tuple<string, int>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Model, Mark, COUNT(*) as CountCars FROM Orders GROUP BY Model, Mark ORDER BY CountCars DESC";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string carName = reader["Mark"].ToString();
                    int totalRentals = Convert.ToInt32(reader["CountCars"]);
                    carRentals.Add(new Tuple<string, int>(carName, totalRentals));
                }
            }
            carRentals.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            var top3CarRentals = carRentals.Take(3).ToList();

            foreach (var carRental in top3CarRentals)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Title = carRental.Item1,
                    Values = new ChartValues<int> { carRental.Item2 }
                });
            }
        }

        private void CalculateProfitCars()
        {
            List<Tuple<string, int>> carProfits = new List<Tuple<string, int>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Mark, CarProfit FROM CarData";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string carModel = reader["Mark"].ToString();
                    int carProfit = Convert.ToInt32(reader["CarProfit"]);
                    carProfits.Add(new Tuple<string, int>(carModel, carProfit));
                }
            }
            carProfits.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            var top3CarProfits = carProfits.Take(3).ToList();

            // Установка культуры для использования доллара
            CultureInfo culture = new CultureInfo("en-US"); // Используем культуру en-US для доллара
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            foreach (var carProfit in top3CarProfits)
            {
                var series = new PieSeries
                {
                    Title = carProfit.Item1,
                    Values = new ChartValues<int> { carProfit.Item2 },
                    DataLabels = true
                };

                series.LabelPoint = chartPoint => string.Format("{0:C}", chartPoint.Y);

                pieChartSecond.Series.Add(series);
            }
        }
    }
}
