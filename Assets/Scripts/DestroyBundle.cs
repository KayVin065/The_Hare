using UnityEngine;

public class DestroyBundle : MonoBehaviour
{
    public AudioClip digSound;
    
    private void OnTriggerExit2D(Collider2D obj) 
    {
        if(obj.CompareTag("Player"))
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(digSound);
        }
    }
}
