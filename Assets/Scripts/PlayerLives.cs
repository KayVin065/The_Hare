using UnityEngine;

public class PlayerLives
{
    public int MaxLives { get; private set; }
    public int CurrentLives { get; private set; }

    public PlayerLives(int maxLives)
    {
        MaxLives = maxLives;
        CurrentLives = maxLives;
    }

    public void LoseLife()
    {
        if (CurrentLives <= 0) return;

        CurrentLives--;
    }

    public bool IsDead() 
    {
        return CurrentLives <= 0;
    }

    public void GainLife() 
    {
        CurrentLives = Mathf.Min(CurrentLives++, MaxLives);
    }
}
