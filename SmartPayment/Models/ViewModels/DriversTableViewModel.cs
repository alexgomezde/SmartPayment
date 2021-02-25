using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPayment.Models.ViewModels
{
    public class DriversTableViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string SecondLastname { get; set; }
        public bool State { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}