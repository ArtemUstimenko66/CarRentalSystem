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
using System.Windows.Shapes;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for CarCondition.xaml
    /// </summary>
    public partial class CarCondition : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public CarCondition()
        {
            InitializeComponent();
        }
        private void btn_AddCarCondition_click(object sender, RoutedEventArgs e)
        {
            DateTime dateOfDamage = DateTime.Parse(txtDamageDate.Text);
            string carCondition = cbCarCondition.SelectedItem.ToString();
            string damageDescription = txtDamageDescription.Text;
            int amountForDamage = int.Parse(txtAmountOfDamage.Text);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = $"INSERT INTO CarCondition (DateOfDamage,CarConditions,DamageDescription,AmountForDamage) VALUES (@dateOfDamage,@carConditions,@damageDescription,@amountOfDamage)";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@dateOfDamage", dateOfDamage);
                command.Parameters.AddWithValue("@carConditions", carCondition);
                command.Parameters.AddWithValue("@damageDescription", damageDescription);
                command.Parameters.AddWithValue("@amountOfDamage", amountForDamage);
              //command.Parameters.AddWithValue("@orderId", OrderId);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Adding damage conditions completed successfully!");
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbCarCondition.Items.Add("Excellent");
            cbCarCondition.Items.Add("Satisfactory");
            cbCarCondition.Items.Add("Bad");
        }
    }
}
