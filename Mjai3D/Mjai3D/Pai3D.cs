using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Wistery.Majong;

namespace Mjai3D
{
    class Pai3D : Object3DBase, Object3D
    {

        public const float WIDTH = 14f;
        public const float HEIGHT = 20f;
        public const float DEPTH = 11f;

        static int[,] faces = new int[6,4] {
            {1,3,0,2},
            {3,7,2,6},
            {1,5,3,7},
            {2,6,0,4},
            {0,4,1,5},
            {7,5,6,4}
        };

        static Vector2[] texs = new Vector2[4]{
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(-1,1)
        };

        static Vector3[] vertices = new Vector3[8] {
            new Vector3(0,HEIGHT,0),
            new Vector3(WIDTH,HEIGHT,0),
            new Vector3(0,HEIGHT,DEPTH),
            new Vector3(WIDTH,HEIGHT,DEPTH),
            new Vector3(0,0,0),
            new Vector3(WIDTH,0,0),
            new Vector3(0,0,DEPTH),
            new Vector3(WIDTH,0,DEPTH)
        };

        static Texture2D[] faceTexture;
        static Texture2D[] textures;

        public static void Initialize(ContentManager content)
        {
            faceTexture = new Texture2D[35];
            for (int i = 0; i < 35; i++)
                faceTexture[i] = content.Load<Texture2D>("MJ" + (i+1).ToString().PadLeft(3, '0'));

            textures = new Texture2D[6] {
                content.Load<Texture2D>("top"),
                null,
                content.Load<Texture2D>("side"),
                content.Load<Texture2D>("side2"),
                content.Load<Texture2D>("back"),
                content.Load<Texture2D>("bottom"),
            };
        }

        int x;
        VertexBuffer[] buf;

        public Pai3D(Pai pai)
        {
            this.x = pai.ToInt34();
        }

        public Pai pai
        {
            get
            {
                return Pai.FromInt34(x);
            }
        }

        public BoundingBox BoundingBox
        {
            get
            {
                // TODO: calcutating every time is waste.
                // ...but because this is called not frequently, I leave this.
                Matrix rot = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
                List<Vector3> vertices_ = vertices.Select(v => Vector3.Transform(v, rot) + Position).ToList();
                vertices_.Sort(new Comparison<Vector3>((v1, v2) =>
                {
                    float l1 = v1.LengthSquared();
                    float l2 = v2.LengthSquared();
                    if (l1 > l2) return 1;
                    else if (l1 < l2) return -1;
                    return 0;
                }));

                return new BoundingBox(vertices_.First(), vertices_.Last());
            }
        }

        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            
            if (buf == null)
            {
                buf = new VertexBuffer[6];
                for (int i = 0; i < 6; i++)
                {
                    buf[i] = new VertexBuffer(device, typeof(VertexPositionTexture), 4, BufferUsage.None);
                    var vpt = new VertexPositionTexture[4];
                    for (int j = 0; j < 4; j++)
                    {
                        vpt[j] = new VertexPositionTexture(vertices[faces[i, j]], texs[j]);
                    }
                    buf[i].SetData(vpt);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                device.SetVertexBuffer(buf[i]);
                effect.TextureEnabled = true;
                effect.Texture = (i == 1) ? faceTexture[x] : textures[i];

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                }
            }
        }
    }
}
