using System;
using System.IO;
using System.Collections.Generic;

using OpenTK;

using engine.Models;

namespace engine.RenderEngine
{
	public class OBJLoader
	{
		private const string PATH = @"..\..\res\models\";

		public RawModel LoadObjModel(string fileName, Loader loader)
		{
			string line;
			List<Vector3> vertices = new List<Vector3>();
			List<Vector2> textures = new List<Vector2>();
			List<Vector3> normals = new List<Vector3>();
			List<int> indices = new List<int>();

			float[] verticesArray;
			float[] texturesArray;
			float[] normalsArray;
			int[] indicesArray;

			StreamReader stream;

			try
			{
				stream = new StreamReader($"{PATH}{fileName}.obj");
			}
			catch (IOException ex)
			{
				stream = new StreamReader($"{PATH}default_model.obj");
				Console.WriteLine($"Model file [ {fileName}.obj ] not found. Loaded default model.");
				Console.WriteLine(ex.StackTrace);
			}

			using (stream)
			{
				while (true)
				{
					line = stream.ReadLine();
					string[] currentLine = line.Split(' ');

					if (line.StartsWith("v "))
					{
						float.TryParse(currentLine[1], out float x);
						float.TryParse(currentLine[2], out float y);
						float.TryParse(currentLine[3], out float z);

						var vertex = new Vector3(x, y, z);
						vertices.Add(vertex);
					}
					else if (line.StartsWith("vt "))
					{
						float.TryParse(currentLine[1], out float u);
						float.TryParse(currentLine[2], out float v);

						var texture = new Vector2(u, v);
						textures.Add(texture);
					}
					else if (line.StartsWith("vn "))
					{
						float.TryParse(currentLine[1], out float x);
						float.TryParse(currentLine[2], out float y);
						float.TryParse(currentLine[3], out float z);

						var normal = new Vector3(x, y, z);
						normals.Add(normal);
					}
					else if (line.StartsWith("f "))
					{
						texturesArray = new float[vertices.Count * 2];
						normalsArray = new float[vertices.Count * 3];
						break;
					}
				}

				while (!stream.EndOfStream)
				{
					if (!line.StartsWith("f "))
					{
						line = stream.ReadLine();
						continue;
					}
					string[] currentLine = line.Split(' ');
					string[] vertex1 = currentLine[1].Split('/');
					string[] vertex2 = currentLine[2].Split('/');
					string[] vertex3 = currentLine[3].Split('/');

					ProcessVertex(vertex1, indices, textures, normals, texturesArray, normalsArray);
					ProcessVertex(vertex2, indices, textures, normals, texturesArray, normalsArray);
					ProcessVertex(vertex3, indices, textures, normals, texturesArray, normalsArray);

					line = stream.ReadLine();
				}
			}

			verticesArray = new float[vertices.Count * 3];
			indicesArray = new int[indices.Count];

			int pointer = 0;
			foreach (var vertex in vertices)
			{
				verticesArray[pointer++] = vertex.X;
				verticesArray[pointer++] = vertex.Y;
				verticesArray[pointer++] = vertex.Z;
			}

			indices.CopyTo(indicesArray);

			return loader.LoadToVao(verticesArray, texturesArray, normalsArray, indicesArray);
		}

		private void ProcessVertex(string[] data, List<int> indices, List<Vector2> textures, List<Vector3> normals,
			float[] texturesArray, float[] normalsArray)
		{
			int.TryParse(data[0], out int currentVertexPointer);
			currentVertexPointer -= 1;
			indices.Add(currentVertexPointer);

			int.TryParse(data[1], out int currentTexturePointer);
			currentTexturePointer -= 1;
			var currentTexture = textures[currentTexturePointer];

			texturesArray[currentVertexPointer * 2] = currentTexture.X;
			texturesArray[currentVertexPointer * 2 + 1] = 1 - currentTexture.Y;

			int.TryParse(data[2], out int currentNormalPointer);
			currentNormalPointer -= 1;
			var currentNormal = normals[currentNormalPointer];

			normalsArray[currentVertexPointer * 3] = currentNormal.X;
			normalsArray[currentVertexPointer * 3 + 1] = currentNormal.Y;
			normalsArray[currentVertexPointer * 3 + 2] = currentNormal.Z;
		}
	}
}
