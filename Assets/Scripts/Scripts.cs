using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : MonoBehaviour {

    private void Start() {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Players"));
        Rpc.MainThreadRunner = MainThreadManager.Instance;
    }

    public static GameObject GetGameObject() {
        return GameObject.FindGameObjectWithTag("Scripts");
    }
    public static T GetScriptComponent<T>() {
        return GetGameObject().GetComponent<T>();
    }

    public static void Print(object obj) {
        print(obj);

    }
}
