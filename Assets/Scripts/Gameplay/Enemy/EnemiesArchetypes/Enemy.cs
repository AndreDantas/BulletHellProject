﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.Entities;
public abstract class Enemy : MonoBehaviour
{
    public bool active = true;
    [SerializeField]
    protected Shoot shoot;

    protected virtual void Awake()
    {
        if (!shoot)
            shoot = GetComponent<Shoot>();
    }

    protected virtual void Update()
    {
        if (!active)
            return;
        RequestShoot();

    }

    public void RequestShoot()
    {
        if (!active)
            return;
        if (shoot?.CanShoot ?? false)
        {

            Shoot();
            shoot.CanShoot = false;
        }
    }

    protected abstract void Shoot();

}
