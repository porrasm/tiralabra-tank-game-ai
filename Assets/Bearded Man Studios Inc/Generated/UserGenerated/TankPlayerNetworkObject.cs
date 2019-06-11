using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class TankPlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 10;

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

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			HealthInterpolation.current = HealthInterpolation.target;
			ScoreInterpolation.current = ScoreInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Health);
			UnityObjectMapper.Instance.MapBytes(data, _Score);

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
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Health);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Score);

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
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TankPlayerNetworkObject() : base() { Initialize(); }
		public TankPlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TankPlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
