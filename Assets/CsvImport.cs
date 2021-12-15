using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


public class CountryData
{
    public float fertility;
    public float mortality;
    public float population;

    public CountryData(float _fertility, float _mortality, float _population)
    {
        fertility = _fertility;
        mortality = _mortality;
        population = _population;
    }
}

public class CsvImport : MonoBehaviour
{
    private Dictionary<string, Dictionary<int, CountryData>> CountryDataDict = new Dictionary<string, Dictionary<int, CountryData>>();

    string path;

    #region Singleton
    public static CsvImport Instance;
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
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CountryData temp = FetchData("Switzerland", 1932);
            Debug.Log(temp.population);
            Debug.Log(temp.fertility);
            Debug.Log(temp.mortality);
        }*/
    }

    private void ReadPopulation()
    {
        Dictionary<int, CountryData> temp;
        CountryData tempCdata;
        path = Path.Combine(Environment.CurrentDirectory, @"Assets\data\population-since-1800.csv");
        StreamReader strReader = new StreamReader(path);
        bool endOfFile = false;

        while (!endOfFile)
        {
            String data_String = strReader.ReadLine();
            if(data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');

            if (!CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                Dictionary<int, CountryData> tempDict = new Dictionary<int, CountryData>();
                CountryDataDict.Add(data_values[0], tempDict);
            }

            if(CountryDataDict.TryGetValue(data_values[0], out temp))
            {
                if(!temp.TryGetValue(int.Parse(data_values[2]), out tempCdata))
                {
                    CountryData tempData = new CountryData(0, 0, float.Parse(data_values[3]));
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.population = float.Parse(data_values[3]);
                }
            }

            //Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString() + " " + data_values[2].ToString() + " " + data_values[3].ToString());
        }
    }

    private void ReadFertility()
    {
        Dictionary<int, CountryData> temp;
        CountryData tempCdata;
        path = Path.Combine(Environment.CurrentDirectory, @"Assets\data\children-born-per-woman.csv");
        StreamReader strReader = new StreamReader(path);
        bool endOfFile = false;

        while (!endOfFile)
        {
            String data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
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
                    CountryData tempData = new CountryData(float.Parse(data_values[3]), 0, 0);
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.fertility = float.Parse(data_values[3]);
                }
            }

            //Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString() + " " + data_values[2].ToString() + " " + data_values[3].ToString());
        }
    }

    private void ReadChildMortality()
    {
        Dictionary<int, CountryData> temp;
        CountryData tempCdata;
        path = Path.Combine(Environment.CurrentDirectory, @"Assets\data\child-mortality.csv");
        StreamReader strReader = new StreamReader(path);
        bool endOfFile = false;

        while (!endOfFile)
        {
            String data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
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
                    CountryData tempData = new CountryData(0, float.Parse(data_values[3]), 0);
                    temp.Add(int.Parse(data_values[2]), tempData);
                }
                else
                {
                    tempCdata.mortality = float.Parse(data_values[3]);
                }
            }

            //Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString() + " " + data_values[2].ToString() + " " + data_values[3].ToString());
        }
    }

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

    public Dictionary<int, CountryData> FetchCountryDict(string _country)
    {
        Dictionary<int, CountryData> temp;
        if (CountryDataDict.TryGetValue(_country, out temp))
        {
            return temp;
        }
        return null;
    }
}
