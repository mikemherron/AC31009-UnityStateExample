namespace PlayerState
{

  using UnityEngine;


  // BaseState is a base class for all of the player's states. It provides
  // access to the player's components and implements empty versions of the 
  // state's methods so that we don't have to implement them in every state.
  // States only need to implement the methods that they need.
  public abstract class BaseState : State
  {
    protected GameObject player;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected HealthController healthController;
    protected PlayerController playerController;

    public BaseState(GameObject player)
    {
      this.player = player;
      this.spriteRenderer = player.GetComponent<SpriteRenderer>();
      this.healthController = player.GetComponent<HealthController>();
      this.playerController = player.GetComponent<PlayerController>();
      this.rigidBody = player.GetComponent<Rigidbody2D>();
    }

    public virtual void OnEnter()
    {

    }

    public virtual State OnUpdate()
    {
      return this;
    }

    public virtual State OnFixedUpdate()
    {
      return this;
    }

    public virtual State OnTurretCollision(GameObject turret)
    {
      return this;
    }

    public virtual State OnBulletCollision()
    {
      return this;
    }
  }

}