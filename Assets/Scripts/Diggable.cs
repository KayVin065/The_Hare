using UnityEngine;

public class Diggable : MonoBehaviour
{
    public int digsRequired = 2;
    public int currentDigs = 0;

    public void Dig() 
    {
        currentDigs++;

        if(currentDigs >= digsRequired) {
            OnFullyDug();
            Destroy(gameObject);
        }
        else {
            Debug.Log("You dug...");
        }
    }

    protected virtual void OnFullyDug()
    {
        Debug.Log("I've been dug up! :(");
    }
}
