using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentACar.Controls
{
    /// <summary>
    /// Interaction logic for AdminProfile.xaml
    /// </summary>
    public partial class AdminProfile : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private byte[] imageData;
        private int Id;


        public AdminProfile(int id)
        {
            InitializeComponent();
            Id = id;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Assume you have an admin ID stored in a variable named adminId
                string query = "SELECT Adminname, Password, Photo FROM RegisteredAdmins WHERE Id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", Id);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string adminName = reader.GetString(reader.GetOrdinal("Adminname"));
                            string password = reader.GetString(reader.GetOrdinal("Password"));
                            txtNewAdminName.Text = adminName;
                            txtNewAdminPassword.Password = password;

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

        private void btn_browse_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                myImage.Background = new ImageBrush(bitmap);
                imageData = File.ReadAllBytes(imagePath);
            }
        }

        private void btn_Save_click(object sender, RoutedEventArgs e)
        {
            string newAdminName = txtNewAdminName.Text;
            string newAdminPassword = txtNewAdminPassword.Password;
            string query = $"UPDATE RegisteredAdmins SET Adminname=@NewAdminName, Password=@NewAdminPassword, Photo=@ImageData WHERE Id=@id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add("@ImageData", SqlDbType.Image).Value = imageData;
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                sqlCommand.Parameters.Add("@NewAdminName", SqlDbType.NVarChar).Value = newAdminName;
                sqlCommand.Parameters.Add("@NewAdminPassword", SqlDbType.NVarChar).Value = newAdminPassword;
                connection.Open();
                int rows = sqlCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Your data has been successfully updated!");
                }
                else
                {
                    MessageBox.Show("Failed to update your data!");
                }
            }
        }

        private void btn_DeleteAcc_click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM RegisteredAdmins WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                connection.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete this user?", "Deletion Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        // Пользователь подтвердил удаление
                        // Дополнительный код для удаления учетной записи
                        MessageBox.Show("Account deleted successfully!");
                        Application.Current.Shutdown(); // Закрыть приложение
                    }
                    else
                    {
                        // Пользователь отказался от удаления
                        // Дополнительный код, если требуется
                        MessageBox.Show("Account deletion canceled!");
                    }
                }
                else
                {
                    MessageBox.Show("Failed to delete your account!");
                }
            }
        }
    }
}


