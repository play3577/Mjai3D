using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mjai3D
{
    class Ribou3D : Object3DBase, Object3D
    {

        public const float WIDTH = 45f;
        public const float HEIGHT = 2f;
        public const float DEPTH = 4f;

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

        static Texture2D texture;

        public static void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("ribou");
        }

        VertexBuffer[] buf;

        public Ribou3D() { }

        public BoundingBox BoundingBox
        {
            // TODO: implementation
            get { return new BoundingBox(); }
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

                if (i == 0)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = texture;
                }
                else
                {
                    effect.TextureEnabled = false;
                }

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                }
            }
        }
    }
}
