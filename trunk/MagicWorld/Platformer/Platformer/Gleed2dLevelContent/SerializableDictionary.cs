using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.Gleed2dLevelContent
{
    public class SerializableDictionary : Dictionary<String, CustomProperty>, IXmlSerializable
    {

        public SerializableDictionary()
            : base()
        {

        }

        public SerializableDictionary(SerializableDictionary copyfrom)
            : base(copyfrom)
        {
            string[] keyscopy = new string[Keys.Count];
            Keys.CopyTo(keyscopy, 0);
            foreach (string key in keyscopy)
            {
                this[key] = this[key].clone();
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty) return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                CustomProperty cp = new CustomProperty();
                cp.name = reader.GetAttribute("Name");
                cp.description = reader.GetAttribute("Description");

                string type = reader.GetAttribute("Type");
                if (type == "string") cp.type = typeof(string);
                if (type == "bool") cp.type = typeof(bool);
                if (type == "Vector2") cp.type = typeof(Vector2);
                if (type == "Color") cp.type = typeof(Color);
                if (type == "Item") cp.type = typeof(Item);

                if (cp.type == typeof(Item))
                {
                    cp.value = reader.ReadInnerXml();
                    this.Add(cp.name, cp);
                }
                else
                {
                    reader.ReadStartElement("Property");
                    XmlSerializer valueSerializer = new XmlSerializer(cp.type);
                    object obj = valueSerializer.Deserialize(reader);
                    cp.value = Convert.ChangeType(obj, cp.type);
                    this.Add(cp.name, cp);
                    reader.ReadEndElement();
                }

                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (String key in this.Keys)
            {
                writer.WriteStartElement("Property");
                writer.WriteAttributeString("Name", this[key].name);
                if (this[key].type == typeof(string)) writer.WriteAttributeString("Type", "string");
                if (this[key].type == typeof(bool)) writer.WriteAttributeString("Type", "bool");
                if (this[key].type == typeof(Vector2)) writer.WriteAttributeString("Type", "Vector2");
                if (this[key].type == typeof(Color)) writer.WriteAttributeString("Type", "Color");
                if (this[key].type == typeof(Item)) writer.WriteAttributeString("Type", "Item");
                writer.WriteAttributeString("Description", this[key].description);

                if (this[key].type == typeof(Item))
                {
                    Item item = (Item)this[key].value;
                    if (item != null) writer.WriteString(item.Name);
                    else writer.WriteString("$null$");
                }
                else
                {
                    XmlSerializer valueSerializer = new XmlSerializer(this[key].type);
                    valueSerializer.Serialize(writer, this[key].value);
                }
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Must be called after all Items have been deserialized. 
        /// Restores the Item references in CustomProperties of type Item.
        /// </summary>
        public void RestoreItemAssociations(Gleed2dLevelLoader level)
        {
            foreach (CustomProperty cp in Values)
            {
                if (cp.type == typeof(Item)) cp.value = level.getItemByName((string)cp.value);
            }
        }


    }
}