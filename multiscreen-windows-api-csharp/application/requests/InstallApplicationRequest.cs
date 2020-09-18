// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.requests.InstallApplicationRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace com.samsung.multiscreen.application.requests
{
  public class InstallApplicationRequest
  {
    private Uri installURI;
    private ApplicationAsyncResult<bool> callback;

    public InstallApplicationRequest(Uri installURI, ApplicationAsyncResult<bool> callback)
    {
      this.installURI = installURI;
      this.callback = callback;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      Logger.Debug("InstallApplicationRequest URI: " + (object) this.installURI);
      try
      {
        IDictionary<string, IList<string>> headers = NetworkUtil.InitGetHeaders(this.installURI);
        HttpWebResponse httpWebResponse = NetworkUtil.Get(this.installURI.ToString(), headers, new int?(2000), (string) null, (CookieCollection) null);
        if (httpWebResponse == null)
          this.callback.OnResult(false);
        else if (httpWebResponse.StatusCode == HttpStatusCode.OK)
          this.callback.OnResult(true);
        else
          this.callback.OnResult(false);
      }
      catch (Exception ex)
      {
        this.callback.OnError(ApplicationError.CreateWithException(ex));
      }
    }
  }
}
