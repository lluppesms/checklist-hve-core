BEGIN TRAN


DECLARE 
    @OwnerName nvarchar(50) = 'lyle@luppes.com',
	@Set1Id int,
	@List1Id int,
	@Category1Id int,
	@Category2Id int,
	@Category3Id int,
	@Category4Id int,
	@SetName nvarchar(50) = 'RV Goddess - Setup Tasks'

DELETE FROM CheckSet Where SetName = @SetName

Insert into CheckSet(SetName, OwnerName) Values (@SetName, @OwnerName)
	SET @Set1Id = SCOPE_IDENTITY()
Insert into CheckList(ListName, SetId) Values ('Departure List', @Set1Id)
	SET @List1Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Interior', @List1Id)
	SET @Category1Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Passenger Area', @List1Id)
	SET @Category2Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('Exterior', @List1Id)
	SET @Category3Id = SCOPE_IDENTITY()
Insert into CheckCategory(CategoryText, ListId) Values ('At Departure', @List1Id)
	SET @Category4Id = SCOPE_IDENTITY()
Insert into CheckAction(ActionText, CategoryId, SortOrder, ListId) Values 
	('Items stored', @Category1Id, 10, @List1Id),
	('Cupboard doors closed', @Category1Id, 20, @List1Id),
	('Ceiling vents closed', @Category1Id, 30, @List1Id),
	('Windows closed', @Category1Id, 40, @List1Id),
	('TV/satellite dish down', @Category1Id, 50, @List1Id),
	('TV antenna down', @Category1Id, 60, @List1Id),
	('Refrigerator items secure', @Category1Id, 70, @List1Id),
	('Refrigerator power selected', @Category1Id, 80, @List1Id),
	('Water pump turned off', @Category1Id, 90, @List1Id),
	('Slide rooms in and secured', @Category1Id, 100, @List1Id),
	('Interior doors secured', @Category1Id, 110, @List1Id),
	('Turn off water heater/furnace/air conditioner', @Category1Id, 120, @List1Id),
	('Discard trash/recycling', @Category1Id, 130, @List1Id),

	('Drink/coffee cups filled', @Category2Id, 10, @List1Id),
	('Seat belts located', @Category2Id, 20, @List1Id),
	('Note mileage in Camping Journal', @Category2Id, 30, @List1Id),
	('Music/radio ready', @Category2Id, 40, @List1Id),
	('Maps/Route/GPS ready', @Category2Id, 50, @List1Id),
	('Kids games/books/music/videos/snacks ready', @Category2Id, 60, @List1Id),
	('Cell phone', @Category2Id, 70, @List1Id),
	('All keys accounted for', @Category2Id, 80, @List1Id),
	('Sunglasses', @Category2Id, 90, @List1Id),
	('Tissues/wet wipes', @Category2Id, 100, @List1Id),
	('Pets walked & happy', @Category2Id, 110, @List1Id),

	('Drain holding tanks', @Category3Id, 10, @List1Id),
	('Check fresh water tank level', @Category3Id, 20, @List1Id),
	('Power, water, sewer, cable, etc. unplugged from RV', @Category3Id, 30, @List1Id),
	('Propane', @Category3Id, 40, @List1Id),
	('Range vent secured', @Category3Id, 50, @List1Id),
	('Outside storage bay door secured/locked', @Category3Id, 60, @List1Id),
	('Raise leveling jacks', @Category3Id, 70, @List1Id),
	('Remove wheel blocks', @Category3Id, 80, @List1Id),
	('Windshields cleaned', @Category3Id, 90, @List1Id),
	('Tire pressure checked', @Category3Id, 100, @List1Id),
	('Fluid levels checked', @Category3Id, 110, @List1Id),
	('Entry steps stowed', @Category3Id, 120, @List1Id),

	('Safety inspection - turn signals, brakes, headlights', @Category4Id, 10, @List1Id),
	('RV walk-around: windows, slide rooms, roof antennas/vents, steps', @Category4Id, 20, @List1Id),
	('Tow vehicle walk-around, hitch inspection, safety inspection', @Category4Id, 30, @List1Id),
	('Inspect site for left items, litter', @Category4Id, 40, @List1Id)

Select s.SetId, s.SetName, l.ListId, l.ListName, l.ListDscr, c.CategoryId, c.CategoryText, a.ActionId, a.ActionText, a.ActionDscr
From CheckSet s
INNER JOIN CheckList l on s.SetId = l.SetId
INNER JOIN CheckCategory c on c.ListId = l.ListId
INNER JOIN CheckAction a on a.CategoryId = c.CategoryId
Where s.SetName = @SetName

COMMIT
-- ROLLBACK

