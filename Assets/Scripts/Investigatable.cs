using UnityEngine;

public class Investigatable : MonoBehaviour
{
    public bool investigated = false;

    public Sprite usedSprite;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Investigate()
    {
        if(investigated) { return; }

        investigated = true;

        Debug.Log("I've been investigated!");
        sr.sprite = usedSprite;
    }
}
