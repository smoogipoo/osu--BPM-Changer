using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace smgiFuncs
{
    #region "Updater"
    public class Updater
    {
        public Updater(Settings settings)
        {
            foreach (string f in System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.*", System.IO.SearchOption.TopDirectoryOnly))
            {
                if (f.Contains("."))
                {
                    if (f.Substring(f.LastIndexOf(".", StringComparison.Ordinal)) == ".old")
                    {
                        System.IO.File.Delete(f);
                    }
                }

            }
            Dictionary<string, string> versions = new Dictionary<string, string>();
            foreach (sString f in System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.*", System.IO.SearchOption.TopDirectoryOnly))
            {
                settings.AddSetting("v_" + f.SubString(f.LastIndexOf("\\") + 1), "1.0.0", false);
            }
            foreach (string key in settings.GetKeys())
            {
                if (key.StartsWith("v_"))
                {
                    string fname = key.Substring(2);
                    if (versions.ContainsKey(fname) == false)
                    {
                        versions.Add(fname, settings.GetSetting(key));
                    }
                }
            }
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                if (System.IO.File.Exists(Environment.CurrentDirectory + "\\files.updt"))
                {
                    System.IO.File.Delete(Environment.CurrentDirectory + "\\files.updt");
                }
                wc.DownloadFile("http://repo.smgi.me/" + Assembly.GetExecutingAssembly().GetName().Name + "/files.updt", Environment.CurrentDirectory + "\\files.updt");
                using (System.IO.StreamReader sR = new System.IO.StreamReader(Environment.CurrentDirectory + "\\files.updt"))
                {
                    while (sR.Peek() != -1)
                    {
                        //Example /asdf.exe:\asdf.exe:1.0.1
                        sString line = sR.ReadLine();
                        if (line != null)
                        {
                            string onlinepath = line.SubString(0, line.nthDexOf(":", 0));
                            sString localpath = line.SubString(line.nthDexOf(":", 0) + 1, line.nthDexOf(":", 1));
                            string version = line.SubString(line.nthDexOf(":", 1) + 1);
                            string name = localpath.SubString(localpath.LastIndexOf("\\") + 1);
                            if (versions.ContainsKey(name) == false)
                            {
                                wc.DownloadFile("http://repo.smgi.me/" + Assembly.GetExecutingAssembly().GetName().Name + onlinepath, Environment.CurrentDirectory + localpath);
                                settings.AddSetting("v_" + name, version);
                            }
                            else
                            {
                                if (version != versions[name])
                                {
                                    if (System.IO.File.Exists(Environment.CurrentDirectory + localpath + ".old"))
                                    {
                                        System.IO.File.Delete(Environment.CurrentDirectory + localpath + ".old");
                                    }
                                    System.IO.File.Move(Environment.CurrentDirectory + localpath, Environment.CurrentDirectory + localpath + ".old");
                                    wc.DownloadFile("http://repo.smgi.me/" + Assembly.GetExecutingAssembly().GetName().Name + onlinepath, Environment.CurrentDirectory + localpath);
                                    settings.AddSetting("v_" + name, version);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            if (System.IO.File.Exists(Environment.CurrentDirectory + "\\files.updt"))
            {
                System.IO.File.Delete(Environment.CurrentDirectory + "\\files.updt");
            }
            settings.Save();
        }
    }
    #endregion
    #region "Application Settings"
    public class Settings
    {
        readonly Dictionary<string, string> s_settings = new Dictionary<string, string>();
        System.IO.FileStream s_file;

        public Settings()
        {
            LoadSettings();
        }
        public void LoadSettings()
        {
            s_file = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\settings.dat", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            using (System.IO.StreamReader sR = new System.IO.StreamReader(s_file))
            {
                while (sR.Peek() != -1)
                {
                    string s = sR.ReadLine();
                    if (s != null)
                        s_settings.Add(s.Substring(0, s.IndexOf(":", StringComparison.Ordinal)), s.Substring(s.IndexOf(":", StringComparison.Ordinal) + 1));
                }
            }
            s_file = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\settings.dat", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
        }
        public bool ContainsSetting(string name)
        {
            return s_settings.ContainsKey(name);
        }

        public List<string> GetKeys()
        {
            return s_settings.Keys.ToList();

        }
        public void AddSetting(string name, string value, bool overwrite = true)
        {
            if ((s_settings.ContainsKey(name)) & (overwrite))
            {
                s_settings[name] = value;
            }
            else if (s_settings.ContainsKey(name) == false)
            {
                s_settings.Add(name, value);
            }
        }
        public string GetSetting(string name)
        {
            return s_settings.ContainsKey(name) ? s_settings[name] : "";
        }

        public void DeleteSetting(string name)
        {
            if (s_settings.ContainsKey(name))
            {
                s_settings.Remove(name);
            }
        }
        public void Save()
        {
            string constructedString = s_settings.Aggregate("", (str, di) => str + (di.Key + ":" + di.Value + Environment.NewLine));
            if (constructedString != "")
            {
                constructedString = constructedString.Substring(0, constructedString.LastIndexOf(Environment.NewLine, StringComparison.Ordinal));
            }
            s_file.SetLength(constructedString.Length);
            s_file.Position = 0;
            byte[] bytesToWrite = System.Text.Encoding.ASCII.GetBytes(constructedString);
            s_file.Write(bytesToWrite, 0, bytesToWrite.Length);
            s_file.Flush();
        }
    }
    #endregion
    #region "String Datatype"
    public class sString
    {
        internal readonly string _data;
        public sString(String s)
        {
            _data = s;
        }
        public static implicit operator sString(String s)
        {
            return new sString(s);
        }
        public override string ToString()
        {
            return _data;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as sString;
            if (other == null)
            {
                return false;
            }
            return other._data == _data;
        }
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }
        public int CountOf(string splitter)
        {
            int indx = -1;
            int count = 0;
            indx = _data.IndexOf(splitter, indx + 1, StringComparison.Ordinal);
            if (indx != -1)
            {
                while (indx != -1)
                {
                    count += 1;
                    indx = _data.IndexOf(splitter, indx + 1, StringComparison.Ordinal);
                }
            }
            return count;
        }
        public int nthDexOf(string splitter, int count)
        {
            int camnt = -1;
            int indx = _data.IndexOf(splitter, StringComparison.Ordinal);
            camnt += 1;
            while (!((camnt == count) | (indx == -1)))
            {
                indx = _data.IndexOf(splitter, indx + 1, StringComparison.Ordinal);
                if (indx == -1)
                {
                    return indx;
                }
                camnt += 1;
            }
            return indx;
        }
        public string SubString(int startindex, int endindex = -1)
        {
            if (startindex < 0)
            {
                throw new Exception("The startindex value of '" + startindex + "' is less than zero.", new Exception("String: " + _data));
            }
            if (endindex < -1)
            {
                throw new Exception("The endindex value of '" + endindex + "' is less than -1.", new Exception("String: " + _data));
            }
            if ((endindex < startindex) & (endindex != -1))
            {
                throw new Exception("The endindex value of '" + endindex + "' is less than the startindex value of '" + startindex + "'.", new Exception("String: " + _data));
            }
            if (endindex == -1)
            {
                return _data.Substring(startindex, _data.Length - startindex);
            }
            if (endindex > _data.Length)
            {
                throw new Exception("The endindex value of '" + endindex + "' exceeds the string length.", new Exception("String: " + _data, new Exception("Length: " + _data.Length)));
            }
            return _data.Substring(startindex, endindex - startindex);
        }
        public int LastIndexOf(string splitter)
        {
            return _data.LastIndexOf(splitter, StringComparison.Ordinal);
        }
    }
    #endregion
}