using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {

    private void Start() {

        if (Scripts.GetScriptComponent<Games>() != null) {
            return;
        }

        NetworkManager.Instance.InstantiateGames();
    }
}
