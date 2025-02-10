using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public float maxHealth;
    public GameObject healthBar;

    private float health;
    private ArcadeBarController healthBarController;

    void Start() 
    { 
        health = maxHealth; 
        if (healthBar != null)
        {
            healthBarController = healthBar.GetComponent<ArcadeBarController>();
        }
    }

    public float GetHealth() { return health; }

    public void Hurt(float amount)
    {
        Heal(-amount);
        Debug.Log("PLAYER " + GameEventManager.GetPlayerNumber(gameObject) + " TAKES " + amount + " DAMAGE! REMANINING HEALTH: " + health);
        DeathCheck();
    }

    public void Hurt(float amount, Vector2 knockback)
    {
        Hurt(amount);
        GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0f, maxHealth);
        if (healthBarController != null)
        {
            healthBarController.SetDisplayPercentage(health);
        }
    }

    private void DeathCheck()
    {
        if (health == 0f) PlayerDeath();
    }

    private void PlayerDeath()
    {
        gameObject.GetComponent<PlayerMovement>().DisableControl();
        GameEventManager.OnPlayerDeath(gameObject);
        gameObject.GetComponent<Animator>().Play("dying");
    }
}
