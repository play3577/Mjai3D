using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mjai3D
{
    class Scene3D
    {
        public Camera3D Camera { get; set; }
        public List<Object3D> Children { get; set; }

        GraphicsDevice device;
        BasicEffect effect;

        public Scene3D(GraphicsDevice device)
        {
            Children = new List<Object3D>();
            this.device = device;
            this.effect = new BasicEffect(device);
        }

        public void Draw()
        {
            Matrix view = Matrix.CreateLookAt(Camera.Position, Camera.Target, Camera.UpVector);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(Camera.FieldOfView, Camera.AspectRatio, Camera.NearPlaneDistance, Camera.FarPlaneDistance);

            foreach (Object3D child in Children)
            {
                Matrix rot = Matrix.CreateRotationX(child.Rotation.X) * Matrix.CreateRotationY(child.Rotation.Y) * Matrix.CreateRotationZ(child.Rotation.Z);
                effect.View = rot * Matrix.CreateTranslation(child.Position) * view;
                effect.Projection = projection;
                child.Draw(device, effect);
            }
        }

        public Ray CreateRay(Point screenPoint)
        {
            Matrix view = Matrix.CreateLookAt(Camera.Position, Camera.Target, Camera.UpVector);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(Camera.FieldOfView, Camera.AspectRatio, Camera.NearPlaneDistance, Camera.FarPlaneDistance);

            Vector3 nearPoint = device.Viewport.Unproject(new Vector3(screenPoint.X, screenPoint.Y, 0f), projection, view, Matrix.Identity);
            Vector3 farPoint = device.Viewport.Unproject(new Vector3(screenPoint.X, screenPoint.Y, 1f), projection, view, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }
    }
}
