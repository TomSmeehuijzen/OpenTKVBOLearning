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
        private int vertexDataBufferHandle, vertexColorBufferHandle, programHandle, MVPMatrixHandle;
        private float[] vertexData, colorData;
        Matrix4 modelMatrix;
        Camera camera;
        public ObjectDrawer(Camera camera)
        {
            WindowManager.Draw += OnDraw;
            // Fill the vertex coordinates array
            FillVertexData(-10,-10,0,20f,20f);
            // Fill the vertex color array
            FillColorData();
            // Compile the shaders
            CompileShaderProgram();
            // Prepare and fill the buffers
            PrepareBuffer();

            // Initialize an untransformed model matrix
            modelMatrix = Matrix4.Identity;

            // Get a handle for a uniform called "MVP"
            MVPMatrixHandle = GL.GetUniformLocation(programHandle, "MVP");
            this.camera = camera;
        }

        // Initialization functions:
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
            // Create shaders
            int vertShader = GL.CreateShader(ShaderType.VertexShader);
            int fragShader = GL.CreateShader(ShaderType.FragmentShader);
            // Load in the shaders from my files
            GL.ShaderSource(vertShader, File.ReadAllText("VBOVertexShader.vert"));
            GL.ShaderSource(fragShader, File.ReadAllText("VBOFragShader.frag"));

            // Generate a program
            programHandle = GL.CreateProgram();
            // Attach the shaders
            GL.AttachShader(programHandle, vertShader);
            GL.AttachShader(programHandle, fragShader);
            // Don't actually know what this does
            GL.LinkProgram(programHandle);
            // Clean up shop
            GL.DetachShader(programHandle, vertShader);
            GL.DetachShader(programHandle, fragShader);
            GL.DeleteShader(vertShader);
            GL.DeleteShader(fragShader);
        }
        private void PrepareBuffer()
        {
            // Generate buffers
            vertexDataBufferHandle = GL.GenBuffer();
            vertexColorBufferHandle = GL.GenBuffer();

            // Bind and fill the vertex buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexDataBufferHandle);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, System.Buffer.ByteLength(vertexData), vertexData, BufferUsageHint.StaticDraw);
            
            // Bind and fill the color buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexColorBufferHandle);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, System.Buffer.ByteLength(colorData), colorData, BufferUsageHint.StaticDraw);

            // Unbind from the arraybuffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        // Actual drawing function
        private void OnDraw(object sender, EventArgs e)
        {
            // Tell openGL to use my shaders
            GL.UseProgram(programHandle);
            // Get the MVP matrix from my camera
            Matrix4 mvpMatrix = camera.GetMVPMatrix(modelMatrix);
            // Put the mvpMatrix in the uniform
            GL.UniformMatrix4(MVPMatrixHandle, false, ref mvpMatrix);

            // Get the vertex data to the GPU
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexDataBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Get the color data to the GPU
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexColorBufferHandle);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

            // Draw my stuff
            GL.DrawArrays(PrimitiveType.Quads, 0, vertexData.Length / 2);

            // Disable the attributes
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
    }
}
