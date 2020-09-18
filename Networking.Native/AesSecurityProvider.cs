// Decompiled with JetBrains decompiler
// Type: Networking.Native.AesSecurityProvider
// Assembly: Networking.Native, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=null
// MVID: 38FC6B2B-E053-44FF-9024-85D24680777E
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\Networking.Native.dll

using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SmartView2.Core;
using SmartView2.Devices.SecondTv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Networking.Native
{
	public class AesSecurityProvider : ISecondTvSecurityProvider
	{
		private readonly int sessionId;

		private readonly byte[] key;

		private readonly byte[] iv;

		public AesSecurityProvider(byte[] key, int sessionId)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.sessionId = sessionId;
			this.key = key;
			iv = new byte[this.key.Length];
		}

		public dynamic EncryptData(dynamic data)
		{
			string text = JsonConvert.SerializeObject(data).ToString();
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Logger.Instance.LogMessageFormat("JSON Data: {0}", text);
			byte[] values = Encrypt(bytes);
			string text2 = string.Format("[{0}]", string.Join(",", values));
			Logger.Instance.LogMessageFormat("Encrypted Bytes: {0}", text2);
			return new
			{
				body = text2,
				Session_Id = sessionId
			};
		}

		private byte[] Encrypt(byte[] source)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			ICryptoTransform transform = aes.CreateEncryptor(key, iv);
			using MemoryStream memoryStream = new MemoryStream();
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				cryptoStream.Write(source, 0, source.Length);
				cryptoStream.FlushFinalBlock();
			}
			return memoryStream.ToArray();
		}

		public dynamic DecryptData(dynamic data)
		{
			string text = data.ToString();
			Regex regex = new Regex("^(\\d+,?)+$");
			if (!regex.IsMatch(text))
			{
				return text;
			}
			Logger.Instance.LogMessageFormat("Decrypting Bytes: {0}", text);
			string[] source = text.Split(new char[1]
			{
			','
			}, StringSplitOptions.RemoveEmptyEntries);
			byte[] source2 = source.Select((string arg) => byte.Parse(arg)).ToArray();
			byte[] bytes = Decrypt(source2);
			string @string = Encoding.UTF8.GetString(bytes);
			Logger.Instance.LogMessageFormat("Decrypted JSON Data: {0}", @string);
			return @string;
		}

		private byte[] Decrypt(byte[] source)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.ECB;
			ICryptoTransform transform = aes.CreateDecryptor(key, iv);
			using MemoryStream memoryStream = new MemoryStream();
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				cryptoStream.Write(source, 0, source.Length);
				cryptoStream.FlushFinalBlock();
			}
			return memoryStream.ToArray();
		}
	}
}
