using System;
using System.Collections.Generic;

namespace Rovio.Common
{
    public class RandomGenerator
    {
        private const uint RangeMax = 0x7fffffffU;

        private uint m_Seed;

        public RandomGenerator(uint seed)
        {
            m_Seed = seed & RangeMax;
        }

        public RandomGenerator(int seed)
        {
            Seed = ((uint)seed);
        }

        public uint Seed
        {
            get { return m_Seed; }
            set { m_Seed = value & RangeMax; }
        }

        public int Next()
        {
            m_Seed = (m_Seed * 1103515245U + 12345U) & RangeMax;
            return (int)m_Seed;
        }

        /// <summary>
        /// returns in [0, maxValue) range
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int Next(int maxValue)
        {
            return (int)((long)Next() * maxValue / RangeMax);
        }

        /// <summary>
        /// [minValue, maxValue)
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int Next(int minValue, int maxValue)
        {
            if (minValue == maxValue) return maxValue;
            var diff = maxValue - minValue;
            return Next(diff) + minValue;
        }
    }
}