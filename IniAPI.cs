using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Ini
{
    public class IniAPI
    {
        const int __BUFF_SIZE = 32000;

        //[DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", 
        //    SetLastError = true, 
        //    CharSet = CharSet.Unicode, 
        //    ExactSpelling = true, 
        //    CallingConvention = CallingConvention.StdCall)]
        //private static extern int GetPrivateProfileString(string lpAppName, 
        //    string lpKeyName, 
        //    string lpDefault,
        //    string lpReturnString, 
        //    int nSize,
        //    string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW", 
            SetLastError = true, 
            CharSet = CharSet.Unicode, 
            ExactSpelling = true, 
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string def,
            string retVal,
            int size,
            string filePath);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW", 
            SetLastError = true, 
            CharSet = CharSet.Unicode, 
            ExactSpelling = true, 
            CallingConvention = CallingConvention.StdCall)]
        private static extern long WritePrivateProfileString(
            string section,
            string key,
            string val,
            string filePath);


        /// <summary>
        /// 값을 읽어 온다. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string GetValue(string path, string section, string key, string def = "")
        {
            string buff = new string(' ', __BUFF_SIZE);
            int i = GetPrivateProfileString(section, key, def, buff, __BUFF_SIZE, path);
            List<string> stuff = new List<string>(buff.ToString().Split('\0'));

            Debug.WriteLine("READ {0}\t{1}\t{2}\t{3}", path, section, key, stuff[0].ToString());

            return stuff[0].ToString();
        }

        /// <summary>
        /// 값을 쓴다
        /// </summary>
        /// <param name="path"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string SetValue(string path, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);

            Debug.WriteLine("WRITE {0}\t{1}\t{2}\t{3}", path, section, key, value);
            return value;
        }

        /// <summary>
        /// 값을 리스트로 읽어 온다
        /// </summary>
        /// <param name="path"></param>
        /// <param name="section">인자가 없으면, 섹션 리스트를 가져온다</param>
        /// <param name="key">인자가 없으면, 키 리스트를 가져온다</param>
        /// <returns></returns>
        public static List<string> GetValueAll(string path, string section = null, string key = null)
        {
            const string no_value_found = "no_value_found";

            string buff = new string(' ', __BUFF_SIZE);

            GetPrivateProfileString(section, key, no_value_found, buff, __BUFF_SIZE, path);

            List<string> stuff = new List<string>(buff.ToString().Split('\0'));

            //쓰레기값 제거
            var count = 0;
            var idx = stuff.Count - 1;
            for (; idx > 0; idx--)
                if (!stuff[idx].Trim().Equals(string.Empty))
                    break;
                else
                    count++;

            stuff.RemoveRange(idx + 1, count);

            foreach(var it in stuff)
                Debug.WriteLine("LIST {0}\t{1}\t{2}\t{3}", path, section, key, it);

            return stuff;
        }

    }
}
