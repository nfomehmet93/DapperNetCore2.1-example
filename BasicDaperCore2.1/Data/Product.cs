using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFullServices.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public DateTime CreateonUtc { get; set; }
        public Nullable<DateTime> UpdateonUtc { get; set; }

    }
}
