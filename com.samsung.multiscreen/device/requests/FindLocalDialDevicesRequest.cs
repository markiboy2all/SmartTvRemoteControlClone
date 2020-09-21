// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.FindLocalDialDevicesRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.device.requests.impl;
using com.samsung.multiscreen.net;
using com.samsung.multiscreen.net.ssdp;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.device.requests
{
  public class FindLocalDialDevicesRequest : SSDPSearchListener
  {
    protected internal const string DIAL_URN = "urn:dial-multiscreen-org:service:dial:1";
    private IList<Device> deviceResults;
    private IList<DeviceError> deviceErrors;
    private DeviceAsyncResult<IList<Device>> callback;
    private SSDPSearch search;
    private int timeout;
    private string targetVersion;

    public FindLocalDialDevicesRequest(
      int timeout,
      string targetVersion,
      DeviceAsyncResult<IList<Device>> callback)
    {
      this.deviceResults = (IList<Device>) new List<Device>();
      this.deviceErrors = (IList<DeviceError>) new List<DeviceError>();
      this.timeout = timeout;
      this.targetVersion = targetVersion;
      this.callback = callback;
    }

    public void run() => this.performRequest();

    public void OnResult(SSDPSearchResult result)
    {
    }

    public void OnResults(IList<SSDPSearchResult> ssdpResults)
    {
      Logger.Trace("FindLocalDialDevicesRequest Results() size: " + (object) ssdpResults.Count);
      Console.WriteLine("FindLocalDialDevicesRequest Results() size: " + (object) ssdpResults.Count);
      if (ssdpResults.Count == 0)
      {
        this.callback.OnResult(this.deviceResults);
      }
      else
      {
        Logger.Trace("DIAL ssdpSearchResultsSize: " + (object) ssdpResults.Count);
        DeviceResultAccumulator deviceAccumulator = new DeviceResultAccumulator((IList<Device>) new List<Device>(), (IList<DeviceError>) new List<DeviceError>(), ssdpResults.Count, this.callback);
        try
        {
          AsyncResult<DeviceURIResult> callback = (AsyncResult<DeviceURIResult>) new FindLocalDialDevicesRequest.DeviceURICallback(deviceAccumulator);
          foreach (SSDPSearchResult ssdpResult in (IEnumerable<SSDPSearchResult>) ssdpResults)
          {
            Logger.Trace("DIAL search result: " + (object) ssdpResult);
            new GetDialDeviceDescriptorRequest(new Uri(ssdpResult.Location), this.targetVersion, callback).run();
          }
        }
        catch (Exception ex)
        {
          Logger.Trace("FindLocalDialDevicesRequest FAILED: " + (object) ex);
          Console.WriteLine(ex.ToString());
          Console.Write(ex.StackTrace);
        }
      }
    }

    protected internal virtual void performRequest()
    {
      this.search = new SSDPSearch("urn:dial-multiscreen-org:service:dial:1");
      this.search.Start(this.timeout, (SSDPSearchListener) this);
    }

    private class DeviceURICallback : AsyncResult<DeviceURIResult>
    {
      private DeviceResultAccumulator deviceAccumulator;

      public DeviceURICallback(DeviceResultAccumulator deviceAccumulator) => this.deviceAccumulator = deviceAccumulator;

      public void onResult(DeviceURIResult result)
      {
        Logger.Trace("FindLocalDialDevicesRequest Res: " + result.ToString() + "\n");
        Console.WriteLine(" FindLocalDialDevicesRequest Res: " + result.ToString() + "\n");
        new GetDialDeviceRequest(result, (DeviceAsyncResult<Device>) this.deviceAccumulator).run();
      }

      public void onException(Exception e)
      {
        Logger.Trace("FindLocalDevicesRequest: got exception: " + e.ToString());
        Console.WriteLine("FindLocalDevicesRequest: got exception: " + e.ToString() + "\n");
        this.deviceAccumulator.OnError(new DeviceError(e.Message));
      }
    }
  }
}
