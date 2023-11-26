Zoo Database README File

Overview:
The Zoo Database aims to fulfill all the operational needs of a zoo, managing various aspects including animal details, visitor information, sales, employee records, and departmental functionalities.

Schema:
Tables:
amenity_sales: Records sales transactions for zoo amenities.
animal: Stores information about zoo animals.
department: Contains departmental details within the zoo.
employee: Manages employee records and details.
enclosure: Stores information about animal enclosures.
feeding_pattern: Tracks feeding patterns of animals (linked to animal table).
notification: Logs notifications and messages within the zoo.
restricted: Manages restricted areas within the zoo.
revenue: Records revenue and financial details.
ticket_sales: Tracks ticket sales information.
visitor: Stores visitor details.
zoo_user: Manages user accounts within the zoo.
zoo_user_role: Defines roles and permissions for zoo users.

Relationships:
feeding_pattern links to animal using Animal_ID.
Relationships exist between employee and department tables.
Further relationships are established between sales tables and respective entities.

Reports:

Triggers Overview:
These triggers enforce data integrity, implement business rules, or perform additional actions based on defined conditions.

Existing Triggers:
PreventOvercrowding Trigger:
Semantic constraint: ensure that an enclosure doesn't allow more than its stated capacity, reflecting real world policies.
Event: AFTER INSERT, UPDATE on enclosure.
Action: Checks if the number of occupants exceeds the defined capacity and rolls back the transaction if the condition is met.

UpdateLocationOnTransfer Trigger:
Semantic constraint: Every time an animal's status is updated to transferred, all zookeepers must be notified.
Event: AFTER UPDATE on animal.
Action: Updates location information and generates a notification to zookeepers regarding the animal transfer event.

Data Dictionary:
Detailed data dictionary is available for each table, including column names, data types, constraints, and explanations of stored data.

Usage Guidelines:
Queries: Instructions for querying information from various tables.
Inserts & Updates: Guidelines for inserting and updating records.
Security Measures: Outline access control and user permissions.
Common Operations: Steps for common operations like sales, visitor management, and employee record updates.
