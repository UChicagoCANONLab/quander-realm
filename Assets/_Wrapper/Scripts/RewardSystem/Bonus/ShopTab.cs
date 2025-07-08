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
        public List<BonusAsset> bAssetList;
        public List<GameObject> bGameObjectList;



        public ShopTab(GameObject tabGO, Game game)
        {
            this.game = game;
        }

        public void AddBonus(GameObject bonusGO)
        {
            bGameObjectList.Add(bonusGO);
        }
    }
}