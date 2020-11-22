using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Crypto_Tool
{
    public sealed class Crypto_Security
    {
        private static string AESEncrypter(string file, string passwd)
        {
            string content;
            try
            {
                content = File.ReadAllText(file);
            }
            catch
            {
                Console.WriteLine("Unknown file.");
                return null;
            }

            byte[] bStr = Encoding.UTF8.GetBytes(content);
            byte[] key = GenerateAlgotihmInputs(passwd)[0];
            byte[] iv = GenerateAlgotihmInputs(passwd)[1];

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform aesEncrypter = aes.CreateEncryptor(key, iv);
            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, aesEncrypter, CryptoStreamMode.Write);
            cs.Write(bStr, 0, bStr.Length);
            cs.FlushFinalBlock();

            byte[] CipherBytes = ms.ToArray();

            ms.Close();
            cs.Close();

            return Convert.ToBase64String(CipherBytes);
        }
        private static string AESDecrypter(string file, string passwd)
        {
            string content;
            try
            {
                content = File.ReadAllText(file);
            }
            catch
            {
                Console.WriteLine("This file doesn't exists.");
                return null;
            }

            byte[] cipheredData = Convert.FromBase64String(content);
            byte[] key = GenerateAlgotihmInputs(passwd)[0];
            byte[] iv = GenerateAlgotihmInputs(passwd)[1];

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform aesDecryptor = aes.CreateDecryptor(key, iv);
            MemoryStream ms = new MemoryStream(cipheredData);
            CryptoStream cs = new CryptoStream(ms, aesDecryptor, CryptoStreamMode.Read);

            byte[] bStr = new byte[cipheredData.Length];

            int decryptedByteCount = cs.Read(bStr, 0, bStr.Length);

            ms.Close();
            cs.Close();

            return Encoding.UTF8.GetString(bStr, 0, decryptedByteCount);
        }
        private static List<byte[]> GenerateAlgotihmInputs(string password)
        {

            byte[] key;
            byte[] iv;

            List<byte[]> result = new List<byte[]>();

            byte[] salt = Encoding.UTF8.GetBytes(password);
            salt = lib.elongateByteArray(salt, 512);

            Rfc2898DeriveBytes rfcDb = new Rfc2898DeriveBytes(password, salt);

            key = rfcDb.GetBytes(16);
            iv = rfcDb.GetBytes(16);

            result.Add(key);
            result.Add(iv);

            return result;
        }

        public static void EncryptFile(string file, string passwd, ushort mode = 0, string outputFile = "UseIfOnlyModeIsSetTo1")
        {
            string ciphered = AESEncrypter(file, passwd);
            if (ciphered == null)
            {
                return;
            }

            if (mode == 0)
            {
                File.WriteAllText(file, ciphered);
                return;
            }
            else if (mode == 1)
            {
                File.WriteAllText(outputFile, ciphered);
                return;
            }
            else
            {
                Console.WriteLine("Bad input was given : mode must be 0 or 1, not " + mode);
                return;
            }
        }
        public static void DecryptFile(string file, string passwd, ushort mode = 0, string outputFile = "UseIfOnlyModeIsSetTo1")
        {
            string plain = AESDecrypter(file, passwd);
            if (plain == null)
            {
                return;
            }

            if (mode == 0)
            {
                File.WriteAllText(file, plain);
                return;
            }
            else if (mode == 1)
            {
                File.WriteAllText(outputFile, plain);
                return;
            }
            else
            {
                Console.WriteLine("Bad input was given : mode must be 0 or 1, not " + mode);
                return;
            }
        }

        public static void EncryptFolder(string folderpath, string passwd, ushort mode = 0, string outFileExt = ".crypted")
        {
            FileAttributes attr = File.GetAttributes(folderpath);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("Specified path is not a folder path.");
                return;
            }

            DirectoryInfo di = new DirectoryInfo(folderpath);
            foreach (var file in di.GetFiles())
            {
                EncryptFile(file.FullName, passwd, mode, (file.FullName + outFileExt));
            }
        }
        public static void DecryptFolder(string folderpath, string passwd, ushort mode = 0, string outFileExt = ".plain")
        {
            FileAttributes attr = File.GetAttributes(folderpath);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("Specified path is not a folder path.");
                return;
            }

            DirectoryInfo di = new DirectoryInfo(folderpath);
            foreach (var file in di.GetFiles())
            {
                DecryptFile(file.FullName, passwd, mode, (file.FullName + outFileExt));
            }
        }

        public static void ComputeFilePrint(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("This file doesn't exists.");
                return;
            }
            FileInfo attr = new FileInfo(file);

            string file_content = File.ReadAllText(file);
            string file_name = attr.FullName;
            string file_ext = attr.Extension;
            string file_creation_time = attr.CreationTime.ToString("[MM/dd/yyyy at [HH:mm:ss]");
            string file_last_access = attr.LastAccessTimeUtc.ToString("[MM/dd/yyyy at [HH:mm:ss]");
            string file_last_write = attr.LastWriteTimeUtc.ToString("[MM/dd/yyyy at [HH:mm:ss]");
            string file_readonly = attr.IsReadOnly.ToString();
            string file_length = attr.Length.ToString();

            string formated_attributes = $"{file_content}||{file_name}||{file_ext}//{file_creation_time}//{file_last_access}" +
                $"//{file_last_write}##{file_readonly}##{file_length}";

            byte[] encoded_data = Encoding.UTF8.GetBytes(formated_attributes);

            SHA512 sha512 = SHA512.Create();

            byte[] file_print = sha512.ComputeHash(encoded_data);

            File.WriteAllBytes(file + ".print", file_print);
        }
    }
}
