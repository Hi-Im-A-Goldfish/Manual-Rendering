using ManualGraphics.Game.Rendering;
using ManualGraphics.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ManualGraphics.Game
{
    public class GameEngine : GameWindow
    {
        Random rnd = new Random();
        public bool IsRunning { get; private set;}
        public GameEngine(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings) : 
            base(gameWindowSettings, nativeWindowSettings)
            {

            }
        public bool Initialise()
        {
            IsRunning = false;
            if (InitialiseOpenGL())
            {
                Console.WriteLine("Init Game Engine");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to Init Game Engine");
                return false;
            }
        }

        public bool InitialiseOpenGL()
        {
            GLFWBindingsContext binding = new GLFWBindingsContext();
            GL.LoadBindings(binding);

            if (GLFW.Init())
            {
                Console.WriteLine("Initialised GLFW and OpenGL");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to init GLFW and OpenGL");
                return false;
            }
        }
        public void RunGameLoop()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Console.WriteLine("Starting Game loop");
                base.Run();
            }
        }
        float[] vertices_cube =
        {
            // Front face - 2 triangles
            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,

            // Top Face
            -1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f, -1.0f,
            
            // Right Face
             1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            
            // Bottom Face
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
            
            // Back Face
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,
            
            // Left Face
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
        };
        float[] vertices_pyramid =
        {
            // Front face - 2 triangles
            0f,     1.0f,  0f,
            -1.0f, -1.0f, -1.0f,
            1.0f,  -1.0f, -1.0f,
            
            // Right Face
            0f,   1.0f,   0f,
            1.0f, -1.0f,  1.0f,
            1.0f, -1.0f, -1.0f,
            
            // Bottom Face
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, 1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, 1.0f,
            
            // Back Face
             0f,  1.0f, 0f,
             -1.0f, -1.0f, 1.0f,
            
            // Left Face
             0f,  1.0f, 0f,
            -1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, -1.0f,
        };


        public Camera cam;
        public Player player;
        
        public GameObject[] objects_cube = {
            new GameObject(), 
            new GameObject(), 
            new GameObject(),
            new GameObject(), 
            new GameObject(), 
            new GameObject(), 
            new GameObject() 
        };
        public GameObject[] objects_cube_width = {
            new GameObject(), 
            new GameObject(), 
            new GameObject(),
            new GameObject(), 
            new GameObject(), 
            new GameObject(), 
            new GameObject() 
        };
        public GameObject[] objects_pyramid = {
             
        };

        public void GetWidth(int pass, GameObject[] source)
        {
            string vertexShader =
                "#version 330\n" +
                "uniform mat4 mvp;\n" +
                "in vec3 in_pos;\n" +
                "void main() { gl_Position = mvp * vec4(in_pos, 1.0); }";

            string fragmentShader = 
                "#version 330\n" +
                "void main() { gl_FragColor = vec4(0.2, 0.2, 0.2, 1.0); }\n";

            for(int i = 0; i < source.Count(); i++)
            {
                var tri2 = source[i];

                tri2 = new GameObject(vertexShader, fragmentShader, vertices_cube);
                Console.WriteLine(pass*-2.0f);
                tri2.Position = new Vector3(-2.0f*pass, Convert.ToSingle(rnd.NextDouble()*1.0), -2.0f*(i+1));
                source[i] = tri2; 
            }
            return;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            string vertexShader =
                "#version 330\n" +
                "uniform mat4 mvp;\n" +
                "in vec3 in_pos;\n" +
                "void main() { gl_Position = mvp * vec4(in_pos, 1.0); }";

            string fragmentShader = 
                "#version 330\n" +
                "void main() { gl_FragColor = vec4(0.2, 0.2, 0.2, 1.0); }\n";

            for(int i = 0; i < objects_cube.Count(); i++)
            {
                var tri = objects_cube[i];

                tri = new GameObject(vertexShader, fragmentShader, vertices_cube);
                tri.Position = new Vector3(0, Convert.ToSingle(rnd.NextDouble()*1.0), -2.0f*(i+1));
                objects_cube[i] = tri;
            }
            int pass = 1;
            var source = objects_cube_width;
            for(int i = 0; i < source.Count(); i++)
            {
                GetWidth(pass, source);
                Console.WriteLine("gotwidth {0} {1}", pass, source);
                if (i == source.Count() - 1)
                {
                    Console.WriteLine("if: true");
                    pass++;
                    i = 1;
                    var target = (GameObject[])source.Clone();
                    source = null;
                    source = target;
                    for(int x = 0; x < source.Count(); x++){
                        Console.WriteLine(x);
                    }
                    
                    target = null;

                    if (pass == 5)
                    {
                        Console.WriteLine("Pass if: true");
                        i = source.Count();
                    }
                }else{
                    Console.WriteLine("If: false");
                }
            }
            for(int x = 0; x < objects_pyramid.Count(); x++)
            {
                var tri = objects_pyramid[x];
                tri = new GameObject(vertexShader, fragmentShader, vertices_pyramid);
                tri.Position = new Vector3(0, 2, -3.0f*(x+1));
                objects_pyramid[x] = tri;
            }
            cam = new Camera();
            player = new Player();

            base.CursorGrabbed = true;

            Size = new OpenTK.Mathematics.Vector2i(1280, 720);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        float ticksElapsed;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            ticksElapsed++;

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }

            GameTime.Delta = float.Parse(args.Time.ToString());
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);

            cam.WorldView = player.WorldToCamera();
            cam.SetSize(Size.X, Size.Y, 0.01f, 100.0f, 65.0f);
            cam.UseViewport();

            player.Update(MouseState, KeyboardState);
            for(int i = 0; i < objects_cube.Count(); i++)
            {
                objects_cube[i].Update(MouseState, KeyboardState);
                objects_cube[i].Draw(cam);
            }
            for(int x = 0; x < objects_cube_width.Count(); x++)
            {
                objects_cube_width[x].Update(MouseState, KeyboardState);
                objects_cube_width[x].Draw(cam);
            }

            Context.SwapBuffers();
        }
    }

}