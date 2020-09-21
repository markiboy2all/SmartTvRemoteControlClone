// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.application.Application
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.application.requests;
using com.samsung.multiscreen.device;
using com.samsung.multiscreen.net.dial;
using com.samsung.multiscreen.net.json;
using System;
using System.Collections.Generic;

namespace com.samsung.multiscreen.application
{
  public class Application
  {
    private static string STATE_NOT_RUNNING = "Not running";
    private static string STATE_NOT_STARTED = "not started";
    private static string STATE_STARTING = "Starting";
    private static string STATE_RUNNING = "running";
    private static string STATE_STOPPED = "stopped";
    private static string STATE_INSTALLABLE = "installable";
    private string runTitle;
    private Device device;
    private Uri dialURI;
    private Application.Status lastKnownStatus;
    private string link;
    private string installURL;

    public static Application.Status StatusFromString(string state) => state != null ? (!string.Equals(state, Application.STATE_NOT_RUNNING, StringComparison.CurrentCultureIgnoreCase) ? (!string.Equals(state, Application.STATE_NOT_STARTED, StringComparison.CurrentCultureIgnoreCase) ? (!string.Equals(state, Application.STATE_STOPPED, StringComparison.CurrentCultureIgnoreCase) ? (!string.Equals(state, Application.STATE_RUNNING, StringComparison.CurrentCultureIgnoreCase) ? (!string.Equals(state, Application.STATE_STARTING, StringComparison.CurrentCultureIgnoreCase) ? (!state.Contains(Application.STATE_INSTALLABLE.ToLower()) ? Application.Status.STOPPED : Application.Status.INSTALLABLE) : Application.Status.RUNNING) : Application.Status.RUNNING) : Application.Status.STOPPED) : Application.Status.STOPPED) : Application.Status.STOPPED) : Application.Status.STOPPED;

    public Application(
      Device device,
      Uri appURI,
      string runTitle,
      Application.Status initialStatus,
      string link,
      string installURL)
    {
      this.device = device;
      this.dialURI = appURI;
      this.runTitle = runTitle;
      this.lastKnownStatus = initialStatus;
      this.link = link;
      this.installURL = installURL;
    }

    public Device Device => this.device;

    public string RunTitle => this.runTitle;

    public Application.Status LastKnownStatus => this.lastKnownStatus;

    public void UpdateStatus(
      ApplicationAsyncResult<Application.Status> callback) => new GetApplicationStateRequest(this.runTitle, new DialClient(this.dialURI.ToString()), (ApplicationAsyncResult<Application.Status>) new Application.GetApplicationStateRequestCallback(this, callback)).run();

    public void Launch(ApplicationAsyncResult<bool> callback) => this.Launch((IDictionary<string, string>) new Dictionary<string, string>(), callback);

    public void Launch(
      IDictionary<string, string> parameters,
      ApplicationAsyncResult<bool> callback)
    {
      IDictionary<string, List<string>> additionalHeaders = this.initAdditionalHeaders(parameters);
      new LaunchApplicationRequest(this.runTitle, parameters, additionalHeaders, this.dialURI, (ApplicationAsyncResult<bool>) new Application.LaunchApplicationRequestCallback(this, callback)).run();
    }

    private IDictionary<string, List<string>> initAdditionalHeaders(
      IDictionary<string, string> parameters)
    {
      string key1 = "_headers";
      IDictionary<string, List<string>> dictionary1 = (IDictionary<string, List<string>>) new Dictionary<string, List<string>>();
      if (parameters.ContainsKey(key1))
      {
        IDictionary<string, object> dictionary2 = JSONUtil.Parse(parameters[key1]);
        foreach (string key2 in (IEnumerable<string>) dictionary2.Keys)
        {
          object obj = dictionary2[key2];
          if (obj != null)
            dictionary1[key2] = new List<string>()
            {
              obj.ToString()
            };
        }
        parameters.Remove(key1);
      }
      return dictionary1;
    }

    public void Terminate(ApplicationAsyncResult<bool> callback) => new TerminateApplicationRequest(this.runTitle, this.link == null ? "run" : this.link, this.dialURI, callback).run();

    public void Install(ApplicationAsyncResult<bool> callback)
    {
      if (this.installURL == null || this.installURL.Length == 0)
      {
        callback.OnResult(false);
      }
      else
      {
        Uri installURI = new Uri(this.installURL);
        if (installURI == (Uri) null || installURI.ToString().Length == 0)
          callback.OnResult(false);
        else
          new InstallApplicationRequest(installURI, callback).run();
      }
    }

    public enum Status
    {
      STOPPED,
      RUNNING,
      INSTALLABLE,
    }

    private class GetApplicationStateRequestCallback : ApplicationAsyncResult<Application.Status>
    {
      private readonly Application outerInstance;
      private ApplicationAsyncResult<Application.Status> cb;

      public GetApplicationStateRequestCallback(
        Application outerInstance,
        ApplicationAsyncResult<Application.Status> cb)
      {
        this.outerInstance = outerInstance;
        this.cb = cb;
      }

      public void OnResult(Application.Status result)
      {
        this.outerInstance.lastKnownStatus = result;
        this.cb.OnResult(result);
      }

      public void OnError(ApplicationError e) => this.cb.OnError(e);
    }

    private class LaunchApplicationRequestCallback : ApplicationAsyncResult<bool>
    {
      private readonly Application appInstance;
      private ApplicationAsyncResult<bool> cb;

      public LaunchApplicationRequestCallback(
        Application outerInstance,
        ApplicationAsyncResult<bool> cb)
      {
        this.appInstance = outerInstance;
        this.cb = cb;
      }

      public void OnResult(bool result)
      {
        if (result)
          this.appInstance.lastKnownStatus = Application.Status.RUNNING;
        else if (this.appInstance.lastKnownStatus != Application.Status.INSTALLABLE)
          this.appInstance.lastKnownStatus = Application.Status.STOPPED;
        this.cb.OnResult(result);
      }

      public void OnError(ApplicationError e) => this.cb.OnError(e);
    }
  }
}
