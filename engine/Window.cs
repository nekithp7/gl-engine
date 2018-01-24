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

		Camera camera = new Camera();
		Loader loader = new Loader();
		OBJLoader objLoader = new OBJLoader();
		Light light = new Light(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f));

		MasterRenderer renderer;
		Entity entity;
		Terrain terrain;		

		public Window()
			: base(WIDTH, HEIGHT, GraphicsMode.Default, "test")
		{
			VSync = VSyncMode.On;
			GL.Viewport(0, 0, WIDTH, HEIGHT);

			renderer = new MasterRenderer(WIDTH, HEIGHT);

			var rawModel = objLoader.LoadObjModel("entity", loader);
			var texture = new ModelTexture(loader.LoadTexture("texture_entity"))
			{
				ShineDamper = 10.0f,
				Reflectivity = 1.0f
			};
			var texturedModel = new TexturedModel(rawModel, texture);
			entity = new Entity(texturedModel,
				new Vector3(0.0f, 0.0f, -15.0f),
				new Vector3(0.0f),
				1.0f);

			var terrainTexture = new ModelTexture(loader.LoadTexture("texture_terrain"));
			terrain = new Terrain(0, 0, loader, terrainTexture);			
		}

		public sealed override void Dispose()
		{
			renderer.CleanUp();
			loader.CleanUp();
			Dispose(true);
			base.Dispose();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);			

			HandleKeyboardState(Keyboard.GetState());
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			renderer.ProcessTerrain(terrain);
			renderer.ProcessEntity(entity);
			renderer.Render(light, camera);

			SwapBuffers();
		}

		private void HandleKeyboardState(KeyboardState state)
		{
			if (state.IsAnyKeyDown)
			{
				if (state[Key.Escape])
				{
					Exit();
				}
				else if (state[Key.W])
				{
					camera.Move(Key.W);
				}
				else if (state[Key.A])
				{
					camera.Move(Key.A);
				}
				else if (state[Key.S])
				{
					camera.Move(Key.S);
				}
				else if (state[Key.D])
				{
					camera.Move(Key.D);
				}
			}
		}
	}
}
