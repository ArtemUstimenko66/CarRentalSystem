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
using RentACar.Controls;
using System.Collections.ObjectModel;

namespace RentACar
{
    /// <summary>
    /// Interaction logic for DocumentForm.xaml
    /// </summary>
    public partial class DocumentForm : Window
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LogAndRegBd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private int CurrentUserId = 25035;
        private byte[] imageData;
        public DocumentForm()
        {
            InitializeComponent();
        }
        private void btn_DocumentAdd_click(object sender, RoutedEventArgs e)
        {
            string documentName = txtDocumentName.Text;
            if (imageData == null)
            {
                MessageBox.Show("Документ не загружен!");
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = $"INSERT INTO DocumentInfo (DocumentLink,DocumentName,UserId) VALUES (@documentLink,@documentName,@userId)";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@documentName", documentName);
                command.Parameters.Add("@documentLink", SqlDbType.Image).Value = imageData;
                command.Parameters.AddWithValue("@userId", CurrentUserId);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Документ успешно добавлен!");
                }
            }
        }
        private void btn_close(object sender, RoutedEventArgs e)
            {
               this.Close();
            }

        private void btn_BrowseDoc_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                txtDocumentLink.Text = fileName;
                imageData = File.ReadAllBytes(fileName);
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
    }
}
