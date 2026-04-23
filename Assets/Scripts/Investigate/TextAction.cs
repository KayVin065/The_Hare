using UnityEngine;

[CreateAssetMenu(menuName = "Investigation/Text Action")]
public class TextAction : InvestigateAction
{
    [TextArea]
    public string message;

    public override void Execute(CutsceneManager manager)
    {
        manager.ShowText(message);
    }
}
