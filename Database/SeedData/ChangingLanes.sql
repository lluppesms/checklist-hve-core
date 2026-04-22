-- =============================================================
-- Changing Lanes Check Lists - Template seed data
-- Derived from old-source/Database/ChangingLanesLists.sql
-- Covers: T-24, T-12, T-0 prep, and Hitching lists
-- =============================================================

-- TemplateSets
SET IDENTITY_INSERT [dbo].[TemplateSets] ON
INSERT INTO [dbo].[TemplateSets] ([Id], [SetName], [SetDscr], [OwnerName], [ActiveInd], [SortOrder])
VALUES (2, N'Changing Lanes Check Lists', N'Departure preparation checklists organized by time before departure', N'lyle@luppes.com', N'Y', 20)
SET IDENTITY_INSERT [dbo].[TemplateSets] OFF

-- TemplateLists
SET IDENTITY_INSERT [dbo].[TemplateLists] ON
INSERT INTO [dbo].[TemplateLists] ([Id], [SetId], [ListName], [ListDscr], [ActiveInd], [SortOrder])
VALUES
    (4, 2, N'T-24',                           N'These can be done casually any time the day before', N'Y', 10),
    (5, 2, N'T-12',                           N'Do these the night before departure, usually just before bed', N'Y', 20),
    (6, 2, N'T-0',                            N'Do these the morning of departure', N'Y', 30),
    (7, 2, N'Hitching Checklist (time to roll!)', N'BY FAR the most important checklist. Missing steps can cause severe damage.', N'Y', 40)
SET IDENTITY_INSERT [dbo].[TemplateLists] OFF

-- TemplateCategories
SET IDENTITY_INSERT [dbo].[TemplateCategories] ON
INSERT INTO [dbo].[TemplateCategories] ([Id], [ListId], [CategoryText], [ActiveInd], [SortOrder])
VALUES
    (4, 4, N'Main', N'Y', 10),
    (5, 5, N'Main', N'Y', 10),
    (6, 6, N'Main', N'Y', 10),
    (7, 7, N'Main', N'Y', 10)
SET IDENTITY_INSERT [dbo].[TemplateCategories] OFF

-- TemplateActions - T-24 (CategoryId=4)
SET IDENTITY_INSERT [dbo].[TemplateActions] ON
INSERT INTO [dbo].[TemplateActions] ([Id], [CategoryId], [ActionText], [ActionDscr], [ActiveInd], [SortOrder])
VALUES
    (66,  4, N'Dump / Flush REAR black tank',                           NULL, N'Y', 10),
    (67,  4, N'Secure REAR Dump Hose',                                  N'Less stuff to stow on departure day', N'Y', 20),
    (68,  4, N'TREAT REAR Black Tank',                                  NULL, N'Y', 30),
    (69,  4, N'Dump / Flush FRONT black tank',                          NULL, N'Y', 40),
    (70,  4, N'TREAT FRONT black tank',                                 NULL, N'Y', 50),
    (71,  4, N'Fuel Truck',                                             N'Much simpler to fuel without the rig attached', N'Y', 60),
    (72,  4, N'Inspect HITCH bolts and connectors, including hitch head retaining pins', NULL, N'Y', 70),
    (73,  4, N'Inspect Rig wheels and suspension',                      N'Torque lug nuts once a month - general visual inspection', N'Y', 80),
    (74,  4, N'Sweep slide toppers and inspect roof',                   NULL, N'Y', 90),
    (75,  4, N'Fill fresh tank (% based on next location)',             N'Usually just 1/3 tank unless boondocking', N'Y', 100),
    (76,  4, N'Check Rig tires (inflation, general inspection)',        N'TPMS makes this simple - target +/- 3psi', N'Y', 110),
    (77,  4, N'Check Truck tires (inflation, general inspection)',      NULL, N'Y', 120),
    (78,  4, N'Check Pin Box Airbag pressure (100psi)',                 N'FlexAir pin box - 100psi cold with no load', N'Y', 130),
    (79,  4, N'Check Radios / charge if needed',                       NULL, N'Y', 140),

-- TemplateActions - T-12 (CategoryId=5)
    (80,  5, N'Secure outside items',                                   N'Rug, chairs, etc', N'Y', 10),
    (81,  5, N'Plan route and fuel',                                    N'Gas stations, rest stops, text address to phone for GPS', N'Y', 20),
    (82,  5, N'Garage: rear bathroom vent closed and lights off',       NULL, N'Y', 30),
    (83,  5, N'Garage: rear bathroom secure and door closed',           NULL, N'Y', 40),
    (84,  5, N'Garage: roll up carpet',                                 NULL, N'Y', 50),
    (85,  5, N'Garage: desk in stow position',                          NULL, N'Y', 60),
    (86,  5, N'Garage: wheel-dock in place and tie downs ready',        NULL, N'Y', 70),
    (87,  5, N'Garage: Happpijack fully stowed / confirm items strapped to bunk are secure', NULL, N'Y', 80),
    (88,  5, N'Load Lucile (if possible) and strap down loosely',       NULL, N'Y', 90),
    (89,  5, N'Secure patio',                                           N'Collapse patio rails and close rear door', N'Y', 100),
    (90,  5, N'Dump grey tanks and geo-treat (AFTER showers, dishes, etc)', NULL, N'Y', 110),

-- TemplateActions - T-0 (CategoryId=6)
    (91,  6, N'Usually Tara starts in the front (bedroom), I start in the rear (garage), and we meet in the middle', NULL, N'Y', 10),
    (92,  6, N'Run generator (exercise) while prepping to leave (if etiquette allows)', N'Switch to genny power last 30-60 mins to exercise it', N'Y', 20),
    (93,  6, N'Bedroom secure and carpet stowed',                       N'Closet door latched, laundry door closed', N'Y', 30),
    (94,  6, N'Bedroom windows closed',                                 NULL, N'Y', 40),
    (95,  6, N'Bedroom slide in',                                       NULL, N'Y', 50),
    (96,  6, N'Bedroom A/C and lights off',                             NULL, N'Y', 60),
    (97,  6, N'Bathroom shower secure',                                 N'Everything off shelves, shower door locked open', N'Y', 70),
    (98,  6, N'Bathroom secure',                                        N'Vent closed, counter items stowed, bathroom door latched open', N'Y', 80),
    (99,  6, N'Hall window blind up',                                   NULL, N'Y', 90),
    (100, 6, N'Garage: Desk secured',                                   N'NAS disconnected and stowed, monitor in travel mode, desk and chair strapped down', N'Y', 100),
    (101, 6, N'Garage: secure',                                         N'Blinds up, patio doors latched, garage door locked, rolled rug on floor, tv locked in, happi-jack all the way up', N'Y', 110),
    (102, 6, N'Garage: Motorcycle straps tightened down',               NULL, N'Y', 120),
    (103, 6, N'Garage: Rear A/C and lights off',                        NULL, N'Y', 130),
    (104, 6, N'Living room secure',                                     N'Refrigerator doors shut and latched, carpets stowed, chairs secured, all drawers and cabinets closed', N'Y', 140),
    (105, 6, N'Pantry door closed',                                     NULL, N'Y', 150),
    (106, 6, N'Daisy''s food and water bowls put away',                 NULL, N'Y', 160),
    (107, 6, N'Windows closed / blinds up',                             NULL, N'Y', 170),
    (108, 6, N'Middle A/C off',                                         NULL, N'Y', 180),
    (109, 6, N'One-Control Items off',                                  N'Water pump, water heater, ALL Lights', N'Y', 190),
    (110, 6, N'Generator off',                                          NULL, N'Y', 200),
    (111, 6, N'Inverter off',                                           NULL, N'Y', 210),
    (112, 6, N'Refrigerator switched to gas (auto)',                    NULL, N'Y', 220),
    (113, 6, N'Slides: one last check for obstacles inside and out',    NULL, N'Y', 230),
    (114, 6, N'Slides: Retract with door, vent, or window open for air flow', NULL, N'Y', 240),
    (115, 6, N'Put Spare key in truck',                                 N'We don''t like our home keys in the truck when camped', N'Y', 250),
    (116, 6, N'Secure main entrance',                                   N'Stow steps, lock door, secure hand hold', N'Y', 260),
    (117, 6, N'Secure Electrical and plumbing',                         N'Water hose, power cord, poop hose', N'Y', 270),
    (118, 6, N'Nautilus in dry camp mode',                              N'Relieve system pressure before switching', N'Y', 280),

-- TemplateActions - Hitching (CategoryId=7)
    (119, 7, N'T-24, T-12, T-0 Checklists Complete',                   NULL, N'Y', 10),
    (120, 7, N'RIG - Slides and awnings IN',                            NULL, N'Y', 20),
    (121, 7, N'RIG - Stairs and hand rail STOWED',                      NULL, N'Y', 30),
    (122, 7, N'RIG - Forward Bay Closed and Latched',                   NULL, N'Y', 40),
    (123, 7, N'RIG - Pin Lock Removed',                                 NULL, N'Y', 50),
    (124, 7, N'RIG - Jacks / Stabilizers to TOW HEIGHT (Middle and Rear Retracted)', NULL, N'Y', 60),
    (125, 7, N'TRUCK - TPMS ON',                                        NULL, N'Y', 70),
    (126, 7, N'TRUCK - Tailgate DOWN',                                  NULL, N'Y', 80),
    (127, 7, N'TRUCK - Back ALMOST to pin-box',                         NULL, N'Y', 90),
    (128, 7, N'RIG - Adjust Kingpin height to proper hitch height of truck', NULL, N'Y', 100),
    (129, 7, N'TRUCK - OPEN Hitch Latch',                               NULL, N'Y', 110),
    (130, 7, N'TRUCK - Back truck into Kingpin',                        NULL, N'Y', 120),
    (131, 7, N'TRUCK - Visually check KINGPIN LOCK BAR IS LOCKED',      NULL, N'Y', 130),
    (132, 7, N'TRUCK - Connect Electrical cord and Breakaway cable',    NULL, N'Y', 140),
    (133, 7, N'RIG - Lower Rig with Front landing gear to pull test height (front jacks 1" off ground)', NULL, N'Y', 150),
    (134, 7, N'TRUCK - Tow/Haul Mode',                                  NULL, N'Y', 160),
    (135, 7, N'TRUCK - Mirrors EXTENDED',                               NULL, N'Y', 170),
    (136, 7, N'TRUCK - PULL TEST',                                      N'Manually engage trailer brakes and try to pull away SLOW', N'Y', 180),
    (137, 7, N'TRUCK - Check that trailer brakes are adjusted and trailer is connected', NULL, N'Y', 190),
    (138, 7, N'TRUCK - Set parking brake',                              NULL, N'Y', 200),
    (139, 7, N'RIG - Retract ALL on landing gear',                      NULL, N'Y', 210),
    (140, 7, N'TRUCK - CLOSE TAILGATE',                                 NULL, N'Y', 220),
    (141, 7, N'RIG - Remove and stow wheel chocks and landing gear blocks', NULL, N'Y', 230),
    (142, 7, N'RIG - Check and close all storage doors and lock',       NULL, N'Y', 240),
    (143, 7, N'TRUCK - Lights on / Rear Camera working',                NULL, N'Y', 250),
    (144, 7, N'TRUCK - Validate TPMS reads all 6 tires and validate pressure / temperature', NULL, N'Y', 260),
    (145, 7, N'TRUCK / RIG - Verify all lights are operable (signals, brake, reverse)', NULL, N'Y', 270),
    (146, 7, N'TRUCK / RIG - Full Walkaround',                          NULL, N'Y', 280),
    (147, 7, N'TRUCK - Reset Trip Meter',                               NULL, N'Y', 290),
    (148, 7, N'GO!',                                                    NULL, N'Y', 300)
SET IDENTITY_INSERT [dbo].[TemplateActions] OFF
