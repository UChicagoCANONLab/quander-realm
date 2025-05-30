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
        [SerializeField] public string title;
        [SerializeField] public Game game;
        [SerializeField] public int cost;
        [SerializeField] public int numUnlockable;
        [SerializeField] public string effect;
        [SerializeField] public string unlockCriteria;
        [SerializeField] public CriteriaType criteria;
        [SerializeField] public string dependency;

        private bool inSelector = false;
        public int numberUsed = 0;
        public int numberUnlocked = 0;

        [Header("Display GameObjects: General")]
        [SerializeField] private Image gemType;
        [SerializeField] private TextMeshProUGUI titleText;

        // [Header("Display GameObjects: In Shop")]
        // [SerializeField] private TextMeshProUGUI descriptionText;
        // [SerializeField] private TextMeshProUGUI costText;
        // [SerializeField] private TextMeshProUGUI numAvailableText;

        [Header("Display GameObjects: Usable")]
        [SerializeField] private TextMeshProUGUI numUsableText;
        [SerializeField] public GameObject counterObject;


        public void InitBonus(BonusAsset boAsset, bool buyable)
        {
            // Set values from BonusAsset
            game = boAsset.game;
            cost = boAsset.cost;
            numUnlockable = boAsset.number;
            effect = boAsset.effect;
            unlockCriteria = boAsset.unlockCriteria;
            criteria = boAsset.criteriaType;
            dependency = boAsset.dependency;

            // Set texts and icons
            titleText.text = boAsset.title;
            // descriptionText.text = effect;
            // Determine how to assign gem color/icon here

            // If buyable, need to display cost and how many available
            // If usable, do not need cost; do need how many you already have

            // How to get NumUsableText gameObject
            // counterObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        }

        public int CalculateNumberBuyable()
        {
            // bonus.numberUnlocked - bonus.numberUsed;
            return 0;
        }

        public void UseBonus(string name)
        {
            switch(name)
            {
                case "SkipLevel":
                    SkipLevel(Events.GetCurrentGame.Invoke());
                    break;
            }
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