using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class BreakdownManager : MonoBehaviour
{
    [Header("Настройки сложности")]
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float spawnInterval = 15f;

    [Header("Таймер")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float startTime = 30f;

    [Header("Проигрыш")]
    [SerializeField] private GameObject gameOverPanel;

    private float timeLeft;
    private int maxNumBreakdowns;
    private int brokenCount;
    private List<BreakdownPoint> _points = new List<BreakdownPoint>();
    private Coroutine _spawnCoroutine;

    void Start()
    {
        timeLeft = startTime;

        _points.AddRange(FindObjectsOfType<BreakdownPoint>());


        maxNumBreakdowns = Mathf.Max(maxNumBreakdowns, _points.Count);
        _spawnCoroutine = StartCoroutine(SpawnBreakdownsRoutine());
    }

    private void Update()
    {
        SetTimer();
    }

    private IEnumerator SpawnBreakdownsRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            brokenCount = _points.Count(point => point.IsBroken);

            if (brokenCount < maxNumBreakdowns)
            {
                var availablePoints = _points.Where(point => !point.IsBroken).ToList();
                
                if (availablePoints.Count > 0)
                {
                    BreakdownPoint pointToBreak = availablePoints[Random.Range(0, availablePoints.Count)];
                    pointToBreak.Break();
                    Debug.Log($"Новая поломка! Всего сломано: {brokenCount + 1}/{maxNumBreakdowns}");
                    GameOver();
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnDestroy()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
    }

    private void GameOver()
    {
        if (brokenCount >= 5)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void Win()
    {
        if (brokenCount <= 5 && timeLeft <= 0)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void SetTimer()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
            timeLeft = 0;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text = string.Format("Time {0:00}:{1:00}", minutes, seconds);
    }
}