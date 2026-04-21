-- Update CheckAction Set CompleteInd = 'Y' Where ActionId IN (25,27,29,31,33,35)
-- Update CheckAction Set CompleteInd = 'N' Where CompleteInd  IS NULL
--select * From CheckSet
--select * From CheckList
--select * From CheckCategory
--select * From CheckAction
-- Update CheckList Set ListName = 'List 2' where listId = 6

select s.SetId, s.SetName, l.ListId, l.ListName, c.CategoryText, a.* From CheckAction a
INNER JOIN CheckCategory c on a.CategoryId = c.CategoryId
INNER JOIN CheckList l on c.ListId = l.ListId
INNER JOIN CheckSet s on l.SetId = s.SetId
Order by s.SortOrder, s.SetName, l.SortOrder, l.ListName, c.SortOrder, c.CategoryText, a.SortOrder, a.ActionText
