using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

using engine.Models;

namespace engine.RenderEngine
{
	public class Loader
	{
		private List<int> vaos = new List<int>();
		private List<int> vbos = new List<int>();
		private List<int> textures = new List<int>();

		public RawModel LoadToVao(float[] positions, float[] textureCoords, float[] normals, int[] indices)
		{
			int vaoID = CreateVao();
			BindIndicesBuffer(indices);
			StoreDataInAttributeList(0, 3, positions);
			StoreDataInAttributeList(1, 2, textureCoords);
			StoreDataInAttributeList(2, 3, normals);
			GL.BindVertexArray(0);
			return new RawModel(vaoID, indices.Length);
		}

		public void CleanUp()
		{
			foreach (var vaoID in vaos)
			{
				GL.DeleteVertexArray(vaoID);
			}

			foreach (var vboID in vbos)
			{
				GL.DeleteBuffer(vboID);
			}

			foreach (var texture in textures)
			{
				GL.DeleteTexture(texture);
			}
		}

		private int CreateVao()
		{
			int vaoID = GL.GenVertexArray();

			vaos.Add(vaoID);

			GL.BindVertexArray(vaoID);
			return vaoID;
		}

		private void StoreDataInAttributeList(int attributeNumber, int coordinateSize, float[] data)
		{
			GL.GenBuffers(1, out int vboID);

			vbos.Add(vboID);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);

			GL.BufferData(
				BufferTarget.ArrayBuffer,
				(IntPtr)(data.Length * sizeof(float)),
				data,
				BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(
				attributeNumber,
				coordinateSize,
				VertexAttribPointerType.Float,
				false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		private void BindIndicesBuffer(int[] indices)
		{
			GL.GenBuffers(1, out int vboID);
			vbos.Add(vboID);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);

			GL.BufferData(
				BufferTarget.ElementArrayBuffer,
				(IntPtr)(indices.Length * sizeof(int)),
				indices,
				BufferUsageHint.StaticDraw);
		}

		public int LoadTexture(string file)
		{
			var textureId = GL.GenTexture();
			textures.Add(textureId);

			Bitmap image = new Bitmap(@"..\..\res\" + file + ".png");
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			BitmapData data = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			image.UnlockBits(data);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			return textureId;
		}
	}
}
