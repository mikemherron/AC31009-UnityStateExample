namespace PlayerState
{

  using UnityEngine;

  public class Charging : BaseState
  {
    const float ChargeRate = 20f;

    private float chargePower = 0f;

    public Charging(GameObject player) : base(player) { }

    public override void OnEnter()
    {
      player.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override State OnFixedUpdate()
    {
      chargePower += Time.deltaTime * ChargeRate;
      // When we LET GO OF SPACE, transition CHARGING->DASHING
      if (!Input.GetKey(KeyCode.Space))
      {
        return new Dashing(player, chargePower);
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