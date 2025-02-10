using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed;
    public float damage;
    public float launchKnockback;
    public float hitKnockback;
    public float manaCost;

    private HashSet<Collider2D> IgnoredColliders;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        rb.velocity = transform.TransformDirection(new Vector2(speed, 0f));
    }

    public void AddIgnoredCollider(Collider2D IgnoredCollider)
    {
        if (IgnoredColliders == null) IgnoredColliders = new();
        IgnoredColliders.Add(IgnoredCollider);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (!IgnoredColliders.Contains(other)) Hit(other);
    }

    private void Hit(Collider2D other)
    {
        rb.velocity = new Vector2();
        animator.SetBool("HasCollided", true);
        Destroy(GetComponent<CircleCollider2D>());

        /* When colliding with player */
        PlayerHealthController healthController = other.gameObject.GetComponent<PlayerHealthController>();
        if (healthController != null)
        {
            healthController.Hurt(damage, GetHitKnockback());
        }
        Destroy(gameObject, 0.2f);
    }

    public Vector2 GetLaunchKnockback()
    {
        Vector2 knockBackDir = transform.TransformDirection(new Vector2(-1f, 0f));
        return knockBackDir * launchKnockback;
    }

    public Vector2 GetHitKnockback()
    {
        Vector2 knockBackDir = transform.TransformDirection(new Vector2(1f, 0f));
        return knockBackDir * hitKnockback;
    }

    public void Misfire()
    {
        Destroy(gameObject);
    }
}
