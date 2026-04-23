using UnityEngine;

public class Investigatable : MonoBehaviour
{
    public bool investigated = false;
    public CutsceneManager manager;

    public InvestigateAction action;

    public AudioClip investigateSound;

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
        AudioManager.instance.PlaySFX(investigateSound);

        action.Execute(manager);
        sr.sprite = usedSprite;
    }
}
