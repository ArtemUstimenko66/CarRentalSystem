using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RentACar
{
  
     public class Car
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public byte[] Photo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCheckedCar { get; set; }
        public bool IsVisible { get; set; }
        public string Availability { get; set; }
        public string LicensePlate { get; set; }
        public bool IsPaymentCompleted { get; set; }
        public int CarId { get; set; }
        public int RateDay { get; set; }
        public string CarClass { get; set; }
    }
}
   
