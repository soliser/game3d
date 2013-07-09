using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Game3d
{

    public class TargetObject : GraphicObject
    {
        Vector3 start;//store start position
        Vector3 direction;//store direction
        public TargetObject(Model currentModelInput, Vector3 position, Vector3 inputDirection)
        {
            currentModel = currentModelInput;
            active = true;
            Rotation.X = -1.57f;
            Translation = position;
            Scale = .02f;
            start = Translation;
            direction = inputDirection;
        }

        public void update(GameTime gameTime)
        {

            if (active)
            {
                Translation += direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 50;
                
                //make target bounce off bounded space
                if (Translation.X > 60)
                    direction.X = -direction.X;
                else if (Translation.X < -60)
                {
                    direction.X = -direction.X;
                }
                if (Translation.Y > 70)
                    direction.Y = -direction.Y;
                else if (Translation.Y < -10)
                {
                    direction.Y = -direction.Y;
                }
                if (Translation.Z > -150)
                    direction.Z = -direction.Z;
                else if (Translation.Z < -250)
                {
                    direction.Z = -direction.Z;
                }
            }

        }


    }
}