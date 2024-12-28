using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public float specialAttackDamage = 30f;
    public float specialAttackCooldown = 5f;
    private float nextSpecialAttack = 0f;

    protected override void Move()
    {
        if (Time.time >= nextSpecialAttack)
        {
            SpecialAttack();
            nextSpecialAttack = Time.time + specialAttackCooldown;
        }
        else
        {
            base.Move();
        }
    }

    void SpecialAttack()
    {
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            playerComponent.TakeDamage(specialAttackDamage);
        }
    }
}