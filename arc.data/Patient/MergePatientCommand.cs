using arc.app.Common;
using arc.common.Models.Patient;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    internal class MergePatientCommand : ICommandWithTypeReturningInteger<MergePatientModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, MergePatientModel patientInfo, ILogWriter logWriter)
        {

            var sql = @"select id from patient where PatientRef = @NewPatientRef;";
            var newId = await connect.QueryFirstAsync<int>(sql, new { NewPatientRef = patientInfo.PatientRef });

            if (newId > 0)
            {
                sql = @"update specimen set PatientId = @NewPatientId where PatientId = @OldPatientId";

                await connect.ExecuteAsync(sql, new { NewPatientId = patientInfo.Id, OldPatientId = newId });

                sql = @"update patientcomment set PatientId = @NewPatientId where PatientId = @OldPatientId";

                await connect.ExecuteAsync(sql, new { NewPatientId = patientInfo.Id, OldPatientId = newId });

                sql = "delete from patient where id = @OldPatientId";

                return await connect.ExecuteAsync(sql, new { OldPatientId = newId });


            } else
            {
                return 0;
            }

        }
    }
}
