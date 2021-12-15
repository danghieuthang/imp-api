CREATE PROCEDURE DeleteCampaign
	@id int
AS 
BEGIN
	UPDATE Campaigns 
	SET IsDeleted=1 
	WHERE ID=@id
END
