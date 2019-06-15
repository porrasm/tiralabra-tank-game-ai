using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGameUI : MonoBehaviour {

    private TankControls controls;

    private void Start() {
        StartCoroutine(GetControls());
    }
    private IEnumerator GetControls() {

        while (true) {
            yield return null;

            GameObject tank = TankNetworking.MyTank();

            if (tank == null) {  
                continue;
            }

            controls = tank.GetComponent<TankControls>();
            
            if (controls == null) {
                continue;
            }

            break;
        }
    }

    public void Fire() {
        controls.Fire();
    }

    public void Powerup() {
        controls.Powerup();
    }
}
