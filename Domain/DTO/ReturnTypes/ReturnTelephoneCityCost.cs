using Domain.DTO.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.ReturnTypes
{
    public class ReturnTelephoneCityCost
    {
        public int Telephone { get; set; }

        public int City { get; set; }

        public decimal Cost { get; set; }
    }
}
