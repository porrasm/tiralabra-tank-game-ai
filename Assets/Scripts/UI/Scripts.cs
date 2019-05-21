using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour {

    public static GameObject GetGameObject() {
        return GameObject.FindGameObjectWithTag("Scripts");
    }

}
