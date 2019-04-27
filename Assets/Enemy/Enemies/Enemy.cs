﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Enemy : MonoBehaviour
{
    public SpawnEffect spawnEffect;

    new Rigidbody rigidbody;
    PlayerController targetPlayer;
    Rigidbody targetPlayerRigidbody;
    Transform currentTarget;
    BoxCollider boxCollider;

    const float MaxSpeed = 500f;
    const float MinSpeed = 1000f;
    float speed = MinSpeed;

    public abstract int PointValue { get; }

    public void Initialize(PlayerController playerController, Rigidbody playerRigidbody)
    {
        targetPlayer = playerController;
        targetPlayerRigidbody = playerRigidbody;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPlayerRigidbody.rotation, 1.0f);
    }

    void Start()
    {
        speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        rigidbody = GetComponentInParent<Rigidbody>();
        boxCollider = GetComponentInParent<BoxCollider>();
        Assert.IsNotNull(rigidbody);
        rigidbody.useGravity = false;
        IgnoreCollisions(true);
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = currentTarget.position - transform.position;
        direction = direction.ResetY();
        direction.Normalize();
        rigidbody.AddForce(direction * speed * Time.deltaTime);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity.normalized), Time.deltaTime * 5f);
    }

    public abstract void Die(Vector3 deathVector);

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ReleaseFromSpawner()
    {
        IgnoreCollisions(false);
        SetTarget(targetPlayerRigidbody.transform);
        rigidbody.useGravity = true;
    }

    void IgnoreCollisions(bool ignore)
    {
        boxCollider.enabled = !ignore;
    }

    public virtual void DeathEffect(Vector3 direction)
    {
        GameObject go = Instantiate(spawnEffect.gameObject);
        go.transform.position = transform.position;

        go.GetComponent<SpawnEffect>().Spawn(direction.normalized, 10);
    }
}

