//// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

//using System;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;

//namespace UtilitiesExtensions.Utility
//{
//    public class Encryption
//    {
//        private const string Salt = "woVyVdq95N2YbEpx";

//        private const int Keysize = 256;

//        public string EncryptString(string plainText, string passPhrase)
//        {
//            byte[] bytes = Encoding.UTF8.GetBytes("woVyVdq95N2YbEpx");
//            byte[] bytes2 = Encoding.UTF8.GetBytes(plainText);
//            byte[] bytes3 = new Rfc2898DeriveBytes(passPhrase, Encoding.ASCII.GetBytes("woVyVdq95N2YbEpx")).GetBytes(32);
//            ICryptoTransform transform = new RijndaelManaged
//            {
//                Mode = CipherMode.CBC
//            }.CreateEncryptor(bytes3, bytes);
//            using (MemoryStream memoryStream = new MemoryStream())
//            {
//                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
//                {
//                    cryptoStream.Write(bytes2, 0, bytes2.Length);
//                    cryptoStream.FlushFinalBlock();
//                    byte[] inArray = memoryStream.ToArray();
//                    cryptoStream.Close();
//                    return Convert.ToBase64String(inArray);
//                }
//            }
//        }

//        public string DecryptString(string cipherText, string passPhrase)
//        {
//            byte[] bytes = Encoding.ASCII.GetBytes("woVyVdq95N2YbEpx");
//            byte[] array = Convert.FromBase64String(cipherText);
//            byte[] bytes2 = new Rfc2898DeriveBytes(passPhrase, Encoding.ASCII.GetBytes("woVyVdq95N2YbEpx")).GetBytes(32);
//            ICryptoTransform transform = new RijndaelManaged
//            {
//                Mode = CipherMode.CBC
//            }.CreateDecryptor(bytes2, bytes);
//            using (MemoryStream stream = new MemoryStream(array))
//            {
//                using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
//                {
//                    byte[] array2 = new byte[array.Length];
//                    int count = cryptoStream.Read(array2, 0, array2.Length);
//                    return Encoding.UTF8.GetString(array2, 0, count);
//                }
//            }
//        }
//    }
//}
