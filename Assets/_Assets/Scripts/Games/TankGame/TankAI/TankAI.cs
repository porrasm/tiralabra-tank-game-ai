using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour {

    #region fields
    private TankControls controls;
    private TankNetworking net;

    private TankDFSPath dfs;
    private TankAStarPath aStar;

    private IntCoords[] path;

    private TankAIMovement movement;
    #endregion

    private void Start() {
        controls = GetComponent<TankControls>();
        net = GetComponent<TankNetworking>();

        TankGameManager.Instance().SubscribeRoundStart(ResetAI);
    }

    public void ResetAI() {

        print("reset ai");

        StopAllCoroutines();

        dfs = new TankDFSPath();
        aStar = new TankAStarPath(TankLevelGenerator.Level);
        path = null;

        movement = new TankAIMovement(this);

        path = aStar.FindPath(Vector.PositionToCoords(transform.position),
            Vector.PositionToCoords(TankNetworking.MyTank().transform.position));

        movement.TraversePath(path);
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