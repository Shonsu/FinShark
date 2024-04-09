CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `Stocks` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Symbol` longtext NOT NULL,
    `CompanyName` longtext NOT NULL,
    `Purchase` decimal(18,2) NOT NULL,
    `LastDiv` decimal(18,2) NOT NULL,
    `Industry` longtext NOT NULL,
    `MarketCap` bigint NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Comments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` longtext NOT NULL,
    `Content` longtext NOT NULL,
    `CreatedOn` datetime(6) NOT NULL,
    `StockId` int NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Comments_Stocks_StockId` FOREIGN KEY (`StockId`) REFERENCES `Stocks` (`Id`)
);

CREATE INDEX `IX_Comments_StockId` ON `Comments` (`StockId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240409114207_InitialCreate', '8.0.3');

COMMIT;

