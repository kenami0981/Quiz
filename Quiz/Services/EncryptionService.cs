using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    internal class EncryptionService
    {
        private readonly byte[] _key;
        public EncryptionService(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                Console.WriteLine($"Generated key: {BitConverter.ToString(_key).Replace("-", "")}");

            }

        }
        public void EncryptAndSaveToFile(string text, string filePath)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.GenerateIV();  
                byte[] iv = aes.IV;

                using (var fsOutput = new FileStream(filePath, FileMode.Create))
                {
                    fsOutput.Write(iv, 0, iv.Length);

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        using (var csEncrypt = new CryptoStream(fsOutput, encryptor, CryptoStreamMode.Write))
                        using (var writer = new StreamWriter(csEncrypt))
                        {
                            writer.Write(text);
                        }
                    }
                }
            }
        }



        public void Decrypt(string inputFilePath, string outputFilePath)
        {
            using (var fsInput = new FileStream(inputFilePath, FileMode.Open))
            {
                byte[] iv = new byte[16];
                fsInput.Read(iv, 0, iv.Length); 

                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        using (var csDecrypt = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                        using (var srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {
                            string decryptedText = srDecrypt.ReadToEnd();
                            File.WriteAllText(outputFilePath, decryptedText, Encoding.UTF8);
                        }
                    }
                }
            }
        }


    }


}

