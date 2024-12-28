using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float fuel = 100f;
    public float maxFuel = 100f;
    public float fuelConsumption = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Canvas canvas;

    private Rigidbody2D rb;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Move();
        ConsumeFuel();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RepairSkill();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BombSkill();
        }
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.velocity = movement * moveSpeed;
    }

    void ConsumeFuel()
    {
        fuel -= fuelConsumption * Time.deltaTime;
        if (fuel <= 0)
        {
            gameManager.GameOver();
        }
    }

    void Shoot()
    {
        var clone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        clone.gameObject.transform.SetParent(canvas.transform);
    }

    void RepairSkill()
    {
        currentHp = maxHp;
    }

    void BombSkill()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(50f);
        }
    }
}