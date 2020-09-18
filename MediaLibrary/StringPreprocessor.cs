// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.StringPreprocessor
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;

namespace MediaLibrary.DataModels
{
  public static class StringPreprocessor
  {
    private const int ZERO_WIDTH_NO_BREAK_SPACE_CODE = 65279;
    private static readonly char ZERO_WIDTH_NO_BREAK_SPACE = Convert.ToChar(65279);

    public static string RemoveInvisibleChars(string input) => string.IsNullOrEmpty(input) ? input : input.Replace(string.Concat((object) StringPreprocessor.ZERO_WIDTH_NO_BREAK_SPACE), "").Trim();

    public static string GetFirstLetter(string input) => string.IsNullOrEmpty(input) ? string.Empty : StringPreprocessor.RemoveInvisibleChars(input).Substring(0, 1).ToUpper();
  }
}
