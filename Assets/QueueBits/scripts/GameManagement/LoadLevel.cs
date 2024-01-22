using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QueueBits
{
    public class LoadLevel : MonoBehaviour
    {
        public int currentLevel;
#if LITE_VERSION
        int MAXLEVEL = 8;
#else
        int MAXLEVEL = 15;
#endif


        public void loadlevel (string level)
        {
#if LITE_VERSION
            if (level=="Level9") {
                level = "LevelSelect";
            }
#endif
            SceneManager.LoadScene("QB_" + level);
        }

        public void restart() {
            SceneManager.LoadScene($"QB_Level{currentLevel}");
        }

        public void nextLevel() {
            if (currentLevel < MAXLEVEL) {
                SceneManager.LoadScene($"QB_Level{currentLevel+1}");
            }
            else {
                SceneManager.LoadScene("QB_LevelSelect");
            }
            
        }
    }
}