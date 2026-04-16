using UnityEngine;
using System.Collections;

public class AvalancheSpawner : MonoBehaviour
{
    public SnowChunkPool pool;
    public float spawnRate = 0.08f;
    public float spreadY = 6f;
    public float chunkSpeed = 9f;

    public LayerMask groundMask;
    public float rayDistance = 10f;

    private Vector2 slopeDirection;
    private bool spawning = true;

    private Coroutine spawnRoutine;

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawningRoutine());
    }

    IEnumerator SpawningRoutine()
    {
        while(true)
        {
            if(spawning)
            {
                CalculateSlopeDirection();
                SpawnChunk();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnChunk()
    {
        Vector3 pos = transform.position;
        pos.y  += Random.Range(-spreadY, spreadY);
        
        SnowChunk chunk = pool.GetChunk();
        chunk.transform.position = pos;

        chunk.Launch(slopeDirection, chunkSpeed);
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
            slopeDirection = Vector2.right;
            return;
        }

        Vector2 normal = hit.normal;

        slopeDirection = new Vector2(normal.y, -normal.x);

        if (slopeDirection.y > 0)
            slopeDirection = -slopeDirection;
    }

    public void StartSpawning() { spawning = true; }
    public void StopSpawning() { spawning = false; }
}
