using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;

namespace Q9Core
{
    public static class Settings
    {
        #region
        //Used to tag audio sources to indicate which setting they should obey.
        [System.Serializable]
        public enum volumeTypes
        {
            sfx,
            world,
            ambience,
            music,
            ui,
            voice,
        }

        public class Volume
        {
            public float sfx = 0.5f;
            public float world = 0.5f;
            public float ambience = 0.5f;
            public float music = 0.5f;
            public float ui = 0.5f;
            public float voice = 0.5f;

            public Volume(float s, float w, float a, float m, float u, float v)
            {
                sfx = s;
                world = w;
                ambience = a;
                music = m;
                ui = u;
                voice = v;
            }

            public Volume()
            {
                sfx = 0.5f;
                world = 0.5f;
                ambience = 0.5f;
                music = 0.5f;
                ui = 0.5f;
                voice = 0.5f;
            }
        }

        public static Volume currentVolumes;

        public static bool settingsOK = false;

        public static string SettingsXML;
        #endregion

        //Read the Settings.xml file.
        public static void ReadSettings()
        {
            if (File.Exists("Settings.xml"))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("Settings.xml");

                string s, w, a, m, u, v;
                s = xDoc.SelectSingleNode("Settings/Volume/sfx").InnerText;
                w = xDoc.SelectSingleNode("Settings/Volume/world").InnerText;
                a = xDoc.SelectSingleNode("Settings/Volume/ambience").InnerText;
                m = xDoc.SelectSingleNode("Settings/Volume/music").InnerText;
                u = xDoc.SelectSingleNode("Settings/Volume/ui").InnerText;
                v = xDoc.SelectSingleNode("Settings/Volume/voice").InnerText;

                Volume loadVolumes = new Volume();

                //Try loading the XML file
                try
                {
                    loadVolumes.sfx = Convert.ToSingle(s);
                    loadVolumes.world = Convert.ToSingle(w);
                    loadVolumes.ambience = Convert.ToSingle(a);
                    loadVolumes.music = Convert.ToSingle(m);
                    loadVolumes.ui = Convert.ToSingle(u);
                    loadVolumes.voice = Convert.ToSingle(v);
                }
                catch
                {
                    //If the converter fails on any node, rewrite the whole settings file and retry
                    WriteDefaultSettings();
                    return;
                }
                finally
                {
                    //If successful, commit the loaded variables into the global variables
                    Volume = loadVolumes;
                }
            }
            else
            {
                WriteDefaultSettings();
            }
        }

        public void WriteSettings(float s, float w, float a, float m, float u, float v)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Settings.xml");

            xDoc.SelectSingleNode("Settings/Volume/sfx").InnerText = s.ToString();
            xDoc.SelectSingleNode("Settings/Volume/world").InnerText = w.ToString();
            xDoc.SelectSingleNode("Settings/Volume/ambience").InnerText = a.ToString();
            xDoc.SelectSingleNode("Settings/Volume/music").InnerText = m.ToString();
            xDoc.SelectSingleNode("Settings/Volume/ui").InnerText = u.ToString();
            xDoc.SelectSingleNode("Settings/Volume/voice").InnerText = v.ToString();

            xDoc.Save(SettingsXML);
            ReadSettings();
        }

        public void WriteDefaultSettings()
        {
            //Write default settings
            XmlTextWriter xWriter = new XmlTextWriter("Settings.xml", Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.WriteStartElement("Settings");

            xWriter.WriteStartElement("Volume");
            xWriter.WriteElementString("sfx", "0.5");
            xWriter.WriteElementString("world", "0.5");
            xWriter.WriteElementString("ambience", "0.5");
            xWriter.WriteElementString("music", "0.5");
            xWriter.WriteElementString("ui", "0.5");
            xWriter.WriteElementString("voice", "0.5");
            xWriter.WriteEndElement();//Volume
            xWriter.WriteEndElement();//Settings

            xWriter.Close();
        }
    }
}