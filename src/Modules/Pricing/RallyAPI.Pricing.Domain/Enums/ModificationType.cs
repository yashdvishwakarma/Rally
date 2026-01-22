using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyAPI.Pricing.Domain.Enums;

    public enum ModificationType
    {
        Flat = 0,           // Add fixed amount: +₹20
        Percentage = 1,     // Add percentage: +10%
        Multiplier = 2      // Multiply total: 1.5x
    }


