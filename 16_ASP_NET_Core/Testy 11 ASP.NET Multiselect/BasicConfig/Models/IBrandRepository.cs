﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class IBrandRepository
    {
        IQueryable<Brand> Brands { get; }
    }
}
