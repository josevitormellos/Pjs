CREATE DATABASE PJS;
CREATE TABLE [dbo].[Address] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [PublicPlace]  NVARCHAR (MAX) NOT NULL,
    [Number]       INT            NOT NULL,
    [Complement]   NVARCHAR (MAX) NOT NULL,
    [Neighborhood] NVARCHAR (MAX) NOT NULL,
    [City]         NVARCHAR (MAX) NOT NULL,
    [State]        NVARCHAR (MAX) NOT NULL,
    [Cep]          NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_address] PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[User] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [Email]     NVARCHAR (MAX) NOT NULL,
    [Password]  NVARCHAR (MAX) NOT NULL,
    [Phone]     NVARCHAR (MAX) NOT NULL,
    [DateBirth] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Doctor] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [IdUser]    INT            NOT NULL,
    [Crm]       NVARCHAR (20)  NOT NULL,
    [Specialty] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_doctor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_doctor_User_IdUser] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Doctor_Crm]
    ON [dbo].[Doctor]([Crm] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_doctor_IdUser]
    ON [dbo].[Doctor]([IdUser] ASC);

CREATE TABLE [dbo].[FeedBack] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CrmDoctor]    NVARCHAR (20)  NOT NULL,
    [IdDiagnosis]  INT            NOT NULL,
    [Title]        NVARCHAR (MAX) NOT NULL,
    [Comment]      NVARCHAR (MAX) NOT NULL,
    [Sort]         NVARCHAR (MAX) NOT NULL,
    [ShippingDate] DATETIME2 (7)  NOT NULL,
    [TestDate]     INT            NOT NULL,
    CONSTRAINT [PK_feedBack] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_feedBack_Diagnosis_IdDiagnosis] FOREIGN KEY ([IdDiagnosis]) REFERENCES [dbo].[Diagnosis] ([Id]),
    CONSTRAINT [FK_feedBack_Doctor_CrmDoctor] FOREIGN KEY ([CrmDoctor]) REFERENCES [dbo].[Doctor] ([Crm])
);

CREATE TABLE [dbo].[Patient] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [CrmDoctor] NVARCHAR (20)  NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [DateBirth] DATETIME2 (7)  NOT NULL,
    [Cpf]       NVARCHAR (11)  NOT NULL,
    [IdAddress] INT            NOT NULL,
    CONSTRAINT [PK_patient] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_patient_Doctor_CrmDoctor] FOREIGN KEY ([CrmDoctor]) REFERENCES [dbo].[Doctor] ([Crm]),
    CONSTRAINT [FK_patient_Address_IdAddress] FOREIGN KEY ([IdAddress]) REFERENCES [dbo].[Address] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Patient_Cpf]
    ON [dbo].[Patient]([Cpf] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_patient_IdAddress]
    ON [dbo].[Patient]([IdAddress] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_patient_CrmDoctor]
    ON [dbo].[Patient]([CrmDoctor] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_feedBack_CrmDoctor]
    ON [dbo].[FeedBack]([CrmDoctor] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_feedBack_IdDiagnosis]
    ON [dbo].[FeedBack]([IdDiagnosis] ASC);

CREATE TABLE [dbo].[MethodDiagnosis] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CrmDoctor]       NVARCHAR (20)  NOT NULL,
    [Name]            NVARCHAR (MAX) NOT NULL,
    [Description]     NVARCHAR (MAX) NOT NULL,
    [Effectiveness]   NVARCHAR (1)   NOT NULL,
    [ResponseTime]    INT            NOT NULL,
    [Recommendations] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_methodDiagnosis] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_methodDiagnosis_Doctor_CrmDoctor] FOREIGN KEY ([CrmDoctor]) REFERENCES [dbo].[Doctor] ([Crm])
);


GO
CREATE NONCLUSTERED INDEX [IX_methodDiagnosis_CrmDoctor]
    ON [dbo].[MethodDiagnosis]([CrmDoctor] ASC);


CREATE TABLE [dbo].[Administrator] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [IdUser]      INT NOT NULL,
    [AccessLevel] INT NOT NULL,
    CONSTRAINT [PK_administrator] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_administrator_user_IdUser] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_administrator_IdUser]
    ON [dbo].[Administrator]([IdUser] ASC);

    CREATE TABLE [dbo].[Diagnosis] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [CrmDoctor]         NVARCHAR (20)  NOT NULL,
    [CpfPatient]        NVARCHAR (11)  NOT NULL,
    [Date]              DATETIME2 (7)  NOT NULL,
    [Result]            BIT            NOT NULL,
    [IdMethodDiagnosis] INT            NOT NULL,
    [Observation]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_diagnosis] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_diagnosis_Doctor_CrmDoctor] FOREIGN KEY ([CrmDoctor]) REFERENCES [dbo].[Doctor] ([Crm]),
    CONSTRAINT [FK_diagnosis_Patient_CpfPatient] FOREIGN KEY ([CpfPatient]) REFERENCES [dbo].[Patient] ([Cpf]),
    CONSTRAINT [FK_diagnosis_MethodDiagnosis_IdMethodDiagnosis] FOREIGN KEY ([IdMethodDiagnosis]) REFERENCES [dbo].[MethodDiagnosis] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_diagnosis_IdMethodDiagnosis]
    ON [dbo].[Diagnosis]([IdMethodDiagnosis] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_diagnosis_CpfPatient]
    ON [dbo].[Diagnosis]([CpfPatient] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_diagnosis_CrmDoctor]
    ON [dbo].[Diagnosis]([CrmDoctor] ASC);

    CREATE TABLE [dbo].[Report] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CrmDoctor]   NVARCHAR (20)  NOT NULL,
    [Date]        DATETIME2 (7)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_reports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_report_Doctor_CrmDoctor] FOREIGN KEY ([CrmDoctor]) REFERENCES [dbo].[Doctor] ([Crm])
);


GO
CREATE NONCLUSTERED INDEX [IX_report_CrmDoctor]
    ON [dbo].[Report]([CrmDoctor] ASC);

CREATE TABLE [dbo].[ReportDiagnosis] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [IdReport]    INT NOT NULL,
    [IdDiagnosis] INT NOT NULL,
    CONSTRAINT [PK_reportDiagnoses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_reportDiagnoses_diagnosiss_IdDiagnosis] FOREIGN KEY ([IdDiagnosis]) REFERENCES [dbo].[Diagnosis] ([Id]),
    CONSTRAINT [FK_reportDiagnosis_Report_IdReport] FOREIGN KEY ([IdReport]) REFERENCES [dbo].[Report] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_reportDiagnosis_IdDiagnosis]
    ON [dbo].[ReportDiagnosis]([IdDiagnosis] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_reportDiagnosis_IdReport]
    ON [dbo].[ReportDiagnosis]([IdReport] ASC);



