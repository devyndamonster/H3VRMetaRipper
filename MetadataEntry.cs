using FistVR;
using MagazinePatcher;
using OtherLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaRipper
{
    public class MetadataEntry
    {

        public string ObjectID;
        public string Category;
        public string Era;
        public string Set;
        public string CountryOfOrigin;
        public int FirstYear;
        public string FirearmSize;
        public string FirearmAction;
        public string FirearmRoundPower;
        public string AttachmentFeature;
        public string AttachmentMount;
        public string MeleeHandedness;
        public string MeleeStyle;
        public string PowerupType;
        public string ThrownDamageType;
        public string ThrownType;

        public List<string> FirearmFeedOptions = new List<string>();
        public List<string> FirearmFiringModes = new List<string>();
        public List<string> FirearmMounts = new List<string>();
        public List<string> ModTags = new List<string>();

        public int MagazineType;
        public int ClipType;
        public int RoundType;
        public int Capacity;
        public int MinCapacityRelated;
        public int MaxCapacityRelated;

        public bool DoesUseSpeedLoader;
        public bool UsesRoundType;
        public bool IsModContent;
        public bool SpawnInTable;

        public bool HasSpawnerEntry;
        public string EntryPath;
        public bool IsDisplayedInMainEntry;


        public MetadataEntry() { }

        public MetadataEntry(FVRObject fvrObject)
        {
            ObjectID = fvrObject.ItemID;
            Category = fvrObject.Category.ToString();
            Era = fvrObject.TagEra.ToString();
            Set = fvrObject.TagSet.ToString();
            CountryOfOrigin = fvrObject.TagFirearmCountryOfOrigin.ToString();
            FirstYear = fvrObject.TagFirearmFirstYear;
            FirearmSize = fvrObject.TagFirearmSize.ToString();
            FirearmAction = fvrObject.TagFirearmAction.ToString();
            FirearmRoundPower = fvrObject.TagFirearmRoundPower.ToString();
            AttachmentFeature = fvrObject.TagAttachmentFeature.ToString();
            AttachmentMount = fvrObject.TagAttachmentMount.ToString();
            MeleeHandedness = fvrObject.TagMeleeHandedness.ToString();
            MeleeStyle = fvrObject.TagMeleeStyle.ToString();
            PowerupType = fvrObject.TagPowerupType.ToString();
            ThrownDamageType = fvrObject.TagThrownDamageType.ToString();
            ThrownType = fvrObject.TagThrownType.ToString();

            FirearmFeedOptions = fvrObject.TagFirearmFeedOption.Select(x => x.ToString()).ToList();
            FirearmFiringModes = fvrObject.TagFirearmFiringModes.Select(x => x.ToString()).ToList();
            FirearmMounts = fvrObject.TagFirearmMounts.Select(x => x.ToString()).ToList();

            MagazineType = (int)fvrObject.MagazineType;
            ClipType = (int)fvrObject.ClipType;
            RoundType = (int)fvrObject.RoundType;
            Capacity = fvrObject.MagazineCapacity;
            MinCapacityRelated = fvrObject.MinCapacityRelated;
            MaxCapacityRelated = fvrObject.MaxCapacityRelated;

            UsesRoundType = fvrObject.UsesRoundTypeFlag;
            IsModContent = fvrObject.IsModContent;
            SpawnInTable = fvrObject.OSple;

            if(fvrObject.Category == FVRObject.ObjectCategory.Firearm)
            {
                MagazineCacheEntry entry;
                if(CompatibleMagazineCache.Instance.Entries.TryGetValue(fvrObject.ItemID, out entry))
                {
                    DoesUseSpeedLoader = entry.DoesUseSpeedloader;
                }
            }


            ItemSpawnerEntry spawnerEntry;
            if(OtherLoader.OtherLoader.SpawnerEntriesByID.TryGetValue(fvrObject.ItemID, out spawnerEntry))
            {
                HasSpawnerEntry = true;
                EntryPath = spawnerEntry.EntryPath;
                IsDisplayedInMainEntry = spawnerEntry.IsDisplayedInMainEntry;
            }
        }

    }
}
