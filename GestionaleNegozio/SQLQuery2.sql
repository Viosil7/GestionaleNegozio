SELECT * INTO #TempOrdini FROM Ordini;

DROP TABLE Ordini;

CREATE TABLE Ordini (
    Id INT PRIMARY KEY IDENTITY(1,1),
    idNegozio INT NOT NULL,
    nota NVARCHAR(MAX),
    dataOrdine DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (idNegozio) REFERENCES Negozi(Id)
);

CREATE TABLE DettagliOrdine (
    Id INT PRIMARY KEY IDENTITY(1,1),
    idOrdine INT NOT NULL,
    idProdotto INT NOT NULL,
    quantita INT NOT NULL,
    FOREIGN KEY (idOrdine) REFERENCES Ordini(Id),
    FOREIGN KEY (idProdotto) REFERENCES Prodotti(Id)
);

INSERT INTO Ordini (idNegozio, nota, dataOrdine)
SELECT DISTINCT idNegozio, nota, dataOrdine
FROM #TempOrdini;

INSERT INTO DettagliOrdine (idOrdine, idProdotto, quantita)
SELECT o.Id, t.idProdotto, t.quantitaTotale
FROM #TempOrdini t
JOIN Ordini o ON o.idNegozio = t.idNegozio 
    AND o.dataOrdine = t.dataOrdine;

DROP TABLE #TempOrdini;