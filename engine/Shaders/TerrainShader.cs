using OpenTK;

using engine.Entities;
using engine.Tools;

namespace engine.Shaders
{
	public class TerrainShader : ShaderProgram
	{
		private const string VERTEX_FILE = @"..\..\Shaders\Programs\terrainVertexShader.vert";
		private const string FRAGMENT_FILE = @"..\..\Shaders\Programs\terrainFragmentShader.frag";

		private int transformationMatrix;
		private int projectionMatrix;
		private int viewMatrix;
		private int lightPosition;
		private int lightColour;
		private int shineDamper;
		private int reflectivity;

		public TerrainShader() : base(VERTEX_FILE, FRAGMENT_FILE) { }

		public override void BindAtributes()
		{
			BindAtributes(0, "position");
			BindAtributes(1, "textureCoords");
			BindAtributes(2, "normal");
		}

		public override void GetAllUniformLocations()
		{
			projectionMatrix = GetUniformLocation("projectionMatrix");
			viewMatrix = GetUniformLocation("viewMatrix");
			transformationMatrix = GetUniformLocation("transformationMatrix");
			lightPosition = GetUniformLocation("lightPosition");
			lightColour = GetUniformLocation("lightColour");
			shineDamper = GetUniformLocation("shineDamper");
			reflectivity = GetUniformLocation("reflectivity");
		}

		public void LoadShineVariables(float shineDamper, float reflectivity)
		{
			LoadFloat(this.shineDamper, shineDamper);
			LoadFloat(this.reflectivity, reflectivity);
		}

		public void LoadProjectionMatrix(Matrix4 matrix) => LoadMatrix(projectionMatrix, matrix);

		public void LoadViewMatrix(Camera camera)
		{
			var matrix = Maths.CreateViewMatrix(camera);

			LoadMatrix(viewMatrix, matrix);
		}

		public void LoadTransformationMatrix(Matrix4 matrix) => LoadMatrix(transformationMatrix, matrix);

		public void LoadLight(Light light)
		{
			LoadVector(lightPosition, light.Position);
			LoadVector(lightColour, light.Colour);
		}
	}
}
