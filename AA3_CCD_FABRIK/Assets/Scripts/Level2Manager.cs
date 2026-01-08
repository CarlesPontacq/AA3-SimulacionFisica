using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] private List<ButtonHolder> buttons;
    [SerializeField] private List<GameObject> lasers;
    [SerializeField] private FABRIKSolver solver;
    [SerializeField] private Transform defaultTarget;

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

        ActivateVictory();
    }

    void ActivateVictory()
    {
        ChangeTarget(defaultTarget);

        for (int i = 0; i < lasers.Count; i++)
        {
            lasers[i].SetActive(false);
        }

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
