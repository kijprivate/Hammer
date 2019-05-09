using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class BlurFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed =2f;

    private BlurOptimized blur;

    private void Awake()
    {
        blur = GetComponent<BlurOptimized>();
        
        if(LevelContainer.MenuHided)
        { blur.enabled = false; }

        EventManager.EventMenuHided += OnMenuHided;
    }

    private void OnMenuHided()
    {
        StartCoroutine(FadeBlur());
    }

    private IEnumerator FadeBlur()
    {
        while (blur.blurSize > Mathf.Epsilon)
        {
            yield return new WaitForEndOfFrame();
            blur.blurSize -= Time.deltaTime*fadeSpeed;
        }
        blur.enabled = false;
    }

    public void DisableBlur()
    {
        blur.enabled = false;
    }

    private void OnDestroy()
    {
        EventManager.EventMenuHided -= OnMenuHided;
    }
}
