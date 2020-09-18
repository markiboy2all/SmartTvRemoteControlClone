// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.DataContracts.ChannelFormats
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System;

namespace SmartView2.Devices.DataContracts
{
  [Flags]
  public enum ChannelFormats
  {
    ADDED = 1,
    TV = 2,
    RADIO = 4,
    DATA_OTHER = 8,
    SCRAMBLED = 16, // 0x00000010
    SEARCHED = 32, // 0x00000020
    FAVORITE = 64, // 0x00000040
    MYCHANNEL_ONE = 128, // 0x00000080
    MYCHANNEL_TWO = 256, // 0x00000100
    MYCHANNEL_THREE = 512, // 0x00000200
    MYCHANNEL_FOUR = 1024, // 0x00000400
    LOCKED = 2048, // 0x00000800
    ANALOG = 4096, // 0x00001000
    MODIFIED_CHNAME = 8192, // 0x00002000
    HIDDEN = 16384, // 0x00004000
    MYCHANNEL_FIVE = 32768, // 0x00008000
    DIGITAL = 65536, // 0x00010000
    All = DIGITAL | MYCHANNEL_FIVE | HIDDEN | MODIFIED_CHNAME | ANALOG | LOCKED | MYCHANNEL_FOUR | MYCHANNEL_THREE | MYCHANNEL_TWO | MYCHANNEL_ONE | FAVORITE | SEARCHED | SCRAMBLED | DATA_OTHER | RADIO | TV | ADDED, // 0x0001FFFF
  }
}
