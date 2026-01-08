using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningArea : MonoBehaviour
{
    [SerializeField] string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
