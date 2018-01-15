using OpenTK;

namespace engine.Entities
{
	public class Light
	{
		private Vector3 position;
		private Vector3 colour;

		public Light(Vector3 position, Vector3 colour)
		{
			this.position = position;
			this.colour = colour;
		}

		public Vector3 Position => position;
		public Vector3 Colour => colour;
	}
}
