// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.FindDeviceByCodeRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace com.samsung.multiscreen.device.requests
{
  public class FindDeviceByCodeRequest
  {
    private string targetVersion;
    private DeviceAsyncResult<Device> callback;
    private Uri requestURI;

    public FindDeviceByCodeRequest(
      Uri requestURI,
      string targetVersion,
      DeviceAsyncResult<Device> callback)
    {
      this.requestURI = requestURI;
      this.targetVersion = targetVersion;
      this.callback = callback;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        response = NetworkUtil.Get(this.requestURI.ToString(), (IDictionary<string, IList<string>>) null, new int?(20000), (string) null, (CookieCollection) null);
        if (response == null)
          this.callback.OnError(new DeviceError("error code:" + (object) response.StatusCode));
        else if (response.StatusCode == HttpStatusCode.OK)
        {
          this.parseReponse(response);
        }
        else
        {
          Logger.Trace("FindDeviceByCodeRequest ERROR: " + (object) response.StatusCode);
          Console.WriteLine("FindDeviceByCodeRequest ERROR: " + (object) response.StatusCode);
          this.callback.OnError(new DeviceError("Error code = " + (object) response.StatusCode));
        }
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError("Invalid request URL"));
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

    protected internal virtual void parseReponse(HttpWebResponse response)
    {
      try
      {
        string end = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        Console.WriteLine("FindDeviceByCodeRequest parseReponse = " + end);
        Device deviceWithCapability = DeviceFactory.ParseDeviceWithCapability(end, this.targetVersion);
        if (deviceWithCapability == null)
        {
          Logger.Trace("FindDeviceByCodeRequest readResponse() FAILED TO CREATE DEVICE");
          Console.WriteLine("FindDeviceByCodeRequest readResponse() FAILED TO CREATE DEVICE");
          this.callback.OnError(new DeviceError("Could not create device"));
        }
        else
          this.callback.OnResult(deviceWithCapability);
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError("Unable to parse device data"));
      }
    }
  }
}
