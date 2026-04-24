using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject textbox;
    //public TMPro.TextMeshProUGUI textUI;

    void Start()
    {
        textbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Player"))
        {
            textbox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D obj) 
    {
        if(obj.CompareTag("Player"))
        {
            textbox.SetActive(false);
        }
    }
}
