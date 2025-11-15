-- Seed initial rooms for the RPG world
-- This creates a small starting world that students can expand upon

-- Room 1: Town Square (Center of the world)
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Town Square', 'A bustling town square filled with merchants and travelers. Stone fountains decorate the corners, and a large notice board stands in the center.', 0, 0, NULL, NULL, NULL, NULL);

DECLARE @TownSquareId INT = SCOPE_IDENTITY();

-- Room 2: Northern Gate
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Northern Gate', 'A massive wooden gate reinforced with iron bars guards the northern entrance to the town. Guards patrol the ramparts above.', 0, 1, NULL, @TownSquareId, NULL, NULL);

DECLARE @NorthernGateId INT = SCOPE_IDENTITY();

-- Room 3: Eastern Market
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Eastern Market', 'Colorful stalls line the streets, selling exotic goods from distant lands. The smell of spices and fresh bread fills the air.', 1, 0, NULL, NULL, NULL, @TownSquareId);

DECLARE @EasternMarketId INT = SCOPE_IDENTITY();

-- Room 4: Western Tavern
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('The Prancing Pony Tavern', 'A warm and welcoming tavern with a roaring fireplace. Adventurers share tales of their exploits over mugs of ale.', -1, 0, NULL, NULL, @TownSquareId, NULL);

DECLARE @WesternTavernId INT = SCOPE_IDENTITY();

-- Room 5: Southern Forest Edge
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Forest Edge', 'The dense Darkwood Forest begins here. Ancient trees loom overhead, their branches creating an ominous canopy.', 0, -1, @TownSquareId, NULL, NULL, NULL);

DECLARE @ForestEdgeId INT = SCOPE_IDENTITY();

-- Room 6: Deep Forest (dangerous area with potential monsters)
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Deep Darkwood', 'The heart of the forest is dark and foreboding. Strange sounds echo through the trees, and you feel like you are being watched.', 0, -2, @ForestEdgeId, NULL, NULL, NULL);

DECLARE @DeepForestId INT = SCOPE_IDENTITY();

-- Room 7: Training Grounds
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Training Grounds', 'An open field where warriors practice their combat skills. Training dummies and weapon racks are scattered about.', 1, 1, NULL, NULL, NULL, NULL);

DECLARE @TrainingGroundsId INT = SCOPE_IDENTITY();

-- Room 8: Old Library
INSERT INTO Rooms (Name, Description, X, Y, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId)
VALUES ('Ancient Library', 'Towering bookshelves filled with dusty tomes and ancient scrolls. A musty smell permeates the air.', -1, 1, NULL, NULL, NULL, NULL);

DECLARE @LibraryId INT = SCOPE_IDENTITY();

-- Now update the Town Square with exits to all adjacent rooms
UPDATE Rooms
SET NorthRoomId = @NorthernGateId,
    SouthRoomId = @ForestEdgeId,
    EastRoomId = @EasternMarketId,
    WestRoomId = @WesternTavernId
WHERE Id = @TownSquareId;

-- Update Northern Gate with connection to Training Grounds (East)
UPDATE Rooms
SET EastRoomId = @TrainingGroundsId,
    WestRoomId = @LibraryId
WHERE Id = @NorthernGateId;

-- Update Training Grounds with connection to Northern Gate (West)
UPDATE Rooms
SET WestRoomId = @NorthernGateId,
    SouthRoomId = @EasternMarketId
WHERE Id = @TrainingGroundsId;

-- Update Library with connection to Northern Gate (East)
UPDATE Rooms
SET EastRoomId = @NorthernGateId,
    SouthRoomId = @WesternTavernId
WHERE Id = @LibraryId;

-- Update Eastern Market with connection to Training Grounds (North)
UPDATE Rooms
SET NorthRoomId = @TrainingGroundsId
WHERE Id = @EasternMarketId;

-- Update Western Tavern with connection to Library (North)
UPDATE Rooms
SET NorthRoomId = @LibraryId
WHERE Id = @WesternTavernId;

-- Update Forest Edge with connection to Deep Forest (South)
UPDATE Rooms
SET SouthRoomId = @DeepForestId
WHERE Id = @ForestEdgeId;

-- Place a Goblin in the Deep Darkwood (if any exist)
IF EXISTS (SELECT 1 FROM Monsters WHERE MonsterType = 'Goblin')
BEGIN
    UPDATE TOP(1) Monsters
    SET RoomId = @DeepForestId
    WHERE MonsterType = 'Goblin' AND RoomId IS NULL;
END
