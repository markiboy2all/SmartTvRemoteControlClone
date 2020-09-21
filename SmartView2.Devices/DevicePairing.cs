// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DevicePairing
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using Networking;
using SmartView2.Core;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wrapper;

namespace SmartView2.Devices
{
  public class DevicePairing : IDevicePairing, IDisposable
  {
    private const string AppId = "12345";
    private const string UserId = "654321";
    private const int PairingPort = 8080;
    private readonly Guid DeviceId = Guid.Empty;
    private readonly INetworkTransportFactory factory;
    private readonly SpcApiWrapper spcApiBridge;
    private Uri currentAddress;
    private int sessionId;
    private bool isPairingStarted;

    public SpcApiWrapper SpcApi => this.spcApiBridge;

    public int SessionId => this.sessionId;

    public bool EncryptionEnabled { get; private set; }

    public DevicePairing(
      Guid deviceId,
      INetworkTransportFactory transportFactory,
      SpcApiWrapper spcApiBridge)
    {
      if (deviceId == Guid.Empty)
        throw new ArgumentException(nameof (deviceId));
      if (transportFactory == null)
        throw new ArgumentNullException(nameof (transportFactory));
      if (spcApiBridge == null)
        throw new ArgumentNullException(nameof (spcApiBridge));
      this.DeviceId = deviceId;
      this.factory = transportFactory;
      this.spcApiBridge = spcApiBridge;
    }

    public async Task<bool> StartPairingAsync(Uri address)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]StartPairingAsync started... ");
      INetworkTransport transport = this.factory.CreatePairingTransport();
      if (!await this.CheckPinPageOnTvAsync(transport, address.Host, 8080))
        return false;
      await this.ShowPinPageOnTvAsync(transport, address.Host, 8080);
      this.currentAddress = new UriBuilder(address)
      {
        Port = 8080
      }.Uri;
      this.isPairingStarted = true;
      return true;
    }

    public async Task<string> TryPairAsync(Uri address, string pin)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]TryPairAsync started... ");
      if (string.IsNullOrEmpty(pin))
        return string.Empty;
      INetworkTransport transport = this.factory.CreatePairingTransport();
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]Call HelloExchangeAsync.");
      string requestId = await this.HelloExchangeAsync(transport, address.Host, 8080, this.spcApiBridge, pin);
      if (string.IsNullOrEmpty(requestId))
      {
        Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]Hello string is null.");
        return string.Empty;
      }
      this.sessionId = await this.AcknowledgeExchangeAsync(transport, address.Host, 8080, this.spcApiBridge, requestId);
      //this.StartCompanionService(transport, address.Host);
      this.isPairingStarted = true;
      return this.sessionId.ToString();
    }

    public async Task<string> EnterPinAsync(string pin)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]EnterPinAsync started... ");
      if (!this.isPairingStarted)
        throw new InvalidOperationException("Pairing not started");
      if (string.IsNullOrEmpty(pin))
        return string.Empty;
      INetworkTransport transport = this.factory.CreatePairingTransport();
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]First step of pairing.");
      await this.FirstStepOfPairingAsync(transport, this.currentAddress.Host, this.currentAddress.Port);
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]Call HelloExchangeAsync.");
      string requestId = await this.HelloExchangeAsync(transport, this.currentAddress.Host, this.currentAddress.Port, this.spcApiBridge, pin);
      if (string.IsNullOrEmpty(requestId))
      {
        Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]Hello string is null.");
        return string.Empty;
      }
      await this.ClosePinPageOnTvAsync(transport, this.currentAddress.Host, this.currentAddress.Port);
      this.sessionId = await this.AcknowledgeExchangeAsync(transport, this.currentAddress.Host, this.currentAddress.Port, this.spcApiBridge, requestId);
      //this.StartCompanionService(transport, this.currentAddress.Host);
      return this.sessionId.ToString();
    }

    public async Task ClosePinPageAsync()
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]ClosePinPageAsync started...");
      INetworkTransport transport = this.factory.CreatePairingTransport();
      await this.ClosePinPageOnTvAsync(transport, this.currentAddress.Host, this.currentAddress.Port);
    }

    public void Dispose()
    {
      if (this.spcApiBridge == null)
        return;
      this.spcApiBridge.Dispose();
    }

    private async Task FirstStepOfPairingAsync(
      INetworkTransport transport,
      string host,
      int port)
    {
      HttpMessage request = this.GenerateGetMessage(host, port, this.GetRequestUri(0U, "12345", this.DeviceId) + "&type=1");
      HttpMessage response = await transport.SendRequestAsync(request);
      Logger.Instance.LogMessageFormat("Server Response: {0}", (object) response);
    }

    private async Task<bool> CheckPinPageOnTvAsync(
      INetworkTransport transport,
      string host,
      int port)
    {
      HttpMessage request = this.GenerateGetMessage(host, port, "/ws/apps/CloudPINPage");
      HttpMessage response = await transport.SendRequestAsync(request);
      Regex exp = new Regex("\\<state\\>(?<state>.*)\\<\\/state\\>");
      Match match = exp.Match(response.Content);
      return match.Success && match.Groups["state"].Value.ToLower() == "stopped";
    }

    private async Task ShowPinPageOnTvAsync(INetworkTransport transport, string host, int port)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]ShowPinPageOnTvAsync started... ");
      HttpMessage request = this.GeneratePostMessage(host, port, "/ws/apps/CloudPINPage", "pin4");
      HttpMessage httpMessage = await transport.SendRequestAsync(request);
    }

    private async Task ClosePinPageOnTvAsync(
      INetworkTransport transport,
      string host,
      int port)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]ClosePinPageOnTvAsync started...");
      HttpMessage request = this.GenerateDeleteMessage(host, port, "/ws/apps/CloudPINPage/run");
      HttpMessage httpMessage = await transport.SendRequestAsync(request);
    }

    private async void StartCompanionService(INetworkTransport transport, string host)
    {
      HttpMessage request = this.GenerateGetMessage(host, 8000, "/common/1.0.0/service/startService?appID=com.samsung.companion");
      try
      {
        HttpMessage httpMessage = await transport.SendRequestAsync(request);
      }
      catch
      {
      }
    }

    private async Task<string> HelloExchangeAsync(
      INetworkTransport transport,
      string host,
      int port,
      SpcApiWrapper spcApiBridge,
      string pin)
    {
      Logger.Instance.LogMessageFormat("[SmartView2][DevicePairing]HelloExchangeAsync started... ");
      spcApiBridge.Initialize("654321");
      string serverHello = await Task.Run<string>((Func<string>) (() => spcApiBridge.GenerateServerHello(pin)));
      string data = "{\"auth_Data\":{\"auth_type\":\"SPC\",\"GeneratorServerHello\":\"" + serverHello + "\"}}";
      HttpMessage request = this.GeneratePostMessage(host, port, this.GetRequestUri(1U, "12345", this.DeviceId), data);
      HttpMessage response = await transport.SendRequestAsync(request);
      Logger.Instance.LogMessageFormat("Server Response: {0}", (object) response);
      string requestId = "";
      string clientHello = "";
      Regex regex = new Regex("\\\\\"request_id\\\\\":\\\\\"(?<requestId>[\\d]+)\\\\\"");
      if (regex.IsMatch(response.Content))
        requestId = regex.Match(response.Content).Groups["requestId"].Value;
      regex = new Regex("\\\\\"GeneratorClientHello\\\\\":\\\\\"(?<data>[\\w\\d]+)\\\\\"");
      clientHello = regex.Match(response.Content).Groups["data"].Value;
      return await Task.Run<bool>((Func<bool>) (() => spcApiBridge.ParseClientHello(pin, clientHello))) ? requestId : string.Empty;
    }

    private async Task<int> AcknowledgeExchangeAsync(
      INetworkTransport transport,
      string host,
      int port,
      SpcApiWrapper spcApiBridge,
      string requestId)
    {
      string serverAckMessage = await Task.Run<string>((Func<string>) (() => spcApiBridge.GenerateServerAcknowledge()));
      string data = "{\"auth_Data\":{\"auth_type\":\"SPC\",\"request_id\":\"" + requestId + "\",\"ServerAckMsg\":\"" + serverAckMessage + "\"}}";
      HttpMessage request = this.GeneratePostMessage(host, port, this.GetRequestUri(2U, "12345", this.DeviceId), data);
      HttpMessage response = await transport.SendRequestAsync(request);
      Logger.Instance.LogMessageFormat("Server Response: {0}", (object) response);
      int sessionId = 0;
      string clientAck = string.Empty;
      this.EncryptionEnabled = response["secure-mode"].ToLower() == "true";
      Regex regex = new Regex("\\\\\"session_id\\\\\":\\\\\"(?<sessionId>[\\d]+)\\\\\"");
      if (regex.IsMatch(response.Content))
        int.TryParse(regex.Match(response.Content).Groups["sessionId"].Value, out sessionId);
      regex = new Regex("\\\\\"ClientAckMsg\\\\\":\\\\\"(?<data>[\\w\\d]+)\\\\\"");
      clientAck = regex.Match(response.Content).Groups["data"].Value;
      if (!await Task.Run<bool>((Func<bool>) (() => spcApiBridge.ParseClientAcknowledge(clientAck))))
        throw new Exception("Parse client ac message failed.");
      return sessionId;
    }

    private HttpMessage GeneratePostMessage(
      string host,
      int port,
      string uri,
      string content) => new HttpMessage(new Uri(string.Format("http://{0}:{1}", (object) host, (object) port)), "POST", uri)
    {
      ["Content-Type"] = "application/json",
      ["Content-Length"] = content.Length.ToString(),
      ["Host"] = host + ":" + port.ToString(),
      ["Connection"] = "Keep-Alive",
      ["User-Agent"] = "httpclient v0.1",
      Content = content
    };

    private HttpMessage GenerateGetMessage(string host, int port, string uri) => new HttpMessage(new Uri(string.Format("http://{0}:{1}", (object) host, (object) port)), "GET", uri)
    {
      ["Host"] = host + ":" + port.ToString(),
      ["Connection"] = "Keep-Alive",
      ["User-Agent"] = "httpclient v0.1"
    };

    private HttpMessage GenerateDeleteMessage(string host, int port, string uri) => new HttpMessage(new Uri(string.Format("http://{0}:{1}", (object) host, (object) port)), "DELETE", uri)
    {
      ["Host"] = host + ":" + port.ToString(),
      ["Connection"] = "Keep-Alive",
      ["User-Agent"] = "httpclient v0.1"
    };

    private string GetRequestUri(uint step, string appId, Guid deviceId) => string.Format("/ws/pairing?step={0}&app_id={1}&device_id={2}", (object) step, (object) appId, (object) deviceId.ToString());
  }
}
