using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    public class BadgeHolder : MonoBehaviour
    {
        [SerializeField] public List<Badge> badges; 
        [SerializeField] public GameObject badgePrefab;
        [SerializeField] public CriteriaType type;
        [SerializeField] public GameObject holder;


        public void AddBadge(BadgeAsset bAsset, string ID)
        {
            if (bAsset.criteriaType == this.type)
            {
                GameObject bObject = Instantiate(badgePrefab, holder.transform);
                bObject.name = ID;
                bObject.GetComponent<Badge>().InitBadge(bAsset);

                badges.Add(bObject.GetComponent<Badge>());
                holder.GetComponent<RectTransform>().sizeDelta = new Vector2(badges.Count*825, 450);
            }                
        }

        /* public Badge[] GetBadges()
        {
            List<Badge> badgeList = new List<Badge>();
            foreach(Transform badgeChild in holder.transform)
            {
                badgeList.Add(badgeChild.GetComponent<Badge>());
            }
            return(badgeList.ToArray());
        } */
    
    
    
    }
}