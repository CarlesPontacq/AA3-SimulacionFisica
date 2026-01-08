using TMPro;
using UnityEngine;

public class IKDebugPanel : MonoBehaviour
{
    [SerializeField] private MonoBehaviour solverBehaviour;
    private IIKSolverDebug solver;

    public bool showColumn = true;

    [Header("UI")]
    public TMP_Text algorithmText;
    public TMP_Text iterationText;
    public TMP_Text distanceText;

    void Awake()
    {
        if (!showColumn)
        {
            algorithmText.enabled = false;
            iterationText.enabled = false;
            distanceText.enabled = false;
            return;
        }
        else
        {
            algorithmText.enabled = true;
            iterationText.enabled = true;
            distanceText.enabled = true;
        }

        solver = solverBehaviour as IIKSolverDebug;

        if (solver == null)
        {
            Debug.LogError("El solver no implementa IIKSolverDebug");
        }
    }

    void Update()
    {
        if (solver == null) return;

        algorithmText.text = $"Algoritmo: {solver.AlgorithmName}";
        iterationText.text = $"Iteraciones (frame): {solver.LastFrameIterations}";
        distanceText.text = $"Distancia Target: {solver.CurrentDistanceToTarget:F4}";
    }
}
