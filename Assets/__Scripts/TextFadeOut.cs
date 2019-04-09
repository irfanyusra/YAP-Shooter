using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TextFadeOut : MonoBehaviour
{
    private float fadeOutTime = 2.5f;

    public void Awake() // handles the appear and fadeout functions when it's instantiated
    {
        Appear();
        FadeOut();
    }

    public void FadeOut() { // starts coroutine for fading out
        StartCoroutine(FadeOutRoutine());
    }

    public void Appear() // grabs the text component and sets it to white
    {
        Text text = GetComponent<Text>();
        text.color = Color.white;
    }

    private IEnumerator FadeOutRoutine() { //fades the text using color.lerp
        Text text = GetComponent<Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        Destroy(gameObject); // destroys the object when done
    }
}