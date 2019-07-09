using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using UnityEngine;

public class Scripts : MonoBehaviour {

    private const float CoroutineTimeout = 10;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Players"));
        Server.Initialize();
    }

    public static GameObject GetGameObject() {
        return GameObject.FindGameObjectWithTag("Scripts");
    }

    public static T GetScriptComponent<T>() {

        GameObject obj = GetGameObject();

        if (obj == null) {
            Debug.LogError("Scripts object was null");
            return default;
        }

        T component = obj.GetComponent<T>();

        if (component != null) {
            return component;
        }

        foreach (Transform child in obj.transform) {
            component = child.GetComponent<T>();
            if (component != null) {
                return component;
            }
        }

        return default;
    }

    public static void RunCoroutine(IEnumerator coroutine) {
        Scripts scr = GetScriptComponent<Scripts>();
        if (scr != null) {
            scr.InstanceRunCoroutine(coroutine);
        }
    }

    private void InstanceRunCoroutine(IEnumerator coroutine) {
        print("Starting coroutine: " + coroutine);
        StartCoroutine(coroutine);
    }

    public static void Print(object obj) {
        print(obj);
    }
}
