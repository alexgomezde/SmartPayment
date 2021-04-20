using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPayment.Models.ViewModels
{
    public class PaymentsTableViewModel
    {
        public int Id { get; set; }
        public string Id_Client { get; set; }
        public string Id_Driver { get; set; }
        public int Id_Route { get; set; }
        public DateTime DateOfPayment { get; set; }
        public bool State { get; set; }
    }
}