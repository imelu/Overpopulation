using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownManager : MonoBehaviour
{

    [SerializeField] private GameObject Countries;
    private TMP_Dropdown dropdown;
    private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();


    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        foreach(Transform child in Countries.transform)
        {
            TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
            temp.text = child.gameObject.name;
            options.Add(temp);
        }
        dropdown.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
