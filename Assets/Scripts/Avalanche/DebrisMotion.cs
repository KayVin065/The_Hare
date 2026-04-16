using UnityEngine;

public class DebrisMotion : MonoBehaviour
{
    Vector2 moveDir;
    float speed;
    float life;
    bool settled = false;

    public void Launch(Vector2 slopeDir)
    {
        // small cone spread around slope direction
        float spread = Random.Range(-10f, 10f);
        moveDir = Quaternion.Euler(0, 0, spread) * slopeDir;

        // never allow upward movement (kills the arc!)
        moveDir.y = Mathf.Min(moveDir.y, -0.15f);

        speed = Random.Range(6f, 9f);
        life = Random.Range(2.5f, 4f);
    }

    void Update()
    {
        if (settled) return;

        // accelerate downhill so it feels heavy
        speed += 10f * Time.deltaTime;

        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        life -= Time.deltaTime;
        if (life <= 0f)
            Settle();
    }

    void Settle()
    {
        settled = true;

        // once settled it becomes a platform obstacle
        enabled = false;
    }
}
