using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_AI : MonoBehaviour {

    #region fields
    TankControls controls;

    TankDFSPath pf;
	#endregion

    private void Start() {
        controls = GetComponent<TankControls>();

        pf = new TankDFSPath();
    }

    private void Update() {
        controls.ProcessControl(TankControls.Control.Rotation, 1);
    }
}
