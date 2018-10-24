﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Watcher : CoreEnemy
{
    public override int Id => 1;

    public override string Name => "Watcher";

    public GameObject Eye;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Shoot()
    {
        if (player)
        {
            var dir = player.transform.position - transform.position;
            dir.Normalize();

            SpawnBulletsDirection spawnBullets = new SpawnBulletsDirection();
            spawnBullets.bulletData = shoot.data;
            spawnBullets.bulletData.position = transform.position;
            spawnBullets.bulletData.direction = dir;
            spawnBullets.Spawn();
        }
    }
}
