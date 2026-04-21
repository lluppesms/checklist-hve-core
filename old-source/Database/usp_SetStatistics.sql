ALTER PROCEDURE usp_SetStatistics (
  @UserName    nvarchar(128),
  @SetId       int
)
AS
/*
 EXEC usp_SetStatistics @UserName = 'lyle@Luppes.com', @SetId = 2
 EXEC usp_SetStatistics @UserName = 'lyle@Luppes.com', @SetId = 3
*/
BEGIN

	DECLARE @Stats Table (
	  ListId           int,
	  Completed        int,
	  Total            int,
	  PercentComplete  decimal(8,2)
	)

	INSERT INTO @Stats
	SELECT  l.ListId,
	  SUM(CASE WHEN a.CompleteInd = 'Y' THEN 1 ELSE 0 END) as Completed,
	  COUNT(*) as Total,
	  CASE WHEN COUNT(*) > 0 THEN SUM(CASE WHEN a.CompleteInd = 'Y' THEN 1 ELSE 0 END) / CAST(COUNT(*) as DECIMAL) ELSE 0 END * CAST(100 as Decimal) as PercentComplete
	FROM CheckAction a
	INNER JOIN CheckList l on a.ListId = l.ListId
	WHERE l.SetId = @SetId
	GROUP BY l.ListId

	SELECT l.ListId, l.ListName, l.ListDscr, l.ActiveInd, l.SortOrder,
		s.SetId, s.SetName, s.SetDscr,
		t.Completed, t.Total, t.PercentComplete
	FROM CheckList l
	INNER JOIN CheckSet s on l.SetId = s.SetId
	INNER JOIN @Stats t on l.ListId = t.ListId
	WHERE l.SetId = @SetId

END
GO