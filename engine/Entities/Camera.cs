using OpenTK;
using OpenTK.Input;

namespace engine.Entities
{
	public class Camera
	{
		private Vector3 position;
		private float moveStep = 1.0f;
		private float pitch;
		private float yaw;
		private float roll;

		public Camera()
		{
			position = new Vector3(0.0f);

			pitch = 0;
			yaw = 0;
			roll = 0;
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
