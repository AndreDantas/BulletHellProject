﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Level : MonoBehaviour
{

    public LevelDetails levelDetails;
    protected bool levelStarted;
    [SerializeField, ReadOnly]
    protected bool spawningEnemies;
    protected List<GameObject> activeEnemies = new List<GameObject>();
    protected int currentWave = 0;


    private void Start()
    {
        StartLevel();
    }

    public virtual void StartLevel()
    {
        if (levelDetails)
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                Destroy(activeEnemies[i]);
            }
            activeEnemies.Clear();
            currentWave = 0;
            levelStarted = true;
            StartCoroutine(SpawnWave());
        }
    }

    protected virtual IEnumerator SpawnWave()
    {
        if (!levelDetails || spawningEnemies || (!levelDetails?.waves?.ValidIndex(currentWave) ?? true))
            yield break;

        spawningEnemies = true;

        var wave = levelDetails.waves[currentWave];

        if (!wave.skipWave)
        {
            yield return new WaitForSeconds(wave.waveDelay);
            for (int i = 0; i < wave?.spawns?.Count; i++)
            {

                var spawn = wave.spawns[i];
                if (spawn.enemyPrefab)
                {
                    yield return new WaitForSeconds(spawn.spawnDelay);

                    var enemy = Instantiate(spawn.enemyPrefab).GetComponent<Enemy>();
                    Vector2 start, end;
                    if (spawn.scaleToScreenSize)
                    {
                        var uq = new UnitaryQuadrant(UtilityFunctions.ScreenWidth, UtilityFunctions.ScreenHeight);
                        start = uq.GetPoint(spawn.startPoint);
                        end = uq.GetPoint(spawn.endPoint);
                    }
                    else
                    {
                        start = spawn.startPoint;
                        end = spawn.endPoint;
                    }
                    activeEnemies.Add(enemy.gameObject);

                    enemy.active = false;
                    enemy.gameObject.Activate();
                    enemy.transform.position = start;

                    var t = enemy.transform.MoveTo(end, spawn.moveTime, EasingEquations.GetEquation(spawn.moveEquation));
                    t.destroyOnDisable = true;
                    t.easingControl.completedEvent += (object sender, System.EventArgs args) => enemy.active = true;
                }
            }

        }

        spawningEnemies = false;
        currentWave++;
    }

    public bool LevelComplete()
    {
        return currentWave >= levelDetails?.waves?.Count;
    }

    private void Update()
    {

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (!activeEnemies[i].activeSelf)
            {
                Destroy(activeEnemies[i]);
                activeEnemies.RemoveAt(i);

            }
        }

        if (!levelStarted)
            return;

        if (levelStarted && !spawningEnemies && activeEnemies?.Count == 0)
        {
            StartCoroutine(SpawnWave());
        }

        if (activeEnemies?.Count == 0 && !spawningEnemies && LevelComplete())
            levelStarted = false;
    }
}