using UnityEngine;

public class DiggableTree : Diggable
{
    [SerializeField] private GameObject tree;
    private TreeBehavior tb;

    private void Awake()
    {
        tb = tree.GetComponent<TreeBehavior>();
    }

    public override void Dig() 
    {
        currentDigs++;

        if(currentDigs >= digsRequired) {
            OnFullyDug();
            Destroy(gameObject);
        }
        else {
            tb.SpawnLeaves();
        }

        AudioManager.instance.PlaySFX(digSound);
    }


    protected override void OnFullyDug()
    {
        tb.StartFalling();
    }
}
