using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
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
            Settings s = new Settings();
            new Updater(s);

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
                    case 0:
                        //Clear directory
                        File.Delete(Environment.CurrentDirectory + "\\temp.wav");
                        File.Delete(Environment.CurrentDirectory + "\\temp2.wav");
                        File.Delete(Environment.CurrentDirectory + "\\temp3.mp3");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Select option by typing any of the following numbers:");
                        Console.WriteLine("(1) Change BPM");
                        Console.WriteLine("(2) Change version");
                        Console.WriteLine("(3) Save beatmap\n");

                        Console.ForegroundColor=ConsoleColor.White;
                        Console.WriteLine("Option: ");

                        int option;
                        if (!int.TryParse(Console.ReadLine(), out option))
                        {
                            lastText = "Entered option must be a numerical value.";
                            page = 0;
                            continue;
                        }
                        if (option < 1 || option > 3)
                        {
                            lastText = "Entered option value must be betwee 1 and 3.";
                            page = 0;
                            continue;
                        }
                        page = option;
                        continue;

                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter the BPM increase:");
                        Console.WriteLine("(Example: +10 or -10)\n");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("BPM: ");

                        double increase;
                        if (!double.TryParse(Console.ReadLine(), out increase))
                        {
                            lastText = "Target BPM must be a numerical value.";
                            page = 1;
                            continue;
                        }

                        Console.WriteLine("-------------------------------------------------------------------------------");
                        Console.WriteLine("Processing timingpoints...");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        bool setRatio = false;
                        foreach (TimingPointInfo tp in BM.TimingPoints.Where(tp => tp.inheritsBPM == false))
                        {
                            double currentBPM = 60000 / tp.bpmDelay;
                            double newDelay = 60000 / (currentBPM + increase);
                            if (!setRatio)
                            {
                                bpmRatio = oldBPM / (currentBPM + increase);
                                setRatio = true;
                            }
                            tp.bpmDelay = newDelay;
                            tp.time = (int)(tp.time * bpmRatio);
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
                        p.StartInfo.Arguments = "temp.wav temp2.wav -tempo=" + (1 - bpmRatio > 0 ? (1 - bpmRatio) * 200 : (1 - bpmRatio) * 50);
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.FileName = "lame.exe";
                        p.StartInfo.Arguments = "temp2.wav temp3.mp3";
                        p.Start();
                        p.WaitForExit();
                        moveFile(Environment.CurrentDirectory + "\\temp3.mp3", BM.Filename.Substring(0, BM.Filename.LastIndexOf("\\", StringComparison.InvariantCulture)) + "\\" + BM.AudioFilename).Wait();
                        BM.Save(BM.Filename);
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
