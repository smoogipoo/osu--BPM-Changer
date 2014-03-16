﻿using System;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using BMAPI;
using smgiFuncs;

namespace osu__BPM_Changer
{
    class Program
    {
        private static Beatmap BM;
        private static string lastText = "";
        private static double bpmRatio;
        private static double oldBPM;
        
        [STAThread]
        static void Main()
        {
            Application.CurrentCulture = new CultureInfo("en-US", false);
            Thread updaterThread = new Thread(updaterStart);
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

        public static void updaterStart()
        {
            Settings s = new Settings();
            new Updater(s);
        }
        public static void BeginGUI(int page)
        {
            while (true)
            {
                //Main GUI
                Console.Clear();
                if (lastText != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(lastText);
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    lastText = "";
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Loaded beatmap " + BM.Source + (BM.Source != "" ? " (" + BM.Artist + ")" : BM.Artist) + " - " + BM.Title + " [" + BM.Version + "]\n");
                double minBPM = double.MaxValue, maxBPM = double.MinValue;
                foreach (TimingPointInfo tp in BM.TimingPoints.Where(tp => tp.inheritsBPM == false))
                {
                    if (60000/tp.bpmDelay < minBPM)
                        minBPM = 60000/tp.bpmDelay;
                    if (60000/tp.bpmDelay > maxBPM)
                        maxBPM = 60000/tp.bpmDelay;
                }
                oldBPM = minBPM;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Map BPM: " + minBPM + (minBPM != maxBPM ? " - " + maxBPM : ""));
                Console.WriteLine("Beatmap will be saved as version: [" + BM.Version + "]");
                Console.WriteLine("-------------------------------------------------------------------------------");

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
                                }
                                catch (Exception e)
                                {
                                    lastText = ("The beatmap could not be parsed. Please post the following error in the forums:\n" + e);
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
                        Console.WriteLine("(0) Select another beatmap\n");

                        Console.ForegroundColor=ConsoleColor.White;
                        Console.WriteLine("Option: ");

                        int option;
                        if (!int.TryParse(Console.ReadLine(), out option))
                        {
                            lastText = "Entered option must be a numerical value.";
                            page = 0;
                            continue;
                        }
                        if (option < 0 || option > 3 )
                        {
                            lastText = "Entered option value must be betwee 1 and 3.";
                            page = 0;
                            continue;
                        }
                        if (option == 0)
                        {
                            page = -1;
                            continue;
                        }
                        page = option;
                        continue;

                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter the BPM increase:");
                        Console.WriteLine("(Example: +N, -N, *N, /N, +N%, -N%, *N%, /N%, etc)\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("BPM: ");

                        string operations = Console.ReadLine();

                        Console.WriteLine("-------------------------------------------------------------------------------");
                        Console.WriteLine("Processing timingpoints...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        bool setRatio = false;
                        foreach (TimingPointInfo tp in BM.TimingPoints)
                        {
                            if (tp.inheritsBPM == false)
                            {
                                double currentBPM = 60000 / tp.bpmDelay;
                                double newBPM = Convert.ToDouble(new DataTable().Compute(currentBPM + operations, null));
                                double newDelay = 60000 / newBPM;
                                if (!setRatio)
                                {
                                    bpmRatio = oldBPM / newBPM;
                                    setRatio = true;
                                }
                                tp.bpmDelay = newDelay;
                                tp.time = (int)(tp.time * bpmRatio);
                            }
                            else
                            {
                                tp.time = (int)(tp.time * bpmRatio);
                                tp.bpmDelay = tp.bpmDelay * bpmRatio;
                            }
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Processing events...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        foreach(dynamic e in (IEnumerable<dynamic>)BM.Events)
                        {
                            e.startTime = (int)(e.startTime * bpmRatio);
                            if (e.GetType() == typeof(BreakInfo))
                                e.endTime = (int)(e.endTime * bpmRatio);
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\nProcessing hitobjects...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        foreach (dynamic hO in (IEnumerable<dynamic>)BM.HitObjects)
                        {
                            hO.startTime = (int)(hO.startTime * bpmRatio);
                            if (hO.GetType() == typeof(SpinnerInfo))
                                hO.endTime = (int)(hO.endTime * bpmRatio);
                        }
                        page = 0;
                        continue;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter the version:\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Version: ");
                        BM.Version = Console.ReadLine();
                        BM.Filename = BM.Filename.Substring(0, BM.Filename.LastIndexOf("[", StringComparison.InvariantCulture) + 1) + BM.Version + "].osu";
                        page = 0;
                        continue;

                    case 3:
                        try
                        {
                            moveFile(BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + BM.AudioFilename, Environment.CurrentDirectory + "\\temp.mp3").Wait();
                        }
                        catch
                        {
                            lastText = "Please make sure the beatmap set is not selected in the osu! menu and try again.";
                            page = 0;
                            continue;
                        }
                        BM.AudioFilename = BM.AudioFilename.Substring(0, BM.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + BM.Version + BM.AudioFilename.Substring(BM.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture));
                        Process p = new Process();
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.FileName = "lame.exe";
                        p.StartInfo.Arguments = "--decode temp.mp3 temp.wav";
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.FileName = "soundstretch.exe";
                        p.StartInfo.Arguments = "temp.wav temp2.wav -tempo=" + (Math.Pow(bpmRatio, -1) - 1) * 100;
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.FileName = "lame.exe";
                        p.StartInfo.Arguments = "temp2.wav temp3.mp3";
                        p.Start();
                        p.WaitForExit();
                        moveFile(Environment.CurrentDirectory + "\\temp3.mp3", BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture)) + "\\" + BM.AudioFilename).Wait();
                        BM.Save(BM.Filename);

                        //Clear directory
                        File.Delete(Environment.CurrentDirectory + "\\temp.wav");
                        File.Delete(Environment.CurrentDirectory + "\\temp2.wav");
                        File.Delete(Environment.CurrentDirectory + "\\temp3.mp3");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nDone! Press any key to go back.");
                        Console.ReadKey();
                        page = 0;
                        continue;
                }
                break;
            }
        }

        public static async Task moveFile(string src, string dst)
        {
            using (FileStream srcStream = File.Open(src, FileMode.Open))
            {
                using (FileStream dstStream = File.Create(dst))
                {
                    await srcStream.CopyToAsync(dstStream);
                }
            }
        }
    }
}
