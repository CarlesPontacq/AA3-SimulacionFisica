using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3anager : MonoBehaviour
{
    [SerializeField] private List<ButtonPressed> buttons;
    private bool bothButtonsPressed = false;
    private bool changeScene = false;
    public float Timer = 3f;
    private float currentTimer = 0f;

    [SerializeField] string nextScene;

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(!buttons[i].pressed)
            {
                bothButtonsPressed = false;
                currentTimer = 0f;
                changeScene = false;
                return;
            }
            bothButtonsPressed = true;

        }

        StartCountdown();

        if (changeScene)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    void StartCountdown()
    {
        currentTimer += Time.deltaTime;

        if(currentTimer >= Timer)
        {
            changeScene = true;
        }
    }
}
