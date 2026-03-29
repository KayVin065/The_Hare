using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] lifeIcons;
    [SerializeField] private GameObject[] deathIcons;

    public void UpdateLives(int currentLives)
    {
        for(int i = 0; i < lifeIcons.Length; i++) {
            bool hasLife = i < currentLives;

            lifeIcons[i].SetActive(hasLife);
            deathIcons[i].SetActive(!hasLife);
        }
    }

    public void Connect(Player player)
    {
        player.OnLivesChanged += UpdateLives;
    }
}
