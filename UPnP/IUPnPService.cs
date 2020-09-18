// Decompiled with JetBrains decompiler
// Type: UPnP.IUPnPService
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using System;
using System.Threading.Tasks;

namespace UPnP
{
  public interface IUPnPService : IDisposable
  {
    void Connect(string callbackAddress, int callbackPort);

    Task ConnectAsync(string callbackAddress, int callbackPort);

    string InvokeAction(string actionName, params ActionArgument[] args);

    Task<string> InvokeActionAsync(string actionName, params ActionArgument[] args);

    event EventHandler<PropertyChangedEventArgs> PropertyChanged;

    string ID { get; set; }
  }
}
