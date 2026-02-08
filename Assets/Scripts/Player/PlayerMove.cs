using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;



public class PlayerMove : MonoBehaviour, IAnimatableCharacter
{
    public bool isFalling { get; private set; }
    public float moveDirection { get; private set; }
    
    public event Action OnJump;
    public event Action OnLand;
    public event Action<CharacterPartType> OnSwipeDown;
    public event Action<CharacterPartType> OnSwipeUp;

    public event Action<CharacterPartType> OnPartAttached;
    public event Action<CharacterPartType> OnPartDetached;
    
    public float jumpForce = 5.0f;
    public float moveSpeed = 5.0f;

    [SerializeField] private bool isGrounded;
    private Rigidbody2D rb;
    private Collider2D col;
    private BodyManager bm;
    public LayerMask groundLayer; // ���⿡ �� ���̾� ����

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bm = GetComponent<BodyManager>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        isFalling = rb.linearVelocityY < 0f;
        
        float rayLength = col.bounds.extents.y - col.offset.y + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1.3f, groundLayer);
    
        bool currentGrounded = (hit.collider != null);
        
        Debug.DrawRay(transform.position, Vector2.down * rayLength, currentGrounded ? Color.green : Color.red);
        
        if (!isGrounded && currentGrounded && rb.linearVelocity.y <= 0.1f)
        {
            Debug.Log("Landed!");
            OnLand?.Invoke();
        }
        
        isGrounded = currentGrounded;
        
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                OnJump?.Invoke();
                Jump();
            }
        }
    }
    
    
    public void DetachPart(CharacterPartType part)
    {
        OnPartDetached?.Invoke(part);
    }

    void FixedUpdate()
    {

        float h = UnityEngine.Input.GetAxisRaw("Horizontal");
        moveDirection = h;
        float speedMultiplier = bm.GetMovementSpeedMultiplier();
        float appliedSpeed = moveSpeed * speedMultiplier;

        rb.linearVelocity = new Vector2(h * appliedSpeed, rb.linearVelocityY);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }
}
