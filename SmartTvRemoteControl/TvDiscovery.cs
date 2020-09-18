using Networking;
using SmartView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UPnP;
using UPnP.DataContracts;

namespace SmartTvRemoteControl
{
    public class TvDiscovery : IDeviceDiscovery, IDisposable
    {
        private IDeviceDiscovery upnpDiscovery;

        private INetworkTransport transport;

        private DevicePool devicePool;

        private ThreadSafeCollection<TvDiscovery.Tv2014InitInfo> foundTvs = new ThreadSafeCollection<TvDiscovery.Tv2014InitInfo>();



        public event EventHandler<DeviceInfoEventArgs> DeviceConnected;
        public event EventHandler<DeviceInfoEventArgs> DeviceUpdated;
        public event EventHandler<DeviceInfoEventArgs> DeviceDisconnected;

        public TvDiscovery(IDeviceDiscovery upnpDiscovery, INetworkTransport networkTransport)
        {
            this.transport = networkTransport;
            this.upnpDiscovery = upnpDiscovery;
            this.upnpDiscovery.DeviceConnected += new EventHandler<DeviceInfoEventArgs>(this.upnpDiscovery_DeviceConnected);
            this.upnpDiscovery.DeviceDisconnected += new EventHandler<DeviceInfoEventArgs>(this.upnpDiscovery_DeviceDisconnected);
            this.devicePool = new DevicePool();
            this.devicePool.DeviceAdded += new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceAdded);
            this.devicePool.DeviceRemoved += new EventHandler<DeviceInfoEventArgs>(this.devicePool_DeviceRemoved);
        }

        private void upnpDiscovery_DeviceConnected(object sender, DeviceInfoEventArgs e)
        {
            if (e.DeviceInfo.DeviceType != "urn:dial-multiscreen-org:device:dialreceiver:1")
            {
                Logger instance = Logger.Instance;
                object[] deviceType = new object[] { e.DeviceInfo.DeviceType, e.DeviceInfo.DeviceAddress };
                instance.LogMessageFormat("upnpDiscovery_DeviceConnected Device is not Dial Receiver, TV type is {0}, with address: {1}.", deviceType);
                return;
            }
            TvDiscovery.Tv2014InitInfo tv2014Data = TvDiscovery.GetTv2014Data(e.DeviceInfo);
            if (tv2014Data != null)
            {
                Console.WriteLine("Model 2014 Samsung TV found.");
                this.foundTvs.Add(tv2014Data);
                return;
            }
            Console.WriteLine("Pre 2014 Samsung TV found");
            this.devicePool.Add(e.DeviceInfo);
        }

        private static TvDiscovery.Tv2014InitInfo GetTv2014Data(DeviceInfo deviceInfo)
        {
            TvDiscovery.Tv2014InitInfo tv2014InitInfo;
            using (TextReader stringReader = new StringReader(deviceInfo.SourceXml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stringReader);
                XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Capability", "http://www.sec.co.kr/dlna");
                if (elementsByTagName == null || elementsByTagName.Count == 0)
                {
                    tv2014InitInfo = null;
                }
                else
                {
                    foreach (XmlElement xmlElement in elementsByTagName)
                    {
                        if (xmlElement.GetAttribute("name") != "samsung:multiscreen:1")
                        {
                            continue;
                        }
                        UriBuilder uriBuilder = new UriBuilder(deviceInfo.DeviceAddress)
                        {
                            Port = int.Parse(xmlElement.GetAttribute("port")),
                            Path = xmlElement.GetAttribute("location")
                        };
                        TvDiscovery.Tv2014InitInfo tv2014InitInfo1 = new TvDiscovery.Tv2014InitInfo()
                        {
                            DeviceInfo = deviceInfo,
                            ServiceUri = uriBuilder.Uri
                        };
                        tv2014InitInfo = tv2014InitInfo1;
                        return tv2014InitInfo;
                    }
                    return null;
                }
            }
            return tv2014InitInfo;
        }

        private void upnpDiscovery_DeviceDisconnected(object sender, DeviceInfoEventArgs e)
        {
            TvDiscovery.Tv2014InitInfo tv2014InitInfo = this.foundTvs.Find((TvDiscovery.Tv2014InitInfo arg) => arg.DeviceInfo.UniqueServiceName == e.DeviceInfo.UniqueServiceName);
            if (tv2014InitInfo != null)
            {
                this.foundTvs.Remove(tv2014InitInfo);
            }
            this.devicePool.Remove(e.DeviceInfo.UniqueServiceName);
        }

        private void devicePool_DeviceAdded(object sender, DeviceInfoEventArgs e)
        {
            this.OnDeviceConnected(this, e);
        }

        private void devicePool_DeviceRemoved(object sender, DeviceInfoEventArgs e)
        {
            this.OnDeviceDisconnected(this, e);
        }

        private void OnDeviceConnected(object sender, DeviceInfoEventArgs e)
        {
            EventHandler<DeviceInfoEventArgs> eventHandler = this.DeviceConnected;
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        private void OnDeviceDisconnected(object sender, DeviceInfoEventArgs e)
        {
            EventHandler<DeviceInfoEventArgs> eventHandler = this.DeviceDisconnected;
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Scan()
        {
            throw new NotImplementedException();
        }

        private class Tv2014InitInfo
        {
            public DeviceInfo DeviceInfo;

            public Uri ServiceUri;

            public Tv2014InitInfo()
            {
            }
        }
    }
}
