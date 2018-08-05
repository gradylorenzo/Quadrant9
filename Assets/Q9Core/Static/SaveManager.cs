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
        #region staticInfo
        private static bool _profileLoaded = false;
        public static bool profileLoaded
        {
            get { return _profileLoaded; }
            set { _profileLoaded = value; }
        }

        private static PlayerProfile _player;
        public static PlayerProfile currentPlayer
        {
            get { return _player; }
        }

        public static void SetPlayerProfile(PlayerProfile pp)
        {
            _player = pp;
            _profileLoaded = true;
        }
        public static void ClearPlayerProfile()
        {
            _player = null;
            _profileLoaded = false;
        }
        #endregion

        #region I/O
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

                foreach (string s in profileArray)
                {
                    try
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(s);
                        string name;
                        name = xDoc.SelectSingleNode("PROFILE/NAME").InnerText;
                        if (name != null && name != "")
                        {
                            FoundProfiles.Add(name);
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                return FoundProfiles.ToArray();
            }

            foreach (string s in FoundProfiles)
            {
                Debug.Log("Found " + s);
            }
        }

        public static void OverwriteProfile(PlayerProfile profile)
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
                Q9Ship newShip = new Q9Ship();

                newShip = LibraryManager.GetShip("TARTARUS");
                newShip._guid = Guid.NewGuid().ToString();
                profile._currentShip = newShip._guid;
                profile._allShips.Add(newShip._guid, newShip);

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
                xw.WriteStartElement("ACTIVE_SYSTEM");
                    xw.WriteElementString("X", 16.ToString());
                    xw.WriteElementString("Y", 16.ToString());
                xw.WriteEndElement();
                xw.WriteElementString("CREDITS", profile._identity._credits.ToString());
                xw.WriteElementString("CURRENTSHIP", profile._currentShip);
                foreach (KeyValuePair<string, Q9Ship> s in profile._allShips)
                {
                    xw.WriteStartElement("SHIP");
                    xw.WriteAttributeString("guid", s.Value._guid);
                    xw.WriteElementString("HULL", s.Value._id);
                    xw.WriteElementString("NAME", profile._identity._name + "'s " + s.Value._name);
                    s.Value._attributes._fitting._highSlots[0] = ScriptableObject.Instantiate(LibraryManager.GetModule("SHIELD_BOOSTER_L1"));
                    s.Value._attributes._fitting._highSlots[1] = ScriptableObject.Instantiate(LibraryManager.GetModule("SHIELD_BOOSTER_L1"));
                    s.Value._attributes._fitting._highSlots[2] = ScriptableObject.Instantiate(LibraryManager.GetModule("TEST_WEAPON_1"));
                    xw.WriteStartElement("FITTING");

                        xw.WriteStartElement("HIGH");
                        for (int i = 0; i < 4; i++)
                        {
                            if (s.Value._attributes._fitting._highSlots[i] != null)
                            {
                                xw.WriteElementString("s" + i.ToString(), s.Value._attributes._fitting._highSlots[i]._id);
                            }
                            else
                            {
                                xw.WriteElementString("s" + i.ToString(), "NULL");
                            }
                        }
                        xw.WriteEndElement();

                        xw.WriteStartElement("MID");
                        for (int i = 0; i < 4; i++)
                        {
                            if (s.Value._attributes._fitting._midSlots[i] != null)
                            {
                                xw.WriteElementString("s" + i.ToString(), s.Value._attributes._fitting._midSlots[i]._id);
                            }
                            else
                            {
                                xw.WriteElementString("s" + i.ToString(), "NULL");
                            }
                        }
                        xw.WriteEndElement();

                        xw.WriteStartElement("LOW");
                        for (int i = 0; i < 4; i++)
                        {
                            if (s.Value._attributes._fitting._lowSlots[i] != null)
                            {
                                xw.WriteElementString("s" + i.ToString(), s.Value._attributes._fitting._lowSlots[i]._id);
                            }
                            else
                            {
                                xw.WriteElementString("s" + i.ToString(), "NULL");
                            }
                        }
                        xw.WriteEndElement();

                        xw.WriteStartElement("RIG");
                        for (int i = 0; i < 3; i++)
                        {
                            if (s.Value._attributes._fitting._rigSlots[i] != null)
                            {
                                xw.WriteElementString("s" + i.ToString(), s.Value._attributes._fitting._rigSlots[i]._id);
                            }
                            else
                            {
                                xw.WriteElementString("s" + i.ToString(), "NULL");
                            }
                        }
                        xw.WriteEndElement();

                    xw.WriteEndElement();

                    xw.WriteStartElement("CARGO");

                        if (s.Value._attributes._cargo._cargo.Count > 0)
                        {
                            foreach (Q9Object o in s.Value._attributes._cargo._cargo)
                            {
                                xw.WriteElementString("OBJECT", o._id);
                            }
                        }

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
        public static bool ReadProfile(string id)
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
                        reader.Load("Profiles/" + id + ".xml");

                        try
                        {
                            newProfile._identity._name = reader.SelectSingleNode("PROFILE/NAME").InnerText;
                            newProfile._identity._credits = Convert.ToInt32(reader.SelectSingleNode("PROFILE/CREDITS").InnerText);
                            newProfile._currentShip = reader.SelectSingleNode("PROFILE/CURRENTSHIP").InnerText;
                            Vector2 pos = new Vector2(
                                Convert.ToSingle(reader.SelectSingleNode("PROFILE/ACTIVE_SYSTEM/X").InnerText),
                                Convert.ToSingle(reader.SelectSingleNode("PROFILE/ACTIVE_SYSTEM/Y").InnerText));
                            newProfile._activeSystem = pos;
                            NavigationManager.SetActiveSystem((int)pos.x, (int)pos.y);

                            XmlNodeList nl = reader.SelectNodes("PROFILE/SHIP");

                            foreach(XmlNode node in nl)
                            {
                                Q9Ship newShip = new Q9Ship();
                                newShip = LibraryManager.GetShip(node.SelectSingleNode("HULL").InnerText);
                                newShip._guid = node.Attributes["guid"].Value;
                                newShip._name = node.SelectSingleNode("NAME").InnerText;

                                Fitting newFitting = new Fitting();
                                newFitting._highSlots = new Q9Module[4];
                                newFitting._midSlots = new Q9Module[4];
                                newFitting._lowSlots = new Q9Module[4];
                                newFitting._rigSlots = new Q9Module[3];

                                //Load High Slots
                                for (int i = 0; i < 4; i++)
                                {
                                    newFitting._highSlots[i] =
                                        LibraryManager.GetModule(node.SelectSingleNode("FITTING/HIGH/s" + i).InnerText);
                                }
                                //Load Mid Slots
                                for (int i = 0; i < 4; i++)
                                {
                                    newFitting._midSlots[i] =
                                        LibraryManager.GetModule(node.SelectSingleNode("FITTING/MID/s" + i).InnerText);
                                }
                                //Load Low Slots
                                for (int i = 0; i < 4; i++)
                                {
                                    newFitting._lowSlots[i] =
                                        LibraryManager.GetModule(node.SelectSingleNode("FITTING/LOW/s" + i).InnerText);
                                }
                                //Load Rig Slots
                                for (int i = 0; i < 3; i++)
                                {
                                    newFitting._rigSlots[i] =
                                        LibraryManager.GetModule(node.SelectSingleNode("FITTING/RIG/s" + i).InnerText);
                                }

                                newShip._attributes._fitting = newFitting;
                                if (!newProfile._allShips.ContainsKey(newShip._guid))
                                {
                                    newProfile._allShips.Add(newShip._guid, newShip);
                                }
                            }
                        }
                        #region exception handling
                        catch(Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                        #endregion
                        finally
                        {
                            
                        }
                        SetPlayerProfile(newProfile);
                        return true;
                    }
                    else
                    {
                        Debug.LogError("Failed to load profile, " + id + ".xml not found in Profiles/");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Failed to load profile, Directory not found");
                    return false;
                }
            }
            else
            {
                Debug.LogError("Failed to load profile, LibraryManager not initialized");
                return false;
            }
        }
        #endregion
    }
}
