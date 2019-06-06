using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : ClientBehavior {
    
    public int ID { get; set; }
    public PlayerColor Color { get; set; }
    public string Name { get; set; }


    public void ChangeName(string newName) {

        //if (!networkObject.IsOwner)
        //    return;

        networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, newName);
    }
    public override void UpdateName(RpcArgs args) {
        // Since there is only 1 argument and it is a string we can safely
        // cast the first argument to a string knowing that it is going to
        // be the name for this player
        Name = args.GetNext<string>();
    }
}
