// Decompiled with JetBrains decompiler
// Type: SmartView2.Devices.SecondTv.NoSecurityProvider
// Assembly: SmartView2.Devices, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: DD366AE7-DCF5-40D7-997B-FABEBA295200
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\SmartView2.Devices.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SmartView2.Devices.SecondTv
{
	public class NoSecurityProvider : ISecondTvSecurityProvider
	{
		public dynamic EncryptData(dynamic data)
		{
			return data;
		}

		public dynamic DecryptData(dynamic data)
		{
			return data.ToString();
		}
	}
}
