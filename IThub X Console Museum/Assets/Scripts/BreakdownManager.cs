using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakdownManager : MonoBehaviour
{
    [Header("Настройки сложности")]
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private GameObject gameOverPanel;


    private int maxNumBreakdowns;
    private int brokenCount;
    private List<BreakdownPoint> _points = new List<BreakdownPoint>();
    private Coroutine _spawnCoroutine;

    void Start()
    {
        _points.AddRange(FindObjectsOfType<BreakdownPoint>());

        if (_points.Count == 0)
        {
            Debug.LogWarning("BreakdownManager: на сцене нет ни одной BreakdownPoint!");
            return;
        }

        maxNumBreakdowns = Mathf.Max(maxNumBreakdowns, _points.Count);
        _spawnCoroutine = StartCoroutine(SpawnBreakdownsRoutine());
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
            Debug.Log("Ошибок слишком много(");
        }
    }
}