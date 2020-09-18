// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.ModelConverter
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.DataContracts;
using System;

namespace SmartView2.Devices
{
  public static class ModelConverter
  {
    public static Channel ToChannel(ChannelInfo channel)
    {
      if (channel == null)
        return (Channel) null;
      return new Channel()
      {
        Name = channel.DisplayChannelName,
        Type = channel.Type,
        PTC = channel.PTC,
        MajorNumber = channel.MajorChannel,
        MinorNumber = channel.MinorChannel,
        DisplayNumber = channel.DisplayChannelNumber,
        ProgramNumber = channel.ProgramNumber,
        ProgramTitle = string.Empty
      };
    }

    public static Source ToSource(SourceInfo source)
    {
      if (source == null)
        return (Source) null;
      return new Source()
      {
        Id = source.Id,
        Type = source.Type
      };
    }

    public static Source ToSource(SourceInfo source, MbrDeviceInfo mbr)
    {
      if (source == null)
        return (Source) null;
      return new Source()
      {
        Id = source.Id,
        Type = source.Type,
        MbrDevice = ModelConverter.ToMbrDevice(mbr)
      };
    }

    public static MbrDevice ToMbrDevice(MbrDeviceInfo mbrDevice)
    {
      if (mbrDevice == null)
        return (MbrDevice) null;
      MbrDevice mbrDevice1 = new MbrDevice();
      mbrDevice1.ActivityIndex = mbrDevice.ActivityIndex;
      DeviceType result = DeviceType.Unknown;
      mbrDevice1.DeviceType = !Enum.TryParse<DeviceType>(mbrDevice.DeviceType, out result) ? DeviceType.Unknown : result;
      mbrDevice1.BrandName = mbrDevice.BrandName;
      return mbrDevice1;
    }
  }
}
