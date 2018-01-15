namespace engine.Models
{
	public class RawModel
	{
		private readonly int vaoID;
		private readonly int vertexCount;

		public RawModel(int vaoID, int vertexCount)
		{
			this.vaoID = vaoID;
			this.vertexCount = vertexCount;
		}

		public int VaoID => vaoID;
		public int VertexCount => vertexCount;
	}
}
