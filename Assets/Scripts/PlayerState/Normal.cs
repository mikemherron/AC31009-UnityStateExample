namespace PlayerState
{

  using UnityEngine;

  public class Normal : BaseState
  {
    const float MoveSpeed = 5f;

    private GameObject bulletPrefab;
    private Animator animator;
    private Vector2 movement;
    
    public Normal(GameObject player) : base(player)
    {
      this.bulletPrefab = player.GetComponent<PlayerController>().bulletPrefab;
      this.animator = player.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
      spriteRenderer.color = Color.white;
      player.transform.eulerAngles = Vector3.zero;
      rigidBody.velocity = Vector2.zero;
    }

    public override State OnUpdate()
    {
      if (Input.GetMouseButtonDown(0))
      {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);

        BulletController controller = bullet.GetComponent<BulletController>();
        controller.direction = direction.normalized;
        controller.firedBy = player;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
      }

      return this;
    }

    public override State OnFixedUpdate()
    {
      movement.x = Input.GetAxisRaw("Horizontal");
      movement.y = Input.GetAxisRaw("Vertical");

      animator.SetFloat("Horizontal", movement.x);
      animator.SetFloat("Vertical", movement.y);
      animator.SetFloat("Speed", movement.sqrMagnitude);

      rigidBody.MovePosition(rigidBody.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);

      // When we HOLD SPACE, tansition NORMAL->CHARGING
      if (Input.GetKey(KeyCode.Space))
      {
        return new Charging(player);
      }

      return this;
    }

    public override State OnBulletCollision()
    {
      healthController.TakeDamage();
      if (healthController.health > 0)
      {
        playerController.hitSound.Play();
      }
      return this;
    }
  }

}