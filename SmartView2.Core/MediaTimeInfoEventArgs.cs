// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.MediaTimeInfoEventArgs
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;

namespace SmartView2.Core
{
  public class MediaTimeInfoEventArgs : EventArgs
  {
    private IMediaTimeInfo mediaTimeInfo;

    public IMediaTimeInfo MediaTimeInfo => this.mediaTimeInfo;

    public MediaTimeInfoEventArgs(IMediaTimeInfo mediaTimeInfo) => this.mediaTimeInfo = mediaTimeInfo;
  }
}
