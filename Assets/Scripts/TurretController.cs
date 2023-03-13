using UnityEngine;

public class TurretController : MonoBehaviour
{
    // Turret can also use te new HealthController component
    private HealthController healthController;
    public GameObject bulletPrefab;

    public float fireDelay = 0f;

    private float timeToNextFire = 0f;

    public float fireSpeed = 0.5f;

    private GameObject fireTarget;

    void Start()
    {
        timeToNextFire = fireDelay * Random.value;
        healthController = GetComponent<HealthController>();
        // Note this has changed from the last demo - we now look up the
        // player by tag rather than requiring the reference to be provided
        // in the editor. This makes it easier to reuse the turret prefab at the
        // expense of a static reference to the player.
        fireTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextFire -= Time.deltaTime;
        if (timeToNextFire <= 0f) {
            timeToNextFire = fireDelay;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Vector2 direction = fireTarget.transform.position - transform.position;
            direction.Normalize();

            BulletController controller = bullet.GetComponent<BulletController>();
            controller.direction = direction;
            controller.speed = fireSpeed;
            controller.firedBy = gameObject;
            
            bullet.GetComponent<SpriteRenderer>().color = Color.red;
        }        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet") {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            if (bullet.firedBy.tag == "Player") {
                Destroy(collision.gameObject);
                healthController.TakeDamage();
                if (healthController.health <= 0) {
                    Destroy(gameObject) ;
                }
            }
        }
    }
}
