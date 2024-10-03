using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Circuits 
{
    public class Circuits_DialogueBehavior : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

            int offset = 0;

            GameData.InitCircuitsSaveData();
            // Debug.Log(GameData.getCurrLevel());

            if (SceneManager.GetActiveScene().name == "Circuits_Title") {
                Wrapper.Events.StartDialogueSequence?.Invoke("CT_Intro");
                return;
            }

            switch (GameData.getCurrLevel() - offset)
            {
                case 0:
                    // Wrapper.Events.StartDialogueSequence?.Invoke("CT_Intro");
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                case 3:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                case 5:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                case 7:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                case 14:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                case 15:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                //    return "DialogC-03B";
                case 22:
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                    break;
                //    return "DialogC-04";
                case 25:
                    Wrapper.Events.StartDialogueSequence?.Invoke("CT_Level25");
                    break;
                //    return "DialogC-04";
                default:
                    // SceneManager.LoadScene("CircuitsLevelScene");
                    // SceneManager.LoadScene("Circuits_Title");
                    break;
            }
        }

        private void OnEnable()
        {
            Wrapper.Events.DialogueSequenceEnded += UnPauseGame;
        }

        private void OnDisable()
        {
            Wrapper.Events.DialogueSequenceEnded -= UnPauseGame;
        }

        private void UnPauseGame()
        {
            if (SceneManager.GetActiveScene().name == "Circuits_Title") {
                return;
            }
            else {
                var level = GameData.getCurrLevel();
                if (level < 1)
                {
                    SceneManager.LoadScene("CircuitsLevelSceneTutorial");
                }
                else
                {
                    SceneManager.LoadScene("CircuitsLevelScene");
                }
            }
        }

    }
}