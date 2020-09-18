// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.MbrDevice
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System.ComponentModel;

namespace SmartView2.Core
{
  public class MbrDevice : INotifyPropertyChanged
  {
    private int activityIndex;
    private DeviceType deviceType;
    private string brandName;

    public int ActivityIndex
    {
      get => this.activityIndex;
      set
      {
        if (this.activityIndex == value)
          return;
        this.activityIndex = value;
        this.OnPropertyChanged(nameof (ActivityIndex));
      }
    }

    public DeviceType DeviceType
    {
      get => this.deviceType;
      set
      {
        if (this.deviceType == value)
          return;
        this.deviceType = value;
        this.OnPropertyChanged(nameof (DeviceType));
      }
    }

    public string BrandName
    {
      get => this.brandName;
      set
      {
        if (!(this.brandName != value))
          return;
        this.brandName = value;
        this.OnPropertyChanged(nameof (BrandName));
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
