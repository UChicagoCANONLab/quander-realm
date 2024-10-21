using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QueueBits
{
    public class LoadLevel : MonoBehaviour
    {
        // Initializes MAXLEVEL
#if LITE_VERSION
        int MAXLEVEL = 8;
#else
        int MAXLEVEL = 15;
#endif

        // Loads level chosen by number
        public void loadLevel(int level) {
            GameManager.LEVEL = level;
            GameManager.Save();
            Debug.Log(level);

            SceneManager.LoadScene("QB_Level");
        }

        // Loads next nevel unless at max level
        public void nextLevel() {
            if (GameManager.LEVEL < MAXLEVEL) {
                GameManager.LEVEL += 1;
                GameManager.Save();

                SceneManager.LoadScene("QB_Level");
            }
            else {
                SceneManager.LoadScene("QB_LevelSelect");
            }         
        }

        // Functions that will always load the same scene
        // Loads Level Select
        public void loadLevelSelect() {
            SceneManager.LoadScene("QB_LevelSelect");
        }
        // Loads Home page
        public void loadMenu() {
            SceneManager.LoadScene("QB_Home");
        }
        // Reloads current scene without changing level
        public void restart() {
            SceneManager.LoadScene("QB_Level");
        }
    }
}