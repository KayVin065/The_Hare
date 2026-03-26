using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] lifeIcons;

    public void UpdateLives(int currentLives)
    {
        for(int i = 0; i < lifeIcons.Length; i++) {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }

    public void Connect(Player player)
    {
        player.OnLivesChanged += UpdateLives;
    }
}
