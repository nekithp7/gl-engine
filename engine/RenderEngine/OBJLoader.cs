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
			List<Vector3> faces = new List<Vector3>();
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
				while (!stream.EndOfStream)
				{
					line = stream.ReadLine();
					string[] currentLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (currentLine.Length != 0)
					{
						if (currentLine[0] == "v")
						{
							float.TryParse(currentLine[1], out float x);
							float.TryParse(currentLine[2], out float y);
							float.TryParse(currentLine[3], out float z);

							var vertex = new Vector3(x, y, z);
							vertices.Add(vertex);
						}
						else if (currentLine[0] == "vt")
						{
							float.TryParse(currentLine[1], out float u);
							float.TryParse(currentLine[2], out float v);

							var texture = new Vector2(u, v);
							textures.Add(texture);
						}
						else if (currentLine[0] == "vn")
						{
							float.TryParse(currentLine[1], out float x);
							float.TryParse(currentLine[2], out float y);
							float.TryParse(currentLine[3], out float z);

							var normal = new Vector3(x, y, z);
							normals.Add(normal);
						}
						else if (currentLine[0] == "f")
						{
							for (int i = 1; i < 4; i++)
							{								
								var face = currentLine[i].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

								int.TryParse(face[0], out int v);
								int.TryParse(face[1], out int vt);
								int.TryParse(face[2], out int vn);

								faces.Add(new Vector3(v, vt, vn));
							}
						}
					}
				}
			}

			verticesArray = new float[vertices.Count * 3];
			texturesArray = new float[vertices.Count * 2];
			normalsArray = new float[vertices.Count * 3];

			foreach (var face in faces)
			{
				ProcessVertex(face, indices, textures, normals, texturesArray, normalsArray);
			}

			int pointer = 0;
			foreach (var vertex in vertices)
			{
				verticesArray[pointer++] = vertex.X;
				verticesArray[pointer++] = vertex.Y;
				verticesArray[pointer++] = vertex.Z;
			}

			indicesArray = new int[indices.Count];
			indices.CopyTo(indicesArray);

			return loader.LoadToVao(verticesArray, texturesArray, normalsArray, indicesArray);
		}

		private void ProcessVertex(Vector3 data, List<int> indices, List<Vector2> textures, List<Vector3> normals,
			float[] texturesArray, float[] normalsArray)
		{
			int currentVertexPointer = (int)data.X - 1;
			indices.Add(currentVertexPointer);

			int currentTexturePointer = (int)data.Y - 1;
			var currentTexture = textures[currentTexturePointer];

			texturesArray[currentVertexPointer * 2] = currentTexture.X;
			texturesArray[currentVertexPointer * 2 + 1] = 1 - currentTexture.Y;

			int currentNormalPointer = (int)data.Z - 1;
			var currentNormal = normals[currentNormalPointer];

			normalsArray[currentVertexPointer * 3] = currentNormal.X;
			normalsArray[currentVertexPointer * 3 + 1] = currentNormal.Y;
			normalsArray[currentVertexPointer * 3 + 2] = currentNormal.Z;
		}
	}
}
