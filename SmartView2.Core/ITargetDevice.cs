// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.ITargetDevice
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SmartView2.Core
{
  public interface ITargetDevice : INotifyPropertyChanged, IDisposable
  {
    Uri VideoUrl { get; set; }

    Source CurrentSource { get; set; }

    SourceList SourceList { get; set; }

    ViewMode ViewMode { get; set; }

    IMultiScreen MultiScreen { get; }

    IDlnaServer DlnaServer { get; }

    IDataLibrary DataLibrary { get; }

    bool IsUSABoard { get; }

    bool IsProgramTimeAvaliable { get; }

    long StartProgramTime { get; set; }

    long EndProgramTime { get; set; }

    long NowTime { get; set; }

    bool IsVideoStarted { get; set; }

    bool IsCloneView { get; }

    MessageType? VideoMessageInfoType { get; set; }

    string StreamStatus { get; }

    Task SetRemoteControlStateAsync(bool isRemote);

    void SetChannelList(Source source, ChannelList channelList);

    void SetCurrentChannel(Source source, Channel channel);

    Task InitializeAsync();

    void CancelInitializing();

    Task InitializeMultiScreenAsync();

    void CloseMultiScreen();

    void InitializeRemoteControl();

    Task DisconnectAsync();

    IRemoteControl CreateRemoteControl(Source source);

    Task SetChannelAsync(Channel newChannel);

    Task SetSourceAsync(Source newSource);

    Task ChannelUpAsync();

    Task ChannelDownAsync();

    Task SendDeviceViewToTv();

    Task SendTvViewToDevice();

    Task SetInputTextAsync(string text);

    Task EndInputAsync();

    Task RestartView();

    Task<bool> StopRecordAsync();

    void ShowErrorMessage(MessageType messageType, bool isPopup = false);

    event EventHandler<EventArgs> ShowInputKeyboard;

    event EventHandler<EventArgs> ShowPasswordKeyboard;

    event EventHandler<UpdateTextEventArgs> TextUpdated;

    event EventHandler<EventArgs> HideKeyboard;

    event EventHandler<ErrorMessageEventArgs> ErrorMessageArised;

    event EventHandler<EventArgs> Disconnecting;

    event EventHandler VideoShutDown;

    event EventHandler<CCDataEventArgs> CCDataReceived;

    event EventHandler ViewStarted;
  }
}
