using BMAPI.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    class DifficultyCalculator
    {
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
    }
}
