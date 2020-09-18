// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.ContentCompare
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System.Collections.Generic;

namespace MediaLibrary.DataModels
{
  public class ContentCompare : EqualityComparer<Content>
  {
    public override bool Equals(Content x, Content y) => x.ID == y.ID;

    public override int GetHashCode(Content obj) => (obj.ID.GetHashCode() ^ obj.ID.GetHashCode()).GetHashCode();
  }
}
