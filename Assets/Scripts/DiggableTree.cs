using UnityEngine;

public class DiggableTree : Diggable
{
    [SerializeField] private GameObject tree;
    private TreeBehavior tb;

    private void Awake()
    {
        tb = tree.GetComponent<TreeBehavior>();
    }

    protected override void OnFullyDug()
    {
        tb.StartFalling();
    }
}
