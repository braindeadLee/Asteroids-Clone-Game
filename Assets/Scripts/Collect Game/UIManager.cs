using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CollectedItemText;

    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI GameOverText;

    public void UpdateCollectedItemsText(int currentItemCount, int totalCollectableItems) => CollectedItemText.text = $"{currentItemCount} of {totalCollectableItems}";

    void Start()
    {
        TimerText.text = string.Empty;
        GameOverText.text = string.Empty;
    }

    public void UpdateTimerText(int currentSeconds, int totalSeconds)
    {
        var ts = System.TimeSpan.FromSeconds(totalSeconds - currentSeconds);
        TimerText.text = $"{ts.Minutes}:{ts.Seconds:00}";
    }

    public void SetGameOverText(string text) => GameOverText.text = text;
}
