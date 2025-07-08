using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public class Avatar : MonoBehaviour
    {
        [SerializeField] public Image character;
        [SerializeField] public Image background;

        [SerializeField] private Sprite[] backgroundOptions;
        [SerializeField] private Sprite[] characterOptions;
    }
}