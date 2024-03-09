using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_Shop.Models;

namespace Tech_Shop.Interfaces
{
    internal interface IAllDevices
    {
        IEnumerable<Device> devices { get;  }
        Device getDeviceByID(int deviceId);
    }
}
