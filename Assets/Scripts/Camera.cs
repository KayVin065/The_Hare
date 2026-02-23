using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos;

    [SerializeField]
    private float minX, maxX;

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

        if(tempPos.x < minX) {
            tempPos.x = minX;
        }

        if(tempPos.x > maxX) {
            tempPos.x = maxX;
        }

        transform.position = tempPos;
        
    }
}
