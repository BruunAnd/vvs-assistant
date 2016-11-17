
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server Compact Edition
-- --------------------------------------------------
-- Date Created: 11/17/2016 10:57:57
-- Generated from EDMX file: C:\Users\Zen\Documents\ds305e16\VVSAssistant\VVSAssistant\Models\AssistantModel.edmx
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------

    ALTER TABLE [Clients] DROP CONSTRAINT [FK_ClientInformationAssociation];
GO
    ALTER TABLE [Offers] DROP CONSTRAINT [FK_ClientOffer];
GO
    ALTER TABLE [Offers] DROP CONSTRAINT [FK_OfferOfferInformation];
GO
    ALTER TABLE [Offers] DROP CONSTRAINT [FK_OfferPackagedSolutionAssociation];
GO
    ALTER TABLE [PackagedSolutionApplianceAssociation] DROP CONSTRAINT [FK_PackagedSolutionApplianceAssociation_PackagedSolution];
GO
    ALTER TABLE [PackagedSolutionApplianceAssociation] DROP CONSTRAINT [FK_PackagedSolutionApplianceAssociation_Appliance];
GO
    ALTER TABLE [UnitPrices_Salary] DROP CONSTRAINT [FK_OfferSalary];
GO
    ALTER TABLE [UnitPrices_Material] DROP CONSTRAINT [FK_OfferMaterial];
GO
    ALTER TABLE [UnitPrices_Salary] DROP CONSTRAINT [FK_Salary_inherits_UnitPrice];
GO
    ALTER TABLE [UnitPrices_Material] DROP CONSTRAINT [FK_Material_inherits_UnitPrice];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- NOTE: if the table does not exist, an ignorable error will be reported.
-- --------------------------------------------------

    DROP TABLE [Clients];
GO
    DROP TABLE [ClientInformation];
GO
    DROP TABLE [Offers];
GO
    DROP TABLE [OfferInformation];
GO
    DROP TABLE [PackagedSolutions];
GO
    DROP TABLE [Appliances];
GO
    DROP TABLE [UnitPrices];
GO
    DROP TABLE [UnitPrices_Salary];
GO
    DROP TABLE [UnitPrices_Material];
GO
    DROP TABLE [PackagedSolutionApplianceAssociation];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Clients'
CREATE TABLE [Clients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [ClientInformation_Id] int  NOT NULL
);
GO

-- Creating table 'ClientInformation'
CREATE TABLE [ClientInformation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [Email] nvarchar(4000)  NOT NULL,
    [Address] nvarchar(4000)  NOT NULL,
    [PhoneNumber] nvarchar(4000)  NOT NULL
);
GO

-- Creating table 'Offers'
CREATE TABLE [Offers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Client_Id] int  NOT NULL,
    [OfferInformation_Id] int  NOT NULL,
    [PackagedSolution_Id] int  NOT NULL
);
GO

-- Creating table 'OfferInformation'
CREATE TABLE [OfferInformation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'PackagedSolutions'
CREATE TABLE [PackagedSolutions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [CreationDate] datetime  NOT NULL
);
GO

-- Creating table 'Appliances'
CREATE TABLE [Appliances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Type] int  NOT NULL,
    [DataSheet_Id] int  NOT NULL
);
GO

-- Creating table 'UnitPrices'
CREATE TABLE [UnitPrices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] int  NOT NULL,
    [UnitCostPrice] float  NOT NULL,
    [UnitSalesPrice] float  NOT NULL
);
GO

-- Creating table 'DataSheets'
CREATE TABLE [DataSheets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Price] float  NOT NULL
);
GO

-- Creating table 'UnitPrices_Salary'
CREATE TABLE [UnitPrices_Salary] (
    [Name] nvarchar(4000)  NOT NULL,
    [Id] int  NOT NULL,
    [OfferSalary_Salary_Id] int  NOT NULL
);
GO

-- Creating table 'UnitPrices_Material'
CREATE TABLE [UnitPrices_Material] (
    [VvsNumber] int  NOT NULL,
    [Name] nvarchar(4000)  NOT NULL,
    [Id] int  NOT NULL,
    [OfferMaterial_Material_Id] int  NOT NULL
);
GO

-- Creating table 'DataSheets_HeatPumpDataSheet'
CREATE TABLE [DataSheets_HeatPumpDataSheet] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'DataSheets_TemperatureControllerDataSheet'
CREATE TABLE [DataSheets_TemperatureControllerDataSheet] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'PackagedSolutionApplianceAssociation'
CREATE TABLE [PackagedSolutionApplianceAssociation] (
    [PackagedSolutionApplianceAssociation_Appliance_Id] int  NOT NULL,
    [Appliances_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Clients'
ALTER TABLE [Clients]
ADD CONSTRAINT [PK_Clients]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'ClientInformation'
ALTER TABLE [ClientInformation]
ADD CONSTRAINT [PK_ClientInformation]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'Offers'
ALTER TABLE [Offers]
ADD CONSTRAINT [PK_Offers]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'OfferInformation'
ALTER TABLE [OfferInformation]
ADD CONSTRAINT [PK_OfferInformation]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'PackagedSolutions'
ALTER TABLE [PackagedSolutions]
ADD CONSTRAINT [PK_PackagedSolutions]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'Appliances'
ALTER TABLE [Appliances]
ADD CONSTRAINT [PK_Appliances]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'UnitPrices'
ALTER TABLE [UnitPrices]
ADD CONSTRAINT [PK_UnitPrices]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'DataSheets'
ALTER TABLE [DataSheets]
ADD CONSTRAINT [PK_DataSheets]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'UnitPrices_Salary'
ALTER TABLE [UnitPrices_Salary]
ADD CONSTRAINT [PK_UnitPrices_Salary]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'UnitPrices_Material'
ALTER TABLE [UnitPrices_Material]
ADD CONSTRAINT [PK_UnitPrices_Material]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'DataSheets_HeatPumpDataSheet'
ALTER TABLE [DataSheets_HeatPumpDataSheet]
ADD CONSTRAINT [PK_DataSheets_HeatPumpDataSheet]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'DataSheets_TemperatureControllerDataSheet'
ALTER TABLE [DataSheets_TemperatureControllerDataSheet]
ADD CONSTRAINT [PK_DataSheets_TemperatureControllerDataSheet]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [PackagedSolutionApplianceAssociation_Appliance_Id], [Appliances_Id] in table 'PackagedSolutionApplianceAssociation'
ALTER TABLE [PackagedSolutionApplianceAssociation]
ADD CONSTRAINT [PK_PackagedSolutionApplianceAssociation]
    PRIMARY KEY ([PackagedSolutionApplianceAssociation_Appliance_Id], [Appliances_Id] );
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientInformation_Id] in table 'Clients'
ALTER TABLE [Clients]
ADD CONSTRAINT [FK_ClientInformationAssociation]
    FOREIGN KEY ([ClientInformation_Id])
    REFERENCES [ClientInformation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientInformationAssociation'
CREATE INDEX [IX_FK_ClientInformationAssociation]
ON [Clients]
    ([ClientInformation_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Offers'
ALTER TABLE [Offers]
ADD CONSTRAINT [FK_ClientOffer]
    FOREIGN KEY ([Client_Id])
    REFERENCES [Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientOffer'
CREATE INDEX [IX_FK_ClientOffer]
ON [Offers]
    ([Client_Id]);
GO

-- Creating foreign key on [OfferInformation_Id] in table 'Offers'
ALTER TABLE [Offers]
ADD CONSTRAINT [FK_OfferOfferInformation]
    FOREIGN KEY ([OfferInformation_Id])
    REFERENCES [OfferInformation]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferOfferInformation'
CREATE INDEX [IX_FK_OfferOfferInformation]
ON [Offers]
    ([OfferInformation_Id]);
GO

-- Creating foreign key on [PackagedSolution_Id] in table 'Offers'
ALTER TABLE [Offers]
ADD CONSTRAINT [FK_OfferPackagedSolutionAssociation]
    FOREIGN KEY ([PackagedSolution_Id])
    REFERENCES [PackagedSolutions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferPackagedSolutionAssociation'
CREATE INDEX [IX_FK_OfferPackagedSolutionAssociation]
ON [Offers]
    ([PackagedSolution_Id]);
GO

-- Creating foreign key on [PackagedSolutionApplianceAssociation_Appliance_Id] in table 'PackagedSolutionApplianceAssociation'
ALTER TABLE [PackagedSolutionApplianceAssociation]
ADD CONSTRAINT [FK_PackagedSolutionApplianceAssociation_PackagedSolution]
    FOREIGN KEY ([PackagedSolutionApplianceAssociation_Appliance_Id])
    REFERENCES [PackagedSolutions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Appliances_Id] in table 'PackagedSolutionApplianceAssociation'
ALTER TABLE [PackagedSolutionApplianceAssociation]
ADD CONSTRAINT [FK_PackagedSolutionApplianceAssociation_Appliance]
    FOREIGN KEY ([Appliances_Id])
    REFERENCES [Appliances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PackagedSolutionApplianceAssociation_Appliance'
CREATE INDEX [IX_FK_PackagedSolutionApplianceAssociation_Appliance]
ON [PackagedSolutionApplianceAssociation]
    ([Appliances_Id]);
GO

-- Creating foreign key on [OfferSalary_Salary_Id] in table 'UnitPrices_Salary'
ALTER TABLE [UnitPrices_Salary]
ADD CONSTRAINT [FK_OfferSalary]
    FOREIGN KEY ([OfferSalary_Salary_Id])
    REFERENCES [Offers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferSalary'
CREATE INDEX [IX_FK_OfferSalary]
ON [UnitPrices_Salary]
    ([OfferSalary_Salary_Id]);
GO

-- Creating foreign key on [OfferMaterial_Material_Id] in table 'UnitPrices_Material'
ALTER TABLE [UnitPrices_Material]
ADD CONSTRAINT [FK_OfferMaterial]
    FOREIGN KEY ([OfferMaterial_Material_Id])
    REFERENCES [Offers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferMaterial'
CREATE INDEX [IX_FK_OfferMaterial]
ON [UnitPrices_Material]
    ([OfferMaterial_Material_Id]);
GO

-- Creating foreign key on [DataSheet_Id] in table 'Appliances'
ALTER TABLE [Appliances]
ADD CONSTRAINT [FK_ApplianceDataSheet]
    FOREIGN KEY ([DataSheet_Id])
    REFERENCES [DataSheets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplianceDataSheet'
CREATE INDEX [IX_FK_ApplianceDataSheet]
ON [Appliances]
    ([DataSheet_Id]);
GO

-- Creating foreign key on [Id] in table 'UnitPrices_Salary'
ALTER TABLE [UnitPrices_Salary]
ADD CONSTRAINT [FK_Salary_inherits_UnitPrice]
    FOREIGN KEY ([Id])
    REFERENCES [UnitPrices]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'UnitPrices_Material'
ALTER TABLE [UnitPrices_Material]
ADD CONSTRAINT [FK_Material_inherits_UnitPrice]
    FOREIGN KEY ([Id])
    REFERENCES [UnitPrices]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'DataSheets_HeatPumpDataSheet'
ALTER TABLE [DataSheets_HeatPumpDataSheet]
ADD CONSTRAINT [FK_HeatPumpDataSheet_inherits_DataSheet]
    FOREIGN KEY ([Id])
    REFERENCES [DataSheets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'DataSheets_TemperatureControllerDataSheet'
ALTER TABLE [DataSheets_TemperatureControllerDataSheet]
ADD CONSTRAINT [FK_TemperatureControllerDataSheet_inherits_DataSheet]
    FOREIGN KEY ([Id])
    REFERENCES [DataSheets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------