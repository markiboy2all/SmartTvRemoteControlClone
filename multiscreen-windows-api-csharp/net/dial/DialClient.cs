// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.dial.DialClient
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.application;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace com.samsung.multiscreen.net.dial
{
  public class DialClient
  {
    public const string URN = "urn:dial-multiscreen-org:device:dialreceiver:1";
    private string END_POINT;
    internal DialResponseHandler handler;

    public DialClient(string endPoint)
    {
      this.END_POINT = endPoint;
      this.handler = new DialResponseHandler();
    }

    public virtual void LaunchApplication(
      string id,
      string payload,
      IDictionary<string, List<string>> additionalHeaders,
      ApplicationAsyncResult<bool> callback)
    {
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        Uri uri = new Uri(this.END_POINT + id);
        IDictionary<string, IList<string>> headers = NetworkUtil.InitPOSTHeaders(uri);
        if (additionalHeaders != null)
        {
          foreach (string key in (IEnumerable<string>) additionalHeaders.Keys)
            headers[key] = (IList<string>) additionalHeaders[key];
        }
        response = NetworkUtil.DoPost(uri.ToString(), headers, payload, new int?(10000), (string) null, (Encoding) null, (CookieCollection) null, "text/plain");
        if (response == null)
        {
          callback.OnResult(false);
        }
        else
        {
          Console.WriteLine("DialClient.launchApplication result=" + (object) response.StatusCode);
          this.handler.HandleLaunchResponse(response, callback);
          response.Close();
          response.Dispose();
        }
      }
      catch (Exception ex)
      {
        Logger.Trace("Exception during Launch an application: " + ex.ToString());
        Console.WriteLine("Exception during Launch an application: " + ex.ToString());
        callback.OnError(ApplicationError.CreateWithException(ex));
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

    public virtual void StopApplication(
      string id,
      string link,
      ApplicationAsyncResult<bool> callback)
    {
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        Uri uri = new Uri(this.END_POINT + id + (link == null || link.Length <= 0 ? "" : "/" + link));
        IDictionary<string, IList<string>> headers = NetworkUtil.InitDeleteHeaders(uri);
        response = NetworkUtil.Delete(uri.ToString(), headers, new int?(10000));
        if (response == null)
        {
          callback.OnError(new ApplicationError("Not found"));
        }
        else
        {
          Console.WriteLine("DialClient.stopApplication result=" + (object) response.StatusCode);
          this.handler.HandleStopResponse(response, callback);
        }
      }
      catch (Exception ex)
      {
        Logger.Trace("Exception during Stop an application: " + ex.ToString());
        Console.WriteLine("Exception during Stop an application: " + ex.ToString());
        callback.OnError(ApplicationError.CreateWithException(ex));
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

    public virtual void GetApplication(string id, ApplicationAsyncResult<DialApplication> callback)
    {
      HttpWebResponse response = (HttpWebResponse) null;
      try
      {
        Uri uri = new Uri(this.END_POINT + id);
        NetworkUtil.InitPOSTHeaders(uri);
        response = NetworkUtil.Get(uri.ToString(), (IDictionary<string, IList<string>>) null, new int?(10000), (string) null, (CookieCollection) null);
        if (response == null)
        {
          callback.OnError(new ApplicationError("Not found"));
        }
        else
        {
          Console.WriteLine("DialClient.getApplicationInfo result=" + (object) response.StatusCode);
          this.handler.HandleGetApplicationResponse(response, callback);
        }
      }
      catch (Exception ex)
      {
        Logger.Trace("Exception during Get application info: " + ex.ToString());
        Console.WriteLine("Exception during Get application info: " + ex.ToString());
        callback.OnError(ApplicationError.CreateWithException(ex));
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
  }
}
