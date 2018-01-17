using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using engine.Models;
using engine.Textures;
using engine.Entities;
using engine.Terrains;
using engine.RenderEngine;

namespace engine
{
	public class Window : GameWindow
	{
		private const int WIDTH = 800;
		private const int HEIGHT = 600;

		Loader loader;
		MasterRenderer renderer;

		Entity entity;
		Terrain terrain;
		Light light;
		Camera camera;

		public Window()
			: base(WIDTH, HEIGHT, GraphicsMode.Default, "test")
		{
			VSync = VSyncMode.On;

			GL.Viewport(0, 0, WIDTH, HEIGHT);

			loader = new Loader();
			renderer = new MasterRenderer(WIDTH, HEIGHT);
			camera = new Camera();

			var objLoader = new OBJLoader();
			var rawModel = objLoader.LoadObjModel("entity", loader);
			var texture = new ModelTexture(loader.LoadTexture("texture_entity"))
			{
				ShineDamper = 10.0f,
				Reflectivity = 1.0f
			};
			var texturedModel = new TexturedModel(rawModel, texture);

			var terrainTexture = new ModelTexture(loader.LoadTexture("texture_terrain"));
			terrain = new Terrain(0, 0, loader, terrainTexture);

			entity = new Entity(texturedModel,
				new Vector3(0.0f, 0.0f, -15.0f),
				new Vector3(0.0f),
				1.0f);
			light = new Light(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f));

			camera = new Camera();

			KeyDown += OnKeyDown;
		}

		protected override void OnUpdateFrame(FrameEventArgs e) => base.OnUpdateFrame(e);

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			renderer.ProcessTerrain(terrain);
			renderer.ProcessEntity(entity);
			renderer.Render(light, camera);

			SwapBuffers();
		}

		public sealed override void Dispose()
		{
			renderer.CleanUp();
			loader.CleanUp();
			Dispose(true);
			base.Dispose();
		}

		private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					Exit();
					break;
				default:
					camera.Move(e.Key);
					break;
			}
		}
	}
}
