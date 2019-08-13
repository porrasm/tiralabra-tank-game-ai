using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCell : MonoBehaviour {

    public enum CellWall {
        Top, Right, Both
    }

    public bool Top {
        get {
            return transform.GetChild(0).GetChild(0).gameObject.activeInHierarchy;
        }
    }
    public bool Right {
        get {
            return transform.GetChild(1).GetChild(0).gameObject.activeInHierarchy;
        }
    }

    private GameObject topWall, rightWall;

    private bool active;

    private void Start() {
        topWall = transform.GetChild(0).GetChild(0).gameObject;
        rightWall = transform.GetChild(1).GetChild(0).gameObject;
    }

    private GameObject TopWall() {
        return transform.GetChild(0).GetChild(0).gameObject;
    }
    private GameObject RightWall() {
        return transform.GetChild(1).GetChild(0).gameObject;
    }

    public void DisableWall(CellWall type) {
        if (type == CellWall.Top || type == CellWall.Both) {
            TopWall().SetActive(false);
        }
        if (type == CellWall.Right || type == CellWall.Both) {
            RightWall().SetActive(false);
        }

        if (BothDisabled()) {

            // Destroy(gameObject);
        }
    }

    private bool BothDisabled() {
        return !TopWall().activeInHierarchy && !RightWall().activeInHierarchy;
    }

    public void SetWalls(bool enabled) {

        active = enabled;

        transform.GetChild(0).gameObject.SetActive(enabled);
        transform.GetChild(1).gameObject.SetActive(enabled);
    }
}
