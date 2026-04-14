using UnityEngine;
using System.Collections;

public class TreeBehavior : MonoBehaviour
{
    private bool isFalling = false;
    private bool hasLanded = false;

    [SerializeField] private GameObject[] tree;
    [SerializeField] private ParticleSystem leavesParticles;
    private Rigidbody2D rb;

    [SerializeField] private float fallPushForce = 2f; 
    [SerializeField] private float fallTorque = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularDamping = 2f;
        rb.linearDamping = 0.2f;
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Enemy" && !hasLanded && isFalling)
        {
            hasLanded = true;
            StartCoroutine(FreezeAfterLanding());
        }
    }

    public void StartFalling()
    {
        if (isFalling) return;

        isFalling = true;

        if (leavesParticles != null)
        {
            Debug.Log("Leaves particles not null Start Falling");
            // leavesParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // leavesParticles.Play();
            SpawnLeaves();
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        rb.constraints = RigidbodyConstraints2D.None;

        Vector2 push = new Vector2(1f, 0.2f).normalized;
        rb.AddForce(push * fallPushForce, ForceMode2D.Impulse);
        rb.AddTorque(-fallTorque, ForceMode2D.Impulse);
    }

    private void FallenDown()
    {
        if (leavesParticles != null)
        {
            Debug.Log("Leaves particles not null Fallen Down");
            SpawnLeaves();
            // leavesParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // leavesParticles.Play();
        }

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f; 
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        for(int i = 0; i < tree.Length; i++)
        {
            tree[i].SetActive(false);
        }

        // make it explode into particles
    }

    private IEnumerator FreezeAfterLanding()
    {
        yield return new WaitForSeconds(0.3f);
        FallenDown();
    }

    private void SpawnLeaves()
    {
        if (leavesParticles == null) return;

        Instantiate(
            leavesParticles,
            transform.position,        // where the tree is
            Quaternion.identity
        );
    }
}
