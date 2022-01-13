using BepInEx;
using FistVR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft;
using Valve.Newtonsoft.Json;

namespace MetaRipper {

    [BepInPlugin("devyndamonster.metaripper", "Meta Ripper", "1.0.0")]
    public class MetaRipperPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            // Plugin startup logic
            Logger.LogInfo($"Loaded Meta Ripper");

            StartCoroutine(WaitForMetadataComplete());
        }

        private IEnumerator WaitForMetadataComplete()
        {
            Logger.LogInfo($"Waiting for meta data to be ready");

            while (MagazinePatcher.PatcherStatus.PatcherProgress < 1)
            {
                yield return null;
            }

            Logger.LogInfo($"Metadata is ready");

            RipMetaData();
        }


        private string SetupOutputDirectory()
        {
            string outputFilePath = Application.dataPath.Replace("/h3vr_Data", "/MetaRipper");

            if (!Directory.Exists(outputFilePath))
            {
                Directory.CreateDirectory(outputFilePath);
            }

            return outputFilePath;
        }


        private void RipMetaData()
        {
            Logger.LogInfo($"Ripping metadata");

            string outputFilePath = SetupOutputDirectory();

            CreateMetaCSV(outputFilePath);
            CreateMetaJSON(outputFilePath);
        }


        private void CreateMetaCSV(string path)
        {
            try
            {
                if (File.Exists(path + "/ObjectData.csv"))
                {
                    File.Delete(path + "/ObjectData.csv");
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(path + "/ObjectData.csv"))
                {
                    sw.WriteLine("ObjectID,Category,Era,Set,Country of Origin,Attachment Feature,Firearm Action,Firearm Feed Option,Firing Modes,Firearm Mounts,Attachment Mount,Round Power,Size,Melee Handedness,Melee Style,Powerup Type,Thrown Damage Type,Thrown Type");
                    foreach (FVRObject obj in IM.OD.Values)
                    {
                        sw.WriteLine(
                            obj.ItemID.Replace(",", ".") + "," +
                            obj.Category + "," +
                            obj.TagEra + "," +
                            obj.TagSet + "," +
                            obj.TagFirearmCountryOfOrigin + "," +
                            obj.TagAttachmentFeature + "," +
                            obj.TagFirearmAction + "," +
                            string.Join("+", obj.TagFirearmFeedOption.Select(o => o.ToString()).ToArray()) + "," +
                            string.Join("+", obj.TagFirearmFiringModes.Select(o => o.ToString()).ToArray()) + "," +
                            string.Join("+", obj.TagFirearmMounts.Select(o => o.ToString()).ToArray()) + "," +
                            obj.TagAttachmentMount + "," +
                            obj.TagFirearmRoundPower + "," +
                            obj.TagFirearmSize + "," +
                            obj.TagMeleeHandedness + "," +
                            obj.TagMeleeStyle + "," +
                            obj.TagPowerupType + "," +
                            obj.TagThrownDamageType + "," +
                            obj.TagThrownType);
                    }
                    sw.Close();
                }
            }

            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }


        private void CreateMetaJSON(string path)
        {
            try
            {
                if (File.Exists(path + "/ObjectData.json"))
                {
                    File.Delete(path + "/ObjectData.json");
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(path + "/ObjectData.json"))
                {
                    List<MetadataEntry> entries = new List<MetadataEntry>();

                    foreach(FVRObject fvrObject in IM.OD.Values)
                    {
                        entries.Add(new MetadataEntry(fvrObject));
                    }
    
                    string json = JsonConvert.SerializeObject(entries);

                    sw.WriteLine(json);
                    sw.Close();
                }
            }

            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

    }

}
