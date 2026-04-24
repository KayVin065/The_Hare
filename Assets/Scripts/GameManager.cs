using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void TriggerEndGame()
    {
        StartCoroutine(GameCompleteRoutine());
    }

    IEnumerator GameCompleteRoutine()
    {
        Debug.Log("Before waiting...");
        yield return new WaitForSeconds(13f);
        Debug.Log("After waiting...");
        GameComplete();
    }

    private void GameComplete() 
    {
        Debug.Log("Game complete");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}