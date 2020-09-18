// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.RemoteControls.TvRemoteControl
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;

namespace SmartView2.Devices.RemoteControls
{
  public class TvRemoteControl : BaseRemoteControl
  {
    public override RemoteControlType RemoteControlType => RemoteControlType.Tv;

    public TvRemoteControl(IKeySender keySender)
      : base(keySender)
    {
    }

    protected override object GetPowerCode() => (object) TvKeyCode.BD_KEY_POWER;

    protected override object GetMbrPowerCode() => (object) null;

    protected override object GetMenuCode() => (object) TvKeyCode.KEY_MENU;

    protected override object GetSmartHubCode() => (object) TvKeyCode.KEY_CONTENTS;

    protected override object GetSearchCode() => (object) TvKeyCode.KEY_DTV_SIGNAL;

    protected override object GetGuideCode() => (object) TvKeyCode.KEY_GUIDE;

    protected override object GetInfoCode() => (object) TvKeyCode.KEY_INFO;

    protected override object GetToolsCode() => (object) TvKeyCode.KEY_TOOLS;

    protected override object GetMinusCode() => (object) TvKeyCode.KEY_PLUS100;

    protected override object GetChannelListCode() => (object) TvKeyCode.KEY_CH_LIST;

    protected override object GetChannelUpCode() => (object) TvKeyCode.KEY_CHUP;

    protected override object GetChannelDownCode() => (object) TvKeyCode.KEY_CHDOWN;

    protected override object GetPreviousChannelCode() => (object) TvKeyCode.KEY_PRECH;

    protected override object GetVolumeUpCode() => (object) TvKeyCode.KEY_VOLUP;

    protected override object GetVolumeDownCode() => (object) TvKeyCode.KEY_VOLDOWN;

    protected override object GetMuteCode() => (object) TvKeyCode.KEY_MUTE;

    protected override object GetNum0Code() => (object) TvKeyCode.KEY_0;

    protected override object GetNum1Code() => (object) TvKeyCode.KEY_1;

    protected override object GetNum2Code() => (object) TvKeyCode.KEY_2;

    protected override object GetNum3Code() => (object) TvKeyCode.KEY_3;

    protected override object GetNum4Code() => (object) TvKeyCode.KEY_4;

    protected override object GetNum5Code() => (object) TvKeyCode.KEY_5;

    protected override object GetNum6Code() => (object) TvKeyCode.KEY_6;

    protected override object GetNum7Code() => (object) TvKeyCode.KEY_7;

    protected override object GetNum8Code() => (object) TvKeyCode.KEY_8;

    protected override object GetNum9Code() => (object) TvKeyCode.KEY_9;

    protected override object GetButtonACode() => (object) TvKeyCode.KEY_RED;

    protected override object GetButtonBCode() => (object) TvKeyCode.KEY_GREEN;

    protected override object GetButtonCCode() => (object) TvKeyCode.KEY_YELLOW;

    protected override object GetButtonDCode() => (object) TvKeyCode.KEY_CYAN;

    protected override object GetJoystickOkCode() => (object) TvKeyCode.KEY_ENTER;

    protected override object GetJoystickUpCode() => (object) TvKeyCode.KEY_UP;

    protected override object GetJoystickDownCode() => (object) TvKeyCode.KEY_DOWN;

    protected override object GetJoystickLeftCode() => (object) TvKeyCode.KEY_LEFT;

    protected override object GetJoystickRightCode() => (object) TvKeyCode.KEY_RIGHT;

    protected override object GetRecordCode() => (object) TvKeyCode.KEY_REC;

    protected override object GetPauseCode() => (object) TvKeyCode.KEY_PAUSE;

    protected override object GetPlayCode() => (object) TvKeyCode.KEY_PLAY;

    protected override object GetStopCode() => (object) TvKeyCode.KEY_STOP;

    protected override object GetRewindCode() => (object) TvKeyCode.KEY_REWIND;

    protected override object GetFastRewindCode() => (object) TvKeyCode.KEY_REWIND_;

    protected override object GetForwardCode() => (object) TvKeyCode.KEY_FF;

    protected override object GetFastForwardCode() => (object) TvKeyCode.KEY_FF_;

    protected override object GetKeypadCode() => (object) null;

    protected override object GetReturnCode() => (object) TvKeyCode.KEY_RETURN;

    protected override object GetExitCode() => (object) TvKeyCode.KEY_EXIT;

    protected override object GetHomeCode() => (object) null;

    protected override object GetDiscMenuCode() => (object) null;

    protected override object GetSubTitleCode() => (object) null;

    protected override object GetStbMenuCode() => (object) null;
  }
}
