using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControls : MonoBehaviour {

    #region fields
    private TankNetworking net;

    private Matrix4x4 calibrationMatrix;
    private Vector3 calibration;

    private bool testControls;

    public enum Control { Movement, Rotation, HeadRotation, Fire, Powerup }
    #endregion

    private void Start() {
        testControls = false;

        if (Player.MyPlayer().Local) {
            testControls = true;
        }

        net = GetComponent<TankNetworking>();

        calibrationMatrix = Matrix4x4.identity;
    }

    private void Update() {

        if (net.Owner == null || net.Owner.AI) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            net.ChangeState(TankPlayer.PlayerState.Enabled);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            testControls = !testControls;
        }

        if (net.State != TankPlayer.PlayerState.Enabled) {
            return;
        }

        PCTestControls();
    }
    private void PCTestControls() {

        if (!testControls) {
            return;
        }

        // PC CONTROLS FOR TESTING

        Vector2 movement = Vector2.zero;
        float rotation = 0;

        if (Input.GetKey(KeyCode.W)) {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement.x += 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement.x -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            net.Fire++;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            net.Powerup++;
        }

        net.Movement = movement;
        net.Rotation = rotation;
    }
    private void GyroControls() {

        if (testControls) {
            return;
        }

        Vector3 movement = FixOrientation(GetOrientation());

        net.Movement = new Vector2(movement.x, movement.z);
    }

    private Vector3 FixOrientation(Vector3 orientation) {

        orientation -= calibration;
        // orientation = Quaternion.Euler(Vector3.right) * orientation;

        return LimitVector(orientation);
    }
    private Vector3 LimitVector(Vector3 vector) {

        if (vector.x > 1) {
            vector.x = 1;
        } else if (vector.x < -1) {
            vector.x = -1;
        }

        if (vector.y > 1) {
            vector.y = 1;
        } else if (vector.y < -1) {
            vector.y = -1;
        }

        if (vector.z > 1) {
            vector.z = 1;
        } else if (vector.z < -1) {
            vector.z = -1;
        }

        return vector;
    }

    private Vector3 GetOrientation() {

        return Input.acceleration;

        Vector3 orientation = Vector3.zero;

        if (SystemInfo.supportsGyroscope) {
            orientation = GyroToUnity(Input.gyro.attitude).eulerAngles;
        } else if (SystemInfo.supportsAccelerometer) {
            orientation = Input.acceleration;
        } else {
            Debug.LogError("Neither gyro or accelerometer available");
        }

        return orientation;
    }
    private Quaternion GyroToUnity(Quaternion q) {
        print("GyroToUnity: " + q);
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void CalibrateGyro() {
        StartCoroutine(CalibrateGyroCoroutine());
    }
    private IEnumerator CalibrateGyroCoroutine() {

        double x = 0;
        double y = 0;
        double z = 0;

        for (int i = 0; i < 120; i++) {

            Vector3 orientation = GetOrientation();
            x += orientation.x;
            y += orientation.y;
            z += orientation.z;

            yield return null;
        }

        calibration = new Vector3((float)x / 120, (float)y / 120, (float)z / 120);

        print("Gyro rest: (" + calibration.x + ", " + calibration.y + ", " + calibration.z + ")");
    }

    #region Control Methods
    public void Fire() {
        net.Fire++;
    }

    public void Powerup() {
        net.Powerup++;
    }

    public void ProcessControl(Control control) {
        ProcessControl(control, 0);
    }
    public void ProcessControl(Control control, float value) {

        if (control == Control.Movement) {
            Vector2 movement = net.Movement;
            movement.y = value;
            net.Movement = movement;
        } else if (control == Control.Rotation) {
            Vector2 movement = net.Movement;
            movement.x = value;
            net.Movement = movement;
        } else if (control == Control.HeadRotation) {
            net.Rotation = value;
        } else if (control == Control.Fire) {
            Fire();
        } else if (control == Control.Powerup) {
            Powerup();
        }
    }
    #endregion
}
