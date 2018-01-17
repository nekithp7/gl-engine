using System;

using OpenTK;

using engine.Entities;

namespace engine.Tools
{
	public class Maths
	{
		public static Matrix4 CreateProjectionMatrix(int width, int height, float FOV, float farPlane, float nearPlane)
		{
			float aspectRatio = width / height;
			float yScale = (float)((1.0f / Math.Tan(ToRad(FOV / 2.0f))) * aspectRatio);
			float xScale = yScale / aspectRatio;
			float frustumLenght = farPlane - nearPlane;

			var matrix = new Matrix4
			{
				M11 = xScale,
				M22 = yScale,
				M33 = -((farPlane + nearPlane) / frustumLenght),
				M34 = -1,
				M43 = -((2 * nearPlane * farPlane) / frustumLenght),
				M44 = 0
			};

			return matrix;
		}

		public static Matrix4 CreateViewMatrix(Camera camera)
		{
			var rotationMatrix = Matrix4.CreateRotationX(ToRad(camera.Pitch));
			rotationMatrix *= Matrix4.CreateRotationY(ToRad(camera.Yaw));

			var negativeCameraPos = new Vector3(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
			var translationMatrix = Matrix4.CreateTranslation(negativeCameraPos);

			return rotationMatrix * translationMatrix;
		}

		public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, float scale)
		{
			VectorToRad(ref rotation, out Vector3 rotationInRad);

			var translationMatrix = Matrix4.CreateTranslation(translation);

			var rotationMatrix = Matrix4.CreateRotationX(rotationInRad.X);
			rotationMatrix *= Matrix4.CreateRotationY(rotationInRad.Y);
			rotationMatrix *= Matrix4.CreateRotationZ(rotationInRad.Z);

			var scaleMatrix = Matrix4.CreateScale(scale);

			return scaleMatrix * rotationMatrix * translationMatrix;
		}

		private static void VectorToRad(ref Vector3 rotation, out Vector3 vec)
		{
			rotation.X = ToRad(rotation.X);
			rotation.Y = ToRad(rotation.Y);
			rotation.Z = ToRad(rotation.Z);
			vec = rotation;
		}

		private static float ToRad(float angle) => angle * (float)(Math.PI / 180);
	}
}
