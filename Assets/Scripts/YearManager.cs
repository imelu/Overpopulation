using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YearManager : MonoBehaviour
{
    [SerializeField] private int minYear;
    [SerializeField] private int maxYear;

    private Slider YearSlider;
    [SerializeField] private TMP_Text CurrentYearDisplay;

    public int currentYear = 1750;

    [SerializeField] private GameObject WorldMap;
    [SerializeField] private GameObject WorldCountry;
    [SerializeField] private GameObject WorldGraph;
    [SerializeField] private GameObject CountryGraphLeft;
    [SerializeField] private GameObject CountryGraphRight;

    [SerializeField] private Slider SliderWorld;
    [SerializeField] private Slider SliderC1;
    [SerializeField] private Slider SliderC2;
    #region Singleton
    public static YearManager Instance;
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
        YearSlider = GetComponent<Slider>();
        StartCoroutine(UpdateCountries());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateYear()
    {
        currentYear = (int)YearSlider.value;
        CurrentYearDisplay.text = currentYear.ToString();
        SliderWorld.value = YearSlider.value;
        SliderC1.value = YearSlider.value;
        SliderC2.value = YearSlider.value;
    }

    IEnumerator UpdateCountries()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        foreach (Transform child in WorldMap.transform)
        {
            //if (!child.gameObject.GetComponent<CountryManager>().processing)
            //{
                child.gameObject.GetComponent<CountryManager>().updateYear();
            //}
        }
        WorldGraph.GetComponent<WorldPopGraphScript>().updateYear(currentYear);
        CountryGraphLeft.GetComponent<CountryPopGraphScript>().updateYear(currentYear);
        CountryGraphRight.GetComponent<CountryPopGraphScript>().updateYear(currentYear);

        StartCoroutine(UpdateCountries());
    }
}