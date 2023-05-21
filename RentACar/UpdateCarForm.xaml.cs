using Microsoft.Win32;
using System;
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
using System.Windows.Shapes;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for UpdateCarForm.xaml
    /// </summary>
    public partial class UpdateCarForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string Color;
        private string Mark;
        private string Model;
        private string Year;
        private string Fueltype;
        private string Licenseplate;
        private string Numberofseats;
        private string Mileage;
        private string Availability;
        private string Transmission;
        private string DailyContent;
        private string CarClass;
        private DataRowView RowView;
        private byte[] ImageData;
        public UpdateCarForm(string color, string mark, string model, string year, string fueltype, string licenseplate, string numberofseats, string mileage,DataRowView rowView, string availability, byte[] imageData, string transmission,string priceperday,string carclass)
        {
            InitializeComponent();

            this.Color = color;
            this.Mark = mark;
            this.Model = model;
            this.Year = year;
            this.Fueltype = fueltype;
            this.Licenseplate = licenseplate;
            this.Numberofseats = numberofseats;
            this.Mileage = mileage;
            this.RowView = rowView;
            this.Availability = availability;
            this.ImageData = imageData;
            this.Transmission = transmission;
            this.CarClass = carclass;
   
            cbColor.SelectedItem = color;
            txtMark.Text = mark;
            txtModel.Text = model;
            txtYear.Text = year;
            cbFuelType.SelectedItem = fueltype;
            txtLicensePlate.Text = licenseplate;
            txtNumberOfSeats.Text = numberofseats;
            txtMileage.Text = mileage;
            cbAvalaibility.SelectedItem = availability;
            cbTransmission.SelectedItem = transmission;
            txtPricePerDay.Text = priceperday;
            cbCarClass.SelectedItem = carclass;
        
            if (imageData != null)
            {
                using (var stream = new MemoryStream(imageData))
                {
                    var imageBrush = new ImageBrush();
                    imageBrush.ImageSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    imageBrush.Stretch = Stretch.Uniform;
                    myImage.Background = imageBrush;
                }
            }
         
        }

        private void btn_SaveCarForm_click(object sender, RoutedEventArgs e)
        {
            string newColor = cbColor.SelectedItem.ToString();
            string newMark = txtMark.Text;
            string newModel = txtModel.Text;
            string newYear = txtYear.Text;
            string newFuelType = cbFuelType.SelectedItem.ToString();
            string newLicensePlate = txtLicensePlate.Text;
            string newNumberOfSeats = txtNumberOfSeats.Text;
            string newMileage = txtMileage.Text;
            string newAvalaibility = cbAvalaibility.SelectedItem.ToString();
            string newTransmission = cbTransmission.SelectedItem.ToString();
            string newPricePerDay = txtPricePerDay.Text;
            string newCarClass = cbCarClass.SelectedItem.ToString();

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

            string query = $"UPDATE CarData SET Color='{newColor}',Mark='{newMark}',Model='{newModel}',Year='{newYear}',FuelType='{newFuelType}',LicensePlate='{newLicensePlate}',NumberOfSeats='{newNumberOfSeats}',Mileage='{newMileage}',Availability='{newAvalaibility}',Transmission='{newTransmission}',RateDay='{newPricePerDay}',CarClass='{newCarClass}',Photo=@ImageData WHERE Id IN (SELECT TOP 1 Id FROM CarData ORDER BY NEWID())";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add("@ImageData", SqlDbType.Image).Value = ImageData;
                connection.Open();
                int rows = sqlCommand.ExecuteNonQuery();
                try
                {
                    if (rows > 0)
                    {
                        RowView["Color"] = newColor;
                        RowView["Mark"] = newMark;
                        RowView["Model"] = newModel;
                        RowView["Year"] = newYear;
                        RowView["FuelType"] = newFuelType;
                        RowView["LicensePlate"] = newLicensePlate;
                        RowView["NumberOfSeats"] = newNumberOfSeats;
                        RowView["Mileage"] = newMileage;
                        RowView["Availability"] = newAvalaibility;
                        RowView["Transmission"] = newTransmission;
                        RowView["RateDay"] = newPricePerDay;
                        RowView["CarClass"] = newCarClass;
                      
                        MessageBox.Show("Данные машины успешно обновлены.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить данные машины.");
                    }
                  
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btn_UpdateCarPhoto_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                myImage.Background = new ImageBrush(bitmap);

                ImageData = File.ReadAllBytes(imagePath); // загружаем изображение в байтовый массив
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
            cbColor.Items.Add("Red");
            cbColor.Items.Add("Green");
            cbColor.Items.Add("Blue");
            cbColor.Items.Add("Black");
            cbColor.Items.Add("White");
            cbFuelType.Items.Add("Dizel");
            cbFuelType.Items.Add("Benzin");
            cbFuelType.Items.Add("Gaz");
            cbAvalaibility.Items.Add("Avalaible");
            cbAvalaibility.Items.Add("On Rent");
            cbAvalaibility.Items.Add("On Servise");
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
