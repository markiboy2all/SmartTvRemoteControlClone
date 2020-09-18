// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvDevice
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UPnP.DataContracts;

namespace SmartView2.Devices
{
  public class TvDevice : ITargetDevice, INotifyPropertyChanged, IDisposable
  {
    private Uri videoUrl;
    private ISecondTv secondTv;
    private IPlayerNotificationProvider playerNotification;
    private IRemoteInput remoteInput;
    private readonly IRemoteControlFactory remoteControlFactory;
    private readonly IBaseDispatcher dispatcher;
    private readonly IHttpServer httpServer;
    private readonly IMultiScreen multiScreen;
    private readonly IDlnaServer dlnaServer;
    private IDataLibrary dataLibrary;
    private readonly DeviceInfo deviceInfo;
    private readonly TvController tvController;
    private Source currentSource;
    private SourceList sourceList;
    private StreamMedia streamType;
    private bool isRemoteControl;
    private string streamStatus;
    private ViewMode viewMode;
    private bool isUSABoard;
    private long startProgramTime;
    private long endProgramTime;
    private long nowTime;
    private bool isVideoStarted;
    private MessageType? videoMessageInfoType;
    private CancellationTokenSource tokenSource;

    public Uri VideoUrl
    {
      get => this.videoUrl;
      set
      {
        this.videoUrl = value;
        this.OnPropertyChanged((object) this, nameof (VideoUrl));
      }
    }

    public Source CurrentSource
    {
      get => this.currentSource;
      set
      {
        this.currentSource = value;
        this.OnPropertyChanged((object) this, nameof (CurrentSource));
      }
    }

    public SourceList SourceList
    {
      get => this.sourceList;
      set
      {
        this.sourceList = value;
        this.OnPropertyChanged((object) this, nameof (SourceList));
      }
    }

    public StreamMedia StreamType
    {
      get => this.streamType;
      set
      {
        this.streamType = value;
        this.OnPropertyChanged((object) this, nameof (StreamType));
      }
    }

    public IMultiScreen MultiScreen => this.multiScreen;

    public IDlnaServer DlnaServer => this.dlnaServer;

    public IDataLibrary DataLibrary => this.dataLibrary;

    internal ISecondTv SecondTv { get; private set; }

    public ViewMode ViewMode
    {
      get => this.viewMode;
      set
      {
        this.viewMode = value;
        this.OnPropertyChanged((object) this, nameof (ViewMode));
      }
    }

    public bool IsUSABoard
    {
      get => this.isUSABoard;
      private set
      {
        this.isUSABoard = value;
        this.OnPropertyChanged((object) this, nameof (IsUSABoard));
      }
    }

    public bool IsProgramTimeAvaliable => this.StartProgramTime != -1L && this.EndProgramTime != -1L && DateTime.Now.Ticks < this.EndProgramTime;

    public long StartProgramTime
    {
      get => this.startProgramTime;
      set
      {
        this.startProgramTime = value;
        this.OnPropertyChanged((object) this, nameof (StartProgramTime));
        this.OnPropertyChanged((object) this, "IsProgramTimeAvaliable");
      }
    }

    public long EndProgramTime
    {
      get => this.endProgramTime;
      set
      {
        this.endProgramTime = value;
        this.OnPropertyChanged((object) this, nameof (EndProgramTime));
        this.OnPropertyChanged((object) this, "IsProgramTimeAvaliable");
      }
    }

    public long NowTime
    {
      get => this.nowTime;
      set
      {
        this.nowTime = value;
        this.OnPropertyChanged((object) this, nameof (NowTime));
        this.OnPropertyChanged((object) this, "IsProgramTimeAvaliable");
      }
    }

    public bool IsVideoStarted
    {
      get => this.isVideoStarted;
      set
      {
        this.isVideoStarted = value;
        this.OnPropertyChanged((object) this, nameof (IsVideoStarted));
      }
    }

    public bool IsCloneView => this.tvController.IsCloneView;

    public MessageType? VideoMessageInfoType
    {
      get => this.videoMessageInfoType;
      set
      {
        this.videoMessageInfoType = value;
        this.OnPropertyChanged((object) this, nameof (VideoMessageInfoType));
      }
    }

    public string StreamStatus
    {
      get => this.streamStatus;
      private set
      {
        this.streamStatus = value;
        this.OnPropertyChanged((object) this, nameof (StreamStatus));
      }
    }

    public TvDevice(
      DeviceInfo deviceInfo,
      ISecondTv secondTv,
      IPlayerNotificationProvider playerNotification,
      IRemoteInput remoteInput,
      IRemoteControlFactory remoteControlFactory,
      IBaseDispatcher dispatcher,
      IHttpServer httpServer,
      IMultiScreen multiScreen,
      IDlnaServer dlnaServer,
      IDataLibrary dataLibrary)
    {
      if (deviceInfo == null)
        throw new ArgumentNullException(nameof (deviceInfo));
      if (secondTv == null)
        throw new ArgumentNullException(nameof (secondTv));
      if (playerNotification == null)
        throw new ArgumentNullException(nameof (playerNotification));
      if (remoteInput == null)
        throw new ArgumentNullException(nameof (remoteInput));
      if (remoteControlFactory == null)
        throw new ArgumentNullException(nameof (remoteControlFactory));
      if (dispatcher == null)
        throw new ArgumentNullException(nameof (dispatcher));
      if (multiScreen == null)
        throw new ArgumentNullException(nameof (multiScreen));
      if (dlnaServer == null)
        throw new ArgumentNullException(nameof (dlnaServer));
      if (dataLibrary == null)
        throw new ArgumentNullException(nameof (dataLibrary));
      this.deviceInfo = deviceInfo;
      this.secondTv = secondTv;
      this.playerNotification = playerNotification;
      this.remoteInput = remoteInput;
      this.remoteControlFactory = remoteControlFactory;
      this.dispatcher = dispatcher;
      this.httpServer = httpServer;
      this.multiScreen = multiScreen;
      this.dlnaServer = dlnaServer;
      this.dataLibrary = dataLibrary;
      this.secondTv.ChannelListChanged += new EventHandler<EventArgs>(this.secondTv_ChannelListChanged);
      this.secondTv.ChannelChanged += new EventHandler<EventArgs>(this.secondTv_ChannelChanged);
      this.secondTv.SourceListChanged += new EventHandler<EventArgs>(this.secondTv_SourceListChanged);
      this.secondTv.ChannelListTypeChanged += new EventHandler<EventArgs>(this.secondTv_ChannelListTypeChanged);
      this.secondTv.MbrDeviceListChanged += new EventHandler<EventArgs>(this.secondTv_MbrDeviceListChanged);
      this.secondTv.SourceDisconnected += new EventHandler<EventArgs>(this.secondTv_SourceDisconnected);
      this.secondTv.PriorityDisconnect += new EventHandler<EventArgs>(this.secondTv_PriorityDisconnect);
      this.secondTv.PowerOff += new EventHandler<EventArgs>(this.secondTv_PowerOff);
      this.remoteInput.ShowInputKeyboard += new EventHandler<EventArgs>(this.secondTv_ShowInputKeyboard);
      this.remoteInput.ShowPasswordKeyboard += new EventHandler<EventArgs>(this.secondTv_ShowPasswordKeyboard);
      this.remoteInput.TextUpdated += new EventHandler<UpdateTextEventArgs>(this.secondTv_TextUpdated);
      this.remoteInput.HideKeyboard += new EventHandler<EventArgs>(this.secondTv_HideKeyboard);
      this.playerNotification = playerNotification;
      this.playerNotification.StreamAudioSampleRateChanged += new EventHandler<EventArgs>(this.playerNotification_StreamAudioSampleRateChanged);
      this.playerNotification.StreamVersionChanged += new EventHandler<EventArgs>(this.playerNotification_StreamVersionChanged);
      this.playerNotification.StreamResolutionChanged += new EventHandler<EventArgs>(this.playerNotification_StreamResolutionChanged);
      this.playerNotification.StreamPmtInfoChanged += new EventHandler<EventArgs>(this.playerNotification_StreamPmtInfoChanged);
      this.playerNotification.StreamMediaChanged += new EventHandler<StreamMediaEventArgs>(this.playerNotification_StreamMediaChanged);
      this.playerNotification.StreamVideoStatusChanged += new EventHandler<string>(this.playerNotification_StreamVideoStatusChanged);
      this.playerNotification.VideoShutDown += new EventHandler(this.playerNotification_VideoShutDown);
      this.playerNotification.CCDataReceived += new EventHandler<CCDataEventArgs>(this.playerNotification_CCDataReceived);
      this.tvController = new TvController((ITargetDevice) this, this.secondTv);
      this.tvController.ViewStarted += new EventHandler(this.TvDevice_ViewStarted);
      this.tokenSource = new CancellationTokenSource();
    }

    public async Task InitializeAsync()
    {
      await this.tvController.InitializeAsync(this.tokenSource.Token);
      this.InitializeRemoteControl();
      await this.httpServer.InitializeAsync(this.deviceInfo.LocalAddress.Host, this.deviceInfo.DeviceAddress.Host);
      await this.dlnaServer.InitializeAsync(this.deviceInfo.LocalAddress.Host, this.deviceInfo.DeviceAddress.Host, this.dataLibrary, this.httpServer);
      string result = await this.secondTv.GetTargetLocationAsync();
      this.IsUSABoard = result == "2";
      this.dataLibrary.LoadLibrary();
    }

    public void CancelInitializing() => this.tokenSource.Cancel();

    public async Task InitializeMultiScreenAsync() => await this.multiScreen.InitializeAsync(this.deviceInfo.LocalAddress.Host, this.deviceInfo.DeviceAddress.Host, this.httpServer);

    public void CloseMultiScreen() => this.multiScreen.Close();

    public void InitializeRemoteControl()
    {
      foreach (Source source1 in (Collection<Source>) this.SourceList)
      {
        Source source = source1;
        IRemoteControl rc = this.CreateRemoteControl(source);
        this.dispatcher.Invoke((Action) (() => source.RemoteControl = rc));
      }
    }

    public async Task DisconnectAsync()
    {
      this.CancelInitializing();
      if (this.dataLibrary == null)
        return;
      this.dataLibrary = (IDataLibrary) null;
    }

    public IRemoteControl CreateRemoteControl(Source source) => this.remoteControlFactory.CreateRemoteControl(source);

    public void SetChannelList(Source source, ChannelList channelList) => this.dispatcher.Invoke((Action) (() => source.ChannelList = channelList));

    public void SetCurrentChannel(Source source, Channel channel) => this.dispatcher.Invoke((Action) (() => source.CurrentChannel = channel));

    public async Task SetChannelAsync(Channel newChannel) => await this.tvController.SetChannelAsync(newChannel);

    public async Task SetSourceAsync(Source newSource)
    {
      Logger.Instance.LogMessage("[SmartView2][TvDevice]SetSourceAsync started...");
      await this.tvController.SetSourceAsync(newSource);
      Logger.Instance.LogMessage("[SmartView2][TvDevice]SetSourceAsync ended...");
    }

    public async Task ChannelUpAsync() => await this.tvController.ChannelUpAsync();

    public async Task ChannelDownAsync() => await this.tvController.ChannelDownAsync();

    public async Task SendDeviceViewToTv() => await this.tvController.SendDeviceViewToTv();

    public async Task SendTvViewToDevice() => await this.tvController.SendTvViewToDevice();

    public async Task SetInputTextAsync(string text) => await this.remoteInput.UpdateTextAsync(text);

    public async Task EndInputAsync() => await this.remoteInput.EndInputAsync();

    public async Task SetRemoteControlStateAsync(bool isRemote)
    {
      if (this.isRemoteControl == isRemote)
        return;
      this.isRemoteControl = isRemote;
      try
      {
        await this.tvController.SetIsRemoteControl(isRemote);
      }
      catch
      {
      }
    }

    public async Task<bool> StopRecordAsync() => await this.tvController.StopRecordAsync();

    private async void secondTv_ChannelChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv - ChannelChanged()");
      await this.tvController.MainChannelUpdateAsync();
      this.OnPropertyChanged((object) this, "IsCloneView");
    }

    private async void secondTv_ChannelListChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv - ChannelListChanged()");
      await this.tvController.UpdateChannelListAsync();
      this.OnPropertyChanged((object) this, "IsCloneView");
    }

    private async void secondTv_ChannelListTypeChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv - ChannelListTypeChanged()");
      await this.tvController.UpdateChannelListAsync();
    }

    private async void secondTv_SourceListChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv - SourceListChanged()");
      await this.tvController.UpdateSourceListAsync();
      this.OnPropertyChanged((object) this, "IsCloneView");
    }

    private async void secondTv_MbrDeviceListChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv = MbrDeviceListChanged()");
      await this.tvController.UpdateSourceListAsync();
    }

    private async void secondTv_SourceDisconnected(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv = SourceDisconnected()");
      await this.tvController.SourceDisconnect();
    }

    private void secondTv_PriorityDisconnect(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv = PriorityDisconnect()");
      this.tvController.LowPriority();
    }

    private void secondTv_PowerOff(object sender, EventArgs e) => this.OnDisconnecting((object) this, EventArgs.Empty);

    private void secondTv_ShowInputKeyboard(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv = Show input keyboard()");
      this.dispatcher.Invoke((Action) (() => this.OnShowInputKeyboard((object) this, EventArgs.Empty)));
    }

    private void secondTv_ShowPasswordKeyboard(object sender, EventArgs e) => this.dispatcher.Invoke((Action) (() => this.OnShowPasswordKeyboard((object) this, EventArgs.Empty)));

    private void secondTv_TextUpdated(object sender, UpdateTextEventArgs e) => this.dispatcher.Invoke((Action) (() => this.OnTextUpdated((object) this, e)));

    private void secondTv_HideKeyboard(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: secondTv = Hide input keyboard()");
      this.dispatcher.Invoke((Action) (() => this.OnHideKeyboard((object) this, EventArgs.Empty)));
    }

    private void playerNotification_StreamMediaChanged(object sender, StreamMediaEventArgs e)
    {
      Logger.Instance.LogMessageFormat("TVDevice: playerNotification - StreamMediaChanged() to {0}", (object) e.StreamMedia);
      this.StreamType = e.StreamMedia;
      try
      {
        this.VideoMessageInfoType = new MessageType?((MessageType) Enum.Parse(typeof (MessageType), this.streamType.ToString()));
      }
      catch
      {
        Logger.Instance.LogMessage("Unknown stream type.");
      }
    }

    private void playerNotification_StreamPmtInfoChanged(object sender, EventArgs e) => Logger.Instance.LogMessage("TVDevice: playerNotification - StreamPmtInfoChanged()");

    private async void playerNotification_StreamResolutionChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: playerNotification - StreamResolutionChanged()");
      await this.tvController.UpdateBannerInfoAsync();
      this.OnPropertyChanged((object) this, "IsCloneView");
    }

    private async void playerNotification_StreamVersionChanged(object sender, EventArgs e)
    {
      Logger.Instance.LogMessage("TVDevice: playerNotification - StreamVersionChanged()");
      await this.tvController.UpdateBannerInfoAsync();
      this.OnPropertyChanged((object) this, "IsCloneView");
    }

    private void playerNotification_StreamAudioSampleRateChanged(object sender, EventArgs e) => Logger.Instance.LogMessage("TVDevice: playerNotification - StreamAudioSampleRateChanged()");

    private void playerNotification_StreamVideoStatusChanged(object sender, string e)
    {
      Logger.Instance.LogMessage("TVDevice: playerNotification - StreamVideoStatusChanged to " + e);
      this.StreamStatus = e;
      if (e.ToLower().Contains("started"))
        this.IsVideoStarted = true;
      else
        this.IsVideoStarted = false;
    }

    private void playerNotification_VideoShutDown(object sender, EventArgs e)
    {
      if (this.isRemoteControl)
        return;
      this.OnVideoShutDown(sender, e);
    }

    private void playerNotification_CCDataReceived(object sender, CCDataEventArgs e) => this.OnCCDataReceived((object) this, e);

    private void TvDevice_ViewStarted(object sender, EventArgs e) => this.OnViewStarted(sender, e);

    public async Task CloseAsync() => await this.tvController.CloseAsync();

    public async Task RestartView() => await this.tvController.SourceDisconnect();

    public void ShowErrorMessage(MessageType messageType, bool isPopup = false)
    {
      if (isPopup)
        this.OnErrorMessageArised((object) this, messageType);
      else
        this.VideoMessageInfoType = new MessageType?(messageType);
    }

    public async void Dispose()
    {
      if (this.tvController != null)
        this.tvController.ViewStarted -= new EventHandler(this.TvDevice_ViewStarted);
      if (this.secondTv != null)
      {
        this.secondTv.ChannelListChanged -= new EventHandler<EventArgs>(this.secondTv_ChannelListChanged);
        this.secondTv.ChannelChanged -= new EventHandler<EventArgs>(this.secondTv_ChannelChanged);
        this.secondTv.SourceListChanged -= new EventHandler<EventArgs>(this.secondTv_SourceListChanged);
        this.secondTv.ChannelListTypeChanged -= new EventHandler<EventArgs>(this.secondTv_ChannelListTypeChanged);
        this.secondTv.MbrDeviceListChanged -= new EventHandler<EventArgs>(this.secondTv_MbrDeviceListChanged);
        this.secondTv.SourceDisconnected -= new EventHandler<EventArgs>(this.secondTv_SourceDisconnected);
        this.secondTv.PriorityDisconnect -= new EventHandler<EventArgs>(this.secondTv_PriorityDisconnect);
        this.secondTv.PowerOff -= new EventHandler<EventArgs>(this.secondTv_PowerOff);
        this.secondTv.Dispose();
        this.secondTv = (ISecondTv) null;
      }
      if (this.remoteInput != null)
      {
        this.remoteInput.ShowInputKeyboard -= new EventHandler<EventArgs>(this.secondTv_ShowInputKeyboard);
        this.remoteInput.ShowPasswordKeyboard -= new EventHandler<EventArgs>(this.secondTv_ShowPasswordKeyboard);
        this.remoteInput.TextUpdated -= new EventHandler<UpdateTextEventArgs>(this.secondTv_TextUpdated);
        this.remoteInput.HideKeyboard -= new EventHandler<EventArgs>(this.secondTv_HideKeyboard);
        this.remoteInput.Dispose();
        this.remoteInput = (IRemoteInput) null;
      }
      if (this.playerNotification != null)
      {
        this.playerNotification.StreamAudioSampleRateChanged -= new EventHandler<EventArgs>(this.playerNotification_StreamAudioSampleRateChanged);
        this.playerNotification.StreamVersionChanged -= new EventHandler<EventArgs>(this.playerNotification_StreamVersionChanged);
        this.playerNotification.StreamResolutionChanged -= new EventHandler<EventArgs>(this.playerNotification_StreamResolutionChanged);
        this.playerNotification.StreamPmtInfoChanged -= new EventHandler<EventArgs>(this.playerNotification_StreamPmtInfoChanged);
        this.playerNotification.StreamMediaChanged -= new EventHandler<StreamMediaEventArgs>(this.playerNotification_StreamMediaChanged);
        this.playerNotification.StreamVideoStatusChanged -= new EventHandler<string>(this.playerNotification_StreamVideoStatusChanged);
        this.playerNotification.VideoShutDown -= new EventHandler(this.playerNotification_VideoShutDown);
        this.playerNotification.CCDataReceived -= new EventHandler<CCDataEventArgs>(this.playerNotification_CCDataReceived);
        this.playerNotification = (IPlayerNotificationProvider) null;
      }
      if (this.httpServer != null)
        this.httpServer.Dispose();
      if (this.multiScreen != null)
        this.multiScreen.Dispose();
      if (this.dlnaServer == null)
        return;
      this.dlnaServer.Dispose();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(object sender, string propertyName) => this.dispatcher.Invoke((Action) (() =>
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(sender, new PropertyChangedEventArgs(propertyName));
    }));

    public event EventHandler<EventArgs> ShowInputKeyboard;

    private void OnShowInputKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> showInputKeyboard = this.ShowInputKeyboard;
      if (showInputKeyboard == null)
        return;
      showInputKeyboard(sender, e);
    }

    public event EventHandler<EventArgs> ShowPasswordKeyboard;

    private void OnShowPasswordKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> passwordKeyboard = this.ShowPasswordKeyboard;
      if (passwordKeyboard == null)
        return;
      passwordKeyboard(sender, e);
    }

    public event EventHandler<UpdateTextEventArgs> TextUpdated;

    private void OnTextUpdated(object sender, UpdateTextEventArgs e)
    {
      EventHandler<UpdateTextEventArgs> textUpdated = this.TextUpdated;
      if (textUpdated == null)
        return;
      textUpdated(sender, e);
    }

    public event EventHandler<EventArgs> HideKeyboard;

    private void OnHideKeyboard(object sender, EventArgs e)
    {
      EventHandler<EventArgs> hideKeyboard = this.HideKeyboard;
      if (hideKeyboard == null)
        return;
      hideKeyboard(sender, e);
    }

    public event EventHandler<ErrorMessageEventArgs> ErrorMessageArised;

    private void OnErrorMessageArised(object sender, MessageType messageType)
    {
      EventHandler<ErrorMessageEventArgs> errorMessageArised = this.ErrorMessageArised;
      if (errorMessageArised == null)
        return;
      errorMessageArised(sender, new ErrorMessageEventArgs(messageType));
    }

    public event EventHandler<EventArgs> Disconnecting;

    private void OnDisconnecting(object sender, EventArgs e)
    {
      EventHandler<EventArgs> disconnecting = this.Disconnecting;
      if (disconnecting == null)
        return;
      disconnecting(sender, e);
    }

    public event EventHandler VideoShutDown;

    private void OnVideoShutDown(object sender, EventArgs args)
    {
      EventHandler videoShutDown = this.VideoShutDown;
      if (videoShutDown == null)
        return;
      videoShutDown(sender, args);
    }

    public event EventHandler<CCDataEventArgs> CCDataReceived;

    private void OnCCDataReceived(object sender, CCDataEventArgs e)
    {
      if (this.CCDataReceived == null)
        return;
      this.CCDataReceived(sender, e);
    }

    public event EventHandler ViewStarted;

    private void OnViewStarted(object sender, EventArgs e)
    {
      EventHandler viewStarted = this.ViewStarted;
      if (viewStarted == null)
        return;
      viewStarted(sender, e);
    }
  }
}
