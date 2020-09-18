using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UPnP.DataContracts;

namespace UPnP
{
	public class DevicePool : ICollection<DeviceInfo>, IEnumerable<DeviceInfo>, IEnumerable
	{
		private IDictionary<string, DeviceInfo> devices = new Dictionary<string, DeviceInfo>();

		private object collectionLock = new object();

		public DeviceInfo this[string usn]
		{
			get
			{
				lock (collectionLock)
				{
					DeviceInfo value = null;
					devices.TryGetValue(usn, out value);
					return value;
				}
			}
		}

		public int Count
		{
			get
			{
				lock (collectionLock)
				{
					return devices.Count;
				}
			}
		}

		public bool IsReadOnly => false;

		public event EventHandler<DeviceInfoEventArgs> DeviceAdded;

		public event EventHandler<DeviceInfoEventArgs> DeviceRemoved;

		public void Add(DeviceInfo item)
		{
			lock (collectionLock)
			{
				if (!devices.ContainsKey(item.UniqueServiceName))
				{
					devices.Add(item.UniqueServiceName, item);
					OnDeviceAdded(this, new DeviceInfoEventArgs(item));
				}
				else
				{
					DeviceInfo deviceInfo = devices[item.UniqueServiceName];
					deviceInfo.LastActive = item.LastActive;
				}
			}
		}

		public void Update(string usn, DateTime timestamp)
		{
			lock (collectionLock)
			{
				if (devices.ContainsKey(usn))
				{
					DeviceInfo deviceInfo = devices[usn];
					deviceInfo.LastActive = timestamp;
				}
			}
		}

		public void Update(string usn, Action<DeviceInfo> action)
		{
			lock (collectionLock)
			{
				if (devices.ContainsKey(usn))
				{
					DeviceInfo obj = devices[usn];
					action(obj);
				}
			}
		}

		public bool Remove(DeviceInfo device)
		{
			lock (collectionLock)
			{
				if (Contains(device.UniqueServiceName))
				{
					devices.Remove(device.UniqueServiceName);
					OnDeviceRemoved(this, new DeviceInfoEventArgs(device));
					return true;
				}
				return false;
			}
		}

		public bool Remove(string usn)
		{
			lock (collectionLock)
			{
				if (Contains(usn))
				{
					DeviceInfo deviceInfo = devices[usn];
					devices.Remove(deviceInfo.UniqueServiceName);
					OnDeviceRemoved(this, new DeviceInfoEventArgs(deviceInfo));
					return true;
				}
				return false;
			}
		}

		public void RemoveInactiveDevices(DateTime currentTimestamp, TimeSpan inactiveTime)
		{
			lock (collectionLock)
			{
				string[] array = (from arg in devices
								  where currentTimestamp - arg.Value.LastActive > inactiveTime
								  select arg.Key).ToArray();
				string[] array2 = array;
				foreach (string key in array2)
				{
					DeviceInfo device = devices[key];
					devices.Remove(key);
					OnDeviceRemoved(this, new DeviceInfoEventArgs(device));
				}
			}
		}

		public bool Contains(DeviceInfo device)
		{
			lock (collectionLock)
			{
				return devices.ContainsKey(device.UniqueServiceName);
			}
		}

		public bool Contains(string usn)
		{
			lock (collectionLock)
			{
				return devices.ContainsKey(usn);
			}
		}

		public void CopyTo(DeviceInfo[] array, int arrayIndex)
		{
			lock (collectionLock)
			{
				devices.Values.CopyTo(array, arrayIndex);
			}
		}

		public void Clear()
		{
			lock (collectionLock)
			{
				DeviceInfo[] array = devices.Select((KeyValuePair<string, DeviceInfo> arg) => arg.Value).ToArray();
				DeviceInfo[] array2 = array;
				foreach (DeviceInfo deviceInfo in array2)
				{
					devices.Remove(deviceInfo.UniqueServiceName);
					OnDeviceRemoved(this, new DeviceInfoEventArgs(deviceInfo));
				}
			}
		}

		public IEnumerator<DeviceInfo> GetEnumerator()
		{
			lock (collectionLock)
			{
				return devices.Values.GetEnumerator();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (collectionLock)
			{
				return devices.Values.GetEnumerator();
			}
		}

		private void OnDeviceAdded(object sender, DeviceInfoEventArgs e)
		{
			this.DeviceAdded?.Invoke(sender, e);
		}

		private void OnDeviceRemoved(object sender, DeviceInfoEventArgs e)
		{
			this.DeviceRemoved?.Invoke(sender, e);
		}
	}
}
