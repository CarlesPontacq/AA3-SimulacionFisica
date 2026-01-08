using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    public Transform button;
    public float pressedButtonOffset = 0.25f;
    public float pressSpeed = 10f;

    public bool pressed = false;
    private bool pressedAtLeastOnce = false;
    private VectorUtils3D startLocalPos;
    private VectorUtils3D pressedLocalPos;

    [SerializeField] Material unpressedMaterial;
    [SerializeField] Material pressedMaterial;
    [SerializeField] private MeshRenderer buttonMaterial;

    void Start()
    {
        startLocalPos = VectorUtils3D.ToVectorUtils3D(button.localPosition);

        VectorUtils3D forward = VectorUtils3D.ToVectorUtils3D(button.forward).Normalize();

        pressedLocalPos = startLocalPos - forward * pressedButtonOffset;
    }

    void Update()
    {
        VectorUtils3D currentPos = VectorUtils3D.ToVectorUtils3D(button.localPosition);

        VectorUtils3D targetPos = pressed ? pressedLocalPos : startLocalPos;

        VectorUtils3D newPos = VectorUtils3D.LERP(currentPos, targetPos, Time.deltaTime * pressSpeed);

        button.localPosition = newPos.GetAsUnityVector();

        if(buttonMaterial != null)
        {
            if(pressed)
            {
                buttonMaterial.material = pressedMaterial;
            }
            else
            {
                buttonMaterial.material = unpressedMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("endEffector"))
        {
            pressed = true;
            pressedAtLeastOnce = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("endEffector"))
            pressed = false;
    }

    public bool GotPressedAtLeastOnce()
    {
        return pressedAtLeastOnce;
    }
}
