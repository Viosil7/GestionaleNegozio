CREATE DATABASE GestionaleNegozio;
GO

USE GestionaleNegozio;
GO

CREATE TABLE Staff (
    Id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) NOT NULL,
    passw NVARCHAR(100) NOT NULL,
    ruolo NVARCHAR(20) NOT NULL
);

CREATE TABLE Negozi (
    Id INT PRIMARY KEY IDENTITY(1,1),
    regione NVARCHAR(50) NOT NULL,
    città NVARCHAR(50) NOT NULL,
    indirizzo NVARCHAR(100) NOT NULL
);

CREATE TABLE Prodotti (
    Id INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(100) NOT NULL,
    categoria NVARCHAR(50) NOT NULL,
    descrizione NVARCHAR(MAX),
    prezzo DECIMAL(10,2) NOT NULL
);

CREATE TABLE Magazzino (
    idNegozio INT NOT NULL,
    idProdotto INT NOT NULL,
    quantità INT NOT NULL,
    PRIMARY KEY (idNegozio, idProdotto),
    FOREIGN KEY (idNegozio) REFERENCES Negozi(Id),
    FOREIGN KEY (idProdotto) REFERENCES Prodotti(Id)
);

CREATE TABLE Ordini (
    Id INT PRIMARY KEY IDENTITY(1,1),
    idNegozio INT NOT NULL,
    idProdotto INT NOT NULL,
    nota NVARCHAR(MAX),
    quantitaTotale INT NOT NULL,
    dataOrdine DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (idNegozio) REFERENCES Negozi(Id),
    FOREIGN KEY (idProdotto) REFERENCES Prodotti(Id)
);

INSERT INTO Staff (username, passw, ruolo)
VALUES ('Manager', 'Manager1234!', 'Manager');
