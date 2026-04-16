using UnityEngine;

public class AvalancheKillTrigger : MonoBehaviour
{
    [SerializeField] private AvalancheController avalanche;
    
    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Avalanche"))
        {
            avalanche.Deactivate();
        }
    }
}
