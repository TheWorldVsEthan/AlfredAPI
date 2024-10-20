using AlfredAPI.AlfredAPI;
using CUE4Parse.GameTypes.FN.Enums;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.UObject;
using SkiaSharp;
using System.Drawing;

namespace AlfredAPI.Models
{
    public class CosmeticModel : ICosmetic
    {
        public CosmeticModel(UObject package)
        {
            // THANKS 4sval! https://github.com/4sval/FModel/blob/8e2363d1142babbfaafd60d51445d15ae326c060/FModel/Creator/Bases/FN/BaseIcon.cs
            // Also, No Way FModel Still Working Because New Data Holds "ItemName", "ItemDescription", ect Instead Of Just "DisplayName", "Description", ect


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
            else if (package.ExportType.Equals("AthenaItemWrapDefinition"))
                Type = "Wrap";
            if (package.TryGetValue(out FPackageIndex series, "Series"))
                Series = series;
            if (package.TryGetValue(out FName rarity, "Rarity"))
                Rarity = rarity.Text.Replace("EFortRarity::", "");

            PreviewRarity = Series != null ? Series.Name : Rarity;
            

            if (package.TryGetValue(out FSoftObjectPath displayAssetPath, "DisplayAssetPath") &&
                displayAssetPath.TryLoad(out var displayAsset) &&
                displayAsset.TryGetValue(out FStructFallback titleImage, "TitleImage") &&
                titleImage.TryGetValue(out UTexture2D displayImage, "ResourceObject"))
            {
                // Icon = SKImage.FromBitmap(SKBitmap.Decode(displayImage.Decode()?.Encode(SKEncodedImageFormat.Png, 100)));
            }

            if (package.TryGetValue(out FSoftObjectPath previewImagePath, "LargePreviewImage", "SmallPreviewImage") &&
                !previewImagePath.AssetPathName.Text.Contains("/Athena/Prototype/Textures/") &&
                Global.Provider.TryLoadObject(previewImagePath.AssetPathName.Text, out UTexture2D previewImage))
            {
                // Icon = SKImage.FromBitmap(SKBitmap.Decode(previewImage.Decode()?.Encode(SKEncodedImageFormat.Png, 100)));

            }

            if (package.TryGetValue(out FPackageIndex herDef, "HeroDefinition", "WeaponDefinition") &&
                herDef.TryLoad(out var heroDef) &&
                heroDef is not null &&
                heroDef.TryGetValue(out UTexture2D heroDefTexture, "LargePreviewImage"))
            {
                // Icon = SKImage.FromBitmap(SKBitmap.Decode(heroDefTexture.Decode()?.Encode(SKEncodedImageFormat.Png, 100)));

            }
        }
    }

    public abstract class ICosmetic
    {
        public string DisplayName { get; protected set; }
        public string Description { get; protected set; }
        public string ShortDescription { get; set; }
        
        public string Type { get; set; }

        public FPackageIndex Series { get; set; }
        public string PreviewRarity { get; set; }
        public string Rarity { get; set; }
        public string ImagePath { get; set; }

        // public SKImage Icon { get; set; }

        // Tags And Other Still Will Be Added Later! (Or You Could Add It To A Commit) If You Want It Faster!
    }
}
