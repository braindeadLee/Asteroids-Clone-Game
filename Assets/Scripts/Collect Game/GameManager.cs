using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class GameManager : MonoBehaviour
{
    public UIManager UI;
    public Timer GameTimer;

    public int GameplayTime;

    private int _collectedItemCount;

    private int _totalCollectibleItems;

    public string WinText = "You win, yey!";
    public string LoseText = "YOU ARE A FAILURE!";

    public static GameManager Instance { get; private set; }
    private void Awake() => Instance = this;

    public void AddCollectibleItem() => _totalCollectibleItems++;

    private void ItemCollected()
    {
        UI.UpdateCollectedItemsText(++_collectedItemCount, _totalCollectibleItems);

        if (_collectedItemCount == _totalCollectibleItems) Win();
    }

    private void Start()
    {
        Debug.Log($"Items collected: {_collectedItemCount}, Total: {_totalCollectibleItems}.");
        StartCoroutine(UpdateUIAfterFrame());

        Invoke(nameof(StartTimer), 2f);

    }

    private void StartTimer() => GameTimer.StartTimer(GameplayTime);

    private IEnumerator UpdateUIAfterFrame()
    {
        yield return null;
        UI.UpdateCollectedItemsText(0, _totalCollectibleItems);
    }

    private void OnEnable()
    {
        CollectItem.OnItemCollected += ItemCollected;

        GameTimer.OnTimeUpdate += TimeUpdated;
        GameTimer.OnTimeExpired += TimeExpired;

    }

    private void TimeUpdated(int seconds) => UI.UpdateTimerText(seconds, GameplayTime);

    private void OnDisable()
    {
        CollectItem.OnItemCollected -= ItemCollected;

        GameTimer.OnTimeUpdate -= TimeUpdated;
        GameTimer.OnTimeExpired -= TimeExpired;
    }

    private void TimeExpired() => Lose();

    private void Win()
    {
        GameTimer.StopTimer();
        UI.SetGameOverText(WinText);
        Time.timeScale = 0f;
    }

    private void Lose()
    {
        UI.SetGameOverText(LoseText);
        Time.timeScale = 0f;
    }
}
