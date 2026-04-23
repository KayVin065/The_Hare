using UnityEngine;

public abstract class InvestigateAction : ScriptableObject
{
    public abstract void Execute(CutsceneManager manager);
}
