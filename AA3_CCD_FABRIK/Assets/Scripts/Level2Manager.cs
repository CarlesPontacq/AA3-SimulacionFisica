using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] private List<ButtonHolder> buttons;
    [SerializeField] private FABRIKSolver solver;
    [SerializeField] private Transform defaultTarget;
    [SerializeField] private GameObject winningArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].GotPressed())
            {
                ChangeTarget(buttons[i].GetTarget());
                return;
            }
        }

        ChangeTarget(defaultTarget);
        winningArea.SetActive(true);
    }

    void ChangeTarget(Transform newTarget)
    {
        if (solver.GetTarget() == newTarget) return;

        solver.SetTarget(newTarget);
        solver.SetSmoothSpeed(0.4f);
        StartCoroutine(SmoothSpeedTimer());
    }

    private IEnumerator SmoothSpeedTimer()
    {
        yield return new WaitForSeconds(2f);
        solver.SetSmoothSpeed(2f);
    }
}
