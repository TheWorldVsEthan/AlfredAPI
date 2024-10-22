using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.UObject;

namespace AlfredAPI.Models
{
    public class CosmeticModel : ICosmetic
    {
        public CosmeticModel(UObject package)
        {
            // THANKS 4sval! https://github.com/4sval/FModel/blob/8e2363d1142babbfaafd60d51445d15ae326c060/FModel/Creator/Bases/FN/BaseIcon.cs

            // DisplayName, Description, And ShortDescription
            if (package.TryGetValue(out FText displayName, "ItemName", "DisplayName", "DefaultHeaderText", "UIDisplayName", "EntryName", "EventCalloutTitle"))
                DisplayName = displayName.Text;

            if (package.TryGetValue(out FText description, "ItemDescription", "Description", "GeneralDescription", "DefaultBodyText", "UIDescription", "UIDisplayDescription", "EntryDescription", "EventCalloutDescription"))
                Description = description.Text;
            else if (package.TryGetValue(out FText[] descriptions, "Description"))
                Description = string.Join('\n', descriptions.Select(x => x.Text));

            if (package.TryGetValue(out FText shortDescription, "ItemShortDescription", "ShortDescription", "UIDisplaySubName"))
                ShortDescription = shortDescription.Text;


            // Rarity And Series
            if (package.TryGetValue(out FPackageIndex series, "Series"))
                Series = series;
            if (package.TryGetValue(out FName rarity, "Rarity"))
                Rarity = rarity.Text.Replace("EFortRarity::", "");

            PreviewRarity = Series != null ? Series.Name : Rarity;

            // ExportType
            ExportType = package.ExportType.ToString();

            // Icons
            // For Some Stupid Ass Reason Epic Decided To Put "LargeIcon" And "Icon" Inside DataList[]
            if (package.TryGetValue(out FInstancedStruct[] dataList, "DataList"))
            {
                if (dataList.FirstOrDefault(d => d.NonConstStruct?.TryGetValue(out FSoftObjectPath p, "LargeIcon") == true && !p.AssetPathName.IsNone) is { NonConstStruct: not null } isl)
                {
                    var largeIconPath = isl.NonConstStruct.Get<FSoftObjectPath>("LargeIcon");
                    imagePath = largeIconPath.ToString();
                }

                else if (dataList.FirstOrDefault(d => d.NonConstStruct?.TryGetValue(out FSoftObjectPath p, "Icon") == true && !p.AssetPathName.IsNone) is { NonConstStruct: not null } isi)
                {
                    var iconPath = isi.NonConstStruct.Get<FSoftObjectPath>("Icon");
                    imagePath = iconPath.ToString();
                }
            }
        }
    }

    public abstract class ICosmetic
    {
        public string DisplayName { get; protected set; }
        public string Description { get; protected set; }
        public string ShortDescription { get; set; }
        
        public string ExportType { get; set; }

        public FPackageIndex Series { get; set; }
        public string PreviewRarity { get; set; }
        public string Rarity { get; set; }
        public string imagePath { get; set; }

        // public SKImage Icon { get; set; }

        // Tags And Other Stuff Will Be Added Later! (Or You Could Add It To A Commit) If You Want It Faster!
    }
}