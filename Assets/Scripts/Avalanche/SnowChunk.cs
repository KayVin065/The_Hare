using UnityEngine;

public class SnowChunk : MonoBehaviour
{
    public float speed = 8f;
    private Vector2 moveDirection;

    public void Launch(Vector2 dir, float newSpeed)
    {
        moveDirection = dir.normalized;
        speed = newSpeed;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
