// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.RemoteControls.BaseRemoteControl
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Core.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartView2.Devices.RemoteControls
{
  public abstract class BaseRemoteControl : IRemoteControl, IDisposable
  {
    private const int DefaultTimeout = 250;
    private const int JoystickTimeout = 150;
    private const int RepeatTimeout = 150;
    private readonly IKeySender keySender;
    private readonly AutoResetEvent keyLock = new AutoResetEvent(true);

    public ICommand Power { get; private set; }

    public ICommand MbrPower { get; private set; }

    public ICommand Menu { get; private set; }

    public ICommand SmartHub { get; private set; }

    public ICommand Search { get; private set; }

    public ICommand Guide { get; private set; }

    public ICommand Info { get; private set; }

    public ICommand Tools { get; private set; }

    public ICommand Minus { get; private set; }

    public ICommand ChannelList { get; private set; }

    public ICommand ChannelUp { get; private set; }

    public ICommand ChannelDown { get; private set; }

    public ICommand PreviousChannel { get; private set; }

    public ICommand VolumeUp { get; private set; }

    public ICommand VolumeDown { get; private set; }

    public ICommand Mute { get; private set; }

    public ICommand Num0 { get; private set; }

    public ICommand Num1 { get; private set; }

    public ICommand Num2 { get; private set; }

    public ICommand Num3 { get; private set; }

    public ICommand Num4 { get; private set; }

    public ICommand Num5 { get; private set; }

    public ICommand Num6 { get; private set; }

    public ICommand Num7 { get; private set; }

    public ICommand Num8 { get; private set; }

    public ICommand Num9 { get; private set; }

    public ICommand ButtonA { get; private set; }

    public ICommand ButtonB { get; private set; }

    public ICommand ButtonC { get; private set; }

    public ICommand ButtonD { get; private set; }

    public ICommand JoystickOk { get; private set; }

    public ICommand JoystickUp { get; private set; }

    public ICommand JoystickDown { get; private set; }

    public ICommand JoystickLeft { get; private set; }

    public ICommand JoystickRight { get; private set; }

    public ICommand Record { get; private set; }

    public ICommand Pause { get; private set; }

    public ICommand Play { get; private set; }

    public ICommand Stop { get; private set; }

    public ICommand Rewind { get; private set; }

    public ICommand FastRewind { get; private set; }

    public ICommand Forward { get; private set; }

    public ICommand FastForward { get; private set; }

    public ICommand Keypad { get; private set; }

    public ICommand Return { get; private set; }

    public ICommand Exit { get; private set; }

    public ICommand Home { get; private set; }

    public ICommand DiscMenu { get; private set; }

    public ICommand SubTitle { get; private set; }

    public ICommand StbMenu { get; private set; }

    public abstract RemoteControlType RemoteControlType { get; }

    public BaseRemoteControl(IKeySender keySender) => this.keySender = keySender != null ? keySender : throw new ArgumentNullException(nameof (keySender));

    public void Initialize()
    {
      this.Power = this.CreateCommand(new Func<object>(this.GetPowerCode));
      this.MbrPower = this.CreateCommand(new Func<object>(this.GetMbrPowerCode));
      this.Menu = this.CreateCommand(new Func<object>(this.GetMenuCode));
      this.SmartHub = this.CreateCommand(new Func<object>(this.GetSmartHubCode));
      this.Search = this.CreateCommand(new Func<object>(this.GetSearchCode));
      this.Guide = this.CreateCommand(new Func<object>(this.GetGuideCode));
      this.Info = this.CreateCommand(new Func<object>(this.GetInfoCode));
      this.Tools = this.CreateCommand(new Func<object>(this.GetToolsCode));
      this.Minus = this.CreateCommand(new Func<object>(this.GetMinusCode));
      this.ChannelList = this.CreateCommand(new Func<object>(this.GetChannelListCode));
      this.ChannelUp = this.CreateCommand(new Func<object>(this.GetChannelUpCode));
      this.ChannelDown = this.CreateCommand(new Func<object>(this.GetChannelDownCode));
      this.PreviousChannel = this.CreateCommand(new Func<object>(this.GetPreviousChannelCode));
      this.VolumeUp = this.CreateCommand(new Func<object>(this.GetVolumeUpCode));
      this.VolumeDown = this.CreateCommand(new Func<object>(this.GetVolumeDownCode));
      this.Mute = this.CreateCommand(new Func<object>(this.GetMuteCode));
      this.Num0 = this.CreateCommand(new Func<object>(this.GetNum0Code));
      this.Num1 = this.CreateCommand(new Func<object>(this.GetNum1Code));
      this.Num2 = this.CreateCommand(new Func<object>(this.GetNum2Code));
      this.Num3 = this.CreateCommand(new Func<object>(this.GetNum3Code));
      this.Num4 = this.CreateCommand(new Func<object>(this.GetNum4Code));
      this.Num5 = this.CreateCommand(new Func<object>(this.GetNum5Code));
      this.Num6 = this.CreateCommand(new Func<object>(this.GetNum6Code));
      this.Num7 = this.CreateCommand(new Func<object>(this.GetNum7Code));
      this.Num8 = this.CreateCommand(new Func<object>(this.GetNum8Code));
      this.Num9 = this.CreateCommand(new Func<object>(this.GetNum9Code));
      this.ButtonA = this.CreateCommand(new Func<object>(this.GetButtonACode));
      this.ButtonB = this.CreateCommand(new Func<object>(this.GetButtonBCode));
      this.ButtonC = this.CreateCommand(new Func<object>(this.GetButtonCCode));
      this.ButtonD = this.CreateCommand(new Func<object>(this.GetButtonDCode));
      this.JoystickOk = this.CreateCommand(new Func<object>(this.GetJoystickOkCode));
      this.JoystickUp = this.CreateCommand(new Func<object>(this.GetJoystickUpCode));
      this.JoystickDown = this.CreateCommand(new Func<object>(this.GetJoystickDownCode));
      this.JoystickLeft = this.CreateCommand(new Func<object>(this.GetJoystickLeftCode));
      this.JoystickRight = this.CreateCommand(new Func<object>(this.GetJoystickRightCode));
      this.Record = this.CreateCommand(new Func<object>(this.GetRecordCode));
      this.Pause = this.CreateCommand(new Func<object>(this.GetPauseCode));
      this.Play = this.CreateCommand(new Func<object>(this.GetPlayCode));
      this.Stop = this.CreateCommand(new Func<object>(this.GetStopCode));
      this.Rewind = this.CreateCommand(new Func<object>(this.GetRewindCode));
      this.FastRewind = this.CreateCommand(new Func<object>(this.GetFastRewindCode));
      this.Forward = this.CreateCommand(new Func<object>(this.GetForwardCode));
      this.FastForward = this.CreateCommand(new Func<object>(this.GetFastForwardCode));
      this.Keypad = this.CreateCommand(new Func<object>(this.GetKeypadCode));
      this.Return = this.CreateCommand(new Func<object>(this.GetReturnCode));
      this.Exit = this.CreateCommand(new Func<object>(this.GetExitCode));
      this.Home = this.CreateCommand(new Func<object>(this.GetHomeCode));
      this.DiscMenu = this.CreateCommand(new Func<object>(this.GetDiscMenuCode));
      this.SubTitle = this.CreateCommand(new Func<object>(this.GetSubTitleCode));
      this.StbMenu = this.CreateCommand(new Func<object>(this.GetStbMenuCode));
    }

    protected virtual ICommand CreateCommand(Func<object> keyCodeProvider)
    {
      if (keyCodeProvider() == null)
        return (ICommand) new Command((Action<object>) (arg => {}), (Predicate<object>) (arg => false));
      object keyCode = keyCodeProvider();
      return (ICommand) new AsyncCommand((Func<object, Task>) (arg => this.SendKeyAsync(this.keySender, (object) keyCode.ToString())));
    }

    protected async Task SendKeyAsync(
      IKeySender keySender,
      object keyCode,
      int milisecondsTimeout = 250)
    {
      try
      {
        await keySender.SendKeyAsync(keyCode);
        await Task.Delay(100);
      }
      catch (NullReferenceException ex)
      {
      }
      catch (ObjectDisposedException ex)
      {
      }
    }

    protected abstract object GetPowerCode();

    protected abstract object GetMbrPowerCode();

    protected abstract object GetMenuCode();

    protected abstract object GetSmartHubCode();

    protected abstract object GetSearchCode();

    protected abstract object GetGuideCode();

    protected abstract object GetInfoCode();

    protected abstract object GetToolsCode();

    protected abstract object GetMinusCode();

    protected abstract object GetChannelListCode();

    protected abstract object GetChannelUpCode();

    protected abstract object GetChannelDownCode();

    protected abstract object GetPreviousChannelCode();

    protected abstract object GetVolumeUpCode();

    protected abstract object GetVolumeDownCode();

    protected abstract object GetMuteCode();

    protected abstract object GetNum0Code();

    protected abstract object GetNum1Code();

    protected abstract object GetNum2Code();

    protected abstract object GetNum3Code();

    protected abstract object GetNum4Code();

    protected abstract object GetNum5Code();

    protected abstract object GetNum6Code();

    protected abstract object GetNum7Code();

    protected abstract object GetNum8Code();

    protected abstract object GetNum9Code();

    protected abstract object GetButtonACode();

    protected abstract object GetButtonBCode();

    protected abstract object GetButtonCCode();

    protected abstract object GetButtonDCode();

    protected abstract object GetJoystickOkCode();

    protected abstract object GetJoystickUpCode();

    protected abstract object GetJoystickDownCode();

    protected abstract object GetJoystickLeftCode();

    protected abstract object GetJoystickRightCode();

    protected abstract object GetRecordCode();

    protected abstract object GetPauseCode();

    protected abstract object GetPlayCode();

    protected abstract object GetStopCode();

    protected abstract object GetRewindCode();

    protected abstract object GetFastRewindCode();

    protected abstract object GetForwardCode();

    protected abstract object GetFastForwardCode();

    protected abstract object GetKeypadCode();

    protected abstract object GetReturnCode();

    protected abstract object GetExitCode();

    protected abstract object GetHomeCode();

    protected abstract object GetDiscMenuCode();

    protected abstract object GetSubTitleCode();

    protected abstract object GetStbMenuCode();

    public void Dispose()
    {
      this.keyLock.WaitOne();
      this.keyLock.Dispose();
    }
  }
}
