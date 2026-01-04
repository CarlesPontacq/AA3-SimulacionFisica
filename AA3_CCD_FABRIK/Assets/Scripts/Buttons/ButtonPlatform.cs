using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    public float amplitude = 2f;
    public float frequency = 1f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;        
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + Vector3.up * offset;
    }
}
