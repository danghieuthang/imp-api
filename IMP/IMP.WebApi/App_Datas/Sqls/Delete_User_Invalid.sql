DELETE [Identity].RefreshTokens
WHERE UserId in (SELECT ID FROM [Identity].[User] WHERE ApplicationUserId is null)
GO

DELETE [IMP_Identity].[Identity].[User]
WHERE ApplicationUserId is null
GO

-- Delete application user
DELETE [IMP_Application].dbo.Rankings
WHERE InfluencerId NOT IN (SELECT ApplicationUserId FROM [IMP_Identity].[Identity].[User])
GO

DELETE [IMP_Application].dbo.Wallets
WHERE ApplicationUserId NOT IN (SELECT ApplicationUserId FROM [IMP_Identity].[Identity].[User])
GO

DELETE [IMP_Application].dbo.ApplicationUsers
WHERE ID NOT IN (SELECT ApplicationUserId FROM [IMP_Identity].[Identity].[User])