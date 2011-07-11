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


namespace N00bGame.Components
{
    public enum CameraType
    {
        FirstPerson,
        ThirdPerson
    }

    public enum Actions
    {
        MoveForward,
        MoveBackward,
        MoveUp,
        MoveDown,
        StrafeLeft,
        StrafeRight,
        TurnLeft,
        TurnRight,
        LookUp,
        LookDown,
        RollLeft,
        RollRight
    }

    /// <summary>
    /// This component implements a camera
    /// </summary>
    public class CameraComponent : Microsoft.Xna.Framework.GameComponent
    {
        private const float DEFAULT_NEARPLANE = 1.0f;
        private const float DEFAULT_FARPLANE = 1000.0f;
        private const float DEFAULT_FOV = 45.0f;
        private const float DEFAULT_CAMERASPEED = 0.2f;

        Matrix cameraMatrix = Matrix.Identity;

        private Quaternion _orientation = Quaternion.Identity;

        private Vector3 _position = Vector3.Zero;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector3 _target = Vector3.Zero;
        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private Vector3 _up = Vector3.Up;
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        private float _speed = DEFAULT_CAMERASPEED;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float RotationSpeed
        {
            get { return _speed / 10; }
        }

        private float _nearPlane = DEFAULT_NEARPLANE;
        private float _farPlane = DEFAULT_FARPLANE;
        private float _fov = DEFAULT_FOV;
        private float _aspectRatio;

        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
        }

        private Matrix _projection;
        public Matrix Projection
        {
            get { return _projection; }
        }

        public CameraComponent(Game game)
            : base(game)
        {
            
        }

        private Matrix ComputeViewMatrix()
        {
            //Vector3 lookAt = Vector3.Transform(Vector3.Forward, cameraMatrix);
            //return Matrix.CreateLookAt(cameraMatrix.Translation, lookAt, Vector3.Up);
            Matrix positionMatrix = Matrix.Identity;
            //_orientation.Normalize();
            Matrix rotationMatrix = Matrix.CreateFromQuaternion(_orientation);
            positionMatrix.Translation = _position;
            positionMatrix = rotationMatrix * positionMatrix;

            Vector3 lookAt = Vector3.Transform(Vector3.Forward, positionMatrix);
            return Matrix.CreateLookAt(_position, lookAt, Vector3.Up);
        }

        private Matrix ComputeProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fov), _aspectRatio, _nearPlane, _farPlane);
        }

        public void Move(Actions Action)
        {
            switch (Action)
            {
                case Actions.MoveForward:
                    {
                        //Move(cameraMatrix.Forward);
                        Move(Vector3.Forward);
                        break;
                    }
                case Actions.MoveBackward:
                    {
                        //Move(cameraMatrix.Backward);
                        Move(Vector3.Backward);
                        break;
                    }
                case Actions.MoveUp:
                    {
                        //Move(cameraMatrix.Up);
                        Move(Vector3.Up);
                        break;
                    }
                case Actions.MoveDown:
                    {
                        //Move(cameraMatrix.Down);
                        Move(Vector3.Down);
                        break;
                    }
                case Actions.StrafeLeft:
                    {
                        //Move(cameraMatrix.Left);
                        Move(Vector3.Left);
                        break;
                    }
                case Actions.StrafeRight:
                    {
                        Move(Vector3.Right);
                        break;
                    }
                case Actions.TurnLeft:
                    {
                        Rotate(Matrix.Identity.Up);
                        break;
                    }
                case Actions.TurnRight:
                    {
                        Rotate(Matrix.Identity.Down);
                        break;
                    }
                case Actions.LookUp:
                    {
                        Rotate(cameraMatrix.Right);
                        break;
                    }
                case Actions.LookDown:
                    {
                        Rotate(cameraMatrix.Left);
                        break;
                    }
                case Actions.RollLeft:
                    {
                        Rotate(0f, 0f, 0.2f);
                        break;
                    }
                case Actions.RollRight:
                    {
                        Rotate(0f, 0f, -0.2f);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Move(Vector3 moveVector)
        {
            //cameraMatrix *= Matrix.CreateTranslation(moveVector * Speed);
            Vector3 transformedMoveVector = Vector3.Transform(moveVector, _orientation);
            Position += (transformedMoveVector * Speed);
        }

        public void Rotate(float yaw, float pitch, float roll)
        {
            
            ////temporarily save the current position as the next statement
            ////will also rotate the translation vector of the cameraMatrix
            //Vector3 position = cameraMatrix.Translation;
            //cameraMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(yaw));
            //cameraMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(pitch));
            
            ////reset the camera position
            //cameraMatrix.Translation = position;

            _orientation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw),
                MathHelper.ToRadians(pitch),
                MathHelper.ToRadians(roll));
        }

        public void Rotate(Actions Action, float Distance)
        {
            switch (Action)
            {
                case Actions.LookUp:
                    {
                        Rotate(cameraMatrix.Right, Distance);
                        break;
                    }
                case Actions.LookDown:
                    {
                        Rotate(cameraMatrix.Left, Distance);
                        break;
                    }
                case Actions.TurnLeft:
                    {
                        Rotate(cameraMatrix.Up, Distance);
                        break;
                    }
                case Actions.TurnRight:
                    {
                        Rotate(cameraMatrix.Down, Distance);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Rotate(Vector3 axis)
        {
            Rotate(axis, RotationSpeed);
        }

        private void Rotate(Vector3 axis, float Distance)
        {
            ////temporarily save the current position as the next statement
            ////will also rotate the translation vector of the cameraMatrix
            //Vector3 position = cameraMatrix.Translation;
            //cameraMatrix.Translation = Vector3.Zero;
            //cameraMatrix *= Matrix.CreateFromAxisAngle(Vector3.Normalize(axis), Distance);

            ////reset the camera position
            //cameraMatrix.Translation = position;
        }

        public override void Initialize()
        {
            _aspectRatio = this.Game.GraphicsDevice.Viewport.AspectRatio;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _view = ComputeViewMatrix();
            _projection = ComputeProjectionMatrix();

            base.Update(gameTime);
        }
    }
}
