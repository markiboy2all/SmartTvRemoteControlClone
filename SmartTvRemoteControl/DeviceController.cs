using Microsoft.AspNetCore.Components;
using Networking.Native;
using SmartView2.Core;
using SmartView2.Devices;
using SmartView2.Devices.SecondTv;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIFoundation;
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

		public ITargetDevice CurrentDevice
		{
			get;
			private set;
		}

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

		public async Task<DeviceInfo[]> ExecuteDiscovery()
		{
			Console.WriteLine("Discovery started... ");
			deviceDiscovery.Scan();
			await Task.Delay(2000);
			
			Console.WriteLine("Discovery completed... ");
			return deviceDiscovery.GetFoundTVDeviceInfo();
		}

		public async Task<bool> SetPin(string pin)
		{
			bool flag;
			string str = await devicePairing.EnterPinAsync(pin);
			if (string.IsNullOrEmpty(str))
			{
				flag = false;
			}
			else
			{
				await InitTargetDevice(this.CurrentDeviceInfo);
				this.settingProvider.Save(this.CurrentDeviceInfo.DeviceAddress.Host, pin);
				flag = true;
			}
			return flag;
		}


		public async Task ConnectToDevice(DeviceInfo deviceInfo)
		{
			Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]ConnectToDevice started... ");
			try
			{
				if (deviceInfo == null)
				{
					return;
				}
				else if (!TvDiscovery.IsTv2014(deviceInfo))
				{
					Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]TV is not 14 year. ");
					Console.WriteLine("[SmartView2][DeviceListViewModel]TV is not 14 year. ");
					return;
				}
				else if (await TryToConnect(deviceInfo.DeviceAddress.Host))
				{
					Console.WriteLine("Connected tot device ");
				}
				else
				{
					Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Call deviceController Connect . ");
					switch (await Connect(deviceInfo))
					{
						case DeviceController.ConnectResult.PinPageAlreadyShown:
							{
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Another device is connecting. ");
								Console.WriteLine("[SmartView2][DeviceListViewModel]Another device is connecting. ");
								return;
							}
						case DeviceController.ConnectResult.SocketException:
							{
								//this.deviceController.RefreshDiscovery();
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]TV is not ready. ");
								Console.WriteLine("[SmartView2][DeviceListViewModel]TV is not ready. ");
								return;
							}
						case DeviceController.ConnectResult.OtherException:
							{
								Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]something went wrong. ");
								Console.WriteLine("[SmartView2][DeviceListViewModel]TV is not ready. ");
								return;
							}
						default:
							{
								return;
							}
					}
				}
			}
			catch (Exception exception2)
			{
				Exception exception1 = exception2;
				Logger.Instance.LogMessageFormat("[SmartView2][DeviceListViewModel]Catch unknown error.", new object[0]);
				Logger instance1 = Logger.Instance;
				instance1.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message: {0} ", new object[] { exception1.Message });
				Logger logger1 = Logger.Instance;
				logger1.LogMessageFormat("[SmartView2][DeviceListViewModel]Error message stack trace: {0} ", new object[] { exception1.StackTrace });
			}
		}

		public async Task<DeviceController.ConnectResult> Connect(DeviceInfo device)
		{
			DeviceController.ConnectResult connectResult;
			Console.WriteLine("Connect started... ");
			try
			{
				if (!await this.devicePairing.StartPairingAsync(device.DeviceAddress))
				{
					connectResult = DeviceController.ConnectResult.PinPageAlreadyShown;
				}
				else
				{
					this.SetCurrentDeviceInfo(device);
					connectResult = DeviceController.ConnectResult.Ok;
				}
			}
			catch (SocketException socketException)
			{
				connectResult = DeviceController.ConnectResult.SocketException;
			}
			catch
			{
				connectResult = DeviceController.ConnectResult.OtherException;
			}
			return connectResult;
		}

		private void SetCurrentDeviceInfo(DeviceInfo device)
		{
			CurrentDeviceInfo = device;
		}

		public async Task<bool> TryToConnect(string deviceAddress = "")
		{
			bool flag;
			string str = deviceAddress;
			if (str == "")
			{
				str = this.settingProvider.LoadLastIp();
			}
			if (!string.IsNullOrEmpty(str))
			{
				string str1 = this.settingProvider.LoadPin(str);
				if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str1))
				{
					ObservableCollection<DeviceInfo> devices = this.Devices;
					DeviceInfo deviceInfo = devices.FirstOrDefault<DeviceInfo>((DeviceInfo d) => d.DeviceAddress.Host.Contains(str));
					if (deviceInfo != null)
					{
						try
						{
							string str2 = await this.devicePairing.TryPairAsync(deviceInfo.DeviceAddress, str1);
							if (string.IsNullOrEmpty(str2))
							{
								this.settingProvider.Save(str, string.Empty);
							}
							else
							{
								this.SetCurrentDeviceInfo(deviceInfo);
								this.settingProvider.Save(str, str1);
								await this.InitTargetDevice(deviceInfo);
								flag = true;
								return flag;
							}
						}
						catch
						{
							flag = false;
							return flag;
						}
					}
				}
				flag = false;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		private async Task InitTargetDevice(DeviceInfo device)
		{
			Console.WriteLine("InitTargetDevice - started... ");
			Logger instance = Logger.Instance;
			object[] encryptionEnabled = new object[] { this.devicePairing.EncryptionEnabled };
			instance.LogMessageFormat("InitTargetDevice - EncryptionEnabled : {0}", encryptionEnabled);
			ISecondTvSecurityProvider noSecurityProvider = null;
			if (!this.devicePairing.EncryptionEnabled)
			{
				noSecurityProvider = new NoSecurityProvider();
			}
			else
			{
				noSecurityProvider = new AesSecurityProvider(this.devicePairing.SpcApi.GetKey(), this.devicePairing.SessionId);
			}
			Console.WriteLine("InitTargetDevice - Created Tv device.");

			ITargetDevice targetDevice = DeviceFactory.CreateTvDevice(device, this.notificationProvider, new DispatcherWrapper(Dispatcher.CreateDefault()), noSecurityProvider);
			Console.WriteLine("InitTargetDevice - Set current device.");
			this.SetCurrentDevice(targetDevice);
			try
			{
				Console.WriteLine("InitTargetDevice - Initialize Tv device.");
				await targetDevice.InitializeAsync();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				Console.WriteLine("[ERROR]Catch exception when initialize TV.");
				Logger logger = Logger.Instance;
				logger.LogMessageFormat("Exception message: {0}", new object[] { exception.Message });
				Logger instance1 = Logger.Instance;
				instance1.LogMessageFormat("Exception stacktrace: {0}", new object[] { exception.StackTrace });
			}
			finally
			{
				Console.WriteLine("InitTargetDevice is complete");
			}
		}

		private void SetCurrentDevice(ITargetDevice device)
		{
			if (this.CurrentDevice != null)
			{
				this.CurrentDevice.Disconnecting -= new EventHandler<EventArgs>(this.CurrentDevice_Disconnecting);
			}
			this.CurrentDevice = device;
			if (this.CurrentDevice != null)
			{
				this.CurrentDevice.Disconnecting += new EventHandler<EventArgs>(this.CurrentDevice_Disconnecting);
			}
		}

		private void CurrentDevice_Disconnecting(object sender, EventArgs e)
		{
			Console.WriteLine("CurrentDevice_Disconnecting started... ");
			OnCurrentDeviceDisconnected(this, EventArgs.Empty);
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

		public enum ConnectResult
		{
			Ok,
			PinPageAlreadyShown,
			SocketException,
			OtherException
		}
	}
}
