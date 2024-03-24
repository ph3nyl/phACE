DELETE FROM `spell` WHERE `id` = 6902;

INSERT INTO `spell` (`id`, `name`, `stat_Mod_Type`, `stat_Mod_Key`, `stat_Mod_Val`, `imbued_Effect`, `dot_Duration`, `last_Modified`)
VALUES (6902, 'Culinary Ecstasy', 24578 /* SecondAtt, MultipleStat, Multiplicative */, 0 /* Undef */, 1.1, 3760, -1.0, '2024-03-10 00:00:00');
