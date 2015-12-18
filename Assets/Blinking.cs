using UnityEngine;
using System.Collections;

public class Blinking : MonoBehaviour
{
    public float blinkTime;
    public float frequency;

    private Renderer sprite;

    void Start()
    {
        OnBlinkActivate();
    }


    public void OnBlinkActivate()
    {
        StartCoroutine(DoStopBlink());
        StartCoroutine(DoBlink());

    }

    IEnumerator DoBlink()
    {
        while (true)
        { 
            yield return new WaitForSeconds(1f / frequency);

            sprite = GetComponent<Renderer>();
            sprite.enabled = !sprite.enabled;
        }
    }

    IEnumerator DoStopBlink()
    {
        yield return new WaitForSeconds(blinkTime);

        StopAllCoroutines();
        sprite.enabled = true;
    }
}