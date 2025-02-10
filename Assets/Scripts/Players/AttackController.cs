using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public GameObject projectile;
    public float maxMana;
    public float manaRegenPerSecond;
    public GameObject manaBar;
    public float depletedKnockback;
    
    private Rigidbody2D rb;
    private float mana;
    private ArcadeBarController manaBarController;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        mana = maxMana; 
        if (manaBar != null)
        {
            manaBarController = manaBar.GetComponent<ArcadeBarController>();
        }
    }

    void Update()
    {
        if (mana < maxMana)
        {
            mana += manaRegenPerSecond * Time.deltaTime;
        }
        else if (mana > maxMana)
        {
            mana -= manaRegenPerSecond * Time.deltaTime * 0.25f;
        }
        if (mana > maxMana - manaRegenPerSecond * Time.deltaTime && mana < maxMana + manaRegenPerSecond * Time.deltaTime)
        {
            mana = maxMana;
        }
        UpdateManaUI();
    }

    public void AddMana(float amount) 
    { 
        mana += amount; 
        UpdateManaUI();
    }

    public void RemoveMana(float amount) 
    { 
        mana -= amount; 
        UpdateManaUI();
    }

    public Vector2 CastFireball(Quaternion angle)
    {
        /* ---- NOTE TO SELF, destorying the fireball *after* it is instantiated 
                rather than not instantiating it in the first place could be inefficient, 
                investigate */

        /* Create badass fireball */
        GameObject fireball = Instantiate(projectile, rb.position, angle);
        ProjectileController[] fireballControllers = fireball.GetComponentsInChildren<ProjectileController>();

        Vector2 totalKnockback = new Vector2();

        foreach (ProjectileController fireballController in fireballControllers)
        {
            if (fireballController.manaCost > mana)
            {
                fireballController.Misfire();
                if (angle.Equals(Quaternion.Euler(0, 0, 0))) totalKnockback += new Vector2(-depletedKnockback, 0f);
                else if (angle.Equals(Quaternion.Euler(0, 0, 180))) totalKnockback += new Vector2(depletedKnockback, 0f);
            }
            else
            {
                fireballController.AddIgnoredCollider(GetComponent<Collider2D>());
                totalKnockback += fireballController.GetLaunchKnockback();
                mana -= fireballController.manaCost;
            }
        }

        /* Pass through knockback and update UI */
        UpdateManaUI();
        return totalKnockback;
    }

    private void UpdateManaUI()
    {
        if (manaBarController != null)
        {
            manaBarController.SetDisplayPercentage(mana);
        }
    }
}
