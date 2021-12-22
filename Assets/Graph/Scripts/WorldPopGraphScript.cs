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




public class WorldPopGraphScript : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    int currmaxyear;

    private Dictionary<int, CountryData> CountryDict;
    private CountryYearData yearData;
    private int currentYear;
    private CountryData currentCountryData = new CountryData(1000000, 1000000, 1000000);
    private List<float> WorldData = new List<float>();
    private List<GameObject> graphComponents = new List<GameObject>();
    bool once = false;
    int minYear = 1800;
    int maxYear = 2021;
    [SerializeField] private Color popColor;

    [SerializeField] public Image cheatSheet;
    private float lastSavedYear;

    private void Start()
    {
        currentYear = YearManager.Instance.currentYear;

        CountryDict = DataManager.Instance.FetchCountryDict("World");
        yearData = DataManager.Instance.FetchCountryYearData("World");


    }

    public void updateYear(int currentYear)
    {

        if (CountryDict == null)
        {
            CountryDict = DataManager.Instance.FetchCountryDict("World");
        }
        else
        {
            if (lastSavedYear != currentYear)
            {
                lastSavedYear = currentYear;
                cheatSheet.fillAmount = 1f / 221f * (maxYear - lastSavedYear);
            }
                if (once == false)
            {
                once = true;
                getAllData();
                ShowGraph(WorldData);
                cheatSheet.type = Image.Type.Filled;
                cheatSheet.fillAmount = 1f / 221f * (maxYear - currentYear);
                Debug.Log(cheatSheet.fillAmount);
            }
            /*
            else
            {
                if (currentYear >= 1800 && currentYear <= 2021)
                {
                    for (int i = 0; i < graphComponents.Count; i++)
                    {
                        Destroy(graphComponents[i]);
                    }

                    int listIndex = currentYear - minYear;
                    List<float> WorldYearData = new List<float>(WorldData);
                    WorldYearData.RemoveRange(listIndex, maxYear - currentYear + 1);
                    ShowGraph(WorldYearData);
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
                    ShowGraph(WorldData);
                }
            }
            */
        }
        currmaxyear = currentYear;
            
    }

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();


    }

    void getAllData()
    {
        for (int i = minYear; i <= maxYear; i++)
        {
            CountryData temp;
            if (CountryDict.TryGetValue(i, out temp))
            {
                WorldData.Add(temp.population);
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
        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 12000000000f;
        float xSize = 0.97f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        graphComponents.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = popColor;
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
