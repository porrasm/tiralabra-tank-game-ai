using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour {

    private void Start() {

        if (Scripts.GetScriptComponent<GameManager>() != null) {
            return;
        }

        NetworkManager.Instance.InstantiateGames();
    }
}
