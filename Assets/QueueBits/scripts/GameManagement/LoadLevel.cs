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


        public void setCurrentLevel(int lev) {
            this.currentLevel = lev;
        }
        

        /* public void loadlevel (string level)
        {
            // GameManager.saveData.LEVEL = int.Parse(level);
#if LITE_VERSION
            if (level=="Level9") {
                level = "LevelSelect";
            }
#endif
            SceneManager.LoadScene("QB_" + level);
        } */

        public void loadLevel(int level) {
            GameManager.LEVEL = level;
            GameManager.Save();

            SceneManager.LoadScene("QB_Level");
        }


        public void nextLevel() {
            if (currentLevel < MAXLEVEL) {
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
        // Reloads current scenes
        public void restart() {
            SceneManager.LoadScene("QB_Level");
        }
    }
}