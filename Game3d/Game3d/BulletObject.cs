using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Game3d
{

    public class BulletObject : GraphicObject
    {
        Vector3 start;//store verious positions
        Vector3 speed;//direction
        public BulletObject(Model currentModelInput)
        {
            currentModel = currentModelInput;
            active = false;
            Rotation.X = -1.57f;//set initial rotation
            Scale = .01f;
            start = Translation;
        }
        public void fire(Vector3 direction)
        {
            active = true;
            speed = direction;
            Rotation += direction;//rotate model towards the direction shot
        }

        public void update(GameTime gameTime, Vector3 plane)
        {

            if (active)
            {

                Translation += speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
                //make bullet reset when leave feild
                if (Translation.Z < -300)
                {
                    active = false;
                    Rotation = new Vector3(-1.57f, 0, 0);
                }
            }
            else
            {
                Translation = plane;//returns model to planes position if not active
            }
        }


    }
}