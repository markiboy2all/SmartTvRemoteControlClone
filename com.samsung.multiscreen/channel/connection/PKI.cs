// Decompiled with JetBrains decompiler
// Type: com.samsung.multiscreen.channel.connection.PKI
// Assembly: multiscreen-windows-api-csharp, Version=1.0.4.21, Culture=neutral, PublicKeyToken=null
// MVID: A26F56CC-21BF-4CAA-9AB1-271EE8423FF3
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\multiscreen-windows-api-csharp.dll

using com.samsung.multiscreen.util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace com.samsung.multiscreen.channel.connection
{
  public class PKI
  {
    private const int PEM_LINE_LENGTH = 64;
    private const string BEGIN = "-----BEGIN ";
    private const string END = "-----END ";
    private static int CHUNK_SIZE = 117;
    private static SecureRandom random = new SecureRandom();
    protected internal static readonly char[] hexArray = "0123456789ABCDEF".ToCharArray();

    public static AsymmetricCipherKeyPair generateKeyPair()
    {
      AsymmetricCipherKeyPair asymmetricCipherKeyPair = (AsymmetricCipherKeyPair) null;
      try
      {
        RsaKeyPairGenerator keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
        asymmetricCipherKeyPair = keyPairGenerator.GenerateKeyPair();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      return asymmetricCipherKeyPair;
    }

    private static bool ByteArrayEquals(byte[] a, byte[] b)
    {
      if (a == null || b == null || a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if ((int) a[index] != (int) b[index])
          return false;
      }
      return true;
    }

    private static RSACryptoServiceProvider DecodeX509PublicKey(
      byte[] x509key)
    {
      byte[] b = new byte[9]
      {
        (byte) 42,
        (byte) 134,
        (byte) 72,
        (byte) 134,
        (byte) 247,
        (byte) 13,
        (byte) 1,
        (byte) 1,
        (byte) 1
      };
      BinaryReader reader = new BinaryReader((Stream) new MemoryStream(x509key));
      if (reader.ReadByte() != (byte) 48)
        return (RSACryptoServiceProvider) null;
      PKI.ReadASNLength(reader);
      if (reader.ReadByte() != (byte) 48)
        return (RSACryptoServiceProvider) null;
      int num1 = PKI.ReadASNLength(reader);
      if (reader.ReadByte() == (byte) 6)
      {
        byte[] numArray = new byte[PKI.ReadASNLength(reader)];
        reader.Read(numArray, 0, numArray.Length);
        if (!PKI.ByteArrayEquals(numArray, b))
          return (RSACryptoServiceProvider) null;
        int count = num1 - 2 - numArray.Length;
        reader.ReadBytes(count);
      }
      if (reader.ReadByte() == (byte) 3)
      {
        PKI.ReadASNLength(reader);
        int num2 = (int) reader.ReadByte();
        if (reader.ReadByte() == (byte) 48)
        {
          PKI.ReadASNLength(reader);
          if (reader.ReadByte() == (byte) 2)
          {
            byte[] buffer1 = new byte[PKI.ReadASNLength(reader)];
            reader.Read(buffer1, 0, buffer1.Length);
            if (buffer1[0] == (byte) 0)
            {
              byte[] numArray = new byte[buffer1.Length - 1];
              Array.Copy((Array) buffer1, 1, (Array) numArray, 0, buffer1.Length - 1);
              buffer1 = numArray;
            }
            if (reader.ReadByte() == (byte) 2)
            {
              byte[] buffer2 = new byte[PKI.ReadASNLength(reader)];
              reader.Read(buffer2, 0, buffer2.Length);
              RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
              cryptoServiceProvider.ImportParameters(new RSAParameters()
              {
                Modulus = buffer1,
                Exponent = buffer2
              });
              return cryptoServiceProvider;
            }
          }
        }
      }
      return (RSACryptoServiceProvider) null;
    }

    private static int ReadASNLength(BinaryReader reader)
    {
      int num = (int) reader.ReadByte();
      if ((num & 128) == 128)
      {
        int count = num & 15;
        byte[] buffer = new byte[4];
        reader.Read(buffer, 4 - count, count);
        Array.Reverse((Array) buffer);
        num = BitConverter.ToInt32(buffer, 0);
      }
      return num;
    }

    public static string keyAsPEM(AsymmetricKeyParameter key, string type)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("-----BEGIN ").Append(type).Append("-----\r\n");
      byte[] bytes = Encoding.UTF8.GetBytes(Convert.ToBase64String(!key.IsPrivate ? SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key).GetEncoded() : PrivateKeyInfoFactory.CreatePrivateKeyInfo(key).GetEncoded()));
      char[] chArray = new char[64];
      for (int index1 = 0; index1 < bytes.Length; index1 += chArray.Length)
      {
        for (int index2 = 0; index2 != chArray.Length && index1 + index2 < bytes.Length; ++index2)
          chArray[index2] = (char) bytes[index1 + index2];
        stringBuilder.Append(chArray);
        stringBuilder.AppendLine();
      }
      stringBuilder.Append("-----END ").Append(type).Append("-----");
      return stringBuilder.ToString();
    }

    public static AsymmetricKeyParameter pemAsKey(string pem, bool isPublic)
    {
      if (string.IsNullOrEmpty(pem))
        return (AsymmetricKeyParameter) null;
      AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter) null;
      try
      {
        StreamReader streamReader = new StreamReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(pem)));
        string str1 = streamReader.ReadLine();
        while (str1 != null && !str1.StartsWith("-----BEGIN "))
          str1 = streamReader.ReadLine();
        StringBuilder stringBuilder = new StringBuilder();
        if (str1 != null)
        {
          string str2;
          while ((str2 = streamReader.ReadLine()) != null && !str2.Contains("-----END "))
            stringBuilder.Append(str2.Trim());
          Console.WriteLine("read PEM key info: " + stringBuilder.ToString());
          byte[] numArray = Convert.FromBase64String(stringBuilder.ToString());
          asymmetricKeyParameter = !isPublic ? PrivateKeyFactory.CreateKey(numArray) : PublicKeyFactory.CreateKey(numArray);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      return asymmetricKeyParameter;
    }

    public static string encryptStringAsHex(string str, AsymmetricKeyParameter key)
    {
      byte[] bytes = PKI.encryptString(str, key);
      return bytes == null ? (string) null : PKI.bytesToHex(bytes);
    }

    public static byte[] encryptString(string str, AsymmetricKeyParameter key)
    {
      if (key != null)
      {
        if (str != null)
        {
          try
          {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            List<byte> byteList = new List<byte>();
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(true, (ICipherParameters) key);
            int length = bytes.Length % PKI.CHUNK_SIZE;
            int inOff;
            for (inOff = 0; inOff + PKI.CHUNK_SIZE <= bytes.Length; inOff += PKI.CHUNK_SIZE)
            {
              byte[] numArray = cipher.DoFinal(bytes, inOff, PKI.CHUNK_SIZE);
              byteList.AddRange((IEnumerable<byte>) numArray);
            }
            if (length > 0)
            {
              byte[] numArray = cipher.DoFinal(bytes, inOff, length);
              byteList.AddRange((IEnumerable<byte>) numArray);
            }
            return byteList.ToArray();
          }
          catch (IOException ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          catch (InvalidKeyException ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          return (byte[]) null;
        }
      }
      return (byte[]) null;
    }

    public static string decryptHexString(string str, AsymmetricKeyParameter key)
    {
      byte[] bytes = PKI.decryptString(PKI.hexToBytes(str), key);
      try
      {
        return bytes != null ? Encoding.UTF8.GetString(bytes) : (string) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      return (string) null;
    }

    public static byte[] decryptString(byte[] data, AsymmetricKeyParameter key)
    {
      if (key != null)
      {
        if (data != null)
        {
          try
          {
            List<byte> byteList = new List<byte>();
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, (ICipherParameters) key);
            int length1 = 128;
            Console.WriteLine("\n\n\t\t OutputSize = " + (object) length1);
            int length2 = data.Length % length1;
            int inOff;
            for (inOff = 0; inOff + length1 <= data.Length; inOff += length1)
            {
              byte[] numArray = cipher.DoFinal(data, inOff, length1);
              byteList.AddRange((IEnumerable<byte>) numArray);
            }
            if (length2 > 0)
            {
              byte[] numArray = cipher.DoFinal(data, inOff, length2);
              byteList.AddRange((IEnumerable<byte>) numArray);
            }
            return byteList.ToArray();
          }
          catch (InvalidKeyException ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          catch (IOException ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
            Console.Write(ex.StackTrace);
          }
          return (byte[]) null;
        }
      }
      return (byte[]) null;
    }

    private static byte[] DoFinal(
      byte[] DataToEncrypt,
      AsymmetricKeyParameter Key,
      bool IsEncryption)
    {
      try
      {
        IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
        cipher.Init(IsEncryption, (ICipherParameters) Key);
        return cipher.DoFinal(DataToEncrypt);
      }
      catch (Exception ex)
      {
        Logger.Debug("Exception while encrypt/decript data: " + ex.ToString());
        Logger.Trace("Exception while encrypt/decript data: " + ex.ToString());
      }
      return (byte[]) null;
    }

    public static string bytesToHex(byte[] bytes)
    {
      char[] chArray = new char[bytes.Length * 2];
      for (int index = 0; index < bytes.Length; ++index)
      {
        int num = (int) bytes[index] & (int) byte.MaxValue;
        chArray[index * 2] = PKI.hexArray[(int) ((uint) num >> 4)];
        chArray[index * 2 + 1] = PKI.hexArray[num & 15];
      }
      return new string(chArray);
    }

    public static byte[] hexToBytes(string s)
    {
      int length = s.Length;
      byte[] numArray = new byte[length / 2];
      for (int index = 0; index < length; index += 2)
        numArray[index / 2] = (byte) ((Convert.ToInt32(s[index].ToString(), 16) << 4) + Convert.ToInt32(s[index + 1].ToString(), 16));
      return numArray;
    }
  }
}
