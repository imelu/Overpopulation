/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CountryPopGraphScript : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    int currmaxyear;

    private Dictionary<int, CountryData> CountryDict;
    private CountryYearData yearData;
    private int currentYear;
    private CountryData currentCountryData = new CountryData(1000000, 1000000, 1000000);
    private List<float> CountryDataPop = new List<float>();
    private List<float> CountryDataFert = new List<float>();
    private List<float> CountryDataMort = new List<float>();
    private List<GameObject> graphComponents = new List<GameObject>();
    int minYear = 1800;
    int maxYear = 2021;
    private GameObject CountryLocal;
    private GameObject CountryGlobal;
    [SerializeField] private bool isleft;
    [SerializeField] private bool isright;
    private float maxpop;
    private float maxfert;
    private float maxmort;
    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();


    }
    private void Start()
    {
        currentYear = YearManager.Instance.currentYear;
    }
    private void Update()
    {

        if (isleft == true)
        {
            CountryGlobal = CountryCompareManager.Instance.CountryLeft;
            if (CountryGlobal != null)
            {
                if (CountryLocal != CountryGlobal)
                {
                    for (int i = 0; i < graphComponents.Count; i++)
                    {
                        Destroy(graphComponents[i]);
                    }
                    CountryDataPop.Clear();
                    CountryDataFert.Clear();
                    CountryDataMort.Clear();
                    maxpop = 0;
                    maxfert = 0;
                    maxmort = 0;
                    CountryLocal = CountryGlobal;
                    CountryDict = DataManager.Instance.FetchCountryDict(CountryLocal.name);
                    //yearData = DataManager.Instance.FetchCountryYearData(CountryLocal.name);
                    getAllData();
                }
            }

        }
        if (isright == true)
        {
            CountryGlobal = CountryCompareManager.Instance.CountryRight;
            if (CountryGlobal != null)
            {
                if (CountryLocal != CountryGlobal)
                {
                    for (int i = 0; i < graphComponents.Count; i++)
                    {
                        Destroy(graphComponents[i]);
                    }
                    CountryDataPop.Clear();
                    CountryDataFert.Clear();
                    CountryDataMort.Clear();
                    maxpop = 0;
                    maxfert = 0;
                    maxmort = 0;
                    CountryLocal = CountryGlobal;
                    CountryDict = DataManager.Instance.FetchCountryDict(CountryLocal.name);
                    //yearData = DataManager.Instance.FetchCountryYearData(CountryLocal.name);
                    getAllData();
                }
            }
        }


    }
    public void updateYear(int currentYear)
    {
        if (CountryLocal != null)
        {
            if (currentYear >= 1800 && currentYear <= 2021)
            {
                for (int i = 0; i < graphComponents.Count; i++)
                {
                    Destroy(graphComponents[i]);
                }

                int listIndex = currentYear - minYear;
                //popdata
                List<float> CountryYearDataPop = new List<float>(CountryDataPop);
                CountryYearDataPop.RemoveRange(listIndex, maxYear - currentYear + 1);
                ShowGraph(CountryYearDataPop,maxpop);
                //fertdata
                List<float> CountryYearDataFert = new List<float>(CountryDataFert);
                CountryYearDataFert.RemoveRange(listIndex, maxYear - currentYear + 1);
                ShowGraph(CountryYearDataFert,maxfert);
                //mortdata
                List<float> CountryYearDataMort = new List<float>(CountryDataMort);
                CountryYearDataMort.RemoveRange(listIndex, maxYear - currentYear + 1);
                ShowGraph(CountryYearDataMort,maxmort);


            }
            else if (currentYear < 1800)
            {
                for (int i = 0; i < graphComponents.Count; i++)
                {
                    Destroy(graphComponents[i]);
                }
            }
            else if (currentYear > 2021)
            {
                for (int i = 0; i < graphComponents.Count; i++)
                {
                    Destroy(graphComponents[i]);
                }
                ShowGraph(CountryDataPop, maxpop);
                ShowGraph(CountryDataFert, maxfert);
                ShowGraph(CountryDataMort, maxmort);

            }

        }
        currmaxyear = currentYear;

    }

    void getAllData()
    {


        for (int i = minYear; i <= maxYear; i++)
        {
            CountryData temp;
            if (CountryDict.TryGetValue(i, out temp))
            {

                CountryDataPop.Add(temp.population);
                CountryDataFert.Add(temp.fertility);
                CountryDataMort.Add(temp.mortality);
                if (temp.population > maxpop) maxpop = temp.population;
                if (temp.fertility > maxfert) maxfert = temp.fertility;
                if (temp.mortality > maxmort) maxmort = temp.mortality;
            }

            else
            {
                CountryDataPop.Add(0);
                CountryDataFert.Add(0);
                CountryDataMort.Add(0);
            }
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        graphComponents.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(1, 1);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList,float maxY)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = maxY;
        float xSize = 0.8f;

        GameObject lastCircleGameObject = null;
        float gapsize = 1;
        float lastx = 0;
        for (int i = 0; i < valueList.Count; i++)
        {

            if (valueList[i] <= 0)
            {
                gapsize++;
            }
            else
            {

                float xPosition = gapsize * xSize + lastx;
                lastx = xPosition;
                float yPosition = (valueList[i] / yMaximum) * graphHeight;
                gapsize = 1;
                GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
                if (lastCircleGameObject != null)
                {
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                }
                lastCircleGameObject = circleGameObject;
            }

        }

    }



    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        graphComponents.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }


}
