using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectable : MonoBehaviour
{
    public float rechargeSize;

    void OnTriggerEnter2D (Collider2D other)
    {
        AttackController attackController = other.gameObject.GetComponent<AttackController>();
        if (attackController != null)
        {
            attackController.AddMana(rechargeSize);
            Destroy(gameObject);
        }
    }
}
