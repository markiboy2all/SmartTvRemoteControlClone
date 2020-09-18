// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.requests.TerminateApplicationRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.dial;
using com.samsung.multiscreen.util;
using System;
using System.Threading;

namespace com.samsung.multiscreen.application.requests
{
  public class TerminateApplicationRequest : ApplicationAsyncResult<bool>
  {
    private string runTitle;
    private string link;
    private Uri dialURI;
    private ApplicationAsyncResult<bool> callback;

    public TerminateApplicationRequest(
      string runTitle,
      string link,
      Uri dialURI,
      ApplicationAsyncResult<bool> callback)
    {
      this.runTitle = runTitle;
      this.link = link;
      this.dialURI = dialURI;
      this.callback = callback;
    }

    public void OnResult(bool result) => this.callback.OnResult(result);

    public void OnError(ApplicationError e)
    {
      Logger.Debug("TerminateApplicationpRequest: exception: " + e.ToString());
      this.callback.OnError(e);
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      Logger.Debug("TerminateApplicationRequest: runTitle: " + this.runTitle + " link: " + this.link);
      new DialClient(this.dialURI.ToString()).StopApplication(this.runTitle, this.link, (ApplicationAsyncResult<bool>) this);
    }
  }
}
