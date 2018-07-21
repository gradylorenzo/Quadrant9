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

        public static void WriteNewProfile(string _name)
        {
            if (LibraryManager.isInitialized)
            {
                if (!Directory.Exists("Profiles"))
                {
                    Directory.CreateDirectory("Profiles");
                }

                PlayerProfile profile = new PlayerProfile();
                profile._identity._name = _name;
                profile._identity._credits = 10000;
                profile._allShips = new Q9Ship[1];
                profile._allShips[0] = UnityEngine.Object.Instantiate(LibraryManager.L_SHIPS["EMBRYO"]);
                profile._allShips[0]._guid = Guid.NewGuid().ToString();
                profile._currentShip = profile._allShips[0];
                    

                XmlTextWriter xw = new XmlTextWriter("Profiles/" + _name + ".xml", Encoding.UTF8);
                xw.Formatting = Formatting.Indented;

                xw.WriteStartElement("PROFILE");
                xw.WriteElementString("NAME", profile._identity._name);
                xw.WriteStartElement("SETTINGS");
                    xw.WriteStartElement("VOLUME");
                        xw.WriteElementString("SFX", "0.5");
                        xw.WriteElementString("WORLD", "0.5");
                        xw.WriteElementString("AMBIENCE", "0.5");
                        xw.WriteElementString("MUSIC", "0.5");
                        xw.WriteElementString("UI", "0.5");
                        xw.WriteElementString("VOICE", "0.5");
                    xw.WriteEndElement();
                xw.WriteEndElement();
                xw.WriteElementString("SEED", Guid.NewGuid().ToString("N"));
                xw.WriteElementString("CREDITS", profile._identity._credits.ToString());
                xw.WriteElementString("CURRENTSHIP", profile._currentShip._guid);
                foreach (Q9Ship s in profile._allShips)
                {
                    xw.WriteStartElement("SHIP");
                    xw.WriteAttributeString("guid", s._guid);
                    xw.WriteElementString("HULL", s._id);
                    xw.WriteElementString("NAME", profile._identity._name + "'s " + s._name);
                    xw.WriteStartElement("FITTING");

                    xw.WriteStartElement("HIGH");
                    for (int i = 0; i < 4; i++)
                    {
                        if (s._attributes._fitting._highSlots[i] != null)
                        {
                            xw.WriteElementString(i.ToString(), s._attributes._fitting._highSlots[i]._id);
                        }
                        else
                        {
                            xw.WriteElementString(i.ToString(), "NULL");
                        }
                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("MID");
                    for (int i = 0; i < 4; i++)
                    {
                        if (s._attributes._fitting._midSlots[i] != null)
                        {
                            xw.WriteElementString(i.ToString(), s._attributes._fitting._midSlots[i]._id);
                        }
                        else
                        {
                            xw.WriteElementString(i.ToString(), "NULL");
                        }
                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("LOW");
                    for (int i = 0; i < 4; i++)
                    {
                        if (s._attributes._fitting._lowSlots[i] != null)
                        {
                            xw.WriteElementString(i.ToString(), s._attributes._fitting._lowSlots[i]._id);
                        }
                        else
                        {
                            xw.WriteElementString(i.ToString(), "NULL");
                        }
                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("RIG");
                    for (int i = 0; i < 3; i++)
                    {
                        if (s._attributes._fitting._rigSlots[i] != null)
                        {
                            xw.WriteElementString(i.ToString(), s._attributes._fitting._rigSlots[i]._id);
                        }
                        else
                        {
                            xw.WriteElementString(i.ToString(), "NULL");
                        }
                    }
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }

                

                xw.WriteEndElement();

                xw.Close();
            }
            else
            {
                Debug.LogError("Library not initialized, cannot write new profile");
            }
        }

        public static PlayerProfile ReadProfile(string id)
        {
            if (LibraryManager.isInitialized)
            {
                if (Directory.Exists("Profiles"))
                {
                    if(File.Exists("Profiles/" + id + ".xml"))
                    {
                        string path = "Profiles/" + id + ".xml";
                        PlayerProfile newProfile = new PlayerProfile();
                        XmlDocument reader = new XmlDocument();
                        try
                        {
                            newProfile._identity._name = reader.SelectSingleNode("NAME").InnerText;
                            newProfile._identity._credits = Convert.ToInt32(reader.SelectSingleNode("CREDITS").InnerText);

                        }
                        catch(Exception e)
                        {
                            Debug.LogError(e.Message);
                            return null;
                        }
                        finally
                        {
                            
                        }
                        return newProfile;
                    }
                    else
                    {
                        Debug.LogError("Failed to load profile, " + id + ".xml not found in Profiles/");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError("Failed to load profile, Directory not found");
                    return null;
                }
            }
            else
            {
                Debug.LogError("Failed to load profile, LibraryManager not initialized");
                return null;
            }
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
                    if(xDoc.SelectSingleNode("PROFILE/NAME").Value != null)
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
