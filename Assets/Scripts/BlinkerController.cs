using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkerController : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }

    private void Update()
    {
        if (text.color.a <= 0.25f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInText());
        }

        if (text.color.a >= 1.0f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutText());
        }
    }

    private IEnumerator FadeInText()
    {
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.01f);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator FadeOutText()
    {
        while (text.color.a > 0.25f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f);
            yield return new WaitForSeconds(0);
        }
    }
}
