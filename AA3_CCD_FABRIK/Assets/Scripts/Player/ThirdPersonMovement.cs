using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float y = 0f;

        if (Input.GetKey(KeyCode.Q)) y = -1f;
        if (Input.GetKey(KeyCode.E)) y = 1f;

        VectorUtils3D rightVector = new VectorUtils3D();
        VectorUtils3D forwardVector = new VectorUtils3D();
        VectorUtils3D upVector = new VectorUtils3D();

        rightVector.AssignFromUnityVector(transform.right);
        forwardVector.AssignFromUnityVector(transform.forward);
        upVector.AssignFromUnityVector(transform.up);

        VectorUtils3D velocity = (rightVector * h + forwardVector * v + upVector * y);
        
        body.linearVelocity = (velocity * speed).GetAsUnityVector();

    }
}
