using arc.app.Common;

namespace arc.app.Config.Views.DiaryViews
{
    internal class SpecimenDiaryConfig : IDefinition
    {
        public string Get()
        {

            var view = @"{
                            'title': 'Specimen Diary',
                            'name': 'specimendiary',
                            'initialquery': 'specimendiaryentryquery',
                            'workflow': 'SpecimenDefault',
                            'buttons': [],
                            'text': [
                                { id: '627', text: '<state> : <l>Acknowledged Receipt</l> of specimen from <user>.', icon: 'ReceiptCheck' },
                                { id: '628', text: '<state> : <l>Remote request</l> received from <user>.', icon: 'Mail' },
                                { id: '629', text: '<state> : <l>New specimen</l> entered by <user>.', icon: 'TestBeakerSolid' },
                                { id: '631', text: '<state> : <l>New Culture</l> added by <user>.', icon: 'TestPlan' },
                                { id: '632', text: '<state> : <l>Existing Culture</l> edited by <user>.', icon: 'Edit'},
                                { id: '634', text: '<state> : <l>First Stage Approval</l> of specimen by <user>.', icon: 'DocumentApproval'},
                                { id: '635', text: '<state> : <l>Second Stage Approval</l> of specimen by <user>.', icon: 'DocumentApproval'},
                                { id: '636', text: '<state> : Specimen <l>Request Cancelled</l> by <user>.', icon: 'Cancel'},
                                { id: '637', text: '<state> : <l>Comment</l> entered by <user>.', icon: 'CommentAdd'},
                                { id: '638', text: '<state> : <l>Submitted</l> for approval by <user>.', icon: 'Generate'},
                                { id: '639', text: '<state> : <l>Day 0 Bench Read</l> by <user>.', icon: 'RepeatAll'},
                                { id: '640', text: '<state> : <l>Day 1 Bench Read</l> by <user>.', icon: 'RepeatAll'},
                                { id: '641', text: '<state> : <l>AST Results</l> updated by <user>.', icon: 'TestBeakerSolid'},
                                { id: '655', text: '<state> : <l>Cell Count</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '658', text: '<state> : <l>Gram Stain</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '659', text: '<state> : <l>India Ink</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '661', text: '<state> : <l>Wet Prep</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '662', text: '<state> : <l>ZN Stain</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '657', text: '<state> : <l>Test record</l> deleted by <user>.', icon: 'Cancel'},
                                { id: '745', text: '<state> : <l>Auramine</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '746', text: '<state> : <l>Direct Microscopy</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '747', text: '<state> : <l>Wright Stain</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '748', text: '<state> : <l>Biochemistry</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '749', text: '<state> : <l>Jev Serology</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '750', text: '<state> : <l>Wet Prep Fungal</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '751', text: '<state> : <l>Dipstick</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '752', text: '<state> : <l>H. pylori Antigen</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '805', text: '<state> : <l>Betalactamase</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '807', text: '<state> : <l>Carbapenemase</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '806', text: '<state> : <l>Esbl</l> test entered by <user>.', icon: 'TestExploreSolid'},
                                { id: '976', text: '<state> : <l>Api Panel</l> test entered by <user>.', icon: 'TestExploreSolid'}
                            ]
                         }";

            return view;
        }
    }
}

                            //'buttons': [
                            //    { 'key': 'Add Comment', 'text': 'Add Comment', 'icon': 'CommentAdd', uievent: 'specimencommentuievent', onFinish : 'refresh' }
                            //],
