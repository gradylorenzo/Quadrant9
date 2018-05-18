using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;

namespace Q9Core
{
    public class SaveIO
    {
        public class saveData
        {
            public string name;
            public int credits;

            public saveData (string n, int c)
            {
                name = n;
                credits = c;
            }
        }

        public saveData CurrentSaveData;

        public Dictionary<string, string> SavesList()
        {
            XmlDocument xDoc = new XmlDocument();

            Dictionary<string, string> foundSaves = new Dictionary<string, string>();
            string[] dirList = new string[0];

            //Make sure the saves directory exists. If it does not, create it.
            if (!Directory.Exists("Saves"))
            {
                Debug.Log("Saves directory not found, creating..");
                Directory.CreateDirectory("Saves");
            }

            //At this point, the directory should exist
            dirList = Directory.GetDirectories("Saves/");

            if (dirList.Length > 0)
            {
                foreach(string d in dirList)
                {
                    Debug.Log("Found " + d);
                    string path = d + "/save.xml";
                    string name;
                    if(File.Exists(path))
                    {
                        xDoc.Load(path);
                        name = xDoc.SelectSingleNode("Save/Name").InnerText;
                        foundSaves.Add(name, path);
                    }
                    else
                    {
                        Debug.Log("No valid save file found in " + d + ", skipping!");
                    }
                }
            }
            else
            {
                Debug.Log("No saves found!");
            }

            if(foundSaves.Count > 0)
            {
                return foundSaves;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        public void CreateNewSave(string name, bool loadAfterCreating)
        {
            if (!Directory.Exists("Saves/" + name))
            {
                Directory.CreateDirectory("Saves/" + name);
                XmlTextWriter xWriter = new XmlTextWriter("Saves/" + name + "/save.xml", Encoding.UTF8);
                xWriter.Formatting = Formatting.Indented;

                xWriter.WriteStartElement("Save");
                xWriter.WriteElementString("Name", name);
                xWriter.WriteElementString("Credits", "10000");
                xWriter.WriteEndElement();//Save
                xWriter.Close();

                if (loadAfterCreating)
                {
                    LoadSave(name);
                }
            }
        }

        public void LoadSave(string name)
        {
            XmlDocument xDoc = new XmlDocument();
            string path = "Saves/" + name;

            string saveName;
            int saveCredits;
            if(Directory.Exists("Saves/" + name))
            {
                List<System.IO.FileInfo> sortedFiles = new DirectoryInfo(path).GetFiles().OrderByDescending(x => x.LastWriteTime).ToList();
                if(sortedFiles.Count > 0)
                {
                    xDoc.Load("Saves/" + name + "/" + sortedFiles[0].Name);

                    saveName = xDoc.SelectSingleNode("Save/Name").InnerText;
                    saveCredits = Convert.ToInt32(xDoc.SelectSingleNode("Save/Credits").InnerText);

                    CurrentSaveData = new saveData(saveName, saveCredits);
                }
            }
            else
            {
                Debug.LogError("u bad hacker, much shame upon your family >:[");
            }
        }

        public void DeleteSave(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
