-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ProfileAddOrEdit]
	-- Add the parameters for the stored procedure here
	@ProfileId INT,
	@FirstName VARCHAR(100),
	@LastName VARCHAR(100),
	@DateOfBirth DATETIME,
	@PhoneNumber VARCHAR(100),
	@EmailId VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ProfileId=0
	BEGIN
		INSERT INTO Profile(FirstName,LastName,DateOfBirth,PhoneNumber,EmailId)
		VALUES(@FirstName,@LastName,@DateOfBirth,@PhoneNumber,@EmailId)
	END
	ELSE
	BEGIN
		UPDATE Profile
		SET 
			FirstName=@FirstName,
			LastName=@LastName,
			DateOfBirth=@DateOfBirth,
			PhoneNumber=@PhoneNumber,
			EmailId=@EmailId
		WHERE ProfileId =@ProfileId
	END
END
GO
