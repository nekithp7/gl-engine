using OpenTK;

using engine.Models;

namespace engine.Entities
{
	public class Entity
	{
		private TexturedModel model;
		private Vector3 position;
		private Vector3 rotation;
		private float scale;

		public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
		{
			this.model = model;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		public TexturedModel Model => model;
		public Vector3 Position => position;
		public Vector3 Rotation => rotation;
		public float Scale => scale;

		public void IncreasePosition(Vector3 delta) => Vector3.Add(ref position, ref delta, out position);

		public void IncreaseRotation(Vector3 delta) => Vector3.Add(ref rotation, ref delta, out rotation);
	}
}
