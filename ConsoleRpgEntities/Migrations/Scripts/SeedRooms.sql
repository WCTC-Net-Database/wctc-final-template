-- Seed initial rooms for the game world
-- This creates a simple 3x3 grid of rooms that students can expand

-- First, insert rooms without navigation (we'll update navigation in a second pass)
INSERT INTO Rooms (Name, Description, NorthRoomId, SouthRoomId, EastRoomId, WestRoomId) VALUES
('Town Square', 'A bustling town square with a fountain in the center. Merchants sell their wares and townsfolk go about their business.', NULL, NULL, NULL, NULL),
('Dark Forest', 'A dense forest where sunlight barely penetrates the canopy. Strange sounds echo through the trees.', NULL, NULL, NULL, NULL),
('Ancient Temple', 'An old temple with crumbling stone pillars. Mysterious symbols are carved into the walls.', NULL, NULL, NULL, NULL),
('Mountain Path', 'A narrow path winding up the mountainside. The air grows thin and cold.', NULL, NULL, NULL, NULL),
('Crystal Cave', 'A sparkling cave filled with luminescent crystals that cast an eerie blue glow.', NULL, NULL, NULL, NULL),
('Abandoned Mine', 'An old mine shaft with rusty rails and broken carts. It smells of damp earth and decay.', NULL, NULL, NULL, NULL),
('Riverside Camp', 'A peaceful camp by the river. The sound of flowing water is soothing.', NULL, NULL, NULL, NULL),
('Goblin Lair', 'A foul-smelling cave filled with bones and refuse. Goblins have made this their home.', NULL, NULL, NULL, NULL),
('Wizard Tower', 'A tall tower that pierces the clouds. Magical energy crackles in the air.', NULL, NULL, NULL, NULL);

-- Now update the navigation links to create a 3x3 grid
-- Layout:
-- [1] Town Square    [2] Dark Forest     [3] Ancient Temple
-- [4] Mountain Path  [5] Crystal Cave    [6] Abandoned Mine
-- [7] Riverside Camp [8] Goblin Lair     [9] Wizard Tower

-- Row 1: Town Square, Dark Forest, Ancient Temple
UPDATE Rooms SET EastRoomId = 2, SouthRoomId = 4 WHERE Id = 1;  -- Town Square
UPDATE Rooms SET WestRoomId = 1, EastRoomId = 3, SouthRoomId = 5 WHERE Id = 2;  -- Dark Forest
UPDATE Rooms SET WestRoomId = 2, SouthRoomId = 6 WHERE Id = 3;  -- Ancient Temple

-- Row 2: Mountain Path, Crystal Cave, Abandoned Mine
UPDATE Rooms SET NorthRoomId = 1, EastRoomId = 5, SouthRoomId = 7 WHERE Id = 4;  -- Mountain Path
UPDATE Rooms SET NorthRoomId = 2, WestRoomId = 4, EastRoomId = 6, SouthRoomId = 8 WHERE Id = 5;  -- Crystal Cave
UPDATE Rooms SET NorthRoomId = 3, WestRoomId = 5, SouthRoomId = 9 WHERE Id = 6;  -- Abandoned Mine

-- Row 3: Riverside Camp, Goblin Lair, Wizard Tower
UPDATE Rooms SET NorthRoomId = 4, EastRoomId = 8 WHERE Id = 7;  -- Riverside Camp
UPDATE Rooms SET NorthRoomId = 5, WestRoomId = 7, EastRoomId = 9 WHERE Id = 8;  -- Goblin Lair
UPDATE Rooms SET NorthRoomId = 6, WestRoomId = 8 WHERE Id = 9;  -- Wizard Tower

-- Place some monsters in rooms
UPDATE Monsters SET RoomId = 2 WHERE Id = 1;  -- Place first goblin in Dark Forest
UPDATE Monsters SET RoomId = 8 WHERE Id = 2;  -- Place second goblin in Goblin Lair (if exists)

-- Place the first player in Town Square
UPDATE Players SET RoomId = 1 WHERE Id = 1;  -- Place first player in Town Square
