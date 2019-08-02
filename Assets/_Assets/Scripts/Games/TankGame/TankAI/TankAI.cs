using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_AI : MonoBehaviour {

    #region fields
    TankControls controls;
	#endregion

    private void Start() {
        controls = GetComponent<TankControls>();
    }

    private void Update() {
        controls.ProcessControl(TankControls.Control.Rotation, 1);
    }
}
