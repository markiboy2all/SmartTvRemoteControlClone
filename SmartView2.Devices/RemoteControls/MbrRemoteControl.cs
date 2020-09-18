// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.RemoteControls.MbrRemoteControl
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using SmartView2.Core;
using SmartView2.Core.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartView2.Devices.RemoteControls
{
  public abstract class MbrRemoteControl : BaseRemoteControl
  {
    private IKeySender mbrKeySender;
    private MbrDevice device;

    public MbrRemoteControl(IKeySender tvKeySender, IKeySender mbrKeySender, MbrDevice device)
      : base(tvKeySender)
    {
      if (tvKeySender == null)
        throw new ArgumentNullException(nameof (tvKeySender));
      if (mbrKeySender == null)
        throw new ArgumentNullException(nameof (mbrKeySender));
      if (device == null)
        throw new ArgumentNullException(nameof (device));
      this.mbrKeySender = mbrKeySender;
      this.device = device;
    }

    protected override ICommand CreateCommand(Func<object> keyCodeProvider)
    {
      if (keyCodeProvider == null)
        return base.CreateCommand(keyCodeProvider);
      object obj = keyCodeProvider();
      if (!(obj is MbrKeyCode))
        return base.CreateCommand(keyCodeProvider);
      string xmlRequest = string.Format("<SendMBRIRKey><ActivityIndex>{0}</ActivityIndex><MBRDevice>{1}</MBRDevice><MBRIRKey>{2}</MBRIRKey></SendMBRIRKey>", (object) this.device.ActivityIndex, (object) this.device.DeviceType.ToString(), (object) ("0x" + ((int) obj).ToString("X2")));
      return (ICommand) new AsyncCommand((Func<object, Task>) (arg => this.SendKeyAsync(this.mbrKeySender, (object) xmlRequest)));
    }
  }
}
