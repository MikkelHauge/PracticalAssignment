CREATE TABLE Usernames
(
    UserID INT PRIMARY KEY,
    Username VARCHAR(50)
)
go
CREATE TABLE Emails
(
    UserID INT PRIMARY KEY,
    Email VARCHAR(50)
)
go
CREATE TABLE Addresses
(
    UserID INT PRIMARY KEY,
    Address VARCHAR(100)
)