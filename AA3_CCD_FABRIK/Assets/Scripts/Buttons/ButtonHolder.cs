using UnityEngine;

public class ButtonHolder : MonoBehaviour
{
    [SerializeField] ButtonPressed button;
    [SerializeField] Material pressedMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    bool pressed = false;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed && button.GotPressedAtLeastOnce())
        {
            meshRenderer.material = pressedMaterial;
            pressed = true;
        }
    }

    public bool GotPressed()
    {
        return pressed;
    }

    public Transform GetTarget()
    {
        return button.transform;
    }
}
