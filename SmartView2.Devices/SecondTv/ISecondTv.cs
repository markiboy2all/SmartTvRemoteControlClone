// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTv.ISecondTv
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Devices.DataContracts;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices.SecondTv
{
  public interface ISecondTv : IDisposable
  {
    Task<string> StartCloneViewAsync();

    Task<string> StartSecondTvViewAsync(
      BaseChannelInfo channel,
      ChannelListType channelListType,
      int satelliteId,
      ForcedFlag forcedFlag = ForcedFlag.Normal,
      DrmType drmType = DrmType.PrivateTZ);

    Task<string> StartExternalSourceViewAsync(
      BaseSourceInfo source,
      ForcedFlag forcedFlag = ForcedFlag.Normal,
      DrmType drmType = DrmType.PrivateTZ);

    Task StopViewAsync(string videoUrl);

    Task<TvCapabilities> GetAvailableActionsAsync();

    Task<string> GetTargetLocationAsync();

    Task<BaseChannelInfo> GetCurrentChannelAsync();

    Task<BaseChannelInfo> GetRecordedChannelAsync();

    Task<BannerInfo> GetBannerInformationAsync();

    Task<ChannelListInfo> GetChannelListAsync();

    Task<BaseSourceInfo> GetCurrentSourceAsync();

    Task<DtvInfo> GetDtvInformationAsync();

    Task<MbrDeviceInfo[]> GetMbrDeviceListAsync();

    Task<SourceListInfo> GetSourceListAsync();

    Task SetAntennaModeAsync(AntennaMode antennaMode);

    Task SetMainTvChannelAsync(
      BaseChannelInfo channel,
      ChannelListType channelListType,
      int satelliteId);

    Task SetMainTvSourceAsync(BaseSourceInfo source);

    Task<BaseChannelInfo> GetChannelInformationAsync(
      int majorChannel,
      int minorChannel,
      int ptc);

    Task<AntennaMode> GetCurrentAntennaModeAsync();

    Task<ChannelType[]> GetChannelTypesAsync(AntennaMode antennaMode);

    Task<ProgramInfo> GetProgramInformationAsync(
      AntennaMode antennaMode,
      BaseChannelInfo channel,
      string startTime = "Current");

    Task StopRecordAsync(BaseChannelInfo channel);

    event EventHandler<EventArgs> PowerOff;

    event EventHandler<EventArgs> NetworkChanged;

    event EventHandler<EventArgs> ChannelChanged;

    event EventHandler<EventArgs> ChannelListChanged;

    event EventHandler<EventArgs> ChannelListTypeChanged;

    event EventHandler<EventArgs> SourceListChanged;

    event EventHandler<EventArgs> SourceDisconnected;

    event EventHandler<EventArgs> MbrDeviceListChanged;

    event EventHandler<EventArgs> PriorityDisconnect;
  }
}
