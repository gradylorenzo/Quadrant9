using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using Q9Core;
using Q9Core.CommonData;
using Q9Core.PlayerData;

namespace Q9Core
{
    public static class SaveManager
    {
        public static void OverwriteProfile()
        {
            
        }

        public static void WriteNewProfile(PlayerProfile profile)
        {
            if (!Directory.Exists("Profiles"))
            {
                Directory.CreateDirectory("Profiles");
            }
            XmlTextWriter xw = new XmlTextWriter("Profiles/" + profile._identity._name + ".xml", Encoding.UTF8);
            xw.Formatting = Formatting.Indented;
            XmlSerializer xs = new XmlSerializer(typeof(PlayerProfile));
            xs.Serialize(xw, profile);
            xw.Close();
        }

        public static void ReadProfile(string id)
        {

        }

        public static string[] GetProfiles()
        {
            List<string> FoundProfiles = new List<string>();

            if (!Directory.Exists("Profiles"))
            {
                Directory.CreateDirectory("Profiles");
                return new string[0];
            }

            else
            {
                string[] profileArray;
                profileArray = Directory.GetFiles("Profiles");

                foreach(string s in profileArray)
                {
                    XmlDocument xDoc = new XmlDocument();
                    if(xDoc.SelectSingleNode("NAME").Value != null)
                    {
                        FoundProfiles.Add(s);
                    }
                }

                return FoundProfiles.ToArray();
            }

            foreach(string s in FoundProfiles)
            {
                Debug.Log("Found " + s);
            }
        }
    }
}
