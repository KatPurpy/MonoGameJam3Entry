using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public static class PlayerProfile
    {
        public static bool 
            Race1Unlock,                //0
            Race2Unlock,                //1
            Race3Unlock,                //2
            FootballUnlock,             //3
            SpaceUnlock,                //4
            SpaceComplete,              //5
            PlayedRaceStoryIntro,       //6
            PlayedRaceStoryEnding,      //7
            PlayedFootballStoryIntro,   //8
            PlayedFootballStoryEnding,  //9
            PlayedSpaceStoryIntro,      //10
            PlayedSpaceStoryEnding;     //11
        public static uint GetPacked()
        {
            uint number = 0;

            SetFlag(ref number, 0, Race1Unlock);
            SetFlag(ref number, 1, Race2Unlock);
            SetFlag(ref number, 2, Race3Unlock);
            SetFlag(ref number, 3, FootballUnlock);
            SetFlag(ref number, 4, SpaceUnlock);
            SetFlag(ref number, 5, SpaceComplete);
            SetFlag(ref number, 6, PlayedRaceStoryIntro);
            SetFlag(ref number, 7, PlayedRaceStoryEnding);
            SetFlag(ref number, 8, PlayedFootballStoryIntro);
            SetFlag(ref number, 9, PlayedFootballStoryEnding);
            SetFlag(ref number, 10, PlayedSpaceStoryIntro);
            SetFlag(ref number, 11, PlayedSpaceStoryEnding);
            return number;
        } 

        static void SetFlag(ref uint number, int bit, bool set)
        {
            if (set)
            {
                number |= ((uint)1 << bit);
            }
        }

        static bool GetFlag(uint number, int bit)
        {
            var mask = (((uint)1 << bit));
            return (number & mask) == mask;
        }

        static void GetFlagAndSetIt(uint number, int bit, ref bool b)
        {
            b = GetFlag(number,bit);
        }

        public static bool[] DebugGetFlags()
        {
            bool[] b = new bool[11];

            for(int i =0; i < b.Length; i++)
            {
                b[i] = GetFlag(GetPacked(), i);
            }

            return b;
        }

        public static void Save()
        {
            uint value = GetPacked();
            File.WriteAllBytes("PLRSAV", BitConverter.GetBytes(value));
            Load();
            Debug.Assert(value == GetPacked());
        }
        public static void Load()
        {
            if (File.Exists("PLRSAV"))
            {
                uint number = BitConverter.ToUInt32(File.ReadAllBytes("PLRSAV"));

                GetFlagAndSetIt(number, 0, ref Race1Unlock);
                GetFlagAndSetIt(number, 1, ref Race2Unlock);
                GetFlagAndSetIt(number, 2, ref Race3Unlock);
                GetFlagAndSetIt(number, 3, ref FootballUnlock);
                GetFlagAndSetIt(number, 4, ref SpaceUnlock);
                GetFlagAndSetIt(number, 5, ref SpaceComplete);
                GetFlagAndSetIt(number, 6, ref PlayedRaceStoryIntro);
                GetFlagAndSetIt(number, 7, ref PlayedRaceStoryEnding);
                GetFlagAndSetIt(number, 8, ref PlayedFootballStoryIntro);
                GetFlagAndSetIt(number, 9, ref PlayedFootballStoryEnding);
                GetFlagAndSetIt(number, 10, ref PlayedSpaceStoryIntro);
                GetFlagAndSetIt(number, 11, ref PlayedSpaceStoryEnding);
            }
        }
    }
}
