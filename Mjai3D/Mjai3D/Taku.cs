using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mjai3D
{
    class Taku : Object3DBase, Object3D
    {
        public const float SIZE = Pai3D.WIDTH * 25;

        static Texture2D texture;

        public static void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("taku");
        }

        VertexBuffer buf;

        public Taku()
        {
            
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox();
            }
        }

        public void Draw(GraphicsDevice device, BasicEffect effect)
        {
            if (buf == null)
            {
                buf = new VertexBuffer(device, typeof(VertexPositionTexture), 4, BufferUsage.None);
                var vpt = new VertexPositionTexture[4]{
                    new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0,0)),
                    new VertexPositionTexture(new Vector3(SIZE, 0, 0), new Vector2(1,0)),
                    new VertexPositionTexture(new Vector3(0, 0, SIZE), new Vector2(0,1)),
                    new VertexPositionTexture(new Vector3(SIZE, 0, SIZE), new Vector2(1,1))
                };

                buf.SetData(vpt);
            }

            device.SetVertexBuffer(buf);

            effect.TextureEnabled = true;
            effect.Texture = texture;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }
    }
}
