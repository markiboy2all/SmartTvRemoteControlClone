// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.RemoteControls.RemoteControlFactory
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using System;

namespace SmartView2.Devices.RemoteControls
{
  public class RemoteControlFactory : IRemoteControlFactory
  {
    private readonly IKeySender tvKeySender;
    private readonly IKeySender mbrKeySender;

    public RemoteControlFactory(IKeySender tvKeySender, IKeySender mbrKeySender)
    {
      if (tvKeySender == null)
        throw new ArgumentNullException(nameof (tvKeySender));
      if (mbrKeySender == null)
        throw new ArgumentNullException(nameof (mbrKeySender));
      this.tvKeySender = tvKeySender;
      this.mbrKeySender = mbrKeySender;
    }

    public IRemoteControl CreateRemoteControl(Source source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      BaseRemoteControl baseRemoteControl = (BaseRemoteControl) null;
      if (source.IsMbr)
      {
        switch (source.MbrDevice.DeviceType)
        {
          case DeviceType.BD:
            baseRemoteControl = (BaseRemoteControl) new BdRemoteControl(this.tvKeySender, this.mbrKeySender, source.MbrDevice);
            break;
          case DeviceType.HTS:
            baseRemoteControl = (BaseRemoteControl) new HtsRemoteControl(this.tvKeySender, this.mbrKeySender, source.MbrDevice);
            break;
          case DeviceType.STB:
            baseRemoteControl = (BaseRemoteControl) new StbRemoteControl(this.tvKeySender, this.mbrKeySender, source.MbrDevice);
            break;
          case DeviceType.Unknown:
            baseRemoteControl = (BaseRemoteControl) new UnknownRemoteControl(this.tvKeySender);
            break;
        }
      }
      else if (source.Type == SourceType.TV)
        baseRemoteControl = (BaseRemoteControl) new TvRemoteControl(this.tvKeySender);
      if (baseRemoteControl == null)
        baseRemoteControl = (BaseRemoteControl) new HdmiRemoteControl(this.tvKeySender);
      baseRemoteControl.Initialize();
      return (IRemoteControl) baseRemoteControl;
    }
  }
}
