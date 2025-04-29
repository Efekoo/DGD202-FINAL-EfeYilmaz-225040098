using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public int health = 1;

    [Header("Firing")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireInterval = 2f;
    private float fireTimer;

    [Header("Effects")]
    public GameObject explosionEffect;
    public AudioClip explosionClip;
    public AudioClip shootClip;

    void Update()
    {
        Move();
        HandleShooting();
        CheckOffScreen();
    }

    void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (enemyBulletPrefab == null || firePoint == null) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        Instantiate(enemyBulletPrefab, firePoint.position, enemyBulletPrefab.transform.rotation);

        if (shootClip != null)
            AudioSource.PlayClipAtPoint(shootClip, transform.position);
    }

    void CheckOffScreen()
    {
        if (transform.position.x < -10f)
        {
            GameManager.Instance.EnemyPassed();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0) return;

        GameManager.Instance.AddScore(1);
        GameManager.Instance.EnemyKilled();

        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 0.2f);
        }

        if (explosionClip != null)
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player?.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}