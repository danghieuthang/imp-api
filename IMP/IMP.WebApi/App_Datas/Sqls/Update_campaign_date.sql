EXEC UpdateCampaignStatus 377,6


-- Hôm nay là ngày quảng cáo
Update Campaigns
Set OpeningDate=DATEADD(DAY, -2, dbo.GetToDate()), ApplyingDate=DATEADD(DAY, -1, dbo.GetToDate()), AdvertisingDate = dbo.GetToDate()
WHERE ID = 377

-- Hôm nay là ngày thông báo
Update Campaigns
Set OpeningDate=DATEADD(DAY, -2, dbo.GetToDate()),
	ApplyingDate=DATEADD(DAY, -1, dbo.GetToDate()), 
	ApplyingDate=DATEADD(DAY, -1, dbo.GetToDate()), 
	AnnouncingDate = dbo.GetToDate()
WHERE ID = 377

SELECT GetUtcDate()

SELECT dbo.GetToDate()
