using UnityEngine;

public class DiggableDoor : Diggable
{
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject player;

    public CutsceneManager manager;
    public InvestigateAction action = null;

    void Awake()
    {
        winText.SetActive(false);
    }

    protected override void OnFullyDug()
    {
        Debug.Log("Entered door");
        LevelComplete();
    }

    private void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE: You made it home :)");
        winText.SetActive(true);
        player.SetActive(false);

        // for triggering cutscenes post-level
        if(action != null)
            action.Execute(manager);
        
        // call game manager to switch to next level        
    }
}
