-- Create ChartTypes reference table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChartTypes')
BEGIN
    CREATE TABLE ChartTypes (
        Id INT PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL,
        Description NVARCHAR(100) NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
        IsActive BIT DEFAULT 1
    )
END

-- Insert chart types if they don't exist
IF NOT EXISTS (SELECT * FROM ChartTypes WHERE Id = 1)
BEGIN
    INSERT INTO ChartTypes (Id, Name, Description) VALUES
    (1, 'Line', 'Line Chart - Shows trends over time'),
    (2, 'Bar', 'Bar Chart - Compares values across categories'),
    (3, 'Pie', 'Pie Chart - Shows proportions of a whole'),
    (4, 'Scatter', 'Scatter Plot - Shows relationships between variables'),
    (5, 'Area', 'Area Chart - Shows cumulative values over time'),
    (6, 'Column', 'Column Chart - Vertical bar chart'),
    (7, 'Bubble', 'Bubble Chart - Shows three dimensions of data'),
    (8, 'Radar', 'Radar Chart - Shows multiple variables'),
    (9, 'HeatMap', 'Heat Map - Shows data density'),
    (10, 'BoxPlot', 'Box Plot - Shows statistical distributions'),
    (11, 'Funnel', 'Funnel Chart - Shows stages in a process'),
    (12, 'Gauge', 'Gauge Chart - Shows progress towards a goal'),
    (13, 'Waterfall', 'Waterfall Chart - Shows cumulative effect'),
    (14, 'Candlestick', 'Candlestick Chart - Shows price movements'),
    (15, 'TreeMap', 'Tree Map - Shows hierarchical data')
END

-- Create foreign key constraint on ChartConfiguration table
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ChartConfigurations')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ChartConfigurations_ChartTypes')
    BEGIN
        ALTER TABLE ChartConfigurations
        ADD CONSTRAINT FK_ChartConfigurations_ChartTypes
        FOREIGN KEY (Type) REFERENCES ChartTypes(Id)
    END
END

-- Create index for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ChartTypes_Name')
BEGIN
    CREATE INDEX IX_ChartTypes_Name ON ChartTypes(Name)
END 