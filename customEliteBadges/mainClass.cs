using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Harmony;
using BWModLoader;
using UnityEngine;

namespace customEliteBadges
{

    internal static class Log
    {
        static readonly public ModLogger logger = new ModLogger("[customEliteBadges]", ModLoader.LogPath + "\\customEliteBadges.txt");
    }

    [Mod]
    public class mainClass : MonoBehaviour
    {

        static Dictionary<int, Texture2D> customBadges = new Dictionary<int, Texture2D>();
        static List<int> levels = new List<int>();

        static string texturesFilePath = "/Managed/Mods/Assets/CustomEliteBadges/";

        static void log(string contents)
        {
            Log.logger.Log(contents);
        }

        void Start()
        {
            //Setup harmony patching
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.archie");
            harmony.PatchAll();
            initMod();
        }
        void setMainmenu()
        {
            MainMenu mm = FindObjectOfType<MainMenu>();

            try
            {
                int.TryParse(mm.mainMenuPres.ToString(), out int prestige);
                log(prestige.ToString());
                if (prestige >= 10)
                {
                    int.TryParse(mm.mainMenuLevel.ToString(), out int level);
                    log(level.ToString());
                    int index = ~levels.BinarySearch(level);
                    if (customBadges.TryGetValue(levels[index - 1], out Texture2D newBadgeTexture))
                    {
                        if (newBadgeTexture.name != "NOTFOUND")
                        {
                            mm.menuBadge.texture = newBadgeTexture;
                        }
                    }
                    return;

                }
            }
            catch (Exception e)
            {
                log("Failed to assign custom badge to a player in main menu");
                log(e.Message);
            }
        }
        void initMod()
        {
            if (File.Exists(Application.dataPath + "/Managed/Mods/customBadges.txt"))
            {
                string[] text = File.ReadAllLines(Application.dataPath + "/Managed/Mods/customBadges.txt");
                Texture2D newTexture;
                for (int i = 0; i < text.Length; i++)
                {
                    string[] lineContents = text[i].Split('=');
                    try
                    {
                        newTexture = loadTexture(lineContents[1], 100, 40);
                        customBadges.Add(Int32.Parse(lineContents[0]), newTexture);
                        levels.Add(Int32.Parse(lineContents[0]));
                    }
                    catch (Exception e)
                    {
                        log(e.Message);
                    }
                }
                levels.Sort();
            }
            else
            {
                createConfig();
            }
            setMainmenu();
        }

        void createConfig()
        {
            if (!File.Exists(Application.dataPath + texturesFilePath))
            {
                Directory.CreateDirectory(Application.dataPath + texturesFilePath);
            }
            if (!File.Exists(Application.dataPath + "/Managed/Mods/customBadges.txt"))
            {
                string[] lines = { "100=silver", "250=eyes", "500=diamond" };
                File.WriteAllLines(Application.dataPath + "/Managed/Mods/customBadges.txt", lines);
            }
            initMod();
        }

        static Texture2D loadTexture(string texName, int imgWidth, int imgHeight)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(Application.dataPath + texturesFilePath + texName + ".png");

                Texture2D tex = new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false);
                tex.LoadImage(fileData);
                return tex;

            }
            catch (Exception e)
            {
                log(string.Format("Error loading texture {0}", texName));
                log(e.Message);
                Texture2D tex = Texture2D.whiteTexture;
                tex.name = "NOTFOUND";
                return tex;
            }
        }

        [HarmonyPatch(typeof(ScoreboardSlot), "ñòæëíîêïæîí", new Type[] { typeof(string), typeof(int), typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(bool), typeof(bool), typeof(bool), typeof(int), typeof(int), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool) })]
        static class ScoreBoardSlotAdjuster
        {
            static void Postfix(ScoreboardSlot __instance, string ìåäòäóëäêèæ, int óîèòèðîðçîì, string ñíçæóñðæéòó, int çïîèïçïñêïæ, int äïóïåòòéðåç, int ìëäòìèçñçìí, int óíïòðíäóïçç, int íîóìóíèíñìå, bool ðèæòðìêóëïð, bool äåîéíèñèììñ, bool æíèòîîìðçóî, int ïîñíñóóåîîñ, int æìíñèéçñîíí, bool òêóçíïåæíîë, bool æåèòðéóçêçó, bool èëçòëæêäêîå, bool ëååííåïäæîè, bool ñîäèñæïîóçó)
            {
                try
                {
                    Texture2D newBadgeTexture;
                    //æìíñèéçñîíí = prestige
                    //ïîñíñóóåîîñ = level
                    if (æìíñèéçñîíí >= 10)
                    {
                        int index = ~levels.BinarySearch(ïîñíñóóåîîñ);
                        if (customBadges.TryGetValue(levels[index - 1], out newBadgeTexture))
                        {
                            if (newBadgeTexture.name != "NOTFOUND")
                            {
                                __instance.éòëèïòëóæèó.texture = newBadgeTexture;
                            }
                        }
                        return;

                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Object reference not set to an instance of an object"))
                    {
                        //Go do one
                    }
                    else
                    {
                        log("Failed to assign custom badge to a player:");
                        log(e.Message);
                    }
                }

            }
        }

    }
}