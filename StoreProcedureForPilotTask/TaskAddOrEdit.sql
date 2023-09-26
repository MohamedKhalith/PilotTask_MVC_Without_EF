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
CREATE PROCEDURE [dbo].[TaskAddOrEdit]
	-- Add the parameters for the stored procedure here
	@Id INT,
	@ProfileId INT,
	@TaskName VARCHAR(100),
	@TaskDescription VARCHAR(100),
	@StartTime dateTime,
	@Status INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Id=0
	BEGIN
		INSERT INTO Task(ProfileId,TaskName,TaskDescription,StartTime,Status)
		VALUES(@ProfileId,@TaskName,@TaskDescription,@StartTime,@Status)
	END
	ELSE
	BEGIN
		UPDATE Task
		SET 
			ProfileId=@ProfileId,
			TaskName=@TaskName,
			TaskDescription=@TaskDescription,
			StartTime=@StartTime,
			Status=@Status
		WHERE Id =@Id
	END
END
GO
