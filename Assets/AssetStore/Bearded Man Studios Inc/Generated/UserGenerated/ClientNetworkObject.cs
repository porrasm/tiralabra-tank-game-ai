using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class ClientNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private byte _ID;
		public event FieldEvent<byte> IDChanged;
		public Interpolated<byte> IDInterpolation = new Interpolated<byte>() { LerpT = 0f, Enabled = false };
		public byte ID
		{
			get { return _ID; }
			set
			{
				// Don't do anything if the value is the same
				if (_ID == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_ID = value;
				hasDirtyFields = true;
			}
		}

		public void SetIDDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_ID(ulong timestep)
		{
			if (IDChanged != null) IDChanged(_ID, timestep);
			if (fieldAltered != null) fieldAltered("ID", _ID, timestep);
		}
		[ForgeGeneratedField]
		private byte _Color;
		public event FieldEvent<byte> ColorChanged;
		public Interpolated<byte> ColorInterpolation = new Interpolated<byte>() { LerpT = 0f, Enabled = false };
		public byte Color
		{
			get { return _Color; }
			set
			{
				// Don't do anything if the value is the same
				if (_Color == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Color = value;
				hasDirtyFields = true;
			}
		}

		public void SetColorDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Color(ulong timestep)
		{
			if (ColorChanged != null) ColorChanged(_Color, timestep);
			if (fieldAltered != null) fieldAltered("Color", _Color, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			IDInterpolation.current = IDInterpolation.target;
			ColorInterpolation.current = ColorInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _ID);
			UnityObjectMapper.Instance.MapBytes(data, _Color);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_ID = UnityObjectMapper.Instance.Map<byte>(payload);
			IDInterpolation.current = _ID;
			IDInterpolation.target = _ID;
			RunChange_ID(timestep);
			_Color = UnityObjectMapper.Instance.Map<byte>(payload);
			ColorInterpolation.current = _Color;
			ColorInterpolation.target = _Color;
			RunChange_Color(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _ID);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Color);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (IDInterpolation.Enabled)
				{
					IDInterpolation.target = UnityObjectMapper.Instance.Map<byte>(data);
					IDInterpolation.Timestep = timestep;
				}
				else
				{
					_ID = UnityObjectMapper.Instance.Map<byte>(data);
					RunChange_ID(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (ColorInterpolation.Enabled)
				{
					ColorInterpolation.target = UnityObjectMapper.Instance.Map<byte>(data);
					ColorInterpolation.Timestep = timestep;
				}
				else
				{
					_Color = UnityObjectMapper.Instance.Map<byte>(data);
					RunChange_Color(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (IDInterpolation.Enabled && !IDInterpolation.current.UnityNear(IDInterpolation.target, 0.0015f))
			{
				_ID = (byte)IDInterpolation.Interpolate();
				//RunChange_ID(IDInterpolation.Timestep);
			}
			if (ColorInterpolation.Enabled && !ColorInterpolation.current.UnityNear(ColorInterpolation.target, 0.0015f))
			{
				_Color = (byte)ColorInterpolation.Interpolate();
				//RunChange_Color(ColorInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public ClientNetworkObject() : base() { Initialize(); }
		public ClientNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public ClientNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
