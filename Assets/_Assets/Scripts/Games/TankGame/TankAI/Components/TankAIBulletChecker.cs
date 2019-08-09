using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIBulletChecker : TankAIComponent {

    #region fields

    private TankBulletTrajectory[] trajectories;

    public byte[,] CellBulletCounts { get; private set; }
    public bool BulletWillHit { get; set; }
    #endregion

    public TankAIBulletChecker(TankAI ai) : base(ai) {
    }

    public override void Update() {
        CheckCollisionStatus();
    }

    public void CheckCollisionStatus() {

        BulletWillHit = false;

        CellBulletCounts = new byte[TankSettings.LevelWidth, TankSettings.LevelHeight];

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        trajectories = new TankBulletTrajectory[bullets.Length];

        for (int i = 0; i < bullets.Length; i++) {
            TankBulletTrajectory t = new TankBulletTrajectory(this, bullets[i].GetComponent<TankBullet>());
            t.CalculateTrajectory();

            TankPathVisualizer.DrawRoute(t.Trajectory);

            if (t.HitAI) {
                BulletWillHit = true;
            }
        }
    }
    public Vector[] GetPathToSafeCoords(IntCoords current, TankAIPathfinding pf) {

        bool FoundCondition(IntCoords c) {
            return CellBulletCounts[c.x, c.y] == 0;
        }

        return pf.FindPath(current, new IntCoords(-1, -1), FoundCondition);
    } 
}