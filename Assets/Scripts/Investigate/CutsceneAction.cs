using UnityEngine;

[CreateAssetMenu(menuName = "Investigation/Cutscene Action")]
public class CutsceneAction : InvestigateAction
{
    public Sprite[] frames;
    public float frameDuration = 2f;

    public override void Execute(CutsceneManager manager)
    {
        manager.PlayCutscene(frames, frameDuration);
    }
}
