using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIBehaviourJob {

    #region fields
    protected TankAI ai;
    protected TankNetworking target;
    protected int executeIndex;

    public Type JobType { get; set; }
    public enum Type { Inactive, TargetPlayer, EvadePlayer, EvadeBullets }

    public bool Active { get; set; }
    public bool Executing { get; set; }
    #endregion

    public TankAIBehaviourJob(TankAI ai, TankNetworking target) {
        this.ai = ai;
        this.JobType = Type.Inactive;
        this.target = target;
    }

    public virtual IEnumerator Execute(int executeIndex) {
        yield return null;
    }
    protected virtual bool CorrectIndex { get {
            return ai.Behaviour.ExecuteIndex == executeIndex;
        }
    }

    

    public override bool Equals(object obj) {

        if (obj.GetType() != GetType()) {
            return false;
        }

        TankAIBehaviourJob other = (TankAIBehaviourJob)obj;

        if (JobType != other.JobType) {
            return false;
        }
        if (target == null) {
            return target == other.target;
        }

        return target.Equals(other.target);
    }
}
