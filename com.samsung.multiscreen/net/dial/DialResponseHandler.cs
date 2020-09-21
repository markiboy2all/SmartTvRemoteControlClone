// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.dial.DialResponseHandler
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.application;
using com.samsung.multiscreen.util;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace com.samsung.multiscreen.net.dial
{
  public class DialResponseHandler
  {
    public virtual void HandleLaunchResponse(
      HttpWebResponse response,
      ApplicationAsyncResult<bool> callback)
    {
      if (response.StatusCode >= HttpStatusCode.OK)
      {
        if (response.StatusCode < HttpStatusCode.MultipleChoices)
        {
          try
          {
            Logger.Debug("launchApplication() response:\nHEADERS: " + (object) response.Headers);
            callback.OnResult(true);
            Logger.Debug("launchApplication() result: true");
            return;
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
            callback.OnError(ApplicationError.CreateWithException(ex));
            return;
          }
        }
      }
      if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
        callback.OnError(new ApplicationError("Service unavailable"));
      else if (response.StatusCode == HttpStatusCode.NotFound)
        callback.OnError(new ApplicationError("Not found"));
      else if (response.StatusCode == HttpStatusCode.RequestEntityTooLarge)
        callback.OnError(new ApplicationError("Request entity too large"));
      else if (response.StatusCode == HttpStatusCode.LengthRequired)
        callback.OnError(new ApplicationError("Length required"));
      else
        callback.OnResult(false);
    }

    public virtual void HandleStopResponse(
      HttpWebResponse response,
      ApplicationAsyncResult<bool> callback)
    {
      Logger.Trace("stopApplication() response: " + (object) response.StatusCode);
      Logger.Debug("stopApplication() response: " + (object) response.StatusCode);
      if (response.StatusCode == HttpStatusCode.OK)
        callback.OnResult(true);
      else if (response.StatusCode == HttpStatusCode.NotImplemented)
        callback.OnError(new ApplicationError("Not implemented"));
      else if (response.StatusCode == HttpStatusCode.NotFound)
        callback.OnError(new ApplicationError("Not found"));
      else
        callback.OnResult(false);
    }

    public virtual void HandleGetApplicationResponse(
      HttpWebResponse response,
      ApplicationAsyncResult<DialApplication> callback)
    {
      if (response.StatusCode == HttpStatusCode.OK)
      {
        try
        {
          StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
          DialApplication result = new DialApplication();
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load((TextReader) streamReader);
          XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
          nsmgr.AddNamespace("ns", "urn:dial-multiscreen-org:schemas:dial");
          XmlNode xmlNode1 = xmlDocument.SelectSingleNode("/ns:service/ns:name", nsmgr);
          if (xmlNode1 != null)
            result.Name = xmlNode1.InnerText;
          XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/ns:service/ns:state", nsmgr);
          if (xmlNode2 != null)
            result.State = xmlNode2.InnerText;
          XmlNode xmlNode3 = xmlDocument.SelectSingleNode("/ns:service/ns:options", nsmgr);
          if (xmlNode3 != null && xmlNode3.Attributes != null)
          {
            string str = xmlNode3.Attributes["allowStop"].Value;
            result.StopAllowed = bool.Parse(str);
          }
          XmlNode xmlNode4 = xmlDocument.SelectSingleNode("/ns:service/ns:link", nsmgr);
          if (xmlNode4 == null)
          {
            string namespaceURI = xmlDocument.NameTable.Get("atom");
            if (!string.IsNullOrEmpty(namespaceURI))
            {
              XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("link", namespaceURI);
              if (elementsByTagName != null && elementsByTagName.Count > 0)
                xmlNode4 = elementsByTagName[0];
            }
          }
          if (xmlNode4 != null && xmlNode4.Attributes != null)
          {
            string str1 = xmlNode3.Attributes["rel"].Value;
            result.RelLink = str1;
            string str2 = xmlNode3.Attributes["href"].Value;
            result.HrefLink = str2;
          }
          Logger.Trace("getApplication() result:\n" + (object) result);
          callback.OnResult(result);
        }
        catch (Exception ex)
        {
          callback.OnError(new ApplicationError(ex.ToString()));
        }
      }
      else
        callback.OnError(new ApplicationError("Not found"));
    }
  }
}
