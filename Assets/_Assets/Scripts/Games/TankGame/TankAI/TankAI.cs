using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour {

    #region fields
    private TankNetworking net;

    public TankControls Controls { get; set; }
    public TankAIPlayBehaviour Behaviour { get; set; }
    public TankAIMovement Movement { get; set; }
    public TankAIShooting Shooting { get; set; }
    public TankAIBulletChecker Bullets { get; set; }
    public TankDFSPath DFS { get; set; }
    public TankAStarPath AStar { get; set; }
    #endregion

    private void Start() {
        Controls = GetComponent<TankControls>();
        net = GetComponent<TankNetworking>();

        TankGameManager.Instance().SubscribeRoundStart(ResetAI);
    }

    /// <summary>
    /// Resets the AI. This function is called before every round.
    /// </summary>
    public void ResetAI() {

        print("reset ai");

        StopAllCoroutines();

        DFS = new TankDFSPath(TankLevelGenerator.Instance.Level);
        AStar = new TankAStarPath(TankLevelGenerator.Instance.Level);

        Behaviour = new TankAIPlayBehaviour(this);
        Movement = new TankAIMovement(this);
        Bullets = new TankAIBulletChecker(this);
        Shooting = new TankAIShooting(this);

        //path = aStar.FindPath(
        //    Vector.PositionToCoords(transform.position),
        //    Vector.PositionToCoords(TankNetworking.MyTank().transform.position));

        //movement.TraversePath(path);

        TankEvents.Instance.SubscribeToEvent(BulletEvent, TankEvents.EventType.BulletEvent);
    }

    /// <summary>
    /// Called every time a bullet is shot or when a bullet bounces off a wall. Checks if a bullet will hit the AI.
    /// </summary>
    private void BulletEvent() {

        Bullets.CheckCollisionStatus();
        if (Bullets.BulletWillHit) {
            DodgeBullets();
        }
    }

    private void Update() {

        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        Controls.ResetControls();

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetAI();
        }

        // Components
        Behaviour.Update();
        Shooting.Update();
    }

    public void DodgeBullets() {

        print("Dodging bullet");

        IntCoords current = Vector.PositionToCoords(transform.position);
        Vector[] safePath = Bullets.GetPathToSafeCoords(current, AStar);
        Debug.Log("Path length; " + safePath.Length);
        Debug.Log("Path pos: " + safePath[safePath.Length - 1]);
        Movement.TraversePath(safePath);
    }
}