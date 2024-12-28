using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float attackRange = 10f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    protected Transform player;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>().transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
        
        Move();
    }

    protected virtual void Move()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    protected virtual void Attack()
    {
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        gameManager.AddScore(10);
        base.Die();
    }
}