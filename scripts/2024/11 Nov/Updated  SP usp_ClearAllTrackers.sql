/****** Object:  StoredProcedure [dbo].[usp_ClearAllTrackers]    Script Date: 11/6/2024 12:44:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_ClearAllTrackers] (@userId NVARCHAR(450))
AS
BEGIN
    BEGIN TRY
        -- Start transaction if necessary
        BEGIN TRANSACTION;
        
        -- Delete all records from the Tracker table
        DELETE FROM Tracker WHERE UserId=@userId;

        -- Commit transaction if successful
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH

        -- If an error occurs, rollback transaction
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Declare variables to capture the error details
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        -- Assign the error information to variables
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Return or log the error details; here it is raised again
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
