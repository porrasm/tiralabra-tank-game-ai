using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIShooting : TankAIComponent {

    #region fields
    private IBulletCountHolder checker;

    private struct Container : IBulletCountHolder {

        public Container(TankAI ai) {
            AI = ai;
            CellBulletCounts = null;
        }

        public TankAI AI { get; set; }

        public byte[,] CellBulletCounts { get; set; }

        public void ResetCellBulletCounts() {
            CellBulletCounts = new byte[TankSettings.LevelWidth, TankSettings.LevelHeight];
        }
    }

    public int Shot { get; set; }
    #endregion

    public TankAIShooting(TankAI ai) : base(ai) {
        checker = new Container(ai);
        checker.ResetCellBulletCounts();
    }

    public override void Update() {
        CheckCurrentShot();
    }

    private void CheckCurrentShot() {

        TankBulletTrajectory trajectory = new TankBulletTrajectory(
            checker,
            TankSettings.BulletBounces,
            new Vector(ai.transform.position),
            new Vector(ai.transform.forward),
            false);

        trajectory.CalculateTrajectory();

        if (trajectory.HitPlayer && !trajectory.HitAI) {
            trajectory = new TankBulletTrajectory(
            checker,
            TankSettings.BulletBounces,
            new Vector(ai.transform.position),
            new Vector(ai.transform.forward),
            false);

            trajectory.CalculateTrajectory();

            if (trajectory.HitPlayer && !trajectory.HitAI) {
                Shot = 2;
            } else {
                Shot = 1;
                Shoot();
            }
        } else {
            Shot = 0;
        }
    }

    private void Shoot() {
        //ai.Controls.ProcessControl(TankControls.Control.Fire);
    }
}
