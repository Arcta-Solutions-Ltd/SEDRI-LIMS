using arc.app.Common;

namespace arc.app.Config.Views.DiaryViews
{
    internal class PatientDiaryConfig : IDefinition
    {
        public string Get()
        {

            var view = @"{
                            'title': 'Patient Diary',
                            'name': 'patientdiary',
                            'initialquery': 'patientdiaryentryquery',
                            'buttons': [],
                            'text': [
                                { id: '627', text: '<state> : <user> <l>Acknowledged Receipt</l> of specimen <accessionnumber>.', icon: 'ReceiptCheck' },
                                { id: '628', text: '<state> : <l>Remote request</l> received from <user>.', icon: 'Mail' },
                                { id: '629', text: '<state> : <l>New specimen <accessionnumber> </l> entered by <user>.', icon: 'TestBeakerSolid' },
                                { id: '631', text: '<state> : <l>New Culture</l> added to specimen <accessionnumber> by <user>.', icon: 'TestPlan' },
                                { id: '632', text: '<state> : <l>Existing Culture</l> for specimen <accessionnumber> edited by <user>.', icon: 'Edit'},
                                { id: '634', text: '<state> : <l>First Stage Approval</l> of specimen <accessionnumber> by <user>.', icon: 'DocumentApproval'},
                                { id: '635', text: '<state> : <l>Second Stage Approval</l> of specimen <accessionnumber> by <user>.', icon: 'DocumentApproval'},
                                { id: '636', text: '<state> : Specimen <l>Request Cancelled</l> by <user>.', icon: 'Cancel'},
                                { id: '637', text: '<state> : <l>Comment</l> entered by <user> for specimen <accessionnumber>.', icon: 'CommentAdd'},
                                { id: '648', text: '<l>Comment</l> entered by <user>.', icon: 'CommentAdd'},
                                { id: '638', text: '<state> : <l>Submitted</l> <accessionnumber> for approval by <user>.', icon: 'Generate'},
                                { id: '639', text: '<state> : <l>Day 0 Bench Read</l> for specimen <accessionnumber> by <user>.', icon: 'RepeatAll'},
                                { id: '640', text: '<state> : <l>Day 1 Bench Read</l> for specimen <accessionnumber> by <user>.', icon: 'RepeatAll'},
                                { id: '641', text: '<state> : <l>AST Results</l> for specimen <accessionnumber> updated by <user>.', icon: 'TestBeakerSolid'},
                                { id: '660', text: '<state> : <l>Direct Tests</l> for specimen <accessionnumber> selected by <user>.', icon: 'TestStep'},
                                { id: '655', text: '<state> : <l>Cell Count</l> test for specimen <accessionnumber> entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '658', text: '<state> : <l>Gram Stain</l> test for specimen <accessionnumber> entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '659', text: '<state> : <l>India Ink</l> test for specimen <accessionnumber> entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '661', text: '<state> : <l>Wet Prep</l> test for specimen <accessionnumber> entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '662', text: '<state> : <l>ZN Stain</l> test for specimen <accessionnumber> entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '667', text: '<state> : <l>Test record</l> for specimen <accessionnumber> deleted by <user>.', icon: 'Cancel'}
                            ]
                         }";

            return view;
        }
    }
}

//'buttons': [
//    { 'key': 'Add Comment', 'text': 'Add Comment', 'icon': 'CommentAdd', uievent: 'patientcommentuievent', onFinish: 'update' }
//],
