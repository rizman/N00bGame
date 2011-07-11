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


namespace N00bGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Grid : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public Vector3 GridCenter = Vector3.Zero;
        //public int GridLength = 1000;
        public int GridScale = 16;
        public int GridSize = 11;
        public Color GridColor = Color.White;

        private BasicEffect effect;
        private VertexBuffer vertexBuffer;
        //private int numberOfLines = 10;

        public Grid(Game game)
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

        VertexPositionColor[] vertexData;
        int primitiveCount = 5;
        protected override void LoadContent()
        {
            effect = new BasicEffect(this.GraphicsDevice);

            BuildGrid();
            base.LoadContent();
        }

        public void BuildGrid()
        {
            float gridLength = GridScale * GridSize;
            float halfGridLength = gridLength / 2;

            primitiveCount = GridSize * 2;
            int vertexCount = primitiveCount * 2;
            vertexData = new VertexPositionColor[vertexCount];

            //first create the vertices for the lines running along the x-axis
            float minX = GridCenter.X - GridSize / 2 * GridScale;
            float maxX = GridCenter.X + GridSize / 2 * GridScale;
            float currentX = minX;
            for (int i = 0; i < GridSize; i++)
            {
                vertexData[2 * i] = new VertexPositionColor(new Vector3(currentX, 0, halfGridLength), GridColor);
                vertexData[2 * i + 1] = new VertexPositionColor(new Vector3(currentX, 0, -halfGridLength), GridColor);
                currentX += GridScale;
            }

            float minZ = GridCenter.Z - GridSize / 2 * GridScale;
            float maxZ = GridCenter.Z + GridSize / 2 * GridScale;
            float currentZ = minZ;
            for (int i = GridSize; i < primitiveCount; i++)
            {
                vertexData[2 * i] = new VertexPositionColor(new Vector3(halfGridLength + GridCenter.X, 0, currentZ), GridColor);
                vertexData[2 * i + 1] = new VertexPositionColor(new Vector3(-halfGridLength + GridCenter.X, 0, currentZ), GridColor);
                currentZ += GridScale;
            }


            vertexBuffer = new VertexBuffer(this.GraphicsDevice, typeof(VertexPositionColor), vertexCount, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertexData);
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
            effect.World = this.World;
            effect.View = this.View;
            effect.Projection = this.Projection;
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            this.GraphicsDevice.SetVertexBuffer(vertexBuffer, 0);
            
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, primitiveCount);
            }

            base.Draw(gameTime);
        }
    }
}
