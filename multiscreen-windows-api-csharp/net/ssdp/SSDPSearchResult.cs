// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.net.ssdp.SSDPSearchResult
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;
using System.IO;

namespace com.samsung.multiscreen.net.ssdp
{
  public class SSDPSearchResult
  {
    private string uniqueServiceName;
    private string uuid;
    private string searchTarget;
    private string location;
    private string deviceUri;
    private string host;
    private int port;
    private string server;

    public override string ToString() => "[SSDPSearchResult]" + "\nuniqueServiceName: " + this.uniqueServiceName + "\nuuid: " + this.uuid + "\nsearchTarget: " + this.searchTarget + "\nlocation: " + this.location + "\ndeviceUri: " + this.deviceUri + "\nhost: " + this.host + ", port: " + (object) this.port + "\nserver: " + this.server;

    public override bool Equals(object o)
    {
      if (this == o)
        return true;
      if (o == null || this.GetType() != o.GetType())
        return false;
      SSDPSearchResult ssdpSearchResult = (SSDPSearchResult) o;
      return this.port == ssdpSearchResult.port && (this.deviceUri != null ? (!this.deviceUri.Equals(ssdpSearchResult.deviceUri) ? 1 : 0) : (ssdpSearchResult.deviceUri != null ? 1 : 0)) == 0 && ((this.host != null ? (!this.host.Equals(ssdpSearchResult.host) ? 1 : 0) : (ssdpSearchResult.host != null ? 1 : 0)) == 0 && (this.location != null ? (!this.location.Equals(ssdpSearchResult.location) ? 1 : 0) : (ssdpSearchResult.location != null ? 1 : 0)) == 0) && ((this.searchTarget != null ? (!this.searchTarget.Equals(ssdpSearchResult.searchTarget) ? 1 : 0) : (ssdpSearchResult.searchTarget != null ? 1 : 0)) == 0 && (this.server != null ? (!this.server.Equals(ssdpSearchResult.server) ? 1 : 0) : (ssdpSearchResult.server != null ? 1 : 0)) == 0 && ((this.uniqueServiceName != null ? (!this.uniqueServiceName.Equals(ssdpSearchResult.uniqueServiceName) ? 1 : 0) : (ssdpSearchResult.uniqueServiceName != null ? 1 : 0)) == 0 && (this.uuid != null ? (!this.uuid.Equals(ssdpSearchResult.uuid) ? 1 : 0) : (ssdpSearchResult.uuid != null ? 1 : 0)) == 0));
    }

    public override int GetHashCode() => 31 * (31 * (31 * (31 * (31 * (31 * (31 * (this.uniqueServiceName != null ? this.uniqueServiceName.GetHashCode() : 0) + (this.uuid != null ? this.uuid.GetHashCode() : 0)) + (this.searchTarget != null ? this.searchTarget.GetHashCode() : 0)) + (this.location != null ? this.location.GetHashCode() : 0)) + (this.deviceUri != null ? this.deviceUri.GetHashCode() : 0)) + (this.host != null ? this.host.GetHashCode() : 0)) + this.port) + (this.server != null ? this.server.GetHashCode() : 0);

    public virtual string UniqueServiceName => this.uniqueServiceName;

    public virtual string Uuid => this.uuid;

    public virtual string SearchTarget => this.searchTarget;

    public virtual string Location => this.location;

    public virtual string Host => this.host;

    public virtual int Port => this.port;

    public virtual string DeviceUri => this.deviceUri;

    public virtual string Server => this.server;

    protected internal static SSDPSearchResult CreateResult(string data)
    {
      SSDPSearchResult ssdpSearchResult = new SSDPSearchResult();
      ssdpSearchResult.uniqueServiceName = SSDPSearchResult.GetHeader(data, "USN");
      ssdpSearchResult.searchTarget = SSDPSearchResult.GetHeader(data, "ST");
      ssdpSearchResult.location = SSDPSearchResult.GetHeader(data, "LOCATION");
      string[] strArray = ssdpSearchResult.uniqueServiceName.Split("::".ToCharArray())[0].Split("uuid:".ToCharArray());
      ssdpSearchResult.uuid = strArray[1];
      Uri uri = new Uri(ssdpSearchResult.location);
      ssdpSearchResult.host = uri.Host;
      ssdpSearchResult.port = uri.Port;
      ssdpSearchResult.deviceUri = uri.Scheme + "://" + ssdpSearchResult.host + ":" + (object) ssdpSearchResult.port;
      ssdpSearchResult.server = SSDPSearchResult.GetHeader(data, "SERVER");
      return ssdpSearchResult;
    }

    private static string GetHeader(string data, string headerName)
    {
      StreamReader streamReader = new StreamReader(SSDPSearchResult.GenerateStreamFromString(data));
      streamReader.ReadLine();
      while (streamReader.Peek() >= 0)
      {
        string str1 = streamReader.ReadLine();
        int length = str1.IndexOf(':');
        string str2 = str1.Substring(0, length);
        if (headerName.Equals(str2.Trim(), StringComparison.CurrentCultureIgnoreCase))
          return str1.Substring(length + 1).Trim();
      }
      return (string) null;
    }

    private static Stream GenerateStreamFromString(string s)
    {
      MemoryStream memoryStream = new MemoryStream();
      StreamWriter streamWriter = new StreamWriter((Stream) memoryStream);
      streamWriter.Write(s);
      streamWriter.Flush();
      memoryStream.Position = 0L;
      return (Stream) memoryStream;
    }
  }
}
