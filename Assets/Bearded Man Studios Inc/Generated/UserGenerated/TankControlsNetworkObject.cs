using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0,0,0]")]
	public partial class TankControlsNetworkObject : NetworkObject
	{
		public const int IDENTITY = 10;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector2 _Movement;
		public event FieldEvent<Vector2> MovementChanged;
		public InterpolateVector2 MovementInterpolation = new InterpolateVector2() { LerpT = 0f, Enabled = false };
		public Vector2 Movement
		{
			get { return _Movement; }
			set
			{
				// Don't do anything if the value is the same
				if (_Movement == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_Movement = value;
				hasDirtyFields = true;
			}
		}

		public void SetMovementDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_Movement(ulong timestep)
		{
			if (MovementChanged != null) MovementChanged(_Movement, timestep);
			if (fieldAltered != null) fieldAltered("Movement", _Movement, timestep);
		}
		[ForgeGeneratedField]
		private float _Rotation;
		public event FieldEvent<float> RotationChanged;
		public InterpolateFloat RotationInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float Rotation
		{
			get { return _Rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_Rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Rotation(ulong timestep)
		{
			if (RotationChanged != null) RotationChanged(_Rotation, timestep);
			if (fieldAltered != null) fieldAltered("Rotation", _Rotation, timestep);
		}
		[ForgeGeneratedField]
		private float _HeadRotation;
		public event FieldEvent<float> HeadRotationChanged;
		public InterpolateFloat HeadRotationInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float HeadRotation
		{
			get { return _HeadRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_HeadRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_HeadRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetHeadRotationDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_HeadRotation(ulong timestep)
		{
			if (HeadRotationChanged != null) HeadRotationChanged(_HeadRotation, timestep);
			if (fieldAltered != null) fieldAltered("HeadRotation", _HeadRotation, timestep);
		}
		[ForgeGeneratedField]
		private int _Fire;
		public event FieldEvent<int> FireChanged;
		public Interpolated<int> FireInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int Fire
		{
			get { return _Fire; }
			set
			{
				// Don't do anything if the value is the same
				if (_Fire == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_Fire = value;
				hasDirtyFields = true;
			}
		}

		public void SetFireDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_Fire(ulong timestep)
		{
			if (FireChanged != null) FireChanged(_Fire, timestep);
			if (fieldAltered != null) fieldAltered("Fire", _Fire, timestep);
		}
		[ForgeGeneratedField]
		private int _Powerup;
		public event FieldEvent<int> PowerupChanged;
		public Interpolated<int> PowerupInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int Powerup
		{
			get { return _Powerup; }
			set
			{
				// Don't do anything if the value is the same
				if (_Powerup == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_Powerup = value;
				hasDirtyFields = true;
			}
		}

		public void SetPowerupDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_Powerup(ulong timestep)
		{
			if (PowerupChanged != null) PowerupChanged(_Powerup, timestep);
			if (fieldAltered != null) fieldAltered("Powerup", _Powerup, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			MovementInterpolation.current = MovementInterpolation.target;
			RotationInterpolation.current = RotationInterpolation.target;
			HeadRotationInterpolation.current = HeadRotationInterpolation.target;
			FireInterpolation.current = FireInterpolation.target;
			PowerupInterpolation.current = PowerupInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Movement);
			UnityObjectMapper.Instance.MapBytes(data, _Rotation);
			UnityObjectMapper.Instance.MapBytes(data, _HeadRotation);
			UnityObjectMapper.Instance.MapBytes(data, _Fire);
			UnityObjectMapper.Instance.MapBytes(data, _Powerup);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_Movement = UnityObjectMapper.Instance.Map<Vector2>(payload);
			MovementInterpolation.current = _Movement;
			MovementInterpolation.target = _Movement;
			RunChange_Movement(timestep);
			_Rotation = UnityObjectMapper.Instance.Map<float>(payload);
			RotationInterpolation.current = _Rotation;
			RotationInterpolation.target = _Rotation;
			RunChange_Rotation(timestep);
			_HeadRotation = UnityObjectMapper.Instance.Map<float>(payload);
			HeadRotationInterpolation.current = _HeadRotation;
			HeadRotationInterpolation.target = _HeadRotation;
			RunChange_HeadRotation(timestep);
			_Fire = UnityObjectMapper.Instance.Map<int>(payload);
			FireInterpolation.current = _Fire;
			FireInterpolation.target = _Fire;
			RunChange_Fire(timestep);
			_Powerup = UnityObjectMapper.Instance.Map<int>(payload);
			PowerupInterpolation.current = _Powerup;
			PowerupInterpolation.target = _Powerup;
			RunChange_Powerup(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Movement);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _HeadRotation);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Fire);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Powerup);

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
				if (MovementInterpolation.Enabled)
				{
					MovementInterpolation.target = UnityObjectMapper.Instance.Map<Vector2>(data);
					MovementInterpolation.Timestep = timestep;
				}
				else
				{
					_Movement = UnityObjectMapper.Instance.Map<Vector2>(data);
					RunChange_Movement(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (RotationInterpolation.Enabled)
				{
					RotationInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					RotationInterpolation.Timestep = timestep;
				}
				else
				{
					_Rotation = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_Rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (HeadRotationInterpolation.Enabled)
				{
					HeadRotationInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					HeadRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_HeadRotation = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_HeadRotation(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (FireInterpolation.Enabled)
				{
					FireInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					FireInterpolation.Timestep = timestep;
				}
				else
				{
					_Fire = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_Fire(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (PowerupInterpolation.Enabled)
				{
					PowerupInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					PowerupInterpolation.Timestep = timestep;
				}
				else
				{
					_Powerup = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_Powerup(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (MovementInterpolation.Enabled && !MovementInterpolation.current.UnityNear(MovementInterpolation.target, 0.0015f))
			{
				_Movement = (Vector2)MovementInterpolation.Interpolate();
				//RunChange_Movement(MovementInterpolation.Timestep);
			}
			if (RotationInterpolation.Enabled && !RotationInterpolation.current.UnityNear(RotationInterpolation.target, 0.0015f))
			{
				_Rotation = (float)RotationInterpolation.Interpolate();
				//RunChange_Rotation(RotationInterpolation.Timestep);
			}
			if (HeadRotationInterpolation.Enabled && !HeadRotationInterpolation.current.UnityNear(HeadRotationInterpolation.target, 0.0015f))
			{
				_HeadRotation = (float)HeadRotationInterpolation.Interpolate();
				//RunChange_HeadRotation(HeadRotationInterpolation.Timestep);
			}
			if (FireInterpolation.Enabled && !FireInterpolation.current.UnityNear(FireInterpolation.target, 0.0015f))
			{
				_Fire = (int)FireInterpolation.Interpolate();
				//RunChange_Fire(FireInterpolation.Timestep);
			}
			if (PowerupInterpolation.Enabled && !PowerupInterpolation.current.UnityNear(PowerupInterpolation.target, 0.0015f))
			{
				_Powerup = (int)PowerupInterpolation.Interpolate();
				//RunChange_Powerup(PowerupInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TankControlsNetworkObject() : base() { Initialize(); }
		public TankControlsNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TankControlsNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
