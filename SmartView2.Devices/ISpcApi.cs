// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.ISpcApi
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System;

namespace SmartView2.Devices
{
  public interface ISpcApi : IDisposable
  {
    void Initialize(string userId);

    string GenerateServerHello(string pin);

    bool ParseClientHello(string pin, string clientHello);

    string GenerateServerAcknowledge();

    bool ParseClientAcknowledge(string clientAcknowledge);

    byte[] GetKey();
  }
}
