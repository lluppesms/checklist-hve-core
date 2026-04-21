BEGIN TRAN

DECLARE 
    @OwnerName nvarchar(50) = 'lyle@luppes.com',
	@Set2Id int,
	@List2Id int,
	@List3Id int,
	@List4Id int,
	@List5Id int,
	@Category11Id int,
	@Category12Id int,
	@Category13Id int,
	@Category14Id int,
	@SetName nvarchar(50) = 'Changing Lanes Check Lists'

DELETE FROM CheckSet Where SetName = @SetName
	
Insert into CheckSet(SetName, OwnerName) Values (@SetName, @OwnerName)
	SET @Set2Id = SCOPE_IDENTITY()

Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('T-24', 'These can be done casually any time the day before', 10, @Set2Id)
	SET @List2Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List2Id)
	SET @Category11Id = SCOPE_IDENTITY()

Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('T-12', 'Do these the night before departure, usually just before bed', 20, @Set2Id)
	SET @List3Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List3Id)
	SET @Category12Id = SCOPE_IDENTITY()

Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('T-0', 'Do these the morning of departure', 30, @Set2Id)
	SET @List4Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List4Id)
	SET @Category13Id = SCOPE_IDENTITY()

Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('Hitching Checklist (time to roll!)', 
'This is BY FAR the most important checklist.  Missing steps here can cause severe damage to the rig and / or truck, and possibly even injuries.  There are some redundancies on this list, but it''s good to double check some things from the outside.', 40, @Set2Id)
	SET @List5Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List5Id)
	SET @Category14Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr,  CategoryId, SortOrder, ListId) Values 
  ('Dump / Flush REAR black tank', null, @Category11Id, 10, @List2Id),
  ('Secure REAR Dump Hose', 'Less stuff to stow on departure day.  Has tube under RV to store them)', @Category11Id, 20, @List2Id),
  ('TREAT REAR Black Tank', null, @Category11Id, 30, @List2Id),
  ('Dump / Flush FRONT black tank', 'We leave this hooked up as it''s also the dump for Grey 1 and Grey 2, the hose is under the slide anyway)', @Category11Id, 40, @List2Id),
  ('TREAT FRONT black tank.', null, @Category11Id, 50, @List2Id),
  ('Fuel Truck', 'Much simpler to fuel without the rig attached, and usually cheaper', @Category11Id, 60, @List2Id),
  ('Inspect HITCH bolts and connectors, including hitch head retaining pins', null, @Category11Id, 70, @List2Id),
  ('Inspect Rig wheels and suspension', 'Torque lug nuts once a month - just a general visual inspection of the leaf springs, attachment points, etc. looking for anything amiss)', @Category11Id, 80, @List2Id),
  ('Sweep slide toppers and inspect roof.', 'While slide toppers really eliminate this need, I still like to get stuff off them that may have accumulated during our stay)', @Category11Id, 90, @List2Id),
  ('Fill fresh tank (% based on next location)', '(usually just 1/3 tank unless we''ll be boondocking', @Category11Id, 100, @List2Id),
  ('Check Rig tires (inflation, general inspection)', 'A TPMS / Tire Pressure Monitoring System makes this a simple task.  I always make sure the tires are +/- 3psi of target of 110psi', @Category11Id, 110, @List2Id),
  ('Check Truck tires (inflation, general inspection)', 'A truck TPMS also greatly simplifies this', @Category11Id, 120, @List2Id),
  ('Check Pin Box Airbag pressure (100psi)', 'Our FlexAir pin box has a shock and airbag.  I''ve found that 100psi cold with no load puts it right about where it''s supposed to ride when loaded', @Category11Id, 130, @List2Id),
  ('Check Radios / charge if needed (we use these)', null, @Category11Id, 140, @List2Id)

Insert into CheckAction(ActionText, ActionDscr,  CategoryId, SortOrder, ListId) Values 
  ('Secure outside items.', 'Rug, chairs, etc', @Category12Id, 10, @List3Id),
  ('Plan route and fuel', 'Get a feel on gas station locations, rest stops, etc.  I also text the next address to myself so I have it in the truck for the GPS in the morning', @Category12Id, 20, @List3Id),
  ('Garage: rear bathroom vent closed and lights off', null, @Category12Id, 30, @List3Id),
  ('Garage: rear bathroom secure and door closed', null, @Category12Id, 40, @List3Id),
  ('Garage: roll up carpet', 'This does require some jugging of the furniture', @Category12Id, 50, @List3Id),
  ('Garage: desk in stow position', 'up against the bathroom door but not tied down yet as I''ll likely still need it', @Category12Id, 60, @List3Id),
  ('Garage: wheel-dock in place and tie downs ready', 'simply on the floor where they will go for travel', @Category12Id, 70, @List3Id),
  ('Garage: Happpijack fully stowed / confirm items strapped to bunk are secure.', null, @Category12Id, 80, @List3Id),
  ('Load Lucile (if possible) and strap down loosely', 'No sense compressing the shocks longer than necessary', @Category12Id, 90, @List3Id),
  ('Secure patio', 'Collapse patio rails and close rear door', @Category12Id, 100, @List3Id),
  ('Dump grey tanks and geo-treat (AFTER showers, dishes, etc)', 'I like to put about 5 gallons of water in both grey tanks along with a cup of water softener and bit of dawn dish soap.  This concoction will splash around in there while we drive and clean the tanks and sensors', @Category12Id, 110, @List3Id)

Insert into CheckAction(ActionText, ActionDscr,  CategoryId, SortOrder, ListId) Values 
  ('Usually Tara starts in the front (bedroom), and I start in the rear (garage), and we meet in the middle.', null, @Category13Id, 10, @List4Id),
  ('Run generator (exercise) while prepping to leave (if etiquette allows)', 'when we can, we switch over to genny power to run the A/C, etc for the last 30 mins to hour to exercise it', @Category13Id, 20, @List4Id),
  ('Bedroom secure and carpet stowed', 'closet door latched, laundry door closed', @Category13Id, 30, @List4Id),
  ('Bedroom windows closed', null, @Category13Id, 40, @List4Id),
  ('Bedroom slide in', null, @Category13Id, 50, @List4Id),
  ('Bedroom A/C and lights off', null, @Category13Id, 60, @List4Id),
  ('Bathroom shower secure', 'everything off shelves, shower door locked open)', @Category13Id, 70, @List4Id),
  ('Bathroom secure', 'vent closed, counter items stowed, bathroom door latched open', @Category13Id, 80, @List4Id),
  ('Hall window blind up.', null, @Category13Id, 90, @List4Id),
  ('Garage: Desk secured', 'NAS disconnected and stowed, monitor in travel mode, desk and chair strapped down)', @Category13Id, 100, @List4Id),
  ('Garage: secure ', 'blinds up, patio doors latched, garage door locked, rolled rug on floor, tv locked in, happi-jack all the way up)', @Category13Id, 110, @List4Id),
  ('Garage: Motorcycle straps tightened down', null, @Category13Id, 120, @List4Id),
  ('Garage: Rear A/C and lights off', null, @Category13Id, 130, @List4Id),
  ('Living room secure', 'refrigerator doors shut and latched, carpets stowed, chairs secured with padding, all drawers and cabinets closed)', @Category13Id, 140, @List4Id),
  ('Pantry door closed', null, @Category13Id, 150, @List4Id),
  ('Daisy''s food and water bowls put away', null, @Category13Id, 160, @List4Id),
  ('Windows closed / blinds up', null, @Category13Id, 170, @List4Id),
  ('Middle A/C off', null, @Category13Id, 180, @List4Id),
  ('One-Control Items off', 'water pump, water heater, ALL Lights)', @Category13Id, 190, @List4Id),
  ('Generator off', null, @Category13Id, 200, @List4Id),
  ('Inverter off', null, @Category13Id, 210, @List4Id),
  ('Refrigerator switched to gas (auto)', null, @Category13Id, 220, @List4Id),
  ('Slides: one last check for obstacles inside and out', null, @Category13Id, 230, @List4Id),
  ('Slides: Retract with door, vent, or window open for air flow', null, @Category13Id, 240, @List4Id),
  ('Put Spare key in truck', 'We don''t like our home''s keys in the truck when camped', @Category13Id, 250, @List4Id),
  ('Secure main entrance', 'stow steps, lock door, secure hand hold)', @Category13Id, 260, @List4Id),
  ('Secure Electrical and plumbing', 'water hose, power cord, poop hose)', @Category13Id, 270, @List4Id),
  ('Nautilus in dry camp mode', 'relieve system pressure before switching)', @Category13Id, 280, @List4Id)

Insert into CheckAction(ActionText, ActionDscr,  CategoryId, SortOrder, ListId) Values 
  ('T-24, T-12, T-0 Checklists Complete', null, @Category14Id, 10, @List5Id),
  ('RIG - Slides and awnings IN', null, @Category14Id, 20, @List5Id),
  ('RIG - Stairs and hand rail STOWED.', null, @Category14Id, 30, @List5Id),
  ('RIG - Forward Bay Closed and Latched', null, @Category14Id, 40, @List5Id),
  ('RIG - Pin Lock Removed', 'we use this pin lock - yes we''re paranoid', @Category14Id, 50, @List5Id),
  ('RIG - Jacks / Stabilizers to TOW HEIGHT ', 'Middle and Rear Retracted', @Category14Id, 60, @List5Id),
  ('TRUCK - TPMS ON', null, @Category14Id, 70, @List5Id),
  ('TRUCK - Tailgate DOWN', null, @Category14Id, 80, @List5Id),
  ('TRUCK - Back ALMOST to pin-box', null, @Category14Id, 90, @List5Id),
  ('RIG - Adjust Kingpin height to proper hitch height of truck.', null, @Category14Id, 100, @List5Id),
  ('TRUCK - OPEN Hitch Latch', null, @Category14Id, 110, @List5Id),
  ('TRUCK - Back truck into Kingpin', null, @Category14Id, 120, @List5Id),
  ('TRUCK - Visually check KINGPIN LOCK BAR IS LOCKED', null, @Category14Id, 130, @List5Id),
  ('TRUCK - Connect Electrical cord and Breakaway cable.', null, @Category14Id, 140, @List5Id),
  ('RIG - Lower Rig with Front landing gear to pull test height', 'front jacks 1? off the ground', @Category14Id, 150, @List5Id),
  ('TRUCK - Tow/Haul Mode', null, @Category14Id, 160, @List5Id),
  ('TRUCK - Mirrors EXTENDED', null, @Category14Id, 170, @List5Id),
  ('TRUCK - PULL TEST', 'manually engage trailer brakes and try to pull away SLOW', @Category14Id, 180, @List5Id),
  ('TRUCK - Check that trailer brakes are adjusted and trailer is connected in ford system.', null, @Category14Id, 190, @List5Id),
  ('TRUCK - Set parking brake.', null, @Category14Id, 200, @List5Id),
  ('RIG - Retract ALL on landing gear', null, @Category14Id, 210, @List5Id),
  ('TRUCK - CLOSE TAILGATE', null, @Category14Id, 220, @List5Id),
  ('RIG - Remove and stow wheel chocks and landing gear blocks.', null, @Category14Id, 230, @List5Id),
  ('RIG - Check and close all storage doors and lock.', null, @Category14Id, 240, @List5Id),
  ('TRUCK - Lights on / Rear Camera working', null, @Category14Id, 250, @List5Id),
  ('TRUCK - Validate TPMS reads all 6 tires and validate pressure / temperature', null, @Category14Id, 260, @List5Id),
  ('TRUCK / RIG - Verify all lights are operable (signals, brake, reverse)', null, @Category14Id, 270, @List5Id),
  ('TRUCK / RIG - Full Walkaround', null, @Category14Id, 280, @List5Id),
  ('TRUCK - Rest Trip Meter', null, @Category14Id, 290, @List5Id),
  ('GO!', null, @Category14Id, 300, @List5Id)

Select s.SetId, s.SetName, l.ListId, l.ListName, l.ListDscr, c.CategoryId, c.CategoryText, a.ActionId, a.ActionText, a.ActionDscr
From CheckSet s
INNER JOIN CheckList l on s.SetId = l.SetId
INNER JOIN CheckCategory c on c.ListId = l.ListId
INNER JOIN CheckAction a on a.CategoryId = c.CategoryId
Where s.SetName = @SetName

COMMIT
-- ROLLBACK

