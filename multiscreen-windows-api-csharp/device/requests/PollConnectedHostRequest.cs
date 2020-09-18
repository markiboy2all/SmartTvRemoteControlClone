using System;
using System.Threading;
using com.samsung.multiscreen.channel;
using com.samsung.multiscreen.channel.connection;
using com.samsung.multiscreen.channel.info;
using com.samsung.multiscreen.device;
using com.samsung.multiscreen.device.requests;
using com.samsung.multiscreen.util;

namespace multiscreen_windows_api_csharp.device.requests
{
	internal class PollConnectedHostRequest : DeviceAsyncResult<ChannelInfo>
	{
		private int delayMilliseconds = 2000;

		private Uri restEndpoint;

		private string channelId;

		private DeviceAsyncResult<Channel> callback;

		private int attempts;

		public PollConnectedHostRequest(Uri endpoint, string channelId, int attempts, int delayMS, DeviceAsyncResult<Channel> callback)
		{
			restEndpoint = endpoint;
			this.channelId = channelId;
			this.attempts = attempts;
			delayMilliseconds = delayMS;
			this.callback = callback;
		}

		public void OnResult(ChannelInfo result)
		{
			if (result != null && result.HostConnected)
			{
				Logger.Debug("PollConnectedHostRequest[onResult] -- got a connected host, returning channel!");
				Channel result2 = new Channel(result, new ConnectionFactory());
				callback.OnResult(result2);
				return;
			}
			attempts--;
			Logger.Debug("PollConnectedHostRequest[onResult] -- attempts remaining: " + attempts);
			if (attempts > 0)
			{
				Logger.Debug("PollConnectedHostRequest[onResult] -- scheduling poll in " + delayMilliseconds + " seconds");
				Thread.Sleep(delayMilliseconds);
				ThreadPool.QueueUserWorkItem(PerformRequest, null);
			}
			else
			{
				callback.OnError(new DeviceError(-1L, "Timeout: channel not ready"));
			}
		}

		public void OnError(DeviceError error)
		{
			attempts--;
			Logger.Debug("PollConnectedHostRequest[onError] -- attempts remaining: " + attempts);
			if (attempts > 0)
			{
				Logger.Debug("PollConnectedHostRequest[onError] -- scheduling poll in " + delayMilliseconds + " ms");
				Thread.Sleep(delayMilliseconds);
				ThreadPool.QueueUserWorkItem(PerformRequest, null);
			}
			else
			{
				Logger.Debug("PollConnectedHostRequest[onError] -- last attempt failed, returning error: " + error);
				callback.OnError(error);
			}
		}

		public void run()
		{
			ThreadPool.QueueUserWorkItem(PerformRequest, null);
		}

		protected void PerformRequest(object data)
		{
			GetChannelInfoRequest getChannelInfoRequest = new GetChannelInfoRequest(restEndpoint, channelId, this);
			getChannelInfoRequest.run();
		}
	}

}

