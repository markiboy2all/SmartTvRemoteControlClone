// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.IMultiScreen
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using MediaLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SmartView2.Core
{
  public interface IMultiScreen : IDisposable, INotifyPropertyChanged
  {
    Content CurrentMediaContent { get; }

    IEnumerable<Content> MediaQueue { get; }

    int MediaQueueCount { get; }

    IMediaTimeInfo CurrentMediaTimeInfo { get; set; }

    LoadState LoadState { get; }

    MediaState MediaState { get; }

    string QueueSpeed { get; }

    string QueueEffect { get; }

    Task InitializeAsync(string targetAddress, string serverAddress, IHttpServer httpServer);

    void Close();

    void PushMediaToTv(Content file);

    void PushMediaToTvQueue(Content file);

    void PushMediaToTvQueue(IEnumerable<Content> files);

    void MediaPlay();

    void MediaPause();

    void MediaStop();

    void QueuePlay();

    void QueuePause();

    void QueueStop();

    void QueueNext();

    void MoveQueueItem(string mediaId, int newIndex);

    void DeleteQueueItem(string mediaId);

    void UpdateMediaQueue();

    void SetCurrentMediaTimePosition(double currentTime);

    void SetSlideShowSettings(SlideShowSettingsModel settings);

    event EventHandler MultiscreenDisconnected;

    event EventHandler MultiscreenQueueEnded;

    event EventHandler MultiscreenQueueUpdated;

    event EventHandler MultiscreenStartFailed;

    event EventHandler PushToTvEnded;

    event EventHandler<MediaQueueEventArgs> PushToTvQueueEnded;

    event EventHandler MultiscreenCurrentMediaContentUpdated;

    event EventHandler MultiScreenContentBroken;

    event EventHandler MultiScreenContentFailed;

    event EventHandler MultiScreenContentNotSupported;
  }
}
