using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.i18N;

namespace AlfredAPI.Models
{
    public class CosmeticModel : ICosmetic
    {
        public CosmeticModel(UObject package)
        {
            // THANKS 4sval! https://github.com/4sval/FModel/blob/8e2363d1142babbfaafd60d51445d15ae326c060/FModel/Creator/Bases/FN/BaseIcon.cs
            // Also, No Way FModel Still Working Because New Data Holds "ItemName", "ItemDescription", ect Instead Of Just "DisplayName", "Description", ect

            if (package.TryGetValue(out FText displayName, "ItemName", "DisplayName", "DefaultHeaderText", "UIDisplayName", "EntryName", "EventCalloutTitle"))
                DisplayName = displayName.Text;

            if (package.TryGetValue(out FText description, "ItemDescription", "Description", "GeneralDescription", "DefaultBodyText", "UIDescription", "UIDisplayDescription", "EntryDescription", "EventCalloutDescription"))
                Description = description.Text;
            else if (package.TryGetValue(out FText[] descriptions, "Description"))
                Description = string.Join('\n', descriptions.Select(x => x.Text));

            if (package.TryGetValue(out FText shortDescription, "ItemShortDescription", "ShortDescription", "UIDisplaySubName"))
                ShortDescription = shortDescription.Text;
        }
    }

    public abstract class ICosmetic
    {
        public string DisplayName { get; protected set; }
        public string Description { get; protected set; }
        public string ShortDescription { get; set; }
    }
}
