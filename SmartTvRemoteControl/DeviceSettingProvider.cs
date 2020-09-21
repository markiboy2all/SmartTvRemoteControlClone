using Microsoft.Extensions.Configuration;
using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
			string str;
			if (configuration.GetSection("ConnectedDeviceAddresses").Value == null)
			{
				configuration.GetSection("ConnectedDeviceAddresses").Value = string.Empty;
			}
			else
			{
				var stringArray = configuration.GetSection("ConnectedDeviceAddresses").Value.Split(',');
				foreach(var current in stringArray)
                {
					if (!current.Contains(lastConnectedDeviceAddress))
					{
						continue;
					}
					string[] strArrays = current.Split(new char[] { ':' });
					if ((int)strArrays.Length != 2)
					{
						continue;
					}
					str = strArrays[1];
					return str;
				}
			}
			return "";
		}

        public void ResetLastConnectionAddress()
        {
            throw new NotImplementedException();
        }

        public void Save(string lastConnectedDeviceAddress, string lastConnectedDevicePin)
        {
			if (configuration.GetSection("ConnectedDeviceAddresses").Value == null)
			{
				configuration.GetSection("ConnectedDeviceAddresses").Value = string.Empty;
			}
			else
			{
				var stringArray = configuration.GetSection("ConnectedDeviceAddresses").Value.Split(',');
				foreach (string connectedDeviceAddress in stringArray)
				{
					if (!connectedDeviceAddress.Contains(lastConnectedDeviceAddress))
					{
						continue;
					}
					var newValue = Regex.Replace(configuration.GetSection("ConnectedDeviceAddresses").Value, @"," + lastConnectedDeviceAddress + @":.+?(?=,)", "");
					configuration.GetSection("ConnectedDeviceAddresses").Value = newValue;
					break;
				}
			}
			if (string.IsNullOrEmpty(lastConnectedDevicePin))
			{
				configuration.GetSection("LastConnectedDeviceAddress").Value = string.Empty;
			}
			else
			{
				configuration.GetSection("ConnectedDeviceAddresses").Value +=string.Format("{0}:{1}", lastConnectedDeviceAddress, lastConnectedDevicePin);
				configuration.GetSection("LastConnectedDeviceAddress").Value = lastConnectedDeviceAddress;
			}
		}
    }
}
