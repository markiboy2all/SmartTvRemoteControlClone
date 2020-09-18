// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.GetDeviceRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net;
using com.samsung.multiscreen.net.json;
using com.samsung.multiscreen.util;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace com.samsung.multiscreen.device.requests
{
  public class GetDeviceRequest
  {
    private DeviceAsyncResult<Device> callback;
    private Uri requestURI;

    public GetDeviceRequest(Uri requestURI, DeviceAsyncResult<Device> callback)
    {
      this.requestURI = requestURI;
      this.callback = callback;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      JSONRPCMessage jsonrpcMessage = new JSONRPCMessage(JSONRPCMessage.MessageType.MESSAGE, "ms.device.getInfo");
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        response = NetworkUtil.Post(this.requestURI.ToString(), NetworkUtil.InitJSONPostHeaders(this.requestURI), jsonrpcMessage.toJSONString(), new int?(0), "application/json");
        if (response == null)
          this.callback.OnError(new DeviceError("Error code=" + (object) response.StatusCode));
        else if (response.StatusCode == HttpStatusCode.OK)
        {
          this.parseResponse(response);
        }
        else
        {
          Logger.Trace("GetDeviceRequest ERROR: " + (object) response.StatusCode);
          Console.WriteLine("GetDeviceRequest ERROR: " + (object) response.StatusCode);
          this.callback.OnError(new DeviceError("Error code=" + (object) response.StatusCode));
        }
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError(ex.StackTrace));
        Logger.Trace("GetDeviceRequest FAILED: " + (object) ex);
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
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

    private void parseResponse(HttpWebResponse response)
    {
      try
      {
        JSONRPCMessage withJsonData = JSONRPCMessage.CreateWithJSONData(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        Logger.Trace("getDevice() rpcMessage: " + (object) withJsonData);
        if (withJsonData.IsError())
        {
          this.callback.OnError(DeviceError.CreateWithJSONRPCError(withJsonData.GetError()));
        }
        else
        {
          Device withMap = DeviceFactory.CreateWithMap(withJsonData.GetResult());
          if (withMap != null)
          {
            this.callback.OnResult(withMap);
          }
          else
          {
            Logger.Trace("GetDeviceRequest FAILED TO CREATE DEVICE");
            this.callback.OnError(new DeviceError("Could not create device"));
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
    }
  }
}
