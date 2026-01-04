using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    public float amplitude = 2f;
    public float frequency = 1f;

    VectorUtils3D startPos;

    void Start()
    {
        startPos = VectorUtils3D.ToVectorUtils3D(transform.position);        
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;

        VectorUtils3D newPos = startPos + VectorUtils3D.up * offset;

        transform.position = newPos.GetAsUnityVector();
    }
}
