using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using SkinnedModel;


namespace Game3d
{

    public class GraphicObject : Microsoft.Xna.Framework.Game
    {

        public Model currentModel; //Model reference
        public Vector3 Translation = new Vector3(0, 0, 0); //current position on the screen
        public Vector3 Rotation = new Vector3(0, 0, 0); //Current rotation
        public float Scale = 1.0f; //Current scale
        public Boolean active = true;

        public GraphicObject()
        {
        }
        public GraphicObject(Model currentModelInput)
        {
            currentModel = currentModelInput;


        }

        public void ModelDraw(GraphicsDevice device, Vector3 cameraPosition, Vector3 cameraTarget, float farPlaneDistance)
        {
            Matrix[] transforms = new Matrix[currentModel.Bones.Count];
            currentModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Compute camera matrices.
            Matrix view =  Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
        

            //Calculate the aspect ratio
            float aspectRatio = (float)device.Viewport.Width /
                                        (float)device.Viewport.Height;

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio,
                1.0f, farPlaneDistance);

            // Draw the model. A model can have multiple meshes, so loop.
            
            {
                foreach (ModelMesh mesh in currentModel.Meshes)
                {
                    // This is where the mesh orientation is set, as well as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] *
                            Matrix.CreateRotationX(Rotation.X) *
                            Matrix.CreateRotationY(Rotation.Y) *
                            Matrix.CreateRotationZ(Rotation.Z) *
                            Matrix.CreateScale(Scale) *
                            Matrix.CreateWorld(Translation, Vector3.Forward, Vector3.Up);

                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }
            }

        }


    }
}