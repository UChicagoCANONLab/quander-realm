using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Wrapper
{
    public class Bonus : MonoBehaviour
    {
        public string bonusID;
        public string title;
        private int maximum;
        public int[] cost;
        private bool inSequence;
        public string effect;
        [SerializeField] private List<string> usableScenes = new List<string>();
        private string[] unlockCriteria;
        private string dependency;

        public int numberAvailable;
        public bool currentlyUsable;
        private GemType gemType;

        [Header("Display GameObjects")]
        [SerializeField] private Image gemGraphic;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI numUsableText;
        [SerializeField] public GameObject counterObject;

        [SerializeField] private Sprite[] gemOptions;
        private enum GemType { SkipLevel, StreakFreeze };


        void OnEnable()
        {
            UpdateBonus();
        }

        public void InitBonus(BonusAsset boAsset)
        {         
            bonusID = boAsset.ID;   
            title = boAsset.title;
            maximum = boAsset.maximum;
            cost = boAsset.cost;
            inSequence = boAsset.inSequence;
            effect = boAsset.effect;
            usableScenes.AddRange(boAsset.usableScenes);
            unlockCriteria = boAsset.unlockCriteria;
            dependency = boAsset.dependency;

            numberAvailable = Events.NumberBonuses.Invoke(bonusID);
            currentlyUsable = usableScenes.Contains(SceneManager.GetActiveScene().name);
            gemType = (GemType)Enum.Parse(typeof(GemType), bonusID);

            gemGraphic.sprite = gemOptions[(int)gemType];
            titleText.text = $"{title}";
            numUsableText.text = $"{numberAvailable}";
        }

        public void UpdateBonus()
        {
            numberAvailable = Events.NumberBonuses.Invoke(bonusID);
            currentlyUsable = usableScenes.Contains(SceneManager.GetActiveScene().name);

            numUsableText.text = $"{numberAvailable}";
        }



        public void DisplayOnly(bool disabled)
        {
            this.gameObject.GetComponent<Button>().interactable = disabled;
            // counterObject.SetActive(disabled);
        }



        public bool UseBonus()
        {
            if (!currentlyUsable) {
                Debug.Log("NOT AVAILABLE IN THIS SCENE");
                return false; }
            if (!Events.UseAvailableBonus.Invoke(bonusID)) {
                Debug.Log("NOT FOUND IN LIST OF OWNED BONUSES");
                return false; }

            switch(gemType)
            {
                case GemType.SkipLevel:
                    SkipLevel(Events.GetCurrentGame.Invoke());
                    break;
            }

            UpdateBonus();
            return true; 
        }


        public void SkipLevel(Game game)
        {
            // Check if applicable
            if (!Events.GetGameUnlocked.Invoke(game)) return;
            else if (Events.GetMinigameAllLevelsUnlocked.Invoke(game)) return;

            // Unlock next level in save file
            Events.UnlockNextLevel.Invoke(game);

            // Reload minigame save file
            switch(game)
            {
                case Game.Qupcakes:
                    Qupcakery.LoadGame.Load();
                    if (SceneManager.GetActiveScene().name != "QU_LevelSelection") {
                        GameObject.Find("MenuButton").GetComponent<Qupcakery.MenuButton>().LoadLevelSelectionMenu();
                    } else {
                        SceneManager.LoadScene("QU_LevelSelection");
                    }
                    break;
                case Game.Labyrinth:
                    // Loads data at start of scene
                    SceneManager.LoadScene("LA_LevelSelect");
                    break;
                case Game.Circuits:
                    // Loads data at start of scene
                    SceneManager.LoadScene("Circuits_Menu");
                    break;
                case Game.QueueBits:
                    // Loads data at start of scene
                    SceneManager.LoadScene("QB_LevelSelect");
                    break;
                case Game.BlackBox:
                    // WORK ON THIS ONE
                    break;
            }
            return;
        }

    }
}