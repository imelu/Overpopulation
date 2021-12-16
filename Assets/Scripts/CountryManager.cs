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

    // Start is called before the first frame update
    void Start()
    {
        countryName = gameObject.name;
        CountryDict = CsvImport.Instance.FetchCountryDict(countryName);
        colStage1 = CsvImport.Instance.colStage1;
        colStage2 = CsvImport.Instance.colStage2;
        colStage3 = CsvImport.Instance.colStage3;
        colStage4 = CsvImport.Instance.colStage4;
        image = GetComponent<SpriteRenderer>();
        image.color = colStage1;
        //Debug.Log(countryName);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space)) // call this when the current year changes
        {
            CountryData temp;
            currentYear = YearManager.Instance.currentYear;
            if(CountryDict == null)
            {
                CountryDict = CsvImport.Instance.FetchCountryDict(countryName);
            }
            if(CountryDict.TryGetValue(currentYear, out temp))
            {
                switch (currentStage)
                {
                    case 1:
                        if(temp.mortality < mortalityThreshhold1)
                        {
                            currentStage = 2;
                        }
                        break;

                    case 2:
                        if(temp.mortality < mortalityThreshhold2 && temp.fertility < fertilityThreshhold1)
                        {
                            currentStage = 3;
                        }
                        break;

                    case 3:
                        if(temp.fertility < fertilityThreshhold2)
                        {
                            currentStage = 4;
                        }
                        break;

                    case 4:

                        break;
                }
            }
            else
            {
                Debug.Log("No data for year " + currentYear + " in the country " + countryName + " has been found");
            }
        }*/
    }

    public void updateYear()
    {
        CountryData temp;
        currentYear = YearManager.Instance.currentYear;
        if (CountryDict == null)
        {
            CountryDict = CsvImport.Instance.FetchCountryDict(countryName);
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
                if (CountryDict.TryGetValue(currentYear, out temp))
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
                        image.color = colStage1;
                    }
                    switch (currentStage)
                    {
                        case 1:
                            if (currentCountryData.mortality < mortalityThreshhold1)
                            {
                                currentStage = 2;
                                image.color = colStage2;
                                updateYear();
                            }
                            break;

                        case 2:
                            if (currentCountryData.mortality > mortalityThreshhold1)
                            {
                                currentStage = 1;
                                image.color = colStage1;
                                updateYear();
                            }
                            if (currentCountryData.mortality < mortalityThreshhold2 && currentCountryData.fertility < fertilityThreshhold1)
                            {
                                currentStage = 3;
                                image.color = colStage3;
                                updateYear();
                            }
                            break;

                        case 3:
                            if (currentCountryData.mortality > mortalityThreshhold2 || currentCountryData.fertility > fertilityThreshhold1)
                            {
                                currentStage = 2;
                                image.color = colStage2;
                                updateYear();
                            }
                            if (currentCountryData.fertility < fertilityThreshhold2)
                            {
                                currentStage = 4;
                                image.color = colStage4;
                                updateYear();
                            }
                            break;

                        case 4:
                            if (currentCountryData.fertility > fertilityThreshhold2)
                            {
                                currentStage = 3;
                                image.color = colStage3;
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
        currentYearOld = currentYear;
        processing = false;
    }
}
