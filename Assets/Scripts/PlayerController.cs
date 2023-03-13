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

    // We don't need these flags any more...
    // 
    // private bool isCharging = false;
    // private bool isDashing = false;
    // private bool isNormal = true;

    // We start by using an enum to store the available
    // states of the player
    enum State {
        Normal,
        Charging,
        Dashing        
    }

    // When then have a variable to keep track of the 
    // current state, rather than a bunch of booleans
    private State state = State.Normal;


    private Vector2 dashDirection;
    private float chargePower = 0f;
    private const float ChargeRate = 20;
    private const float ChargeFriection = 2;

    void Update()
    {
        if (state == State.Normal && Input.GetMouseButtonDown(0)) {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletController controller = bullet.GetComponent<BulletController>();
            controller.direction = direction;
            controller.firedBy = gameObject;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void FixedUpdate() 
    {
        switch (state) {
            case State.Normal:
                // Do everything we should do in the "Normal" state
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);

                rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

                // Transition to charging state when space is pressesd and 
                // we are in the normal state
                if (Input.GetKey(KeyCode.Space)) {
                    state = State.Charging;

                    // Perform anything that should happen when we transition
                    // to the charging state
                    spriteRenderer.color = Color.red;
                }

                break;
            case State.Charging:
                // Do everything we should do in the charging state:         
                 chargePower += Time.deltaTime * ChargeRate;
                 dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                 
                 // Transition to dashing state when space is released and
                 // we are in the charging state
                 if(!Input.GetKey(KeyCode.Space)) {
                     state = State.Dashing;

                    // Perform anything that should happen when we transition
                    // to the dashing state
                    rigidBody.AddForce(dashDirection.normalized * chargePower, ForceMode2D.Impulse);
                    chargePower = 0f;
                 }

                 break;
             case State.Dashing:
                // Do everything we should do in the dashing state:
                rigidBody.velocity = rigidBody.velocity * (1 - ChargeFriection * Time.deltaTime);
                transform.Rotate(0, 0, 100 * Time.deltaTime * rigidBody.velocity.magnitude);
                spriteRenderer.color = Random.ColorHSV();
                
                // Transition to normal state when we are no longer dashing
                // and we are in the dashing state
                if( rigidBody.velocity.magnitude < 0.5f ) {
                    state = State.Normal;

                    // Perform anything that should happen when we transition
                    // to the normal state
                    spriteRenderer.color = Color.white;
                    rigidBody.velocity = Vector2.zero;
                    transform.eulerAngles = Vector3.zero;
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Turret" && state == State.Dashing) {
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
                if (state == State.Dashing) {
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