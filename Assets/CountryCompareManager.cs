using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountryCompareManager : MonoBehaviour
{
    private GameObject CountryLeft;
    private GameObject CountryRight;

    [SerializeField] private TMP_Text CountryLeftText;
    [SerializeField] private TMP_Text CountryRightText;

    #region Singleton
    public static CountryCompareManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CountryLeft != null)
        {
            CountryLeftText.color = CountryLeft.GetComponent<MouseManager>().savedColor;
        }
        if(CountryRight != null)
        {
            CountryRightText.color = CountryRight.GetComponent<MouseManager>().savedColor;
        }
    }

    public bool AddCountry(GameObject _Country)
    {
        if(CountryLeft == null)
        {
            CountryLeft = _Country;
            CountryLeftText.text = _Country.name;
            CountryLeftText.color = _Country.GetComponent<MouseManager>().savedColor;
            return true;
        } else if(CountryRight == null)
        {
            CountryRight = _Country;
            CountryRightText.text = _Country.name;
            CountryRightText.color = _Country.GetComponent<MouseManager>().savedColor;
            return true;
        }
        return false;
    }

    public void RemoveCountry(GameObject _Country)
    {
        if(GameObject.ReferenceEquals(CountryLeft, _Country))
        {
            CountryLeft = null;
            CountryLeftText.text = "SELECT COUNTRY";
            CountryLeftText.color = DataManager.Instance.HoverColor;
        } else if (GameObject.ReferenceEquals(CountryRight, _Country))
        {
            CountryRight = null;
            CountryRightText.text = "SELECT COUNTRY";
            CountryRightText.color = DataManager.Instance.HoverColor;
        }
    }
}
