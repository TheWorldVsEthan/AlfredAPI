﻿using Serilog;
using AlfredAPI.Models;
using AlfredAPI.AlfredAPI;
using CUE4Parse.UE4.Assets.Exports;
using AlfredAPI.Properties;
using SkiaSharp;
using CUE4Parse.GameTypes.FN.Enums;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.Math;

namespace AlfredAPI.Extraction
{
    // Will Log Parsed Data To The Console! 
    public static class CosmeticExtractor
    {
        public static async Task LoadAsset(string? assetPath)
        {
            // If No Asset Path Return Error
            if (assetPath == null)
                Log.Error("Invalid Asset Path!");

            else
            {
                // Try To Load The Object, If False Return Error
                if (!Global.Provider.TryLoadObject(assetPath, out UObject package))
                    Log.Error($"Failed To Locate: {assetPath}");

                else
                {
                    // Pass Our UObject Threw Our Model To Parse JSON Data
                    // STILL WORKING ON MODEL For All Export Types, Therefor May Break!
                    CosmeticModel cos = new CosmeticModel(package);
                    Log.Information($"Parsing: {assetPath}...");
                    Console.WriteLine("ExportType: " + cos.ExportType);
                    Console.WriteLine("DisplayName: " + cos.DisplayName);
                    Console.WriteLine("Description: " + cos.Description);
                    Console.WriteLine("ShortDescription: " + cos.ShortDescription);
                    Console.WriteLine("Rarity: " + cos.PreviewRarity);
                    Console.WriteLine("Icon Path: " + cos.imagePath);

                    // Generate Image
                    var image = GenerateImage(cos);
                    if (image != null)
                    {
                        // Save Icon To Exports Folder
                        await SaveIcon(image, cos.DisplayName);
                    }
                }
            }
        }

        // MEMORY SAFE PRIORITY!!!!!!
        private static async Task SaveIcon(SKImage? icon, string displayName)
        {

            // Save Icon To Exports Folder
            if (icon != null)
            {
                using var image = SKImage.FromBitmap(SKBitmap.FromImage(icon));
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                var path = Path.Combine(Settings.exportsFolder, $"{displayName}.png");

                try
                {
                    await using var stream = File.OpenWrite(path);
                    data.SaveTo(stream);
                    Log.Information($"Saved Icon To: {path}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed To Save Icon To: {path}: {ex.Message}");
                }
            }
        }

        private static SKImage? GenerateImage(CosmeticModel cosmetic)
        {
            // Create Canvas
            var info = new SKImageInfo(1024, 1024);
            using var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;

            using var background = new SKPaint();
            using var card = new SKPaint();

            // TryLoad RarirtData
            if (!Global.Provider.TryLoadObject("fortnitegame/content/balance/raritydata.raritydata",
                    out UObject rarityData)) return null;

            // Rarties
            var idx = EFortRarity.Uncommon;
            switch (cosmetic.Rarity)
            {
                case "EFortRarity::Common":
                case "EFortRarity::Handmade":
                    idx = EFortRarity.Common;
                    break;
                case "EFortRarity::Rare":
                case "EFortRarity::Sturdy":
                    idx = EFortRarity.Rare;
                    break;
                case "EFortRarity::Epic":
                case "EFortRarity::Quality":
                    idx = EFortRarity.Epic;
                    break;
                case "EFortRarity::Legendary":
                case "EFortRarity::Fine":
                    idx = EFortRarity.Legendary;
                    break;
                case "EFortRarity::Mythic":
                case "EFortRarity::Elegant":
                    idx = EFortRarity.Mythic;
                    break;
                case "EFortRarity::Transcendent":
                case "EFortRarity::Masterwork":
                    idx = EFortRarity.Transcendent;
                    break;
                case "EFortRarity::Unattainable":
                case "EFortRarity::Badass":
                    idx = EFortRarity.Unattainable;
                    break;
            }

            // Get And Paint Colors
            if (rarityData.GetByIndex<FStructFallback>((int)idx) is { } data &&
                data.TryGetValue(out FLinearColor color1, "Color1") &&
                data.TryGetValue(out FLinearColor color2, "Color2") &&
                data.TryGetValue(out FLinearColor color3, "Color3"))
            {
                background.Shader = SKShader.CreateRadialGradient(
                    new SKPoint(info.Rect.MidX, info.Rect.MidY),
                    819,
                    new[] { SKColor.Parse(color1.Hex), SKColor.Parse(color2.Hex), SKColor.Parse(color3.Hex) },
                    null,
                    SKShaderTileMode.Clamp
                );

                card.Color = SKColor.Parse(color2.Hex);
                card.StrokeWidth = 15;
                card.Style = SKPaintStyle.Stroke;
            }

            // Draw Background And Icon
            canvas.DrawRect(info.Rect, background);
            canvas.DrawImage(cosmetic.Icon, new SKRect(0, 0, 1024, 1024), new SKPaint());

            // Setup Paint Object For DisplayName And Description
            // var paint = new SKPaint
            // {
                // Color = SKColors.White,
                // TextSize = 60,
                // IsAntialias = true,
                // TextAlign = SKTextAlign.Center,
                // Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            // };

            // Register Fortnite Font
            // string fontFilePath = "C:\\Users\\ethan\\Desktop\\Coding\\Projects\\C#\\AlfredAPI\\AlfredAPI\\Resources\\BurbankBigCondensed-Bold.ttf"; // Will Change To Auto Detect Later On
            // var typeface = SKTypeface.FromFile(fontFilePath);
            // paint.Typeface = typeface;

            // Draw DisplayName 
            // canvas.DrawText(cosmetic.DisplayName, info.Rect.MidX, 900, paint);

            cosmetic.Image = surface.Snapshot();
            return cosmetic.Image;
        }
    }
}