-- Unit Type Queries
-- Get all unit types
SELECT id, name, acronym 
FROM [CakePrize].[dbo].[unit_type]

-- Get unit type by ID
SELECT id, name, acronym 
FROM [CakePrize].[dbo].[unit_type] 
WHERE id = @UnitTypeId

-- Insert new unit type
INSERT INTO [CakePrize].[dbo].[unit_type] (id, name, acronym)
VALUES (@Id, @Name, @Acronym)
