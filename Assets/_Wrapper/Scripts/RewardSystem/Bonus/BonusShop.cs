using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class BonusShop : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Header("Shop Interface")]
        [SerializeField] public GameObject bonusBuyerContainer;
        [SerializeField] public GameObject confirmationContainer;

        [Header("Section Tabs")]
        [SerializeField] private GameObject GenTab;
        [SerializeField] private GameObject BBTab;
        [SerializeField] private GameObject CTTab;
        [SerializeField] private GameObject QUTab;
        [SerializeField] private GameObject LATab;
        [SerializeField] private GameObject QBTab;

        private Dictionary<Game, ShopTab> shop;
        private ShopTab currentTab;

        [Header("Bonus GameObjects")]
        [SerializeField] private GameObject bonusPrefab;
        private GameObject featuredBonus;

        
        void Awake()
        {
            InitShop();
        }

        private void Start()
        {
            PopulateShop();

        }


        private void InitShop()
        {
            shop = new Dictionary<Game, ShopTab>
            {
                { Game.None,        new ShopTab(GenTab, Game.None)},
                { Game.BlackBox,    new ShopTab(BBTab, Game.BlackBox)},
                { Game.Circuits,    new ShopTab(CTTab, Game.Circuits)},
                { Game.QueueBits,   new ShopTab(QUTab, Game.QueueBits)},
                { Game.Labyrinth,   new ShopTab(LATab, Game.Labyrinth)},
                { Game.Qupcakes,    new ShopTab(QBTab, Game.Qupcakes)}
            };
        }

        private void PopulateShop()
        {
            // iterate through all bonus assets
            // create bonus gameobject from asset
            // add gameobject to correct shop page
        }

    }
}