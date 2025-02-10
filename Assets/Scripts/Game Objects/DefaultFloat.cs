using UnityEngine;

public class DefaultFloat : MonoBehaviour
{
    public float amplitude = 0.5f; // Distance to move up and down
    public float frequency = 5;  // Speed of the bobbing motion

    // Starting position of the GameObject
    private Vector2 startPosition;

    void Start()
    {
        // Record the starting position of the GameObject
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector2(startPosition.x, startPosition.y + offset);
    }
}
