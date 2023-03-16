namespace PlayerState
{

  using UnityEngine;

  public class Dashing : BaseState
  {
    
    const float DashFriction = 2;
    private float power = 0f;

    public Dashing(GameObject player, float power) : base(player)
    {
      this.power = power;
    }

    public override void OnEnter()
    {
      Vector2 dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
      rigidBody.AddForce(dashDirection.normalized * power, ForceMode2D.Impulse);
    }

    public override State OnFixedUpdate()
    {
      player.transform.Rotate(0, 0, 100 * Time.deltaTime * rigidBody.velocity.magnitude);
      spriteRenderer.color = Random.ColorHSV();

      rigidBody.velocity = rigidBody.velocity * (1 - DashFriction * Time.deltaTime);
      if (rigidBody.velocity.magnitude < 0.5f)
      {
        return new Normal(player);
      }

      return this;
    }

    public override State OnTurretCollision(GameObject turret)
    {
      GameObject.Destroy(turret);
      return this;
    }
  }
}