using System;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using System.Collections.Generic;
using System.Collections.Immutable;
using CommandLine;
using Noggog;

namespace BaboKeywordPatcher
{
    public class BaboSettings
    {
        public bool ArmorPrettyDefault { get; set; }
        public bool ArmorEroticDefault { get; set; }
        public bool EroticDresses { get; set; }
        public HashSet<ModKey> ModsToPatch { get; set; } = new HashSet<ModKey>();
        public HashSet<ModKey> ModsToNotPatch { get; set; } = new HashSet<ModKey>();
        public List<string> IMPORTANTCONSTANTS { get; set; } = new List<string>();
    }

    public class Program
    {
        public static Lazy<BaboSettings> _settings = null!;
        public static BaboSettings Settings => _settings.Value;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "BaboKeywords.esp")
                .SetAutogeneratedSettings(
                    nickname: "Settings",
                    path: "settings.json",
                    out _settings,
                    true)
                .Run(args);
        }

        public static IKeywordGetter LoadKeyword(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, string kwd)
        {
            if (!state.LinkCache.TryResolve<IKeywordGetter>(kwd, out var returnKwd))
            {
                throw new Exception($"Failed to load keyword: {kwd}");
            }
            return returnKwd;
        }

        public static bool StrMatch(string name, string comparator)
        {
            return name.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static void LoadKeywords(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            // Load all keywords here
            SLA_ArmorHarness = LoadKeyword(state, "SLA_ArmorHarness");

            try
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpendex");
            }
            catch
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpandex");
            }

            SLA_ArmorTransparent = LoadKeyword(state, "SLA_ArmorTransparent");
            SLA_BootsHeels = LoadKeyword(state, "SLA_BootsHeels");
            SLA_VaginalDildo = LoadKeyword(state, "SLA_VaginalDildo");
            SLA_AnalPlug = LoadKeyword(state, "SLA_AnalPlug");
            SLA_PiercingClit = LoadKeyword(state, "SLA_PiercingClit");
            SLA_PiercingNipple = LoadKeyword(state, "SLA_PiercingNipple");
            SLA_ArmorPretty = LoadKeyword(state, "SLA_ArmorPretty");
            EroticArmor = LoadKeyword(state, "EroticArmor");
            SLA_ArmorBondage = LoadKeyword(state, "SLA_ArmorBondage");
            SLA_AnalPlugTail = LoadKeyword(state, "SLA_AnalPlugTail");
            SLA_AnalBeads = LoadKeyword(state, "SLA_AnalPlugBeads");
            SLA_VaginalBeads = LoadKeyword(state, "SLA_VaginalBeads");
            SLA_ArmorRubber = LoadKeyword(state, "SLA_ArmorRubber");
            SLA_ThongT = LoadKeyword(state, "SLA_ThongT");
            SLA_PantiesNormal = LoadKeyword(state, "SLA_PantiesNormal");
            SLA_HasLeggings = LoadKeyword(state, "SLA_HasLeggings");
            SLA_HasStockings = LoadKeyword(state, "SLA_HasStockings");
            SLA_MiniSkirt = LoadKeyword(state, "SLA_MiniSkirt");
            SLA_ArmorHalfNakedBikini = LoadKeyword(state, "SLA_ArmorHalfNakedBikini");
            SLA_FullSkirt = LoadKeyword(state, "SLA_FullSkirt");
            SLA_ArmorCurtain = LoadKeyword(state, "SLA_ArmorCurtain");
            SLA_ArmorPartBottom = LoadKeyword(state, "SLA_ArmorPartBottom");
            SLA_HasSleeves = LoadKeyword(state, "SLA_HasSleeves");
            SLA_MicroSkirt = LoadKeyword(state, "SLA_MicroSkirt");
            SLA_Brabikini = LoadKeyword(state, "SLA_Brabikini");
            SLA_ArmorHalfNaked = LoadKeyword(state, "SLA_ArmorHalfNaked");
            SLA_PiercingBelly = LoadKeyword(state, "SLA_PiercingBelly");
            SLA_PiercingVulva = LoadKeyword(state, "SLA_PiercingVulva");
            SLA_ThongGstring = LoadKeyword(state, "SLA_ThongGstring");
            SLA_MicroHotpants = LoadKeyword(state, "SLA_MicroHotpants");
            SLA_PantsNormal = LoadKeyword(state, "SLA_PantsNormal");
            SLA_ArmorIllegal = LoadKeyword(state, "SLA_ArmorIllegal");
            SLA_KillerHeels = LoadKeyword(state, "SLA_KillerHeels");
            SLA_ThongCString = LoadKeyword(state, "SLA_ThongCString");
            SLA_ThongLowleg = LoadKeyword(state, "SLA_ThongLowleg");
            SLA_Earrings = LoadKeyword(state, "SLA_Earrings");
            SLA_PiercingNose = LoadKeyword(state, "SLA_PiercingNose");
            SLA_PiercingLips = LoadKeyword(state, "SLA_PiercingLips");
            SLA_ShowgirlSkirt = LoadKeyword(state, "SLA_ShowgirlSkirt");
            SLA_PelvicCurtain = LoadKeyword(state, "SLA_PelvicCurtain");
            SLA_ArmorPartTop = LoadKeyword(state, "SLA_ArmorPartTop");
            SLA_ArmorLewdLeotard = LoadKeyword(state, "SLA_ArmorLewdLeotard");
            SLA_ImpossibleClothes = LoadKeyword(state, "SLA_ImpossibleClothes");
            SLA_ArmorCapeMini = LoadKeyword(state, "SLA_ArmorCapeMini");
            SLA_ArmorCapeFull = LoadKeyword(state, "SLA_ArmorCapeFull");
            SLA_PastiesNipple = LoadKeyword(state, "SLA_PastiesNipple");
            SLA_PastiesCrotch = LoadKeyword(state, "SLA_PastiesCrotch");
        }

        private static void AddTag(Armor armorEditObj, IKeywordGetter tag)
        {
            armorEditObj.Keywords ??= new ExtendedList<IFormLinkGetter<IKeywordGetter>>();

            if (!armorEditObj.Keywords.Contains(tag))
            {
                armorEditObj.Keywords.Add(tag);
            }
        }

        public static void ParseName(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, IArmorGetter armor, string name)
        {
            bool matched = false;
            var armorEditObj = state.PatchMod.Armors.GetOrAddAsOverride(armor);

            if (armorEditObj == null)
            {
                Console.WriteLine($"Armor is null for {name}");
                return;
            }

            if (StrMatch(name, "harness") || StrMatch(name, "corset") || StrMatch(name, "StraitJacket") ||
                StrMatch(name, "hobble") || StrMatch(name, "tentacles") ||
                StrMatch(name, "slave") || StrMatch(name, "chastity") || StrMatch(name, "cuff") || StrMatch(name, "binder") ||
                StrMatch(name, "yoke") || StrMatch(name, "mitten"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorTransparent);
            }

            if (Settings.ArmorPrettyDefault && !matched && (StrMatch(name, "armor") || StrMatch(name, "cuiras") || StrMatch(name, "robes")))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorPretty);
            }
            else if (Settings.ArmorEroticDefault && !matched && (StrMatch(name, "armor") || StrMatch(name, "cuiras") || StrMatch(name, "robes")))
            {
                matched = true;
                AddTag(armorEditObj, EroticArmor);
            }

            if (matched)
            {
                state.PatchMod.Armors.Set(armorEditObj);
            }
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            HashSet<string> MASTER_MODS = new HashSet<string>()
            {
                "SexLabAroused.esm",
            };

            var modsToPatch = Settings.ModsToPatch;
            var modsToNotPatch = Settings.ModsToNotPatch;

            if (modsToNotPatch.Any())
            {
                Console.WriteLine($"Blacklist:\n{string.Join("\n", modsToNotPatch)}");
            }

            var shortenedLoadOrder = modsToPatch.Any()
                ? state.LoadOrder.PriorityOrder
                    .Where(x => modsToPatch.Contains(x.ModKey))
                : state.LoadOrder.PriorityOrder
                    .Where(x => !modsToNotPatch.Contains(x.ModKey));

            LoadKeywords(state);

            foreach (var mod in shortenedLoadOrder)
            {
                if (mod.Mod == null)
                {
                    continue;
                }

                /* var keywordsToCheck = new HashSet<IFormLinkGetter<IKeywordGetter>>
                {
                    SLA_ArmorHarness,
                    SLA_ArmorSpendex,
                    SLA_ArmorTransparent,
                    SLA_BootsHeels,
                    SLA_VaginalDildo,
                    SLA_AnalPlug,
                    SLA_PiercingClit,
                    SLA_PiercingNipple,
                    SLA_ArmorPretty,
                    EroticArmor,
                    SLA_ArmorBondage,
                    SLA_AnalPlugTail,
                    SLA_AnalBeads,
                    SLA_VaginalBeads,
                    SLA_ArmorRubber,
                    SLA_ThongT,
                    SLA_PantiesNormal,
                    SLA_HasLeggings,
                    SLA_HasStockings,
                    SLA_MiniSkirt,
                    SLA_ArmorHalfNakedBikini,
                    SLA_FullSkirt,
                    SLA_ArmorCurtain,
                    SLA_ArmorPartBottom,
                    SLA_HasSleeves,
                    SLA_MicroSkirt,
                    SLA_Brabikini,
                    SLA_ArmorHalfNaked,
                    SLA_PiercingBelly,
                    SLA_PiercingVulva,
                    SLA_ThongGstring,
                    SLA_MicroHotpants,
                    SLA_PantsNormal,
                    SLA_ArmorIllegal,
                    SLA_KillerHeels,
                    SLA_ThongCString,
                    SLA_ThongLowleg,
                    SLA_Earrings,
                    SLA_PiercingNose,
                    SLA_PiercingLips,
                    SLA_ShowgirlSkirt,
                    SLA_PelvicCurtain,
                    SLA_ArmorPartTop,
                    SLA_ArmorLewdLeotard,
                    SLA_ImpossibleClothes,
                    SLA_ArmorCapeMini,
                    SLA_ArmorCapeFull,
                    SLA_PastiesNipple,
                    SLA_PastiesCrotch
                }; */
				var keywordsToCheck = new HashSet<IFormLinkGetter<IKeywordGetter>>
				{
					SLA_ArmorHarness.ToLink(),
					SLA_ArmorSpendex.ToLink(),
					SLA_ArmorTransparent.ToLink(),
					SLA_BootsHeels.ToLink(),
					SLA_VaginalDildo.ToLink(),
					SLA_AnalPlug.ToLink(),
					SLA_PiercingClit.ToLink(),
					SLA_PiercingNipple.ToLink(),
					SLA_ArmorPretty.ToLink(),
					EroticArmor.ToLink(),
					SLA_ArmorBondage.ToLink(),
					SLA_AnalPlugTail.ToLink(),
					SLA_AnalBeads.ToLink(),
					SLA_VaginalBeads.ToLink(),
					SLA_ArmorRubber.ToLink(),
					SLA_ThongT.ToLink(),
					SLA_PantiesNormal.ToLink(),
					SLA_HasLeggings.ToLink(),
					SLA_HasStockings.ToLink(),
					SLA_MiniSkirt.ToLink(),
					SLA_ArmorHalfNakedBikini.ToLink(),
					SLA_FullSkirt.ToLink(),
					SLA_ArmorCurtain.ToLink(),
					SLA_ArmorPartBottom.ToLink(),
					SLA_HasSleeves.ToLink(),
					SLA_MicroSkirt.ToLink(),
					SLA_Brabikini.ToLink(),
					SLA_ArmorHalfNaked.ToLink(),
					SLA_PiercingBelly.ToLink(),
					SLA_PiercingVulva.ToLink(),
					SLA_ThongGstring.ToLink(),
					SLA_MicroHotpants.ToLink(),
					SLA_PantsNormal.ToLink(),
					SLA_ArmorIllegal.ToLink(),
					SLA_KillerHeels.ToLink(),
					SLA_ThongCString.ToLink(),
					SLA_ThongLowleg.ToLink(),
					SLA_Earrings.ToLink(),
					SLA_PiercingNose.ToLink(),
					SLA_PiercingLips.ToLink(),
					SLA_ShowgirlSkirt.ToLink(),
					SLA_PelvicCurtain.ToLink(),
					SLA_ArmorPartTop.ToLink(),
					SLA_ArmorLewdLeotard.ToLink(),
					SLA_ImpossibleClothes.ToLink(),
					SLA_ArmorCapeMini.ToLink(),
					SLA_ArmorCapeFull.ToLink(),
					SLA_PastiesNipple.ToLink(),
					SLA_PastiesCrotch.ToLink()
				};

                foreach (var armor in mod.Mod.Armors)
                {
                    if (armor == null || armor.EditorID == null)
                    {
                        continue;
                    }

                    if (armor.Keywords?.Any(kw => keywordsToCheck.Contains(kw)) == true)
                    {
                        continue;
                    }

                    var armorName = armor.Name?.ToString() ?? armor.EditorID ?? "UnnamedArmor";
                    ParseName(state, armor, armorName);
                }
            }
        }

        private static IKeywordGetter SLA_ArmorHarness;
        private static IKeywordGetter SLA_ArmorSpendex;
        private static IKeywordGetter SLA_ArmorTransparent;
        private static IKeywordGetter SLA_BootsHeels;
        private static IKeywordGetter SLA_VaginalDildo;
        private static IKeywordGetter SLA_AnalPlug;
        private static IKeywordGetter SLA_PiercingClit;
        private static IKeywordGetter SLA_PiercingNipple;
        private static IKeywordGetter SLA_ArmorPretty;
        private static IKeywordGetter EroticArmor;
        private static IKeywordGetter SLA_ArmorBondage;
        private static IKeywordGetter SLA_AnalPlugTail;
        private static IKeywordGetter SLA_AnalBeads;
        private static IKeywordGetter SLA_VaginalBeads;
        private static IKeywordGetter SLA_ArmorRubber;
        private static IKeywordGetter SLA_ThongT;
        private static IKeywordGetter SLA_PantiesNormal;
        private static IKeywordGetter SLA_HasLeggings;
        private static IKeywordGetter SLA_HasStockings;
        private static IKeywordGetter SLA_MiniSkirt;
        private static IKeywordGetter SLA_ArmorHalfNakedBikini;
        private static IKeywordGetter SLA_FullSkirt;
        private static IKeywordGetter SLA_ArmorCurtain;
        private static IKeywordGetter SLA_ArmorPartBottom;
        private static IKeywordGetter SLA_HasSleeves;
        private static IKeywordGetter SLA_MicroSkirt;
        private static IKeywordGetter SLA_Brabikini;
        private static IKeywordGetter SLA_ArmorHalfNaked;
        private static IKeywordGetter SLA_PiercingBelly;
        private static IKeywordGetter SLA_PiercingVulva;
        private static IKeywordGetter SLA_ThongGstring;
        private static IKeywordGetter SLA_MicroHotpants;
        private static IKeywordGetter SLA_PantsNormal;
        private static IKeywordGetter SLA_ArmorIllegal;
        private static IKeywordGetter SLA_KillerHeels;
        private static IKeywordGetter SLA_ThongCString;
        private static IKeywordGetter SLA_ThongLowleg;
        private static IKeywordGetter SLA_Earrings;
        private static IKeywordGetter SLA_PiercingNose;
        private static IKeywordGetter SLA_PiercingLips;
        private static IKeywordGetter SLA_ShowgirlSkirt;
        private static IKeywordGetter SLA_PelvicCurtain;
        private static IKeywordGetter SLA_ArmorPartTop;
        private static IKeywordGetter SLA_ArmorLewdLeotard;
        private static IKeywordGetter SLA_ImpossibleClothes;
        private static IKeywordGetter SLA_ArmorCapeMini;
        private static IKeywordGetter SLA_ArmorCapeFull;
        private static IKeywordGetter SLA_PastiesNipple;
        private static IKeywordGetter SLA_PastiesCrotch;
    }
}
