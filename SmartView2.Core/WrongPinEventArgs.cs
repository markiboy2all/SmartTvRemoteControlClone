// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.WrongPinEventArgs
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;

namespace SmartView2.Core
{
  public class WrongPinEventArgs : EventArgs
  {
    private int pinRequestCount;
    private string host = string.Empty;

    public WrongPinEventArgs(Uri uri, int pinRequestCount)
    {
      this.pinRequestCount = pinRequestCount;
      this.Address = uri;
    }

    public int PinRequestCount => this.pinRequestCount;

    public Uri Address { get; private set; }
  }
}
