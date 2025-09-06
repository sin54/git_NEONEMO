using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;
using InventorySystem;
using Random=UnityEngine.Random;
using Core;
using Type;

public class Spawner : MonoBehaviour
{
    public static event Action OnMassEnemyDeath;
    public static event Action OnEndDay;

    [SerializeField] private SO_LevelData[] allDayData; // 하루 = SO_LevelData 하나
    [SerializeField] private TypeManager TM;
    [SerializeField] private Image waveImg;

    [SerializeField]private int nowDay = 0;
    [SerializeField]private int currentWave = 0;

    [SerializeField] private TMP_Text nowWaveTxt;
    [SerializeField] private TMP_Text nowDayTxt;

    private float remainingWaveTime;
    private float lastSpawnTime;
    private bool nextProcessing;
    private bool isDayFinished;

    private void Start()
    {
        InitWave();
        SetText();
        /*

        */
    }

    private void Update()
    {
        if (nowDay >= allDayData.Length) return;

        var dayData = allDayData[nowDay];
        if (currentWave >= dayData.waveCount)
        {
            // 웨이브가 모두 끝났고, 스페이스키 입력을 기다리는 중
            if (isDayFinished && nextProcessing)
            {
                nowDay++;
                if (nowDay < allDayData.Length)
                {
                    InitWave();
                }
                return;
            }
            return;
        }

        remainingWaveTime -= Time.deltaTime;
        waveImg.fillAmount = remainingWaveTime / dayData.spawnDatas[currentWave].waveLength;

        if (remainingWaveTime <= 0 && !nextProcessing)
        {
            nextProcessing = true;
            StartCoroutine(GoToNextWave());
        }

        if (Time.time > lastSpawnTime + dayData.spawnDatas[currentWave].timeBetweenSpawn && remainingWaveTime > 2.5f)
        {
            Spawn(dayData.spawnDatas[currentWave].enemyToSpawn);
            lastSpawnTime = Time.time;
        }
    }

    private void InitWave()
    {
        var dayData = allDayData[nowDay];
        currentWave = 0;
        remainingWaveTime = dayData.spawnDatas[currentWave].waveLength;
        nextProcessing = false;
        SetText();
    }

    private IEnumerator GoToNextWave()
    {
        OnMassEnemyDeath?.Invoke();
        TM.ShowText();
        OnEndDay?.Invoke();
        yield return new WaitForSeconds(TM.fullText.Length * TM.delay + 1f);
        TM.DeleteText();

        currentWave++;
        var dayData = allDayData[nowDay];

        if (currentWave >= dayData.waveCount)
        {
            isDayFinished = false;

            yield return new WaitUntil(() => (!GameManager.Instance.levelManager.isUpgrading));
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.ChangeToDay();
            yield break;
        }

        remainingWaveTime = dayData.spawnDatas[currentWave].waveLength;
        nextProcessing = false;
        SetText();
    }
    private void SetText()
    {
        nowDayTxt.text = "DAY " + (nowDay + 1).ToString();
        nowWaveTxt.text = "WAVE " + (currentWave + 1).ToString() + "/" + allDayData[nowDay].waveCount.ToString();
    }
    void Spawn(EnemyPercentage[] spawnType)
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        float cumulative = 0f;

        int selectedEnemyNum = spawnType[0].enemySpawnData.enemyPrefabNum;
        int idx = 0;

        foreach (var sp in spawnType)
        {
            cumulative += sp.percent;
            if (rand <= cumulative)
            {
                selectedEnemyNum = sp.enemySpawnData.enemyPrefabNum;
                break;
            }
            idx++;
        }

        Vector2 spawnPos = SetRandomSpawnPos();
        GameObject marker = GameManager.Instance.poolManager.Get(35);
        marker.transform.position = spawnPos;
        marker.transform.localScale = Vector3.one * spawnType[idx].enemySpawnData.markerSize;

        StartCoroutine(SpawnEnemy(spawnPos, selectedEnemyNum, 2f));
    }

    Vector2 SetRandomSpawnPos()
    {
        float chance = UnityEngine.Random.value; // 0~1 사이

        Vector2 randPoint;

        if (chance < 0.6f)
        {
            // 80% 확률로 원의 경계: 정규화된 벡터 사용
            randPoint = UnityEngine.Random.insideUnitCircle.normalized;
            return (GameManager.Instance.borderRadius+Random.Range(-0.5f,1.75f)) * randPoint;
        }
        else
        {
            // 20% 확률로 원 내부
            randPoint = UnityEngine.Random.insideUnitCircle;
            return GameManager.Instance.borderRadius * randPoint;
        }
    }

    IEnumerator SpawnEnemy(Vector2 pos, int selENum, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject enemy = GameManager.Instance.poolManager.Get(selENum);
        enemy.transform.position = pos;
    }
    public void EndDay() => isDayFinished = true;
}
