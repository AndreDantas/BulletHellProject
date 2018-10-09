﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;
using Sirenix.OdinInspector;
using Unity.Entities;

public class DamageSystem : ComponentSystem
{

    struct ReceiverData
    {
        public int Length;
        public ComponentArray<Health> Health;
        public ComponentArray<Faction> Faction;
        public ComponentArray<CollisionRadius> Radius;
    }

    [Inject] ReceiverData receivers;

    struct BulletData
    {
        public int Length;
        public ComponentArray<Faction> Faction;
        public ComponentArray<BulletDamage> Damage;
        public ComponentArray<Bullet> Bullet;

    }

    [Inject] BulletData bullets;

    protected override void OnUpdate()
    {
        if (receivers.Length == 0 || bullets.Length == 0)
            return;

        for (int pi = 0; pi < receivers.Length; ++pi)
        {
            var h = receivers.Health[pi];
            if (h.wasDamaged)
                h.wasDamaged = false;
            if (h.isInvincible)
                continue;
            float damage = 0f;
            float collisionRadius = receivers.Radius[pi].Value;
            float collisionRadiusSquared = collisionRadius * collisionRadius;

            Vector2 receiverPos = (Vector2)h.transform.position + receivers.Radius[pi].Offset;
            Faction.Type receiverFaction = receivers.Faction[pi].Value;

            for (int si = 0; si < bullets.Length; ++si)
            {
                if (bullets.Faction[si].Value != receiverFaction)
                {
                    Vector2 shotPos = bullets.Faction[si].transform.position;
                    Vector2 delta = shotPos - receiverPos;
                    float distSquared = math.dot(delta, delta);
                    if (distSquared <= collisionRadiusSquared)
                    {

                        damage = bullets.Damage[si].Value;

                        bullets.Bullet[si].toRemove = true;
                        break;
                    }
                }
            }


            if (damage > 0f)
            {
                h.isInvincible = true;
                h.CurrentHealth = math.max(h.CurrentHealth - damage, 0.0f);
                h.wasDamaged = true;
            }

        }
    }
}
