// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.GetChannelInfoRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.channel.info;
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
  public class GetChannelInfoRequest
  {
    private Uri restEndpoint;
    private string channelId;
    private DeviceAsyncResult<ChannelInfo> callback;

    public GetChannelInfoRequest(
      Uri endpoint,
      string channelId,
      DeviceAsyncResult<ChannelInfo> callback)
    {
      this.restEndpoint = endpoint;
      this.channelId = channelId;
      this.callback = callback;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    private void PerformRequest(object data)
    {
      JSONRPCMessage jsonrpcMessage = new JSONRPCMessage(JSONRPCMessage.MessageType.MESSAGE, "ms.device.getChannelInfo");
      jsonrpcMessage.Params[JSONRPCMessage.KEY_ID] = (object) this.channelId;
      Console.WriteLine("Request Message = " + (object) jsonrpcMessage);
      Console.WriteLine("uri = " + this.restEndpoint.ToString());
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        IDictionary<string, IList<string>> headers = NetworkUtil.InitPOSTHeaders(this.restEndpoint);
        response = NetworkUtil.Post(this.restEndpoint.ToString(), headers, jsonrpcMessage.toJSONString(), new int?(5000), "application/json");
        if (response == null)
          this.callback.OnError(new DeviceError("Error code=" + (object) response.StatusCode));
        else if (response.StatusCode == HttpStatusCode.OK)
          this.parseResponse(response);
        else
          this.callback.OnError(new DeviceError("Error code=" + (object) response.StatusCode));
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

    protected internal virtual void parseResponse(HttpWebResponse response)
    {
      try
      {
        JSONRPCMessage withJsonData = JSONRPCMessage.CreateWithJSONData(new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd());
        Logger.Debug("GetChannelInfoRequest result rpcMessage: " + (object) withJsonData);
        if (withJsonData.IsError())
        {
          Logger.Debug("GetChannelInfoRequest result rpc error: " + (object) withJsonData.GetError());
          this.callback.OnError(DeviceError.CreateWithJSONRPCError(withJsonData.GetError()));
        }
        else
          this.callback.OnResult(ChannelInfo.createWithMap(withJsonData.GetResult()));
      }
      catch (Exception ex)
      {
        this.callback.OnError(new DeviceError(ex.ToString()));
      }
    }
  }
}
