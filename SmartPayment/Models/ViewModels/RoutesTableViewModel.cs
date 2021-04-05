using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPayment.Models.ViewModels
{
    public class RoutesTableViewModel
    {
        public int Code { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public bool State { get; set; }
    }
}