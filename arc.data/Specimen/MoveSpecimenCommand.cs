using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Patient;
using Dapper;
using Npgsql;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Moves a specimen to a new patient context. If a patient reference is not found,
    /// a new patient record may be created from the provided demographic details.
    /// </summary>
    internal class MoveSpecimenCommand : ICommandWithTypeReturningInteger<MovePatientModel>
    {
        /// <summary>
        /// Executes the move operation, updating the specimen's PatientId.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="patientInfo">Patient details or reference to locate/create the target patient.</param>
        /// <param name="logWriter">Logger for audit information.</param>
        /// <returns>The number of rows affected when updating the specimen record, or 0 if no change.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, MovePatientModel patientInfo, ILogWriter logWriter)
        {
            int newId;
            if (patientInfo.Surname != null)
            {
                if (patientInfo.Gender != null) { patientInfo.GenderId = patientInfo.Gender != null ? int.Parse(patientInfo.Gender) : 0; }
                patientInfo.DateOfBirthAsDate = patientInfo.DateOfBirth.ToNullableDateTime();

                patientInfo.FirstName = patientInfo.FirstName == "" ? " " : patientInfo.FirstName;
                patientInfo.Surname = patientInfo.Surname == "" ? " " : patientInfo.Surname;

                logWriter.LogInfo("Insert a new patient record when moving a specimen", "MoveSpecimenCommand", "ExecuteAsync");
                var patientSql = @"insert into Patient(patientref, firstname, surname, age, dateofbirth, telephonenumber,
					                            genderid, addressline1, addressline2, locationid, zipcode, barcode, lastmodifieddate, moreData)
                                                values (@patientref, @firstname, @surname, @age, @dateofbirthasdate, @telephonenumber, @genderid,
	                                            @addressline1, @addressline2, @locationid, @zipcode, @barcode, now(), cast(@MoreData as json)) returning id";

                patientInfo.PatientId = connect.QueryFirst<int>(patientSql, patientInfo);

                if (string.IsNullOrEmpty(patientInfo.PatientRef))
                {
                    patientInfo.PatientRef = patientInfo.PatientId.ToString("0000000000000");
                    patientSql = @"update Patient set patientref = @patientref, lastmodifieddate = now() where id = @patientid";
                    await connect.ExecuteAsync(patientSql, patientInfo);
                }

                newId = patientInfo.PatientId;
            } else
            {
                var sql = @"select id from patient where PatientRef = @NewPatientRef;";
                newId = await connect.QueryFirstAsync<int>(sql, new { NewPatientRef = patientInfo.PatientRef });
            }


            if (newId > 0)
            {
                var sql = @"update specimen set PatientId = @NewPatientId where Id = @Id";
                return await connect.ExecuteAsync(sql, new { NewPatientId = newId, patientInfo.Id });
            }
            else
            {
                return 0;
            }
        }
    }
}
