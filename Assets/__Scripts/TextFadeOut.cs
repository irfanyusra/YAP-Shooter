using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TextFadeOut : MonoBehaviour
{
    private float fadeOutTime = 2.5f;

    public void Awake()
    {
        Appear();
        FadeOut();
    }

    public void FadeOut() {
        StartCoroutine(FadeOutRoutine());
    }

    public void Appear()
    {
        Text text = GetComponent<Text>();
        text.color = Color.white;
    }

    private IEnumerator FadeOutRoutine() {
        Text text = GetComponent<Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        Destroy(gameObject);
    }
}