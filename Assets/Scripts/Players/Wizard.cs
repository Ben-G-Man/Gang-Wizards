using UnityEngine;

public class Wizard : MonoBehaviour
{ 
    public GameObject wizardController;
    public IMovementController movementController;
    public IHealthController healthController;
    public IAttackController attackController;
}