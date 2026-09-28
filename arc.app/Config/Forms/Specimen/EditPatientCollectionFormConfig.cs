using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditPatientCollectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'editpatientcollectionform',
                            formtype: 'singlepage',
                            pages: [ 'patientcollectiondetails', 'specimenattributeswhenreceived', 'specimentimingswhenreceived', 'specimenaction' ]
                        }";

            return form;
        }
    }
}
