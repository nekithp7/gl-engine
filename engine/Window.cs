using System;
using System.Collections.Generic;

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

		List<Terrain> terrain = new List<Terrain>();
		bool isMouseMoved = false;

		Loader loader = new Loader();
		OBJLoader objLoader = new OBJLoader();
		Light light = new Light(new Vector3(-5.0f, 10.0f, 5.0f), new Vector3(1.0f));

		MasterRenderer renderer;
		Player player;
		Entity entity;

		public Window()
			: base(WIDTH, HEIGHT, GraphicsMode.Default, "test")
		{
			VSync = VSyncMode.On;
			CursorVisible = false;
			GL.Viewport(0, 0, WIDTH, HEIGHT);
			OpenTK.Input.Mouse.SetPosition(WIDTH / 2, HEIGHT / 2);

			Mouse.Move += HandleMouseInput;
		}

		public sealed override void Dispose()
		{
			renderer.CleanUp();
			loader.CleanUp();
			Dispose(true);
			base.Dispose();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			renderer = new MasterRenderer(WIDTH, HEIGHT);
			var playerModelMesh = objLoader.LoadObjModel("player", loader);
			var playerTexture = new ModelTexture(loader.LoadTexture("texture_player"))
			{
				ShineDamper = 5.0f,
				Reflectivity = 1.0f
			};
			var texturedPlayerModel = new TexturedModel(playerModelMesh, playerTexture);

			player = new Player(texturedPlayerModel,
				new Vector3(0.0f, 0.0f, -2.0f),
				new Vector3(0.0f),
				0.05f);

			var rawModel = objLoader.LoadObjModel("entity", loader);
			var texture = new ModelTexture(loader.LoadTexture("texture_entity"))
			{
				ShineDamper = 10.0f,
				Reflectivity = 1.0f
			};
			var texturedModel = new TexturedModel(rawModel, texture);

			entity = new Entity(texturedModel,
				new Vector3(0.0f, 0.0f, -7.0f),
				new Vector3(0.0f),
				1.0f);

			var terrainTexture = new ModelTexture(loader.LoadTexture("texture_terrain"));
			for (int x = -1; x < 1; x++)
			{
				for (int z = -1; z < 1; z++)
				{
					terrain.Add(new Terrain(x, z, loader, terrainTexture));
				}
			}
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			HandleKeyboardInput(Keyboard.GetState());
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			foreach (var block in terrain)
			{
				renderer.ProcessTerrain(block);
			}

			renderer.ProcessEntity(entity);
			renderer.ProcessEntity(player);
			renderer.Render(light, player.Camera);

			SwapBuffers();
		}

		private void HandleKeyboardInput(KeyboardState state)
		{
			if (state.IsAnyKeyDown)
			{
				if (state[Key.Escape])
				{
					Exit();
				}
				else if (state[Key.W])
				{
					player.Move(Key.W);
				}
				else if (state[Key.A])
				{
					player.Move(Key.A);
				}
				else if (state[Key.S])
				{
					player.Move(Key.S);
				}
				else if (state[Key.D])
				{
					player.Move(Key.D);
				}
			}
		}

		private void HandleMouseInput(object sender, MouseMoveEventArgs e)
		{
			if (isMouseMoved)
			{
				player.Rotate(e.XDelta, e.YDelta);
				isMouseMoved = false;
				OpenTK.Input.Mouse.SetPosition(WIDTH / 2, HEIGHT / 2);
			}
			else
			{
				isMouseMoved = true;
			}
		}
	}
}
