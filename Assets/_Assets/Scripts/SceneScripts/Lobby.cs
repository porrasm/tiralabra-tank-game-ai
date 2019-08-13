using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class Lobby : MonoBehaviour {

    private void Start() {

        if (Scripts.GetScriptComponent<GameManager>() != null) {
            return;
        }

        NetworkManager.Instance.InstantiateGames();
    }
}
