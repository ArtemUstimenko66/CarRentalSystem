using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
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

        private void btnRgister_click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string user = txtUsername.Text;
            string email = txtEmail.Text;
            string password = new System.Net.NetworkCredential(string.Empty, txtPassword.Password).Password;
            string confirmPassword = new System.Net.NetworkCredential(string.Empty, txtResetPassword.Password).Password;
            Random rnd = new Random();
            int code = rnd.Next(1000, 10000);
            string to, from, pass, mail;
            to = txtEmail.Text;
            from = "artemmmm5454545@gmail.com";
            mail = code.ToString();
            pass = "hxqdvwocoahxwfuq";
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = mail;
            message.Subject = "Your app name - Vefefication Code";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);
                MessageBox.Show("We have sent you a confirmation code by email!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password mismatch!");
                return;
            }
            int userType = cbBoxChoice.SelectedIndex;
            var confirmationWindow = new ConfirmationWindow(code);
            if (userType == 0)
            {
                if (confirmationWindow.ShowDialog() == true)
                {
                    string insertSql = $"INSERT INTO RegisteredUsers (Users,Password,Email,UserTypeUsers) VALUES ('{user}',{password},'{email}','{userType}')";

                    // Создаем объект подключения к базе данных и команду для выполнения SQL-запроса
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand insertCommand = new SqlCommand(insertSql, connection);

                        // Открываем соединение с базой данных и выполняем команду INSERT
                        connection.Open();
                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User registered successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to register user!");
                        }
                    }
                }
            }
            //---------------------------------
            else if (userType == 1)
            {

                if (confirmationWindow.ShowDialog() == true)
                {
                    string insertSql = $"INSERT INTO RegisteredAdmins (Adminname, Password,Email,UserTypeAdmins) VALUES ('{user}','{password}','{email}','{userType}')";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand insertCommand = new SqlCommand(insertSql, connection);
                        connection.Open();
                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Administrator successfully registered!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to register administrator!");
                        }
                        this.Close();
                    }
                }
            }
           Login login = new Login();
           login.ShowDialog();
        }
    }
}




