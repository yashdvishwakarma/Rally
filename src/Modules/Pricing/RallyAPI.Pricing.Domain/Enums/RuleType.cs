using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyAPI.Pricing.Domain.Enums;

public enum RuleType
{
    BaseFee = 0,
    Distance = 1,
    TimeSurge = 2,
    DaySurge = 3,
    WeatherSurge = 4,
    DemandSurge = 5,
    RestaurantPromo = 6,
    CustomerLoyalty = 7,
    PromoCode = 8,
    MinimumFee = 9,
    MaximumCap = 10
}