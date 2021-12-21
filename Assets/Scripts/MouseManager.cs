using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Color savedColor;
    private Color HoverColor;
    private Color ClickedColor;

    public bool countryAdded = false;
    public bool countryHovered = false;
    
    /*public Camera mainCamera;
    private RaycastHit2D hit;

    private GameObject changedCountry;*/

    // Start is called before the first frame update
    void Start()
    {
        HoverColor = DataManager.Instance.HoverColor;
        ClickedColor = DataManager.Instance.ClickedColor;
    }

    public void OnMouseEnter()
    {
        if (!countryAdded)
        {
            savedColor = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = HoverColor;
            countryHovered = true;
        }
    }

    public void OnMouseExit()
    {
        if (!countryAdded)
        {
            gameObject.GetComponent<SpriteRenderer>().color = savedColor;
            countryHovered = false;
        }
    }

    public void OnMouseDown()
    {
        if (countryAdded)
        {
            //CountryCompareManager.Instance.RemoveCountry(gameObject);
            //countryAdded = false;
        }
        else
        {
            countryAdded = CountryCompareManager.Instance.AddCountry(gameObject);
        }

        if (countryAdded)
        {
            gameObject.GetComponent<SpriteRenderer>().color = ClickedColor;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = HoverColor;
        }
    }

    public void DropDownSelected()
    {
        countryAdded = true;
        savedColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = ClickedColor;
    }
    public void CountryDeselected()
    {
        countryAdded = false;
        gameObject.GetComponent<SpriteRenderer>().color = savedColor;
    }
}
