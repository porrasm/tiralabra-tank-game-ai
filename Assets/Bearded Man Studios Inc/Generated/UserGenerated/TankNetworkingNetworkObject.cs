using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0,0,0,0]")]
	public partial class TankNetworkingNetworkObject : NetworkObject
	{
		public const int IDENTITY = 11;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private int _Health;
		public event FieldEvent<int> HealthChanged;
		public Interpolated<int> HealthInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int Health
		{
			get { return _Health; }
			set
			{
				// Don't do anything if the value is the same
				if (_Health == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_Health = value;
				hasDirtyFields = true;
			}
		}

		public void SetHealthDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_Health(ulong timestep)
		{
			if (HealthChanged != null) HealthChanged(_Health, timestep);
			if (fieldAltered != null) fieldAltered("Health", _Health, timestep);
		}
		[ForgeGeneratedField]
		private int _Score;
		public event FieldEvent<int> ScoreChanged;
		public Interpolated<int> ScoreInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int Score
		{
			get { return _Score; }
			set
			{
				// Don't do anything if the value is the same
				if (_Score == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Score = value;
				hasDirtyFields = true;
			}
		}

		public void SetScoreDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Score(ulong timestep)
		{
			if (ScoreChanged != null) ScoreChanged(_Score, timestep);
			if (fieldAltered != null) fieldAltered("Score", _Score, timestep);
		}
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
				_dirtyFields[0] |= 0x4;
				_Movement = value;
				hasDirtyFields = true;
			}
		}

		public void SetMovementDirty()
		{
			_dirtyFields[0] |= 0x4;
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
				_dirtyFields[0] |= 0x8;
				_Rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetRotationDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_Rotation(ulong timestep)
		{
			if (RotationChanged != null) RotationChanged(_Rotation, timestep);
			if (fieldAltered != null) fieldAltered("Rotation", _Rotation, timestep);
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
				_dirtyFields[0] |= 0x10;
				_Fire = value;
				hasDirtyFields = true;
			}
		}

		public void SetFireDirty()
		{
			_dirtyFields[0] |= 0x10;
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
				_dirtyFields[0] |= 0x20;
				_Powerup = value;
				hasDirtyFields = true;
			}
		}

		public void SetPowerupDirty()
		{
			_dirtyFields[0] |= 0x20;
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
			HealthInterpolation.current = HealthInterpolation.target;
			ScoreInterpolation.current = ScoreInterpolation.target;
			MovementInterpolation.current = MovementInterpolation.target;
			RotationInterpolation.current = RotationInterpolation.target;
			FireInterpolation.current = FireInterpolation.target;
			PowerupInterpolation.current = PowerupInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Health);
			UnityObjectMapper.Instance.MapBytes(data, _Score);
			UnityObjectMapper.Instance.MapBytes(data, _Movement);
			UnityObjectMapper.Instance.MapBytes(data, _Rotation);
			UnityObjectMapper.Instance.MapBytes(data, _Fire);
			UnityObjectMapper.Instance.MapBytes(data, _Powerup);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_Health = UnityObjectMapper.Instance.Map<int>(payload);
			HealthInterpolation.current = _Health;
			HealthInterpolation.target = _Health;
			RunChange_Health(timestep);
			_Score = UnityObjectMapper.Instance.Map<int>(payload);
			ScoreInterpolation.current = _Score;
			ScoreInterpolation.target = _Score;
			RunChange_Score(timestep);
			_Movement = UnityObjectMapper.Instance.Map<Vector2>(payload);
			MovementInterpolation.current = _Movement;
			MovementInterpolation.target = _Movement;
			RunChange_Movement(timestep);
			_Rotation = UnityObjectMapper.Instance.Map<float>(payload);
			RotationInterpolation.current = _Rotation;
			RotationInterpolation.target = _Rotation;
			RunChange_Rotation(timestep);
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
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Health);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Score);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Movement);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Rotation);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Fire);
			if ((0x20 & _dirtyFields[0]) != 0)
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
				if (HealthInterpolation.Enabled)
				{
					HealthInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					HealthInterpolation.Timestep = timestep;
				}
				else
				{
					_Health = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_Health(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (ScoreInterpolation.Enabled)
				{
					ScoreInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					ScoreInterpolation.Timestep = timestep;
				}
				else
				{
					_Score = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_Score(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
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
			if ((0x8 & readDirtyFlags[0]) != 0)
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
			if ((0x10 & readDirtyFlags[0]) != 0)
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
			if ((0x20 & readDirtyFlags[0]) != 0)
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

			if (HealthInterpolation.Enabled && !HealthInterpolation.current.UnityNear(HealthInterpolation.target, 0.0015f))
			{
				_Health = (int)HealthInterpolation.Interpolate();
				//RunChange_Health(HealthInterpolation.Timestep);
			}
			if (ScoreInterpolation.Enabled && !ScoreInterpolation.current.UnityNear(ScoreInterpolation.target, 0.0015f))
			{
				_Score = (int)ScoreInterpolation.Interpolate();
				//RunChange_Score(ScoreInterpolation.Timestep);
			}
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

		public TankNetworkingNetworkObject() : base() { Initialize(); }
		public TankNetworkingNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TankNetworkingNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
