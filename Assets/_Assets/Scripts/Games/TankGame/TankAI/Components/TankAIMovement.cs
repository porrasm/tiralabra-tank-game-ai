using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TankAIMovement : TankAIComponent {

    #region fields
    private Vector[] path;
    private TankControls controls;

    private bool stuck;
    private float stuckTime;
    private Vector stuckPos;
    #endregion

    public TankAIMovement(TankAI ai) : base(ai) {
        controls = ai.GetComponent<TankControls>();
    }

    public override void Update() {

    }

    #region Traversal
    public void TraversePath(Vector[] path) {

        TankPathVisualizer.DrawRoute(path);

        ai.StopCoroutine(TraversePathCoroutine());
        ai.StopCoroutine(RemoveStuck());

        this.path = path;
        ai.StartCoroutine(TraversePathCoroutine());
    }

    private IEnumerator TraversePathCoroutine() {

        int index = 0;

        while (index < path.Length) {

            ResetStuck();

            Vector targetPos = path[index];

            while (!InCoords(index)) {

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
    }

    private void StuckCheck() {

        if (stuck) {
            return;
        }

        if (Vector.Distance(stuckPos, Vector.FromVector3(ai.transform.position)) < AISettings.StuckTresholdDistance) {
            stuckTime += Time.deltaTime;

            if (stuckTime > AISettings.StuckTresholdTime) {
                stuck = true;
                ai.StartCoroutine(RemoveStuck());
            }
        } else {
            ResetStuck();
        }
    }
    private IEnumerator RemoveStuck() {

        float time = AISettings.StuckCooldown;

        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }

        ResetStuck();
    }
    private void ResetStuck() {
        stuck = false;
        stuckTime = 0;
        stuckPos = Vector.FromVector3(ai.transform.position);
    }

    private bool InCoords(int i) {

        Vector target = path[i];
        Vector current = Vector.FromVector3(ai.transform.position);

        return Vector.Distance(target, current) < AISettings.DistanceLimit;
    }

    private void MoveTowards(Vector position) {

        TurnToPosition(position);

        Vector3 currentRotation = ai.transform.eulerAngles;
        Vector3 targetRotation = Quaternion.LookRotation(Vector.ToVector3(position) - ai.transform.position, Vector3.up).eulerAngles;

        if (Vector3.Angle(currentRotation, targetRotation) < AISettings.MovementAngle) {
            controls.ProcessControl(TankControls.Control.Movement, 1);
        }
    }
    private void TurnToPosition(Vector position) {

        // Replace Quaternion.LookRotation
        Vector currentRotation = Vector.FromVector3(ai.transform.eulerAngles);
        Vector targetRotation = Vector.FromVector3(Quaternion.LookRotation(Vector.ToVector3(position) - ai.transform.position, Vector3.up).eulerAngles);

        float rotation = TurnDirection(currentRotation, targetRotation);
        controls.ProcessControl(TankControls.Control.Rotation, rotation);
    }
    private float TurnDirection(Vector current, Vector target) {


        // wtf does this do, found from internet
        int rotateDirection = (((target.y - current.y) + 360f) % 360f) > 180.0f ? -1 : 1;

        float angleDif = (((target.y - current.y) + 360f) % 360f);

        float min = Maths.Min(Maths.Abs(angleDif), Maths.Abs(angleDif - 360));

        if (min < AISettings.TurnAngleLimit) {

            float factor = min / AISettings.TurnAngleLimit;
            return factor * rotateDirection;
        } else {
            return rotateDirection;
        }
    }
    #endregion
}
