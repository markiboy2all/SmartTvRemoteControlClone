// Decompiled with JetBrains decompiler
// Type: UPnP.IDeviceListener
// Assembly: UPnP, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: F50CD9DB-347D-4B80-9A78-EE5F2B049062
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\UPnP.dll

using Networking;
using System;

namespace UPnP
{
  public interface IDeviceListener : IDisposable
  {
    void BroadcastMessage();

    void UpdateNetworkInterfaces();

    event EventHandler<HttpMessageEventArgs> AnswerReceived;

    event EventHandler<HttpMessageEventArgs> NotificationReceived;

    event EventHandler<EventArgs> NetworkInterfacesUpdated;
  }
}
