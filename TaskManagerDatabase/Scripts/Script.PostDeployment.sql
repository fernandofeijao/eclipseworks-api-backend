IF NOT EXISTS(SELECT 1
FROM [USER]
WHERE [Profile] = 1)
BEGIN
    INSERT INTO [USER]
        ([Id], [Name], [Profile])
    VALUES
        ('fernando.feijao', 'Fernando Feijão', 1);
END
GO

IF NOT EXISTS(SELECT 1
FROM [USER]
WHERE [Profile] = 2)
BEGIN
    INSERT INTO [USER]
        ([Id], [Name], [Profile])
    VALUES
        ('thamires.silva', 'Thamires Silva', 2);
END
GO

