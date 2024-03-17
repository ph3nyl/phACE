DELETE FROM `spell` WHERE `id` = 6901;

INSERT INTO `spell` (`id`, `name`, `stat_Mod_Type`, `stat_Mod_Key`, `stat_Mod_Val`, `imbued_Effect`, `dot_Duration`, `last_Modified`)
VALUES (6901, 'Mire Foot', 20496 /* Skill, SingleStat, Multiplicative */, 24 /* Run */, 0.05, 3051, 5.0, '2024-03-10 00:00:00');
