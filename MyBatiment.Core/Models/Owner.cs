using System;
using System.Collections.Generic;
using System.Text;

namespace MyBatiment.Core.Models
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public string Tel { get; set; }
        public string Image { get; set; }
    }
}
