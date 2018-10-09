﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.Entities;
public class ShootCooldownSystem : ComponentSystem
{

    struct Data
    {

        public int Length;
        public ComponentArray<Shoot> Shoot;
    }

    [Inject] Data data;

    protected override void OnUpdate()
    {
        for (int i = data.Length - 1; i >= 0; i--)
        {
            var shoot = data.Shoot[i];
            if (shoot.CanShoot)
                continue;
            shoot.elapsedCooldownTime += Time.deltaTime;

            if (shoot.elapsedCooldownTime >= shoot.Cooldown)
            {
                shoot.CanShoot = true;
                shoot.elapsedCooldownTime = 0f;
            }
        }
    }
}