update [IMP_Identity].[Identity].[User]
set BrandId=(SELECT BrandId FROM [IMP_Application].[dbo].[ApplicationUsers] WHERE Id=ApplicationUserId)
WHERE 0<(SELECT BrandId FROM [IMP_Application].[dbo].[ApplicationUsers] WHERE Id=ApplicationUserId AND BrandId!=null)
