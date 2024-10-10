USE [ToDoListTracker]
GO

/****** Object:  StoredProcedure [dbo].[usp_ClearAllTrackers]    Script Date: 10/10/2024 4:48:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ClearAllTrackers]
AS
BEGIN
    BEGIN TRY
        -- Start transaction if necessary
        BEGIN TRANSACTION;
        
        -- Delete all records from the Tracker table
        DELETE FROM Tracker;

        -- Reset identity column (reseeds to 0)
        DBCC CHECKIDENT('Tracker', RESEED, 0);

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
GO


