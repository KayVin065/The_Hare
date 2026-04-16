using UnityEngine;

public class AvalancheStartTrigger : MonoBehaviour
{
    public AvalancheController avalanche;

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Player"))
        {
            avalanche.Activate();
            gameObject.SetActive(false);
        }
    }
}
