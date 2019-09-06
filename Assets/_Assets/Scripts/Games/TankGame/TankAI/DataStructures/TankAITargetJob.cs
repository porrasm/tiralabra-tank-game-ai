using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAITargetJob : TankAIBehaviourJob {


    #region fields
    private static float preferredDistance = 2.5f;
    private static float maxInactiveTime = 2f;
    private static float calibrateTime = 1f;

    #endregion

    public TankAITargetJob(TankAI ai, TankNetworking target) : base(ai, target) {
        JobType = Type.TargetPlayer;
    }

    public override IEnumerator Execute(int executeIndex) {

        this.executeIndex = executeIndex;

        while (true) {
            ai.Movement.TraversePath(TargetingPath());

            yield return new WaitForSeconds(RNG.Float * maxInactiveTime);

            float time = 0;

            while (ai.Movement.Moving) {

                time -= Time.deltaTime;

                if (time < 0) {
                    break;
                }

                if (!CorrectIndex || target.Health <= 0) {
                    ai.Behaviour.Executing = false;
                    yield break;
                }

                yield return null;
            }
        }
    }

    private Vector[] TargetingPath() {

        Vector[] path = ai.AStar.FindPath(
            Vector.PositionToCoords(ai.transform.position),
            Vector.PositionToCoords(target.transform.position),
            FoundCondition);

        if (path.Length > 2) {
            return path;
        } else return ai.AStar.FindPath(Vector.PositionToCoords(ai.transform.position),
              Vector.PositionToCoords(target.transform.position));
    }

    private bool FoundCondition(IntCoords current) {

        Vector targetPos = target.transform.position;
        Vector currentPos = current;
        Vector aiPos = ai.transform.position;

        if (AreClose(aiPos, targetPos, preferredDistance)) {
            return !AreClose(currentPos, targetPos, preferredDistance);
        } else {
            return AreClose(currentPos, targetPos, preferredDistance);
        }
    }

    private bool AreClose(Vector a, Vector b, float preferredDistance) {
        return Vector.Distance(a, b) < preferredDistance;
    }

}
