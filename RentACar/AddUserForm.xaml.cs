using Microsoft.Win32;
using RentACar.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for AddUserForm.xaml
    /// </summary>
    public partial class AddUserForm : Window
    {
        private byte[] imageData;
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void btn_AddUserForm_click(object sender, RoutedEventArgs e)
        {
            string AddName =  txtLogin.Text;
            string AddPhone = txtPhone.Text;
            string AddEmail = txtEmail.Text;
            string AddAddress = txtAddress.Text;
            string AddGender = txtGender.Text;
            string AddDateOfBirth = txtDateOfBirth.Text;
            string Addpassword = new System.Net.NetworkCredential(string.Empty, txtPassword.Password).Password;
            string resetPassword = new System.Net.NetworkCredential(string.Empty, txtResetPassword.Password).Password;

            if (imageData == null)
            {
                // ваш код для установки значения по умолчанию или вывода сообщения об ошибке
                string defaultImagePath = "F:\\c#\\RentACar\\RentACar\\Images\\user.png";
                imageData = File.ReadAllBytes(defaultImagePath);
            }

            if (Addpassword != resetPassword)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string insertSql = "INSERT INTO RegisteredUsers (Users,Password,Phone,Email,Address,Gender,DateofBirth,Photo) " +
                    "VALUES (@Users, @Password, @Phone, @Email, @Address, @Gender, @DateofBirth,@Photo)";
                SqlCommand insertCommand = new SqlCommand(insertSql, connection);
                insertCommand.Parameters.AddWithValue("@Users", AddName);
                insertCommand.Parameters.AddWithValue("@Password", Addpassword);
                insertCommand.Parameters.AddWithValue("@Phone", AddPhone);
                insertCommand.Parameters.AddWithValue("@Email", AddEmail);
                insertCommand.Parameters.AddWithValue("@Address", AddAddress);
                insertCommand.Parameters.AddWithValue("@Gender", AddGender);
                insertCommand.Parameters.AddWithValue("@DateofBirth", AddDateOfBirth);
                insertCommand.Parameters.AddWithValue("@Photo", imageData != null ? (object)imageData : DBNull.Value); // добавляем байтовый массив изображения в параметры команды SQL

                connection.Open();
                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // ваш код для обновления отображения данных на форме
                    MessageBox.Show("User added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add user!");
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

                imageData = File.ReadAllBytes(imagePath); // загружаем изображение в байтовый массив
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        bool isMax = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Border_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }

        private void Border_mouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
