using OpenTK;
using OpenTK.Input;

namespace engine.Entities
{
	public class Camera
	{
		private Vector3 position = new Vector3(0.0f, 1.0f, 0.0f);
		private float moveStep = 1.0f;
		private float pitch = 0.0f;
		private float yaw = 0.0f;
		private float roll = 0.0f;

		public Camera() { }

		public Camera(float pitch, float yaw, float roll, Vector3 position)
		{
			this.pitch = pitch;
			this.yaw = yaw;
			this.roll = roll;
			this.position = position;
		}

		public Vector3 Position { get => position; }
		public float Pitch { get => pitch; }
		public float Yaw { get => yaw; }
		public float Roll { get => roll; }

		public void Move(Key key)
		{
			switch (key)
			{
				case Key.W:
					position.Z -= moveStep;
					break;
				case Key.A:
					position.X -= moveStep;
					break;
				case Key.S:
					position.Z += moveStep;
					break;
				case Key.D:
					position.X += moveStep;
					break;
				default:
					break;
			}
		}
	}
}
