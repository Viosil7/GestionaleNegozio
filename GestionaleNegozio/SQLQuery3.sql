CREATE TRIGGER InitializeInventory
ON Negozi
AFTER INSERT
AS
BEGIN
    INSERT INTO Magazzino (idNegozio, idProdotto, quantità)
    SELECT i.Id, p.Id, 0
    FROM inserted i
    CROSS JOIN Prodotti p
END
GO
