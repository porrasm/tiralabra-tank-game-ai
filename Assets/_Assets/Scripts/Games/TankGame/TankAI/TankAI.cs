using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour {

    #region fields
    private TankControls controls;
    private TankNetworking net;

    private TankDFSPath dfs;
    private TankAStarPath aStar;

    private Vector[] path;

    private TankAIMovement movement;
    private TankAIBulletChecker bullets;
    #endregion

    private void Start() {
        controls = GetComponent<TankControls>();
        net = GetComponent<TankNetworking>();

        TankGameManager.Instance().SubscribeRoundStart(ResetAI);
    }

    /// <summary>
    /// Resets the AI. This function is called before every round.
    /// </summary>
    public void ResetAI() {

        print("reset ai");

        StopAllCoroutines();

        dfs = new TankDFSPath(TankLevelGenerator.Instance.Level);
        aStar = new TankAStarPath(TankLevelGenerator.Instance.Level);
        path = null;

        movement = new TankAIMovement(this);
        bullets = new TankAIBulletChecker(this);

        path = aStar.FindPath(Vector.PositionToCoords(transform.position),
            Vector.PositionToCoords(TankNetworking.MyTank().transform.position));

        movement.TraversePath(path);

        TankEvents.Instance.SubscribeToEvent(BulletEvent, TankEvents.EventType.BulletEvent);
    }

    private void BulletEvent() {

        print("Bullet event");

        bullets.CheckCollisionStatus();
        if (bullets.BulletWillHit) {
            DodgeBullets();
        }
    }

    private void Update() {

        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        ResetControls();


        if (Input.GetKeyDown(KeyCode.R)) {
            ResetAI();
        }
    }

    private void DodgeBullets() {

        print("Dodging bullet");

        IntCoords current = Vector.PositionToCoords(transform.position);
        movement.TraversePath(bullets.GetPathToSafeCoords(current, aStar));
    }

    private void ResetControls() {
        controls.ResetControls();
    }


}

public class AISettings {
    public static float TurnAngleLimit = 10f;
    public static float DistanceLimit = 0.5f;
    public static float MovementAngle = 15;

    #region Stuck settings
    public static float StuckTresholdTime = 0.2f;
    public static float StuckTresholdDistance = 0.05f;
    public static float AllowedStuckTime = 0.2f;
    public static float StuckCooldown = 0.5f;
    #endregion
}