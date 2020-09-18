// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.Channel
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System.ComponentModel;

namespace SmartView2.Core
{
  public class Channel : INotifyPropertyChanged
  {
    private string name = string.Empty;
    private ChannelType type;
    private int ptc;
    private int majorNumber;
    private int minorNumber;
    private string displayNumber = string.Empty;
    private string programTitle = string.Empty;
    private int programNumber;

    public string Name
    {
      get => this.name;
      set
      {
        if (!(this.name != value))
          return;
        this.name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public ChannelType Type
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

    public int PTC
    {
      get => this.ptc;
      set
      {
        if (this.ptc == value)
          return;
        this.ptc = value;
        this.OnPropertyChanged(nameof (PTC));
      }
    }

    public int MajorNumber
    {
      get => this.majorNumber;
      set
      {
        if (this.majorNumber == value)
          return;
        this.majorNumber = value;
        this.OnPropertyChanged(nameof (MajorNumber));
      }
    }

    public int MinorNumber
    {
      get => this.minorNumber;
      set
      {
        if (this.minorNumber == value)
          return;
        this.minorNumber = value;
        this.OnPropertyChanged(nameof (MinorNumber));
      }
    }

    public string DisplayNumber
    {
      get => this.displayNumber;
      set
      {
        if (!(this.displayNumber != value))
          return;
        this.displayNumber = value;
        this.OnPropertyChanged(nameof (DisplayNumber));
      }
    }

    public string ProgramTitle
    {
      get => string.IsNullOrEmpty(this.programTitle) ? "No Program Title" : this.programTitle;
      set
      {
        if (!(this.programTitle != value))
          return;
        this.programTitle = value;
        this.OnPropertyChanged(nameof (ProgramTitle));
      }
    }

    public int ProgramNumber
    {
      get => this.programNumber;
      set
      {
        if (this.programNumber == value)
          return;
        this.programNumber = value;
        this.OnPropertyChanged(nameof (ProgramNumber));
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
