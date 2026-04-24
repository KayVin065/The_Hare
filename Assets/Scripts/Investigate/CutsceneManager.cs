using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image displayImage;
    public GameObject cutsceneCanvas;

    public GameObject textbox;
    public TMPro.TextMeshProUGUI textUI;

    public GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void PlayCutscene(Sprite[] frames, float duration, float waitTime)
    {
        // Disable player movement while cutscene is playing
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(PlaySequence(frames, duration, waitTime));
    }

    IEnumerator PlaySequence(Sprite[] frames, float duration, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        cutsceneCanvas.SetActive(true);

        foreach (Sprite frame in frames)
        {
            displayImage.sprite = frame;
            yield return new WaitForSeconds(duration);
        }
        EndCutscene();
    }

    void EndCutscene()
    {
        cutsceneCanvas.SetActive(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ShowText(string message)
    {
        textbox.SetActive(true);
        textUI.text = message;
    }
}
