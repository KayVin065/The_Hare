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
    //private PlayerInput playerInput;
    private Rigidbody2D rb;

    void Start()
    {
        //playerInput = player.GetComponent<PlayerInput>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void PlayCutscene(Sprite[] frames, float duration)
    {
        cutsceneCanvas.SetActive(true);

        // Disable player movement while cutscene is playing
        //playerInput.Player.Disable();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(PlaySequence(frames, duration));
    }

    IEnumerator PlaySequence(Sprite[] frames, float duration)
    {
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
        //playerInput.Player.Enable();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ShowText(string message)
    {
        textbox.SetActive(true);
        textUI.text = message;
    }
}
