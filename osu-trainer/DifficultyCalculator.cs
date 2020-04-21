using BMAPI.v1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private static float ApproachRateToMs(float approachRate)
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
        private static float MsToApproachRate(float ms)
        {
            // bullshit
            // TODO: check edge cases
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
        public static float CalculateMultipliedOD(Beatmap map, float BpmMultiplier)
        {
            float newbpmMs = OverallDifficultyToMs(map.OverallDifficulty) / BpmMultiplier;
            float newbpmOD = MsToOverallDifficulty(newbpmMs);
            newbpmOD = (float)Math.Round(newbpmOD * 10.0f) / 10.0f;
            newbpmOD = JunUtils.Clamp(newbpmOD, 0, 10);
            return newbpmOD;
        }
        private static float OverallDifficultyToMs(float od) => -6.0f * od + 79.5f;
        private static float MsToOverallDifficulty(float ms) => (79.5f - ms) / 6.0f;
        public static (float, float, float) CalculateStarRating(Beatmap map)
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
            File.Delete(tempBeatmapPath);
            diffCalcInProgress.Release();

            // json parsing
            JObject oppaiData;
            try
            {
                oppaiData = JObject.Parse(oppaiOutput);
            }
            catch (Exception e)
            {
                return (0, 0, 0);
            }            string errstr     = oppaiData.GetValue("errstr").ToObject<string>();
            if (errstr != "no error")
            {
                // TODO: An error occurs when opening a non-osu!standard map (mania, taiko, etc)
                Console.WriteLine("Could not calculate difficulty");
                return (0, 0, 0);
            }
            float stars      = oppaiData.GetValue("stars").ToObject<float>();
            float aimStars   = oppaiData.GetValue("aim_stars").ToObject<float>();
            float speedStars = oppaiData.GetValue("speed_stars").ToObject<float>();


            return (stars, aimStars, speedStars);
        }
    }
}
