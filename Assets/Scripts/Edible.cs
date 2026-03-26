using UnityEngine;

public class Edible : MonoBehaviour
{
    public void Eat() 
    {
        Debug.Log("I've been eaten!");
        Destroy(gameObject);
    }
}
