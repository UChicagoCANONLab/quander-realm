using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Labyrinth
{
    public class DialogueManager : MonoBehaviour
    {
        private Dictionary<int, bool> levelDialogue = new Dictionary<int, bool>()
        { {0, true}, {1, true}, {5, true}, {6, true}, {10, true}, {11, true}, {15, true} };


        public void checkDialogue(int level) {
            if (levelDialogue[level]) {
                doDialogue(level);
            }
            return;
        }

        private void doDialogue(int level) {

        }

        public void doDialogueString(string level) {
            Wrapper.Events.StartDialogueSequence?.Invoke(level);
        }

    }
}