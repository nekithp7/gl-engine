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

		public Camera Camera { get => camera; set => camera = value; }

		public void Move(Key key)
		{
			switch (key)
			{
				case Key.W:
					currentSpeed = -MOVE_SPEED;
					break;

				case Key.S:
					currentSpeed = MOVE_SPEED;
					break;

				default:
					currentSpeed = 0;
					break;
			}

			float xDelta = currentSpeed * (float)Sin(ToRad(rotation.Y));
			float zDelta = currentSpeed * (float)Cos(ToRad(rotation.Y));

			Translate(new Vector3(xDelta, 0.0f, zDelta));
		}

		public void Rotate(int xDelta, int yDelta)
		{
			Rotate(new Vector3(0, -xDelta * 0.1f, 0));
		}
	}
}
