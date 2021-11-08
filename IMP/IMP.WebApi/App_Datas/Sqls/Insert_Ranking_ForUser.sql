
INSERT INTO Rankings(Created, IsDeleted, RankLevelId, InfluencerId, Score)
SELECT GETDATE(),0, 1, Id, 0
FROM ApplicationUsers AS A
WHERE 0=(SELECT COUNT(ID) FROM Rankings AS R WHERE R.InfluencerId=A.ID)