using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private Renderer targetRenderer;
    private AudioSource SFXAudio;

    private void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        SFXAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (targetRenderer.material.color.a <= 0.5f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInTargetRenderer());
        }

        if (targetRenderer.material.color.a >= 1.0f)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutTargetRenderer());
        }
    }

    private IEnumerator FadeInTargetRenderer()
    {
        while (targetRenderer.material.color.a < 1.0f)
        {
            targetRenderer.material.color = new Color(targetRenderer.material.color.r, targetRenderer.material.color.g, targetRenderer.material.color.b, targetRenderer.material.color.a + 0.01f);
            yield return new WaitForSeconds(0);
        }
    }

    private IEnumerator FadeOutTargetRenderer()
    {
        while (targetRenderer.material.color.a > 0.5f)
        {
            targetRenderer.material.color = new Color(targetRenderer.material.color.r, targetRenderer.material.color.g, targetRenderer.material.color.b, targetRenderer.material.color.a - 0.01f);
            yield return new WaitForSeconds(0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ResetTargetPosition();
        SFXAudio.Play();
    }

    public void ResetTargetPosition()
    {
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.2f, 0.9f), 2.0f));
    }
}
