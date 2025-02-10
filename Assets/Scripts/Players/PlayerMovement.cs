using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* ---- Input Actions ---- */
    public InputAction jump;
    public InputAction walk;
    public InputAction crouch;
    public InputAction attack;
    public LayerMask groundLayer;

    /* ---- Reference variables ---- */
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator animator;
    private AttackController attackController;

    /* ---- Movement and physics variables ---- */
    private static readonly float jumpSpeed = 14f;
    private static readonly float walkAcceleration = 1f;
    private static readonly float walkSpeed = 6f;
    private static readonly float groundDrag = 6f;
    private static readonly float airDrag = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
    }

    void OnEnable()
    {
        jump.Enable();
        jump.performed += Jump;
        attack.Enable();
        attack.performed += Attack;
        walk.Enable();
        crouch.Enable();
    }
    
    void OnDisable()
    {
        jump.Disable();
        jump.performed -= Jump;
        attack.Disable();
        attack.performed -= Attack;
        walk.Disable();
        crouch.Disable();
    }

    void Update()
    {
        Walk();
        UpdateDrag();
        UpdateAnim();
    }

    private void Walk()
    {
        float walkDir = walk.ReadValue<float>();
        rb.velocity = new Vector2(GetWalkSpeed(rb.velocity.x, walkDir), rb.velocity.y);
        if (walkDir != 0) transform.localScale = new Vector2(walkDir, 1);
    }

    private void UpdateDrag()
    {
        if (IsGrounded()) rb.drag = groundDrag;
        else rb.drag = airDrag;
    }

    private void UpdateAnim()
    {
        animator.SetBool("Grounded", IsGrounded());
        animator.SetBool("Moving", walk.ReadValue<float>() != 0f);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && !IsCrouched()) rb.velocity += new Vector2(0, jumpSpeed);
        else CastFireball(Quaternion.Euler(0, 0, 270));
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Quaternion attackDir;
        if (IsCrouched()) attackDir = Quaternion.Euler(0, 0, 90);
        else 
        {
            attackDir = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 0 : 180);
            animator.Play("attacking");
        }
        CastFireball(attackDir);
    }

    public void DisableControl()
    {
        Destroy(attackController);
        Destroy(GetComponent<PlayerHealthController>());
        Destroy(this);
    }

    private void CastFireball(Quaternion attackDir)
    {
        rb.AddForce(attackController.CastFireball(attackDir), ForceMode2D.Impulse);
    }

    /* ---- Helper functions ---- */
    private float GetWalkSpeed(float currentSpeed, float walkDir)
    {
        float walkVelocity = walkSpeed * walkDir;
        float dirWalkAcceleration = walkAcceleration * walkDir;
        
        if (IsFloatInRange(currentSpeed, walkVelocity, dirWalkAcceleration))
        {
            return walkVelocity;
        }
        else
        {
            return currentSpeed + dirWalkAcceleration;
        }
    }

    private bool IsFloatInRange(float value, float target, float range)
    {
        return Mathf.Abs(value - target) < Mathf.Abs(range);
    }

    public bool IsCrouched() { return crouch.ReadValue<float>() != 0f; }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - GetGroundDistance()), Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    public float GetGroundDistance()
    {
        if (collider == null) return 0f;
        return collider.size.y * 0.5f * Mathf.Abs(transform.lossyScale.y);
    }
}
