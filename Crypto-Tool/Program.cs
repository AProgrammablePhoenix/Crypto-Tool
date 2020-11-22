using System;

namespace Crypto_Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] commands = new string[] { "encrypt [-s] [-o]", "decrypt [-s] [-o]", "file_checksum" };

            string _base = "\nCrypto-Tool~$";
            string input;

            while (true)
            {
                Console.Write(_base);
                input = Console.ReadLine();

                if (input == "encrypt")
                {
                    Console.Write("File to encrypt : ");
                    string file = Console.ReadLine();

                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Console.Write("Output file name : ");
                    string output = Console.ReadLine();

                    Crypto_Security.EncryptFile(file, passwd, 1, output);
                }
                else if (input == "encrypt -s")
                {
                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Console.Write("Extension of output files : ");
                    string ext = Console.ReadLine();

                    Crypto_Security.EncryptFolder(Environment.CurrentDirectory, passwd, 1, ext);
                }
                else if (input == "encrypt -o")
                {
                    Console.Write("File to encrypt : ");
                    string file = Console.ReadLine();

                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Crypto_Security.EncryptFile(file, passwd, 0);
                }
                else if (input == "encrypt -s -o" || input == "encrypt -o -s")
                {
                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Crypto_Security.EncryptFolder(Environment.CurrentDirectory, passwd, 0);
                }
                else if (input == "decrypt")
                {
                    Console.Write("File to decrypt : ");
                    string file = Console.ReadLine();

                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Console.Write("Output file name : ");
                    string output = Console.ReadLine();

                    Crypto_Security.DecryptFile(file, passwd, 1, output);
                }
                else if (input == "decrypt -s")
                {
                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Console.Write("Extension of output files : ");
                    string ext = Console.ReadLine();

                    Crypto_Security.DecryptFolder(Environment.CurrentDirectory, passwd, 1, ext);
                }
                else if (input == "decrypt -o")
                {
                    Console.Write("File to decrypt : ");
                    string file = Console.ReadLine();

                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Crypto_Security.EncryptFile(file, passwd, 0);
                }
                else if (input == "decrypt -s -o" || input == "decrypt -o -s")
                {
                    Console.Write("Password : ");
                    string passwd = Console.ReadLine();

                    Crypto_Security.EncryptFile(Environment.CurrentDirectory, passwd, 0);
                }
                else if (input == "file_checksum")
                {
                    Console.Write("File to compute checksum print : ");
                    string file = Console.ReadLine();

                    Crypto_Security.ComputeFilePrint(file);
                    Console.WriteLine("File checksum in output file : " + file + ".print");
                }
                else if (input == "help")
                {
                    foreach (string s in commands)
                    {
                        Console.WriteLine(s);
                    }
                }
                else if (input == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }
        }
    }
}
