using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
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

        GameObject obj = GetGameObject();

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

    public static void Print(object obj) {
        print(obj);
    }
}
