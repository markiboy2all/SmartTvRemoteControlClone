// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTvRemoteInput
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartView2.Devices
{
  public class SecondTvRemoteInput : IRemoteInput, IDisposable, IKeySender
  {
    private ISecondTvTransport transport;
    private Uri localEndpoint;
    private string duid = string.Empty;
    private bool isBase64;

    public SecondTvRemoteInput(ISecondTvTransport transport, Uri localEndpoint)
    {
      if (transport == null)
        throw new ArgumentNullException(nameof (transport));
      if (localEndpoint == (Uri) null)
        throw new ArgumentNullException(nameof (localEndpoint));
      this.transport = transport;
      this.transport.NotificationReceived += new EventHandler<SecondTvNotificationEventArgs>(this.transport_NotificationReceived);
      this.localEndpoint = localEndpoint;
    }

    public void Connect(ISecondTvTransport connectTransport) => this.duid = connectTransport.SendRequest((object) new
    {
      method = "POST",
      body = new
      {
        plugin = "NNavi",
        api = "GetDUID",
        version = "1.000"
      }
    }).ToString();

    private void transport_NotificationReceived(object sender, SecondTvNotificationEventArgs e)
    {
      if (!(e.PluginName == "RemoteControl"))
        return;
      switch (e.NotificationType)
      {
        case 100:
          switch (e.NotificationText.ToLower())
          {
            case "input":
              this.OnShowInputKeyboard((object) this, EventArgs.Empty);
              return;
            case "password":
              this.OnShowPasswordKeyboard((object) this, EventArgs.Empty);
              return;
            default:
              Logger.Instance.LogMessageFormat("Remote input - Unknown NotificationText: {0}", (object) e.NotificationText);
              Debugger.Break();
              return;
          }
        case 101:
          this.OnHideKeyboard((object) this, EventArgs.Empty);
          break;
        case 102:
          this.SynchronizeKeyboard(e.NotificationText, e.NotificationArgument);
          break;
      }
    }

    private void SynchronizeKeyboard(string text, string parameter)
    {
      if (string.IsNullOrEmpty(parameter))
      {
        this.isBase64 = false;
      }
      else
      {
        if (!(parameter == "base64"))
          throw new NotImplementedException("This Case is not implemented yet due to TV's Firmware.");
        this.isBase64 = true;
      }
      string empty = string.Empty;
      string str;
      if (this.isBase64)
      {
        byte[] bytes = Convert.FromBase64String(text);
        str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      }
      else
        str = text;
      this.OnTextUpdated((object) this, new UpdateTextEventArgs(str.Trim(new char[1])));
    }

    public void SendKey(object keyCode) => this.transport.SendRequest(this.CreateRequest("SendRemoteKey", (object) "Click", keyCode, (object) false));

    public async Task SendKeyAsync(object keyCode)
    {
      object obj = await this.transport.SendRequestAsync(this.CreateRequest("SendRemoteKey", (object) "Click", keyCode, (object) false));
    }

    public async Task UpdateTextAsync(string text)
    {
      string arg = string.Empty;
      if (this.isBase64)
      {
        text = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        arg = "base64";
      }
      object obj = await this.transport.SendRequestAsync(this.CreateRequest("SendKeyString", (object) text, (object) arg, (object) string.Empty));
    }

    public async Task EndInputAsync()
    {
      object obj = await this.transport.SendRequestAsync(this.CreateRequest("SendInputEnd", (object) string.Empty, (object) string.Empty, (object) string.Empty));
    }

    private object CreateRequest(string apiName, params object[] parameters)
    {
      if (parameters.Length != 3)
        throw new ArgumentException("Remote Control Request must have 3 Parameters.");
      return (object) new
      {
        method = "POST",
        body = new
        {
          plugin = "RemoteControl",
          version = "1.000",
          api = apiName,
          param1 = this.duid,
          param2 = parameters[0],
          param3 = parameters[1],
          param4 = parameters[2]
        }
      };
    }

    public void Dispose()
    {
      if (this.transport == null)
        return;
      this.transport.NotificationReceived -= new EventHandler<SecondTvNotificationEventArgs>(this.transport_NotificationReceived);
      this.transport.Dispose();
      this.transport = (ISecondTvTransport) null;
    }

    public event EventHandler<EventArgs> ShowInputKeyboard;

    private void OnShowInputKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> showInputKeyboard = this.ShowInputKeyboard;
      if (showInputKeyboard == null)
        return;
      showInputKeyboard(sender, e);
    }

    public event EventHandler<EventArgs> ShowPasswordKeyboard;

    private void OnShowPasswordKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> passwordKeyboard = this.ShowPasswordKeyboard;
      if (passwordKeyboard == null)
        return;
      passwordKeyboard(sender, e);
    }

    public event EventHandler<UpdateTextEventArgs> TextUpdated;

    private void OnTextUpdated(object sender, UpdateTextEventArgs e)
    {
      EventHandler<UpdateTextEventArgs> textUpdated = this.TextUpdated;
      if (textUpdated == null)
        return;
      textUpdated(sender, e);
    }

    public event EventHandler<EventArgs> HideKeyboard;

    private void OnHideKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> hideKeyboard = this.HideKeyboard;
      if (hideKeyboard == null)
        return;
      hideKeyboard(sender, e);
    }
  }
}
