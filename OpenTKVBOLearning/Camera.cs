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
            this.FoV = FoV*(float)Math.PI/180;
            this.width = width;
            this.height = height;
            this.near = nearPlane;
            this.far = farPlane;
            this.lookat = lookat;

            SetPosition(position);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(this.FoV, width / height, near, far);
        }

        public Matrix4 GetMVPMatrix(Matrix4 modelMatrix)
        {
            return projectionMatrix * viewMatrix * modelMatrix;
        }

        public void SetPosition(Vector3 newPosition)
        {
            position = newPosition;
            viewMatrix = Matrix4.LookAt(position, lookat, Vector3.UnitY);
        }
    }
}