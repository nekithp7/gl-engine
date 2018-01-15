using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace engine.Shaders
{
	public abstract class ShaderProgram
	{
		private int programId;
		private int vertexShaderId;
		private int fragmentShaderId;

		public ShaderProgram(string vertexFile, string fragmentFile)
		{
			vertexShaderId = LoadShader(vertexFile, ShaderType.VertexShader);
			fragmentShaderId = LoadShader(fragmentFile, ShaderType.FragmentShader);

			programId = GL.CreateProgram();
			GL.AttachShader(programId, vertexShaderId);
			GL.AttachShader(programId, fragmentShaderId);
			GL.LinkProgram(programId);
			GL.ValidateProgram(programId);

			GetAllUniformLocations();
		}

		public abstract void GetAllUniformLocations();

		protected int GetUniformLocation(string uniformName) => GL.GetUniformLocation(programId, uniformName);

		public void Start() => GL.UseProgram(programId);

		public void Stop() => GL.UseProgram(0);

		public void CleanUp()
		{
			Stop();
			GL.DetachShader(programId, vertexShaderId);
			GL.DetachShader(programId, fragmentShaderId);

			GL.DeleteShader(vertexShaderId);
			GL.DeleteShader(fragmentShaderId);

			GL.DeleteProgram(programId);
		}

		public abstract void BindAtributes();

		public void BindAtributes(int attribute, string varName) => GL.BindAttribLocation(programId, attribute, varName);

		protected void LoadFloat(int location, float value) => GL.Uniform1(location, value);

		protected void LoadVector(int location, Vector3 vector) => GL.Uniform3(location, vector);

		protected void LoadBool(int location, bool value)
		{
			float toLoad = value ? 1.0f : 0.0f;
			GL.Uniform1(location, toLoad);
		}

		protected void LoadMatrix(int location, Matrix4 matrix) => GL.UniformMatrix4(location, false, ref matrix);

		private int LoadShader(string file, ShaderType type)
		{
			var shaderId = GL.CreateShader(type);

			GL.ShaderSource(shaderId, File.ReadAllText(file));

			GL.CompileShader(shaderId);

			return shaderId;
		}
	}
}
