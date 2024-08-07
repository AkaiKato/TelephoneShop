﻿namespace TelephoneShop.Models
{
    public class Cities
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual List<CitiesToTelephoneCost>? CitiesToTelephoneCost { get; set; }
    }
}
