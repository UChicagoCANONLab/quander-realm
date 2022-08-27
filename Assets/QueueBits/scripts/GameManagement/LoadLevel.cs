using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QueueBits
{
    public class LoadLevel : MonoBehaviour
    {
        public void loadlevel (string level)
        {
            SceneManager.LoadScene("QB_" + level);
        }
    }
}