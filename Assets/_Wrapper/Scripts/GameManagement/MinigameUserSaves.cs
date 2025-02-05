using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class MinigameUserSaves : MonoBehaviour
    {
        public Qupcakery.GameData data_Qupcakery;
        public Labyrinth.TTSaveData data_Twintanglement;
        public Circuits.Circuits_SaveData data_TanglesLair;
        public QueueBits.QBSaveData data_Queuebits;
        public BlackBox.BBSaveData data_BuriedTreasure;


        // void Start() 
        // {
        //     LoadAllMinigames();
        // }


        public void LoadAllMinigames()
        {
            LoadMinigameSave(Game.Qupcakes);
            LoadMinigameSave(Game.Labyrinth);
            LoadMinigameSave(Game.QueueBits);
            LoadMinigameSave(Game.Circuits);
            LoadMinigameSave(Game.BlackBox);
        }


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


        public int GetTotalStars(Game game)
        {
            switch(game) {
                case Game.Qupcakes:
                    if (data_Qupcakery != null) {
                        return data_Qupcakery.TotalStars;
                    } break;
                case Game.Labyrinth:
                    if (data_Twintanglement != null) {
                        return data_Twintanglement.TotalStars;
                    } break;
                case Game.Circuits:
                    if (data_TanglesLair != null) {
                        return data_TanglesLair.totalStars;
                    } break;
                case Game.QueueBits:
                    if (data_Queuebits != null) {
                        return data_Queuebits.totalStars;
                    } break;
                case Game.BlackBox:
                    if (data_BuriedTreasure != null) {
                        return data_BuriedTreasure.totalStars;
                    } break;
            } return 0;
        }

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
                        int maxLevel = 0;
                        for(int i=0; i<24; i++) {
                            if (data_BuriedTreasure.starsPerLevel[i] > 0) {
                                maxLevel = i+1;
                            }
                        } return maxLevel;                        
                    } break;
            } return 0;
        }


    }
}