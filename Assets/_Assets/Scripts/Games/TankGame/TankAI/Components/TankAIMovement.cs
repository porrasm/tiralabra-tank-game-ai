﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used to move a tank through a path.
/// </summary>
public class TankAIMovement : TankAIComponent {

    #region fields
    private Vector[] path;
    private TankControls controls;

    private bool stuck;
    private float stuckPosTime;
    private Vector stuckPos;

    private float stuckRotTime;
    private float stuckAngle;

    private int pathIndex;

    private bool urgent;
    public bool Moving { get; set; }

    private Vector targetPos;
    #endregion

    public TankAIMovement(TankAI ai) : base(ai) {
        controls = ai.GetComponent<TankControls>();
    }

    public override void Update() {
    }

    #region Traversal

    /// <summary>
    /// Starts to move the tank towards the end of the path
    /// </summary>
    /// <param name="path"></param>
    public void TraversePath(Vector[] path) {
        TraversePath(path, false);
    }
    public void TraversePath(Vector[] path, bool urgent) {

        Moving = true;

        this.urgent = urgent;

        pathIndex++;

        TankPathVisualizer.DrawRoute(path);

        ai.StopCoroutine(TraversePathCoroutine(pathIndex - 1));
        ai.StopCoroutine(RemoveStuck());

        this.path = path;
        ai.StartCoroutine(TraversePathCoroutine(pathIndex));
    }

    private IEnumerator TraversePathCoroutine(int pi) {

        int index = 0;

        while (index < path.Length) {

            ResetStuck();

            targetPos = path[index];

            while (!InCoords(index)) {

                if (pi != pathIndex) {
                    yield break;
                }

                StuckCheck();

                if (stuck) {
                    controls.ProcessControl(TankControls.Control.Movement, -1);
                } else {
                    MoveTowards(targetPos);
                }

                yield return null;
            }

            index++;
        }

        Moving = false;
    }

    /// <summary>
    /// Checks if the tank is stuck against a wall
    /// </summary>
    private void StuckCheck() {

        if (stuck) {
            return;
        }

        bool pos = Vector.Distance(stuckPos, ai.transform.position) < TankAISettings.StuckTresholdDistance;
        bool rot = Maths.Abs(stuckAngle - ai.transform.eulerAngles.y) < TankAISettings.StuckTreshholdAngle && TurnDirection(ai.transform.position, targetPos) > 0.8f;

        if (pos || rot) {
            stuckPosTime += Time.deltaTime;

            if (stuckPosTime > TankAISettings.StuckTresholdTime) {
                stuck = true;
                ai.StartCoroutine(RemoveStuck());
            }

        } else {
            ResetStuck();
        }
    }
    private IEnumerator RemoveStuck() {

        float time = TankAISettings.StuckCooldown;

        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }

        ResetStuck();
    }
    private void ResetStuck() {
        stuck = false;
        stuckPosTime = 0;
        stuckPos = ai.transform.position;
        stuckAngle = ai.transform.eulerAngles.y;
    }

    /// <summary>
    /// Checks if the tank has reached certain coordinates
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private bool InCoords(int i) {

        if (i >= path.Length) {
            return false;
        }

        Vector target = path[i];
        Vector current = ai.transform.position;

        return Vector.Distance(target, current) < TankAISettings.DistanceLimit;
    }

    /// <summary>
    /// Moves the tank towards a position
    /// </summary>
    /// <param name="position"></param>
    private void MoveTowards(Vector position) {

        TurnToPosition(position);

        Vector3 currentRotation = ai.transform.eulerAngles;
        Vector3 targetRotation = Quaternion.LookRotation(position.Vector3 - ai.transform.position, Vector3.up).eulerAngles;

        if (Vector3.Angle(currentRotation, targetRotation) < TankAISettings.MovementAngle) {
            controls.ProcessControl(TankControls.Control.Movement, 1);
        }
    }

    /// <summary>
    /// Turns the tank towards a position
    /// </summary>
    /// <param name="position"></param>
    private void TurnToPosition(Vector position) {

        // Replace Quaternion.LookRotation
        Vector currentRotation = ai.transform.eulerAngles;
        Vector targetRotation = Quaternion.LookRotation(position.Vector3 - ai.transform.position, Vector3.up).eulerAngles;

        float rotation = TurnDirection(currentRotation, targetRotation);
        controls.ProcessControl(TankControls.Control.Rotation, rotation);
    }

    /// <summary>
    /// Returns left (negative) or right (positive) based on how big an angle the tank will have to turn towards a certain position.
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private float TurnDirection(Vector current, Vector target) {

        int rotateDirection = (((target.y - current.y) + 360f) % 360f) > 180.0f ? -1 : 1;

        float angleDif = ((target.y - current.y) + 360f) % 360f;

        float min = Maths.Min(Maths.Abs(angleDif), Maths.Abs(angleDif - 360));

        if (min < TankAISettings.TurnAngleLimit) {

            float factor = min / TankAISettings.TurnAngleLimit;
            return factor * rotateDirection;
        } else {
            return rotateDirection;
        }
    }
    #endregion
}
