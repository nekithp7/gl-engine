using static System.Math;

using OpenTK;

using static engine.Tools.Maths;

namespace engine.Entities
{
	public class Camera
	{
		private Vector3 position;
		private Matrix4 viewMatrix;
		private float pitch = 10.0f;
		private float yaw = 0.0f;
		private float roll = 0.0f;
		private float distanceFromPlayer;

		public Camera(ref Vector3 playerPosition, float distanceFromPlayer)
		{
			this.distanceFromPlayer = distanceFromPlayer;
			position = new Vector3()
			{
				X = playerPosition.X,
				Y = playerPosition.X + GetVerticalOffset(),
				Z = playerPosition.Z - distanceFromPlayer
			};

			viewMatrix = SetDirection(playerPosition);
		}

		public Vector3 Position => position;
		public Matrix4 ViewMatrix => viewMatrix;
		public float Pitch => pitch;
		public float Yaw => yaw;
		public float Roll => roll;

		public void Move(ref Vector3 playerPosition, Vector3 offset)
		{
			Vector3.Add(ref position, ref offset, out position);
			viewMatrix = SetDirection(playerPosition);
		}

		public void Rotate(ref Vector3 playerPosition, ref Vector3 playerRotation, int xDelta, int yDelta)
		{
			yaw += xDelta * 0.1f;
			pitch += yDelta * 0.1f;

			float horizontal = GetHorizontalOffset();

			var offset = new Vector3()
			{
				X = -horizontal * (float)Sin(ToRad(playerRotation.Y)),
				Y = GetVerticalOffset(),
				Z = -horizontal * (float)Cos(ToRad(playerRotation.Y))
			};

			Vector3.Add(ref playerPosition, ref offset, out position);

			viewMatrix = SetDirection(playerPosition);
		}

		private float GetVerticalOffset() => distanceFromPlayer * (float)Sin(ToRad(pitch));

		private float GetHorizontalOffset() => distanceFromPlayer * (float)Cos(ToRad(pitch));

		private Matrix4 SetDirection(Vector3 playerPosition) => Matrix4.LookAt(position, playerPosition, Vector3.UnitY);
	}
}
