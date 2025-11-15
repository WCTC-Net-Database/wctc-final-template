-- Rollback script for SeedRooms
-- Remove all seeded room data

-- First, remove room assignments from monsters and players
UPDATE Monsters SET RoomId = NULL WHERE RoomId IS NOT NULL;
UPDATE Players SET RoomId = NULL WHERE RoomId IS NOT NULL;

-- Delete all seeded rooms
DELETE FROM Rooms WHERE Id IN (1, 2, 3, 4, 5, 6, 7, 8, 9);
