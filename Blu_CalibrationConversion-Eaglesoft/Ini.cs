using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Blu_CalibrationConversion
{
    
    public class Ini
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        const int MAX_SIZE = 1024;

                

        public static string ReadValue(string section, string key, string filePath, string defaultValue = null)
        {
            var result = new StringBuilder(MAX_SIZE);
            if (GetPrivateProfileString(section, key, defaultValue ?? string.Empty, result, (uint)result.Capacity, filePath) > 0)
                return result.ToString();

            return defaultValue;
        }

        public static bool WriteValue(string section, string key, string value, string filePath)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }
    }
}
