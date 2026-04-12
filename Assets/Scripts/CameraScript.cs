using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos;

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;

    [SerializeField] private float stepHeight = 6f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float vertLerpSpeed = 8f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if(!player)
            return;

        tempPos = transform.position;
        tempPos.x = player.position.x;

        tempPos.x = Mathf.Clamp(tempPos.x, minX, maxX);

        // Calculating camera "steps"
        float zone = Mathf.Floor(player.position.y / stepHeight);

        float targetY = zone * stepHeight + yOffset;

        float clampedTargetY = Mathf.Clamp(targetY, minY, maxY);
        tempPos.y = Mathf.Lerp(transform.position.y, clampedTargetY, vertLerpSpeed * Time.deltaTime);

        transform.position = tempPos;
    }
}
