using System;
using System.Collections.Generic;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for UpdateOrderForm.xaml
    /// </summary>
    public partial class UpdateOrderForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string users;
        private DateTime rentalDate;
        private DateTime returnDate;
        private string days;
        private string rateperday;
        private string totalamount;
        private DataRowView RowView;
        public UpdateOrderForm(string users,DateTime rentalDate,DateTime returnDate,string days,string rateperday,string totalamount, DataRowView rowView)
        {
            InitializeComponent();
            this.users = users;
            this.rentalDate = rentalDate;
            this.returnDate = returnDate;
            this.days = days;
            this.rateperday = rateperday;
            this.totalamount = totalamount;
            this.RowView = rowView;
            txtUserName.Text = users;
            txtRentalDate.Text = rentalDate.ToString("dd.MM.yyyy");
            txtReturnDate.Text = returnDate.ToString("dd.MM.yyyy");
            txtAmountOfDays.Text = days;
            txtTotalAmount.Text = rateperday;
            txtRatePerDay.Text = totalamount;
        }

        private void btn_UpdateOrderForm_click(object sender, RoutedEventArgs e)
        {
            string Newusers = txtUserName.Text;
            DateTime NewrentalDate = DateTime.ParseExact(txtRentalDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime NewreturnDate = DateTime.ParseExact(txtReturnDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string Newdays = txtAmountOfDays.Text;
            string Newrateperday = txtRatePerDay.Text;
            string NewTotalAmount = txtTotalAmount.Text;


            string query = $"UPDATE Orders SET Users='{Newusers}',RentalDate='{NewrentalDate}',ReturnDate='{NewreturnDate}',Days='{Newdays}',RatePerDay='{Newrateperday}',TotalAmount='{NewTotalAmount}' WHERE Id IN (SELECT TOP 1 Id FROM Orders ORDER BY NEWID())";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                connection.Open();
                int rows = sqlCommand.ExecuteNonQuery();
                try
                {
                    if (rows > 0)
                    {
                        RowView["Users"] = Newusers;
                        RowView["RentalDate"] = NewrentalDate;
                        RowView["ReturnDate"] = NewreturnDate;
                        RowView["Days"] = Newdays;
                        RowView["RatePerDay"] = Newrateperday;
                        RowView["TotalAmount"] = NewTotalAmount;

                        MessageBox.Show("Данные машины успешно обновлены.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить данные машины.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        private void txtRentalDateTextCh(object sender, TextChangedEventArgs e)
        {
            UpdateInterval();
        }

        private void txtReturnDateTextCh(object sender, TextChangedEventArgs e)
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

        private void txtTotalAmountCh(object sender, TextChangedEventArgs e)
        {
            CalculateTotalAmount();
        }

        private void txtRatePerDayCh(object sender, TextChangedEventArgs e)
        {
            CalculateTotalAmount();
        }
        private void CalculateTotalAmount()
        {
            if (int.TryParse(txtAmountOfDays.Text, out int rentalDays) && int.TryParse(txtRatePerDay.Text, out int ratePerDay))
            {
                int totalAmount = rentalDays * ratePerDay;
                txtTotalAmount.Text = totalAmount.ToString();
            }
        }
    }
}
