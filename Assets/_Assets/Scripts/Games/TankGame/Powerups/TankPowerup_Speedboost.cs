using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPowerup_Speedboost : TankPowerup {

    public override void Use() {
        base.Use();

        IEnumerator UseCoroutine() {

            float time = TankSettings.P_SpeedboostStartLength;

            float speed = TankSettings.TankSpeed;
            float rotSpeed = TankSettings.TankRotateSpeed;
            float amount = TankSettings.P_SpeedboostAmount - 1;

            TankController controller = GetComponent<TankController>();

            while (time > 0) {

                float factor = 1 - time / TankSettings.P_SpeedboostStartLength;

                float nSpeed = speed + speed * factor * amount;
                float nRotSpeed = rotSpeed + rotSpeed * factor * amount;

                controller.SetSpeeds(nSpeed, nRotSpeed);

                time -= Time.deltaTime;
                yield return null;
            }

            controller.SetSpeeds(speed + speed * amount, rotSpeed + rotSpeed * amount);

            yield return new WaitForSeconds(TankSettings.P_SpeedboostLength);

            time = TankSettings.P_SpeedboostEndLength;

            while (time > 0) {

                float factor = time / TankSettings.P_SpeedboostEndLength;

                float nSpeed = speed + speed * factor * amount;
                float nRotSpeed = rotSpeed + rotSpeed * factor * amount;

                controller.SetSpeeds(nSpeed, nRotSpeed);

                time -= Time.deltaTime;
                yield return null;
            }

            controller.SetSpeeds(speed, rotSpeed);
        }

        StartCoroutine(UseCoroutine());
    }

    public override void Remove() {

        StopAllCoroutines();

        GetComponent<TankController>().SetSpeeds(TankSettings.TankSpeed, TankSettings.TankRotateSpeed);

        base.Remove();
    }
}
