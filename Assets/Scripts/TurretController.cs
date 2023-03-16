using UnityEngine;

public class TurretController : MonoBehaviour
{

  private HealthController healthController;
  public GameObject bulletPrefab;
  public float fireDelay = 0f;
  public float fireSpeed = 0.5f;
  private GameObject fireTarget;
  private float timeToNextFire = 0f;

  void Start()
  {
    timeToNextFire = fireDelay * Random.value;
    healthController = GetComponent<HealthController>();
    fireTarget = GameObject.FindGameObjectWithTag("Player");
  }

  void Update()
  {
    timeToNextFire -= Time.deltaTime;
    if (timeToNextFire <= 0f)
    {
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
    if (collision.gameObject.tag == "Bullet")
    {
      BulletController bullet = collision.gameObject.GetComponent<BulletController>();
      if (bullet.firedBy.tag == "Player")
      {
        Destroy(collision.gameObject);
        healthController.TakeDamage();
        if (healthController.health <= 0)
        {
          Destroy(gameObject);
        }
      }
    }
  }
}
