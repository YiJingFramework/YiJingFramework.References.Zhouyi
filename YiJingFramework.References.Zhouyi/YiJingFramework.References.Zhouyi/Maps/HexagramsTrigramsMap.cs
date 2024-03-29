﻿using System;

namespace YiJingFramework.References.Zhouyi.Maps
{
    internal sealed class HexagramsTrigramsMap
    {
        private readonly byte[,] map
            = new byte[,] {
                        { 0, 9, 12, 24, 43, 5, 32, 11 },
                        { 42, 57, 48, 16, 27, 46, 30, 44 },
                        { 13, 37, 29, 20, 49, 63, 55, 34 },
                        { 33, 53, 54, 50, 31, 39, 61, 15 },
                        { 8, 60, 36, 41, 56, 58, 52, 19 },
                        { 4, 59, 62, 2, 47, 28, 38, 7 },
                        { 25, 40, 21, 26, 17, 3, 51, 22 },
                        { 10, 18, 35, 23, 45, 6, 14, 1 }};

        internal int GetHexagram(int lower, int upper)
        {
            return this.map[upper - 1, lower - 1] + 1;
        }
        internal (int upper, int lower) GetTrigrams(int hexagram)
        {
            var ser = hexagram - 1;
            for (int x = 0; x < 8; ++x)
                for (int y = 0; y < 8;)
                    if (this.map[x, y++] == ser)
                        return (x + 1, y);
            throw new Exception("This line should never be executed.");
        }
    }
}
