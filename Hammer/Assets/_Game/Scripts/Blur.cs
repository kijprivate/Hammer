using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blur : MonoBehaviour
{
    [SerializeField] float fadeSpeed =2f;

    private Image rend;
    private float blurStrength = 1.5f;
    private void Awake()
    {
        rend = GetComponent<Image>();
        if (!LevelContainer.MenuHided)
        {
            rend.material.SetFloat("_Size", blurStrength);
        }

        EventManager.EventMenuHided += OnMenuHided;
    }

    private void OnMenuHided()
    {
        StartCoroutine(FadeBlur());
    }

    private IEnumerator FadeBlur()
    {
        while (blurStrength > Mathf.Epsilon)
        {
            yield return new WaitForEndOfFrame();
            blurStrength -= Time.deltaTime*fadeSpeed;
            rend.material.SetFloat("_Size", blurStrength);
        }
    }

    private void OnDestroy()
    {
        EventManager.EventMenuHided -= OnMenuHided;
    }
}
