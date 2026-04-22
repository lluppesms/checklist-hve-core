-- =============================================================
-- RV Trip Prep & Travel - Template seed data
-- Derived from old-source/Database/LylesCheckList.sql
-- Covers: Trip Prep, Hitching, and Arrival lists
-- =============================================================

-- TemplateSets
SET IDENTITY_INSERT [dbo].[TemplateSets] ON
INSERT INTO [dbo].[TemplateSets] ([Id], [SetName], [SetDscr], [OwnerName], [ActiveInd], [SortOrder])
VALUES (1, N'RV Trip Prep & Travel', N'Checklists for RV trip preparation, hitching, and arrival at campsite', N'LLUPPES', N'Y', 10)
SET IDENTITY_INSERT [dbo].[TemplateSets] OFF

-- TemplateLists
SET IDENTITY_INSERT [dbo].[TemplateLists] ON
INSERT INTO [dbo].[TemplateLists] ([Id], [SetId], [ListName], [ListDscr], [ActiveInd], [SortOrder])
VALUES
    (1, 1, N'1-Trip Prep',  N'These should be done before you leave home', N'Y', 10),
    (2, 1, N'2-Hitching',   N'Do these when you are hitching up to go somewhere', N'Y', 20),
    (3, 1, N'3-Arrival',    N'Do these when you arrive and unhitch', N'Y', 30)
SET IDENTITY_INSERT [dbo].[TemplateLists] OFF

-- TemplateCategories
SET IDENTITY_INSERT [dbo].[TemplateCategories] ON
INSERT INTO [dbo].[TemplateCategories] ([Id], [ListId], [CategoryText], [ActiveInd], [SortOrder])
VALUES
    (1, 1, N'Main', N'Y', 10),
    (2, 2, N'Main', N'Y', 10),
    (3, 3, N'Main', N'Y', 10)
SET IDENTITY_INSERT [dbo].[TemplateCategories] OFF

-- TemplateActions - Trip Prep (CategoryId=1)
SET IDENTITY_INSERT [dbo].[TemplateActions] ON
INSERT INTO [dbo].[TemplateActions] ([Id], [CategoryId], [ActionText], [ActionDscr], [ActiveInd], [SortOrder])
VALUES
    (1,  1, N'Fuel up the Truck',                                                                        NULL, N'Y', 10),
    (2,  1, N'Inspect Hitch bolts and connectors, including hitch head retaining pins',                  NULL, N'Y', 20),
    (3,  1, N'Inspect RV wheels and suspension (torque lug nuts 1x/month)',                              NULL, N'Y', 30),
    (4,  1, N'Fill fresh tank (% based on next location, usually 1/3 unless boondocking)',               NULL, N'Y', 40),
    (5,  1, N'Check RV tires (inflation, general inspection)',                                           NULL, N'Y', 50),
    (6,  1, N'Check Truck tires (inflation, general inspection)',                                        NULL, N'Y', 60),
    (7,  1, N'Top-off RV batteries',                                                                     NULL, N'Y', 70),
    (8,  1, N'Plan route and fuel (gas station locations, rest stops, get address in phone for GPS)',     NULL, N'Y', 80),

-- TemplateActions - Hitching (CategoryId=2)
    (9,  2, N'TRUCK - Tailgate down',                                                                               NULL, N'Y', 10),
    (10, 2, N'TRUCK - Back almost to pin-box',                                                                      NULL, N'Y', 20),
    (11, 2, N'RV - Set Kingpin height to 1/2" above hitch in bed of truck',                                        NULL, N'Y', 30),
    (12, 2, N'TRUCK - Pull handle on hitch out until it clicks',                                                    NULL, N'Y', 40),
    (13, 2, N'TRUCK - Back truck into hitch until hitch engages (you will hear a loud clunk)',                      NULL, N'Y', 50),
    (14, 2, N'TRUCK - Visually check that kingpin hitch is locked - look for white stripe',                         NULL, N'Y', 60),
    (15, 2, N'TRUCK - Insert locking pin in hitch handle',                                                          NULL, N'Y', 70),
    (16, 2, N'TRUCK - Connect 7 pin cable and breakaway cable to truck',                                            NULL, N'Y', 80),
    (17, 2, N'TRUCK - Close tailgate',                                                                              NULL, N'Y', 90),
    (18, 2, N'RV - Retract Front landing gear using Leveling Control to 1" off ground for pull test',              NULL, N'Y', 100),
    (19, 2, N'TRUCK - Put into Tow/Haul Mode',                                                                      NULL, N'Y', 110),
    (20, 2, N'TRUCK - Pull test -- manually engage trailer brakes and try to pull away slowly',                     NULL, N'Y', 120),
    (21, 2, N'TRUCK - Set parking brake',                                                                           NULL, N'Y', 130),
    (22, 2, N'RV - Retract all landing gear using Leveling Control and pressing Retract All',                       NULL, N'Y', 140),
    (23, 2, N'RV - Manually retract front stabilizer legs by pulling pin and lifting them',                         NULL, N'Y', 150),
    (24, 2, N'RV - Manually retract rear stabilizer legs by removing cotter pin and lifting them',                  NULL, N'Y', 160),
    (25, 2, N'RV - Remove and stow wheel chocks, x-chocks, and landing gear blocks',                                NULL, N'Y', 170),
    (26, 2, N'RV - Check and close all storage doors and lock',                                                     NULL, N'Y', 180),
    (27, 2, N'TRUCK - Lights on / rear camera working',                                                             NULL, N'Y', 190),
    (28, 2, N'TRUCK - Validate TPMS reads all tires and validate pressure / temperature',                           NULL, N'Y', 200),
    (29, 2, N'TRUCK / RV - Verify all lights are operable (turn signals, brake lights, reverse)',                   NULL, N'Y', 210),
    (30, 2, N'Do a walk-around the trailer and vehicle and check EVERYTHING before pulling-out',                    NULL, N'Y', 220),
    (31, 2, N'TRUCK - Mirrors extended',                                                                            NULL, N'Y', 230),
    (32, 2, N'TRUCK - Reset Trip Meter',                                                                            NULL, N'Y', 240),
    (33, 2, N'GO!',                                                                                                 NULL, N'Y', 250),

-- TemplateActions - Arrival (CategoryId=3)
    (34, 3, N'Back RV into site and set parking brake on truck',                                                                                     NULL, N'Y', 10),
    (35, 3, N'Verify that there is room to extend all three slides',                                                                                 NULL, N'Y', 20),
    (36, 3, N'Verify that you can reach sewer and electrical connections',                                                                           NULL, N'Y', 30),
    (37, 3, N'Turn battery disconnect to On (red key in drivers front bin)',                                                                         NULL, N'Y', 40),
    (38, 3, N'Level the trailer side-by-side by adding blocks under the tires (Auto level compensates up to 1 degree)',                              NULL, N'Y', 50),
    (39, 3, N'Chock the tires on both sides with 4 black chocks - CRITICAL before detaching hitch',                                                  NULL, N'Y', 60),
    (40, 3, N'Remove cotter pins on both rear stabilizer jacks and drop down to 4th hole (same height on both sides)',                               NULL, N'Y', 70),
    (41, 3, N'Pull pins on front stabilizer jacks and drop to 8" off the ground (same on both sides)',                                              NULL, N'Y', 80),
    (42, 3, N'Place at least one Lego block under each stabilizer jack foot',                                                                        NULL, N'Y', 90),
    (43, 3, N'Turn on leveling panel by pushing up & down buttons simultaneously',                                                                   NULL, N'Y', 100),
    (44, 3, N'Push up button on leveling panel to lower front stabilizers, raising trailer just enough to take weight off truck',                     NULL, N'Y', 110),
    (45, 3, N'Remove cotter locking pin from 5th wheel hitch handle',                                                                               NULL, N'Y', 120),
    (46, 3, N'Pull handle on 5th wheel hitch to unlock king pin',                                                                                    NULL, N'Y', 130),
    (47, 3, N'Drop tailgate',                                                                                                                        NULL, N'Y', 140),
    (48, 3, N'Unhook breakaway cable and 7 pin connection from rear of truck',                                                                       NULL, N'Y', 150),
    (49, 3, N'Pull truck forward slowly until it is out from under the camper',                                                                      NULL, N'Y', 160),
    (50, 3, N'Raise tailgate',                                                                                                                       NULL, N'Y', 170),
    (51, 3, N'Stow the 7 pin connector cable and breakaway cable up in the hitch',                                                                   NULL, N'Y', 180),
    (52, 3, N'Press Auto-Level on leveling panel and RV will start moving all 4 stabilizers until level (2-3 minutes)',                              NULL, N'Y', 190),
    (53, 3, N'Install the X-chocks between tires on both sides to stabilize RV',                                                                     NULL, N'Y', 200),
    (54, 3, N'Turn off breaker and attach Surge Guard to verify circuit is OK',                                                                      NULL, N'Y', 210),
    (55, 3, N'Plug 50 amp extension cord into Surge Guard and into RV; turn on breaker',                                                             NULL, N'Y', 220),
    (56, 3, N'Use Control Panel to extend slides, turn on water heater, and put out awning',                                                         NULL, N'Y', 230),
    (57, 3, N'Spray city water spigot with bleach, hook up pressure regulator, Y adapter, water hose, filter, flexible connector',                   NULL, N'Y', 240),
    (58, 3, N'Turn on water for a minute to push air out of hose before connecting to RV',                                                           NULL, N'Y', 250),
    (59, 3, N'Connect flexible connector to inlet on RV',                                                                                            NULL, N'Y', 260),
    (60, 3, N'Set RV to City Water',                                                                                                                 NULL, N'Y', 270),
    (61, 3, N'Turn on water',                                                                                                                        NULL, N'Y', 280),
    (62, 3, N'If using water tank (not city water), turn on the water pump',                                                                         NULL, N'Y', 290),
    (63, 3, N'Turn on propane',                                                                                                                      NULL, N'Y', 300),
    (64, 3, N'Turn on the refrigerator to Auto',                                                                                                     NULL, N'Y', 310),
    (65, 3, N'Add one scoop of Happy Camper to toilet along with 5 gallons of water',                                                                NULL, N'Y', 320)
SET IDENTITY_INSERT [dbo].[TemplateActions] OFF
