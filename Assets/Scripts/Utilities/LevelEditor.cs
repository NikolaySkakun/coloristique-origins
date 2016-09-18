using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public class LevelEditor
{
	static public XmlDocument document;
	static public XmlNode levelNode;

	static public void CreateLevel(int index = 0)
	{
		XmlWriter writer = XmlWriter.Create (GetPathToXml (index));
		writer.WriteStartDocument ();
		writer.WriteStartElement ("Level");
		writer.WriteAttributeString ("index", index.ToString ());
		writer.WriteFullEndElement();
		writer.Close ();
	}

	static public void LoadLevel(int index)
	{
		(document = new XmlDocument ()).Load (GetPathToXml (index));
		levelNode = document.SelectSingleNode ("Level");
	}

	static public void AddAttribute(XmlNode node, string name, string value)
	{
		XmlAttribute attribute = node.OwnerDocument.CreateAttribute (name);
		attribute.Value = value;
		node.Attributes.Append (attribute);
	}

	static public XmlNode AddNode(XmlNode sourceNode, string name)
	{
		XmlNode node = sourceNode.OwnerDocument.CreateNode (XmlNodeType.Element, name, "");
		node.InnerText = "";
		//sourceNode.doc //.OwnerDocument.DocumentElement.AppendChild (node);
		sourceNode.AppendChild(node);
		return node;

//		roomNode = LevelEditor.levelNode.OwnerDocument.CreateNode(XmlNodeType.Element, "room", "");
//		roomNode.InnerText = "";
//		LevelEditor.AddAttribute(roomNode, "class", "Room");
//		LevelEditor.levelNode.OwnerDocument.DocumentElement.AppendChild(roomNode);
	}

	static public string GetPathToXml(int index)
	{
		return Application.dataPath + "/Resources/Levels/Level_" + index.ToString () + ".xml";
	}

	static void Reset()
	{
		document = null;
		levelNode = null;
	}

	static public void Save(int index)
	{
		if (document != null)
		{
			document.Save (GetPathToXml (index));
		}
	}

	static public void ReindexLevel(int previousIndex, int newIndex)
	{
		LoadLevel (previousIndex);

		levelNode.Attributes ["index"].Value = newIndex.ToString();
		document.Save (GetPathToXml (previousIndex));

		System.IO.File.Move (GetPathToXml (previousIndex), GetPathToXml (newIndex));
	}

	static public void DeleteLevel(int index)
	{
		File.Delete (GetPathToXml (index));
	}

}
