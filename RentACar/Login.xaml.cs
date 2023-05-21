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
using MaterialDesignThemes.Wpf;
using RentACar.Controls;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper palette = new PaletteHelper();
       

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = palette.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            palette.SetTheme(theme);
        }

        private void btn_registerOpenForm_click(object sender, RoutedEventArgs e)
        {
            var registrationForm = new Register();
            registrationForm.Show();
            this.Hide();

        }

        private void btn_Login_click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string user = txtUsername.Text;
            string password = new System.Net.NetworkCredential(string.Empty, txtPassword.Password).Password;
            int userType = cbBoxChoice.SelectedIndex;

            string selectSqlUser = "";
            string selectSqlAdmin = "";
            if (userType == 0)
            {
                selectSqlUser = $"SELECT * FROM RegisteredUsers WHERE Users = '{user}' AND PASSWORD = '{password}'";
            }
            else if (userType == 1)
            {
                selectSqlAdmin = $"SELECT * FROM RegisteredAdmins WHERE Adminname = '{user}' AND PASSWORD = '{password}'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (!string.IsNullOrEmpty(selectSqlUser))
                {
                    SqlCommand selectCommand = new SqlCommand(selectSqlUser, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MessageBox.Show("User authorization was successful!");
                        AdminRentalCars adminRentalCars = new AdminRentalCars(user, reader.GetInt32(0));
                        adminRentalCars.Show();
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("User authorization error. Check your login and password.");
                    }
                }

                if (!string.IsNullOrEmpty(selectSqlAdmin))
                {
                    SqlCommand selectCommand = new SqlCommand(selectSqlAdmin, connection);
                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        MessageBox.Show("Administrator authorization was successful!");
                        AdminRentalCars adminRentalCars = new AdminRentalCars(user, reader.GetInt32(0));
                        adminRentalCars.LoggedInUsername = user;
                        adminRentalCars.Show();
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Administrator authorization error. Please check your login and password.");
                    }
                }
            }
        }
    }
}
