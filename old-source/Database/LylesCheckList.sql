BEGIN TRAN

DECLARE 
	@Set1Id int,
	@Set1Name nvarchar(255),
	@List1Id int,
	@List2Id int,
	@List3Id int,
	@List4Id int,
	@List5Id int,
	@List6Id int,
	@List7Id int,
	@Category1Id int,
	@Category2Id int,
	@Category3Id int,
	@Category4Id int,
	@Category5Id int,
	@Category6Id int,
	@Category7Id int
SET @Set1Name = 'Lyle''s Check Lists'

DELETE FROM CheckSet Where SetName = @Set1Name

Insert into CheckSet(SetName, OwnerName) Values (@Set1Name, 'LLUPPES')
	SET @Set1Id = SCOPE_IDENTITY()

Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('1-Trip Prep', 'These should be done before you leave home', 10, @Set1Id)
	SET @List1Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List1Id)
	SET @Category1Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('Fuel up the Truck', null, @List1Id, @Category1Id, 10),
  ('Inspect Hitch bolts and connectors, including hitch head retaining pins', null, @List1Id, @Category1Id, 20),
  ('Inspect RV wheels and suspension (torque lug nugs 1x/month)', null, @List1Id, @Category1Id, 30),
  ('Fill fresh tank (% based on next location, usually 1/3 unless boondocking)', null, @List1Id, @Category1Id, 40),
  ('Check RV tires (inflation, general inspection)', null, @List1Id, @Category1Id, 50),
  ('Check Truck tires (inflation, general inspection)', null, @List1Id, @Category1Id, 60),
  ('Top-off RV batteries', null, @List1Id, @Category1Id, 70),
  ('Plan route and fuel (gas station locations, rest stops, get address in phone for GPS)', null, @List1Id, @Category1Id, 80)

  
Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('2-Hitching', 'Do these these when you are hitching up to go somewhere', 20, @Set1Id)
	SET @List2Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List2Id)
	SET @Category2Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('TRUCK - Tailgate down', null, @List1Id, @Category2Id, 80),
  ('TRUCK - Back almost to pin-box', null, @List1Id, @Category2Id, 90),
  ('RV - Set Kingpin height to 1/2" above hitch in bed of truck', null, @List1Id, @Category2Id, 100),
  ('TRUCK - Pull handle on hitch out until it clicks', null, @List1Id, @Category2Id, 110),
  ('TRUCK - Back truck into hitch until hitch engages (you will hear a loud clunk)', null, @List1Id, @Category2Id, 120),
  ('TRUCK - Visually check that kingpin hitch is locked - look for white stipe', null, @List1Id, @Category2Id, 130),
  ('TRUCK - Insert locking pin in hitch handle', null, @List1Id, @Category2Id, 140),
  ('TRUCK - Connect 7 pin cable and breakaway cable to truck.', null, @List1Id, @Category2Id, 150),
  ('TRUCK - Close tailgate', null, @List1Id, @Category2Id, 160),
  ('RV - Retract Front landing gear using Leveling Control to 1" off ground for pull test', null, @List1Id, @Category2Id, 170),
  ('TRUCK - Put into Tow/Haul Mode', null, @List1Id, @Category2Id, 180),
  ('TRUCK - Pull test -- manually engage trailer brakes and try to pull away slowly', null, @List1Id, @Category2Id, 190),
  ('TRUCK - Set parking brake', null, @List1Id, @Category2Id, 200),
  ('RV - Retract all landing gear using Leveling Control and pressing Retract All', null, @List1Id, @Category2Id, 210),
  ('RV - Manually retract front stabilizer legs by pulling pin and lifting them', null, @List1Id, @Category2Id, 220),
  ('RV - Manually retract rear stabilizer legs by removing cotter pin pin and lifting them', null, @List1Id, @Category2Id, 230),
  ('RV - Remove and stow wheel chocks, x-chocks, and landing gear blocks.', null, @List1Id, @Category2Id, 240),
  ('RV - Check and close all storage doors and lock.', null, @List1Id, @Category2Id, 250),
  ('TRUCK - Lights on / rear camera working', null, @List1Id, @Category2Id, 260),
  ('TRUCK - Validate TPMS reads all tires and validate pressure / temperature', null, @List1Id, @Category2Id, 270),
  ('TRUCK / RV - Verify all lights are operable (turn signals, brake lights, reverse)', null, @List1Id, @Category2Id, 280),
  ('Do a walk-around  the trailer and vehicle and check EVERYTHING before pulling-out', null, @List1Id, @Category2Id, 290),
  ('TRUCK - Mirrors extended', null, @List1Id, @Category2Id, 300),
  ('TRUCK - Put into Tow/Haul Mode', null, @List1Id, @Category2Id, 310),
  ('TRUCK - Reset Trip Meter', null, @List1Id, @Category2Id, 320),
  ('GO!', null, @List1Id, @Category2Id, 330)


Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('3-Arrival', 'Do these when you arrive and unhitch', 30, @Set1Id)
	SET @List3Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List3Id)
	SET @Category3Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('Back RV into site and set parking brake on truck', null, @List1Id, @Category3Id, 10),
  ('Verify that there is room to extend all three slides', null, @List1Id, @Category3Id, 20),
  ('Verify that you can reach sewer and electrical connections', null, @List1Id, @Category3Id, 30),
  ('Turn battery disconnect to On (red key in drivers front bin)', null, @List1Id, @Category3Id, 40),
  ('Level the trailer side-by-side by adding Lego-blocks under the tires and pulling the RV over them.  (Note: Auto level will compensate for up to 1 degree out of level.  you can see the level values in the One Control App -> Leveling -> Manual Control.', null, @List1Id, @Category3Id, 50),
  ('Chock the tires on both sides with 4 black chocks.  NOTE: This is critical before detaching hitch or trailer may roll.', null, @List1Id, @Category3Id, 60),
  ('Remove cotter pins on both rear stabilizer jacks and drop down to 4th hole (or same height on both sides) and put pins back in', null, @List1Id, @Category3Id, 70),
  ('Pull pins on front stabilizer jacks and drop them to 8" off the ground (or same on both sides)  Don''t drop too far or the auto-level sequence will fail.  If one side is much lower, put blocks under that side.', null, @List1Id, @Category3Id, 80),
  ('Place at least one Lego block under each stabilizer jack foot', null, @List1Id, @Category3Id, 90),
  ('Turn on leveling panel (in drivers front bin) by pushing up & down buttons simultaneously.', null, @List1Id, @Category3Id, 100),
  ('Push up button on leveling panel to lower front stabilizers to raise trailer up and take weight off truck, raising it just enough to see daylight between hitch and the 5th wheel', null, @List1Id, @Category3Id, 110),
  ('Remove cotter locking pin from 5th wheel hitch handle', null, @List1Id, @Category3Id, 120),
  ('Pull handle on 5th wheel hitch to unlock king pin (you may have to back up a smidge)', null, @List1Id, @Category3Id, 130),
  ('Drop tailgate', null, @List1Id, @Category3Id, 140),
  ('Unhook breakaway cable and 7 pin connection from rear of truck', null, @List1Id, @Category3Id, 150),
  ('Pull truck forward slowly until it is out from under the camper.', null, @List1Id, @Category3Id, 160),
  ('Raise tailgate', null, @List1Id, @Category3Id, 170),
  ('Stow the 7 pin connector cable and breakaway cable up in the hitch', null, @List1Id, @Category3Id, 180),
  ('Press Auto-Level on leveling panel and RV will start moving all 4 stabilizers until it is level.  This can take 2-3 minutes.  You can watch status and abort or fix issues using the One Control app.', null, @List1Id, @Category3Id, 190),
  ('Install the X-chocks between tires on both sides to stabilize RV', null, @List1Id, @Category3Id, 200),
  ('Turn off breaker in the Electrical Panel and attach the Surge Guard to verify the circuit is OK.  Turn off breaker.', null, @List1Id, @Category3Id, 210),
  ('Plug 50 amp extension cord into Surge Guard and into RV.  Turn on breaker again and Surge Guard will re-verify circuit and connect the power', null, @List1Id, @Category3Id, 220),
  ('Use the Control Panel to extend the slides, turn-on water heater using the inside switch (gas AND electric), and put out awning', null, @List1Id, @Category3Id, 230),
  ('Spray city water spigot with bleach to clean, then hook up pressure regulator, then Y adapter, then the Zero-G water supply hose, then water filter, then flexible connector', null, @List1Id, @Category3Id, 240),
  ('Turn on water for a minute to push the air out of the hose before connecting to RV', null, @List1Id, @Category3Id, 250),
  ('Connect flexible connector to inlet on RV, then connect hose and filter', null, @List1Id, @Category3Id, 260),
  ('Set RV to City Water', null, @List1Id, @Category3Id, 270),
  ('Turn on water', null, @List1Id, @Category3Id, 280),
  ('If using water tank (not city water), turn on the water pump', null, @List1Id, @Category3Id, 290),
  ('Turn on propane ', null, @List1Id, @Category3Id, 300),
  ('Turn on the refrigerator to Auto', null, @List1Id, @Category3Id, 310),
  ('Add one scoop of Happy Camper to toilet along with 5 gallons of water', null, @List1Id, @Category3Id, 320)


Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('4-Pre-Depart Interior', 'Do these when you are ready to leave', 40, @Set1Id)
	SET @List4Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List4Id)
	SET @Category4Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('Plan route and fuel (gas stations, rest stops, send next location to phone for GPS in truck)', null, @List1Id, @Category4Id, 10),
  ('Wash dishes and pack away', null, @List1Id, @Category4Id, 20),
  ('Bedroom secure, closet doors latched, bathroom door latched open', null, @List1Id, @Category4Id, 30),
  ('Bedroom windows closed', null, @List1Id, @Category4Id, 40),
  ('Toilet - pour jug of water down toilet to loosen up any build-up during the drive back', null, @List1Id, @Category4Id, 50),
  ('Shower secure (everything off shelves, shower door locked closed)', null, @List1Id, @Category4Id, 60),
  ('Bathroom secure (vent closed, counter items stowed, main bathroom door shut)', null, @List1Id, @Category4Id, 70),
  ('Sweep, vacuum, and if necessary, mop floors and carpets; shake-out mats', null, @List1Id, @Category4Id, 80),
  ('Living room secure (carpets stowed, all drawers and cabinets closed)', null, @List1Id, @Category4Id, 90),
  ('Decorations off wall and stowed, coasters and table top items stowed', null, @List1Id, @Category4Id, 100),
  ('Refrigerator items secure', null, @List1Id, @Category4Id, 110),
  ('Refrigerator power selector to OFF for traveling', null, @List1Id, @Category4Id, 120),
  ('Refrigerator doors shut and latched', null, @List1Id, @Category4Id, 130),
  ('Outdoor stove range vent door closed ', null, @List1Id, @Category4Id, 140),
  ('Table and chairs secured with padding', null, @List1Id, @Category4Id, 150),
  ('Discard trash/recycling - both kitchen and bathroom', null, @List1Id, @Category4Id, 160),
  ('Pantry door closed', null, @List1Id, @Category4Id, 170),
  ('Windows closed / blinds up', null, @List1Id, @Category4Id, 180),
  ('Ceiling vents closed', null, @List1Id, @Category4Id, 190),
  ('If returning home: get laundry and clothes ready so you don''t have to open BR slide', null, @List1Id, @Category4Id, 200),
  ('If returning home: get items out of pantry so you don''t have to open LR slide', null, @List1Id, @Category4Id, 210),
  ('One-Control Items off (water pump, water heater, ALL Lights)', null, @List1Id, @Category4Id, 220),
  ('Purse, drinks, snacks all ready and put into truck', null, @List1Id, @Category4Id, 230),
  ('Final walkthrough - re-check doors, windows, surfaces', null, @List1Id, @Category4Id, 240),
  ('Retract awning', null, @List1Id, @Category4Id, 250),
  ('Slides: one last check for obstacles inside and out', null, @List1Id, @Category4Id, 260),
  ('Slides: Retract with door, vent, or window open for air flow', null, @List1Id, @Category4Id, 270)


Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('5-Pre-Depart Exterior', 'Do these when you are ready to leave', 40, @Set1Id)
	SET @List5Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List5Id)
	SET @Category5Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('Tire pressure checked', null, @List1Id, @Category5Id, 20),
  ('Fluid levels checked', null, @List1Id, @Category5Id, 30),
  ('Put spare key in truck', null, @List1Id, @Category5Id, 40),
  ('Clear outside roof on top of all slides is free of debris', null, @List1Id, @Category5Id, 50),
  ('Close all roof vents', null, @List1Id, @Category5Id, 60),
  ('TV antenna turned to transport position', null, @List1Id, @Category5Id, 70),
  ('Disconnect from television cable', null, @List1Id, @Category5Id, 80),
  ('Replace/store amenities (e.g. sweep and roll carpets, BBQ, chairs and tables)', null, @List1Id, @Category5Id, 90),
  ('When connecting stinky slinky, put in ground first, then connect to RV', null, @List1Id, @Category5Id, 100),
  ('Dump black tank', null, @List1Id, @Category5Id, 110),
  ('Flush/rinse black tank by adding water thru inlet with orange hose until water runs clear', null, @List1Id, @Category5Id, 120),
  ('Dump grey tanks and geo-treat (keep 5 gallons in grey tanks to slosh around)', null, @List1Id, @Category5Id, 130),
  ('Shut both valves, WAIT a few minutes, then disconnect and drain from RV towards sewer', null, @List1Id, @Category5Id, 140),
  ('Rinse out dump hose (using orange hose and hose fitting)', null, @List1Id, @Category5Id, 150),
  ('Put away stinky slinky in rear bumper', null, @List1Id, @Category5Id, 160),
  ('Treat Black Tank (???)', null, @List1Id, @Category5Id, 170),
  ('Wash/Disinfect hands', null, @List1Id, @Category5Id, 180),
  ('Turn off Water pump', null, @List1Id, @Category5Id, 190),
  ('Disconnect from Water', null, @List1Id, @Category5Id, 200),
  ('Connect both ends of the potable water hose together to keep them clean', null, @List1Id, @Category5Id, 210),
  ('Turn off water heater/furnace/air conditioner', null, @List1Id, @Category5Id, 220),
  ('Secure electrical and plumbing (water hose, power cord, poop hose)', null, @List1Id, @Category5Id, 230),
  ('Pull in slides', null, @List1Id, @Category5Id, 240),
  ('Disconnect from Power', null, @List1Id, @Category5Id, 250),
  ('Turn off propane', null, @List1Id, @Category5Id, 260),
  ('Outside storage bay door secured', null, @List1Id, @Category5Id, 270),
  ('Inspect site for left items, litter', null, @List1Id, @Category5Id, 280)


Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('6-Back Home', 'Do these when you are arrive back home', 40, @Set1Id)
	SET @List6Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List6Id)
	SET @Category6Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('Move trailer to home', null, @List1Id, @Category6Id, 10),
  ('Do arrival checklist (up to the point of hookups)', null, @List1Id, @Category6Id, 20),
  ('Put out slides or awning if put away wet', null, @List1Id, @Category6Id, 30),
  ('If dry-camping, empty fresh-water tank and refill the trailer with water', null, @List1Id, @Category6Id, 40),
  ('Remove food from fridge and any other perishables', null, @List1Id, @Category6Id, 50),
  ('Launder clothing/towels/bedding and return to trailer', null, @List1Id, @Category6Id, 60),
  ('Confirm that the fridge and propane are turned-off', null, @List1Id, @Category6Id, 70),
  ('Open fridge/freezer and confirm they are off, set lever so they don''t close', null, @List1Id, @Category6Id, 80),
  ('Top-off batteries (if necessary)', null, @List1Id, @Category6Id, 90),
  ('Wipe hitch clean and cover it', null, @List1Id, @Category6Id, 100),
  ('Lock tools away', null, @List1Id, @Category6Id, 110),
  ('Turn-off propane and turn the battery disconnect switch to off (in front bin)', null, @List1Id, @Category6Id, 120),
  ('Confirm everything is locked', null, @List1Id, @Category6Id, 130),
  ('Do a vehicle walk-around to make sure everything is secured', null, @List1Id, @Category6Id, 140)


Insert into CheckList(ListName, ListDscr, SortOrder, SetId) Values ('7-Maintenance', 'Do these periodically', 40, @Set1Id)
	SET @List7Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Main', @List7Id)
	SET @Category7Id = SCOPE_IDENTITY()

Insert into CheckAction(ActionText, ActionDscr, ListId, CategoryId, SortOrder) Values 
  ('2x/year - ¼ c Clorox for 15 gallons watter into fresh water tank (Freshen water; add and flush)', null, @List1Id, @Category7Id, 10),
  ('2x/year - UV retreat roof', null, @List1Id, @Category7Id, 20),
  ('2x/year - Vaseline toilet (prevents TP sticking)', null, @List1Id, @Category7Id, 30),
  ('3 year - Redo roof sealant (strip and replace ~$1000)', null, @List1Id, @Category7Id, 40),
  ('Frequent - Lube hitch ball', null, @List1Id, @Category7Id, 50),
  ('Inspect and clean roof membrane each month with one step cleaner and treatment like protectall', null, @List1Id, @Category7Id, 60),
  ('Slide Outs:   Keep clean and treated and lubricated.  Two seals - one on slide out and one on wall - side is only water right when fully in or out.  Put protectant spray on seals.', null, @List1Id, @Category7Id, 70),
  ('Axle lubricant: repack bearings and remove old grease every 12 months or 12K miles. ', null, @List1Id, @Category7Id, 80),
  ('Brakes: inspect and adjust every 3000 miles or annually (at least)', null, @List1Id, @Category7Id, 90),
  ('Clean in and UNDER your cabinet. Things will leak under the cabinet. Put a bead of silicone sealant around the cabinets to seal it up. ', null, @List1Id, @Category7Id, 100),
  ('AC: take cover off and clean foam filter and debris from entry. Consider adding screens over openings for A/C or water heater. ', null, @List1Id, @Category7Id, 110),
  ('Propane: have propane systems treated and checked annually by certified technician - pressure/leak down test. Tanks must be less than 12 years old.', null, @List1Id, @Category7Id, 120),
  ('Check water levels in batteries (1x/month or more) - add distilled water', null, @List1Id, @Category7Id, 130)


Select s.SetId, s.SetName, l.ListId, l.ListName, l.ListDscr, c.CategoryId, c.CategoryText, a.ActionId, a.ActionText, a.ActionDscr
From CheckSet s
INNER JOIN CheckList l on s.SetId = l.SetId
INNER JOIN CheckCategory c on c.ListId = l.ListId
INNER JOIN CheckAction a on a.CategoryId = c.CategoryId
Where s.SetName = @Set1Name

COMMIT

