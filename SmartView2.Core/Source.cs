// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Source
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System.ComponentModel;

namespace SmartView2.Core
{
  public class Source : INotifyPropertyChanged
  {
    private int id;
    private SourceType type;
    private Channel currentChannel;
    private ChannelList channelList;
    private IRemoteControl remoteControl;
    private MbrDevice mbrDevice;

    public int Id
    {
      get => this.id;
      set
      {
        if (this.id == value)
          return;
        this.id = value;
        this.OnPropertyChanged(nameof (Id));
      }
    }

    public SourceType Type
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

    public Channel CurrentChannel
    {
      get => this.currentChannel;
      set
      {
        if (this.currentChannel == value)
          return;
        this.currentChannel = value;
        this.OnPropertyChanged(nameof (CurrentChannel));
      }
    }

    public ChannelList ChannelList
    {
      get => this.channelList;
      set
      {
        if (this.channelList == value)
          return;
        this.channelList = value;
        this.OnPropertyChanged(nameof (ChannelList));
      }
    }

    public IRemoteControl RemoteControl
    {
      get => this.remoteControl;
      set
      {
        if (this.remoteControl == value)
          return;
        this.remoteControl = value;
        this.OnPropertyChanged(nameof (RemoteControl));
      }
    }

    public MbrDevice MbrDevice
    {
      get => this.mbrDevice;
      set
      {
        if (this.mbrDevice == value)
          return;
        this.mbrDevice = value;
        this.OnPropertyChanged(nameof (MbrDevice));
      }
    }

    public bool IsMbr => this.mbrDevice != null;

    public bool IsValid => !this.Type.ToString().Contains("HDMI");

    public string Title
    {
      get
      {
        if (this.IsMbr)
          return this.mbrDevice.DeviceType.ToString() + "/" + this.mbrDevice.BrandName;
        if (this.Type == SourceType.UNKNOWN_SOURCE)
          return "Unknown source";
        if (!this.type.ToString().Contains(SourceType.SCART.ToString()))
          return this.type.ToString().Replace('_', '/');
        return string.Format("Ext{0}.", (object) this.type.ToString().Remove(0, 5));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
