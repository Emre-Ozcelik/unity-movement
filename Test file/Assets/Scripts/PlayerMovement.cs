using UnityEngine;
 
public class PlayerMovement : MonoBehaviour
{ 
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask WallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    
    
 
    private void Awake()
    {
        //get references from animator and object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
 
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        //flips player
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if(horizontalInput > -0.01f)
            transform.localScale = new Vector3(-1,1,1);
 
        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();
        
        //set animator parameters
        anim.SetBool("run",horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump
        if(wallJumpCoolDown < 0.2f)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

            if(onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;
        }
        else 
            wallJumpCoolDown += Time.deltaTime;

        
    }
    private void Jump()
    {
        if(isGrounded())
        {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("jump");
        }
        else
            body.gravityScale = 3;
        
        if(Input.GetKey(KeyCode.Space))
            Jump();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, WallLayer);
        return raycastHit.collider != null;
    }


}