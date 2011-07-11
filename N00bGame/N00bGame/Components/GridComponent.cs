
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GridComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region "Private fields"
        private VertexBuffer _vertexBuffer;
        private VertexPositionColor[] _vertexData;
        private BasicEffect _effect;
        private int _primitiveCount = 0;
        private int _vertexCount = 0;
        #endregion

        #region "Public properties"

        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public bool ShowIntermediateLines = true;

        private int _size = 10;
        /// <summary>
        /// Gets/Sets the number of fields of the grid. Default = 10. Should be an even number. 
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }


        private float _scale = 30f;
        /// <summary>
        /// Gets/Sets the scale of the grid. Default = 20
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        private Color _majorColor = Color.White;
        /// <summary>
        /// Gets/Sets the color of the grid lines
        /// </summary>
        public Color MajorColor
        {
            get { return _majorColor; }
            set { _majorColor = value; }
        }

        private Color _minorColor = Color.LightGray;
        /// <summary>
        /// Gets/Sets the color of the intermediate grid lines
        /// </summary>
        public Color MinorColor
        {
            get { return _minorColor; }
            set { _minorColor = value; }
        }

        #endregion
        
        public GridComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _effect = new BasicEffect(this.GraphicsDevice);

            BuildGrid();

            base.LoadContent();
        }

        public void BuildGrid()
        {
            int size = _size;
            float scale = _scale;

            if (ShowIntermediateLines)
            {
                size = size * 10;
                scale = scale * 0.1f;
            }
            //primitiveCount is calculated based on the grid's size.
            //e.g. if size == 4 means there are 5 horizontal and 5 vertical lines i.e. 10 primitives
            _primitiveCount = (size + 1) * 2;

            //As every primitive is a single line, _vertexCount == primitiveCount * 2
            _vertexCount = _primitiveCount * 2;

            float gridLength = size * scale;
            float halfLength = gridLength * 0.5f;

            _vertexData = new VertexPositionColor[_vertexCount];
            float lineIndex = -size * 0.5f;

            for (int i = 0; i < _vertexCount; i++)
            {
                Color color;
                if (lineIndex % 10 != 0 && ShowIntermediateLines)
                    color = _minorColor;
                else
                    color = _majorColor;

                _vertexData[i++] = new VertexPositionColor(new Vector3(-halfLength, 0.0f, lineIndex * scale), color);
                _vertexData[i++] = new VertexPositionColor(new Vector3(halfLength, 0.0f, lineIndex * scale), color);
                _vertexData[i++] = new VertexPositionColor(new Vector3(lineIndex * scale, 0.0f, -halfLength), color);
                _vertexData[i] = new VertexPositionColor(new Vector3(lineIndex * scale, 0.0f, halfLength), color);

                lineIndex++;
            }

            _vertexBuffer = new VertexBuffer(this.GraphicsDevice, typeof(VertexPositionColor), _vertexCount, BufferUsage.None);
            _vertexBuffer.SetData<VertexPositionColor>(_vertexData);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _effect.World = this.World;
            _effect.View = this.View;
            _effect.Projection = this.Projection;
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = true;

            this.GraphicsDevice.SetVertexBuffer(_vertexBuffer, 0);

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _primitiveCount);
            }
            base.Draw(gameTime);
        }
    }
}
