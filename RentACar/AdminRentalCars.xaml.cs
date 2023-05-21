
using MahApps.Metro.Controls;
using RentACar.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for AdminRentalCars.xaml
    /// </summary>
    public partial class AdminRentalCars : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string _loggedInUsername;
        private int adminId;
        public AdminRentalCars(string loggedInUsername, int adminId)
        {
            InitializeComponent();
            _loggedInUsername = loggedInUsername;
            this.adminId = adminId;
            GetAdminPhoto();
            var dashboard = new Dashboard();
            MainContent.Content = dashboard;
        }


        public void GetAdminPhoto()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Adminname, Password, Photo FROM RegisteredAdmins WHERE Id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", adminId);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            try
                            {
                                if (reader.IsDBNull(reader.GetOrdinal("Photo")))
                                {
                                    // Если у пользователя нет фото, устанавливаем фото по умолчанию
                                    BitmapImage defaultImage = new BitmapImage(new Uri("Images/user.png"));
                                    myImage.Background = new ImageBrush(defaultImage);
                                }
                                else
                                {
                                    // Иначе, устанавливаем фото из базы данных
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
                                            myImage.Background = new ImageBrush(bitmap);
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
            }
        }

        public string LoggedInUsername
        {
            get { return _loggedInUsername; }
            set
            {
                _loggedInUsername = value;
                txtBlockAdmin.Text = value;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool isMax = false;

        public string UserName { get; internal set; }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(isMax)
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
        private void btnLogoutClick_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnUsersClick_click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Users();
        }

        private void btnCarsClick_click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Cars();
        }

        private void btnOrdersClick_click(object sender, RoutedEventArgs e)
        {
      
            MainContent.Content = new Orders();
        }

        private void btnDashboardClick_click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Dashboard();
         
        }

        private void btnFinanceClick_click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Finance();
        }
        private void btnAdminProfileClick_click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AdminProfile(adminId);
        }

        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


