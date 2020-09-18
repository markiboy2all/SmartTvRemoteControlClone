// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTv.SecondTvException
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Devices.DataContracts;
using System;

namespace SmartView2.Devices.SecondTv
{
  public class SecondTvException : Exception
  {
    private ErrorCode errorType;

    public ErrorCode ErrorType => this.errorType;

    public SecondTvException(ErrorCode errorType)
      : this(errorType, "SecondTv error type: " + errorType.ToString())
    {
    }

    public SecondTvException(ErrorCode errorType, string message)
      : base(message)
      => this.errorType = errorType;
  }
}
