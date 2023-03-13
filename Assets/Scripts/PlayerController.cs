using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private HealthController healthController;
    public GameObject bulletPrefab;

    public float moveSpeed = 0f;
    public Rigidbody2D rigidBody;
    public Vector2 movement;
    public Animator animator;
    public AudioSource hitSound;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private bool isCharging = false;
    private bool isDashing = false;
    private bool isNormal = true;
    private Vector2 dashDirection;
    private float chargePower = 0f;
    private const float ChargeRate = 20;

    private const float ChargeFriection = 2;

    void FixedUpdate() 
    {
         //Holding space, and not dashing - power up charge
        if (Input.GetKey(KeyCode.Space) && !isDashing) {
            isCharging = true;
            isNormal = false;
            chargePower += Time.deltaTime * ChargeRate;
            dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            spriteRenderer.color = Color.red;
        //If if charging, but not holding space, dash
        } else if (isCharging) {
            isCharging = false;
            isDashing = true;
            isNormal = false;
           // transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, dashDirection));
            rigidBody.AddForce(dashDirection.normalized * chargePower, ForceMode2D.Impulse);
            chargePower = 0f;
        }

        if(isDashing) {
            //If dashing, go a random color
            spriteRenderer.color = Random.ColorHSV();
            rigidBody.velocity = rigidBody.velocity * (1 - ChargeFriection * Time.deltaTime);
            transform.Rotate(0, 0, 100 * Time.deltaTime * rigidBody.velocity.magnitude);
            //If we've slowed down enough, stop dashing
            if( rigidBody.velocity.magnitude < 0.5f ) {
                isDashing = false;
                isNormal = true;
                rigidBody.velocity = Vector2.zero;
                transform.eulerAngles = Vector3.zero;
            }
        }

        // Reset color when not charging or dashing
        if(isNormal) {
            spriteRenderer.color = Color.white;
        }

        //Only fire if not charging or dashing
        if (Input.GetMouseButtonDown(0) && isNormal) {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletController controller = bullet.GetComponent<BulletController>();
            controller.direction = direction;
            controller.firedBy = gameObject;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        
        // Only move if not charging or dashing
        if(isNormal) {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
      
            rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Turret" && isDashing) {
           Destroy(collision.collider.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet") {
            BulletController controller = collider.gameObject.GetComponent<BulletController>();
            if (controller.firedBy.tag == "Turret") {
                Destroy(collider.gameObject);
                //Dont take damange from bullet when dashing
                if (isDashing) {
                     return;
                 }
                healthController.TakeDamage();
                 if(healthController.health > 0) {
                    hitSound.Play();
                 }
            }
        }
    }
}