using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPayment.Models.ViewModels
{
    public class ReportsClientTableVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public bool State { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}