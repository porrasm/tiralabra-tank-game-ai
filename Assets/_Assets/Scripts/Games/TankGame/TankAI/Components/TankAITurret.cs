using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used to move a tank through a path.
/// </summary>
public class TankAITurret : TankAIComponent {

    #region fields
    private Vector[] path;
    private TankAIShooting shooting;
    private TankAIMovement movement;
    private TankControls controls;

    private bool stuck;
    private float stuckTime;
    private Vector stuckPos;

    private int pathIndex;

    private bool urgent;
    public bool Moving { get; set; }

    private bool turretMode;
    private bool cancel;
    #endregion

    public TankAITurret(TankAI ai) : base(ai) {
        shooting = ai.Shooting;
        movement = ai.Movement;
        controls = ai.GetComponent<TankControls>();
    }

    public override void Update() {

        if (shooting.Shot == 2 && !turretMode) {
            Debug.Log("Found shot");
            turretMode = true;
           ai.StartCoroutine(StartShotCoroutine());
        }

    }
    public void Cancel() {
        cancel = true;
    }

   private IEnumerator StartShotCoroutine() {

        float endTime = 1 + RNG.Float * 1;
        float cooldown = 3;

        int dir = RNG.Float > 0.5f ? 1 : -1;

        Debug.Log("SHoot coroutine");

        movement.Stop(true);

        cancel = false;

        while (endTime > 0 && !cancel) {

            endTime -= Time.deltaTime;

            if (shooting.Shot == 2) {
                controls.Fire();
            } else {
                controls.ProcessControl(TankControls.Control.Rotation, dir);
            }

            yield return null;
        }

        controls.ResetControls();

        while (cooldown > 0) {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        turretMode = false;
    }
}
