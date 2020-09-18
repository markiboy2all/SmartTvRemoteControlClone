// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.MediaQueueEventArgs
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;

namespace SmartView2.Core
{
  public class MediaQueueEventArgs : EventArgs
  {
    private int addedFiles;
    private int repeatedFiles;

    public MediaQueueEventArgs(int addedFiles, int repeatedFiles)
    {
      this.addedFiles = addedFiles;
      this.repeatedFiles = repeatedFiles;
    }

    public int AddedFiles => this.addedFiles;

    public int RepeatedFiles => this.repeatedFiles;
  }
}
