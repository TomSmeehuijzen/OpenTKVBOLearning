using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace OpenTKVBOLearning
{
    class WindowManager
    {
        ObjectDrawer drawer;
        Camera camera;
        float motionZ, motionX, sininput;
        public WindowManager()
        {
            Program.window.RenderFrame += OnRenderFrame;
            Program.window.Load += OnLoad;
            Program.window.Resize += OnResize;
            camera = new Camera(new Vector3(0,3,-5),new Vector3(0,0,0),45f,Program.window.Width,Program.window.Height,.1f, 100f);
            drawer = new ObjectDrawer(camera);
            sininput = 0;
            motionZ = 0;
            motionX = 0;
        }

        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Program.window.Width, Program.window.Height);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0, Program.window.Width, Program.window.Height, 0, -1, 1);
            //GL.MatrixMode(MatrixMode.Modelview);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            GL.ClearColor(.2f, .3f, .5f, 1f);
        }

        private void OnRenderFrame(object sender, OpenTK.FrameEventArgs e)
        {
            sininput += .01f;
            motionZ = (float)Math.Sin((double)sininput) * 10;
            motionX = (float)Math.Cos((double)sininput) * 10;

            Debug.WriteLine("current motion x:{0}, y:3, z:{1}", motionX, motionZ);

            camera.SetPosition(new Vector3(motionX, 3, motionZ));

            GL.Clear(ClearBufferMask.ColorBufferBit);
            Draw?.Invoke(this, EventArgs.Empty);
            GL.Flush();
            Program.window.SwapBuffers();
        }

        public static event EventHandler Draw;
    }
}
