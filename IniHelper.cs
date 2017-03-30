using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Ini
{
    #region +class CKey

    public class CKey
    {
        string section = string.Empty;
        string key = string.Empty;
        string value = string.Empty;

        public string Section { 
            get { return this.section; }
            set { this.section = value; }
        }
        public string Key { 
            get { return this.key; }
            set { this.key = value; }
        }
        public string Value { 
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return this.value;
        }

        public int ToInt()
        {
            if (this.value.Length == 0)
                this.value = "0";
            return int.Parse(this.value);
        }

        public CKey(string section, string key, string value)
        {
            this.section = section;
            this.key = key;
            this.value = value;
        }

        public void Clear()
        {
            value = string.Empty;
        }

    }

    #endregion

    #region +class CSection

    public class CSection
    {
        string section = string.Empty;

        public Dictionary<string, CKey> value = new Dictionary<string, CKey>();

        public CSection(string section, Dictionary<string, CKey> value)
        {
            this.section = section;
            this.value = value;
        }

        public void Clear()
        {
            value = new Dictionary<string,CKey>();
        }

        public CKey Key(string key)
        {
            if (value.ContainsKey(key.ToLower()))
                return value[key.ToLower()];

            else return new CKey(string.Empty, string.Empty, string.Empty);
        }

        public void Add(string key, CKey dic)
        {
            value.Add(key.ToLower(), dic);
        }
    }

    #endregion

    #region +class IniHelper

    public class IniHelper
    {
        Dictionary<string, CSection> value = new Dictionary<string, CSection>();

        string __IniPath = string.Empty;


        public IniHelper(string iniPath)
        {
            Initialize(iniPath);
        }

        void Initialize(string iniPath)
        {
            __IniPath = iniPath;

            //Read();
        }

        public CSection Section(string section)
        {
            return value[section.ToLower()];
        }

        public void Add(string key, CSection dic)
        {
            value.Add(key.ToLower(), dic);
        }

        public void Read()
        {
            Dictionary<string, CSection> listSection = new Dictionary<string, CSection>();

            List<string> slist = IniAPI.GetValueAll(__IniPath);

            foreach (var s in slist)
            {
                Dictionary<string, CKey> listKey = new Dictionary<string, CKey>();

                List<string> klist = IniAPI.GetValueAll(__IniPath, s);

                foreach (var k in klist)
                {
                    string v = IniAPI.GetValueAll(__IniPath, s, k)[0];

                    CKey key = new CKey(s,k,v);

                    listKey.Add(k.ToLower(), key);
                }

                CSection section = new CSection(s, listKey);

                listSection.Add(s.ToLower(), section);
            }

            value = listSection;

            return;
        }

        public void Write()
        {
            foreach (var s in value)
            {
                foreach (var k in s.Value.value)
                {
                    IniAPI.SetValue(__IniPath, k.Value.Section, k.Value.Key, k.Value.Value);
                }
            }
            return;
        }


    }

    #endregion
}
