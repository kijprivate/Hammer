using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image progressBar;
    private float fillInterval;
    private Hammer hammer;
    void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        progressBar = GetComponent<Image>();
        fillInterval = 1f / LevelContainer.NumberOfNails;
        EventManager.EventNailFinished += OnNailFinished;
    }

    void OnNailFinished()
    {
        progressBar.fillAmount = fillInterval * (hammer.targetIndex + 1);
    }

    private void OnDestroy()
    {
        EventManager.EventNailFinished -= OnNailFinished;
    }
}
