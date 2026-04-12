using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos = Vector3.zero;
    private Rigidbody2D playerRB;
    
    public float moveSpeed;
    public float lookAheadDist = 1f, lookAheadSpeed = 3f;
    private float lookOffset;

    [SerializeField] private float minX, maxX, minY, maxY;
    [SerializeField] private float offset;
    [SerializeField] private float velThreshold = 0.1f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerRB = player.GetComponent<Rigidbody2D>();
        tempPos = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if(!player)
            return;

        //tempPos = transform.position;

        //tempPos.x = player.position.x;
        tempPos.y = player.position.y + offset;

        float rbx = playerRB.linearVelocity.x;

        if(Mathf.Abs(rbx) > velThreshold)
        {
            if(rbx > 0f)
                lookOffset = Mathf.Lerp(lookOffset, lookAheadDist, lookAheadSpeed * Time.deltaTime);
            else
                lookOffset = Mathf.Lerp(lookOffset, -lookAheadDist, lookAheadSpeed * Time.deltaTime);
        }

        tempPos.x = player.position.x + lookOffset;

        tempPos.x = Mathf.Clamp(tempPos.x, minX, maxX);
        tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, tempPos, moveSpeed * Time.deltaTime);
    }
}
