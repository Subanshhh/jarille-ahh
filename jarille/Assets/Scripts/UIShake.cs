using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    public static UIShake Instance;

    private RectTransform rect;
    private Vector3 originalPos;

    void Awake()
    {
        Instance = this;
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            rect.anchoredPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
    }
}