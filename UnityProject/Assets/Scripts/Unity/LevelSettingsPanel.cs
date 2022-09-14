using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rovio.TapMatch.Logic;

public class LevelSettingsPanel : MonoBehaviour
{
    [SerializeField] Slider levelWidthSlider;
    [SerializeField] Slider levelHeightSlider;
    [SerializeField] Slider numberOfColorsSlider;

    public int LevelWidth => (int)levelWidthSlider.value;
    public int LevelHeight => (int)levelHeightSlider.value;
    public int NumberOfColors => (int)numberOfColorsSlider.value;

    private void Start()
    {
        levelWidthSlider.minValue = LogicConstants.MinLevelWidth;
        levelWidthSlider.maxValue = LogicConstants.MaxLevelWidth;
        levelWidthSlider.value = LogicConstants.MinLevelWidth;
        levelWidthSlider.onValueChanged?.Invoke(levelWidthSlider.value);

        levelHeightSlider.minValue = LogicConstants.MinLevelHeight;
        levelHeightSlider.maxValue = LogicConstants.MaxLevelHeight;
        levelHeightSlider.value = LogicConstants.MinLevelHeight;
        levelHeightSlider.onValueChanged?.Invoke(levelHeightSlider.value);

        numberOfColorsSlider.minValue = LogicConstants.MinColorsType;
        numberOfColorsSlider.maxValue = LogicConstants.MaxColorsType;
        numberOfColorsSlider.value = LogicConstants.MinColorsType;
        numberOfColorsSlider.onValueChanged?.Invoke(numberOfColorsSlider.value);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
