using SmartView2.Core;
using SmartView2.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using UPnP;
using UPnP.DataContracts;

namespace SmartTvRemoteControl
{
    public class DeviceController
    {
		private IDeviceDiscovery deviceDiscovery;
		private readonly IBaseDispatcher dispatcher;
		private readonly IDevicePairing devicePairing;
		private readonly IDeviceSettingProvider settingProvider;
		private readonly CancellationTokenSource tokenSource;
		private readonly IPlayerNotificationProvider notificationProvider;

		private bool isRefreshing;

		public DeviceInfo CurrentDeviceInfo
		{
			get;
			private set;
		}

		public ObservableCollection<DeviceInfo> Devices
		{
			get;
			private set;
		}

		public DeviceController(IDeviceDiscovery deviceDiscovery, IDevicePairing devicePairing, IPlayerNotificationProvider notificationProvider, IDeviceSettingProvider settingProvider)
		{
			if (deviceDiscovery == null)
			{
				Console.WriteLine("deviceDiscovery null.");
				throw new ArgumentNullException("deviceDiscovery");
			}
			if (devicePairing == null)
			{
				Console.WriteLine("devicePairing null.");
				throw new ArgumentNullException("devicePairing");
			}
			if (settingProvider == null)
			{
				Console.WriteLine("settingProvider null.");
				throw new ArgumentNullException("settingProvider");
			}
			if (notificationProvider == null)
			{
				Console.WriteLine("notificationProvider null");
				throw new ArgumentNullException("notificationProvider");
			}
			this.deviceDiscovery = deviceDiscovery;
			this.deviceDiscovery.DeviceConnected += new EventHandler<DeviceInfoEventArgs>(this.deviceDiscovery_DeviceConnected);
			this.deviceDiscovery.DeviceUpdated += new EventHandler<DeviceInfoEventArgs>(this.deviceDiscovery_DeviceUpdated);
			this.deviceDiscovery.DeviceDisconnected += new EventHandler<DeviceInfoEventArgs>(this.deviceDiscovery_DeviceDisconnected);
			this.devicePairing = devicePairing;
			this.notificationProvider = notificationProvider;
			this.settingProvider = settingProvider;
			this.Devices = new ObservableCollection<DeviceInfo>();
			this.tokenSource = new CancellationTokenSource();
		}

		private void deviceDiscovery_DeviceConnected(object sender, DeviceInfoEventArgs e)
		{
			Console.WriteLine("UPnP connection extablished with TV - " + e.DeviceInfo.UniqueDeviceName);
			if (this.Devices.Any<DeviceInfo>((DeviceInfo device) => device.UniqueDeviceName == e.DeviceInfo.UniqueDeviceName))
			{
				return;
			}
			Console.WriteLine("DeviceConnected added TV: {0}", new object[] { e.DeviceInfo.DeviceAddress });
			this.Devices.Add(e.DeviceInfo);
		}

		private void deviceDiscovery_DeviceUpdated(object sender, DeviceInfoEventArgs e)
		{
			//this.dispatcher.Invoke(() => {
			Console.WriteLine("Device Updated");
			DeviceInfo friendlyName = (
				from device in this.Devices
				where device.UniqueDeviceName == e.DeviceInfo.UniqueDeviceName
				select device).FirstOrDefault<DeviceInfo>();
			if (friendlyName != null)
			{
				friendlyName.FriendlyName = e.DeviceInfo.FriendlyName;
			}
			//});
		}

		private void deviceDiscovery_DeviceDisconnected(object sender, DeviceInfoEventArgs e)
		{
			bool flag;
			Console.WriteLine("DeviceDisconnected started... ");
			flag = (this.CurrentDeviceInfo == null ? false : this.CurrentDeviceInfo.UniqueDeviceName == e.DeviceInfo.UniqueDeviceName);
			if (this.isRefreshing && flag)
			{
				return;
			}
			//this.dispatcher.Invoke(() => {
			DeviceInfo deviceInfo = (
				from arg in this.Devices
				where arg.UniqueDeviceName == e.DeviceInfo.UniqueDeviceName
				select arg).FirstOrDefault<DeviceInfo>();
			if (deviceInfo != null)
			{
				Console.WriteLine("DeviceDisconnected remove TV: {0}", new object[] { deviceInfo.DeviceAddress });
				this.Devices.Remove(deviceInfo);
				if (this.CurrentDeviceInfo != null && this.CurrentDeviceInfo.UniqueDeviceName == deviceInfo.UniqueDeviceName)
				{
					Console.WriteLine("Disconnect current device.");
					this.OnCurrentDeviceDisconnected(this, EventArgs.Empty);
				}
			}
			//});
		}

		private void OnCurrentDeviceDisconnected(object sender, EventArgs e)
		{
			EventHandler<EventArgs> eventHandler = this.CurrentDeviceDisconnected;
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
		}

		public event EventHandler<EventArgs> CurrentDeviceDisconnected;
	}
}
