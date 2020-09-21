// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.NetworkUtil
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace com.samsung.multiscreen.net
{
  internal class NetworkUtil
  {
    private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    public static HttpWebResponse Get(
      string url,
      IDictionary<string, IList<string>> headers,
      int? timeout,
      string userAgent,
      CookieCollection cookies)
    {
      HttpWebRequest request = !string.IsNullOrEmpty(url) ? WebRequest.Create(url) as HttpWebRequest : throw new ArgumentNullException(nameof (url));
      NetworkUtil.SetHeaders(headers, request);
      request.Method = "GET";
      request.UserAgent = NetworkUtil.DefaultUserAgent;
      if (!string.IsNullOrEmpty(userAgent))
        request.UserAgent = userAgent;
      if (timeout.HasValue)
        request.Timeout = timeout.Value;
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      try
      {
        WebResponse response = request.GetResponse();
        if (response != null)
          httpWebResponse = response as HttpWebResponse;
      }
      catch (Exception ex)
      {
      }
      return httpWebResponse;
    }

    public static HttpWebResponse Delete(
      string url,
      IDictionary<string, IList<string>> headers,
      int? timeout)
    {
      HttpWebRequest request = !string.IsNullOrEmpty(url) ? WebRequest.Create(url) as HttpWebRequest : throw new ArgumentNullException(nameof (url));
      NetworkUtil.SetHeaders(headers, request);
      request.Method = "DELETE";
      request.UserAgent = NetworkUtil.DefaultUserAgent;
      if (timeout.HasValue)
        request.Timeout = timeout.Value;
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      try
      {
        WebResponse response = request.GetResponse();
        if (response != null)
          httpWebResponse = response as HttpWebResponse;
      }
      catch (Exception ex)
      {
      }
      return httpWebResponse;
    }

    public static HttpWebResponse Post(
      string url,
      IDictionary<string, IList<string>> headers,
      string payload,
      int? timeout,
      string contentType) => NetworkUtil.DoPost(url, headers, payload, timeout, (string) null, (Encoding) null, (CookieCollection) null, contentType);

    public static HttpWebResponse DoPost(
      string url,
      IDictionary<string, IList<string>> headers,
      string payload,
      int? timeout,
      string userAgent,
      Encoding requestEncoding,
      CookieCollection cookies,
      string contentType)
    {
      Encoding encoding = Encoding.UTF8;
      if (string.IsNullOrEmpty(url))
        throw new ArgumentNullException(nameof (url));
      if (requestEncoding != null)
        encoding = requestEncoding;
      HttpWebRequest request;
      if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
      {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(NetworkUtil.CheckValidationResult);
        request = WebRequest.Create(url) as HttpWebRequest;
        request.ProtocolVersion = HttpVersion.Version10;
      }
      else
        request = WebRequest.Create(url) as HttpWebRequest;
      request.Method = "POST";
      NetworkUtil.SetHeaders(headers, request);
      if (string.IsNullOrEmpty(contentType))
        request.ContentType = "application/x-www-form-urlencoded";
      else
        request.ContentType = contentType;
      request.UserAgent = string.IsNullOrEmpty(userAgent) ? NetworkUtil.DefaultUserAgent : userAgent;
      int? nullable = timeout;
      if ((nullable.GetValueOrDefault() <= 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        request.Timeout = timeout.Value;
      if (cookies != null)
      {
        request.CookieContainer = new CookieContainer();
        request.CookieContainer.Add(cookies);
      }
      if (!string.IsNullOrEmpty(payload))
        request.GetRequestStream().Write(encoding.GetBytes(payload), 0, payload.Length);
      return request.GetResponse() as HttpWebResponse;
    }

    private static bool CheckValidationResult(
      object sender,
      X509Certificate certificate,
      X509Chain chain,
      SslPolicyErrors errors) => true;

    private static void SetHeaders(
      IDictionary<string, IList<string>> headers,
      HttpWebRequest request)
    {
      if (headers == null || request == null)
        return;
      foreach (KeyValuePair<string, IList<string>> header in (IEnumerable<KeyValuePair<string, IList<string>>>) headers)
      {
        foreach (string str in (IEnumerable<string>) header.Value)
        {
          if (header.Key.Equals("Host"))
            request.Host = str;
          else
            request.Headers[header.Key] = str;
        }
      }
    }

    public static IDictionary<string, IList<string>> InitGetHeaders(Uri uri)
    {
      string host = uri.Host;
      int port = uri.Port;
      string str = host + ":" + (object) (port == -1 ? 80 : port);
      IDictionary<string, IList<string>> dictionary = (IDictionary<string, IList<string>>) new Dictionary<string, IList<string>>();
      IList<string> stringList = (IList<string>) new List<string>();
      stringList.Add(str);
      dictionary.Add("Host", stringList);
      return dictionary;
    }

    public static IDictionary<string, IList<string>> InitDeleteHeaders(Uri uri)
    {
      string host = uri.Host;
      int port = uri.Port;
      string str = host + ":" + (object) (port == -1 ? 80 : port);
      IDictionary<string, IList<string>> dictionary = (IDictionary<string, IList<string>>) new Dictionary<string, IList<string>>();
      IList<string> stringList = (IList<string>) new List<string>();
      stringList.Add(str);
      dictionary.Add("Host", stringList);
      return dictionary;
    }

    public static IDictionary<string, IList<string>> InitJSONGetHeaders(Uri uri)
    {
      IDictionary<string, IList<string>> headers = NetworkUtil.InitGetHeaders(uri);
      IList<string> stringList1 = (IList<string>) new List<string>();
      stringList1.Add("application/json");
      headers.Add("Content-Type", stringList1);
      IList<string> stringList2 = (IList<string>) new List<string>();
      stringList2.Add("close");
      headers.Add("Connection", stringList2);
      return headers;
    }

    public static IDictionary<string, IList<string>> InitPOSTHeaders(Uri uri)
    {
      string host = uri.Host;
      int port = uri.Port;
      string str = host + ":" + (object) (port == -1 ? 80 : port);
      IDictionary<string, IList<string>> dictionary = (IDictionary<string, IList<string>>) new Dictionary<string, IList<string>>();
      IList<string> stringList = (IList<string>) new List<string>();
      stringList.Add(str);
      dictionary.Add("Host", stringList);
      return dictionary;
    }

    public static IDictionary<string, IList<string>> InitJSONPostHeaders(Uri uri)
    {
      IDictionary<string, IList<string>> dictionary = NetworkUtil.InitPOSTHeaders(uri);
      IList<string> stringList = (IList<string>) new List<string>();
      stringList.Add("application/json");
      dictionary.Add("Content-Type", stringList);
      return dictionary;
    }

    public static int GetAvailableLocalPort()
    {
      int num = 1024;
      int maxValue = (int) ushort.MaxValue;
      IList allUsedPorts = NetworkUtil.GetAllUsedPorts();
      for (int index = num; index <= maxValue; ++index)
      {
        if (!allUsedPorts.Contains((object) index))
          return index;
      }
      return 0;
    }

    public static IList GetAllUsedPorts()
    {
      IPGlobalProperties globalProperties = IPGlobalProperties.GetIPGlobalProperties();
      IPEndPoint[] activeTcpListeners = globalProperties.GetActiveTcpListeners();
      IPEndPoint[] activeUdpListeners = globalProperties.GetActiveUdpListeners();
      TcpConnectionInformation[] activeTcpConnections = globalProperties.GetActiveTcpConnections();
      IList list = (IList) new ArrayList();
      foreach (IPEndPoint ipEndPoint in activeTcpListeners)
        list.Add((object) ipEndPoint.Port);
      foreach (IPEndPoint ipEndPoint in activeUdpListeners)
        list.Add((object) ipEndPoint.Port);
      foreach (TcpConnectionInformation connectionInformation in activeTcpConnections)
        list.Add((object) connectionInformation.LocalEndPoint.Port);
      return list;
    }

    public static NetworkInterface getInetAddressByName(string name)
    {
      try
      {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
          if (networkInterface.Name.Equals(name))
            return networkInterface;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return (NetworkInterface) null;
    }

    public static bool isUsableNetworkInterface(NetworkInterface networkInterface)
    {
      try
      {
        if (!networkInterface.SupportsMulticast)
          return false;
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool isUsableAddress(NetworkInterface adapter) => adapter.Supports(NetworkInterfaceComponent.IPv4);

    public static IList<NetworkInterface> UsableNetworkInterfaces
    {
      get
      {
        IList<NetworkInterface> networkInterfaceList = (IList<NetworkInterface>) new List<NetworkInterface>();
        try
        {
          foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
          {
            if (NetworkUtil.isUsableNetworkInterface(networkInterface))
              networkInterfaceList.Add(networkInterface);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
          Console.Write(ex.StackTrace);
        }
        return networkInterfaceList;
      }
    }

    public static IList<IPInterfaceProperties> UsableAddresses
    {
      get
      {
        List<IPInterfaceProperties> interfacePropertiesList = new List<IPInterfaceProperties>();
        foreach (NetworkInterface networkInterface in (IEnumerable<NetworkInterface>) NetworkUtil.UsableNetworkInterfaces)
          interfacePropertiesList.Add(networkInterface.GetIPProperties());
        return (IList<IPInterfaceProperties>) interfacePropertiesList;
      }
    }

    public static string getLocalIPAddress()
    {
      string str = "";
      foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
      {
        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
          str = address.ToString();
          break;
        }
      }
      return str;
    }

    public static string LocalHostName => Dns.GetHostName();

    public static string GetMacAddress()
    {
      try
      {
        string str = "";
        foreach (ManagementObject instance in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
        {
          if ((bool) instance["IPEnabled"])
          {
            str = instance["MacAddress"].ToString();
            break;
          }
        }
        return str;
      }
      catch
      {
        return "unknow";
      }
    }

    public static string getHostURL(string host, int port, string uri)
    {
      string str = host;
      if (NetworkUtil.isIPv6Address(host))
        str = "[" + host + "]";
      return "http://" + str + ":" + Convert.ToString(port) + uri;
    }

    public static bool isIPv6Address(string host) => host.IndexOf(":") > -1;
  }
}
