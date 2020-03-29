using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_trainer
{
    class Helper
    {
        public static T Clamp<T>(T val, T min, T max) where T : IComparable {
            return val.CompareTo(max) > 0 ? max : val.CompareTo(min) < 0 ? min : val;
        }
        public static decimal Quantize(decimal value, decimal step) {
            value += step / 2; // make function symmetrical
            int steps = (int)(value / step);
            return steps * step;
        }
    }
}
