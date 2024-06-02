using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_Shop.Models;

namespace Tech_Shop.Interfaces
{
    public interface IDeviceCategory
    {
        IEnumerable<DeviceCategory> AllCategories { get; }
    }
}
