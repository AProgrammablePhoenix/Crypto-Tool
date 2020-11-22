using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto_Tool
{
    public sealed class lib
    {
        public static byte[] elongateByteArray(byte[] input, int desiredLength)
        {
            if (input.Length >= desiredLength)
            {
                return input;
            }
            List<byte> buffer = arrayToList<byte>(input);

            while (buffer.Count < desiredLength)
            {
                List<byte> temp = buffer;
                buffer = new List<byte>();
                buffer.Add(0);
                for (int i = 0; i < temp.Count; i++)
                {
                    buffer.Add(temp[i]);
                }
            }

            return buffer.ToArray();
        }
        public static List<T> arrayToList<T>(T[] array)
        {
            List<T> buffer = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                buffer.Add(array[i]);
            }
            return buffer;
        }
        public static bool compareStrings(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static string fillString(string input, char c, int desiredLength)
        {
            while (input.Length < desiredLength)
            {
                input += c;
            }
            return input;
        }
        public static T[] extractSubArray<T>(T[] input, int start, int end)
        {
            T[] temp = new T[end - start];
            for (int i = start, j = 0; i < end && j < temp.Length; i++,j++)
            {
                temp[j] = input[i];
            }
            return temp;
        }
    }
}
