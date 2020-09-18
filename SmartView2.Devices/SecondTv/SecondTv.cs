using SmartView2.Core;
using SmartView2.Devices.DataContracts;
using System;
using System.Threading.Tasks;

namespace SmartView2.Devices.SecondTv
{
	public class SecondTv : ISecondTv, IDisposable
	{
		private ISecondTvTransport transport;

		private Uri localEndpoint;

		public event EventHandler<EventArgs> PowerOff;

		public event EventHandler<EventArgs> NetworkChanged;

		public event EventHandler<EventArgs> ChannelChanged;

		public event EventHandler<EventArgs> ChannelListChanged;

		public event EventHandler<EventArgs> ChannelListTypeChanged;

		public event EventHandler<EventArgs> SourceListChanged;

		public event EventHandler<EventArgs> SourceDisconnected;

		public event EventHandler<EventArgs> MbrDeviceListChanged;

		public event EventHandler<EventArgs> PriorityDisconnect;

		public SecondTv(ISecondTvTransport transport, Uri localEndpoint)
		{
			if (transport == null)
			{
				throw new ArgumentNullException("transport");
			}
			if (localEndpoint == null)
			{
				throw new ArgumentNullException("localEndpoint");
			}
			this.transport = transport;
			this.transport.NotificationReceived += transport_NotificationReceived;
			this.localEndpoint = localEndpoint;
		}

		private void transport_NotificationReceived(object sender, SecondTvNotificationEventArgs e)
		{
			if (e.PluginName == "SecondTV")
			{
				switch (e.NotificationText)
				{
					case "EMP_TV_POWER_OFF":
						OnPowerOff(this, EventArgs.Empty);
						break;
					case "EMP_NETWORK_CHANGED":
						OnNetworkChanged(this, EventArgs.Empty);
						break;
					case "EMP_TV_CHANNEL_CHANGED":
						OnChannelChanged(this, EventArgs.Empty);
						break;
					case "EMP_CHANNEL_LIST_CHANGED":
						OnChannelListChanged(this, EventArgs.Empty);
						break;
					case "EMP_SOURCE_LIST_CHANGED":
						OnSourceListChanged(this, EventArgs.Empty);
						break;
					case "EMP_DISCONNECT_SOURCE":
						OnSourceDisconnected(this, EventArgs.Empty);
						break;
					case "EMP_CHANNEL_LIST_TYPE_CHANGED":
						OnChannelListTypeChanged(this, EventArgs.Empty);
						break;
					case "EMP_MBR_DEVICE_LIST_CHANGED":
						OnMbrDeviceListChanged(this, EventArgs.Empty);
						break;
					case "EMP_VIEW_DISCONNECT_PRIORITY":
					case "EMP_DISCONNECT_PRIORITY":
						OnPriorityDisconnect(this, EventArgs.Empty);
						break;
					case "EMP_FAVORITE_NAME_CHANGED":
						throw new NotImplementedException();
				}
			}
		}

		public async Task<string> StartCloneViewAsync()
		{
			string response = await InvokeActionAsync("StartCloneView", "<StartCloneView><ForcedFlag>Normal</ForcedFlag><DRMType>PrivateTZ</DRMType></StartCloneView>");
			StartCloneViewResponse parsed = (StartCloneViewResponse)SecondTvResponse.Parse(response);
			return parsed.CloneViewUrl;
		}

		public async Task<string> StartSecondTvViewAsync(BaseChannelInfo channel, ChannelListType channelListType, int satelliteId, ForcedFlag forcedFlag, DrmType drmType)
		{
			object[] array = new object[1];
			int num = (int)channelListType;
			array[0] = num.ToString("X2");
			string hexType = string.Format("0x{0}", array);
			string channelData = $"<?xml version=\"1.0\" encoding=\"utf-8\"><Channel><ChType>{channel.Type}</ChType><MajorCh>{channel.MajorChannel}</MajorCh><MinorCh>{channel.MinorChannel}</MinorCh><PTC>{channel.PTC}</PTC><ProgNum>{channel.ProgramNumber}</ProgNum></Channel>".ToQuote();
			string argument = $"<StartSecondTVView><ChannelListType>{hexType}</ChannelListType><SatelliteID>{satelliteId}</SatelliteID><Channel>{channelData}</Channel><ForcedFlag>{forcedFlag}</ForcedFlag><DRMType>{drmType}</DRMType></StartSecondTVView>";
			string response = await InvokeActionAsync("StartSecondTVView", argument);
			StartSecondTvViewResponse parsed = (StartSecondTvViewResponse)SecondTvResponse.Parse(response);
			return parsed.SecondTvUrl;
		}

		public async Task<string> StartExternalSourceViewAsync(BaseSourceInfo source, ForcedFlag forcedFlag, DrmType drmType)
		{
			StartExternalSourceResponse parsed = (StartExternalSourceResponse)SecondTvResponse.Parse(await InvokeActionAsync("StartExtSourceView", $"<StartExtSourceView><Source>{source.TypeXml}</Source><ID>{source.Id}</ID><ForcedFlag>{forcedFlag}</ForcedFlag><DRMType>{drmType}</DRMType></StartExtSourceView>"));
			return parsed.ExternalSourceViewUrl;
		}

		public async Task StopViewAsync(string videoUrl)
		{
			if (string.IsNullOrEmpty(videoUrl))
			{
				throw new ArgumentNullException("videoUrl");
			}
			_ = (SetResponse)SecondTvResponse.Parse(await InvokeActionAsync("StopView", $"<StopView><ViewURL>{videoUrl}</ViewURL></StopView>"));
		}

		public async Task<TvCapabilities> GetAvailableActionsAsync()
		{
			string response = await InvokeActionAsync("GetAvailableActions", "<GetAvailableActions></GetAvailableActions>");
			GetAvailableActionsResponse parsed = (GetAvailableActionsResponse)SecondTvResponse.Parse(response);
			return TvCapabilities.FromString(parsed.AvailableActions);
		}

		public async Task<string> GetTargetLocationAsync()
		{
			dynamic response = await transport.SendRequestAsync(new
			{
				method = "POST",
				type = "EMP",
				api = "Execute",
				body = new
				{
					api = "GetTargetLocation",
					plugin = "TV",
					version = "1.000"
				}
			});
			return response.ToString();
		}

		public async Task<BaseChannelInfo> GetCurrentChannelAsync()
		{
			string response = await InvokeActionAsync("GetCurrentMainTVChannel", "<GetCurrentMainTVChannel></GetCurrentMainTVChannel>");
			GetCurrentMainTvChannelResponse parsed = (GetCurrentMainTvChannelResponse)SecondTvResponse.Parse(response);
			return BaseChannelInfo.Parse(parsed.CurrentChannel);
		}

		public async Task<BaseChannelInfo> GetRecordedChannelAsync()
		{
			string response = await InvokeActionAsync("GetRecordChannel", "<GetRecordChannel></GetRecordChannel>");
			GetRecordChannelResponse parsed = (GetRecordChannelResponse)SecondTvResponse.Parse(response);
			return BaseChannelInfo.Parse(parsed.RecordedChannel);
		}

		public async Task<BannerInfo> GetBannerInformationAsync()
		{
			string response = await InvokeActionAsync("GetBannerInformation", "<GetBannerInformation></GetBannerInformation>");
			GetBannerInformationResponse parsed = (GetBannerInformationResponse)SecondTvResponse.Parse(response);
			return BannerInfo.Parse(parsed.BannerInformation);
		}

		public async Task<ChannelListInfo> GetChannelListAsync()
		{
			string response = await InvokeActionAsync("GetChannelList", "<GetChannelList></GetChannelList>");
			GetChannelListResponse parsed = (GetChannelListResponse)SecondTvResponse.Parse(response);
			try
			{
				ChannelListInfo channelListInfo = new ChannelListInfo();
				channelListInfo.ChannelList = parsed.ChannelList;
				channelListInfo.ChannelListType = (ChannelListType)Convert.ToInt32(parsed.ChannelListType, 16);
				channelListInfo.SatelliteId = parsed.SatelliteId;
				return channelListInfo;
			}
			catch (FormatException ex)
			{
				Logger.Instance.LogMessageFormat("Invalid convert to ChannelListType {0}", parsed.ChannelListType);
				throw ex;
			}
		}

		public async Task<BaseSourceInfo> GetCurrentSourceAsync()
		{
			string response = await InvokeActionAsync("GetCurrentExternalSource", "<GetCurrentExternalSource></GetCurrentExternalSource>");
			GetCurrentExternalSourceResponse parsed = (GetCurrentExternalSourceResponse)SecondTvResponse.Parse(response);
			return new BaseSourceInfo
			{
				TypeXml = parsed.CurrentExternalSource,
				Id = parsed.ID,
				MbrActivityIndex = parsed.CurrentMBRActivityIndex
			};
		}

		public async Task<DtvInfo> GetDtvInformationAsync()
		{
			string response = await InvokeActionAsync("GetDTVInformation", "<GetDTVInformation></GetDTVInformation>");
			GetDTVInformationResponse parsed = (GetDTVInformationResponse)SecondTvResponse.Parse(response);
			return DtvInfo.Parse(parsed.DTVInformation);
		}

		public async Task<MbrDeviceInfo[]> GetMbrDeviceListAsync()
		{
			string response = await InvokeActionAsync("GetMBRDeviceList", "<GetMBRDeviceList></GetMBRDeviceList>");
			GetMbrDeviceListResponse parsed = (GetMbrDeviceListResponse)SecondTvResponse.Parse(response);
			return MbrDeviceListInfo.Parse(parsed.MbrDeviceList).MbrList;
		}

		public async Task<SourceListInfo> GetSourceListAsync()
		{
			string response = await InvokeActionAsync("GetSourceList", "<GetSourceList></GetSourceList>");
			GetSourceListResponse parsed = (GetSourceListResponse)SecondTvResponse.Parse(response);
			return SourceListInfo.Parse(parsed.SourceList);
		}

		public async Task SetAntennaModeAsync(AntennaMode antennaMode)
		{
			_ = (SetResponse)SecondTvResponse.Parse(await InvokeActionAsync("SetAntennaMode", $"<SetAntennaMode><AntennaMode>{(int)antennaMode}</AntennaMode></SetAntennaMode>"));
		}

		public async Task SetMainTvChannelAsync(BaseChannelInfo channel, ChannelListType channelListType, int satelliteId)
		{
			string channelData = $"<?xml version=\"1.0\" encoding=\"utf-8\"><Channel><ChType>{channel.Type}</ChType><MajorCh>{channel.MajorChannel}</MajorCh><MinorCh>{channel.MinorChannel}</MinorCh><PTC>{channel.PTC}</PTC><ProgNum>{channel.ProgramNumber}</ProgNum></Channel>".ToQuote();
			object[] array = new object[1];
			int num = (int)channelListType;
			array[0] = num.ToString("X2");
			string hexType = string.Format("0x{0}", array);
			string argument = string.Format("<SetMainTVChannel><ChannelListType>{0}</ChannelListType><SatelliteId>{1}</SatelliteId><Channel>{2}</Channel></SetMainTVChannel>", new object[3]
			{
			hexType,
			satelliteId,
			channelData
			});
			string response = await InvokeActionAsync("SetMainTVChannel", argument);
			_ = (SetResponse)SecondTvResponse.Parse(response);
		}

		public async Task SetMainTvSourceAsync(BaseSourceInfo source)
		{
			_ = (SetResponse)SecondTvResponse.Parse(await InvokeActionAsync("SetMainTVSource", string.Format("<SetMainTVSource><Source>{0}</Source><ID>{1}</ID></SetMainTVSource>", new object[2]
			{
			source.TypeXml,
			source.Id
			})));
		}

		public async Task<BaseChannelInfo> GetChannelInformationAsync(int majorChannel, int minorChannel, int ptc)
		{
			GetChannelInformationResponse parsed = (GetChannelInformationResponse)SecondTvResponse.Parse(await InvokeActionAsync("GetChannelInformation", string.Format("<GetChannelInformation><MajorCh>{0}</MajorCh><MinorCh>{1}</MinorCh><PTC>{2}</PTC></GetChannelInformation>", new object[3]
			{
			majorChannel,
			minorChannel,
			ptc
			})));
			return BaseChannelInfo.Parse(parsed.ChannelInformation);
		}

		public async Task<AntennaMode> GetCurrentAntennaModeAsync()
		{
			string response = await InvokeActionAsync("GetCurrentAntennaMode", "<GetCurrentAntennaMode></GetCurrentAntennaMode>");
			GetCurrentAntennaModeResponse parsed = (GetCurrentAntennaModeResponse)SecondTvResponse.Parse(response);
			try
			{
				return (AntennaMode)Convert.ToInt32(parsed.AntennaMode, 16);
			}
			catch (FormatException ex)
			{
				Logger.Instance.LogMessageFormat("Invalid convert to AntennaMode: {0}", parsed.AntennaMode);
				throw ex;
			}
		}

		public async Task<ChannelType[]> GetChannelTypesAsync(AntennaMode antennaMode)
		{
			GetChannelTypeWithAntennaModeResponse parsed = (GetChannelTypeWithAntennaModeResponse)SecondTvResponse.Parse(await InvokeActionAsync("GetChannelTypeWithAntennaMode", $"<GetChannelTypeWithAntennaMode><AntennaMode>{(int)antennaMode}</AntennaMode></GetChannelTypeWithAntennaMode>"));
			return parsed.ChannelTypes;
		}

		public async Task<ProgramInfo> GetProgramInformationAsync(AntennaMode antennaMode, BaseChannelInfo channel, string startTime = "Current")
		{
			string channelData = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><Channel><ChType>{channel.Type}</ChType><MajorCh>{channel.MajorChannel}</MajorCh><MinorCh>{channel.MinorChannel}</MinorCh><PTC>{channel.PTC}</PTC><ProgNum>{channel.ProgramNumber}</ProgNum></Channel>".ToQuote();
			string argument = string.Format("<GetDetailProgramInformation><AntennaMode>{0}</AntennaMode><Channel>{1}</Channel><StartTime>{2}</StartTime></GetDetailProgramInformation>", new object[3]
			{
			(int)antennaMode,
			channelData,
			startTime
			});
			try
			{
				string response = await InvokeActionAsync("GetDetailProgramInformation", argument);
				GetDetailProgramInformationResponse parsed = (GetDetailProgramInformationResponse)SecondTvResponse.Parse(response);
				return ProgramInfo.Parse(parsed.DetailProgramInformation);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}

		public async Task StopRecordAsync(BaseChannelInfo channel)
		{
			string channelData = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><Channel><ChType>{channel.Type}</ChType><MajorCh>{channel.MajorChannel}</MajorCh><MinorCh>{channel.MinorChannel}</MinorCh><PTC>{channel.PTC}</PTC><ProgNum>{channel.ProgramNumber}</ProgNum></Channel>".ToQuote();
			string argument = $"<StopRecord><Channel>{channelData}</Channel></StopRecord>";
			string response = await InvokeActionAsync("StopRecord", argument);
			_ = (SetResponse)SecondTvResponse.Parse(response);
		}

		private async Task<string> InvokeActionAsync(string actionName, string parameter)
		{
			dynamic response = await transport.SendRequestAsync(new
			{
				method = "POST",
				body = new
				{
					api = "ExecuteSecondTVEMP",
					param1 = actionName,
					param2 = localEndpoint.Host,
					param3 = parameter,
					plugin = "SecondTV",
					version = "1.000"
				}
			});
			return response.ToString();
		}

		public void Dispose()
		{
			if (transport != null)
			{
				transport.NotificationReceived -= transport_NotificationReceived;
				transport.Dispose();
				transport = null;
			}
		}

		private void OnPowerOff(object sender, EventArgs e)
		{
			this.PowerOff?.Invoke(sender, e);
			Logger.Instance.LogMessage("PowerOff Event raised.");
		}

		private void OnNetworkChanged(object sender, EventArgs e)
		{
			this.NetworkChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("NetworkChanged Event raised.");
		}

		private void OnChannelChanged(object sender, EventArgs e)
		{
			this.ChannelChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("ChannelChanged Event raised.");
		}

		private void OnChannelListChanged(object sender, EventArgs e)
		{
			this.ChannelListChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("ChannelListChanged Event raised.");
		}

		private void OnChannelListTypeChanged(object sender, EventArgs e)
		{
			this.ChannelListTypeChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("ChannelListTypeChanged Event raised.");
		}

		private void OnSourceListChanged(object sender, EventArgs e)
		{
			this.SourceListChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("SourceListChanged Event raised.");
		}

		private void OnSourceDisconnected(object sender, EventArgs e)
		{
			this.SourceDisconnected?.Invoke(sender, e);
			Logger.Instance.LogMessage("SourceDisconnected Event raised.");
		}

		private void OnMbrDeviceListChanged(object sender, EventArgs e)
		{
			this.MbrDeviceListChanged?.Invoke(sender, e);
			Logger.Instance.LogMessage("DeviceListChanged Event raised.");
		}

		private void OnPriorityDisconnect(object sender, EventArgs e)
		{
			this.PriorityDisconnect?.Invoke(sender, e);
			Logger.Instance.LogMessage("PriorityDisconnect Event raised.");
		}
	}

}
