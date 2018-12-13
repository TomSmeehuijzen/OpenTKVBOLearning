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
            // Initialize camera at 0,3,-5, looking at the origin, widht an FoV of 45 degrees
            camera = new Camera(new Vector3(0, 3, -5), new Vector3(0, 0, 0), 45f, Program.window.Width, Program.window.Height, .1f, 100f);
            drawer = new ObjectDrawer(camera);
            sininput = 0;
            motionZ = 0;
            motionX = 0;

            // Subscribe to relevant events
            Program.window.RenderFrame += OnRenderFrame;
            Program.window.Load += OnLoad;
            Program.window.Resize += OnResize;
        }

        private void OnResize(object sender, EventArgs e)
        {
            // Reset the viewport to cover the whole window.
            GL.Viewport(0, 0, Program.window.Width, Program.window.Height);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // Set the background color to a nice ugly blue
            GL.ClearColor(.2f, .3f, .5f, 1f);
        }

        private void OnRenderFrame(object sender, OpenTK.FrameEventArgs e)
        {
            // Rotate the position of the camera around to make sure I'm not
            // running into any weird orientation issues.
            sininput += .01f;
            motionZ = (float)Math.Sin((double)sininput) * 10;
            motionX = (float)Math.Cos((double)sininput) * 10;
            camera.SetPosition(new Vector3(motionX, 3, motionZ));
            // For monitoring:
            Debug.WriteLine("current motion x:{0}, y:3, z:{1}", motionX, motionZ);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            // Raise the draw event
            Draw?.Invoke(this, EventArgs.Empty);
            GL.Flush();
            Program.window.SwapBuffers();
        }

        public static event EventHandler Draw;
    }
}