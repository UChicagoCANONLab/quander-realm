using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Qupcakery
{
    public static class Utilities
    {
        public static List<Sprite> helpMenuSprites = new List<Sprite> { };

        // Tutorial Management
        public static SortedList<int, List<Sprite>> tutorialSprites = new SortedList<int, List<Sprite>>();
        public static List<int> completedTutorials = new List<int>();

        // Add levels that have tutorials
        // TODO: refactor help, remove tutorials
        public static void InitializeTutorialAndHelpMenu()
        {
            LoadSprites(helpMenuSprites, "Sprites/Help");

            int[] tutorialLevelNum = {};
            foreach (int i in tutorialLevelNum)
            {
                LoadTutorialSprites(i);
            }

            completedTutorials.Clear();
        }

        static void LoadSprites(List<Sprite> list, string folder)
        {
            Sprite[] LoadedObjects = Resources.LoadAll(folder, typeof(Sprite)).Cast<Sprite>().ToArray();
            foreach (var obj in LoadedObjects)
            {
                list.Add(obj);
            }
        }

        static void LoadTutorialSprites(int tutorialLevel)
        {
            tutorialSprites.Add(tutorialLevel, new List<Sprite>());
            LoadSprites(tutorialSprites[tutorialLevel], "Sprites/Tutorial/Level" + tutorialLevel.ToString());
        }

        public static string GetStringFrom2DIntArray(int[,] arr, int rowCnt, int colCnt)
        {
            string str = "";
            for (int i = 0; i < rowCnt; i++)
            {
                str += "[";
                for (int j = 0; j < colCnt; j++)
                {
                    str += arr[i, j].ToString();
                    if (j < colCnt - 1) str += " ,";
                }
                str += "]\n";
            }
            return str;
        }
    }
}