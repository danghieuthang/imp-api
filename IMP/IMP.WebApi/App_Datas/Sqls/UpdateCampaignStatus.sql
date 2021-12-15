
CREATE PROCEDURE UpdateCampaignStatus
	@id int,
	@status int
AS
	BEGIN
		UPDATE Campaigns
		SET Status=@status
		WHERE Id=@id
	END
GO