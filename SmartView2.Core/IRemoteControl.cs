// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.IRemoteControl
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Windows.Input;

namespace SmartView2.Core
{
  public interface IRemoteControl : IDisposable
  {
    ICommand Power { get; }

    ICommand MbrPower { get; }

    ICommand Menu { get; }

    ICommand SmartHub { get; }

    ICommand Search { get; }

    ICommand Guide { get; }

    ICommand Info { get; }

    ICommand Tools { get; }

    ICommand Minus { get; }

    ICommand ChannelList { get; }

    ICommand ChannelUp { get; }

    ICommand ChannelDown { get; }

    ICommand PreviousChannel { get; }

    ICommand VolumeUp { get; }

    ICommand VolumeDown { get; }

    ICommand Mute { get; }

    ICommand Num0 { get; }

    ICommand Num1 { get; }

    ICommand Num2 { get; }

    ICommand Num3 { get; }

    ICommand Num4 { get; }

    ICommand Num5 { get; }

    ICommand Num6 { get; }

    ICommand Num7 { get; }

    ICommand Num8 { get; }

    ICommand Num9 { get; }

    ICommand ButtonA { get; }

    ICommand ButtonB { get; }

    ICommand ButtonC { get; }

    ICommand ButtonD { get; }

    ICommand JoystickOk { get; }

    ICommand JoystickUp { get; }

    ICommand JoystickDown { get; }

    ICommand JoystickLeft { get; }

    ICommand JoystickRight { get; }

    ICommand Record { get; }

    ICommand Pause { get; }

    ICommand Play { get; }

    ICommand Stop { get; }

    ICommand Rewind { get; }

    ICommand FastRewind { get; }

    ICommand Forward { get; }

    ICommand FastForward { get; }

    ICommand Keypad { get; }

    ICommand Return { get; }

    ICommand Exit { get; }

    ICommand Home { get; }

    ICommand DiscMenu { get; }

    ICommand SubTitle { get; }

    ICommand StbMenu { get; }

    RemoteControlType RemoteControlType { get; }
  }
}
