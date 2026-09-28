using arc.common.Models.Instruments;
using arc.data.Common;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// Represents a query that retrieves a list of instrument results.
    /// Implements the <see cref="IQueryReturningType{List{InstrumentResultsListModel}}"/> interface.
    /// </summary>
    internal class InstrumentResultListQuery : IQueryReturningType<List<InstrumentResultsListModel>>
    {
        /// <summary>
        /// Executes the query asynchronously and retrieves a list of instrument result models.
        /// </summary>
        /// <param name="connect">The PostgreSQL database connection.</param>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of instrument result models.</returns>
        public async Task<List<InstrumentResultsListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var parameterInfo = new List<ParameterInfo> {
                new() { Key = "startdate", SqlExpression = "ir.RequestMade >= @startDate", Type = "date" },
                new() { Key = "enddate", SqlExpression = "ir.RequestMade <= @endDate", Type = "date" },
                new() { Key = "culturetypeid", SqlExpression = "li2.Id IN (@culturetypeid)", Type = "list" },
                new() { Key = "statusid", SqlExpression = "li3.Id IN (@StatusId)", Type = "list" },
                new() { Key = "specimentypeid", SqlExpression = "li1.Id IN (@SpecimenTypeId)", Type = "list" },
                new() { Key = "laboratoryId", SqlExpression = "s.LaboratoryId IN (@LaboratoryId)", Type = "int" }
            };


            var (whereClause, parameters) = ParameterBuilder.BuildWhereClause(queryFilters, parameterInfo);
            var sql = BuildSqlQuery(whereClause, queryFilters);
            var result = await connect.QueryAsync<InstrumentResultsListModel>(sql, parameters);

            return result.ToList();
        }

        /// <summary>
        /// Builds the SQL query string based on the WHERE clause and query filters.
        /// </summary>
        /// <param name="whereClause">The WHERE clause for filtering data.</param>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A string representing the complete SQL query.</returns>
        private string BuildSqlQuery(string whereClause, QueryFilterConfig queryFilters)
        {
            var orderBy = BuildOrderByClause(queryFilters);

            return $@"
            SELECT 
                ir.Id,
                s.accessionnumber, 
                ir.instrumentprofile, 
                li1.Value AS specimentype, 
                li2.Value AS culturetype,
                COALESCE(p.firstname || ' ' || p.surname, p.surname) AS patientname,
                li3.Value AS status,
                CASE WHEN ir.requestmade IS NULL THEN '' ELSE TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS RequestMade,
                CASE WHEN ir.resultreceived IS NULL THEN '' ELSE TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS ResultReceived,
                ir.rawresult AS testresults, 
                ir.barcode,
                ir.statusid as stateid
            FROM instrumentresults ir
            INNER JOIN specimen s ON s.id = ir.specimenid
            INNER JOIN patient p ON s.patientid = p.id
            LEFT JOIN culture c ON ir.cultureid = c.id
            LEFT JOIN listitem li1 ON s.specimentypeid = li1.id
            LEFT JOIN listitem li2 ON c.typeid = li2.id
            LEFT JOIN listitem li3 ON ir.statusid = li3.id
            {whereClause}
            ORDER BY {orderBy} 
            LIMIT 500";
        }

        /// <summary>
        /// Builds the ORDER BY clause for the SQL query based on the query filters.
        /// </summary>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A string representing the ORDER BY clause.</returns>
        private string BuildOrderByClause(QueryFilterConfig queryFilters)
        {
            var orderBy = queryFilters.OrderBy?.ToLower() switch
            {
                "instrumentprofile" => "ir.instrumentprofile",
                "barcode" => "ir.barcode",
                "specimentype" => "li1.Value",
                "culturetype" => "li2.Value",
                "patientname" => "COALESCE(p.firstname || ' ' || p.surname, p.surname)",
                "requestmade" => "TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS')",
                "resultreceived" => "TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS')",
                _ => "s.accessionnumber"
            };

            var orderDirection = queryFilters.OrderDescending ? "DESC" : "ASC";
            return string.Concat(orderBy, " ", orderDirection);
        }
    }
}

