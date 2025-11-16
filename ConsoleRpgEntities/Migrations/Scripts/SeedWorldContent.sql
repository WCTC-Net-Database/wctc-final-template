-- =============================================
-- Comprehensive World Content Seeding Script
-- This populates the RPG world with diverse content
-- =============================================

-- Variables to store IDs
DECLARE @TownSquareId INT = (SELECT Id FROM Rooms WHERE Name = 'Town Square');
DECLARE @TavernId INT = (SELECT Id FROM Rooms WHERE Name = 'The Prancing Pony Tavern');
DECLARE @TrainingGroundsId INT = (SELECT Id FROM Rooms WHERE Name = 'Training Grounds');
DECLARE @ForestEdgeId INT = (SELECT Id FROM Rooms WHERE Name = 'Forest Edge');
DECLARE @DeepForestId INT = (SELECT Id FROM Rooms WHERE Name = 'Deep Darkwood');
DECLARE @LibraryId INT = (SELECT Id FROM Rooms WHERE Name = 'Ancient Library');
DECLARE @EasternMarketId INT = (SELECT Id FROM Rooms WHERE Name = 'Eastern Market');

-- =============================================
-- SEED ABILITIES
-- =============================================
PRINT 'Seeding Abilities...';

-- Shove Ability (might already exist, check first)
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Shove')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Shove', 'ShoveAbility', 'Push an enemy backward with force', 5, 3);
END

-- Fireball Ability
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Fireball')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Fireball', 'ShoveAbility', 'Hurl a ball of flame at your enemy', 10, 15);
END

-- Heal Ability
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Heal')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Heal', 'ShoveAbility', 'Restore health to yourself or an ally', 5, -10);
END

-- Stealth Strike
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Stealth Strike')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Stealth Strike', 'ShoveAbility', 'Attack from the shadows with deadly precision', 3, 12);
END

-- Power Attack
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Power Attack')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Power Attack', 'ShoveAbility', 'A devastating melee attack', 2, 18);
END

-- Shield Bash
IF NOT EXISTS (SELECT 1 FROM Abilities WHERE Name = 'Shield Bash')
BEGIN
    INSERT INTO Abilities (Name, AbilityType, Description, Distance, Damage)
    VALUES ('Shield Bash', 'ShoveAbility', 'Stun your enemy with your shield', 2, 8);
END

-- =============================================
-- SEED ITEMS (Weapons and Armor)
-- =============================================
PRINT 'Seeding Items...';

-- Weapons
IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Rusty Sword')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Rusty Sword', 'Weapon', 5, 0, 4.50, 10);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Steel Longsword')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Steel Longsword', 'Weapon', 12, 0, 6.00, 50);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Battle Axe')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Battle Axe', 'Weapon', 15, 0, 7.50, 65);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Iron Dagger')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Iron Dagger', 'Weapon', 7, 0, 1.20, 15);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Wooden Staff')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Wooden Staff', 'Weapon', 8, 0, 2.00, 20);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Longbow')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Longbow', 'Weapon', 10, 0, 2.80, 35);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'War Hammer')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('War Hammer', 'Weapon', 16, 0, 8.00, 70);
END

-- Armor
IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Leather Armor')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Leather Armor', 'Armor', 0, 5, 5.00, 25);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Chain Mail')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Chain Mail', 'Armor', 0, 10, 10.00, 60);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Plate Armor')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Plate Armor', 'Armor', 0, 15, 15.00, 120);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Mage Robes')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Mage Robes', 'Armor', 0, 3, 2.00, 40);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Wooden Shield')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Wooden Shield', 'Armor', 0, 4, 4.00, 18);
END

IF NOT EXISTS (SELECT 1 FROM Items WHERE Name = 'Iron Shield')
BEGIN
    INSERT INTO Items (Name, Type, Attack, Defense, Weight, Value)
    VALUES ('Iron Shield', 'Armor', 0, 8, 7.00, 45);
END

-- =============================================
-- SEED EQUIPMENT SETS
-- =============================================
PRINT 'Seeding Equipment...';

-- Get Item IDs for equipment creation
DECLARE @RustySwordId INT = (SELECT Id FROM Items WHERE Name = 'Rusty Sword');
DECLARE @SteelSwordId INT = (SELECT Id FROM Items WHERE Name = 'Steel Longsword');
DECLARE @BattleAxeId INT = (SELECT Id FROM Items WHERE Name = 'Battle Axe');
DECLARE @DaggerId INT = (SELECT Id FROM Items WHERE Name = 'Iron Dagger');
DECLARE @StaffId INT = (SELECT Id FROM Items WHERE Name = 'Wooden Staff');
DECLARE @BowId INT = (SELECT Id FROM Items WHERE Name = 'Longbow');
DECLARE @HammerId INT = (SELECT Id FROM Items WHERE Name = 'War Hammer');

DECLARE @LeatherArmorId INT = (SELECT Id FROM Items WHERE Name = 'Leather Armor');
DECLARE @ChainMailId INT = (SELECT Id FROM Items WHERE Name = 'Chain Mail');
DECLARE @PlateArmorId INT = (SELECT Id FROM Items WHERE Name = 'Plate Armor');
DECLARE @RobesId INT = (SELECT Id FROM Items WHERE Name = 'Mage Robes');
DECLARE @WoodenShieldId INT = (SELECT Id FROM Items WHERE Name = 'Wooden Shield');
DECLARE @IronShieldId INT = (SELECT Id FROM Items WHERE Name = 'Iron Shield');

-- Warrior Equipment (Sword + Chain Mail)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@SteelSwordId, @ChainMailId);
DECLARE @WarriorEquipId INT = SCOPE_IDENTITY();

-- Tank Equipment (Hammer + Plate Armor)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@HammerId, @PlateArmorId);
DECLARE @TankEquipId INT = SCOPE_IDENTITY();

-- Rogue Equipment (Dagger + Leather Armor)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@DaggerId, @LeatherArmorId);
DECLARE @RogueEquipId INT = SCOPE_IDENTITY();

-- Mage Equipment (Staff + Robes)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@StaffId, @RobesId);
DECLARE @MageEquipId INT = SCOPE_IDENTITY();

-- Ranger Equipment (Bow + Leather Armor)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@BowId, @LeatherArmorId);
DECLARE @RangerEquipId INT = SCOPE_IDENTITY();

-- Knight Equipment (Battle Axe + Plate Armor)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@BattleAxeId, @PlateArmorId);
DECLARE @KnightEquipId INT = SCOPE_IDENTITY();

-- Beginner Equipment (Rusty Sword + Wooden Shield)
INSERT INTO Equipments (WeaponId, ArmorId) VALUES (@RustySwordId, @WoodenShieldId);
DECLARE @BeginnerEquipId INT = SCOPE_IDENTITY();

-- =============================================
-- SEED PLAYERS
-- =============================================
PRINT 'Seeding Players...';

-- Warrior in Training Grounds
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Sir Aldric the Brave', 120, 150, @WarriorEquipId, @TrainingGroundsId);
DECLARE @AldricId INT = SCOPE_IDENTITY();

-- Mage in Library
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Mystara the Wise', 80, 200, @MageEquipId, @LibraryId);
DECLARE @MystaraId INT = SCOPE_IDENTITY();

-- Rogue in Tavern
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Shadow the Silent', 90, 175, @RogueEquipId, @TavernId);
DECLARE @ShadowId INT = SCOPE_IDENTITY();

-- Tank in Town Square
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Thorin Ironshield', 150, 180, @TankEquipId, @TownSquareId);
DECLARE @ThorinId INT = SCOPE_IDENTITY();

-- Ranger in Eastern Market
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Elara Swiftarrow', 95, 160, @RangerEquipId, @EasternMarketId);
DECLARE @ElaraId INT = SCOPE_IDENTITY();

-- Knight in Town Square
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Captain Reginald', 130, 220, @KnightEquipId, @TownSquareId);
DECLARE @ReginaldId INT = SCOPE_IDENTITY();

-- Novice Adventurer in Tavern
INSERT INTO Players (Name, Health, Experience, EquipmentId, RoomId)
VALUES ('Tom the Hopeful', 70, 50, @BeginnerEquipId, @TavernId);
DECLARE @TomId INT = SCOPE_IDENTITY();


-- =============================================
-- ASSIGN ABILITIES TO PLAYERS
-- =============================================
PRINT 'Assigning Abilities to Players...';

-- Get Ability IDs
DECLARE @ShoveId INT = (SELECT Id FROM Abilities WHERE Name = 'Shove');
DECLARE @FireballId INT = (SELECT Id FROM Abilities WHERE Name = 'Fireball');
DECLARE @HealId INT = (SELECT Id FROM Abilities WHERE Name = 'Heal');
DECLARE @StealthId INT = (SELECT Id FROM Abilities WHERE Name = 'Stealth Strike');
DECLARE @PowerAttackId INT = (SELECT Id FROM Abilities WHERE Name = 'Power Attack');
DECLARE @ShieldBashId INT = (SELECT Id FROM Abilities WHERE Name = 'Shield Bash');

-- Sir Aldric (Warrior) - Power Attack, Shove
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@AldricId, @PowerAttackId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@AldricId, @ShoveId);

-- Mystara (Mage) - Fireball, Heal
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@MystaraId, @FireballId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@MystaraId, @HealId);

-- Shadow (Rogue) - Stealth Strike, Shove
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ShadowId, @StealthId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ShadowId, @ShoveId);

-- Thorin (Tank) - Shield Bash, Power Attack
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ThorinId, @ShieldBashId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ThorinId, @PowerAttackId);

-- Elara (Ranger) - Stealth Strike
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ElaraId, @StealthId);

-- Captain Reginald (Knight) - Power Attack, Shield Bash, Shove
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ReginaldId, @PowerAttackId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ReginaldId, @ShieldBashId);
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@ReginaldId, @ShoveId);

-- Tom (Novice) - Shove only
INSERT INTO PlayerAbilities (PlayersId, AbilitiesId) VALUES (@TomId, @ShoveId);

-- =============================================
-- SEED MONSTERS
-- =============================================
PRINT 'Seeding Monsters...';

-- Goblins in Deep Darkwood
INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Goblin Warrior', 40, 8, 'Goblin', @DeepForestId);

INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Goblin Scout', 30, 6, 'Goblin', @DeepForestId);

INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Goblin Shaman', 35, 10, 'Goblin', @DeepForestId);

-- Wolves near Forest Edge
INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Gray Wolf', 45, 7, 'Goblin', @ForestEdgeId);

INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Alpha Wolf', 60, 9, 'Goblin', @ForestEdgeId);

-- Skeleton in Ancient Library
INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Ancient Skeleton', 50, 8, 'Goblin', @LibraryId);

INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Cursed Scholar', 40, 6, 'Goblin', @LibraryId);

-- Bandits near Market
INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Bandit Thug', 55, 7, 'Goblin', @EasternMarketId);

INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Bandit Leader', 70, 10, 'Goblin', @EasternMarketId);

-- Training Dummy (Low aggression for practice)
INSERT INTO Monsters (Name, Health, AggressionLevel, MonsterType, RoomId)
VALUES ('Training Dummy', 100, 0, 'Goblin', @TrainingGroundsId);

PRINT 'World content seeding completed successfully!';
PRINT '================================================';
PRINT 'Summary:';
PRINT '- 6 Abilities created';
PRINT '- 13 Items created (7 weapons, 6 armor)';
PRINT '- 7 Equipment sets created';
PRINT '- 7 Players created and placed in rooms';
PRINT '- 15+ Ability assignments to players';
PRINT '- 10 Monsters created and placed in rooms';
PRINT '================================================';
