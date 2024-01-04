CREATE TRIGGER [LimitRecom]
	 ON Recommandations AFTER INSERT
    AS
    BEGIN
        ;WITH NumberedRows AS (
            SELECT Id, Id_User, Id_Category, ROW_NUMBER() OVER (PARTITION BY Id_User ORDER BY Id DESC) AS RowNum
            FROM Recommandations
        )
        DELETE FROM NumberedRows
        WHERE RowNum > 10
    END