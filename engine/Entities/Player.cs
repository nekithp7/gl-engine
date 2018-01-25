using static System.Math;

using OpenTK;
using OpenTK.Input;

using engine.Models;
using static engine.Tools.Maths;

namespace engine.Entities
{
	public class Player : Entity
	{
		private const float MOVE_SPEED = 0.1f;

		private Camera camera;
		private float currentSpeed = 0.0f;

		public Player(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
			: base(model, position, rotation, scale) => camera = new Camera(position);

		public Camera Camera => camera;

		public void Move(Key key)
		{
			bool isStrafe = false;
			switch (key)
			{
				case Key.W:
					currentSpeed = -MOVE_SPEED;
					isStrafe = false;
					break;

				case Key.S:
					currentSpeed = MOVE_SPEED;
					isStrafe = false;
					break;

				case Key.D:
					currentSpeed = MOVE_SPEED;
					isStrafe = true;
					break;

				case Key.A:
					currentSpeed = -MOVE_SPEED;
					isStrafe = true;
					break;

				default:
					currentSpeed = 0;
					break;
			}

			var (offsetX, offsetZ) = GetPosition(isStrafe);

			Translate(new Vector3(offsetX, 0.0f, offsetZ));
		}

		public void Rotate(int xDelta, int yDelta)
		{
			Rotate(new Vector3(0, -xDelta * 0.1f, 0));
		}

		private (float offsetX, float offsetZ) GetPosition(bool isStrafe)
		{
			float offsetX = currentSpeed * (float)Sin(ToRad(rotation.Y));
			float offsetZ = currentSpeed * (float)Cos(ToRad(rotation.Y));

			return isStrafe ? (offsetZ, offsetX) : (offsetX, offsetZ);
		}
	}
}
