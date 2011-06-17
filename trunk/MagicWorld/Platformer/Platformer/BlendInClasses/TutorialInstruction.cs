using System;
using Microsoft.Xna.Framework;

namespace MagicWorld.BlendInClasses
{
    class TutorialInstruction
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
        Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
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
        
        public TutorialInstruction(String text)
        {
            //TODO: make this dynamic
            position = new Vector2(100, 100);
            displayTime = TimeSpan.FromSeconds((text.Length + 10) / 10);
            initialTime = TimeSpan.FromSeconds((text.Length + 10) / 10).TotalMilliseconds;
            color = Color.White;
            this.text = text;
            isActive = false;
        }
    }
}
