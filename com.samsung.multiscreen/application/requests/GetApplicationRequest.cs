// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.requests.GetApplicationRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.device;
using com.samsung.multiscreen.net.dial;
using System;
using System.Threading;

namespace com.samsung.multiscreen.application.requests
{
  public class GetApplicationRequest : ApplicationAsyncResult<DialApplication>
  {
    private string runTitle;
    private Device device;
    private Uri appURI;
    private DialClient dialClient;
    private ApplicationAsyncResult<Application> callback;

    public GetApplicationRequest(
      string runTitle,
      Uri appURI,
      Device device,
      DialClient dialClient,
      ApplicationAsyncResult<Application> callback)
    {
      this.runTitle = runTitle;
      this.appURI = appURI;
      this.device = device;
      this.dialClient = dialClient;
      this.callback = callback;
    }

    public void OnResult(DialApplication dialApplication)
    {
      if (dialApplication != null)
        this.callback.OnResult(this.createApplication(dialApplication));
      else
        this.callback.OnError(new ApplicationError("not found"));
    }

    public void OnError(ApplicationError e) => this.callback.OnError(e);

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data) => this.dialClient.GetApplication(this.runTitle, (ApplicationAsyncResult<DialApplication>) this);

    protected internal virtual Application createApplication(
      DialApplication dialApplication)
    {
      Application.Status initialStatus = Application.StatusFromString(dialApplication.State.ToLower());
      string installURL = "";
      if (initialStatus.Equals((object) Application.Status.INSTALLABLE))
      {
        string[] strArray = dialApplication.State.Split('=');
        if (strArray.Length == 2)
          installURL = strArray[1];
      }
      return new Application(this.device, this.appURI, this.runTitle, initialStatus, dialApplication.RelLink, installURL);
    }
  }
}
