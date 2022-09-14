using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class SliderValueText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI valueText;
    private void Awake()
    {
        valueText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void OnSliderValueChange(float value)
    {
        valueText.text = value.ToString();
    }
}
