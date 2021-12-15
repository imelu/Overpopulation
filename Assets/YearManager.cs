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

    public int currentYear;

    #region Singleton
    public static YearManager Instance;
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
        YearSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateYear()
    {
        currentYear = (int)((maxYear - minYear) * YearSlider.value + minYear);
        CurrentYearDisplay.text = currentYear.ToString();
    }
}