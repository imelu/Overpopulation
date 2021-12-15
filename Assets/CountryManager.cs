using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour
{
    [SerializeField] private string countryName;
    private Dictionary<int, CountryData> CountryDict;

    [SerializeField] private int currentYear;

    // Start is called before the first frame update
    void Start()
    {
        CountryDict = CsvImport.Instance.FetchCountryDict(countryName);
    }

    // Update is called once per frame
    void Update()
    {
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
                Debug.Log(temp.population);
                Debug.Log(temp.fertility);
                Debug.Log(temp.mortality);
            }
            else
            {
                Debug.Log("No data for year " + currentYear + " in the country " + countryName + " has been found");
            }
        }
    }
}
