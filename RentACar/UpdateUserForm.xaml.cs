using Microsoft.Win32;
using RentACar.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Interaction logic for UpdateUserForm.xaml
    /// </summary>
    public partial class UpdateUserForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string Login;
        private string Email;
        private string Phone;
        private string Address;
        private string Gender;
        private string DateOfBirth;
        private DataRowView RowView;
        private byte[] ImageData;
       
        public UpdateUserForm(string userLogin,string email, string phone, string address, string gender, string dateofbirth, DataRowView rowView, byte[] imageData)
        {
            InitializeComponent();

            this.Login = userLogin;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.Gender = gender;
            this.DateOfBirth = dateofbirth;
            this.RowView = rowView;
            this.ImageData = imageData;
            txtLogin.Text = userLogin;
            txtEmail.Text = email;
            txtAddress.Text = address;
            txtGender.Text = gender;
            txtPhone.Text = phone;
            txtDateOfBirth.Text = dateofbirth;

        }
        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_UpdateUserForm_click(object sender, RoutedEventArgs e)
        {
            string newUserLogin = txtLogin.Text;
            string newEmail = txtEmail.Text;
            string newPhone = txtPhone.Text;
            string newAddress = txtAddress.Text;
            string newGender = txtGender.Text;
            string newDateofBirth = txtDateOfBirth.Text;
            
            if (ImageData == null)
            {
                // Если изображение не выбрано, установить изображение по умолчанию
                var uriSource = new Uri("F:\\c#\\RentACar\\RentACar\\Images\\user.png", UriKind.Relative);
                var defaultImageBrush = new ImageBrush(new BitmapImage(uriSource));
                defaultImageBrush.Stretch = Stretch.Uniform;
                myImage.Background = defaultImageBrush;
            }
            else
            {
                // Если изображение выбрано, отобразить его в ImageBrush
                using (var stream = new MemoryStream(ImageData))
                {
                    var imageBrush = new ImageBrush();
                    imageBrush.ImageSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    imageBrush.Stretch = Stretch.Uniform;
                    myImage.Background = imageBrush;
                }
            }

           string query = $"UPDATE RegisteredUsers SET Users='{newUserLogin}',Email='{newEmail}',Phone='{newPhone}',Address='{newAddress}',Gender='{newGender}',DateOfBirth='{newDateofBirth}',Photo=@ImageData WHERE Id IN (SELECT TOP 1 Id FROM RegisteredUsers ORDER BY NEWID())";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add("@ImageData", SqlDbType.Image).Value = ImageData;
                connection.Open();
                int rows = sqlCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    RowView["Users"] = newUserLogin;
                    RowView["Email"] = newEmail;
                    RowView["Phone"] = newPhone;
                    RowView["Address"] = newAddress;
                    RowView["Gender"] = newGender;
                    RowView["DateofBirth"] = newDateofBirth; 
                    MessageBox.Show("User data updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update user data.");
                }
                this.Close();
            }
        }

        private void btn_UpdatePhoto_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                myImage.Background = new ImageBrush(bitmap);

                ImageData = File.ReadAllBytes(imagePath); 
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

        private void Border_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        } 
    }
}
