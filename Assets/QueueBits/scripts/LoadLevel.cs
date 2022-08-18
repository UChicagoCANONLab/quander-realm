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
            SceneManager.LoadScene(level);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}