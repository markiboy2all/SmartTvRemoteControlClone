using Microsoft.Extensions.Configuration;
using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTvRemoteControl
{
    public class DeviceSettingProvider : IDeviceSettingProvider
	{
		private IConfigurationRoot configuration;

		public DeviceSettingProvider(IConfigurationRoot configuration)
        {
			this.configuration = configuration;
        }

        public string LoadLastIp()
        {
            throw new NotImplementedException();
        }

        public string LoadPin(string lastConnectedDeviceAddress)
        {
            throw new NotImplementedException();
        }

        public void ResetLastConnectionAddress()
        {
            throw new NotImplementedException();
        }

        public void Save(string lastConnectedDeviceAddress, string lastConnectedDevicePin)
        {
            throw new NotImplementedException();
        }
    }
}
