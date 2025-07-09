using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class MinigameUserSaves : MonoBehaviour
    {
        [SerializeField] public Qupcakery.GameData data_Qupcakery;
        [SerializeField] public Labyrinth.TTSaveData data_Twintanglement;
        [SerializeField] public Circuits.Circuits_SaveData data_TanglesLair;
        [SerializeField] public QueueBits.QBSaveData data_Queuebits;
        [SerializeField] public BlackBox.BBSaveData data_BuriedTreasure;

        // [SerializeField] public int TOTALSTARS = 0;

        private Game[] gamesArray = {
            Game.Qupcakes, Game.Labyrinth, Game.Circuits, Game.QueueBits, Game.BlackBox
        };


        private void OnEnable()
        {
            Events.GetMinigameTotalStars += GetMinigameStars;
            Events.GetOverallTotalStars += GetOverallStars;

            Events.GetGameUnlocked += GetMinigameUnlocked;
            Events.GetMinigameMaxLevel += GetMaxLevelUnlocked;
            Events.GetMinigameAllLevelsUnlocked += GetAllLevelsUnlocked;

            Events.LoadMinigameSave += LoadMinigameSave;
            Events.LoadAllMinigameSaves += LoadAllMinigames;

            Events.UnlockNextLevel += UnlockNextLevel;
        }

        private void OnDisable()
        {
            Events.GetMinigameTotalStars -= GetMinigameStars;
            Events.GetOverallTotalStars -= GetOverallStars;

            Events.GetGameUnlocked -= GetMinigameUnlocked;
            Events.GetMinigameMaxLevel -= GetMaxLevelUnlocked;
            Events.GetMinigameAllLevelsUnlocked -= GetAllLevelsUnlocked;

            Events.LoadMinigameSave -= LoadMinigameSave;
            Events.LoadAllMinigameSaves -= LoadAllMinigames;

            Events.UnlockNextLevel -= UnlockNextLevel;
        }

#region Loading UserSaves for each Minigame

        // Loads/Updates all local minigame saves
        public void LoadAllMinigames()
        {
            foreach(Game game in gamesArray) {
                LoadMinigameSave(game);
            }
        }

        // Loads/Updates the local minigame save
        public void LoadMinigameSave(Game game)
        {
            string tempData;
            switch(game) {
                case Game.Qupcakes:
                    tempData = Events.GetMinigameSaveData?.Invoke(Game.Qupcakes);
                    data_Qupcakery = JsonUtility.FromJson<Qupcakery.GameData>(tempData);
                    return;

                case Game.Labyrinth:
                    tempData = Events.GetMinigameSaveData?.Invoke(Game.Labyrinth);
                    data_Twintanglement = JsonUtility.FromJson<Labyrinth.TTSaveData>(tempData);
                    return;

                case Game.Circuits:
                    tempData = Events.GetMinigameSaveData?.Invoke(Game.Circuits);
                    data_TanglesLair = JsonUtility.FromJson<Circuits.Circuits_SaveData>(tempData);
                    return;

                case Game.QueueBits:
                    tempData = Events.GetMinigameSaveData?.Invoke(Game.QueueBits);
                    data_Queuebits = JsonUtility.FromJson<QueueBits.QBSaveData>(tempData);
                    return;

                case Game.BlackBox:
                    tempData = Events.GetMinigameSaveData?.Invoke(Game.BlackBox);
                    data_BuriedTreasure = JsonUtility.FromJson<BlackBox.BBSaveData>(tempData);
                    return;

                default: return;
            }
        }
#endregion

#region Stars

        // Returns total stars won in minigame
        public int GetMinigameStars(Game game)
        {
            int stars = 0;
            switch(game) {
                case Game.Qupcakes:
                    if (data_Qupcakery != null) {
                        stars = data_Qupcakery.TotalStars;
                    } break;
                case Game.Labyrinth:
                    if (data_Twintanglement != null) {
                        stars = data_Twintanglement.TotalStars;
                    } break;
                case Game.Circuits:
                    if (data_TanglesLair != null) {
                        stars = data_TanglesLair.totalStars;
                    } break;
                case Game.QueueBits:
                    if (data_Queuebits != null) {
                        stars = data_Queuebits.totalStars;
                    } break;
                case Game.BlackBox:
                    if (data_BuriedTreasure != null) {
                        stars = data_BuriedTreasure.totalStars;
                    } break;
            } 
            Events.UpdateMinigameStarCount?.Invoke(game, stars);
            return stars;
        }

        // Returns total stars won across all games
        public int GetOverallStars() 
        {
            int tempTotal = 0;
            foreach(Game game in gamesArray)
            {
                tempTotal += GetMinigameStars(game);
            }
            tempTotal += Events.GetMinigameStarCount.Invoke(Game.Rewards);
            
            Events.UpdateUserSaveTotalStars?.Invoke(tempTotal);
            return tempTotal;
        }

#endregion


#region Unlocked (level/game)

        // NOTE: Some games save the max level the player has completed, others the 
        // max level they have access to play. This returns the max level they can play
        public int GetMaxLevelUnlocked(Game game)
        {
            switch(game) {
                case Game.Qupcakes:
                    if (data_Qupcakery != null) {
                        return data_Qupcakery.MaxLevelCompleted + 1;
                    } break;
                
                case Game.Labyrinth:
                    if (data_Twintanglement != null) {
                        return data_Twintanglement.MaxLevelUnlocked;
                    } break;
                
                case Game.Circuits:
                    if (data_TanglesLair != null) {
                        return data_TanglesLair.maxLevel + 1;
                    } break;
                
                case Game.QueueBits:
                    if (data_Queuebits != null) {
                        return data_Queuebits.maxLevelUnlocked;
                    } break;
                
                case Game.BlackBox:
                    if (data_BuriedTreasure != null) {
                        if (data_BuriedTreasure.completed) return 25;
                        return data_BuriedTreasure.maxLevelUnlocked;
                        // int maxLevel = 0;
                        // for(int i=0; i<24; i++) {
                        //     if (data_BuriedTreasure.starsPerLevel[i] > 0) {
                        //         maxLevel = i+1;
                        //     }
                        // } return maxLevel;                        
                    } break;
            }
            return 0;
        }

        // Returns if player has unlocked all levels in the game
        public bool GetAllLevelsUnlocked(Game game)
        {
            switch(game) {
                case Game.Qupcakes:
                    if (data_Qupcakery != null) {
                        return (data_Qupcakery.MaxLevelCompleted == 27);
                    } break;
                case Game.Labyrinth:
                     if (data_Twintanglement != null) {
                        return (data_Twintanglement.MaxLevelUnlocked == 16);
                    } break;
                case Game.Circuits:
                    if (data_TanglesLair != null) {
                        return (data_TanglesLair.maxLevel == 25);
                    } break;

                case Game.QueueBits:
                    if (data_Queuebits != null) {
                        return (data_Queuebits.maxLevelUnlocked == 16);
                    } break;
                case Game.BlackBox:
                    if (data_BuriedTreasure != null) {
                        return data_BuriedTreasure.completed;
                    } break;
            }
            return false;
        }


        // Returns if game is unlocked based on criteria; different for lite/full
        public bool GetMinigameUnlocked(Game game)
        {
            switch(game) {
                case Game.Qupcakes: // CRITERIA: unlocked
                    return true; break;
                case Game.Labyrinth: // CRITERIA: unlocked
                    return true; break;
#if LITE_VERSION
                case Game.Circuits: // CRITERIA: 12 QC stars
                    if (GetMinigameStars(Game.Qupcakes) >= 12) {
                        return true;
                    } break;
                case Game.QueueBits: // CRITERIA: unlocked
                    return true; break;
                case Game.BlackBox: // CRITERIA: 50 total stars
                    if (GetOverallStars() >= 50) {
                        return true;
                    } break;
#else
                case Game.Circuits: // CRITERIA: 27 QC stars
                    if (GetMinigameStars(Game.Qupcakes) >= 27) {
                        return true;
                    } break;
                case Game.QueueBits: // CRITERIA: 10 QC && 10 TT stars
                    if (GetMinigameStars(Game.Qupcakes) >= 10
                    && GetMinigameStars(Game.Labyrinth) >= 10) {
                        return true;
                    } break;
                case Game.BlackBox: // CRITERIA: 120 total stars
                    if (GetOverallStars() >= 120) {
                        return true;
                    } break;
#endif
            } return false;
        }
#endregion

        public void UnlockNextLevel(Game game)
        {
            switch(game){
                case Game.Qupcakes:
                    if (data_Qupcakery != null) {
                        data_Qupcakery.MaxLevelCompleted++;
                        Events.UpdateMinigameSaveData.Invoke(Game.Qupcakes, data_Qupcakery);
                    } break;
                case Game.Labyrinth:
                     if (data_Twintanglement != null) {
                        data_Twintanglement.MaxLevelUnlocked++;
                        Events.UpdateMinigameSaveData.Invoke(Game.Labyrinth, data_Twintanglement);
                    } break;
                case Game.Circuits:
                    if (data_TanglesLair != null) {
                        data_TanglesLair.maxLevel++;
                        Events.UpdateMinigameSaveData.Invoke(Game.Circuits, data_TanglesLair);
                    } break;

                case Game.QueueBits:
                    if (data_Queuebits != null) {
                        data_Queuebits.maxLevelUnlocked++;
                        Events.UpdateMinigameSaveData.Invoke(Game.QueueBits, data_Queuebits);
                    } break;
                case Game.BlackBox:
                    if (data_BuriedTreasure != null) {
                        data_BuriedTreasure.maxLevelUnlocked++;
                        Events.UpdateMinigameSaveData.Invoke(Game.BlackBox, data_BuriedTreasure);
                    } break;
            } return;
        }

    }
}