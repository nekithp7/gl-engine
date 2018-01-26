using OpenTK;
using OpenTK.Input;

using engine.Models;

namespace engine.Entities
{
	public class Player : Entity
	{
		private const float MOVE_SPEED = 0.1f;

		private Camera camera;

		public Player(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
			: base(model, position, rotation, scale) => camera = new Camera(ref position, 2.0f);

		public Camera Camera => camera;

		public void Move(Key key)
		{
			float dx = 0, dz = 0;
			switch (key)
			{
				case Key.W:
					dz = 2;
					break;

				case Key.S:
					dz = -2;
					break;

				case Key.D:
					dx = 2;
					break;

				case Key.A:
					dx = -2;
					break;

				default:
					break;
			}

			var viewMatrix = camera.ViewMatrix;
			var forward = new Vector3(viewMatrix[0, 2], viewMatrix[1, 2], viewMatrix[2, 2]);
			var strafe = new Vector3(viewMatrix[0, 0], viewMatrix[1, 0], viewMatrix[2, 0]);

			var offset = (-dz * forward + dx * strafe) * MOVE_SPEED;
			offset.Y = 0.0f;

			Translate(offset);
			camera.Move(ref position, offset);
		}

		public void Rotate(int xDelta, int yDelta)
		{
			Rotate(new Vector3(0, -xDelta * 0.1f, 0));
			camera.Rotate(ref position, ref rotation, xDelta, yDelta);
		}
	}
}
