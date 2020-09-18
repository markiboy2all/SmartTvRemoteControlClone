// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.GetDialDeviceDescriptorRequest
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.device.requests.impl;
using com.samsung.multiscreen.net;
using com.samsung.multiscreen.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace com.samsung.multiscreen.device.requests
{
  public class GetDialDeviceDescriptorRequest
  {
    private Uri descriptorURI;
    private AsyncResult<DeviceURIResult> callback;
    private string targetVersion;

    public GetDialDeviceDescriptorRequest(
      Uri descriptorURI,
      string targetVersion,
      AsyncResult<DeviceURIResult> callback)
    {
      this.descriptorURI = descriptorURI;
      this.targetVersion = targetVersion;
      this.callback = callback;
    }

    public void run() => ThreadPool.QueueUserWorkItem(new WaitCallback(this.PerformRequest), (object) null);

    protected internal virtual void PerformRequest(object data)
    {
      string url = this.descriptorURI.ToString();
      HttpWebResponse hwResponse = (HttpWebResponse) null;
      try
      {
        hwResponse = NetworkUtil.Get(url, (IDictionary<string, IList<string>>) null, new int?(5000), (string) null, (CookieCollection) null);
        if (hwResponse == null)
          this.callback.onException(new Exception("Could not connect to target url:" + url));
        else if (hwResponse.StatusCode == HttpStatusCode.OK)
          this.handleResponse(hwResponse);
        else
          this.callback.onException(new Exception("Non-matching device"));
      }
      catch (Exception ex)
      {
        this.callback.onException(ex);
      }
      finally
      {
        if (hwResponse != null)
        {
          try
          {
            hwResponse.Close();
            hwResponse.Dispose();
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    protected internal virtual void handleResponse(HttpWebResponse hwResponse)
    {
      string uriString = "";
      string baseURL = this.descriptorURI.Scheme + "://" + this.descriptorURI.Host;
      try
      {
        WebHeaderCollection headers = hwResponse.Headers;
        if (headers.HasKeys())
        {
          foreach (string allKey in headers.AllKeys)
          {
            if (allKey.Equals("Application-URL"))
              uriString = headers[allKey];
          }
        }
        string serviceUrl = this.getServiceURL(baseURL, hwResponse);
        if (uriString == null || uriString.Length == 0 || (serviceUrl == null || serviceUrl.Length == 0))
        {
          this.callback.onException(new Exception("Non-matching device"));
        }
        else
        {
          Uri serviceURI = new Uri(serviceUrl);
          Uri applicationURI = new Uri(uriString);
          if (serviceURI != (Uri) null && applicationURI != (Uri) null)
            this.callback.onResult(new DeviceURIResult(serviceURI, applicationURI));
          else
            this.callback.onException(new Exception("Non-matching device"));
        }
      }
      catch (Exception ex)
      {
        this.callback.onException(ex);
        Logger.Trace(ex);
        Console.WriteLine("Error while reading response from TV: " + ex.ToString());
      }
    }

    protected internal virtual string getServiceURL(string baseURL, HttpWebResponse hwResponse)
    {
      string str = "";
      try
      {
        StreamReader streamReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load((TextReader) streamReader);
        XmlNodeList elementsByTagName1 = xmlDocument.GetElementsByTagName("Capability", "http://www.sec.co.kr/dlna");
        if (elementsByTagName1 != null)
        {
          if (elementsByTagName1.Count > 0)
          {
            XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("friendlyName");
            if (elementsByTagName2 != null && elementsByTagName2.Count > 0)
            {
              XmlNode xmlNode = elementsByTagName2[0];
            }
            foreach (XmlElement xmlElement in elementsByTagName1)
            {
              string attribute1 = xmlElement.GetAttribute("name");
              string attribute2 = xmlElement.GetAttribute("port");
              string attribute3 = xmlElement.GetAttribute("location");
              if (attribute1 != null && string.Equals(attribute1, this.targetVersion, StringComparison.CurrentCultureIgnoreCase))
              {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(baseURL);
                if (attribute2 != null && attribute2.Length > 0)
                  stringBuilder.Append(":").Append(attribute2);
                if (attribute3 != null && attribute3.Length > 0)
                  stringBuilder.Append(attribute3);
                str = stringBuilder.ToString();
                break;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Trace("GetDialDeviceDescriptor -- Exception parsing descriptor: " + ex.ToString());
      }
      return str;
    }
  }
}
