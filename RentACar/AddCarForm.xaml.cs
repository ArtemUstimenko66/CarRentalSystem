using RentACar.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Microsoft.Win32;
using System.IO;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for AddCarForm.xaml
    /// </summary>
    public partial class AddCarForm : Window
    {
        private byte[] imageData;

        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public AddCarForm()
        {
            InitializeComponent();   
        }
        private void btn_AddCarForm_click(object sender, RoutedEventArgs e)
        {
            string AddColor = CbColor.SelectedItem.ToString();
            string AddMark = txtMark.Text;
            string AddModel = txtModel.Text;
            string AddYear = txtYear.Text;
            string AddFuelType = CbFuelType.SelectedItem.ToString();
            string AddLicensePlate = txtLicensePlate.Text;
            string AddNumberOfSeats = txtNumberOfSeats.Text;
            string AddMileage = txtMileage.Text;
            string AddAvailability = CbAvailibility.SelectedItem.ToString();
            string AddPrice = txtPrice.Text;
            string AddTransmission = cbTransmission.SelectedItem.ToString();
            string AddDailyContent = txtDailyContent.Text;
            string AddCarClass = cbCarClass.SelectedItem.ToString();    
            if (imageData == null)
            {
                string defaultImagePath = "F:\\c#\\RentACar\\RentACar\\Images\\mersedes.png";
                imageData = File.ReadAllBytes(defaultImagePath);
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string insertSql = "INSERT INTO CarData (Color, Mark, Model, Year, FuelType, LicensePlate, NumberOfSeats, Mileage, Availability, Photo,RateDay,Transmission,DailyContent,CarClass) " +
                    "VALUES (@Color, @Mark, @Model, @Year, @FuelType, @LicensePlate, @NumberOfSeats, @Mileage, @Availability, @Photo,@RateDay,@Transmission,@DailyContent,@CarClass)";
                SqlCommand insertCommand = new SqlCommand(insertSql, connection);

                insertCommand.Parameters.AddWithValue("@Color", AddColor);
                insertCommand.Parameters.AddWithValue("@Mark", AddMark);
                insertCommand.Parameters.AddWithValue("@Model", AddModel);
                insertCommand.Parameters.AddWithValue("@Year", AddYear);
                insertCommand.Parameters.AddWithValue("@FuelType", AddFuelType);
                insertCommand.Parameters.AddWithValue("@LicensePlate", AddLicensePlate);
                insertCommand.Parameters.AddWithValue("@NumberOfSeats", AddNumberOfSeats);
                insertCommand.Parameters.AddWithValue("@Mileage", AddMileage);
                insertCommand.Parameters.AddWithValue("@Availability", AddAvailability);
                insertCommand.Parameters.AddWithValue("@Photo", imageData != null ? (object)imageData : DBNull.Value); // добавляем байтовый массив изображения в параметры команды SQL
                insertCommand.Parameters.AddWithValue("@RateDay", AddPrice);
                insertCommand.Parameters.AddWithValue("@Transmission", AddTransmission);
                insertCommand.Parameters.AddWithValue("@DailyContent", AddDailyContent);
                insertCommand.Parameters.AddWithValue("@CarClass", AddCarClass);
                connection.Open();
                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Car added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add car!");
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
            CbColor.Items.Add("Red");
            CbColor.Items.Add("Green");
            CbColor.Items.Add("Blue");
            CbColor.Items.Add("Black");
            CbColor.Items.Add("Gray");
            CbFuelType.Items.Add("Dizel");
            CbFuelType.Items.Add("Benzin");
            CbFuelType.Items.Add("Gaz");
            CbAvailibility.Items.Add("Avalaible");
            CbAvailibility.Items.Add("On Rent");
            CbAvailibility.Items.Add("On Servise");
            cbTransmission.Items.Add("Automatic");
            cbTransmission.Items.Add("Mechanical");
            cbCarClass.Items.Add("Economy");
            cbCarClass.Items.Add("SUV");
            cbCarClass.Items.Add("Business");
            cbCarClass.Items.Add("Premium");
            cbCarClass.Items.Add("Minivan");
        }
    }
}
