// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.GetDialDeviceRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.device.requests.impl;
using com.samsung.multiscreen.net;
using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace com.samsung.multiscreen.device.requests
{
  public class GetDialDeviceRequest
  {
    private bool responseReceived;
    private DeviceAsyncResult<Device> callback;
    private DeviceURIResult deviceURIs;

    public GetDialDeviceRequest(DeviceURIResult deviceURIs, DeviceAsyncResult<Device> callback)
    {
      this.deviceURIs = deviceURIs;
      this.callback = callback;
      this.responseReceived = false;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      JSONRPCMessage jsonrpcMessage = new JSONRPCMessage(JSONRPCMessage.MessageType.MESSAGE, "ms.device.getInfo");
      string url = this.deviceURIs.ServiceURI.ToString();
      Console.WriteLine("HTTP GET: " + url);
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        response = NetworkUtil.Get(url, (IDictionary<string, IList<string>>) null, new int?(5000), (string) null, (CookieCollection) null);
        if (response == null)
          this.callback.OnError(new DeviceError("error code:" + (object) response.StatusCode));
        else if (response.StatusCode == HttpStatusCode.OK)
          this.handleResponse(response);
        else
          this.callback.OnError(new DeviceError("error code = " + (object) response.StatusCode));
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError(ex.ToString()));
      }
      finally
      {
        if (response != null)
        {
          try
          {
            response.Close();
            response.Dispose();
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    protected internal virtual void handleResponse(HttpWebResponse response)
    {
      if (this.responseReceived)
        return;
      this.responseReceived = true;
      Logger.Trace("response() status: " + (object) response.StatusCode);
      Console.WriteLine("response() status: " + (object) response.StatusCode);
      if (response.StatusCode == HttpStatusCode.OK)
      {
        try
        {
          IDictionary<string, object> map = JSONUtil.Parse(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
          if (map == null)
          {
            this.callback.OnError(new DeviceError("Could not parse JSON data into Dictionary"));
          }
          else
          {
            Device withMap = DeviceFactory.CreateWithMap(map);
            if (withMap != null)
            {
              this.callback.OnResult(withMap);
            }
            else
            {
              Logger.Trace("GetDialDeviceRequest FAILED TO CREATE DEVICE");
              Console.WriteLine("GetDialDeviceRequest FAILED TO CREATE DEVICE");
              this.callback.OnError(new DeviceError("Could not create device"));
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Parse JSON error: " + ex.ToString());
          this.callback.OnError(new DeviceError("Could not create device"));
        }
      }
      else
        this.callback.OnError(new DeviceError("error code = " + (object) response.StatusCode));
    }
  }
}
