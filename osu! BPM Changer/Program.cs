using System;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using BMAPI.v1;
using BMAPI.v1.Events;
using BMAPI.v1.HitObjects;
using smgiFuncs;

namespace osu__BPM_Changer
{
    class Program
    {
        private static readonly Settings settings = new Settings();
        private static bool updateExists;
        private static readonly Updater u = new Updater();
        private static Beatmap BM;
        private static string errorText = "";
        private static double bpmRatio;
        private static double oldBPM;
        private static string oldVersion;
        private static string oldCreator;
        private static bool saveAsMP3 = true;
        private static bool versionSet;

        [STAThread]
        static void Main()
        {
            Application.CurrentCulture = new CultureInfo("en-US", false);
            Thread updaterThread = new Thread(UpdaterStart);
            updaterThread.IsBackground = true;
            updaterThread.Start();

            Console.ForegroundColor = ConsoleColor.White;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Beatmap";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BM = new Beatmap(ofd.FileName);
                        oldVersion = BM.Version;

                        if (settings.ContainsSetting("customCreator"))
                        {
                            oldCreator = BM.Creator;
                            BM.Creator = settings.GetSetting("customCreator");
                        }
                        if (settings.ContainsSetting("customSaveAsMP3"))
                        {
                            saveAsMP3 = Convert.ToBoolean(Convert.ToInt32(settings.GetSetting("customSaveAsMP3")));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The beatmap could not be parsed. Please post the following error in the forums:\n" + e);
                        Console.ReadKey();
                        Application.Exit();
                    }
                    BeginGUI(0);
                    Console.ReadKey();
                }
            }
        }

        public static void UpdaterStart()
        {
            u.updateReady += UpdateCB;
            u.Start(settings);
        }

        public static void UpdateCB(object sender, EventArgs e)
        {
            updateExists = true;
            DisplayUpdateString();
        }

        public static void DisplayUpdateString()
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            int previousX = Console.CursorLeft;
            int previousY = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.Write("Update ready - restart application to apply.");
            Console.SetCursorPosition(previousX, previousY);
            Console.ForegroundColor = previousColor;  
        }

        public static void BeginGUI(int page)
        {
            while (true)
            {
                //Main GUI
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(settings.ContainsSetting("v_osu! BPM Changer.exe")? "osu! BPM Changer v" + settings.GetSetting("v_osu! BPM Changer.exe") : "osu! BPM Changer v1.0.0");

                if (errorText != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorText);
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    errorText = "";
                }
                Console.ForegroundColor = ConsoleColor.Green;          
                double minBPM = double.MaxValue, maxBPM = double.MinValue;
                foreach (TimingPoint tp in BM.TimingPoints.Where(tp => tp.InheritsBPM == false))
                {
                    if (60000/tp.BpmDelay < minBPM)
                        minBPM = 60000 / tp.BpmDelay;
                    if (60000 / tp.BpmDelay > maxBPM)
                        maxBPM = 60000 / tp.BpmDelay;
                }
                if (Math.Abs(oldBPM) <= 0)
                    oldBPM = minBPM;
                if (versionSet == false)
                    BM.Version = oldVersion + minBPM + "BPM";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Loaded beatmap " + BM.Source + (BM.Source != "" ? " (" + BM.Artist + ")" : BM.Artist) + " - " + BM.Title + " [" + oldVersion + "]\n");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("BPM: " + minBPM + (Math.Abs(minBPM - maxBPM) > 0 ? " - " + maxBPM : ""));
                Console.WriteLine("Version: [" + BM.Version + "]");
                Console.WriteLine("Creator: " + BM.Creator);
                Console.WriteLine("Song format: " + (saveAsMP3 ? "MP3" : "WAV"));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------------------------------------------------------");

                if (updateExists)
                    DisplayUpdateString();

                string input;

                switch (page)
                {
                    case -1:
                        using (OpenFileDialog ofd = new OpenFileDialog())
                        {
                            ofd.Title = "Select Beatmap";
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    BM = new Beatmap(ofd.FileName);
                                    oldVersion = BM.Version;
                                    oldBPM = 0;
                                    if (settings.ContainsSetting("customCreator"))
                                    {
                                        oldCreator = BM.Creator;
                                        BM.Creator = settings.GetSetting("customCreator");
                                    }
                                }
                                catch (Exception e)
                                {
                                    errorText = ("The beatmap could not be parsed. Please post the following error in the forums:\n" + e);
                                }
                            }
                        }
                        page = 0;
                        continue;

                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Select option by typing any of the following numbers:");
                        Console.WriteLine("(1) Change BPM");
                        Console.WriteLine("(2) Change version");
                        Console.WriteLine("(3) Save beatmap\n");
                        Console.WriteLine("(8) Change song format");
                        Console.WriteLine("(9) Set custom creator");
                        Console.WriteLine("(0) Select another beatmap\n");

                        Console.ForegroundColor=ConsoleColor.White;
                        Console.WriteLine("Option: ");

                        int option;
                        ConsoleKeyInfo Kinfo = Console.ReadKey();
                        if (Kinfo.Key == ConsoleKey.Escape)
                        {
                            page = 0;
                            continue;
                        }
                        if (!int.TryParse(Kinfo.KeyChar.ToString(CultureInfo.InvariantCulture), out option))
                        {
                            errorText = "Entered option must be a numerical value.";
                            page = 0;
                            continue;
                        }
                        switch (option)
                        {
                            case 0:
                                page = -1;
                                continue;
                            case 1: case 2: case 3: case 8: case 9:
                                page = option;
                                continue;
                            default:
                                errorText = "Entered option value must be a valid option.";
                                page = 0;
                                continue;
                        }
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter the BPM increase:");
                        Console.WriteLine("(Example: N, +N, -N, *N, /N)\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("BPM: ");

                        input = Console.ReadLine();

                        Console.WriteLine("-------------------------------------------------------------------------------");
                        Console.WriteLine("Processing timingpoints...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        bool error = false;
                        bool setRatio = false;
                        foreach (TimingPoint tp in BM.TimingPoints)
                        {
                            if (tp.InheritsBPM == false)
                            {
                                float currentBPM = 60000 / tp.BpmDelay;
                                float tempDbl;
                                float newBPM;
                                if (float.TryParse(input, out tempDbl) && !input.Contains("+") && !input.Contains("-"))
                                {
                                    if (!setRatio)
                                    {
                                        bpmRatio = oldBPM / tempDbl;
                                        setRatio = !setRatio;
                                    }
                                    newBPM = tempDbl;
                                }
                                else
                                {
                                    try
                                    {
                                        newBPM = (float)Convert.ToDouble(new DataTable().Compute(currentBPM + input, null));
                                        if (!setRatio)
                                        {
                                            bpmRatio = oldBPM / Convert.ToDouble(new DataTable().Compute(oldBPM + input, null));
                                            setRatio = !setRatio;
                                        }
                                    }
                                    catch
                                    {
                                        errorText = "BPM requires a numerical value or function.";
                                        error = true;
                                        break;
                                    }
                                }
                                float newDelay = 60000 / newBPM;
                                tp.BpmDelay = newDelay;
                                tp.Time = (int)(tp.Time * bpmRatio);
                            }
                            else
                            {
                                tp.Time = (int)(tp.Time * bpmRatio);
                            }
                        }
                        if (error)
                        {
                            page = 0;
                            continue;
                        }


                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Processing events...");

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        foreach (EventBase e in BM.Events)
                        {
                            e.StartTime = (int)(e.StartTime * bpmRatio);
                            if (e.GetType() == typeof(BreakEvent))
                                ((BreakEvent)e).EndTime = (int)(((BreakEvent)e).EndTime * bpmRatio);
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\nProcessing hitobjects...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        foreach (CircleObject hO in BM.HitObjects)
                        {
                            hO.StartTime = (int)(hO.StartTime * bpmRatio);
                            if (hO.GetType() == typeof(SpinnerObject))
                                ((SpinnerObject)hO).EndTime = (int)(((SpinnerObject)hO).EndTime * bpmRatio);
                        }
                        page = 0;
                        continue;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter the version:\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Version: ");

                        input = Console.ReadLine();

                        BM.Version = input;
                        versionSet = true;
                        page = 0;
                        continue;

                    case 3:
                        string ext = BM.AudioFilename.Substring(BM.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture));

                        //temp1: Audio copy
                        //temp2: Decoded wav
                        //temp3: stretched file
                        //temp4: Encoded mp3
                        string temp1 = getTempFilename("mp3");
                        string temp2 = getTempFilename("wav");
                        string temp3 = getTempFilename("wav");
                        string temp4 = getTempFilename("mp3");

                        try
                        {
                            CopyFile(BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + BM.AudioFilename, temp1);
                        }
                        catch
                        {
                            errorText = "Please make sure the beatmap set is not selected in the osu! menu and try again.";
                            page = 0;
                            continue;
                        }
                        BM.AudioFilename = BM.AudioFilename.Substring(0, BM.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + NormalizeText(BM.Version) + (saveAsMP3 ? ".mp3" : ".wav");
                        Process p = new Process();
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.FileName = "lame.exe";
                        p.StartInfo.Arguments = string.Format("--decode {0} {1}", temp1, temp2);
                        p.Start();
                        p.WaitForExit();

                        p.StartInfo.FileName = "soundstretch.exe";
                        p.StartInfo.Arguments = string.Format("{0} {1} -tempo={2}", temp2, temp3, (Math.Pow(bpmRatio, -1) - 1) * 100);
                        p.Start();
                        p.WaitForExit();
                        if (saveAsMP3)
                        {
                            p.StartInfo.FileName = "lame.exe";
                            p.StartInfo.Arguments = string.Format("{0} {1}", temp3, temp4);
                            p.Start();
                            p.WaitForExit();
                            CopyFile(temp4, BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture)) + "\\" + BM.AudioFilename);
                        }
                        else
                            CopyFile(temp3, BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture)) + "\\" + BM.AudioFilename);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saving beatmap...");
                        BM.Filename = BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + NormalizeText(BM.Artist) + " - " + NormalizeText(BM.Title) + " (" + NormalizeText(BM.Creator) + ")" + " [" + NormalizeText(BM.Version) + "].osu";
                        BM.Save(BM.Filename);

                        Console.WriteLine("Cleaning up...");
                        File.Delete(temp1);
                        File.Delete(temp2);
                        File.Delete(temp3);
                        File.Delete(temp4);

                        
                        Console.WriteLine("Done! Press any key to go to menu.");
                        Console.ReadKey();
                        page = 0;
                        continue;

                    case 8:
                        saveAsMP3 = !saveAsMP3;
                        settings.AddSetting("customSaveAsMP3", Convert.ToInt32(saveAsMP3).ToString(CultureInfo.InvariantCulture));
                        settings.Save();
                        page = 0;
                        continue;
                    case 9:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter a custom creator name. This creator will be used for every single map version created with this program.");
                        Console.WriteLine("Enter /reset to remove custom creator.\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Creator: ");

                        input = Console.ReadLine();

                        if (input == "/reset")
                        {
                            BM.Creator = oldCreator;
                            if (settings.ContainsSetting("customCreator"))
                            {
                                settings.DeleteSetting("customCreator");
                                settings.Save();
                            }

                        }
                        else
                        {
                            settings.AddSetting("customCreator", input);
                            settings.Save();
                            BM.Creator = input;
                        }
                        page = 0;
                        continue;
                }
                break;
            }
        }

        private static string getTempFilename(string ext)
        {
            return Path.GetTempPath() + Guid.NewGuid() + '.' + ext;
        }

        public static string NormalizeText(string str)
        {
            return str.Replace("\"", "").Replace("*", "").Replace("\\", "").Replace("/", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }

        public static void CopyFile(string src, string dst)
        {
            using (FileStream srcStream = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream dstStream = new FileStream(dst, FileMode.Create))
            {
                srcStream.CopyTo(dstStream);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FloatByte
        {
            [FieldOffset(0)]
            public Byte[] Bytes;

            [FieldOffset(0)]
            public float[] Floats;
        }
    }
}
