using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.HelperClasses.Animation
{
    public partial class AnimationLoader
    {
        /// <summary>
        /// Total frames of the spritesheet
        /// </summary>
        [XmlAttribute()]
        public int FrameCount;
        /// <summary>
        /// Width of a single frame.
        /// </summary>
        [XmlAttribute()]
        public int FrameWidth;
        /// <summary>
        /// Height of a single frame.
        /// </summary>
        [XmlAttribute()]
        public int FrameHeight;
        /// <summary>
        /// All the frames on the spritesheet.
        /// </summary>
        public List<Frame> Frames;

        public AnimationLoader()
        {
            Frames = new List<Frame>();
        }
        /// <summary>
        /// Uses a xml file and a spritesheet to load an animation.
        /// </summary>
        /// <param name="filename">This string is the folder in which the spritesheet and xml is, plus the name of the files. The xml and spritesheet must have the same name.</param>
        /// <returns>An AnimationLoader.</returns>
        public static AnimationLoader FromFile(string filename)
        {
            FileStream stream = File.Open(filename + ".xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(AnimationLoader));
            AnimationLoader animationLoader = (AnimationLoader)serializer.Deserialize(stream);
            stream.Close();

            return animationLoader;
        }

        public List<Rectangle> GetFrames()
        {
            List<Rectangle> r = new List<Rectangle>();
            foreach (Frame frame in Frames)
            {
                r.Add(new Rectangle((int)frame.Position.X, (int)frame.Position.Y, FrameWidth, FrameHeight));
            }
            return r;
        }

        public List<Rectangle> GetAnimationFrames(int animationNumber, int animFrames)
        {
            List<Rectangle> r = new List<Rectangle>();

            for (int i = animFrames * animationNumber; i < animFrames * (animationNumber + 1); i++)
            {
                r.Add(new Rectangle((int)Frames[i].Position.X, (int)Frames[i].Position.Y, FrameWidth, FrameHeight));
            }
            return r;
        }
    }

    public partial class Frame
    {
        /// <summary>
        /// Position of the frame on the spritesheet.
        /// </summary>
        public Vector2 Position;

        public Frame() { }
    }
}
