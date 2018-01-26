using engine.Models;
using engine.Textures;
using engine.RenderEngine;

namespace engine.Terrains
{
	public class Terrain
	{
		private const float SIZE = 800;
		private const int VERTEX_COUNT = 128;

		private float x;
		private float z;
		private RawModel model;
		private ModelTexture texture;

		public Terrain(int gridX, int gridZ, Loader loader, ModelTexture texture)
		{
			this.texture = texture;
			x = gridX * SIZE;
			z = gridZ * SIZE;

			model = GenerateTerrain(loader);
		}

		public float X => x;
		public float Z => z;
		public RawModel Model => model;
		public ModelTexture Texture => texture;

		private RawModel GenerateTerrain(Loader loader)
		{
			int count = VERTEX_COUNT * VERTEX_COUNT;
			float[] vertices = new float[count * 3];
			float[] normals = new float[count * 3];
			float[] textureCoords = new float[count * 2];
			int[] indices = new int[6 * (VERTEX_COUNT - 1) * (VERTEX_COUNT - 1)];
			int vertexPointer = 0;
			for (int i = 0; i < VERTEX_COUNT; i++)
			{
				for (int j = 0; j < VERTEX_COUNT; j++)
				{
					vertices[vertexPointer * 3] = j / ((float)VERTEX_COUNT - 1) * SIZE;
					vertices[vertexPointer * 3 + 1] = 0;
					vertices[vertexPointer * 3 + 2] = i / ((float)VERTEX_COUNT - 1) * SIZE;
					normals[vertexPointer * 3] = 0;
					normals[vertexPointer * 3 + 1] = 1;
					normals[vertexPointer * 3 + 2] = 0;
					textureCoords[vertexPointer * 2] = j / ((float)VERTEX_COUNT - 1) * SIZE;
					textureCoords[vertexPointer * 2 + 1] = i / ((float)VERTEX_COUNT - 1) * SIZE;
					vertexPointer++;
				}
			}
			int pointer = 0;
			for (int gz = 0; gz < VERTEX_COUNT - 1; gz++)
			{
				for (int gx = 0; gx < VERTEX_COUNT - 1; gx++)
				{
					int topLeft = (gz * VERTEX_COUNT) + gx;
					int topRight = topLeft + 1;
					int bottomLeft = ((gz + 1) * VERTEX_COUNT) + gx;
					int bottomRight = bottomLeft + 1;
					indices[pointer++] = topLeft;
					indices[pointer++] = bottomLeft;
					indices[pointer++] = topRight;
					indices[pointer++] = topRight;
					indices[pointer++] = bottomLeft;
					indices[pointer++] = bottomRight;
				}
			}
			return loader.LoadToVao(vertices, textureCoords, normals, indices);
		}
	}
}
