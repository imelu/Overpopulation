using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryCompareManager : MonoBehaviour
{
    public GameObject CountryLeft;
    public GameObject CountryRight;

    [SerializeField] private TMP_Text CountryLeftText;
    [SerializeField] private TMP_Text CountryRightText;

    [SerializeField] private DropDownManager DropDownLeft;
    [SerializeField] private DropDownManager DropDownRight;

    [SerializeField] private GameObject Countries;

    private bool leftActive = true;

    [SerializeField] private Image LeftButton;
    [SerializeField] private Image RightButton;

    private Color buttonIdle;
    private Color buttonSelected;

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
        buttonIdle = DataManager.Instance.HoverColor;
        buttonSelected = DataManager.Instance.ClickedColor;
        LeftButton.color = buttonSelected;
        RightButton.color = buttonIdle;
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
        if (leftActive)
        {
            if(CountryLeft != null)
            {
                RemoveCountry(CountryLeft);
            }
            CountryLeft = _Country;
            //CountryLeftText.text = _Country.name;
            CountryLeftText.color = _Country.GetComponent<MouseManager>().savedColor;
            DropDownLeft.ChangeOption(CountryLeft);
        }
        else
        {
            if (CountryRight != null)
            {
                RemoveCountry(CountryRight);
            }
            CountryRight = _Country;
            //CountryLeftText.text = _Country.name;
            CountryRightText.color = _Country.GetComponent<MouseManager>().savedColor;
            DropDownRight.ChangeOption(CountryRight);
        }
        
        return true;
        /*
        if(CountryLeft == null)
        {
            CountryLeft = _Country;
            //CountryLeftText.text = _Country.name;
            CountryLeftText.color = _Country.GetComponent<MouseManager>().savedColor;
            DropDownLeft.ChangeOption(CountryLeft);
            return true;
        } else if(CountryRight == null)
        {
            CountryRight = _Country;
            //CountryRightText.text = _Country.name;
            CountryRightText.color = _Country.GetComponent<MouseManager>().savedColor;
            DropDownRight.ChangeOption(CountryRight);
            return true;
        }
        return false;*/
    }

    public void RemoveCountry(GameObject _Country)
    {
        if (GameObject.ReferenceEquals(CountryLeft, _Country))
        {
            CountryLeft.GetComponent<MouseManager>().CountryDeselected();
            CountryLeft = null;
            //CountryLeftText.text = "SELECT COUNTRY";
            CountryLeftText.color = DataManager.Instance.HoverColor;
            DropDownLeft.RemoveOption();
        } else if (GameObject.ReferenceEquals(CountryRight, _Country))
        {
            CountryRight.GetComponent<MouseManager>().CountryDeselected();
            CountryRight = null;
            //CountryRightText.text = "SELECT COUNTRY";
            CountryRightText.color = DataManager.Instance.HoverColor;
            DropDownRight.RemoveOption();
        }
    }

    public bool AddLeftCountry(string _Country)
    {
        GameObject temp;
        if(CountryLeft != null)
        {
            CountryLeft.GetComponent<MouseManager>().CountryDeselected();
        }
        temp = Countries.transform.Find(_Country).gameObject;
        if(CountryRight != null)
        {
            if (temp != CountryRight)
            {
                CountryLeft = temp;
                CountryLeft.GetComponent<MouseManager>().DropDownSelected();
                return true;
            }
            else
            {
                if (CountryLeft != null)
                {
                    RemoveCountry(CountryLeft);
                }
                return false;
            }
        }
        else
        {
            CountryLeft = temp;
            CountryLeft.GetComponent<MouseManager>().DropDownSelected();
            return true;
        }
    }

    public bool AddRightCountry(string _Country)
    {
        GameObject temp;
        if (CountryRight != null)
        {
            CountryRight.GetComponent<MouseManager>().CountryDeselected();
        }
        temp = Countries.transform.Find(_Country).gameObject;
        if(CountryLeft != null)
        {
            if (temp != CountryLeft)
            {
                CountryRight = temp;
                CountryRight.GetComponent<MouseManager>().DropDownSelected();
                return true;
            }
            else
            {
                if (CountryRight != null)
                {
                    RemoveCountry(CountryRight);
                }
                return false;
            }
        }
        else
        {
            CountryRight = temp;
            CountryRight.GetComponent<MouseManager>().DropDownSelected();
            return true;
        }
    }

    public void SetActiveSide(bool _leftButton)
    {
        leftActive = _leftButton;
        if (leftActive)
        {
            LeftButton.color = buttonSelected;
            RightButton.color = buttonIdle;
        }
        else
        {
            RightButton.color = buttonSelected;
            LeftButton.color = buttonIdle;
        }
    }
}
