using BMAPI.v1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace osu_trainer
{
    class DifficultyCalculator
    {
        static string tempBeatmapPath = "temp.osu";
        static Semaphore diffCalcInProgress;

        static DifficultyCalculator() {
            diffCalcInProgress = new Semaphore(1, 1);
        }

        // AR modifications: HR, DT, DTHR, BpmMultiplier
        public static float CalculateMultipliedAR(Beatmap map, float BpmMultiplier)
        {
            float newbpmMs = ApproachRateToMs(map.ApproachRate) / BpmMultiplier;
            float newbpmAR = MsToApproachRate(newbpmMs);
            return (newbpmAR < 10) ? newbpmAR : 10;
        }
        public static float ApproachRateToMs(float approachRate)
        {
            if (approachRate <= 50)
            {
                return 1800.0f - approachRate * 120.0f;
            }
            else
            {
                float remainder = approachRate - 5;
                return 1200.0f - remainder * 150.0f;
            }
        }
        public static float MsToApproachRate(float ms)
        {
            // bullshit
            float smallestDiff = 100000.0f;
            for (int AR = 0; AR <= 110; AR++)
            {
                var newDiff = Math.Abs(ApproachRateToMs(AR/10.0f) - ms);
                if (newDiff < smallestDiff)
                    smallestDiff = newDiff;
                else
                    return (AR - 1) / 10.0f;
            }
            return 300;
        }

        public static float CalculateStarRating(Beatmap map)
        {
            if (map == null)
                throw new NullReferenceException();
            diffCalcInProgress.WaitOne();
            if (map == null)
            {
                diffCalcInProgress.Release();
                throw new NullReferenceException();
            }
            map.Save(tempBeatmapPath);
            Process oppai = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "oppai\\oppai.exe",
                    Arguments = $"\"{tempBeatmapPath}\" -ojson",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    // fix this omg 
                    //StandardOutputEncoding = Encoding.UTF8
                }
            };
            oppai.Start();
            string oppaiOutput = oppai.StandardOutput.ReadToEnd();
            oppai.WaitForExit();
            diffCalcInProgress.Release();

            // json parsing
            JObject oppaiData = JObject.Parse(oppaiOutput);
            string errstr     = oppaiData.GetValue("errstr").ToObject<string>();
            if (errstr != "no error")
            {
                // TODO: An error occurs when opening a non-osu!standard map (mania, taiko, etc)
                Console.WriteLine("Could not calculate difficulty");
                return 0;
            }
            float stars      = oppaiData.GetValue("stars").ToObject<float>();
            float aimStars   = oppaiData.GetValue("aim_stars").ToObject<float>();
            float speedStars = oppaiData.GetValue("speed_stars").ToObject<float>();


            return stars;
        }
    }
}
