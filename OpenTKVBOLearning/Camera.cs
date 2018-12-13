using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKVBOLearning
{
    class Camera
    {
        float FoV, width, height, near, far;
        Vector3 position, lookat;
        Matrix4 viewMatrix, projectionMatrix;
        public Camera(Vector3 position, Vector3 lookat, float FoV, float width, float height, float nearPlane, float farPlane)
        {
            // Convert from degrees to radians
            this.FoV = FoV*(float)Math.PI/180;

            this.width = width;
            this.height = height;
            this.near = nearPlane;
            this.far = farPlane;
            this.lookat = lookat;
            // Set the initial position and create the view matrix
            SetPosition(position);
            // Create the projection matrix
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(this.FoV, width / height, near, far);
        }

        public Matrix4 GetMVPMatrix(Matrix4 modelMatrix)
        {
            // Multiply the matrices together to get the MVP matrix that needs to be passed to the vertex shader
            return projectionMatrix * viewMatrix * modelMatrix;
        }

        public void SetPosition(Vector3 newPosition)
        {
            // Reset the position and recreate the viewmatrix
            position = newPosition;
            viewMatrix = Matrix4.LookAt(position, lookat, Vector3.UnitY);
        }
    }
}