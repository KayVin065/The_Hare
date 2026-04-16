using UnityEngine;
using System.Collections;

public class AvalancheController : MonoBehaviour
{
    public LayerMask groundMask;
    public float speed = 3f;
    public float acceleration = 0.15f;
    public float maxSpeed = 12f;

    private Vector2 moveDirection;
    public float rayDistance = 10f;

    public GameObject debrisPrefab;
    public float debrisSpawnRate = 0.08f;
    private float debrisTimer;

    private bool isActive = false;

    // Child Objects;
    public AvalancheSpawner spawner;
    private ParticleSystem particles;

    void Awake()
    {
        gameObject.SetActive(false);
        particles = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if(!isActive) return;

        debrisTimer -= Time.deltaTime;

        if(debrisTimer <= 0f)
        {
            SpawnDebris();
            debrisTimer = debrisSpawnRate;
        }

        CalculateSlopeDirection();

        speed += acceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void Activate() 
    { 
        isActive = true; 
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;

        spawner.StopSpawning();
        particles.Stop();
    }

    void CalculateSlopeDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            rayDistance,
            groundMask
        );

        if (hit.collider == null)
        {
            moveDirection = Vector2.right;
            return;
        }

        Vector2 normal = hit.normal;

        moveDirection = new Vector2(normal.y, -normal.x);

        if (moveDirection.y > 0)
            moveDirection = -moveDirection;
    }

    void SpawnDebris()
    {
        Vector3 spawnPos = transform.position;
        Vector3 forward = moveDirection.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        spawnPos -= forward * 2f;
        spawnPos += right * Random.Range(-3f, 3f);
        spawnPos.y -= 0.4f;

        GameObject obj = Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
        obj.GetComponent<DebrisMotion>().Launch(forward);
    }
}
