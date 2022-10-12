using FlexLabs.EntityFrameworkCore.Upsert.Runners;

namespace FlexLabs.EntityFrameworkCore.Upsert.Tests.Runners
{
    public class OracleSqlUpsertCommandRunnerTests : RelationalCommandRunnerTestsBase<OracleUpsertCommandRunner>
    {
        private enum NpgsqlValueGenerationStrategy
        {
            None,
            SequenceHiLo,
            SerialColumn,
            IdentityAlwaysColumn,
            IdentityByDefaultColumn
        }
        public OracleSqlUpsertCommandRunnerTests() : base("Devart.Data.Oracle.Entity.EFCore") { }

        protected override string NoUpdate_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total)";
        protected override string NoUpdate_Multiple_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL " +
            "UNION ALL " +
            "SELECT :p4 as ID, :p5 as Name, :p6 as Status, :p7 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total)";
        protected override string NoUpdate_WithNullable_Sql =>
            "MERGE INTO TestEntityWithNullableKey T USING ( " +
            "SELECT :p0 as ID, :p1 as ID1, :p2 as ID2, :p3 as Name, :p4 as Status, :p5 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID1 = S.ID1 AND ((S.ID2 IS NULL AND T.ID2 IS NULL) OR (S.ID2 IS NOT NULL AND T.ID2 = S.ID2))) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.ID1, T.ID2, T.Name, T.Status, T.Total) VALUES (S.ID, S.ID1, S.ID2, S.Name, S.Status, S.Total)";
        protected override string Update_Constant_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p4";
        protected override string Update_Constant_Multiple_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL " +
            "UNION ALL " +
            "SELECT :p4 as ID, :p5 as Name, :p6 as Status, :p7 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p8";
        protected override string Update_Source_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = S.Name";

        protected override string Update_BinaryAdd_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Total = ( T.Total + :p4 )";

        protected override string Update_Coalesce_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Status = ( COALESCE(T.Status, :p4) )";


        protected override string Update_BinaryAddMultiply_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Total = ( ( T.Total + :p4 ) * S.Total )";

        protected override string Update_BinaryAddMultiplyGroup_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Total = ( T.Total + ( :p4 * S.Total ) )";

        protected override string Update_Condition_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p4 WHERE T.Total > :p5";

        protected override string Update_Condition_UpdateConditionColumn_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p4, Total = ( T.Total + :p5 ) WHERE T.Total > :p6";

        protected override string Update_Condition_AndCondition_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p4 WHERE ( T.Total > :p5 ) AND ( T.Status != S.Status )";


        protected override string Update_Condition_NullCheck_Sql =>
            "MERGE INTO TestEntity T USING ( " +
            "SELECT :p0 as ID, :p1 as Name, :p2 as Status, :p3 as Total FROM DUAL" +
            ") S " +
            "ON (T.ID = S.ID) " +
            "WHEN NOT MATCHED THEN INSERT (T.ID, T.Name, T.Status, T.Total) VALUES (S.ID, S.Name, S.Status, S.Total) " +
            "WHEN MATCHED THEN UPDATE SET Name = :p4 WHERE T.Status IS NOT NULL";
    }
}
