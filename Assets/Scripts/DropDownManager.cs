using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DropDownManager : MonoBehaviour
{

    [SerializeField] private GameObject CountriesAlphabetical;
    private TMP_Dropdown dropdown;
    private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
    [SerializeField] private bool LeftDropdown;
    [SerializeField] private Color start;


    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        foreach(Transform child in CountriesAlphabetical.transform)
        {
            TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
            temp.text = child.gameObject.name;
            options.Add(temp);
        }
        dropdown.AddOptions(options);
        start = dropdown.captionText.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        if(dropdown.value > 0)
        {
            if (LeftDropdown)
            {
                if (!CountryCompareManager.Instance.AddLeftCountry(dropdown.captionText.text))
                {
                    RemoveOption();
                }
            }
            else
            {
                if (!CountryCompareManager.Instance.AddRightCountry(dropdown.captionText.text))
                {
                    RemoveOption();
                }
            }
        }
        else
        {
            if (LeftDropdown)
            {
                if(CountryCompareManager.Instance.CountryLeft != null)
                {
                    CountryCompareManager.Instance.RemoveCountry(CountryCompareManager.Instance.CountryLeft);
                }
            }
            else
            {
                if (CountryCompareManager.Instance.CountryRight != null)
                {
                    CountryCompareManager.Instance.RemoveCountry(CountryCompareManager.Instance.CountryRight);
                }
            }
            dropdown.captionText.color = start;
        }
    }

    public void ChangeOption(GameObject _country)
    {
        int i = 0;
        foreach(TMP_Dropdown.OptionData option in options)
        {
            i++;
            if (string.Equals(option.text, _country.name))
            {
                dropdown.value = i;
                return;
            }
        }
    }

    public void RemoveOption()
    {
        dropdown.value = 0;
    }
}
