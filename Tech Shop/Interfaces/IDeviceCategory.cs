using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_Shop.Models;

namespace Tech_Shop.Interfaces
{
    internal interface IDeviceCategory
    {
        IEnumerable<DeviceCategory> AllCategories { get; }
    }
}
