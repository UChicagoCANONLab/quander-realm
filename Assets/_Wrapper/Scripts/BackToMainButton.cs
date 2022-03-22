using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Wrapper
{
    public class BackToMainButton : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("S_MainScene"));
        }
    }
}