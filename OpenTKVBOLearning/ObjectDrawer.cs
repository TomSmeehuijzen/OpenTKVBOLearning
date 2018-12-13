using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Diagnostics;
using OpenTK;

namespace OpenTKVBOLearning
{
    class ObjectDrawer
    {
        private int vertexDataBufferHandle, vertexColorBufferHandle, programID, MVPMatrixID;
        private float[] vertexData, colorData;
        Matrix4 modelMatrix;
        Camera camera;
        public ObjectDrawer(Camera camera)
        {
            WindowManager.Draw += OnDraw;
            FillVertexData(-10,-10,0,20f,20f);
            FillColorData();
            CompileShaderProgram();
            PrepareBuffer();

            modelMatrix = Matrix4.Identity;
            MVPMatrixID = GL.GetUniformLocation(programID, "MVP");
            this.camera = camera;
        }

        private void FillVertexData(float x, float y, float z, float width, float length)
        {
            vertexData = new float[]
            {
                x, y, z,
                x+width, y, z,
                x+width, y, z+length,
                x, y, z+length
            };
        }
        private void FillColorData()
        {
            Random r = new Random();
            List<float> colorList = new List<float>();
            foreach(float f in vertexData)
            {
                colorList.Add(((float)r.Next(100)) / 100);
                colorList.Add(((float)r.Next(100)) / 100);
                colorList.Add(((float)r.Next(100)) / 100);
            }
            colorData = colorList.ToArray<float>();
        }
        private void CompileShaderProgram()
        {
            int vertShader = GL.CreateShader(ShaderType.VertexShader);
            int fragShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(vertShader, File.ReadAllText("VBOVertexShader.vert"));
            GL.ShaderSource(fragShader, File.ReadAllText("VBOFragShader.frag"));

            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertShader);
            GL.AttachShader(programID, fragShader);
            GL.LinkProgram(programID);
            GL.DetachShader(programID, vertShader);
            GL.DetachShader(programID, fragShader);
            GL.DeleteShader(vertShader);
            GL.DeleteShader(fragShader);
        }

        private void PrepareBuffer()
        {
            vertexDataBufferHandle = GL.GenBuffer();
            vertexColorBufferHandle = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexDataBufferHandle);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, System.Buffer.ByteLength(vertexData), vertexData, BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexColorBufferHandle);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, System.Buffer.ByteLength(colorData), colorData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void OnDraw(object sender, EventArgs e)
        {
            GL.UseProgram(programID);
            
            Matrix4 mvpMatrix = camera.GetMVPMatrix(modelMatrix);
            GL.UniformMatrix4(MVPMatrixID, false, ref mvpMatrix);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexDataBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexColorBufferHandle);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Quads, 0, vertexData.Length / 2);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
    }
}
