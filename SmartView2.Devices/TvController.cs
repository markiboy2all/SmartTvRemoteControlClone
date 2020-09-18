// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvController
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.DataContracts;
using SmartView2.Devices.SecondTv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartView2.Devices
{
  public class TvController
  {
    private readonly ITargetDevice targetDevice;
    private readonly ISecondTv secondTv;
    private SourceListInfo sourceList;
    private ChannelListInfo channelList;
    private IEnumerable<MbrDeviceInfo> mbrDevices;
    private BaseChannelInfo mainChannel;
    private BaseChannelInfo secondChannel;
    private BaseSourceInfo mainSource;
    private BaseSourceInfo secondSource;
    private BannerInfo bannerInfo;
    private AntennaMode currentAntennaMode;
    private ChannelType[] channelListTypes;
    private bool isRemoteControl;
    internal Timer timer;
    private bool isStarting;
    private bool isChannelUpdateOn;

    public TvCapabilities Capabilities { get; private set; }

    public bool IsCloneView
    {
      get
      {
        if (this.mainSource != null && this.secondSource != null && this.mainSource.Type == this.secondSource.Type)
        {
          if (this.mainSource.Type != SourceType.TV)
            return true;
          if (this.mainChannel != null && this.secondChannel != null)
          {
            if (this.mainChannel.MajorChannel == this.secondChannel.MajorChannel && this.mainChannel.MinorChannel == this.secondChannel.MinorChannel && (this.mainChannel.PTC == this.secondChannel.PTC && this.mainChannel.Type == this.secondChannel.Type))
              return true;
          }
          else if (this.mainChannel != null && this.secondChannel == null)
            return true;
        }
        return this.mainSource == null && this.secondSource == null || this.mainSource != null && this.secondSource == null;
      }
    }

    public TvController(ITargetDevice targetDevice, ISecondTv secondTv)
    {
      if (targetDevice == null)
        throw new ArgumentNullException(nameof (targetDevice));
      if (secondTv == null)
        throw new ArgumentNullException(nameof (secondTv));
      this.targetDevice = targetDevice;
      this.secondTv = secondTv;
    }

    public async Task InitializeAsync(CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      this.Capabilities = await this.secondTv.GetAvailableActionsAsync();
      if (token.IsCancellationRequested)
        token.ThrowIfCancellationRequested();
      await this.UpdateSourceListAsync();
      if (token.IsCancellationRequested)
        token.ThrowIfCancellationRequested();
      await this.UpdateMainChannelAsync();
      if (token.IsCancellationRequested)
        token.ThrowIfCancellationRequested();
      await this.StartCloneViewAsync();
      if (token.IsCancellationRequested)
        token.ThrowIfCancellationRequested();
      if (!(this.targetDevice.VideoUrl != (Uri) null))
        return;
      this.OnViewStarted();
    }

    public async Task SetChannelAsync(Channel newChannel)
    {
      if (newChannel == null)
        return;
      BaseChannelInfo nativeChannel = new BaseChannelInfo()
      {
        MajorChannel = newChannel.MajorNumber,
        MinorChannel = newChannel.MinorNumber,
        ProgramNumber = newChannel.ProgramNumber,
        PTC = newChannel.PTC,
        Type = newChannel.Type
      };
      await this.StartViewByChannelAsync(nativeChannel);
    }

    public async Task SetSourceAsync(Source newSource)
    {
      Logger.Instance.LogMessage("[SmartView2][TvController]SetSourceAsync started...");
      if (newSource == null)
      {
        Logger.Instance.LogMessage("[SmartView2][TvController]SetSourceAsync: can't change source. Source is null");
      }
      else
      {
        if (this.isRemoteControl)
        {
          Logger.Instance.LogMessageFormat("[SmartView2][TvController]SetSourceAsync: Try ChangeSourceOnTvAsync in remotecontrol... to: {0}", (object) newSource.Type);
          await this.ChangeSourceOnTvAsync(newSource);
        }
        else
        {
          Logger.Instance.LogMessageFormat("[SmartView2][TvController]SetSourceAsync: Try ChangeSourceOnTvAsync in tv view... to: {0}", (object) newSource.Type);
          if (newSource != this.targetDevice.CurrentSource && newSource.IsValid)
          {
            if (newSource.Type == SourceType.TV)
            {
              BaseChannelInfo ch = (BaseChannelInfo) null;
              if (this.secondChannel != null)
                ch = this.secondChannel;
              else if (this.mainChannel != null)
                ch = this.mainChannel;
              if (ch != null && this.mainSource != null && (this.IsAnalogSource(this.mainSource.Type) && this.IsAnalogChannel(ch.Type)))
                this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
              else
                await this.StartViewByChannelAsync(ch);
            }
            else
            {
              BaseSourceInfo nativeSource = new BaseSourceInfo()
              {
                Id = newSource.Id,
                MbrActivityIndex = newSource.IsMbr ? newSource.MbrDevice.ActivityIndex : 0,
                Type = newSource.Type
              };
              BaseSourceInfo s = (BaseSourceInfo) null;
              if (this.secondSource != null)
                s = this.secondSource;
              else if (this.mainSource != null)
                s = this.mainSource;
              BaseChannelInfo ch = (BaseChannelInfo) null;
              if (this.secondChannel != null)
                ch = this.secondChannel;
              else if (this.mainChannel != null)
                ch = this.mainChannel;
              if (s != null && ch != null && (s.Type == SourceType.TV && this.IsAnalogSource(nativeSource.Type)) && this.IsAnalogChannel(ch.Type))
                this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
              else if (s == null && ch != null && (this.IsAnalogSource(nativeSource.Type) && this.IsAnalogChannel(ch.Type)))
                this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
              else
                await this.StartViewBySourceAsync(nativeSource);
            }
            this.UpdateCurrentSource();
          }
        }
        Logger.Instance.LogMessage("[SmartView2][TvController]SetSourceAsync ended...");
      }
    }

    public async Task ChannelUpAsync()
    {
      Source tvSource = this.targetDevice.SourceList.FirstOrDefault<Source>((Func<Source, bool>) (s => s.Type == SourceType.TV));
      if (tvSource == null)
        return;
      Channel newChannel = tvSource.ChannelList.GetNextChannel(tvSource.CurrentChannel);
      BaseChannelInfo nativeChannel = new BaseChannelInfo()
      {
        MajorChannel = newChannel.MajorNumber,
        MinorChannel = newChannel.MinorNumber,
        ProgramNumber = newChannel.ProgramNumber,
        PTC = newChannel.PTC,
        Type = newChannel.Type
      };
      await this.StartViewByChannelAsync(nativeChannel);
    }

    public async Task ChannelDownAsync()
    {
      Source tvSource = this.targetDevice.SourceList.FirstOrDefault<Source>((Func<Source, bool>) (s => s.Type == SourceType.TV));
      if (tvSource == null)
        return;
      Channel newChannel = tvSource.ChannelList.GetPreviousChannel(tvSource.CurrentChannel);
      BaseChannelInfo nativeChannel = new BaseChannelInfo()
      {
        MajorChannel = newChannel.MajorNumber,
        MinorChannel = newChannel.MinorNumber,
        ProgramNumber = newChannel.ProgramNumber,
        PTC = newChannel.PTC,
        Type = newChannel.Type
      };
      await this.StartViewByChannelAsync(nativeChannel);
    }

    public async Task SetIsRemoteControl(bool value)
    {
      this.isRemoteControl = value;
      if (this.isRemoteControl)
      {
        if (this.targetDevice.VideoUrl != (Uri) null)
        {
          try
          {
            this.secondTv.StopViewAsync(this.targetDevice.VideoUrl.ToString()).Start();
          }
          catch
          {
          }
          this.targetDevice.VideoUrl = (Uri) null;
        }
      }
      else
      {
        this.UpdateCurrentSource();
        await this.UpdateCurrentChannel();
        try
        {
          await this.StartViewAsync();
        }
        catch (SecondTvException ex)
        {
          if (ex.ErrorType == ErrorCode.NOTOK_Recording)
            this.targetDevice.ShowErrorMessage(MessageType.Recording);
        }
      }
      this.UpdateCurrentSource();
    }

    public async Task SendDeviceViewToTv()
    {
      Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Started...");
      try
      {
        this.isStarting = true;
        if (this.targetDevice.CurrentSource.Type == SourceType.TV)
        {
          Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Current Source is TV");
          if (this.secondChannel != null)
          {
            if (this.mainSource != null)
            {
              if (this.mainSource.Type == SourceType.TV)
              {
                if (this.secondChannel.MajorChannel == this.mainChannel.MajorChannel)
                {
                  if (this.secondChannel.MinorChannel == this.mainChannel.MinorChannel)
                  {
                    if (this.secondChannel.ProgramNumber == this.mainChannel.ProgramNumber)
                    {
                      if (this.secondChannel.PTC == this.mainChannel.PTC)
                      {
                        if (this.secondChannel.Type == this.mainChannel.Type)
                        {
                          Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Ended...");
                          return;
                        }
                      }
                    }
                  }
                }
              }
            }
            try
            {
              Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Call SetMainTvChannelAsync");
              await this.secondTv.SetMainTvChannelAsync(this.secondChannel, this.channelList.ChannelListType, this.channelList.SatelliteId);
              await this.UpdateMainSourceAsync();
              Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Ended...");
              return;
            }
            catch (SecondTvException ex)
            {
              Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv catch SecondTvException with: " + (object) ex.ErrorType);
              this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
            }
            catch (Exception ex)
            {
              Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv catch Exception: " + ex.Message);
              this.targetDevice.ShowErrorMessage(MessageType.NeedRestart);
            }
          }
        }
        else if (this.mainSource != null && this.secondSource != null && this.secondSource.Id != this.mainSource.Id)
        {
          Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Current Source is TV");
          try
          {
            Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Call SetMainTvChannelAsync");
            await this.secondTv.SetMainTvSourceAsync(this.secondSource);
          }
          catch (SecondTvException ex)
          {
            Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv catch SecondTvException with: " + (object) ex.ErrorType);
            this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
          }
          catch (Exception ex)
          {
            Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv catch Exception: " + ex.Message);
            this.targetDevice.ShowErrorMessage(MessageType.NeedRestart);
          }
          await this.UpdateMainSourceAsync();
        }
        else
        {
          Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Ended...");
          return;
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv catch Exception: " + ex.Message);
        this.targetDevice.ShowErrorMessage(MessageType.NeedRestart);
      }
      finally
      {
        this.isStarting = false;
      }
      await this.StartViewAsync();
      Logger.Instance.LogMessage("[SmartView2][TvController]SendDeviceViewToTv Ended...");
    }

    public async Task SendTvViewToDevice()
    {
      if (this.mainSource != null)
      {
        if (this.mainSource.Type == SourceType.TV)
        {
          if (this.mainChannel == null)
            return;
          await this.StartViewByChannelAsync(this.mainChannel);
        }
        else if (this.mainSource.Type.ToString().ToLower().Contains("hdmi"))
          this.targetDevice.ShowErrorMessage(MessageType.HdmiConflict, true);
        else
          await this.StartViewBySourceAsync(this.mainSource);
      }
      else
        this.targetDevice.ShowErrorMessage(MessageType.AnalogConflict, true);
    }

    internal async Task MainChannelUpdateAsync()
    {
      if (this.isChannelUpdateOn)
        return;
      this.isChannelUpdateOn = true;
      await this.UpdateMainChannelAsync();
      if (this.targetDevice.ViewMode == ViewMode.None)
        await this.StartViewAsync();
      this.isChannelUpdateOn = false;
    }

    internal async Task UpdateBannerInfoAsync()
    {
      try
      {
        this.bannerInfo = await this.secondTv.GetBannerInformationAsync();
        this.secondChannel = (BaseChannelInfo) null;
        this.secondSource = (BaseSourceInfo) null;
        this.UpdateSecondTvConfig();
        this.UpdateProgramTime();
      }
      catch (SecondTvException ex)
      {
        this.targetDevice.StartProgramTime = this.targetDevice.EndProgramTime = -1L;
        if (ex.ErrorType == ErrorCode.NOTOK_NotStreaming)
        {
          this.secondChannel = this.mainChannel;
          this.secondSource = this.mainSource;
        }
      }
      catch (TimeoutException ex)
      {
        Logger.Instance.LogMessage("[SmartView2][TvController]Time out of banner info.");
      }
      catch (Exception ex)
      {
        Logger.Instance.LogMessage("[SmartView2][TvController]Unknown exception: " + ex.Message);
      }
      try
      {
        this.UpdateCurrentSource();
        if (this.targetDevice.CurrentSource.Type != SourceType.TV)
          return;
        await this.UpdateCurrentChannel();
      }
      catch
      {
      }
    }

    private void UpdateProgramTime()
    {
      DateTime result1 = DateTime.MinValue;
      DateTime result2 = DateTime.MinValue;
      if (!string.IsNullOrEmpty(this.bannerInfo.StartTime))
        DateTime.TryParse(this.bannerInfo.StartTime, out result1);
      if (!string.IsNullOrEmpty(this.bannerInfo.EndTime))
        DateTime.TryParse(this.bannerInfo.EndTime, out result2);
      this.targetDevice.StartProgramTime = !(result1 == DateTime.MinValue) ? result1.Ticks : -1L;
      this.targetDevice.EndProgramTime = !(result2 == DateTime.MinValue) ? result2.Ticks : -1L;
      if (this.targetDevice.IsProgramTimeAvaliable)
      {
        if (this.timer != null)
          return;
        this.timer = new Timer((TimerCallback) (o => this.targetDevice.NowTime = DateTime.Now.Ticks), (object) null, 0, 1000);
      }
      else
      {
        if (this.timer == null)
          return;
        this.timer.Dispose();
        this.timer = (Timer) null;
      }
    }

    internal async Task UpdateChannelListAsync()
    {
      try
      {
        this.channelList = await this.secondTv.GetChannelListAsync();
        Source tvSource = this.targetDevice.SourceList.FirstOrDefault<Source>((Func<Source, bool>) (s => s.Type == SourceType.TV));
        if (tvSource != null)
        {
          ChannelList chList = await this.GetChannelList();
          this.targetDevice.SetChannelList(tvSource, chList);
        }
        if (this.targetDevice.CurrentSource == null || this.targetDevice.CurrentSource.Type != SourceType.TV)
          return;
        await this.UpdateCurrentChannel();
      }
      catch
      {
      }
    }

    internal async Task UpdateSourceListAsync()
    {
      try
      {
        this.sourceList = await this.secondTv.GetSourceListAsync();
        if (this.Capabilities.MbrDeviceListAvailable)
          this.mbrDevices = (IEnumerable<MbrDeviceInfo>) await this.secondTv.GetMbrDeviceListAsync();
        else
          this.mbrDevices = (IEnumerable<MbrDeviceInfo>) new List<MbrDeviceInfo>();
        SourceList sList = this.GetSourceList();
        await this.UpdateMainSourceAsync();
        bool isSourceListUpdated = true;
        if (this.targetDevice.SourceList != null && this.targetDevice.SourceList.Count == sList.Count)
        {
          int index = 0;
          while (index < sList.Count && sList[index].Id == this.targetDevice.SourceList[index].Id)
            ++index;
          isSourceListUpdated = false;
        }
        if (isSourceListUpdated)
          this.targetDevice.SourceList = sList;
        this.targetDevice.InitializeRemoteControl();
        this.UpdateCurrentSource();
        await this.UpdateChannelListAsync();
      }
      catch
      {
      }
    }

    internal async Task SourceDisconnect()
    {
      try
      {
        await this.StartViewAsync();
      }
      catch (SecondTvException ex)
      {
        if (ex.ErrorType != ErrorCode.NOTOK_Recording)
          return;
        this.targetDevice.ShowErrorMessage(MessageType.Recording);
      }
    }

    internal void LowPriority()
    {
      if (this.isRemoteControl)
        return;
      this.targetDevice.ShowErrorMessage(MessageType.LowPriority);
    }

    internal async Task<bool> StopRecordAsync()
    {
      if (this.targetDevice == null || this.targetDevice.CurrentSource == null || (this.targetDevice.CurrentSource.Type != SourceType.TV || this.targetDevice.CurrentSource.CurrentChannel == null))
        return false;
      BaseChannelInfo baseChannel = new BaseChannelInfo()
      {
        MajorChannel = this.targetDevice.CurrentSource.CurrentChannel.MajorNumber,
        MinorChannel = this.targetDevice.CurrentSource.CurrentChannel.MinorNumber,
        ProgramNumber = this.targetDevice.CurrentSource.CurrentChannel.ProgramNumber,
        PTC = this.targetDevice.CurrentSource.CurrentChannel.PTC,
        Type = this.targetDevice.CurrentSource.CurrentChannel.Type
      };
      try
      {
        await this.secondTv.StopRecordAsync(baseChannel);
      }
      catch (SecondTvException ex)
      {
        return ex.ErrorType == ErrorCode.NOTOK_ShortDuration;
      }
      return true;
    }

    private async Task UpdateMainChannelAsync()
    {
      try
      {
        this.mainChannel = await this.secondTv.GetCurrentChannelAsync();
        if (!this.isRemoteControl || this.mainChannel == null || (this.targetDevice.CurrentSource.Type != SourceType.TV || this.targetDevice.CurrentSource.ChannelList.Count <= 0))
          return;
        Channel currCh = this.targetDevice.CurrentSource.ChannelList.GetChannel(this.mainChannel.MajorChannel, this.mainChannel.MinorChannel, this.mainChannel.PTC, this.mainChannel.Type);
        if (currCh == null)
          return;
        ProgramInfo channelProgram = await this.secondTv.GetProgramInformationAsync(this.currentAntennaMode, this.mainChannel);
        currCh.ProgramTitle = channelProgram == null ? string.Empty : channelProgram.ProgramTitle;
        this.targetDevice.SetCurrentChannel(this.targetDevice.CurrentSource, currCh);
      }
      catch
      {
      }
    }

    private async Task StartViewAsync(bool fromRemoteControl = false)
    {
      if (this.isRemoteControl || this.isStarting)
        return;
      Logger.Instance.LogMessage("Try start view...");
      this.isStarting = true;
      try
      {
        if (this.bannerInfo != null)
        {
          if (this.bannerInfo.CurrentMode == "CloneView")
          {
            if (this.mainSource.Type.ToString().ToLower().Contains("hdmi"))
            {
              if (this.secondChannel != null)
              {
                await this.StartViewByChannelAsync(this.secondChannel);
              }
              else
              {
                if (this.secondSource == null || this.secondSource.Type.ToString().ToLower().Contains("hdmi"))
                  return;
                await this.StartViewBySourceAsync(this.secondSource);
              }
            }
            else
              await this.StartCloneViewAsync();
          }
          else if (this.secondChannel != null)
          {
            Logger.Instance.LogMessage("Try start StartViewByChannel");
            if (fromRemoteControl)
            {
              if (this.mainSource != null && this.IsAnalogSource(this.mainSource.Type) && this.IsAnalogChannel(this.secondChannel.Type))
                await this.StartCloneViewAsync();
              else
                await this.StartViewByChannelAsync(this.secondChannel);
            }
            else
              await this.StartViewByChannelAsync(this.secondChannel);
          }
          else if (this.secondSource != null)
          {
            Logger.Instance.LogMessage("Try start StartViewBySource");
            if (fromRemoteControl)
            {
              if (this.IsAnalogSource(this.secondSource.Type) && this.IsAnalogChannel(this.mainChannel.Type))
                await this.StartCloneViewAsync();
              else
                await this.StartViewBySourceAsync(this.secondSource);
            }
            else
              await this.StartViewBySourceAsync(this.secondSource);
          }
          else
            await this.StartCloneViewAsync();
        }
        else
        {
          if (this.mainChannel == null && this.mainSource == null)
            return;
          Logger.Instance.LogMessage("Try start StartCloneView");
          await this.StartCloneViewAsync();
        }
      }
      finally
      {
        this.isStarting = false;
        if (this.targetDevice.VideoUrl != (Uri) null)
          this.OnViewStarted();
      }
    }

    private void OnViewStarted()
    {
      if (this.ViewStarted == null)
        return;
      this.ViewStarted((object) this, new EventArgs());
    }

    private bool IsAnalogSource(SourceType sourceType)
    {
      string lower = sourceType.ToString().ToLower();
      return lower.Contains("av") || lower.Contains("scart") || lower.Contains("component");
    }

    private bool IsAnalogChannel(ChannelType channelType) => channelType.ToString().ToLower().Contains("atv");

    private async Task StartViewByChannelAsync(BaseChannelInfo channel, ForcedFlag forsed = ForcedFlag.Normal)
    {
      ErrorCode secondTvException = ErrorCode.OK;
      string url = string.Empty;
      this.targetDevice.VideoUrl = (Uri) null;
      this.targetDevice.ViewMode = ViewMode.Unknown;
      this.targetDevice.VideoMessageInfoType = new MessageType?();
      try
      {
        int channelListType = (int) this.channelList.ChannelListType;
        int satelliteId = this.channelList.SatelliteId;
        url = await this.secondTv.StartSecondTvViewAsync(channel, this.channelList.ChannelListType, this.channelList.SatelliteId, forsed);
      }
      catch (SecondTvException ex)
      {
        Logger.Instance.LogMessage("Can't start View. StartSecondTvViewAsync error type: " + ex.ErrorType.ToString());
        secondTvException = ex.ErrorType;
      }
      catch (Exception ex)
      {
        Logger.Instance.LogMessage("Can't start View. Error: " + ex.Message);
      }
      if (secondTvException != ErrorCode.OK)
        url = await this.ResolveChannelViewErrorAsync(channel, secondTvException);
      this.targetDevice.VideoUrl = this.GetESPUrlFromString(url);
      if (this.targetDevice.VideoUrl != (Uri) null)
      {
        Logger.Instance.LogMessage("...View started.");
        if (this.targetDevice.ViewMode == ViewMode.Unknown)
          this.targetDevice.ViewMode = ViewMode.Dual;
        await this.UpdateBannerInfoAsync();
      }
      else
      {
        this.targetDevice.ViewMode = ViewMode.None;
        this.targetDevice.ShowErrorMessage(MessageType.NeedRestart);
        Logger.Instance.LogMessage("...View not started.");
      }
    }

    private async Task StartViewBySourceAsync(BaseSourceInfo source, ForcedFlag forsed = ForcedFlag.Normal)
    {
      ErrorCode secondTvException = ErrorCode.OK;
      string url = string.Empty;
      this.targetDevice.VideoUrl = (Uri) null;
      this.targetDevice.ViewMode = ViewMode.Unknown;
      this.targetDevice.VideoMessageInfoType = new MessageType?();
      if (source == null)
        this.targetDevice.ShowErrorMessage(MessageType.SourceConflict);
      else if (source.Type.ToString().ToLower().Contains("hdmi"))
      {
        this.targetDevice.ShowErrorMessage(MessageType.HdmiConflict);
      }
      else
      {
        try
        {
          url = await this.secondTv.StartExternalSourceViewAsync(source, forsed);
        }
        catch (SecondTvException ex)
        {
          Logger.Instance.LogMessage("Can't start View. StartExternalSourceViewAsync error type: " + ex.ErrorType.ToString());
          secondTvException = ex.ErrorType;
        }
        catch (Exception ex)
        {
          Logger.Instance.LogMessage("Can't start View. Error: " + ex.Message);
        }
        if (secondTvException != ErrorCode.OK)
          url = await this.ResolveSourceViewErrorAsync(source, secondTvException);
        this.targetDevice.VideoUrl = this.GetESPUrlFromString(url);
        if (this.targetDevice.VideoUrl != (Uri) null)
        {
          Logger.Instance.LogMessage("...View started.");
          if (this.targetDevice.ViewMode == ViewMode.Unknown)
            this.targetDevice.ViewMode = ViewMode.Dual;
          await this.UpdateBannerInfoAsync();
        }
        else
        {
          this.targetDevice.ViewMode = ViewMode.None;
          Logger.Instance.LogMessage("...View not started.");
          this.targetDevice.ShowErrorMessage(MessageType.NeedRestart);
        }
      }
    }

    private async Task StartCloneViewAsync()
    {
      ErrorCode secondTvException = ErrorCode.OK;
      string url = string.Empty;
      this.targetDevice.VideoUrl = (Uri) null;
      this.targetDevice.ViewMode = ViewMode.Unknown;
      this.targetDevice.VideoMessageInfoType = new MessageType?();
      if (this.mainSource == null)
      {
        Logger.Instance.LogMessage("[SmartView2.Device][TvController]StartCloneViewAsync mainSource is null");
        this.targetDevice.ShowErrorMessage(MessageType.SourceConflict);
      }
      else
      {
        await this.UpdateMainSourceAsync();
        if (this.mainSource.Type.ToString().ToLower().Contains("hdmi"))
        {
          Logger.Instance.LogMessage("[SmartView2.Device][TvController]StartCloneViewAsync mainSource is HDMI");
          this.targetDevice.ShowErrorMessage(MessageType.HdmiConflict);
        }
        else
        {
          try
          {
            url = await this.secondTv.StartCloneViewAsync();
          }
          catch (SecondTvException ex)
          {
            Logger.Instance.LogMessage("Can't start View. StartCloneView error type: " + ex.ErrorType.ToString());
            secondTvException = ex.ErrorType;
          }
          catch (Exception ex)
          {
            Logger.Instance.LogMessage("Can't start View. Error: " + ex.Message);
          }
          if (secondTvException != ErrorCode.OK)
            url = this.ResolveCloneViewErrorAsync(secondTvException);
          this.targetDevice.VideoUrl = this.GetESPUrlFromString(url);
          if (this.targetDevice.VideoUrl != (Uri) null)
          {
            Logger.Instance.LogMessage("...View started.");
            this.targetDevice.ViewMode = ViewMode.Clone;
            await this.UpdateBannerInfoAsync();
          }
          else
          {
            this.targetDevice.ViewMode = ViewMode.None;
            Logger.Instance.LogMessage("...View not started.");
          }
        }
      }
    }

    private async Task<string> ResolveChannelViewErrorAsync(
      BaseChannelInfo channel,
      ErrorCode errorCode)
    {
      Logger.Instance.LogMessage("Try resolve channel view error: " + errorCode.ToString());
      switch (errorCode)
      {
        case ErrorCode.NOTOK_InvalidCh:
          await this.StartCloneViewAsync();
          break;
        case ErrorCode.NOTOK_UseADOff:
          Logger.Instance.LogMessage("Try start StartViewByChannel with AdOff");
          await this.StartViewByChannelAsync(channel, ForcedFlag.AdOff);
          break;
        case ErrorCode.NOTOK_SourceConflict:
          await this.StartViewAsync();
          break;
        case ErrorCode.NOTOK_UseCloneView:
          if (this.mainChannel != null || this.mainSource != null)
          {
            Logger.Instance.LogMessage("Try start StartCloneViewAsync");
            await this.StartCloneViewAsync();
            break;
          }
          break;
        case ErrorCode.NOTOK_UseForced:
          Logger.Instance.LogMessage("Try start StartViewByChannel with Forced");
          await this.StartViewByChannelAsync(channel, ForcedFlag.Forced);
          break;
        case ErrorCode.NOTOK_SourceConflictAndDisconnected:
          await this.StartViewAsync();
          break;
        default:
          this.ChooseErrorMessage(errorCode);
          break;
      }
      return !(this.targetDevice.VideoUrl != (Uri) null) ? (string) null : this.targetDevice.VideoUrl.ToString();
    }

    private async Task<string> ResolveSourceViewErrorAsync(
      BaseSourceInfo source,
      ErrorCode errorCode)
    {
      Logger.Instance.LogMessage("Try resolve source view error: " + errorCode.ToString());
      switch (errorCode)
      {
        case ErrorCode.NOTOK_UseADOff:
          Logger.Instance.LogMessage("Try start StartViewBySource with AdOff");
          await this.StartViewBySourceAsync(source, ForcedFlag.AdOff);
          break;
        case ErrorCode.NOTOK_SourceConflict:
          this.targetDevice.ShowErrorMessage(MessageType.SourceConflict, true);
          break;
        case ErrorCode.NOTOK_UseCloneView:
          if (this.mainChannel != null || this.mainSource != null)
          {
            Logger.Instance.LogMessage("Try start StartCloneViewAsync");
            await this.StartCloneViewAsync();
            break;
          }
          break;
        case ErrorCode.NOTOK_UseForced:
          Logger.Instance.LogMessage("Try start StartViewBySource with Forced");
          await this.StartViewBySourceAsync(source, ForcedFlag.Forced);
          break;
        case ErrorCode.NOTOK_SourceConflictAndDisconnected:
          this.targetDevice.ShowErrorMessage(MessageType.SourceConflict, true);
          await this.StartViewAsync();
          break;
        default:
          this.ChooseErrorMessage(errorCode);
          break;
      }
      return !(this.targetDevice.VideoUrl != (Uri) null) ? (string) null : this.targetDevice.VideoUrl.ToString();
    }

    private void ChooseErrorMessage(ErrorCode errorCode)
    {
      switch (errorCode)
      {
        case ErrorCode.NOTOK_Recording:
          throw new SecondTvException(ErrorCode.NOTOK_Recording);
        case ErrorCode.NOTOK_OtherMode:
          this.targetDevice.ShowErrorMessage(MessageType.OtherMode);
          break;
        case ErrorCode.NOTOK_NotConnected:
          this.targetDevice.ShowErrorMessage(MessageType.NotConnected, this.isRemoteControl);
          break;
        case ErrorCode.NOTOK_LowPriority:
          this.targetDevice.ShowErrorMessage(MessageType.AndroidPriority);
          break;
        case ErrorCode.NOTOK_SourceConflict:
          this.targetDevice.ShowErrorMessage(MessageType.SourceConflict);
          break;
        case ErrorCode.NOTOK_SourceConflictAndDisconnected:
          this.targetDevice.ShowErrorMessage(MessageType.SourceConflict);
          break;
        case ErrorCode.NOTOK:
          Logger.Instance.LogMessage("Unknown view error.");
          break;
      }
    }

    private string ResolveCloneViewErrorAsync(ErrorCode errorCode)
    {
      Logger.Instance.LogMessage("Try resolve clone view error: " + errorCode.ToString());
      string empty = string.Empty;
      this.ChooseErrorMessage(errorCode);
      return empty;
    }

    private Uri GetESPUrlFromString(string url)
    {
      if (string.IsNullOrEmpty(url))
        return (Uri) null;
      return new UriBuilder(url) { Scheme = "esp" }.Uri;
    }

    private async Task UpdateCurrentChannel()
    {
      Source tvSource = this.targetDevice.SourceList.FirstOrDefault<Source>((Func<Source, bool>) (s => s.Type == SourceType.TV));
      if (tvSource == null)
        throw new Exception("Can't get tv source");
      if (this.bannerInfo != null)
      {
        if (this.bannerInfo.Channel == null)
          return;
        if (tvSource.ChannelList == null)
          await this.UpdateChannelListAsync();
        Channel currCh = tvSource.ChannelList.GetChannel(this.bannerInfo.Channel.MajorChannel, this.bannerInfo.Channel.MinorChannel, this.bannerInfo.Channel.PTC, this.bannerInfo.Channel.Type);
        if (currCh == null)
          throw new Exception("Can't get current channel");
        currCh.ProgramTitle = this.bannerInfo.ProgramTitle;
        this.targetDevice.SetCurrentChannel(tvSource, currCh);
      }
      else
      {
        if (this.mainChannel == null)
          throw new Exception("Can't get current channel");
        Channel currCh = tvSource.ChannelList.GetChannel(this.mainChannel.MajorChannel, this.mainChannel.MinorChannel, this.mainChannel.PTC, this.mainChannel.Type);
        if (currCh == null)
          throw new Exception("Can't get current channel");
        ProgramInfo channelProgram = await this.secondTv.GetProgramInformationAsync(this.currentAntennaMode, this.mainChannel);
        currCh.ProgramTitle = channelProgram == null ? string.Empty : channelProgram.ProgramTitle;
        this.targetDevice.SetCurrentChannel(tvSource, currCh);
      }
    }

    private void UpdateCurrentSource()
    {
      Source source = (Source) null;
      if (this.isRemoteControl)
      {
        if (this.mainSource != null)
          source = this.targetDevice.SourceList.GetSourceById(this.mainSource.Id) ?? ModelConverter.ToSource(((IEnumerable<SourceInfo>) this.sourceList.SourceList).FirstOrDefault<SourceInfo>((Func<SourceInfo, bool>) (s => s.Id == this.mainSource.Id)));
      }
      else if (this.bannerInfo != null)
      {
        if (this.secondSource != null)
          source = this.targetDevice.SourceList.GetSourceById(this.secondSource.Id);
      }
      else if (this.targetDevice.CurrentSource == null && this.mainSource != null)
        source = this.targetDevice.SourceList.GetSourceById(this.mainSource.Id) ?? ModelConverter.ToSource(((IEnumerable<SourceInfo>) this.sourceList.SourceList).FirstOrDefault<SourceInfo>((Func<SourceInfo, bool>) (s => s.Id == this.mainSource.Id)));
      else if (this.mainSource != null)
        return;
      if (source == null)
        source = new Source()
        {
          Type = SourceType.UNKNOWN_SOURCE,
          Id = -1111
        };
      if (source.RemoteControl == null)
        source.RemoteControl = this.targetDevice.CreateRemoteControl(source);
      this.targetDevice.CurrentSource = source;
    }

    private async Task UpdateMainSourceAsync()
    {
      try
      {
        Logger.Instance.LogMessage("[SmartView2.Device][TvController]UpdateMainSourceAsync start...");
        this.mainSource = await this.secondTv.GetCurrentSourceAsync();
      }
      catch (Exception ex)
      {
        Logger.Instance.LogMessage("[SmartView2.Device][TvController]UpdateMainSourceAsync catch error message: " + ex.Message);
      }
    }

    private void UpdateSecondTvConfig()
    {
      if (this.bannerInfo == null)
        return;
      if (this.bannerInfo.Channel != null)
      {
        this.secondChannel = this.bannerInfo.Channel;
        this.secondSource = (BaseSourceInfo) ((IEnumerable<SourceInfo>) this.sourceList.SourceList).FirstOrDefault<SourceInfo>((Func<SourceInfo, bool>) (s => s.Type == SourceType.TV));
      }
      else
      {
        if (this.bannerInfo.Source == null)
          return;
        this.secondSource = this.bannerInfo.Source;
      }
    }

    private SourceList GetSourceList() => this.sourceList != null && this.sourceList.SourceList != null && this.sourceList.SourceList.Length > 0 ? new SourceList(((IEnumerable<SourceInfo>) this.sourceList.SourceList).Where<SourceInfo>((Func<SourceInfo, bool>) (s => s.Connected)).Select<SourceInfo, Source>((Func<SourceInfo, Source>) (s => ModelConverter.ToSource(s, this.mbrDevices.FirstOrDefault<MbrDeviceInfo>((Func<MbrDeviceInfo, bool>) (m => m.Id == s.Id)))))) : new SourceList();

    private async Task<ChannelList> GetChannelList()
    {
      if (this.channelList == null || this.channelList.ChannelList == null || this.channelList.ChannelList.Length <= 0)
        return new ChannelList();
      this.currentAntennaMode = await this.secondTv.GetCurrentAntennaModeAsync();
      this.channelListTypes = await this.secondTv.GetChannelTypesAsync(this.currentAntennaMode);
      return new ChannelList(((IEnumerable<ChannelInfo>) this.channelList.ChannelList).Where<ChannelInfo>((Func<ChannelInfo, bool>) (ch => ((IEnumerable<ChannelType>) this.channelListTypes).Where<ChannelType>((Func<ChannelType, bool>) (ct => ct == ch.Type)).Count<ChannelType>() > 0)).Select<ChannelInfo, Channel>((Func<ChannelInfo, Channel>) (ch => ModelConverter.ToChannel(ch))))
      {
        Type = this.channelList.ChannelListType,
        SatelliteId = this.channelList.SatelliteId
      };
    }

    private Source GetCurrentSource()
    {
      try
      {
        return this.targetDevice.SourceList.GetSourceById(this.mainSource.Id);
      }
      catch
      {
        throw new Exception("Can't get current source");
      }
    }

    public async Task CloseAsync()
    {
      if (!(this.targetDevice.VideoUrl != (Uri) null))
        return;
      try
      {
        await this.secondTv.StopViewAsync(this.targetDevice.VideoUrl.ToString());
      }
      catch
      {
      }
      this.targetDevice.VideoUrl = (Uri) null;
    }

    private async Task ChangeSourceOnTvAsync(Source newSource)
    {
      if (newSource == null || newSource == this.targetDevice.CurrentSource)
        return;
      SourceInfo source = ((IEnumerable<SourceInfo>) this.sourceList.SourceList).FirstOrDefault<SourceInfo>((Func<SourceInfo, bool>) (s => s.Id == newSource.Id));
      if (source == null)
        return;
      try
      {
        await this.secondTv.SetMainTvSourceAsync((BaseSourceInfo) source);
      }
      catch (SecondTvException ex)
      {
        Logger.Instance.LogMessage("Can't change source on TV. SecondTv error: " + (object) ex.ErrorType);
        this.ChooseErrorMessage(ex.ErrorType);
        this.UpdateCurrentSource();
      }
      catch (Exception ex)
      {
        Logger.Instance.LogMessage("Can't change source on TV. Error: " + ex.Message);
        this.UpdateCurrentSource();
      }
    }

    public event EventHandler ViewStarted;

    private void OnViewStarted(object sender, CCDataEventArgs e)
    {
      if (this.ViewStarted == null)
        return;
      this.ViewStarted(sender, (EventArgs) e);
    }
  }
}
