using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Networking.Native;
using SmartTvRemoteControl.ConfigExtensions;
using SmartView2.Devices;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UPnP;
using Wrapper;
using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;

namespace SmartTvRemoteControl
{
    class Program
    {
        public static IConfigurationRoot configuration;
        private static DeviceController deviceController;

        static async Task Main(string[] args)
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            LoadController();
            var devices = await deviceController.ExecuteDiscovery();
            while(devices.Length == 0)
            {
                devices = await deviceController.ExecuteDiscovery();
            }
            await deviceController.ConnectToDevice(devices.First());
            Console.WriteLine("Enter PIN");
            var pin = Console.ReadLine();
            await deviceController.SetPin(pin);
            Console.WriteLine(deviceController.CurrentDevice.CurrentSource.Title);
        }



        static void LoadController()
        {
            Guid deviceId = Guid.Parse(configuration.GetSection("DeviceId").Value);
            if (deviceId == Guid.Empty)
            {
                Console.WriteLine("No saved device ID found");
                Guid guid = Guid.NewGuid();
                deviceId = guid;
                configuration.GetSection("DeviceId").Value = guid.ToString();
            }
            else
            {
                Console.WriteLine("Previously saved device ID found");
            }
            var transportFactory = new TransportFactory();
            var devicePairing = new DevicePairing(deviceId, transportFactory, new SpcApiWrapper());
            IPlayerNotificationProvider playerNotificationProvider = new PlayerNotificationProvider();
            IDeviceListener uPnPDeviceListener = new UPnPDeviceListener(new NetworkInfoProvider(), transportFactory);
            IDeviceDiscovery uPnPDeviceDiscovery = new UPnPDeviceDiscovery(transportFactory, uPnPDeviceListener);
            IDeviceDiscovery tvDiscovery = new TvDiscovery(uPnPDeviceDiscovery, new TcpWebTransport(TimeSpan.FromSeconds(5)));
            deviceController = new DeviceController(tvDiscovery, devicePairing, playerNotificationProvider, new DeviceSettingProvider(configuration));
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configuration = configurationBuilder.Add<WritableJsonConfigurationSource>(
                s =>
                {
                    s.FileProvider = null;
                    s.Path = "appsettings.json";
                    s.Optional = false;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                }).Build();
        }
    }
}
