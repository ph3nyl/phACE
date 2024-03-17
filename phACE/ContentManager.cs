using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.SQLFormatters.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Command.Handlers.Processors;
using ACE.Server.Network;
using ACE.Shared.Helpers;

namespace phACE
{
    public enum ContentType
    {
        Weenies,
        Landblocks,
        Recipes,
        Spells,
        Quests
    };

    public static class ContentManager
    {
        private static List<uint> wcids = new();
        private static List<ushort> lbids = new();
        private static List<uint> rids = new();
        private static Dictionary<uint, ACE.Server.Entity.Spell> sids = new();
        private static List<string> qfs = new();

        public static List<uint> WCIDs { get => wcids; set => wcids = value; }
        public static List<ushort> LBIDs { get => lbids; set => lbids = value; }
        public static List<uint> RIDs { get => rids; set => rids = value; }
        public static Dictionary<uint, ACE.Server.Entity.Spell> SIDs { get => sids; set => sids = value; }
        public static List<string> QFs { get => qfs; set => qfs = value; }

        public static void ContentCommands(Session session, string modPath, params string[] parameters)
        {
            if (parameters?[0].ToLower() == "content" && parameters[1].ToLower() == "import")
                ImportContent(modPath, session);
            if (parameters?[0].ToLower() == "content" && parameters[1].ToLower() == "revert")
                RevertContent(modPath, session);

            if (parameters?[0].ToLower() == "weenies" && parameters[1].ToLower() == "import")
                ImportSQL(session, modPath, ContentType.Weenies);
            if (parameters?[0].ToLower() == "weenies" && parameters[1].ToLower() == "revert")
                RevertSQL(session, modPath, ContentType.Weenies);

            if (parameters?[0].ToLower() == "landblocks" && parameters[1].ToLower() == "import")
                ImportSQL(session, modPath, ContentType.Landblocks);
            if (parameters?[0].ToLower() == "landblocks" && parameters[1].ToLower() == "revert")
                RevertSQL(session, modPath, ContentType.Landblocks);

            if (parameters?[0].ToLower() == "spells" && parameters[1].ToLower() == "import")
                ImportSQL(session, modPath, ContentType.Spells);
            if (parameters?[0].ToLower() == "spells" && parameters[1].ToLower() == "revert")
                RevertSQL(session, modPath, ContentType.Spells);

            if (parameters?[0].ToLower() == "recipes" && parameters[1].ToLower() == "import")
                ImportSQL(session, modPath, ContentType.Recipes);
            if (parameters?[0].ToLower() == "recipes" && parameters[1].ToLower() == "revert")
                RevertSQL(session, modPath, ContentType.Recipes);

            if (parameters?[0].ToLower() == "quests" && parameters[1].ToLower() == "import")
                ImportSQL(session, modPath, ContentType.Quests);
            if (parameters?[0].ToLower() == "quests" && parameters[1].ToLower() == "revert")
                RevertSQL(session, modPath, ContentType.Quests);
        }

        public static void ImportContent(string modPath, Session? session = null)
        {
            ImportSQL(session, modPath, ContentType.Weenies);
            ImportSQL(session, modPath, ContentType.Landblocks);
            ImportSQL(session, modPath, ContentType.Spells);
            ImportSQL(session, modPath, ContentType.Recipes);
            ImportSQL(session, modPath, ContentType.Quests);
        }

        public static void RevertContent(string modPath, Session? session = null)
        {
            RevertSQL(session, modPath, ContentType.Weenies);
            RevertSQL(session, modPath, ContentType.Landblocks);
            RevertSQL(session, modPath, ContentType.Spells);
            RevertSQL(session, modPath, ContentType.Recipes);
            RevertSQL(session, modPath, ContentType.Quests);
        }

        private static void ImportSQL(Session? session, string modPath, ContentType contentType)
        {
            var files = VerifyFilesForImport(session, modPath, contentType, out DirectoryInfo? buDir);
            if (files == null || buDir == null) return;

            switch (contentType)
            {
                case ContentType.Weenies:
                BackupOldWeenies(session, files, buDir);
                ImportNewWeenies(session, files);
                break;
                case ContentType.Landblocks:
                BackupOldLandblocks(session, files, buDir);
                ImportNewLandblocks(session, files);
                break;
                case ContentType.Spells:
                BackupOldSpells(session, files, buDir);
                ImportNewSpells(session, files);
                break;
                case ContentType.Recipes:
                BackupOldRecipes(session, files, buDir);
                ImportNewRecipes(session, files);
                break;
                case ContentType.Quests:
                BackupOldQuests(session, files, buDir);
                ImportNewQuests(session, files);
                break;
            }
            return;
        }

        private static FileInfo[]? VerifyFilesForImport(Session? session, string modPath, ContentType contentType, out DirectoryInfo? buDir)
        {
            var folder = $"{Path.DirectorySeparatorChar}content{Path.DirectorySeparatorChar}{contentType.ToString().ToLower()}";

            var di = new DirectoryInfo(modPath + folder);
            EnumerationOptions options = new();

            var files = di.Exists ? di.GetFiles($"*.sql", options) : null;

            if (files == null || files.Length == 0)
            {
                session?.Player.SendMessage($"[import] found no {contentType.ToString().ToLower()}");
                buDir = null;
                return null;
            }

            session?.Player.SendMessage($"[import] found {files.Length} {contentType.ToString().ToLower()}");

            buDir = new DirectoryInfo(modPath + folder + Path.DirectorySeparatorChar + "old" + Path.DirectorySeparatorChar);
            if (!buDir.Exists)
                buDir.Create();

            return files;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void BackupOldWeenies(Session? session, FileInfo[] files, DirectoryInfo buDir)
        {
            var wsw = new WeenieSQLWriter();
            foreach (var file in files)
            {
                var wcid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                WCIDs.Add(wcid);

                var weenie = DatabaseManager.World.GetWeenie(wcid);
                StreamWriter? sqlFile;
                if (weenie == null)
                {
                    sqlFile = new StreamWriter(buDir.FullName + wcid + " " + "EMPTY WCID" + ".sql");
                    sqlFile.WriteLine($"DELETE FROM `weenie` WHERE `class_Id` = {wcid};");
                }
                else
                {
                    sqlFile = new StreamWriter(buDir.FullName + wcid + " " + weenie.GetProperty(PropertyString.Name) + ".sql");
                    wsw.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
                    wsw.SpellNames = DatabaseManager.World.GetAllSpellNames();
                    wsw.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
                    wsw.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();
                    wsw.PacketOpCodes = PacketOpCodeNames.Values;
                    wsw.CreateSQLDELETEStatement(weenie, sqlFile);
                    sqlFile.WriteLine();
                    wsw.CreateSQLINSERTStatement(weenie, sqlFile);
                }
                sqlFile?.Close();
            }
        }

        private static void ImportNewWeenies(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var wcid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                DeveloperContentCommands.ImportSQL(file.FullName);

                DatabaseManager.World.ClearCachedWeenie(wcid);
                var weenie = DatabaseManager.World.GetCachedWeenie(wcid);

                session?.Player.SendMessage($"[import] (weenies) {file.Name[..file.Name.IndexOf(".")]}: {weenie != null}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void BackupOldLandblocks(Session? session, FileInfo[] files, DirectoryInfo buDir)
        {
            var lbiw = new LandblockInstanceWriter();
            foreach (var file in files)
            {
                var lbid = ushort.Parse(file.Name[..file.Name.IndexOf(".")], System.Globalization.NumberStyles.HexNumber);
                LBIDs.Add(lbid);

                var instances = DatabaseManager.World.GetCachedInstancesByLandblock(lbid);
                StreamWriter? sqlFile = new(buDir.FullName + lbid.ToString("X4") + ".sql");
                if (instances.Count == 0)
                {
                    sqlFile.WriteLine($"DELETE FROM `landblock_instance` WHERE `landblock` = 0x{lbid:X4};");
                }
                else
                {
                    lbiw.CreateSQLDELETEStatement(instances, sqlFile);
                    sqlFile.WriteLine();
                    lbiw.CreateSQLINSERTStatement(instances, sqlFile);
                }
                sqlFile?.Close();
            }
        }

        private static void ImportNewLandblocks(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var lbid = ushort.Parse(file.Name[..file.Name.IndexOf(".")], System.Globalization.NumberStyles.HexNumber);
                DeveloperContentCommands.ImportSQL(file.FullName);

                DatabaseManager.World.ClearCachedInstancesByLandblock(lbid);
                var inst = DatabaseManager.World.GetCachedInstancesByLandblock(lbid);

                session?.Player.SendMessage($"[import] (landblocks) {file.Name[..file.Name.IndexOf(".")]}: {inst != null}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void BackupOldSpells(Session? session, FileInfo[] files, DirectoryInfo buDir)
        {
            var ssw = new SpellSQLWriter();
            foreach (var file in files)
            {
                var sid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                //SIDs.Add(sid);

                var spell = DatabaseManager.World.GetCachedSpell(sid);
                StreamWriter? sqlFile;
                if (spell == null)
                {
                    sqlFile = new StreamWriter(buDir.FullName + sid.ToString("00000") + " " + "EMPTY SID" + ".sql");

                    sqlFile.WriteLine($"DELETE FROM `spell` WHERE `id` = {sid};");
                }
                else
                {
                    sqlFile = new StreamWriter(buDir.FullName + sid.ToString("00000") + " " + spell.Name + ".sql");

                    ssw.CreateSQLDELETEStatement(spell, sqlFile);
                    sqlFile.WriteLine();
                    ssw.CreateSQLINSERTStatement(spell, sqlFile);
                }
                sqlFile?.Close();
            }
        }

        private static void ImportNewSpells(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var sid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                DeveloperContentCommands.ImportSQL(file.FullName);

                ClearCachedSpell(sid);
                var spell = DatabaseManager.World.GetCachedSpell(sid);
                if (spell.ImbuedEffect > 0 && spell.DotDuration != 0)
                {

                    var baseSpellID = (uint)spell.ImbuedEffect;
                    DatManager.PortalDat.SpellTable.Spells.TryGetValue(baseSpellID, out SpellBase? sb);
                    var baseSpell = new ACE.Server.Entity.Spell(baseSpellID);
                    var newSpell = baseSpell;
                    if (sb != null)
                    {
                        sb.Name = spell.Name;
                        sb.MetaSpellId = baseSpell.Id;
                        sb.Duration = spell.DotDuration.GetValueOrDefault(0);
                    }
                    var s = baseSpell._spell.Clone();
                    s.StatModType = spell.StatModType;
                    s.StatModKey = spell.StatModKey;
                    s.StatModVal = spell.StatModVal;
                    s.DotDuration = spell.DotDuration;
                    newSpell._spell = s;
                    newSpell._spellBase = sb;
                    SIDs.Add(sid, newSpell);
                }
                session?.Player.SendMessage($"[import] (spells) {file.Name[..file.Name.IndexOf(".")]}: {spell != null}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void BackupOldRecipes(Session? session, FileInfo[] files, DirectoryInfo buDir)
        {
            var rsw = new RecipeSQLWriter();
            var cbsw = new CookBookSQLWriter();
            foreach (var file in files)
            {
                var rid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                RIDs.Add(rid);

                var recipe = DatabaseManager.World.GetRecipe(rid);
                var cookbooks = DatabaseManager.World.GetCookbooksByRecipeId(rid);
                var weenieName = DatabaseManager.World.GetWeenie(recipe.SuccessWCID).GetProperty(PropertyString.Name);
                StreamWriter? sqlFile;
                if (cookbooks.Count == 0)
                {
                    sqlFile = new StreamWriter(buDir.FullName + rid.ToString("00000") + " " + "EMPTY RID" + ".sql");

                    sqlFile.WriteLine($"DELETE FROM `recipe` WHERE `id` = {rid};");
                    sqlFile.WriteLine($"DELETE FROM `cook_book` WHERE `recipe_Id` = {rid};");
                }
                else
                {
                    sqlFile = new StreamWriter(buDir.FullName + rid.ToString("00000") + " " + weenieName + ".sql");

                    rsw.CreateSQLDELETEStatement(recipe, sqlFile);
                    sqlFile.WriteLine();
                    rsw.CreateSQLINSERTStatement(recipe, sqlFile);
                    sqlFile.WriteLine();
                    cbsw.CreateSQLDELETEStatement(cookbooks, sqlFile);
                    sqlFile.WriteLine();
                    cbsw.CreateSQLINSERTStatement(cookbooks, sqlFile);
                }
                sqlFile?.Close();
            }
        }

        private static void ImportNewRecipes(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var rid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                DeveloperContentCommands.ImportSQL(file.FullName);

                ClearCachedRecipe(rid);
                DatabaseManager.World.ClearCookbookCache();
                var cbs = DatabaseManager.World.GetCookbooksByRecipeId(rid);

                session?.Player.SendMessage($"[import] (recipes) {file.Name[..file.Name.IndexOf(".")]}: {cbs != null}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void BackupOldQuests(Session? session, FileInfo[] files, DirectoryInfo buDir)
        {
            var qsw = new QuestSQLWriter();
            foreach (var file in files)
            {
                var flag = file.Name[..file.Name.IndexOf(".")];
                QFs.Add(flag);

                var quest = DatabaseManager.World.GetCachedQuest(flag);
                StreamWriter? sqlFile = new(buDir.FullName + flag + ".sql");
                if (quest == null)
                {
                    sqlFile.WriteLine($"DELETE FROM `quest` WHERE `name` = {flag};");
                }
                else
                {
                    qsw.CreateSQLDELETEStatement(quest, sqlFile);
                    sqlFile.WriteLine();
                    qsw.CreateSQLINSERTStatement(quest, sqlFile);
                }
                sqlFile?.Close();
            }
        }

        private static void ImportNewQuests(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var flag = file.Name[..file.Name.IndexOf(".")];
                DeveloperContentCommands.ImportSQL(file.FullName);

                DatabaseManager.World.ClearCachedQuest(flag);
                var quest = DatabaseManager.World.GetCachedQuest(flag);

                session?.Player.SendMessage($"[import] (quests) {file.Name[..file.Name.IndexOf(".")]}: {quest != null}");
            }
        }

        private static void RevertSQL(Session? session, string modPath, ContentType contentType)
        {
            var files = VerifyFilesForRevert(session, modPath, contentType, out DirectoryInfo? buDir);
            if (files == null) return;

            switch (contentType)
            {
                case ContentType.Weenies:
                ImportOldWeenies(session, files);
                break;
                case ContentType.Landblocks:
                ImportOldLandblocks(session, files);
                break;
                case ContentType.Spells:
                ImportOldSpells(session, files);
                break;
                case ContentType.Recipes:
                ImportOldRecipes(session, files);
                break;
                case ContentType.Quests:
                ImportOldQuests(session, files);
                break;
            }

            buDir?.Delete(true);
        }

        private static FileInfo[]? VerifyFilesForRevert(Session? session, string modPath, ContentType contentType, out DirectoryInfo? buDir)
        {
            var folder = $"{Path.DirectorySeparatorChar}content{Path.DirectorySeparatorChar}{contentType.ToString().ToLower()}{Path.DirectorySeparatorChar}old";
            var di = new DirectoryInfo(modPath + folder);
            EnumerationOptions options = new();

            var files = di.Exists ? di.GetFiles($"*.sql", options) : null;

            if (files == null || files.Length == 0)
            {
                session?.Player.SendMessage($"[remove] found no {contentType.ToString().ToLower()}");
                buDir = null;
                return null;
            }

            session?.Player.SendMessage($"[remove] found {files.Length} {contentType.ToString().ToLower()}");
            buDir = di;
            return files;
        }

        private static void ImportOldWeenies(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var wcid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                var name = DatabaseManager.World.GetWeenie(wcid).GetProperty(PropertyString.Name);
                DeveloperContentCommands.ImportSQL(file.FullName);
                DatabaseManager.World.ClearCachedWeenie(wcid);
                var weenie = DatabaseManager.World.GetWeenie(wcid);

                if (file.Name.Contains("EMPTY WCID"))
                {
                    session?.Player.SendMessage($"[remove] (weenies) {file.Name[..file.Name.IndexOf(" ")]} {name}: {weenie == null}");
                }
                else
                {
                    session?.Player.SendMessage($"[remove] (weenies) {file.Name[..file.Name.IndexOf(".")]}: {weenie != null}");
                }
            }
            WCIDs.Clear();
        }

        private static void ImportOldLandblocks(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var lbid = ushort.Parse(file.Name[..file.Name.IndexOf(".")], System.Globalization.NumberStyles.HexNumber);
                DeveloperContentCommands.ImportSQL(file.FullName);
                DatabaseManager.World.ClearCachedInstancesByLandblock(lbid);
                var inst = DatabaseManager.World.GetCachedInstancesByLandblock(lbid);

                session?.Player.SendMessage($"[remove] (landblocks) {file.Name[..file.Name.IndexOf(".")]}: {inst != null}");
            }
            LBIDs.Clear();
        }

        private static void ImportOldSpells(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var sid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);
                var name = DatabaseManager.World.GetCachedSpell(sid).Name;
                DeveloperContentCommands.ImportSQL(file.FullName);
                ClearCachedSpell(sid);
                var spell = DatabaseManager.World.GetCachedSpell(sid);

                if (file.Name.Contains("EMPTY SID"))
                {
                    session?.Player.SendMessage($"[remove] (spells) {file.Name[..file.Name.IndexOf(" ")]} {name}: {spell == null}");
                }
                else
                {
                    session?.Player.SendMessage($"[remove] (spells) {file.Name[..file.Name.IndexOf(".")]}: {spell != null}");
                }
            }
            SIDs.Clear();
        }

        private static void ImportOldRecipes(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var rid = uint.Parse(file.Name[..file.Name.IndexOf(" ")]);

                DeveloperContentCommands.ImportSQL(file.FullName);
                ClearCachedRecipe(rid);
                DatabaseManager.World.ClearCookbookCache();
                var recipe = DatabaseManager.World.GetCachedRecipe(rid);
                var name = DatabaseManager.World.GetWeenie(recipe.SuccessWCID).GetProperty(PropertyString.Name);

                if (file.Name.Contains("EMPTY RID"))
                {
                    session?.Player.SendMessage($"[remove] (recipes) {file.Name[..file.Name.IndexOf(" ")]} {name}: {recipe == null}");
                }
                else
                {
                    session?.Player.SendMessage($"[remove] (recipes) {file.Name[..file.Name.IndexOf(".")]}: {recipe != null}");
                }
            }
            RIDs.Clear();
        }

        private static void ImportOldQuests(Session? session, FileInfo[] files)
        {
            foreach (var file in files)
            {
                var flag = file.Name[..file.Name.IndexOf(".")];

                DeveloperContentCommands.ImportSQL(file.FullName);
                DatabaseManager.World.ClearCachedQuest(flag);
                var quest = DatabaseManager.World.GetCachedQuest(flag);
                session?.Player.SendMessage($"[remove] (quests) {file.Name[..file.Name.IndexOf(".")]}: {quest != null}");

            }
            QFs.Clear();
        }

        private static bool /*DatabaseManager.World.*/ ClearCachedSpell(uint spellID) //why no ace
        {
            return DatabaseManager.World.spellCache.TryRemove(spellID, out _);
        }

        private static bool /*DatabaseManager.World.*/ ClearCachedRecipe(uint recipeID) //why no ace?
        {
            return DatabaseManager.World.recipeCache.Remove(recipeID, out _);
        }
    }
}