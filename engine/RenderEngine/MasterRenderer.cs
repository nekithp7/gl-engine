using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;

using engine.Tools;
using engine.Models;
using engine.Shaders;
using engine.Entities;
using engine.Terrains;

namespace engine.RenderEngine
{
	public class MasterRenderer
	{
		private const float FOV = 70.0f;
		private const float NEAR_PLANE = 0.1f;
		private const float FAR_PLANE = 1000.0f;

		private EntityRenderer entityRenderer;
		private StaticShader staticShader = new StaticShader();

		private TerrainRenderer terrainRenderer;
		private TerrainShader terrainShader = new TerrainShader();

		private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();
		private List<Terrain> terrains = new List<Terrain>();

		public MasterRenderer(int width, int height)
		{
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);

			var projectionMatrix = Maths.CreateProjectionMatrix(width, height, FOV, FAR_PLANE, NEAR_PLANE);
			entityRenderer = new EntityRenderer(staticShader, projectionMatrix);
			terrainRenderer = new TerrainRenderer(terrainShader, projectionMatrix);
		}

		public void Render(Light light, Camera camera)
		{
			Prepare();
			staticShader.Start();
			staticShader.LoadLight(light);
			staticShader.LoadViewMatrix(camera);

			entityRenderer.Render(entities);

			staticShader.Stop();

			terrainShader.Start();
			terrainShader.LoadLight(light);
			terrainShader.LoadViewMatrix(camera);

			terrainRenderer.Render(terrains);

			terrainShader.Stop();

			entities.Clear();
			terrains.Clear();
		}

		public void ProcessEntity(Entity entity)
		{
			var texturedModel = entity.Model;
			var res = entities.TryGetValue(texturedModel, out var batch);

			if (res != false)
			{
				batch.Add(entity);
			}
			else
			{
				var newBatch = new List<Entity>
				{
					entity
				};
				entities.Add(texturedModel, newBatch);
			}
		}

		public void ProcessTerrain(Terrain terrain)
		{
			terrains.Add(terrain);
		}

		public void CleanUp()
		{
			staticShader.CleanUp();
			terrainShader.CleanUp();
		}

		private void Prepare()
		{
			GL.Enable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(0.6f, 0, 0, 1);
		}
	}
}
