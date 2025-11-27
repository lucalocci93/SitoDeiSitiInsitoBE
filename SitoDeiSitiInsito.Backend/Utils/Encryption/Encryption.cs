using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SitoDeiSiti.Utils.Encryption
{
    public class Encryption
    {
        public Encryption()
        {
        }

        public static string EncryptData(string plainText)
        {
            //byte[] Key = Convert.FromBase64String("Jutsuka");

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));

            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                //aesAlg.Key = Key;
                aesAlg.GenerateIV();
                //aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptData(byte[] cipherText)
        {
            byte[] Key = Convert.FromBase64String("Jutsuka");

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            //if (Key == null || Key.Length <= 0)
            //    throw new ArgumentNullException(nameof(Key));

            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                //aesAlg.Key = Key;
                aesAlg.GenerateIV();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }

        //    public static byte[] EncryptData(string data)
        //    {
        //        using (Aes aes = Aes.Create())
        //        {
        //            //aes.Key = Convert.FromBase64String("Jutsuka");
        //            aes.GenerateIV();
        //            byte[] iv = aes.IV;

        //            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
        //            {
        //                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        //                byte[] encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);

        //                // Combine IV and encrypted data
        //                byte[] result = new byte[iv.Length + encryptedData.Length];
        //                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        //                Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);

        //                return result;
        //            }
        //        }
        //    }

        //    public static string DecryptData(byte[] encryptedData)
        //    {
        //        using (Aes aes = Aes.Create())
        //        {
        //            //aes.Key = Convert.FromBase64String("Jutsuka");

        //            // Extract IV from encrypted data
        //            byte[] iv = new byte[aes.BlockSize / 8];
        //            byte[] cipherText = new byte[encryptedData.Length - iv.Length];
        //            Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
        //            Buffer.BlockCopy(encryptedData, iv.Length, cipherText, 0, cipherText.Length);

        //            aes.IV = iv;

        //            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        //            {
        //                byte[] decryptedData = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        //                return Encoding.UTF8.GetString(decryptedData);
        //            }
        //        }
        //    }

        //    public static T Encrypt<T>(T obj) where T : class
        //    {
        //        Type type = typeof(T);
        //        PropertyInfo[] properties = type.GetProperties();
        //        foreach (PropertyInfo property in properties)
        //        {
        //            if (property.PropertyType == typeof(string))
        //            {
        //                string value = (string)property.GetValue(obj);
        //                if (!string.IsNullOrEmpty(value))
        //                {
        //                    byte[] encryptedValue = EncryptData(value);
        //                    property.SetValue(obj, Convert.ToBase64String(encryptedValue));
        //                }
        //            }
        //        }
        //        return obj;
        //    }

        //    public static T Decrypt<T>(T obj) where T : class
        //    {
        //        Type type = typeof(T);
        //        PropertyInfo[] properties = type.GetProperties();
        //        foreach (PropertyInfo property in properties)
        //        {
        //            if (property.PropertyType == typeof(string))
        //            {
        //                string value = (string)property.GetValue(obj);
        //                if (!string.IsNullOrEmpty(value))
        //                {
        //                    byte[] encryptedValue = Convert.FromBase64String(value);
        //                    string decryptedValue = DecryptData(encryptedValue);
        //                    property.SetValue(obj, decryptedValue);
        //                }
        //            }
        //        }
        //        return obj;
        //    }
        //}
    }
