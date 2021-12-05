
INSERT INTO InfluencerPlatforms(Created, IsDeleted,IsVerified, PlatformId, Url, InfluencerId)
SELECT TOP(70) GETDATE(), 0,1,2,'www.tiktok.com/@bachde.lachanh', AU.Id FROM ApplicationUsers AS AU
WHERE BrandId IS NULL AND NOT EXISTS
	(SELECT TOP(1) ID
	FROM InfluencerPlatforms AS IP
	WHERE IP.InfluencerId=AU.Id
	)