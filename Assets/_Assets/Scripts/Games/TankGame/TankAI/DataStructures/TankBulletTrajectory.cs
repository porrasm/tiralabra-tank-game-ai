using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBulletCountHolder {
    TankAI AI { get; }
    byte[,] CellBulletCounts { get; }
    void ResetCellBulletCounts();
}

/// <summary>
/// Class used to determine where a bullet will go
/// </summary>
public class TankBulletTrajectory {

    #region fields;
    private float defaultHeight = 0f;
    private float envHeight = 0.3f;

    private IBulletCountHolder checker;
    private TankAI ai;
    private bool fail;
    private BulletInfo bullet;

    public bool HitAI { get; private set; }
    public bool HitPlayer { get; private set; }

    private Vector[] trajectory;
    public Vector[] Trajectory { get => trajectory; private set => trajectory = value; }

    private struct BulletInfo {
        public BulletInfo(TankBullet bullet, int bounces, Vector startPos, Vector direction) {
            Bullet = bullet;
            Bounces = bounces;
            StartPos = startPos;
            Direction = direction.Normalized;
        }

        public TankBullet Bullet;
        public int Bounces;
        public Vector StartPos;
        public Vector Direction;
    }
    #endregion

    public TankBulletTrajectory(IBulletCountHolder checker, int bounces, Vector position, Vector direction) {
        this.checker = checker;
        ai = checker.AI;
        this.bullet = new BulletInfo(null, bounces, position, direction);
    }
    public TankBulletTrajectory(IBulletCountHolder checker, TankBullet bullet) {
        this.checker = checker;
        ai = checker.AI;
        this.bullet = new BulletInfo(bullet, bullet.Bounces, new Vector(bullet.transform.position), new Vector(bullet.Velocity));
    }

    /// <summary>
    /// Approximates the future trajectory of a bullet
    /// </summary>
    public void CalculateTrajectory() {

        HitAI = false;
        HitPlayer = false;

        int count = bullet.Bounces + 2 < 2 ? 2 : bullet.Bounces + 2;
        Trajectory = new Vector[count];
        Trajectory[0] = bullet.StartPos;

        SetTrajectory();
    }

    private void SetTrajectory() {

        for (int i = 1; i < Trajectory.Length; i++) {

            float height = HitAI ? envHeight : defaultHeight;

            // if (HitAI) {
            //    Array.Resize(ref trajectory, i + 1);
            //    break;
            // }
            if (!CalculateNextHit(ref bullet.Direction, Trajectory[i - 1], i, height)) {
                Array.Resize(ref trajectory, i + 2);
                break;
            }
        }
    }

    /// <summary>
    /// Calculates the bullets next collision position and approximates the bounce of the bullet
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="currentPos"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool CalculateNextHit(ref Vector direction, Vector currentPos, int index, float rayHeight) {


        RaycastHit hit;
        if (!Physics.Raycast(RayCastStartPosition(currentPos, rayHeight), direction.Vector3, out hit, Mathf.Infinity, 1, QueryTriggerInteraction.Ignore)) {
            return false;
        }

        if (hit.collider.gameObject.transform.parent.GetComponent<TankPlayer>() != null) {
            HitPlayer = true;
        }
        if (hit.collider.gameObject.transform.parent == ai.transform) {
            HitAI = true;

            if (rayHeight != envHeight) {
                return CalculateNextHit(ref direction, currentPos, index, envHeight);
            }
        }

        Vector hitPos = new Vector(hit.point);

        Trajectory[index] = hitPos;

        direction = Vector.Reflect(direction, new Vector(hit.normal));

        IncrementValues(currentPos, hitPos, direction);

        return true;
    }
    private Vector3 RayCastStartPosition(Vector pos, float rayHeight) {
        pos.y = rayHeight;
        return pos.Vector3;
    }

    /// <summary>
    /// Increments the values in the TankAIBulletChecker.CellBulletCounts based on whether or not the bullet will pass over certain coordinates.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="endPos"></param>
    /// <param name="direction"></param>
    private void IncrementValues(Vector position, Vector endPos, Vector direction) {

        bool xTest = position.x > endPos.x;
        bool zTest = position.z > endPos.z;

        for (int i = 0; ; i++) {

            CheckHitItself(position, i);

            IntCoords coords = Vector.PositionToCoords(position);

            if (coords.x >= TankSettings.LevelWidth || coords.y >= TankSettings.LevelHeight
                || coords.x < 0 || coords.y < 0) {
                break;
            }

            if (position.x > endPos.x != xTest || position.z > endPos.z != zTest) {
                break;
            }

            checker.CellBulletCounts[coords.x, coords.y]++;

            // Replace Vector3.MoveTowards
            Vector newPosition = new Vector(Vector3.MoveTowards(position.Vector3, endPos.Vector3, 1));
            if (position == newPosition) {
                break;
            }

            position = newPosition;
        }
    }

    private void CheckHitItself(Vector position, int index) {

        if (index == 0) {
            return;
        }

        if (Vector.Distance(position, ai.transform.position) < TankAISettings.TankAIHitLimit) {
            HitAI = true;
        }
    }
}