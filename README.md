# pruebaTecnicaItalmundo

## Base de datos de prueba

CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY,
    ClienteId INT,
    Fecha DATETIME NOT NULL,
    Total DECIMAL(10,2),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
INSERT INTO Clientes (Nombre) VALUES
('Juan'),
('Maria'),
('Pedro');

INSERT INTO Pedidos (ClienteId, Fecha, Total) VALUES
(1, '2026-01-01', 100),
(1, '2026-02-01', 250),
(2, '2026-01-15', 300),
(3, '2026-02-10', 150);

## StoreProcedures

CREATE procedure sp_ObtenerClientes as
BEGIN
	SELECT Id, Nombre FROM Clientes ORDER BY Nombre ASC
END

ALTER PROCEDURE sp_ObtenerPedidosPorCliente 
	@CLienteId INT
AS
BEGIN
	SELECT
		p.Id,
		c.Nombre AS Cliente,
		p.Fecha,
		p.Total
	FROM Pedidos p
	INNER JOIN Clientes c on p.clienteId = c.Id
	WHERE p.ClienteId = @CLienteId
	ORDER BY p.Fecha DESC
END


CREATE PROCEDURE sp_ObtenerUltimoPedido
	@ClienteId INT
AS
BEGIN
	SELECT TOP 1
		Id,
		Fecha,
		Total
	FROM Pedidos
	WHERE ClienteId = @ClienteId
	ORDER BY Fecha DESC
END

## imagen de programa en uso 
<img width="432" height="277" alt="image" src="https://github.com/user-attachments/assets/7bb62936-248d-4015-9b76-c4e38cd5df3d" />

