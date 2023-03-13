using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource hitSound;
    public GameObject bulletPrefab;

    // Store the current State
    private PlayerState.State state;

    void Start()
    {
        // Start with the Normal state
        state = new PlayerState.Normal(gameObject);
    }

    void Update()
    {
        // On update, call the OnUpdate method of the current state. 
        HandleNewState(state.OnUpdate(), state);
    }

    void FixedUpdate() 
    {
        // On FixedUpdate, call the OnFixedUpdate method of the current state.
        HandleNewState(state.OnFixedUpdate(), state);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Turret") {
            // When we collide with a turret, call the OnTurretCollision method of the current state.
            HandleNewState(state.OnTurretCollision(collision.gameObject), state);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Bullet") {
            BulletController controller = collider.gameObject.GetComponent<BulletController>();
            if (controller.firedBy.tag == "Turret") {
                // When we collide with a bullet, call the OnBulletCollision method of the current state.
                HandleNewState(state.OnBulletCollision(), state);
                Destroy(collider.gameObject);
            }
        }
    }
    
    // HandleNewState takes in the new state and the old state. If the new state is
    // a different state, set that as the current state and call OnEnter so the 
    // new state can perform any initialization it needs.
    void HandleNewState(PlayerState.State newState, PlayerState.State oldState)
    {
        if (newState != oldState) {
            state = newState;
            state.OnEnter();
        }
    }
}