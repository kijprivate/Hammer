using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialItemShop : MonoBehaviour
{
    [SerializeField] int specialItemIndex;
    [SerializeField] int cost;
    [SerializeField] Image childImage;
    [SerializeField] Text childText;

    private void Awake()
    {
        if (!PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex))
        {
            GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
            if (childText)
            {
                childText.text = cost.ToString();
            }
        }
        else
        {
            GetComponent<Image>().color = Color.white;
            if (childText)
            {
                childText.enabled = false;
            }
            if (childImage)
            {
                childImage.enabled = false;
            }
        }
    }

    public void OnClick()
    {
        if (!PlayerPrefsManager.IsSpecialItemUnlocked(specialItemIndex) && PlayerPrefsManager.GetNumberOfCoins() >= cost)
        {
            PlayerPrefsManager.UnlockSpecialItem(specialItemIndex);
            PlayerPrefsManager.SetNumberOfCoins(PlayerPrefsManager.GetNumberOfCoins() - cost);
            if (childText)
            {
                childText.enabled = false;
            }
            if (childImage)
            {
                childImage.enabled = false;
            }
            EventManager.RaiseEventCoinsSubstracted();
        }
    }
}
