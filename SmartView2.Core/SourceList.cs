// Decompiled with JetBrains decompiler
// Type: SmartView2.Core.SourceList
// Assembly: SmartView2.Core, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: CE8D5DC3-9665-4838-83F8-C641D1D5BA98
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartView2.Core
{
  public class SourceList : ObservableCollection<Source>
  {
    public SourceList()
    {
    }

    public SourceList(IEnumerable<Source> sourceList)
      : base(sourceList)
    {
    }

    public Source GetSourceById(int Id) => this.Items.FirstOrDefault<Source>((Func<Source, bool>) (s => s.Id == Id));
  }
}
