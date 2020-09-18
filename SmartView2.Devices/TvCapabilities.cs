// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.TvCapabilities
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using System;
using System.Collections.Generic;

namespace SmartView2.Devices
{
  public sealed class TvCapabilities
  {
    private static readonly IDictionary<string, Action<TvCapabilities>> capabilitySetters = (IDictionary<string, Action<TvCapabilities>>) new Dictionary<string, Action<TvCapabilities>>()
    {
      {
        "AddSchedule",
        (Action<TvCapabilities>) (cap => cap.AddScheduleAvailable = true)
      },
      {
        "ChangeSchedule",
        (Action<TvCapabilities>) (cap => cap.ChangeScheduleAvailable = true)
      },
      {
        "DeleteRecordedItem",
        (Action<TvCapabilities>) (cap => cap.DeleteRecordedItemAvailable = true)
      },
      {
        "DeleteSchedule",
        (Action<TvCapabilities>) (cap => cap.DeleteScheduleAvailable = true)
      },
      {
        "DestroyGroupOwner",
        (Action<TvCapabilities>) (cap => cap.DestroyGroupOwnerAvailable = true)
      },
      {
        "EnforceAKE",
        (Action<TvCapabilities>) (cap => cap.EnforceAkeAvailable = true)
      },
      {
        "GetACRCurrentChannelName",
        (Action<TvCapabilities>) (cap => cap.CurrentChannelNameAvailable = true)
      },
      {
        "GetACRCurrentProgramName",
        (Action<TvCapabilities>) (cap => cap.CurrentProgramNameAvailable = true)
      },
      {
        "GetACRMessage",
        (Action<TvCapabilities>) (cap => cap.MessageAvailable = true)
      },
      {
        "GetAPInformation",
        (Action<TvCapabilities>) (cap => cap.ApiInformationAvailable = true)
      },
      {
        "GetAllProgramInformationURL",
        (Action<TvCapabilities>) (cap => cap.AllProgramInformationAvailable = true)
      },
      {
        "GetBannerInformation",
        (Action<TvCapabilities>) (cap => cap.BannerInformationAvailable = true)
      },
      {
        "GetChannelListURL",
        (Action<TvCapabilities>) (cap => cap.ChannelListAvailable = true)
      },
      {
        "GetCurrentBrowserMode",
        (Action<TvCapabilities>) (cap => cap.BrowserModeAvailable = true)
      },
      {
        "GetCurrentBrowserURL",
        (Action<TvCapabilities>) (cap => cap.BrowserUrlAvailable = true)
      },
      {
        "GetCurrentExternalSource",
        (Action<TvCapabilities>) (cap => cap.CurrentExternalSourceAvailable = true)
      },
      {
        "GetCurrentMainTVChannel",
        (Action<TvCapabilities>) (cap => cap.CurrentChannelAvailable = true)
      },
      {
        "GetCurrentProgramInformationURL",
        (Action<TvCapabilities>) (cap => cap.AllProgramInformationAvailable = true)
      },
      {
        "GetDTVInformation",
        (Action<TvCapabilities>) (cap => cap.DtvInformationAvailable = true)
      },
      {
        "GetDetailProgramInformation",
        (Action<TvCapabilities>) (cap => cap.DetailedProgramInformationAvailable = true)
      },
      {
        "GetFilteredProgramURL",
        (Action<TvCapabilities>) (cap => cap.FilteredProgramUrlAvailable = true)
      },
      {
        "GetMBRDeviceList",
        (Action<TvCapabilities>) (cap => cap.MbrDeviceListAvailable = true)
      },
      {
        "GetMBRDongleStatus",
        (Action<TvCapabilities>) (cap => cap.MbrDongleStatusAvailable = true)
      },
      {
        "GetRecordChannel",
        (Action<TvCapabilities>) (cap => cap.RecordChannelAvailable = true)
      },
      {
        "GetScheduleListURL",
        (Action<TvCapabilities>) (cap => cap.ScheduleListAvailable = true)
      },
      {
        "GetSourceList",
        (Action<TvCapabilities>) (cap => cap.SourceListAvailable = true)
      },
      {
        "PlayRecordedItem",
        (Action<TvCapabilities>) (cap => cap.PlayRecordedItemAvailable = true)
      },
      {
        "RunBrowser",
        (Action<TvCapabilities>) (cap => cap.StartBrowserAvailable = true)
      },
      {
        "SendBrowserCommand",
        (Action<TvCapabilities>) (cap => cap.BrowserCommandsAvailable = true)
      },
      {
        "SendMBRIRKey",
        (Action<TvCapabilities>) (cap => cap.MbrKeysAvailable = true)
      },
      {
        "SetMainTVChannel",
        (Action<TvCapabilities>) (cap => cap.SetChannelAvailable = true)
      },
      {
        "SetMainTVSource",
        (Action<TvCapabilities>) (cap => cap.SetSourceAvailable = true)
      },
      {
        "SetRecordDuration",
        (Action<TvCapabilities>) (cap => cap.SetRecordDurationAvailable = true)
      },
      {
        "StopBrowser",
        (Action<TvCapabilities>) (cap => cap.StopBrowserAvailable = true)
      },
      {
        "StopRecord",
        (Action<TvCapabilities>) (cap => cap.StopRecordAvailable = true)
      },
      {
        "StopView",
        (Action<TvCapabilities>) (cap => cap.StopViewAvailable = true)
      },
      {
        "StartCloneView",
        (Action<TvCapabilities>) (cap => cap.CloneViewAvailable = true)
      },
      {
        "StartExtSourceView",
        (Action<TvCapabilities>) (cap => cap.ExternalSourceViewAvailable = true)
      },
      {
        "StartSecondTVView",
        (Action<TvCapabilities>) (cap => cap.SecondTvViewAvailable = true)
      }
    };

    public bool AddScheduleAvailable { get; private set; }

    public bool ChangeScheduleAvailable { get; private set; }

    public bool DeleteScheduleAvailable { get; private set; }

    public bool DeleteRecordedItemAvailable { get; private set; }

    public bool DestroyGroupOwnerAvailable { get; private set; }

    public bool EnforceAkeAvailable { get; private set; }

    public bool CurrentChannelNameAvailable { get; private set; }

    public bool CurrentProgramNameAvailable { get; private set; }

    public bool MessageAvailable { get; private set; }

    public bool ApiInformationAvailable { get; private set; }

    public bool AllProgramInformationAvailable { get; private set; }

    public bool BannerInformationAvailable { get; private set; }

    public bool ChannelListAvailable { get; private set; }

    public bool BrowserModeAvailable { get; private set; }

    public bool BrowserUrlAvailable { get; private set; }

    public bool CurrentExternalSourceAvailable { get; private set; }

    public bool CurrentChannelAvailable { get; private set; }

    public bool CurrentProgramInformationAvailable { get; private set; }

    public bool DtvInformationAvailable { get; private set; }

    public bool DetailedProgramInformationAvailable { get; private set; }

    public bool FilteredProgramUrlAvailable { get; private set; }

    public bool MbrDeviceListAvailable { get; private set; }

    public bool MbrDongleStatusAvailable { get; private set; }

    public bool RecordChannelAvailable { get; private set; }

    public bool ScheduleListAvailable { get; private set; }

    public bool SourceListAvailable { get; private set; }

    public bool PlayRecordedItemAvailable { get; private set; }

    public bool StartBrowserAvailable { get; private set; }

    public bool BrowserCommandsAvailable { get; private set; }

    public bool MbrKeysAvailable { get; private set; }

    public bool SetAntennaModeAvailable { get; private set; }

    public bool SetChannelAvailable { get; private set; }

    public bool SetSourceAvailable { get; private set; }

    public bool SetRecordDurationAvailable { get; private set; }

    public bool CloneViewAvailable { get; private set; }

    public bool SecondTvViewAvailable { get; private set; }

    public bool ExternalSourceViewAvailable { get; private set; }

    public bool StopBrowserAvailable { get; private set; }

    public bool StopRecordAvailable { get; private set; }

    public bool StopViewAvailable { get; private set; }

    public static TvCapabilities FromString(string source)
    {
      TvCapabilities tvCapabilities = new TvCapabilities();
      string str = source;
      char[] separator = new char[1]{ ',' };
      foreach (string key in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        Action<TvCapabilities> action = (Action<TvCapabilities>) null;
        if (TvCapabilities.capabilitySetters.TryGetValue(key, out action))
          action(tvCapabilities);
      }
      return tvCapabilities;
    }
  }
}
