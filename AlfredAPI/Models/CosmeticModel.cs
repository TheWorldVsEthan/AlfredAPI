using AlfredAPI.AlfredAPI;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse_Conversion.Textures;
using Serilog;
using SkiaSharp;
using System.Diagnostics;

namespace AlfredAPI.Models
{
    public class CosmeticModel : ICosmetic
    {
        public CosmeticModel(UObject package)
        {
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

                // Locate LargeIcon And Icon, If Found Get Texture And Save Texture To PC
                if (dataList.FirstOrDefault(d => d.NonConstStruct?.TryGetValue(out FSoftObjectPath p, "LargeIcon") == true && !p.AssetPathName.IsNone) is { NonConstStruct: not null } isl)
                {
                    var largeIconPath = isl.NonConstStruct.Get<FSoftObjectPath>("LargeIcon").AssetPathName.ToString();
                    imagePath = largeIconPath;

                    if (Global.Provider.TryLoadObject(largeIconPath, out UTexture2D a))
                    {
                        try
                        {
                            if (a != null)
                            {
                                a?.SaveToDisk("C:\\Users\\ethan\\Desktop\\New folder"); // "C:\\Users\\ethan\\Desktop\\New folder" Is Just For Testing
                            }                                                           // Will Make API Create "Exports" Folder
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to get map image: {ex.Message}");
                        }

                    }

                    else
                    {
                        Console.WriteLine("ERROR");
                    }
                }

                else if (dataList.FirstOrDefault(d => d.NonConstStruct?.TryGetValue(out FSoftObjectPath p, "Icon") == true && !p.AssetPathName.IsNone) is { NonConstStruct: not null } isi)
                {
                    var iconPath = isi.NonConstStruct.Get<FSoftObjectPath>("Icon").AssetPathName.ToString();
                    imagePath = iconPath;

                    if (Global.Provider.TryLoadObject(iconPath, out UTexture2D a))
                    {
                        try
                        {
                            if (a != null)
                            {
                                a?.SaveToDisk("C:\\Users\\ethan\\Desktop\\New folder"); // "C:\\Users\\ethan\\Desktop\\New folder" Is Just For Testing
                            }                                                           // Will Make API Create "Exports" Folder
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to get map image: {ex.Message}");
                        }

                    }

                    else
                    {
                        Console.WriteLine("ERROR");
                    }
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


    // https://github.com/TheNaeem/Solitude/blob/fa601ff754f6e5fca547dea73fc8c7f3d4fa0ee6/Solitude/Extensions/TextureExtensions.cs
    public static class Extensions
    {
        public static void SaveToDisk(this UTexture2D texture, string outputDir)
        {
            try
            {
                var outputPath = Path.Join(outputDir, texture.Name + ".png");

                var sw = Stopwatch.StartNew();

                using var decoded = texture.Decode();
                using var encoded = decoded?.Encode(SKEncodedImageFormat.Png, 100);
                using var fs = File.Create(outputPath);
                encoded?.AsStream().CopyTo(fs);

                sw.Stop();

                Log.Information("Successfully Exported {Texture} in {Milliseconds} ms", texture.Name, sw.ElapsedMilliseconds);
            }

            catch (Exception e)
            {
                Log.Error("Couldn't Export {Texture}", texture.Name);
                Log.Error(e, string.Empty);
            }
        }
    }
}