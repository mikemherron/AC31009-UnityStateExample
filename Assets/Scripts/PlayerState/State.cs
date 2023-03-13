using UnityEngine;

namespace PlayerState {
    public interface State {
        
        // On Enter should implement any one-off logic that needs to happen when
        // the state is entered.
        void OnEnter();
        
        // On Update should implement any logic that needs to happen every frame.
        State OnUpdate();
        
        // On FixedUpdate should implement any logic that needs to happen every fixed frame.
        State OnFixedUpdate();
        
        // On Turret Collision should implement any logic that needs to happen when
        // the player collides with a turret.
        State OnTurretCollision(GameObject turret);
        
        // On Bullet Collision should implement any logic that needs to happen when
        // the player collides with a bullet.
        State OnBulletCollision();
    }
}