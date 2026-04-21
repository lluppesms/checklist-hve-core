DELETE FROM CheckSet
DBCC CHECKIDENT(CheckSet, Reseed, 0)
DBCC CHECKIDENT(CheckList, Reseed, 0)
DBCC CHECKIDENT(CheckCategory, Reseed, 0)
DBCC CHECKIDENT(CheckAction, Reseed, 0)
