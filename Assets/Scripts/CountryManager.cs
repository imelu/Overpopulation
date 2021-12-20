using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountryManager : MonoBehaviour
{
    private string countryName;
    private Dictionary<int, CountryData> CountryDict;
    private GameObject schraffur;

    private int currentYearOld = 1750;
    [SerializeField] private int currentYear;
    private int dataYear;
    private SpriteRenderer image;

    private Color colStage1;
    private Color colStage2;
    private Color colStage3;
    private Color colStage4;

    private int currentStage = 1;

    private int mortalityThreshhold1 = 20;
    private int mortalityThreshhold2 = 2;
    private int fertilityThreshhold1 = 4;
    private int fertilityThreshhold2 = 2;

    private float currentMort;
    private float currentFert;

    private CountryData currentCountryData = new CountryData(1000000, 1000000, 1000000);
    private CountryYearData yearData;

    public bool processing = false;

    private MouseManager mouseManager;

    private bool noData = false;
    private bool schraffurActive = false;

    // Start is called before the first frame update
    void Start()
    {
        countryName = gameObject.name;
        CountryDict = DataManager.Instance.FetchCountryDict(countryName);
        yearData = DataManager.Instance.FetchCountryYearData(countryName);
        schraffur = DataManager.Instance.WorldMapSchraffiert;
        colStage1 = DataManager.Instance.colStage1;
        colStage2 = DataManager.Instance.colStage2;
        colStage3 = DataManager.Instance.colStage3;
        colStage4 = DataManager.Instance.colStage4;
        image = GetComponent<SpriteRenderer>();
        image.color = colStage1;
        mouseManager = GetComponent<MouseManager>();
        //Debug.Log(countryName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateYear()
    {
        CountryData temp;
        currentYear = YearManager.Instance.currentYear;
        noData = false;
        if (CountryDict == null)
        {
            CountryDict = DataManager.Instance.FetchCountryDict(countryName);
        }
        else
        {
            if(yearData == null)
            {
                yearData = DataManager.Instance.FetchCountryYearData(countryName);
            }
            else
            {
                if (yearData.fertYears.Count > 0 && yearData.mortYears.Count > 0)
                {
                    if (currentYear < yearData.fertYears[0] || currentYear < yearData.mortYears[0])
                    {
                        noData = true;

                        if (yearData.fertYears[0] < yearData.mortYears[0])
                        {
                            dataYear = yearData.mortYears[0];
                        }
                        else
                        {
                            dataYear = yearData.fertYears[0];
                        }
                    }
                    else if (currentYear > yearData.fertYears[yearData.fertYears.Count - 1] || currentYear > yearData.mortYears[yearData.mortYears.Count - 1])
                    {
                        noData = true;
                        if (yearData.fertYears[yearData.fertYears.Count - 1] > yearData.mortYears[yearData.mortYears.Count - 1])
                        {
                            dataYear = yearData.mortYears[yearData.mortYears.Count - 1];
                        }
                        else
                        {
                            dataYear = yearData.fertYears[yearData.fertYears.Count - 1];
                        }
                    }
                    else
                    {
                        dataYear = currentYear;
                    }
                }


                if (CountryDict.TryGetValue(dataYear, out temp))
                {
                    currentCountryData.fertility = temp.fertility;
                    currentCountryData.mortality = temp.mortality;
                    currentCountryData.population = temp.population;

                    if(currentYear < 2015 && noData)
                    {
                        currentStage = 1;
                        ChangeColor(colStage1);
                    }
                    else
                    {
                        switch (currentStage)
                        {
                            case 1:
                                if (currentCountryData.mortality < mortalityThreshhold1)
                                {
                                    currentStage = 2;
                                    ChangeColor(colStage2);
                                    updateYear();
                                }
                                break;

                            case 2:
                                if (currentCountryData.mortality > mortalityThreshhold1)
                                {
                                    currentStage = 1;
                                    ChangeColor(colStage1);
                                    updateYear();
                                }
                                if (currentCountryData.mortality < mortalityThreshhold2 && currentCountryData.fertility < fertilityThreshhold1)
                                {
                                    currentStage = 3;
                                    ChangeColor(colStage3);
                                    updateYear();
                                }
                                break;

                            case 3:
                                if (currentCountryData.mortality > mortalityThreshhold2 || currentCountryData.fertility > fertilityThreshhold1)
                                {
                                    currentStage = 2;
                                    ChangeColor(colStage2);
                                    updateYear();
                                }
                                if (currentCountryData.fertility < fertilityThreshhold2)
                                {
                                    currentStage = 4;
                                    ChangeColor(colStage4);
                                    updateYear();
                                }
                                break;

                            case 4:
                                if (currentCountryData.fertility > fertilityThreshhold2)
                                {
                                    currentStage = 3;
                                    ChangeColor(colStage3);
                                    updateYear();
                                }
                                break;
                        }
                    }
                }
                else
                {
                    //Debug.Log("No data for year " + currentYear + " in the country " + countryName + " has been found");
                }
            }
        }
        if (noData && !schraffurActive)
        {
            foreach(Transform child in schraffur.transform)
            {
                if(string.Equals(child.gameObject.name, gameObject.name))
                {
                    child.gameObject.SetActive(true);
                    schraffurActive = true;
                    return;
                }
            }
        }
        if(!noData && schraffurActive)
        {
            foreach (Transform child in schraffur.transform)
            {
                if (string.Equals(child.gameObject.name, gameObject.name))
                {
                    child.gameObject.SetActive(false);
                    schraffurActive = false;
                    return;
                }
            }
        }
    }

    #region oldupdateyear
    /*
    public void updateYear()
    {
        CountryData temp;
        currentYearOld = currentYear;
        currentYear = YearManager.Instance.currentYear;
        if (CountryDict == null)
        {
            CountryDict = DataManager.Instance.FetchCountryDict(countryName);
        }
        else
        {
            processing = true;
            int fromYear;
            int toYear;
            if(currentYearOld < currentYear)
            {
                fromYear = currentYearOld;
                toYear = currentYear;
            }
            else
            {
                fromYear = currentYear;
                toYear = currentYearOld;
            }
            for(int i = fromYear; i < toYear; i++)
            {
                if (CountryDict.TryGetValue(i, out temp))
                {
                    if (temp.mortality != -1)
                    {
                        currentCountryData.mortality = temp.mortality;
                    }
                    if (temp.fertility != -1)
                    {
                        currentCountryData.fertility = temp.fertility;
                    }
                    if (temp.population != -1)
                    {
                        currentCountryData.population = temp.population;
                    }

                    currentMort = currentCountryData.mortality;
                    currentFert = currentCountryData.fertility;
                    
                    if(temp.mortality == -1 && currentStage == 2 && currentYear < 2000)
                    {
                        currentCountryData.mortality = 1000000;
                        currentStage = 1;
                        ChangeColor(colStage1);
                    }
                    switch (currentStage)
                    {
                        case 1:
                            if (currentCountryData.mortality < mortalityThreshhold1)
                            {
                                currentStage = 2;
                                ChangeColor(colStage2);
                                updateYear();
                            }
                            break;

                        case 2:
                            if (currentCountryData.mortality > mortalityThreshhold1)
                            {
                                currentStage = 1;
                                ChangeColor(colStage1);
                                updateYear();
                            }
                            if (currentCountryData.mortality < mortalityThreshhold2 && currentCountryData.fertility < fertilityThreshhold1)
                            {
                                currentStage = 3;
                                ChangeColor(colStage3);
                                updateYear();
                            }
                            break;

                        case 3:
                            if (currentCountryData.mortality > mortalityThreshhold2 || currentCountryData.fertility > fertilityThreshhold1)
                            {
                                currentStage = 2;
                                ChangeColor(colStage2);
                                updateYear();
                            }
                            if (currentCountryData.fertility < fertilityThreshhold2)
                            {
                                currentStage = 4;
                                ChangeColor(colStage4);
                                updateYear();
                            }
                            break;

                        case 4:
                            if (currentCountryData.fertility > fertilityThreshhold2)
                            {
                                currentStage = 3;
                                ChangeColor(colStage3);
                                updateYear();
                            }
                            break;
                    }
                }
                else
                {
                    //Debug.Log("No data for year " + currentYear + " in the country " + countryName + " has been found");
                }
            }
        }
        
        processing = false;
    }
    */
    #endregion

    // always call this to change color, it checks if the country is hovered or selected first
    private void ChangeColor(Color _color)
    {
        if (mouseManager.countryAdded)
        {
            mouseManager.savedColor = _color;
        }
        else
        {
            if (!mouseManager.countryHovered)
            {
                image.color = _color;
            }
        }
    }
}
