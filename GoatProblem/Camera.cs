//using DirectAndVanquish;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GoatProblem
{
    internal class Camera
    {
        public Camera(Viewport aViewport, Rectangle aMovementBounds)
        {
            myViewport = aViewport;
            myMaxZoom = 3;
            myMinZoom = 0.4f;
            myCentre = myViewport.Bounds.Center.ToVector2();
            movementBounds = aMovementBounds;

            transform = Matrix.CreateTranslation(new Vector3(-myCentre.X, -myCentre.Y, 0)) * Matrix.CreateScale(AccessZoom, AccessZoom, 1) * Matrix.CreateTranslation(myViewport.Width / 2, myViewport.Height / 2, 0);
        }

        public Rectangle movementBounds { get; private set; }
        public Matrix transform { get; private set; }
        private float myVelocity = 0.0f;
        private float myXVelocity = 0.0f;
        private float myZoom = 1;
        private Vector2 myCentre;
        private Viewport myViewport;

        public Viewport AccessViewport { get => myViewport; set => myViewport = value; }
        private float myMaxZoom;

        public float AccessMaxZoom
        { set { myMaxZoom = value; AccessZoom = AccessZoom; } get => myMaxZoom; }

        private float myMinZoom;

        public float AccessMinZoom
        { set { myMinZoom = value; AccessZoom = AccessZoom; } get => myMinZoom; }

        public float AccessZoom
        {
            get
            {
                return myZoom;
            }
            set
            {
                myZoom = value;
                if (myZoom > myMaxZoom || myZoom < myMinZoom)
                {
                }
                myZoom = MathHelper.Clamp(myZoom, myMinZoom, myMaxZoom);
                //zoom = zoom < 0.3f ? 0.3f : zoom;
            }
        }

        public void SetMinZoomFromMovementBounds()
        {
            float tempHeightZoom = (float)myViewport.Height / movementBounds.Height;
            float tempWidthZoom = (float)myViewport.Width / movementBounds.Width;
            AccessMinZoom = tempHeightZoom > tempWidthZoom ? tempHeightZoom : tempWidthZoom;
            AccessZoom = AccessMinZoom;
        }

        public void Follow(Vector2 pos, Rectangle hitbox, float deltaTime)
        {
            var position = Matrix.CreateTranslation(new Vector3(-pos.X / 2 - (hitbox.Width / 2), -pos.Y / 2 - (hitbox.Height / 2), 0));

            var offset = Matrix.CreateTranslation(new Vector3(Game1.AccessScreenSize.X / 4, Game1.AccessScreenSize.Y / 4, 0));
            var scale = Matrix.CreateScale(new Vector3(2f, 2f, 0));
            //transform = position * offset;
            Matrix newPos = position * offset * scale;
            float smoothTime = 0.2f;
            float amountToMoveY = AdvancedMath.SmoothDamp(transform.Translation.Y, newPos.Translation.Y, ref myVelocity, smoothTime, float.MaxValue, deltaTime);
            float amountToMoveX = AdvancedMath.SmoothDamp(transform.Translation.X, newPos.Translation.X, ref myXVelocity, smoothTime, float.MaxValue, deltaTime);
            transform = Matrix.CreateTranslation(new Vector3(amountToMoveX, amountToMoveY, 0));
        }

        public void SetCamera(Vector2 pos, Rectangle hitbox)
        {
            var position = Matrix.CreateTranslation(-pos.X - (hitbox.Width / 2), -pos.Y - (hitbox.Height / 2), 0);

            var offset = Matrix.CreateTranslation(Game1.AccessScreenSize.X / 2, Game1.AccessScreenSize.Y / 2, 0);

            transform = position * offset;
            //Matrix newPos = position * offset;
            //float smoothTime = 0.2f;
            //float amountToMoveY = Game1.SmoothDamp(transform.Translation.Y, newPos.Translation.Y, ref yVelocity, smoothTime, float.MaxValue, deltaTime);
            //float amountToMoveX = Game1.SmoothDamp(transform.Translation.X, newPos.Translation.X, ref xVelocity, smoothTime, float.MaxValue, deltaTime);
            //transform = Matrix.CreateTranslation(amountToMoveX, amountToMoveY, 0);
        }

        public void UpdateCamera(Vector2 pos)
        {
            myCentre = pos;
            /*Vector2 cameraSize = new Vector2(myViewport.Width, myViewport.Height) / AccessZoom;
            if (cameraSize.X > movementBounds.Width || cameraSize.Y > movementBounds.Height)
            {
                float tempHeightZoom = (float)myViewport.Height / movementBounds.Height;
                float tempWidthZoom = (float)myViewport.Width / movementBounds.Width;
                AccessMinZoom = tempHeightZoom < tempWidthZoom ? tempHeightZoom : tempWidthZoom;
            }*/
            Vector2 offset = -CameraPos() + ScreenToWorldSpace(Game1.AccessScreenSize);
            offset /= 2;
            myCentre = new Vector2(MathHelper.Clamp(myCentre.X, (movementBounds.X) + offset.X, movementBounds.Width + movementBounds.X - offset.X), MathHelper.Clamp(myCentre.Y, movementBounds.Y + offset.Y, movementBounds.Height + movementBounds.Y - offset.Y));
            //SetInBounds();
            transform = Matrix.CreateTranslation(new Vector3(-myCentre.X, -myCentre.Y, 0)) * Matrix.CreateScale(AccessZoom, AccessZoom, 1) * Matrix.CreateTranslation(myViewport.Width / 2, myViewport.Height / 2, 0);
            //SetInBounds();
        }

        private void SetInBounds()
        {
            /*Vector2 cameraPosition = CameraPos();
            Matrix inverseTransform = Matrix.Invert(transform);
            Vector2 cameraTopLeft = cameraPosition - new Vector2(myViewport.Width / 2f, myViewport.Height / 2f);
            Vector2 cameraBottomRight = cameraPosition + new Vector2(myViewport.Width / 2f, myViewport.Height / 2f);

            //Transform the screenspace coordinates into world space coordinates.
            Vector2 cameraTopLeftWorld = Vector2.Transform(cameraTopLeft, inverseTransform);
            Vector2 cameraBottomRightWorld = Vector2.Transform(cameraBottomRight, inverseTransform);

            //Create the world space boundary rectangle of the camera
            float width = cameraBottomRightWorld.X - cameraTopLeftWorld.X;
            float height = cameraBottomRightWorld.Y - cameraTopLeftWorld.Y;
            Rectangle bounds = new Rectangle((int)cameraTopLeftWorld.X, (int)cameraTopLeftWorld.Y, (int)width, (int)height);

            //Use the bounds rectangle to restrict the camera position.
            if (bounds.X < movementBounds.X) bounds.X = movementBounds.X; //Off the left side
            if (bounds.Y < movementBounds.Y) bounds.Y = movementBounds.Y; //Off the top side
            if (bounds.Right > movementBounds.Right) bounds.X = movementBounds.Right - bounds.Width; //Off the right side
            if (bounds.Bottom > movementBounds.Bottom) bounds.Y = movementBounds.Bottom - bounds.Height; //Off the bottom side

            //Now take the restricted boundaries and transform them back into a position for the camera
            Vector2 boundsCenter = new Vector2(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
            myCentre = Vector2.Transform(boundsCenter, transform);
            //cameraPosition = cameraCenterPosition;*/

            Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(transform));
            Vector2 cameraSize = new Vector2(myViewport.Width, myViewport.Height) / AccessZoom;
            Vector2 limitWorldMin = new Vector2(movementBounds.Left, movementBounds.Top);
            Vector2 limitWorldMax = new Vector2(movementBounds.Right, movementBounds.Bottom);
            Vector2 positionOffset = myCentre - cameraWorldMin;
            if (cameraWorldMin.X < limitWorldMin.X)
            {
            }
            myCentre = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
        }

        public Vector2 ScreenToWorldSpace(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(transform);
            return Vector2.Transform(point, invertedMatrix);
            //        return
            //Matrix.CreateTranslation(new Vector3(centre.X, centre.Y, 0f)) *

            //Matrix.CreateScale(Zoom, Zoom, 0) *
            //Matrix.CreateTranslation(viewport.Width / 2, viewport.Height / 2, 0);
        }

        public Vector2 CameraPos()
        {
            return ScreenToWorldSpace(new Vector2());
        }

        public Vector2 CenterCameraPos()
        {
            return myCentre;
        }

        private Vector2 myLastMousePosition;
        private bool myEnableMouseDragging;

        public void UpdateMouse()
        {
            if ((Input.GetMouseButtonDown(2) || Input.GetButtonDown(Keys.LeftControl) || Input.GetButtonDown(Keys.Space)) && !myEnableMouseDragging)
                myEnableMouseDragging = true;
            else if ((Input.GetMouseButtonUp(2) || Input.GetButtonUp(Keys.LeftControl) || Input.GetButtonUp(Keys.Space)) && myEnableMouseDragging)
                myEnableMouseDragging = false;

            if (myEnableMouseDragging)
            {
                Vector2 delta = myLastMousePosition - Input.MousePos();

                if (delta != Vector2.Zero)
                {
                    UpdateCamera(myCentre + delta / AccessZoom);
                }
            }
            else
            {
                UpdateCamera(myCentre);
            }

            myLastMousePosition = Input.MousePos();
        }
    }
}