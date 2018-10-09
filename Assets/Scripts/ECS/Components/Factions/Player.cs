﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class Player : MonoBehaviour
{

    [ReadOnly] public Vector2 touchOrigin;
    [ReadOnly] public Vector2 playerOrigin;
    [ReadOnly] public Vector2 position;
    [ReadOnly] public Vector3 refVelocity;

    public float speed = 10f;
    public float smoothing = 0.1f;
    public float tilt = 20f;
    public bool moving = false;

    [ReadOnly] public int pointerID;
}
