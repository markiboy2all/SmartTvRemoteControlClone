// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.ChannelList
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SmartView2.Core
{
  public class ChannelList : ObservableCollection<Channel>, INotifyPropertyChanged
  {
    private int satelliteId;
    private ChannelListType type = ChannelListType.All;

    public int SatelliteId
    {
      get => this.satelliteId;
      set
      {
        if (this.satelliteId == value)
          return;
        this.satelliteId = value;
        this.OnPropertyChanged(nameof (SatelliteId));
      }
    }

    public ChannelListType Type
    {
      get => this.type;
      set
      {
        if (this.type == value)
          return;
        this.type = value;
        this.OnPropertyChanged(nameof (Type));
      }
    }

    public ChannelList()
    {
    }

    public ChannelList(IEnumerable<Channel> collection)
      : base(collection)
    {
    }

    public Channel GetChannel(int majorNumber, int minorNumber, int ptc, ChannelType type) => this.Items.Where<Channel>((Func<Channel, bool>) (arg => arg.MajorNumber == majorNumber && arg.MinorNumber == minorNumber && arg.PTC == ptc && arg.Type == type)).FirstOrDefault<Channel>();

    public Channel GetNextChannel(Channel currentChannel)
    {
      int num = this.IndexOf(currentChannel);
      if (num == -1)
        throw new InvalidOperationException("Specified Channel is not in List.");
      int index = num + 1;
      if (index >= this.Count)
        index = 0;
      return this[index];
    }

    public Channel GetPreviousChannel(Channel currentChannel)
    {
      int num = this.IndexOf(currentChannel);
      if (num == -1)
        throw new InvalidOperationException("Specified Channel is not in List.");
      int index = num - 1;
      if (index < 0)
        index = this.Count - 1;
      return this[index];
    }

    private void OnPropertyChanged(string propertyName) => this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
  }
}
