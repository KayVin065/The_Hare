using UnityEngine;

public class Edible : MonoBehaviour
{
    public AudioClip eatSound;

    public void Eat() 
    {
        Debug.Log("I've been eaten!");
        AudioManager.instance.PlaySFX(eatSound);
        Destroy(gameObject);
    }
}
