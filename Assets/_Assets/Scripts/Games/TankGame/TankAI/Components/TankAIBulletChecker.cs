using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to calculate all the future trajectories of bullets
/// </summary>
public class TankAIBulletChecker : TankAIComponent, IBulletCountHolder {

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

    /// <summary>
    /// Calculates the trajectories of all bullets and saves which how many bullets will pass over a certain cell
    /// </summary>
    public void CheckCollisionStatus() {

        BulletWillHit = false;

        ResetCellBulletCounts();

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

        TankLevelCellVisualizer.VisualizeCells(CellBulletCounts);
    }

    /// <summary>
    /// Returns a path to safe coordinates which no bullet will pass over
    /// </summary>
    /// <param name="current"></param>
    /// <param name="pf"></param>
    /// <returns></returns>
    public Vector[] GetPathToSafeCoords(IntCoords current, TankAIPathfinding pf) {

        bool FoundCondition(IntCoords c) {
            return CellBulletCounts[c.x, c.y] == 0;
        }

        return pf.FindPath(current, new IntCoords(-1, -1), FoundCondition);
    }

    public void ResetCellBulletCounts() {
        CellBulletCounts = new byte[TankSettings.LevelWidth, TankSettings.LevelHeight];
    }
}