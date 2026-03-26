using UnityEngine;

public class DiggableDoor : Diggable
{
    protected override void OnFullyDug()
    {
        Debug.Log("Entered door");
        LevelComplete();
    }

    private void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE: You made it home :)");
        // call game manager to switch to next level
    }
}
