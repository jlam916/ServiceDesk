DECLARE @TestStartDate As DateTime
DECLARE @TestEndDate As DateTime

SET @TestStartDate = '2015-07-31 07:00:00 PM'
SET @TestEndDate = '2015-07-31 10:00:00 PM'

Select TOP 1 * FROM Presentation p
WHERE p.id NOT IN (
	Select PresentationID FROM Reservation r
	WHERE (@TestStartDate BETWEEN r.StartDate AND r.EndDate) OR
		  (@TestEndDate BETWEEN r.StartDate AND r.EndDate) OR
		  (r.StartDate BETWEEN @TestStartDate AND @TestEndDate) OR
		  (r.EndDate BETWEEN @TestStartDate AND @TestEndDate)
)

INSERT INTO Reservation(@TestStartDate, @TestEndDate, Id)