using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class ShopTab : MonoBehaviour
    {
        private Toggle tab;

        private Game game;
        private int pageNumber = 0;
        public List<GameObject> bonusList;



        public ShopTab(GameObject tabGO, Game game)
        {
            this.game = game;
        }

        public void AddBonus(GameObject bonusGO)
        {
            bonusList.Add(bonusGO);
        }
    }
}