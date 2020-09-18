// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.PinCodeRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

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
  public class PinCodeRequest
  {
    private Uri restEndpoint;
    private DeviceAsyncResult<bool> callback;
    private bool isShowPinCode;

    public PinCodeRequest(Uri endpoint, DeviceAsyncResult<bool> callback, bool isShowPinCode)
    {
      this.restEndpoint = endpoint;
      this.callback = callback;
      this.isShowPinCode = isShowPinCode;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    private void PerformRequest(object data)
    {
      string url = this.restEndpoint.ToString();
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        JSONRPCMessage jsonrpcMessage = !this.isShowPinCode ? new JSONRPCMessage(JSONRPCMessage.MessageType.MESSAGE, "ms.device.hidePinCode") : new JSONRPCMessage(JSONRPCMessage.MessageType.MESSAGE, "ms.device.showPinCode");
        IDictionary<string, IList<string>> headers = NetworkUtil.InitPOSTHeaders(this.restEndpoint);
        response = NetworkUtil.Post(url, headers, jsonrpcMessage.toJSONString(), new int?(10000), "application/json");
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
      try
      {
        JSONRPCMessage withJsonData = JSONRPCMessage.CreateWithJSONData(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        Logger.Trace("ShowPinCodeRequest onResult() rpcMessage: " + (object) withJsonData);
        Console.WriteLine("ShowPinCodeRequest onResult() rpcMessage: " + (object) withJsonData);
        if (withJsonData == null || withJsonData.IsError())
        {
          Logger.Trace("ShowPinCodeRequest onResult() rpc error: " + (object) withJsonData.GetError());
          Console.WriteLine("ShowPinCodeRequest onResult() rpc error: " + (object) withJsonData.GetError());
          this.callback.OnError(DeviceError.CreateWithJSONRPCError(withJsonData.GetError()));
        }
        else
          this.callback.OnResult(withJsonData.GetResult().ContainsKey("success"));
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError(ex.ToString()));
      }
    }
  }
}
