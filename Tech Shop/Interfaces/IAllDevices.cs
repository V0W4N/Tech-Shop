using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech_Shop.Models;


namespace Tech_Shop.Interfaces
{
    public interface IAllDevices
    {
        IEnumerable<Device> Devices { get;  }
        Device getDeviceByID(int deviceId);
    }
}
