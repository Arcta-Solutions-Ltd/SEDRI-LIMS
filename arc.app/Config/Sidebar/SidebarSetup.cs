using arc.domain.Configuration.SidebarConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Sidebar;

/// <summary>
/// Provides the configuration for the left sidebar navigation.
/// </summary>
public class SidebarSetup
{
    /// <summary>
    /// Builds and returns the <see cref="SidebarConfig"/> for the left sidebar.
    /// </summary>
    /// <returns>A <see cref="SidebarConfig"/> instance deserialized from the embedded JSON definition.</returns>
    public SidebarConfig GetLeftSidebarConfig()
    {
        var sidebar = @"{
                    links: [
                        { name: '@GenHom@', icon: 'home', key: 'home' },
                        { name: '@GenSpe@', icon: 'TestBeaker', key: 'specimens' },
                        { name: '@GenPat@', icon: 'FabricUserFolder', key: 'patients' },
                        { name: '@GenTes@', icon: 'TestExploreSolid', key: 'testmenu',
                            links: [
                                { name: '@ConDirA@', key: 'tests', icon: 'TestExploreSolid' },
                                { name: '@SpeCulC@', key: 'culturetests', icon: 'TestExploreSolid' }
                           ]
                        },
                        { name: '@GenVieD@', icon: 'ReportDocument', key: 'reports',
                            links: [
                                { name: '@GenVieG@', key: 'approvedreportview', icon: 'RedEye' },
                                { name: '@RepRepD@', key: 'unapprovedreportview', icon: 'ReportDocument' },
                                { name: '@GenGra@', key: 'graphs', icon: 'LineChart' },
                                { name: '@RepAntA@', key: 'antibiogram', icon: 'BIDashboard' }
                           ]
                        },
                        { name: '@InsInsA@', icon: 'Manufacturing', key: 'instruments',
                            links: [
                                { name: '@InsIns@', key: 'instrumentresults', icon: 'ShowResults' },
                                { name: '@InsErrA@', key: 'instrumenterror', icon: 'Error' },
                                { name: '@InsInsB@', key: 'instrumentconfig', icon: 'ConfigurationSolid' }
                           ]
                        },
                        { name: '@SpeSpeP@', icon: 'Archive', key: 'archive' },
                        { name: '@GenExp@', icon: 'Generate', key: 'exportmenu',
                            links: [
                                { name: '@ExpExpDat@', key: 'exportprofile', icon: 'Generate' },
                                { name: '@ExpExpHis@', key: 'exporthistory', icon: 'History' }
                           ]
                        },
                        //{ name: '@AssAss@', icon: 'Import', key: 'assettracking',
                        //    links: [
                        //        { name: '@AssSup@', icon: 'Import', key: 'suppliers' },
                        //        { name: '@AssStr@', key: 'storage', icon: 'CloudImportExport' }
                        //   ]
                        //},
                        { name: '@GenCod@', icon: 'TabletMode', key: 'coding',
                            links: [
                                { name: '@GenOrgB@', key: 'organism', icon: 'Dictionary' },
                                { name: '@GenAntA@', key: 'antibiotics', icon: 'Dictionary' },
                                { name: '@GenTesB@', key: 'testpatterns', icon: 'TestPlan' },
                                { name: '@GenBre@', key: 'breakpoints', icon: 'TestImpactSolid' },
                                { name: '@GenRulA@', key: 'expertrules', icon: 'DecisionSolid' },
                                { name: '@GenAle@', key: 'alertsconfig', icon: 'ShieldAlert' },
                                { name: '@AleTypA@', key: 'alerttype', icon: 'ShieldAlert' },
                                { name: '@GenTagK@', key: 'tagsconfig', icon: 'Tag' },
                                { name: '@GenSpf@', key: 'specifications', icon: 'BookAnswers'}
                           ]
                        },
                        { name: '@Qua@', key: 'quality', icon: 'Certificate',
                            links: [
                                { name: '@QuaIqcTesA@', key: 'iqctests', icon: 'HomeVerify' },
                                { name: '@QuaIqcTesPro@', key: 'iqctestprofile', icon: 'Dictionary' }
                            ]
                        },
                        { name: '@GenMon@', icon: 'CheckList', key: 'monitoring' },
                        //{ name: '@BilBil@', icon: 'Money', key: 'billing',
                        //    links: [
                        //        { name: '@BilBilB@', key: 'billingrecord', icon: 'Money' },
                        //        { name: '@BilBilA@', key: 'billingrule', icon: 'Money' }
                        //    ]
                        //},
                        { name: '@GenAdm@', icon: 'Repair', key: 'admin',
                            links: [
                                { name: '@GenUse@', key: 'users', icon: 'PeopleAdd' },
                                { name: '@GenRol@', key: 'roles', icon: 'Permissions' },
                                { name: '@GenLab@', key: 'laboratories', icon: 'TestUserSolid' },
                                { name: '@GenOrg@', key: 'organisations', icon: 'CityNext2' },
                                { name: '@GenLocB@', key: 'locations', icon: 'POISolid' },
                                { name: '@LanTra@', key: 'language', icon: 'Translate' }
                            ]
                        },
                        { name: '@GenCon@', icon: 'ConfigurationSolid', key: 'configuration',
                            links: [
                                { name: '@GenSet@', key: 'settings', icon: 'Settings' },
                                { name: '@GenTab@', key: 'tables', icon: 'TableGroup' },
                                { name: '@RepImg@', key: 'images', icon: 'ImagePixel' },
                                { name: '@GenBarA@', key: 'specimenbarcodes', icon: 'QRCode' },
                                { name: '@GenBar@', key: 'patientbarcodes', icon: 'QRCode' },
                                { name: '@GenVie@', key: 'views', icon: 'PictureFill' },
                                { name: '@MapTit@', key: 'mappingconfigview', icon: 'Flow' },
                                { name: '@GenHis@', key: 'confighistory', icon: 'PictureFill' }
                            ]
                        }
                    ]
                }";

        var result = JsonConvert.DeserializeObject<SidebarConfig>(sidebar);

        return result;
    }
}

//var sidebar = @"{
//            links: [
//                { name: '@GenHom@', icon: 'home', key: 'home' },
//                { name: '@GenSpe@', icon: 'TestBeaker', key: 'specimens' },
//                { name: '@GenPat@', icon: 'FabricUserFolder', key: 'patients' },
//                { name: '@RepPubA@', key: 'approvedreportview', icon: 'ReportDocument' },
//                { name: '@GenTes@', icon: 'TestExploreSolid', key: 'testmenu',
//                    links: [
//                        { name: '@ConDirA@', key: 'tests', icon: 'TestExploreSolid' },
//                        { name: '@SpeCulC@', key: 'culturetests', icon: 'TestExploreSolid' }
//                   ]
//                },
//                { name: '@GenVieD@', icon: 'ReportDocument', key: 'reports',
//                    links: [
//                        { name: '@GenRep@', key: 'batch', icon: 'ReportDocument' },
//                        { name: '@GenGra@', key: 'graphs', icon: 'LineChart' },
//                        { name: '@RepAntA@', key: 'antibiogram', icon: 'BIDashboard' },
//                        { name: '@RepRepD@', key: 'unapprovedreportview', icon: 'ReportDocument' }
//                   ]
//                },
//                { name: '@InsInsA@', icon: 'Manufacturing', key: 'instruments',
//                    links: [
//                        { name: '@InsIns@', key: 'instrumentresults', icon: 'ShowResults' },
//                        { name: '@InsInsB@', key: 'instrumentconfig', icon: 'ConfigurationSolid' },
//                        { name: '@InsErrA@', key: 'instrumenterror', icon: 'Error' }
//                   ]
//                },
//                { name: '@GenArc@', icon: 'Archive', key: 'archive' },
//                { name: '@GenExp@', icon: 'Generate', key: 'exportprofile'},
//                { name: '@AssAss@', icon: 'Import', key: 'assettracking',
//                    links: [
//                        { name: '@AssSup@', icon: 'Import', key: 'suppliers' },
//                        { name: '@AssStr@', key: 'storage', icon: 'CloudImportExport' }
//                   ]
//                },
//                { name: '@BilBil@', icon: 'Import', key: 'billing',
//                    links: [
//                        { name: '@BilBilA@', icon: 'Import', key: 'billingrule' }
//                   ]
//                },
//                { name: '@GenCod@', icon: 'TabletMode', key: 'coding',
//                    links: [
//                        { name: '@GenOrgB@', key: 'organism', icon: 'Dictionary' },
//                        { name: '@GenBre@', key: 'breakpoints', icon: 'TestImpactSolid' },
//                        { name: '@GenTesB@', key: 'testpatterns', icon: 'TestPlan' },
//                        { name: '@GenAle@', key: 'alertsconfig', icon: 'ShieldAlert' },
//                        { name: '@GenAntA@', key: 'antibiotics', icon: 'Dictionary' },
//                        { name: '@AleTypA@', key: 'alerttype', icon: 'ShieldAlert' }
//                   ]
//                },
//                { name: '@Qua@', key: 'quality', icon: 'Certificate',
//                    links: [
//                        { name: '@QuaIqcTesA@', key: 'iqctests', icon: 'HomeVerify' },
//                        { name: '@QuaIqcTesPro@', key: 'iqctestprofile', icon: 'Dictionary' }
//                    ]
//                },
//                { name: '@GenMon@', icon: 'CheckList', key: 'monitoring' },
//                { name: '@GenAdm@', icon: 'Repair', key: 'admin',
//                    links: [
//                        { name: '@GenUse@', key: 'users', icon: 'PeopleAdd' },
//                        { name: '@GenRol@', key: 'roles', icon: 'Permissions' },
//                        { name: '@GenLab@', key: 'laboratories', icon: 'TestUserSolid' },
//                        { name: '@GenOrg@', key: 'organisations', icon: 'CityNext2' },
//                        { name: '@GenLocB@', key: 'locations', icon: 'POISolid' },
//                        { name: '@GenTab@', key: 'tables', icon: 'TableGroup' },
//                        { name: '@LanTra@', key: 'language', icon: 'Translate' }
//                    ]
//                },
//                { name: '@GenCon@', icon: 'ConfigurationSolid', key: 'configuration',
//                    links: [
//                        { name: '@GenBar@', key: 'patientbarcodes', icon: 'QRCode' },
//                        { name: '@GenBarA@', key: 'specimenbarcodes', icon: 'QRCode' },
//                        { name: '@GenVie@', key: 'views', icon: 'PictureFill' },
//                        { name: '@GenHis@', key: 'confighistory', icon: 'PictureFill' },
//                        { name: '@MapTit@', key: 'mappingconfigview', icon: 'Flow' },
//                        { name: '@GenSet@', key: 'settings', icon: 'Settings' }
//                    ]
//                }
//            ]
//        }";