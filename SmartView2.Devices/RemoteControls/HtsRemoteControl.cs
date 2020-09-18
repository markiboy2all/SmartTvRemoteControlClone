// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.RemoteControls.HtsRemoteControl
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;

namespace SmartView2.Devices.RemoteControls
{
  public class HtsRemoteControl : MbrRemoteControl
  {
    public override RemoteControlType RemoteControlType => RemoteControlType.Hts;

    public HtsRemoteControl(IKeySender tvKeySender, IKeySender mbrKeySender, MbrDevice device)
      : base(tvKeySender, mbrKeySender, device)
    {
    }

    protected override object GetPowerCode() => (object) TvKeyCode.BD_KEY_POWER;

    protected override object GetMbrPowerCode() => (object) MbrKeyCode.KEY_POWER;

    protected override object GetMenuCode() => (object) MbrKeyCode.KEY_MENU;

    protected override object GetSmartHubCode() => (object) TvKeyCode.KEY_CONTENTS;

    protected override object GetSearchCode() => (object) TvKeyCode.KEY_DTV_SIGNAL;

    protected override object GetGuideCode() => (object) null;

    protected override object GetInfoCode() => (object) null;

    protected override object GetToolsCode() => (object) TvKeyCode.KEY_TOOLS;

    protected override object GetMinusCode() => (object) MbrKeyCode.KEY_PLUS100;

    protected override object GetChannelListCode() => (object) null;

    protected override object GetChannelUpCode() => (object) null;

    protected override object GetChannelDownCode() => (object) null;

    protected override object GetPreviousChannelCode() => (object) MbrKeyCode.KEY_PRECH;

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

    protected override object GetButtonACode() => (object) MbrKeyCode.KEY_RED;

    protected override object GetButtonBCode() => (object) MbrKeyCode.KEY_GREEN;

    protected override object GetButtonCCode() => (object) MbrKeyCode.KEY_YELLOW;

    protected override object GetButtonDCode() => (object) MbrKeyCode.KEY_CYAN;

    protected override object GetJoystickOkCode() => (object) TvKeyCode.KEY_ENTER;

    protected override object GetJoystickUpCode() => (object) TvKeyCode.KEY_UP;

    protected override object GetJoystickDownCode() => (object) TvKeyCode.KEY_DOWN;

    protected override object GetJoystickLeftCode() => (object) TvKeyCode.KEY_LEFT;

    protected override object GetJoystickRightCode() => (object) TvKeyCode.KEY_RIGHT;

    protected override object GetRecordCode() => (object) MbrKeyCode.KEY_REC;

    protected override object GetPauseCode() => (object) MbrKeyCode.KEY_PAUSE;

    protected override object GetPlayCode() => (object) MbrKeyCode.KEY_PLAY;

    protected override object GetStopCode() => (object) MbrKeyCode.KEY_STOP;

    protected override object GetRewindCode() => (object) MbrKeyCode.KEY_REWIND;

    protected override object GetFastRewindCode() => (object) MbrKeyCode.KEY_REWIND_;

    protected override object GetForwardCode() => (object) MbrKeyCode.KEY_FF;

    protected override object GetFastForwardCode() => (object) MbrKeyCode.KEY_FF_;

    protected override object GetKeypadCode() => new object();

    protected override object GetReturnCode() => (object) TvKeyCode.KEY_RETURN;

    protected override object GetExitCode() => (object) TvKeyCode.KEY_EXIT;

    protected override object GetHomeCode() => (object) MbrKeyCode.KEY_HOME;

    protected override object GetDiscMenuCode() => (object) MbrKeyCode.KEY_DISC_MENU;

    protected override object GetSubTitleCode() => (object) MbrKeyCode.KEY_SUB_TITLE;

    protected override object GetStbMenuCode() => (object) null;
  }
}
