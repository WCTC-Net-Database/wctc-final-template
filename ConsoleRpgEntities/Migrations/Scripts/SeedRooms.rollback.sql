-- Rollback script for SeedRooms migration
-- This removes all seeded rooms

-- First, remove any room associations from monsters
UPDATE Monsters SET RoomId = NULL WHERE RoomId IS NOT NULL;

-- Remove any room associations from players
UPDATE Players SET RoomId = NULL WHERE RoomId IS NOT NULL;

-- Delete all rooms (in order to handle foreign key constraints)
-- We need to first break the self-referencing foreign keys
UPDATE Rooms SET NorthRoomId = NULL, SouthRoomId = NULL, EastRoomId = NULL, WestRoomId = NULL;

-- Now delete all rooms
DELETE FROM Rooms;

-- Reset identity seed
DBCC CHECKIDENT ('Rooms', RESEED, 0);
