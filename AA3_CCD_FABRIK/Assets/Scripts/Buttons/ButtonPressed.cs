using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    public Transform button;
    public float pressedButtonOffset = 0.25f;
    public float pressSpeed = 10f;

    private bool pressed = false;
    private Vector3 startLocalPos;
    private Vector3 pressedLocalPos;

    void Start()
    {
        startLocalPos = button.localPosition;

        pressedLocalPos = startLocalPos - button.forward * pressedButtonOffset;
    }

    void Update()
    {
        Vector3 targetPos = pressed ? pressedLocalPos : startLocalPos;

        button.localPosition = Vector3.Lerp(
            button.localPosition,
            targetPos,
            Time.deltaTime * pressSpeed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("endEffector"))
            pressed = true;

        Debug.Log("Entered");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("endEffector"))
            pressed = false;

        Debug.Log("Exit");
    }
}
