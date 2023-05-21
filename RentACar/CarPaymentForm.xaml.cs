using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for CarPaymentForm.xaml
    /// </summary>
    public partial class CarPaymentForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private int totalAmount;
      private int Orderid = 12033;
        public CarPaymentForm()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT TotalAmount,Debt,Advance FROM [Orders]";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    totalAmount = 440;
                    int paid = 350;
                    int debt = 90;
                    txtBlockOrderPrice.Text = totalAmount.ToString();
                    txtBlockPaid.Text = paid.ToString();
                    txtBlockDebtAmount.Text = debt.ToString();
                }
                reader.Close();
            }
        }

        private void btn_CarPay_click(object sender, RoutedEventArgs e)
        {
            string paymentType = cbPaymentType.SelectedItem.ToString();
            string paymentMethod = cbPaymentMethod.SelectedItem.ToString();
            DateTime paymentDate = DateTime.Parse(txtPaymentDate.Text);
            int debtAmount = int.Parse(txtDebtAmount.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "INSERT INTO PaymentInfo (PaymentType,PaymentMethod,PaymentDate,DebtAmount,Orderid) VALUES (@paymentType,@paymentMethod,@paymentDate,@debtAmount,@Orderid)";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@paymentType", paymentType);
                command.Parameters.AddWithValue("@paymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@paymentDate", paymentDate);
                command.Parameters.AddWithValue("@debtAmount", debtAmount);
                command.Parameters.AddWithValue("@OrderId", Orderid);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Payment completed successfully!");
                }
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbPaymentType.Items.Add("Prepaid Expense");
            cbPaymentMethod.Items.Add("Cash");
            cbPaymentMethod.Items.Add("Bank");
        }
    }
}
