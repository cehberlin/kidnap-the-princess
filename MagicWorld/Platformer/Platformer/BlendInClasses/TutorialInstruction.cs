using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace MagicWorld.BlendInClasses
{
    public class TutorialInstruction
    {
        TutorialManager manager;
        int oldWhitespacposition = 0;
        public TutorialManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
        Rectangle position;
        public Rectangle Position
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

        public TutorialInstruction(String text, Rectangle pos)
        {
            position = pos;
            displayTime = TimeSpan.FromSeconds((text.Length + 10) / 10)+TimeSpan.FromSeconds(2);
            initialTime = TimeSpan.FromSeconds((text.Length + 10) / 10).TotalMilliseconds;
            this.text = text;
            SplitText();
            isActive = false;
        }

        private void SplitText()
        {
            int LineLength = 30;
            int whiteSpacePosition = 0;
            string target = " ";
            char[] whitespace = target.ToCharArray();
            string temp;

            for (int i = 0; i < text.Length / LineLength; i++ )
            {
                if (whiteSpacePosition + LineLength > text.Length)
                {
                    LineLength = text.Length-whiteSpacePosition;
                }
                temp = text.Substring(whiteSpacePosition, LineLength);
                whiteSpacePosition = temp.LastIndexOfAny(whitespace);
                text = text.Insert(whiteSpacePosition+oldWhitespacposition + 1, "\n");
                oldWhitespacposition = whiteSpacePosition;
            }
            
        }
    }
}
