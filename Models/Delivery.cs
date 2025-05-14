using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string CargoDescription { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string Status { get; set; }
        public string CancellationReason { get; set; }
    }
}
