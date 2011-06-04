using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Gleed2dLevelContent
{
    public class CustomProperty
    {
        public string name;
        public object value;
        public Type type;
        public string description;

        public CustomProperty()
        {
        }

        public CustomProperty(string n, object v, Type t, string d)
        {
            name = n;
            value = v;
            type = t;
            description = d;
        }

        public CustomProperty clone()
        {
            CustomProperty result = new CustomProperty(name, value, type, description);
            return result;
        }
    }
}
