using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mjai3D
{
    interface Object3D
    {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }

        BoundingBox BoundingBox { get; }

        void Draw(GraphicsDevice device, BasicEffect effect);
    }
}
