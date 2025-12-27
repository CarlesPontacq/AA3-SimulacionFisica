using UnityEngine;
using UnityEngine.UI;
public class GameControllerUI : MonoBehaviour
{
    [Header("Referencias")]
    public CCDSolver ccdSolver;

    [Header("UI Controls")]
    public Slider iterationsSlider;
    public Text iterationsText;

    public Slider toleranceSlider;
    public Text toleranceText;

    public Button activationButton;
    private bool isActive = false;

    void Start()
    {
        
        if (ccdSolver != null)
        {
            iterationsSlider.value = ccdSolver.iterations;
            toleranceSlider.value = ccdSolver.tolerance;
            UpdateLabels();
        }

        // Escuchar cambios
        iterationsSlider.onValueChanged.AddListener(OnIterationsChange);
        toleranceSlider.onValueChanged.AddListener(OnToleranceChange);
        activationButton.onClick.AddListener(OnToggleClick);
    }

    void OnIterationsChange(float value)
    {
        ccdSolver.iterations = (int)value;
        UpdateLabels();
    }

    void OnToleranceChange(float value)
    {
        ccdSolver.tolerance = value;
        UpdateLabels();
    }

    void OnToggleClick()
    {
        isActive = !isActive;
        //ccdSolver.ToggleSimulation(isActive);
        
    }

    void UpdateLabels()
    {
        iterationsText.text = "Iteraciones: " + ccdSolver.iterations;
        toleranceText.text = "Tolerancia: " + ccdSolver.tolerance.ToString("F3");
    }
}
