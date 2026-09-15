using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки скорости")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Способ управления")]
    [Tooltip("true - клавиатура и мышка, а false - геймпад")]
    [SerializeField] private bool inputDevise = true;
    // true - клавиатура и мышка, а false - геймпад.

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool facingRight = false;
    // false = смотрит влево (клавиша "D"), true = вправо (клавиша "A").

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputController();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    private void InputController()
    {
        movement = Vector2.zero;

        if (inputDevise == true)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                movement.y = 1f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                movement.y = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                movement.x = 1f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                movement.x = -1f;
            }
        }

        if (inputDevise == false)
        {
            movement = Vector2.zero;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
        }

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        if (movement.x > 0 && facingRight == false)
        {
            Flip();
        }
        if (movement.x < 0 && facingRight == true)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}