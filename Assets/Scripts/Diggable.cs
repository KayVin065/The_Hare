using UnityEngine;

public class Diggable : MonoBehaviour
{
    public int digsRequired = 2;
    public int currentDigs = 0;

    public AudioClip digSound;

    public virtual void Dig() 
    {
        currentDigs++;

        if(currentDigs >= digsRequired) {
            OnFullyDug();
            Destroy(gameObject);
        }
        else {
            Debug.Log("You dug...");
        }

        AudioManager.instance.PlaySFX(digSound);
    }

    protected virtual void OnFullyDug()
    {
        Debug.Log("I've been dug up! :(");
    }
}
