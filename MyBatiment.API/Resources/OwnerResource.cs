﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBatiment.API.Resources
{
    public class OwnerResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public string Tel { get; set; }
        public string Image { get; set; }
    }
}
