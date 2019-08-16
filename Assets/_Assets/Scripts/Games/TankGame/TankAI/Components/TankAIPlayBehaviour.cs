using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIPlayBehaviour : TankAIComponent {

    #region fields
    private const int evadePriority = 3;
    private const int targetPriority = 2;

    private LinkedPriorityList<TankAIBehaviourJob> jobs;

    private TankNetworking[] players;

    public int ExecuteIndex { get; private set; }
    public bool Executing { get; internal set; }
    #endregion

    public TankAIPlayBehaviour(TankAI ai) : base(ai) {
        jobs = new LinkedPriorityList<TankAIBehaviourJob>();
        players = TankNetworking.Tanks();
    }

    public override void Update() {

        if (jobs.Count == 0) {
            ResetJobs();
        }

        if (!ai.Behaviour.Executing) {
            ExecuteIndex++;
            Debug.Log("Execute count: " + jobs.Count);
            Debug.Log("First type: " + jobs.First.JobType);
            Executing = true;

            ai.StartCoroutine(jobs.First.Execute(ExecuteIndex));
        }
    }

    private void ResetJobs() {

        jobs.Clear();

        AddTargetJobs();
        AddEvadeJobs();
    }
    private void AddTargetJobs() {

        TankNetworking owner = ai.GetComponent<TankNetworking>();

        foreach (TankNetworking p in players) {

            if (owner.Equals(p)) {
                continue;
            }

            double priority = owner.Health > p.Health ? 1 - (p.Health / owner.Health) : 0;
            priority += targetPriority;

            jobs.Add(new TankAITargetJob(ai, p), priority);
        }
    }
    private void AddEvadeJobs() {

    }
}
