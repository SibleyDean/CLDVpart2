USE master;
IF EXISTS (SELECT * FROM SYS.databases WHERE name = 'RegistrationDB')
DROP DATABASE RegistrationDB
CREATE DATABASE RegistrationDB

USE RegistrationDB

DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Event;
DROP TABLE IF EXISTS Venue;

-- Create Venue table
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    VenueName VARCHAR(50) NOT NULL,
    Location VARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(MAX)
);

-- Create Event table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventName NVARCHAR(50) NOT NULL,
    EventDate DATE NOT NULL,
    Description NVARCHAR(MAX),
    VenueId INT NOT NULL,
	FOREIGN KEY(VenueId) REFERENCES Venue(VenueId)
);

-- Create Booking table (associative table)
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    EventId INT NOT NULL ,
    VenueId INT NOT NULL ,
    BookingDate DATE NOT NULL ,
	FOREIGN KEY (VenueId) REFERENCES Venue(VenueId),
	FOREIGN KEY (EventId) REFERENCES Event (EventId)
);

INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES ('venue A', 'Location 1.1',100, 'picture.gpg');

INSERT INTO Event (EventName, EventDate, Description, VenueId)
VALUES ('event A', '2025-03-10','Description for 1st event', 1);

INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES (1, 1, '2025-04-11');

SELECT * FROM Venue
SELECT * FROM Event
SELECT * FROM Booking