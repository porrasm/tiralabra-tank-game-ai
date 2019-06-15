using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGameClient : SceneScript {

    private void Start() {
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();

        if (!Server.Networker.IsServer) {
            NetworkManager.Instance.InstantiateTankNetworking();
        }
    }
}
