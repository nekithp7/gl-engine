using engine.Textures;

namespace engine.Models
{
	public class TexturedModel
	{
		private readonly RawModel model;
		private readonly ModelTexture texture;

		public TexturedModel(RawModel model, ModelTexture texture)
		{
			this.model = model;
			this.texture = texture;
		}
		
		public RawModel Model => model;
		public ModelTexture Texture => texture;
	}
}
