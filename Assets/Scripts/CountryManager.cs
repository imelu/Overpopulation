using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour
{
    private string countryName;
    private Dictionary<int, CountryData> CountryDict;

    private int currentYearOld = 1750;
    [SerializeField] private int currentYear;
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

    public bool processing = false;

    private MouseManager mouseManager;

    // Start is called before the first frame update
    void Start()
    {
        countryName = gameObject.name;
        CountryDict = DataManager.Instance.FetchCountryDict(countryName);
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


    private void ChangeColor(Color _color)
    {
        if (mouseManager.countryAdded)
        {
            mouseManager.savedColor = _color;
        }
        else
        {
            image.color = _color;
        }
    }
}
