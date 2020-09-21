// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.requests.GetApplicationStateRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.dial;
using System.Threading;

namespace com.samsung.multiscreen.application.requests
{
  public class GetApplicationStateRequest : ApplicationAsyncResult<DialApplication>
  {
    private string runTitle;
    private DialClient dialClient;
    private ApplicationAsyncResult<Application.Status> callback;

    public GetApplicationStateRequest(
      string runTitle,
      DialClient dialClient,
      ApplicationAsyncResult<Application.Status> callback)
    {
      this.runTitle = runTitle;
      this.dialClient = dialClient;
      this.callback = callback;
    }

    public void OnResult(DialApplication dialApplication)
    {
      if (dialApplication != null)
        this.callback.OnResult(Application.StatusFromString(dialApplication.State));
      else
        this.callback.OnResult(Application.Status.STOPPED);
    }

    public void OnError(ApplicationError error) => this.callback.OnError(error);

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data) => this.dialClient.GetApplication(this.runTitle, (ApplicationAsyncResult<DialApplication>) this);
  }
}
