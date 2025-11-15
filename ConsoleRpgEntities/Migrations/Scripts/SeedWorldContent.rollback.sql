-- =============================================
-- Rollback Script for World Content Seeding
-- This removes all seeded content added by SeedWorldContent
-- =============================================

PRINT 'Rolling back world content...';

-- Remove PlayerAbilities associations
DELETE FROM PlayerAbilities
WHERE PlayersId IN (
    SELECT Id FROM Players
    WHERE Name IN (
        'Sir Aldric the Brave',
        'Mystara the Wise',
        'Shadow the Silent',
        'Thorin Ironshield',
        'Elara Swiftarrow',
        'Captain Reginald',
        'Tom the Hopeful'
    )
);

-- Remove Players
DELETE FROM Players
WHERE Name IN (
    'Sir Aldric the Brave',
    'Mystara the Wise',
    'Shadow the Silent',
    'Thorin Ironshield',
    'Elara Swiftarrow',
    'Captain Reginald',
    'Tom the Hopeful'
);

-- Remove Monsters
DELETE FROM Monsters
WHERE Name IN (
    'Goblin Warrior',
    'Goblin Scout',
    'Goblin Shaman',
    'Gray Wolf',
    'Alpha Wolf',
    'Ancient Skeleton',
    'Cursed Scholar',
    'Bandit Thug',
    'Bandit Leader',
    'Training Dummy'
);

-- Remove Equipment sets
DELETE FROM Equipments
WHERE WeaponId IN (
    SELECT Id FROM Items
    WHERE Name IN (
        'Rusty Sword',
        'Steel Longsword',
        'Battle Axe',
        'Iron Dagger',
        'Wooden Staff',
        'Longbow',
        'War Hammer'
    )
)
OR ArmorId IN (
    SELECT Id FROM Items
    WHERE Name IN (
        'Leather Armor',
        'Chain Mail',
        'Plate Armor',
        'Mage Robes',
        'Wooden Shield',
        'Iron Shield'
    )
);

-- Remove Items
DELETE FROM Items
WHERE Name IN (
    'Rusty Sword',
    'Steel Longsword',
    'Battle Axe',
    'Iron Dagger',
    'Wooden Staff',
    'Longbow',
    'War Hammer',
    'Leather Armor',
    'Chain Mail',
    'Plate Armor',
    'Mage Robes',
    'Wooden Shield',
    'Iron Shield'
);

-- Remove Abilities (but keep ShoveAbility if it was there originally)
DELETE FROM Abilities
WHERE Name IN (
    'Fireball',
    'Heal',
    'Stealth Strike',
    'Power Attack',
    'Shield Bash'
);

-- Only remove Shove if there are no references to it
IF NOT EXISTS (SELECT 1 FROM PlayerAbilities WHERE AbilitiesId = (SELECT Id FROM Abilities WHERE Name = 'Shove'))
BEGIN
    DELETE FROM Abilities WHERE Name = 'Shove';
END

PRINT 'World content rollback completed successfully!';
