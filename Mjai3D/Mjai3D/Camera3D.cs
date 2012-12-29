using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    class Camera3D
    {
        public Microsoft.Xna.Framework.Vector3 Position { get; set; }

        public Microsoft.Xna.Framework.Vector3 Target { get; set; }

        public Microsoft.Xna.Framework.Vector3 UpVector { get; set; }

        public float FieldOfView { get; set; }

        public float AspectRatio { get; set; }

        public float NearPlaneDistance { get; set; }

        public float FarPlaneDistance { get; set; }
    }
}
