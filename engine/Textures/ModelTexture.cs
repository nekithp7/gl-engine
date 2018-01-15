namespace engine.Textures
{
	public class ModelTexture
	{
		private readonly int textureId;
		private float shineDamper = 1.0f;
		private float reflectivity = 0.0f;

		public ModelTexture(int textureId)
		{
			this.textureId = textureId;
		}

		public int TextureId => textureId;
		public float ShineDamper { get => shineDamper; set => shineDamper = value; }
		public float Reflectivity { get => reflectivity; set => reflectivity = value; }
	}
}
