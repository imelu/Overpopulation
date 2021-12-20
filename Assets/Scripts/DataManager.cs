using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

// Data for each country, if the year does not have a kind of data returns -1
public class CountryData
{
    public float fertility = -1;
    public float mortality = -1;
    public float population = -1;

    public CountryData(float _fertility, float _mortality, float _population)
    {
        fertility = _fertility;
        mortality = _mortality;
        population = _population;
    }
}

// Saves the years in which each country has data for each individual data point
public class CountryYearData
{
    public List<int> fertYears;
    public List<int> mortYears;
    public List<int> popYears;

    public CountryYearData()
    {
        fertYears = new List<int>();
        mortYears = new List<int>();
        popYears = new List<int>();
    }
}

public class DataManager : MonoBehaviour
{
    // Dictionary for all Country Dictionaries with the Country Data
    private Dictionary<string, Dictionary<int, CountryData>> CountryDataDict = new Dictionary<string, Dictionary<int, CountryData>>();
    private Dictionary<string, CountryYearData> ListDict = new Dictionary<string, CountryYearData>();

    string path;

    public GameObject WorldMapSchraffiert;

    [SerializeField] private TextAsset popData;
    [SerializeField] private TextAsset mortData;
    [SerializeField] private TextAsset fertData;

    public Color colStage1;
    public Color colStage2;
    public Color colStage3;
    public Color colStage4;

    public Color HoverColor;
    public Color ClickedColor;

    #region Singleton
    public static DataManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ReadPopulation();
        ReadFertility();
        ReadChildMortality();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ReadPopulation()
    {
        Dictionary<int, CountryData> temp;
        CountryYearData tempList;
        CountryData tempCdata;

        var lines = popData.text.Split("\n"[0]);

        foreach(String line in lines)
        {
            String data_String = line;
            var data_values = data_String.Split(',');

            if (!CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                Dictionary<int, CountryData> tempDict = new Dictionary<int, CountryData>();
                CountryDataDict.Add(data_values[0], tempDict);
            }

            if (CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                if (!temp.TryGetValue(int.Parse(data_values[2]), out tempCdata))
                {
                    CountryData tempData = new CountryData(-1, -1, float.Parse(data_values[3]));
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.population = float.Parse(data_values[3]);
                }
            }

            if(!ListDict.TryGetValue(data_values[0], out tempList))
            {
                CountryYearData tempYData = new CountryYearData();
                ListDict.Add(data_values[0], tempYData);
            }

            if(ListDict.TryGetValue(data_values[0], out tempList))
            {
                tempList.popYears.Add(int.Parse(data_values[2]));
            }
        }
    }

    private void ReadFertility()
    {
        Dictionary<int, CountryData> temp;
        CountryYearData tempList;
        CountryData tempCdata;

        var lines = fertData.text.Split("\n"[0]);

        foreach (String line in lines)
        {
            String data_String = line;
            var data_values = data_String.Split(',');

            if (!CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                Dictionary<int, CountryData> tempDict = new Dictionary<int, CountryData>();
                CountryDataDict.Add(data_values[0], tempDict);
            }

            if (CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                if (!temp.TryGetValue(int.Parse(data_values[2]), out tempCdata))
                {
                    CountryData tempData = new CountryData(float.Parse(data_values[3]), -1, -1);
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.fertility = float.Parse(data_values[3]);
                }
            }

            if (!ListDict.TryGetValue(data_values[0], out tempList))
            {
                CountryYearData tempYData = new CountryYearData();
                ListDict.Add(data_values[0], tempYData);
            }

            if (ListDict.TryGetValue(data_values[0], out tempList))
            {
                tempList.fertYears.Add(int.Parse(data_values[2]));
            }
        }
    }

    private void ReadChildMortality()
    {
        Dictionary<int, CountryData> temp;
        CountryYearData tempList;
        CountryData tempCdata;

        var lines = mortData.text.Split("\n"[0]);

        foreach (String line in lines)
        {
            String data_String = line;
            var data_values = data_String.Split(',');

            if (!CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                Dictionary<int, CountryData> tempDict = new Dictionary<int, CountryData>();
                CountryDataDict.Add(data_values[0], tempDict);
            }

            if (CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                if (!temp.TryGetValue(int.Parse(data_values[2]), out tempCdata))
                {
                    CountryData tempData = new CountryData(-1, float.Parse(data_values[3]), -1);
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.mortality = float.Parse(data_values[3]); 
                }
            }

            if (!ListDict.TryGetValue(data_values[0], out tempList))
            {
                CountryYearData tempYData = new CountryYearData();
                ListDict.Add(data_values[0], tempYData);
            }

            if (ListDict.TryGetValue(data_values[0], out tempList))
            {
                tempList.mortYears.Add(int.Parse(data_values[2]));
            }
        }
    }

    // old, fetch current data of a specific country
    public CountryData FetchData(string _country, int _year)
    {
        Dictionary<int, CountryData> temp;
        CountryData tempData;
        if(CountryDataDict.TryGetValue(_country, out temp))
        {
            if(temp.TryGetValue(_year, out tempData))
            {
                return tempData;
            }
        }
        return null;
    }

    // fetch the dictionary of a specific country
    public Dictionary<int, CountryData> FetchCountryDict(string _country)
    {
        Dictionary<int, CountryData> temp;
        if (CountryDataDict.TryGetValue(_country, out temp))
        {
            return temp;
        }
        return null;
    }
    
    // fetch the data of which years have which data for a specific country
    public CountryYearData FetchCountryYearData(string _country)
    {
        CountryYearData temp;
        if(ListDict.TryGetValue(_country, out temp))
        {
            return temp;
        }
        return null;
    }
}
