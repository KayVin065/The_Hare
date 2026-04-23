using UnityEngine;

public class SlidingBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer slidingSprite;
    [SerializeField] private SpriteRenderer regularSprite;
    private float lerpSpeed = 5f;
    public bool sliding = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
        disable regular sprite
        lerp sliding sprite from upright to the 45 degree angle
        maybe spawn particles?? <- do later
        

    */

}
