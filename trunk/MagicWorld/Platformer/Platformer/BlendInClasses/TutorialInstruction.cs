using System;
using Microsoft.Xna.Framework;

namespace MagicWorld.BlendInClasses
{
    public class TutorialInstruction
    {
        TutorialManager manager;
        public TutorialManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }
        public String Text
        {
            get { return text; }
        }
        String text;
        TimeSpan displayTime;
        public TimeSpan DisplayTime
        {
            get { return displayTime; }
            set { displayTime = value; }
        }
        float transparency;
        public float Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }
        private double initialTime;
        public double InitialTime
        {
            get { return initialTime; }
            set { initialTime = value; }
        }
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        
        public TutorialInstruction(String text,Vector2 pos)
        {
            position = pos;
            //Calculate how long the instruction is shown according to the length of the text.
            displayTime = TimeSpan.FromSeconds((text.Length + 10) / 10);
            initialTime = TimeSpan.FromSeconds((text.Length + 10) / 10).TotalMilliseconds;
            this.text = text;
            isActive = false;
        }
    }
}
