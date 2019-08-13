using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Class used to determine where a bullet will go
/// </summary>
public class TankBulletTrajectory {

    #region fields;
    TankAIBulletChecker checker;
    private TankAI ai;
    private TankBullet bullet;
    private bool fail;

    public bool HitAI { get; private set; }

    private Vector[] trajectory;
    public Vector[] Trajectory { get => trajectory; set => trajectory = value; }
    #endregion

    public TankBulletTrajectory(TankAIBulletChecker checker, TankBullet bullet) {
        this.checker = checker;
        ai = checker.AI;
        this.bullet = bullet;
    }

    /// <summary>
    /// Approximates the future trajectory of a bullet
    /// </summary>
    public void CalculateTrajectory() {

        HitAI = false;

        Trajectory = new Vector[bullet.Bounces + 2];
        Trajectory[0] = new Vector(bullet.transform.position);

        SetTrajectory();
    }

    private void SetTrajectory() {

        Vector direction = new Vector(bullet.Velocity.normalized);

        for (int i = 1; i < Trajectory.Length; i++) {

            
            //if (HitAI) {
            //    Array.Resize(ref trajectory, i + 1);
            //    break;
            //}

            if (!CalculateNextHit(ref direction, Trajectory[i - 1], i)) {
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
    private bool CalculateNextHit(ref Vector direction, Vector currentPos, int index) {

        RaycastHit hit;
        if (!Physics.Raycast(currentPos.Vector3, direction.Vector3, out hit, Mathf.Infinity, 9, QueryTriggerInteraction.Ignore)) {
            Debug.Log("Trajectory raycast did not hit anything.");
            return false;
        }

        if (hit.collider.gameObject.transform.parent == ai.transform) {
            HitAI = true;

            Vector3 newPos = hit.point + 0.2f * direction.Normalized.Vector3;

            // Continue next raycast from inside the collider
            if (!Physics.Raycast(ai.transform.position, direction.Vector3, out hit, Mathf.Infinity, 9, QueryTriggerInteraction.Ignore)) {
                Debug.Log("Trajectory raycast did not hit anything after hitting player.");
                return false;
            }
        }

        Vector hitPos = new Vector(hit.point);

        Trajectory[index] = hitPos;

        direction = Vector.Reflect(direction, new Vector(hit.normal));

        IncrementValues(currentPos, hitPos, direction);

        return true;
    }

    /// <summary>
    /// Increments the values in the TankAIBulletChecker.CellBulletCounts based on whether or not the bullet will pass over certain coordinates.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="endPos"></param>
    /// <param name="direction"></param>
    private void IncrementValues(Vector position, Vector endPos, Vector direction) {

        bool xTest = position.x > endPos.x;
        bool yTest = position.y > endPos.y;

        for (int i = 0; ; i++) {
            position += direction.Normalized;

            IntCoords coords = Vector.PositionToCoords(position);

            if (coords.x >= TankSettings.LevelWidth || coords.y >= TankSettings.LevelHeight
                || coords.x < 0 || coords.y < 0) {
                break;
            }

            if (position.x > endPos.x != xTest || position.y > endPos.y != yTest) {
                break;
            }

            checker.CellBulletCounts[coords.x, coords.y]++;
        }
    }
}

