using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses.Animation;

namespace MagicWorld
{
    /// <summary>
    /// Represents an animated texture.
    /// </summary>
    /// <remarks>
    /// Currently, this class assumes that each frame of animation is
    /// as wide as each animation is tall. The number of frames in the
    /// animation are inferred from this.
    /// </remarks>
    public class Animation
    {
        /// <summary>
        /// The spritesheet containing the animation.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        int frameCount = 0;

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
        }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { return Texture.Width / frameCount; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        public float Scale = 1.0f;

        public Color TextureColor = Color.White;

        private List<Rectangle> frames;

        /// <summary>
        /// Gets the frames on the spritesheet that make up the animation.
        /// </summary>
        public List<Rectangle> Frames
        {
            get { return frames; }
            internal set { frames = value; }
        }

        /// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(Texture2D texture, float frameTime, bool isLooping, int frameCount)
        {
            this.frameCount = frameCount;
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }

        /// <summary>
        /// Constructs a new animation out of a multilined spritesheet. Also supports more than one animation.
        /// </summary>
        public Animation(string SpriteSheet, float frameTime, int frameCount, Texture2D texture, int number)
        {
            this.texture = texture;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.isLooping = true;
            Frames = AnimationLoader.FromFile(SpriteSheet).GetAnimationFrames(number);
        }
    }
}
