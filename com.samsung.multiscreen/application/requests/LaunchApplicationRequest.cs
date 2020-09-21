// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.requests.LaunchApplicationRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.net.dial;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace com.samsung.multiscreen.application.requests
{
  public class LaunchApplicationRequest : ApplicationAsyncResult<bool>
  {
    private string runTitle;
    private IDictionary<string, string> parameters;
    private IDictionary<string, List<string>> additionalHeaders;
    private Uri dialURI;
    private ApplicationAsyncResult<bool> callback;
    private long timeout = 30000;
    private long sleepPeriod = 1000;

    public LaunchApplicationRequest(
      string runTitle,
      IDictionary<string, string> parameters,
      IDictionary<string, List<string>> additionalHeaders,
      Uri dialURI,
      ApplicationAsyncResult<bool> callback)
    {
      this.runTitle = runTitle;
      this.parameters = parameters;
      this.additionalHeaders = additionalHeaders;
      this.dialURI = dialURI;
      this.callback = callback;
    }

    public void OnResult(bool result)
    {
      if (result)
      {
        Logger.Debug("Launch succeeded: start polling run state");
        this.pollApplicationState();
      }
      else
        this.callback.OnResult(result);
    }

    public void OnError(ApplicationError e) => this.callback.OnError(e);

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      DialClient dialClient = new DialClient(this.dialURI.ToString());
      string payload = LaunchApplicationRequest.encodeAppParameters(this.parameters);
      Logger.Debug("LaunchApplicationRequest: Launching " + this.runTitle + " with parameters: " + payload);
      dialClient.LaunchApplication(this.runTitle, payload, this.additionalHeaders, (ApplicationAsyncResult<bool>) this);
    }

    protected internal virtual void pollApplicationState()
    {
      long startTime = DateTimeHelperClass.CurrentUnixTimeMillis();
      new GetApplicationStateRequest(this.runTitle, new DialClient(this.dialURI.ToString()), (ApplicationAsyncResult<Application.Status>) new LaunchApplicationRequest.PollApplicationStateCallback(this, startTime)).run();
    }

    protected internal static string encodeAppParameters(IDictionary<string, string> parameters)
    {
      if (parameters == null)
        return "";
      string str = new JavaScriptSerializer().Serialize((object) parameters);
      Logger.Debug("LaunchApplicationRequest: params: " + str);
      try
      {
        return HttpUtility.UrlEncode(str, Encoding.UTF8);
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    private class PollApplicationStateCallback : ApplicationAsyncResult<Application.Status>
    {
      private readonly LaunchApplicationRequest outerInstance;
      private long startTime;

      public PollApplicationStateCallback(LaunchApplicationRequest outerInstance, long startTime)
      {
        this.outerInstance = outerInstance;
        this.startTime = startTime;
      }

      public void OnResult(Application.Status result)
      {
        Logger.Debug("Poll status result: " + result.ToString());
        if (result == Application.Status.RUNNING)
        {
          this.outerInstance.callback.OnResult(true);
        }
        else
        {
          this.outerInstance.timeout -= DateTimeHelperClass.CurrentUnixTimeMillis() - this.startTime;
          Logger.Debug("Remaining state polling timeout: " + (object) this.outerInstance.timeout);
          if (this.outerInstance.timeout > 0L)
          {
            try
            {
              Thread.Sleep(new TimeSpan(this.outerInstance.sleepPeriod));
              this.outerInstance.pollApplicationState();
            }
            catch (Exception ex)
            {
              this.outerInstance.callback.OnResult(false);
            }
          }
          else
            this.outerInstance.callback.OnResult(false);
        }
      }

      public void OnError(ApplicationError e) => this.outerInstance.callback.OnError(e);
    }
  }
}
