using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using engine.Tools;
using engine.Models;
using engine.Shaders;
using engine.Entities;

namespace engine.RenderEngine
{
	public class EntityRenderer
	{
		private StaticShader shader;

		public EntityRenderer(StaticShader shader, Matrix4 projectionMatrix)
		{
			this.shader = shader;
			shader.Start();
			shader.LoadProjectionMatrix(projectionMatrix);
			shader.Stop();
		}

		public void Render(Dictionary<TexturedModel, List<Entity>> entities)
		{

			foreach (var item in entities)
			{
				PrepareTexturedModel(item.Key);
				foreach (var entity in item.Value)
				{
					PrepareInstance(entity);
					GL.DrawElements(BeginMode.Triangles, item.Key.Model.VertexCount, DrawElementsType.UnsignedInt, 0);
				}

				UnbindTexturedModel();
			}
		}

		private void PrepareTexturedModel(TexturedModel texturedModel)
		{
			var rawModel = texturedModel.Model;

			GL.BindVertexArray(rawModel.VaoID);
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.EnableVertexAttribArray(2);

			var texture = texturedModel.Texture;
			shader.LoadShineVariables(texture.ShineDamper, texture.Reflectivity);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texture.TextureId);
		}

		private void PrepareInstance(Entity entity)
		{
			Matrix4 transformationMatrix = Maths.CreateTransformationMatrix(
				entity.Position, entity.Rotation, entity.Scale);
			shader.LoadTransformationMatrix(transformationMatrix);
		}

		private void UnbindTexturedModel()
		{
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
			GL.DisableVertexAttribArray(2);
			GL.BindVertexArray(0);
		}
	}
}
