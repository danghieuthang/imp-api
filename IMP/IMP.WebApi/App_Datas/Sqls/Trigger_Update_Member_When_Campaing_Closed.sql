CREATE TRIGGER before_campaign_close ON Campaigns FOR UPDATE AS
BEGIN
	UPDATE CampaignMembers
	SET Status=7 -- Status closed
	WHERE (Status!=2 AND Status!=4) -- Cancel and reject invite campaign member
		AND CampaignId IN (SELECT ID FROM inserted WHERE Status=9) -- Select id from updated campaign that have status = 9(Closed)
END