using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class BlurFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed =2f;

    private Image blur;
    private float temp=1f;

    private void Awake()
    {
        blur = GetComponent<Image>();
        
        if(LevelContainer.MenuHided)
        { blur.enabled = false; }

        EventManager.EventMenuHided += OnMenuHided;

    }

    private IEnumerator FadeBlur()
    {
        while (blur.color.a > Mathf.Epsilon)
        {
            yield return new WaitForEndOfFrame();
            temp -= Time.deltaTime*fadeSpeed;
            blur.color = new Color(1f, 1f, 1f, temp);
        }
        blur.enabled = false;
    }

    private void OnMenuHided()
    {
        Fade();
    }

    public void Fade()
    {
        StartCoroutine(FadeBlur());
    }

    private void OnDestroy()
    {
        EventManager.EventMenuHided -= OnMenuHided;
    }
}
