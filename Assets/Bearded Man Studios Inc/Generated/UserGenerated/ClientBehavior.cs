using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"byte\", \"byte\", \"string\", \"byte\"][][\"string\"][\"uint\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"ID\", \"Color\", \"Name\", \"Ready\"][][\"name\"][\"PlayerID\"]]")]
	public abstract partial class ClientBehavior : NetworkBehavior
	{
		public const byte RPC_UPDATE_CLIENT_RPC = 0 + 5;
		public const byte RPC_TOGGLE_COLOR_RPC = 1 + 5;
		public const byte RPC_CHANGE_NAME_RPC = 2 + 5;
		public const byte RPC_SET_NETWORK_PLAYER_I_D_R_P_C = 3 + 5;
		
		public ClientNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (ClientNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("UpdateClientRpc", UpdateClientRpc, typeof(byte), typeof(byte), typeof(string), typeof(byte));
			networkObject.RegisterRpc("ToggleColorRpc", ToggleColorRpc);
			networkObject.RegisterRpc("ChangeNameRpc", ChangeNameRpc, typeof(string));
			networkObject.RegisterRpc("SetNetworkPlayerIDRPC", SetNetworkPlayerIDRPC, typeof(uint));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new ClientNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new ClientNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// byte ID
		/// byte Color
		/// string Name
		/// byte Ready
		/// </summary>
		public abstract void UpdateClientRpc(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void ToggleColorRpc(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void ChangeNameRpc(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SetNetworkPlayerIDRPC(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}