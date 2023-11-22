using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ColorUtil
{
    public static Color Alpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color Intensity(Color color, float intensity)
    {
        return new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a);
    }

    public static Color Lerp(Color from, Color to, float t)
    {
        float r = Mathf.Lerp(from.r, to.r, t);
        float g = Mathf.Lerp(from.g, to.g, t);
        float b = Mathf.Lerp(from.b, to.b, t);
        float a = Mathf.Lerp(from.a, to.a, t);
        return new Color(r, g, b, a);
    }

    public static Color SmoothStep(Color from, Color to, float t)
    {
        float r = Mathf.SmoothStep(from.r, to.r, t);
        float g = Mathf.SmoothStep(from.g, to.g, t);
        float b = Mathf.SmoothStep(from.b, to.b, t);
        float a = Mathf.SmoothStep(from.a, to.a, t);
        return new Color(r, g, b, a);
    }

    public static IEnumerator CrossFade(TMP_Text subject, Color from, Color to, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            subject.color = Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        subject.color = to;
    }

    public static IEnumerator CrossFade(Image subject, Color from, Color to, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            subject.color = Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        subject.color = to;
    }

    public static IEnumerator CrossFade(SpriteRenderer subject, Color from, Color to, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            subject.color = Lerp(from, to, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        subject.color = to;
    }
}
