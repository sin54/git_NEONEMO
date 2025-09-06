using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Color dashColor;
    public Color playerColor;
    private PlayerTrail playerTrail;
    private SpriteRenderer SR;
    private Rigidbody2D RB;
    private bool isDashing;
    private float lastPlayerDashStartTime;

    public bool canMove;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCool;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    public Vector2 direction { get; private set; }
    [SerializeField] private float noMoveRange = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerTrail = GetComponent<PlayerTrail>();
        SR = GetComponent<SpriteRenderer>();
        RB=GetComponentInParent<Rigidbody2D>();
    }
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            Move();
        }

        if (isDashing)
        {
            if (Time.time > lastPlayerDashStartTime + dashTime)
            {
                isDashing = false;
                playerTrail.makeGhost = false;
                SetColor(playerColor);
                gameObject.layer = 7;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && CanDash())
            {
                isDashing = true;
                lastPlayerDashStartTime = Time.time;
                playerTrail.ghostDestroyTime = 0.5f;
                PlayerDash();

            }
        }
    }
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            SetVelocity(moveSpeed, direction);
        }
    }
    public void SetVelocity(float speed, Vector2 direction)
    {
        RB.linearVelocity = new Vector2(speed * direction.x, speed * direction.y);
    }
    private void Move()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        if ((mousePosition - transform.position).magnitude < noMoveRange)
        {
            direction = Vector2.zero;
        }
        else
        {
            direction = (mousePosition - transform.position).normalized;
        }



        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!isDashing)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

    }
    private bool CanDash()
    {
        return Time.time > lastPlayerDashStartTime + dashCool && direction != Vector2.zero;
    }
    private void SetColor(Color color)
    {
        SR.material.color = color;
    }
    private void PlayerDash()
    {
        SetColor(dashColor);
        playerTrail.makeGhost = true;
        SetVelocity(dashSpeed, direction);
        gameObject.layer = 9;
    }
}
