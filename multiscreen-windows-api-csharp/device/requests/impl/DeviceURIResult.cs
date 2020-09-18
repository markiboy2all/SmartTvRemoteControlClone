// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.device.requests.impl.DeviceURIResult
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using System;
using System.Text;

namespace com.samsung.multiscreen.device.requests.impl
{
  public class DeviceURIResult
  {
    private Uri serviceURI;
    private Uri applicationURI;

    public DeviceURIResult(Uri serviceURI, Uri applicationURI)
    {
      this.serviceURI = serviceURI;
      this.applicationURI = applicationURI;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[DeviceURIResult] serviceURI: ").Append((object) this.serviceURI).Append(" applicationURI: ").Append((object) this.applicationURI);
      return stringBuilder.ToString();
    }

    public virtual Uri ServiceURI
    {
      get => this.serviceURI;
      set => this.serviceURI = value;
    }

    public virtual Uri ApplicationURI
    {
      get => this.applicationURI;
      set => this.applicationURI = value;
    }

    public override bool Equals(object o)
    {
      if (this == o)
        return true;
      if (o == null || this.GetType() != o.GetType())
        return false;
      DeviceURIResult deviceUriResult = (DeviceURIResult) o;
      return (this.applicationURI != (Uri) null ? (!this.applicationURI.Equals((object) deviceUriResult.applicationURI) ? 1 : 0) : (deviceUriResult.applicationURI != (Uri) null ? 1 : 0)) == 0 && (this.serviceURI != (Uri) null ? (!this.serviceURI.Equals((object) deviceUriResult.serviceURI) ? 1 : 0) : (deviceUriResult.serviceURI != (Uri) null ? 1 : 0)) == 0;
    }

    public override int GetHashCode() => 31 * (this.serviceURI != (Uri) null ? this.serviceURI.GetHashCode() : 0) + (this.applicationURI != (Uri) null ? this.applicationURI.GetHashCode() : 0);
  }
}
