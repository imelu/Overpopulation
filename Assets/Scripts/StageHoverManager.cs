using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageHoverManager : MonoBehaviour
{
    [SerializeField] private GameObject Backdrop;
    [SerializeField] private GameObject Graph;
    private TMP_Text Number;
    [SerializeField] private Color Idle;
    private Color Hovered;

    // Start is called before the first frame update
    void Start()
    {
        Hovered = DataManager.Instance.HoverColor;
        Number = GetComponent<TMP_Text>();
        Number.color = Idle;
    }

    private void OnMouseEnter()
    {
        Number.color = Hovered;
        Backdrop.SetActive(true);
        Graph.SetActive(true);
    }

    private void OnMouseExit()
    {
        Number.color = Idle;
        Backdrop.SetActive(false);
        Graph.SetActive(false);
    }
}
