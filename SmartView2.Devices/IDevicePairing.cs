// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.IDevicePairing
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System;
using System.Threading.Tasks;
using Wrapper;

namespace SmartView2.Devices
{
  public interface IDevicePairing : IDisposable
  {
    SpcApiWrapper SpcApi { get; }

    int SessionId { get; }

    bool EncryptionEnabled { get; }

    Task<bool> StartPairingAsync(Uri address);

    Task<string> TryPairAsync(Uri address, string pin);

    Task<string> EnterPinAsync(string pin);

    Task ClosePinPageAsync();
  }
}
