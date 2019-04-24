using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerSpriteShop : MonoBehaviour
{
    [SerializeField] int hammerIndex;
    [SerializeField] int hammerCost;
    [SerializeField] Image childImage;
    [SerializeField] Text childText;

    private void Awake()
    {
        if(!PlayerPrefsManager.IsHammerUnlocked(hammerIndex))
        {
            GetComponent<Image>().color = Color.grey;
            if (childText)
            {
                childText.text = hammerCost.ToString();
            }
        }
        else
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 180f / 255f);
            if(childText)
            {
                childText.enabled = false;
            }
            if (childImage)
            {
                childImage.enabled = false;
            }
        }

        if(PlayerPrefsManager.GetChosenHammer()==hammerIndex)
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    private void Update()
    {
        if (PlayerPrefsManager.GetChosenHammer() == hammerIndex)
        {
            GetComponent<Image>().color = Color.white;
        }
        else if(PlayerPrefsManager.IsHammerUnlocked(hammerIndex) && ! (PlayerPrefsManager.GetChosenHammer() == hammerIndex))
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 180f / 255f);
        }
    }

    public void OnClick()
    {
        if(!PlayerPrefsManager.IsHammerUnlocked(hammerIndex) && PlayerPrefsManager.GetNumberOfCoins() >= hammerCost)
        {
            PlayerPrefsManager.UnlockHammer(hammerIndex);
            PlayerPrefsManager.SetNumberOfCoins(PlayerPrefsManager.GetNumberOfCoins() - hammerCost);
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
        else if(PlayerPrefsManager.IsHammerUnlocked(hammerIndex))
        {
            PlayerPrefsManager.ChooseHammer(hammerIndex);
            EventManager.RaiseEventHammerSpriteChanged();
        }
    }
}
