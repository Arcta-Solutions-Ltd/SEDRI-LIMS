using arc.app.Common;

namespace arc.app.Config.Language;

internal class DutchLanguage : IDefinition
{
    public string Get()
    {
        return """
					[
					  {
					    "key": "@AleA@",
					    "value": "Een tag moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAdd@",
					    "value": "Alert toevoegen"
					  },
					  {
					    "key": "@AleAddA@",
					    "value": "Organisme Alert toevoegen"
					  },
					  {
					    "key": "@AleAddB@",
					    "value": "Tag toevoegen"
					  },
					  {
					    "key": "@AleAddC@",
					    "value": "Type toevoegen"
					  },
					  {
					    "key": "@AleAddD@",
					    "value": "Waarschuwingstype toevoegen"
					  },
					  {
					    "key": "@AleAddE@",
					    "value": "Een nieuwe alertcategorie toevoegen"
					  },
					  {
					    "key": "@AleAddF@",
					    "value": "Alert toevoegen"
					  },
					  {
					    "key": "@AleAddG@",
					    "value": "Waarschuwing voor organisme toevoegen"
					  },
					  {
					    "key": "@AleAddChildTag@",
					    "value": "Onderliggende tag toevoegen"
					  },
					  {
					    "key": "@AleAddH@",
					    "value": "Tag toevoegen"
					  },
					  {
					    "key": "@AleAle@",
					    "value": "Waarschuwingsnaam"
					  },
					  {
					    "key": "@AleAleA@",
					    "value": "Alertnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAleB@",
					    "value": "Alert Bericht moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAleC@",
					    "value": "Waarschuwingsdetails"
					  },
					  {
					    "key": "@AleAleD@",
					    "value": "Waarschuwingslijst"
					  },
					  {
					    "key": "@AleAleE@",
					    "value": "ALERT Categorienaam moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAleF@",
					    "value": "Niveau moet worden geselecteerd"
					  },
					  {
					    "key": "@AleAleG@",
					    "value": "Alertkleur moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAleH@",
					    "value": "Alert -positie moet worden ingevoerd"
					  },
					  {
					    "key": "@AleAleI@",
					    "value": "Alert -type gebruikt in een waarschuwing"
					  },
					  {
					    "key": "@AleAleJ@",
					    "value": "Alert -criteria"
					  },
					  {
					    "key": "@AleAleK@",
					    "value": "Geef alertcriteria op voor een bepaalde test en stel combinatieregels in waar nodig"
					  },
					  {
					    "key": "@AleB@",
					    "value": "voeg een nieuwe tag toe"
					  },
					  {
					    "key": "@AleCat@",
					    "value": "Alertcategorie"
					  },
					  {
					    "key": "@AleCre@",
					    "value": "Nieuwe en bestaande meldingen maken en beheren"
					  },
					  {
					    "key": "@AleCreA@",
					    "value": "Nieuwe en bestaande waarschuwingstypen maken en beheren"
					  },
					  {
					    "key": "@AleCri@",
					    "value": "Alert Criteria"
					  },
					  {
					    "key": "@AleCriA@",
					    "value": "Specificeer de criteria waaraan moet worden voldaan voor de waarschuwing om te worden geactiveerd"
					  },
					  {
					    "key": "@AleDel@",
					    "value": "Waarschuwing verwijderen"
					  },
					  {
					    "key": "@AleDelA@",
					    "value": "Verwijder deze alert"
					  },
					  {
					    "key": "@AleDelB@",
					    "value": "Verwijder tag"
					  },
					  {
					    "key": "@AleDelC@",
					    "value": "Verwijder deze tag"
					  },
					  {
					    "key": "@AleDelD@",
					    "value": "Verwijder type"
					  },
					  {
					    "key": "@AleDelE@",
					    "value": "Verwijder waarschuwingstype"
					  },
					  {
					    "key": "@AleDelF@",
					    "value": "Verwijder een bestaand waarschuwingstype"
					  },
					  {
					    "key": "@AleDelG@",
					    "value": "Waarschuwing verwijderen"
					  },
					  {
					    "key": "@AleDelH@",
					    "value": "Tag verwijderen"
					  },
					  {
					    "key": "@AleDis@",
					    "value": "Report Display"
					  },
					  {
					    "key": "@AleDoe@",
					    "value": "Alert op het detecteren van organisme?"
					  },
					  {
					    "key": "@AleEdi@",
					    "value": "Bewerk alert"
					  },
					  {
					    "key": "@AleEdiA@",
					    "value": "Tag bewerken"
					  },
					  {
					    "key": "@AleEdiB@",
					    "value": "Een bestaande tag bewerken"
					  },
					  {
					    "key": "@AleEdiC@",
					    "value": "Type bewerken"
					  },
					  {
					    "key": "@AleEdiD@",
					    "value": "een bestaand waarschuwingstype bewerken"
					  },
					  {
					    "key": "@AleEdiE@",
					    "value": "Type bewerken"
					  },
					  {
					    "key": "@AleEdiF@",
					    "value": "Bewerk waarschuwing"
					  },
					  {
					    "key": "@AleEdiG@",
					    "value": "Bewerk tag"
					  },
					  {
					    "key": "@AleEnt@",
					    "value": "Voer waarschuwingsnaam in"
					  },
					  {
					    "key": "@AleEntA@",
					    "value": "Voer waarschuwingsbericht in"
					  },
					  {
					    "key": "@AleMan@",
					    "value": "Beheer waarschuwingen"
					  },
					  {
					    "key": "@AleManA@",
					    "value": "Specificeer algemene alert -eigenschappen, inclusief het meldingsbericht"
					  },
					  {
					    "key": "@AleManB@",
					    "value": "Waarschuwingstypen beheren"
					  },
					  {
					    "key": "@AleManC@",
					    "value": "Alert Eigenschappen bewerken, inclusief het meldingsbericht"
					  },
					  {
					    "key": "@AlePos@",
					    "value": "Meldingspositie"
					  },
					  {
					    "key": "@AleSpe@",
					    "value": "Specimenwaarschuwingen gemarkeerd door het systeem"
					  },
					  {
					    "key": "@AleSus@",
					    "value": "Criteria voor vatbaarheid waarschuwing",
					  },
					  {
					    "key": "@AleSusA@",
					    "value": "Combinatieregel voor vatbaarheid waarschuwing"
					  },
					  {
					    "key": "@AleTes@",
					    "value": "Test waarschuwingscriteria"
					  },
					  {
					    "key": "@AleTesA@",
					    "value": "Combinatieregel Test Alert"
					  },
					  {
					    "key": "@AleThe@",
					    "value": "De tag die u wilt wijzigen, moet worden geselecteerd"
					  },
					  {
					    "key": "@AleTheA@",
					    "value": "De tag die u wilt verwijderen, moet worden geselecteerd"
					  },
					  {
					    "key": "@AleTagHasChildren@",
					    "value": "Kan geen tag verwijderen die onderliggende tags heeft"
					  },
					  {
					    "key": "@AleThi@",
					    "value": "Deze tag bestaat al"
					  },
					  {
					    "key": "@AleTyp@",
					    "value": "waarschuwingstype"
					  },
					  {
					    "key": "@AleTypA@",
					    "value": "Alert -typen"
					  },
					  {
					    "key": "@AleTypB@",
					    "value": "U moet een waarschuwingstype selecteren"
					  },
					  {
					    "key": "@AntAddA@",
					    "value": "Antibioticagroep toevoegen"
					  },
					  {
					    "key": "@AntAddB@",
					    "value": "Antibioticagroep toevoegen"
					  },
					  {
					    "key": "@AntAddC@",
					    "value": "Voeg een nieuwe antibioticagroep toe"
					  },
					  {
					    "key": "@AntAddD@",
					    "value": "Antibioticum toevoegen om te vermelden"
					  },
					  {
					    "key": "@AntAddE@",
					    "value": "een antibioticum toevoegen aan de momenteel geselecteerde lijst"
					  },
					  {
					    "key": "@AntAddReqCod@",
					    "value": "Er moet een code worden ingevoerd"
					  },
					  {
					    "key": "@AntAddReqNam@",
					    "value": "Er moet een antibioticanaam worden ingevoerd"
					  },
					  {
					    "key": "@AntAlrExi@",
					    "value": "Er bestaat al een antibioticum met dezelfde naam of code"
					  },
					  {
					    "key": "@AntAna@",
					    "value": "Er moet een antibioticagroep worden ingevoerd"
					  },
					  {
					    "key": "@AntAnaA@",
					    "value": "Er moet een antibioticagroep worden geselecteerd"
					  },
					  {
					    "key": "@AntAnt@",
					    "value": "Antibioticagroepen"
					  },
					  {
					    "key": "@AntDel@",
					    "value": "Antibioticagroep verwijderen"
					  },
					  {
					    "key": "@AntDelA@",
					    "value": "Verwijder een bestaande antibioticagroep en al zijn inhoud"
					  },
					  {
					    "key": "@AntDelB@",
					    "value": "Antibioticum verwijderen uit de lijst"
					  },
					  {
					    "key": "@AntDelC@",
					    "value": "Een antibioticum verwijderen uit de momenteel geselecteerde lijst"
					  },
					  {
					    "key": "@AntDelIdReq@",
					    "value": "Er moet een antibiotica-ID worden opgegeven om te worden verwijderd"
					  },
					  {
					    "key": "@AntDelSub@",
					    "value": "Verwijder een antibioticum"
					  },
					  {
					    "key": "@AntInUseAst@",
					    "value": "Antibioticum is nog steeds in gebruik door een AST"
					  },
					  {
					    "key": "@AntInUseBre@",
					    "value": "Antibioticum is nog steeds in gebruik door een breekpunt"
					  },
					  {
					    "key": "@AntInUseTes@",
					    "value": "Antibioticum is nog steeds in gebruik door een testpatroon"
					  },
					  {
					    "key": "@AntManA@",
					    "value": "Antibiotica beheren"
					  },
					  {
					    "key": "@AntManAdd@",
					    "value": "Antibioticum toevoegen"
					  },
					  {
					    "key": "@AntManAddSub@",
					    "value": "Definieer de eigenschappen van het te toegevoegde antibioticum"
					  },
					  {
					    "key": "@AntManAtc@",
					    "value": "ATC"
					  },
					  {
					    "key": "@AntManB@",
					    "value": "Beheer de lijst met antibiotica"
					  },
					  {
					    "key": "@AntManCid@",
					    "value": "CID"
					  },
					  {
					    "key": "@AntManDel@",
					    "value": "Antibioticum verwijderen"
					  },
					  {
					    "key": "@AntManEdi@",
					    "value": "Antibioticum bewerken"
					  },
					  {
					    "key": "@AntManEditSub@",
					    "value": "bewerk de eigenschappen van het antibioticum"
					  },
					  {
					    "key": "@AntManLoi@",
					    "value": "Loinc"
					  },
					  {
					    "key": "@AntThe@",
					    "value": "Het antibioticum bestaat niet"
					  },
					  {
					    "key": "@AntTheA@",
					    "value": "het antibioticum is al toegevoegd aan deze coderingslijst"
					  },
					  {
					    "key": "@AntThi@",
					    "value": "Deze antibioticagroep bestaat al"
					  },
					  {
					    "key": "@AssAss@",
					    "value": "Asset Tracking"
					  },
					  {
					    "key": "@AssStr@",
					    "value": "opslaglocaties"
					  },
					  {
					    "key": "@AssSup@",
					    "value": "Leveranciers"
					  },
					  {
					    "key": "@AstAdd@",
					    "value": "AST -gegevens toevoegen of updaten voor een cultuur"
					  },
					  {
					    "key": "@AstAnt@",
					    "value": "Antimicrobiële gevoeligheidstests"
					  },
					  {
					    "key": "@AstCom1@",
					    "value": "ast comment1"
					  },
					  {
					    "key": "@AstCom2@",
					    "value": "ast comment2"
					  },
					  {
					    "key": "@AstCom3@",
					    "value": "Comment 1 (opgenomen in Report)"
					  },
					  {
					    "key": "@AstCom4@",
					    "value": "Comment 2 (opgenomen in Report)"
					  },
					  {
					    "key": "@AstCre@",
					    "value": "AST-resultaten maken en beheren"
					  },
					  {
					    "key": "@AstDis@",
					    "value": "Disk Tests"
					  },
					  {
					    "key": "@AstDos@",
					    "value": "Dosering moet niet nul zijn"
					  },
					  {
					    "key": "@AstDup@",
					    "value": "Duplicaat antibioticum"
					  },
					  {
					    "key": "@AstEmp@",
					    "value": "geen antibioticum geselecteerd"
					  },
					  {
					    "key": "@AstEmpA@",
					    "value": "geen resultaten ingevoerd"
					  },
					  {
					    "key": "@AstEnt@",
					    "value": "Voer antibiotica in om te testen, volgens AST -methodetype en testresultaten indien beschikbaar"
					  },
					  {
					    "key": "@AstEnz@",
					    "value": "Weerstandsenzymen"
					  },
					  {
					    "key": "@AstGui@",
					    "value": "Richtlijnen moeten worden gespecificeerd"
					  },
					  {
					    "key": "@AstMan@",
					    "value": "Ast"
					  },
					  {
					    "key": "@AstMea@",
					    "value": "meting buiten bereik"
					  },
					  {
					    "key": "@AstMeaA@",
					    "value": ", voeg een testresultaat toe of verwijder de rij"
					  },
					  {
					    "key": "@AstMicMea@",
					    "value": "MIC-meting moet een geldig getal bevatten"
					  },
					  {
					    "key": "@AstMic@",
					    "value": "MIC TESTS"
					  },
					  {
					    "key": "@AstMicA@",
					    "value": "MIC"
					  },
					  {
					    "key": "@AstPen@",
					    "value": "In afwachting van ID/AST"
					  },
					  {
					    "key": "@AstPre@",
					    "value": "Selecteer aanwezigheid"
					  },
					  {
					    "key": "@AstRem@",
					    "value": "Verwijder onvolledige rijen"
					  },
					  {
					    "key": "@AstRes@",
					    "value": "AST -resultaten"
					  },
					  {
					    "key": "@AstRow@",
					    "value": "Ast Row Removal Bevestiging"
					  },
					  {
					    "key": "@AstTes@",
					    "value": "AST -tests"
					  },
					  {
					    "key": "@AstTesA@",
					    "value": "AST -testpatroon"
					  },
					  {
					    "key": "@AstTesB@",
					    "value": "Matched Test Patronen"
					  },
					  {
					    "key": "@AstTesC@",
					    "value": "Selecteer testpatroon uit de overeenkomende lijst"
					  },
					  {
					    "key": "@AstTesD@",
					    "value": "Selectie van testpatroon"
					  },
					  {
					    "key": "@AstTesE@",
					    "value": "Alle testpatronen"
					  },
					  {
					    "key": "@AstTesF@",
					    "value": "Selecteer testpatroon uit de volledige lijst"
					  },
					  {
					    "key": "@AstUse@",
					    "value": "Patroon gebruiken voor schijftests"
					  },
					  {
					    "key": "@AstUseA@",
					    "value": "Patroon gebruiken voor striptests"
					  },
					  {
					    "key": "@AutErr@",
					    "value": "Gebruikersnaam niet opgegeven"
					  },
					  {
					    "key": "@AutErrA@",
					    "value": "Wachtwoord niet opgegeven"
					  },
					  {
					    "key": "@AutErrB@",
					    "value": "Geen geldige gebruiker"
					  },
					  {
					    "key": "@AutErrC@",
					    "value": "Ongeldig wachtwoord"
					  },
					  {
					    "key": "@AutErrD@",
					    "value": "De gebruikersnaam is niet herkend"
					  },
					  {
					    "key": "@AutErrE@",
					    "value": "Laboratorium of organisatie niet geleverd"
					  },
					  {
					    "key": "@AutErrF@",
					    "value": "U hebt geen toegang tot dit laboratorium"
					  },
					  {
					    "key": "@AutErrG@",
					    "value": "U hebt geen toegang tot deze organisatie"
					  },
					  {
					    "key": "@BarPat1@",
					    "value": "Patiëntlabel 1"
					  },
					  {
					    "key": "@BarPat2@",
					    "value": "Patiëntlabel 2"
					  },
					  {
					    "key": "@BarSpe1@",
					    "value": "Specimen Label 1"
					  },
					  {
					    "key": "@BarSpe2@",
					    "value": "Specimen Label 2"
					  },
					  {
					    "key": "@BatPro@",
					    "value": "Publiceer gecombineerde exemplaarrapporten voor meerdere monsters. Selecteer eerst de lijst met de filters met behulp van de filters, selecteer vervolgens een of meer specimens uit de lijst of selecteer ze allemaal met de knop links van de koppen"
					  },
					  {
					    "key": "@BatRec@",
					    "value": "Records met succes gepubliceerd"
					  },
					  {
					    "key": "@BilBil@",
					    "value": "Billing"
					  },
					  {
					    "key": "@BilBilA@",
					    "value": "Factureringsregels"
					  },
					  {
					    "key": "@BilAdd@",
					    "value": "Factureringsregel Toevoegen"
					  },
					  {
					    "key": "@BilEdi@",
					    "value": "Factureringsregel Bewerken"
					  },
					  {
					    "key": "@BilDel@",
					    "value": "Factureringsregel Verwijderen"
					  },
					  {
					    "key": "@BilRecEdi@",
					    "value": "Factureringsrecord Bewerken"
					  },
					  {
					    "key": "@BilRecDel@",
					    "value": "Factureringsrecord Verwijderen"
					  },
					  {
					    "key": "@BilRecEdiH@",
					    "value": "Werk de omschrijving of het bedrag bij. Andere velden worden ter referentie getoond."
					  },
					  {
					    "key": "@BilRecDelH@",
					    "value": "Typ ter bevestiging van verwijdering de directe testnaam hieronder."
					  },
					  {
					    "key": "@BreA@",
					    "value": "Een exemplaartype moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAA@",
					    "value": "Een host moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAB@",
					    "value": "Een testmethode moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAC@",
					    "value": "een gevoeligheid moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAD@",
					    "value": "Een testpatroonnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAdd@",
					    "value": "Breakpoint toevoegen"
					  },
					  {
					    "key": "@BreAddA@",
					    "value": "Een nieuw breekpunt toevoegen"
					  },
					  {
					    "key": "@BreAddB@",
					    "value": "Testmethode toevoegen"
					  },
					  {
					    "key": "@BreAddC@",
					    "value": "Host toevoegen"
					  },
					  {
					    "key": "@BreAddD@",
					    "value": "Voeg een nieuwe testmethode toe"
					  },
					  {
					    "key": "@BreAddE@",
					    "value": "Voeg een nieuwe host toe"
					  },
					  {
					    "key": "@BreAddF@",
					    "value": "Voeg gevoeligheid toe"
					  },
					  {
					    "key": "@BreAddG@",
					    "value": "Voeg een nieuwe gevoeligheid toe"
					  },
					  {
					    "key": "@BreAddH@",
					    "value": "Set Criteria"
					  },
					  {
					    "key": "@BreAddI@",
					    "value": "Definieer de andere kenmerken waarvoor het breekpunt van toepassing zal zijn"
					  },
					  {
					    "key": "@BreAddJ@",
					    "value": "Breekpunt definiëren"
					  },
					  {
					    "key": "@BreAddK@",
					    "value": "Stel het meetbereik in voor elke gevoeligheidsbepaling"
					  },
					  {
					    "key": "@BreAddL@",
					    "value": "Breekpunt bewerken"
					  },
					  {
					    "key": "@BreAddM@",
					    "value": "Criteria bewerken"
					  },
					  {
					    "key": "@BreAddN@",
					    "value": "Bewerk de andere kenmerken waarvoor het breekpunt van toepassing is"
					  },
					  {
					    "key": "@BreAddO@",
					    "value": "Breakpoint toevoegen"
					  },
					  {
					    "key": "@BreAddP@",
					    "value": "Testmethode toevoegen"
					  },
					  {
					    "key": "@BreAddQ@",
					    "value": "Host toevoegen"
					  },
					  {
					    "key": "@BreAddR@",
					    "value": "Voeg gevoeligheid toe"
					  },
					  {
					    "key": "@BreAE@",
					    "value": "Een dosering moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAF@",
					    "value": "Een breekpuntbron moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAG@",
					    "value": "Speciale overwegingen moeten worden ingevoerd"
					  },
					  {
					    "key": "@BreAn@",
					    "value": "een antibioticum moet worden ingevoerd"
					  },
					  {
					    "key": "@BreAnA@",
					    "value": "Een bestelling moet worden ingevoerd"
					  },
					  {
					    "key": "@BreBre@",
					    "value": "Breakpoints"
					  },
					  {
					    "key": "@BreCan@",
					    "value": "kan niet verwijderen omdat deze host in gebruik is"
					  },
					  {
					    "key": "@BreCanA@",
					    "value": "kan niet verwijderen omdat deze testmethode in gebruik is"
					  },
					  {
					    "key": "@BreCanB@",
					    "value": "kan niet verwijderen omdat deze gevoeligheid in gebruik is"
					  },
					  {
					    "key": "@BreDel@",
					    "value": "Breakpoint verwijderen"
					  },
					  {
					    "key": "@BreDelA@",
					    "value": "Een breekpunt verwijderen"
					  },
					  {
					    "key": "@BreDelB@",
					    "value": "Testmethode verwijderen"
					  },
					  {
					    "key": "@BreDelC@",
					    "value": "Host verwijderen"
					  },
					  {
					    "key": "@BreDelD@",
					    "value": "Een bestaande testmethode verwijderen"
					  },
					  {
					    "key": "@BreDelE@",
					    "value": "Gevoeligheid verwijderen"
					  },
					  {
					    "key": "@BreDelF@",
					    "value": "Een bestaande gevoeligheid verwijderen"
					  },
					  {
					    "key": "@BreDelG@",
					    "value": "Breakpoint verwijderen"
					  },
					  {
					    "key": "@BreDelH@",
					    "value": "Testmethode verwijderen"
					  },
					  {
					    "key": "@BreDelI@",
					    "value": "Host verwijderen"
					  },
					  {
					    "key": "@BreDelJ@",
					    "value": "Gevoeligheid verwijderen"
					  },
					  {
					    "key": "@BreEdi@",
					    "value": "Breakpoint bewerken"
					  },
					  {
					    "key": "@BreEdiA@",
					    "value": "Een bestaand breekpunt bewerken"
					  },
					  {
					    "key": "@BreEdiB@",
					    "value": "Testmethode bewerken"
					  },
					  {
					    "key": "@BreEdiC@",
					    "value": "Host bewerken"
					  },
					  {
					    "key": "@BreEdiD@",
					    "value": "Een bestaande testmethode bewerken"
					  },
					  {
					    "key": "@BreEdiE@",
					    "value": "Een bestaande host bewerken"
					  },
					  {
					    "key": "@BreEdiF@",
					    "value": "Sulatie bewerken"
					  },
					  {
					    "key": "@BreEdiG@",
					    "value": "Bewerk een bestaande gevoeligheid"
					  },
					  {
					    "key": "@BreEdiH@",
					    "value": "Breakpoint bewerken"
					  },
					  {
					    "key": "@BreEdiI@",
					    "value": "Testmethode bewerken"
					  },
					  {
					    "key": "@BreEdiJ@",
					    "value": "Host bewerken"
					  },
					  {
					    "key": "@BreEdiK@",
					    "value": "Susceptibility bewerken"
					  },
					  {
					    "key": "@BreHos@",
					    "value": "Hostlijst"
					  },
					  {
					    "key": "@BreMan@",
					    "value": "Breekpunten beheren"
					  },
					  {
					    "key": "@BreManA@",
					    "value": "Lijst met breekpunten beheren"
					  },
					  {
					    "key": "@BreNod@",
					    "value": "Geen breekpuntgegevens"
					  },
					  {
					    "key": "@BreOve@",
					    "value": "Overlappende zones"
					  },
					  {
					    "key": "@BreSou@",
					    "value": "Bron"
					  },
					  {
					    "key": "@BreSouA@",
					    "value": "No Breakpoint Source"
					  },
					  {
					    "key": "@BreSpe@",
					    "value": "Speciale overwegingen"
					  },
					  {
					    "key": "@BreSpeA@",
					    "value": "Special"
					  },
					  {
					    "key": "@BreSpeB@",
					    "value": "Special Accessation"
					  },
					  {
					    "key": "@BreTes@",
					    "value": "Test Method"
					  },
					  {
					    "key": "@BreTesA@",
					    "value": "Test Methods"
					  },
					  {
					    "key": "@BreTesB@",
					    "value": "Test Method Precedence"
					  },
					  {
					    "key": "@BreThe@",
					    "value": "De host die u wilt wijzigen, moet worden geselecteerd"
					  },
					  {
					    "key": "@BreTheA@",
					    "value": "De testmethode die u wilt wijzigen, moet worden geselecteerd"
					  },
					  {
					    "key": "@BreTheB@",
					    "value": "De vatbaarheid die u wilt wijzigen, moet worden geselecteerd"
					  },
					  {
					    "key": "@BreThi@",
					    "value": "Deze testmethode bestaat al"
					  },
					  {
					    "key": "@BreThiA@",
					    "value": "Deze host bestaat al"
					  },
					  {
					    "key": "@BreThiB@",
					    "value": "Deze gevoeligheid bestaat al"
					  },
					  {
					    "key": "@BreLin@",
					    "value": "Breekpuntenlijnen"
					  },
					  {
					    "key": "@BreSta@",
					    "value": "Startwaarde"
					  },
					  {
					    "key": "@BreEnd@",
					    "value": "Eindwaarde"
					  },
					  {
					    "key": "@BreAppHis@",
					    "value": "Goedkeuringsgeschiedenis"
					  },
					  {
					    "key": "@BreAddApp@",
					    "value": "Goedkeuring/Afwijzing toevoegen"
					  },
					  {
					    "key": "@BreAddAppB@",
					    "value": "Goedkeuring/Afwijzing toevoegen"
					  },
					  {
					    "key": "@BreDatRec@",
					    "value": "Datum vastgelegd"
					  },
					  {
					    "key": "@BreRecBy@",
					    "value": "Vastgelegd door"
					  },
					  {
					    "key": "@BreId@",
					    "value": "Id"
					  },
					  {
					    "key": "@CfgLab1@",
					    "value": "velden met"
					  },
					  {
					    "key": "@CfgLab@",
					    "value": "itemafmetingen"
					  },
					  {
					    "key": "@CfgLabA@",
					    "value": "Configureer de dimensies van elke labelcomponent"
					  },
					  {
					    "key": "@CfgLabB@",
					    "value": "Hoogte van lineaire (1D) barcode"
					  },
					  {
					    "key": "@CfgLabC@",
					    "value": "Grootte van QR -code"
					  },
					  {
					    "key": "@CfgLabD@",
					    "value": "Barcodes naast elkaar weergeven (indien beide ingeschakeld)?"
					  },
					  {
					    "key": "@CfgLabE@",
					    "value": "Veldlettertype -grootte"
					  },
					  {
					    "key": "@CfgLabF@",
					    "value": "vulling rond barcodes"
					  },
					  {
					    "key": "@CfgLabG@",
					    "value": "Breedte van veldnamen"
					  },
					  {
					    "key": "@CfgLabH@",
					    "value": "Totale breedte van velden"
					  },
					  {
					    "key": "@CfgLabI@",
					    "value": "Labelafmetingen"
					  },
					  {
					    "key": "@CfgLabJ@",
					    "value": "Configureer de afmetingen van elk label"
					  },
					  {
					    "key": "@CfgLabK@",
					    "value": "Linksmarge per label"
					  },
					  {
					    "key": "@CfgLabL@",
					    "value": "Topmarge per label"
					  },
					  {
					    "key": "@CfgLabM@",
					    "value": "Breedte van elk label"
					  },
					  {
					    "key": "@CfgLabN@",
					    "value": "Hoogte van elk label"
					  },
					  {
					    "key": "@CfgLabO@",
					    "value": "Bottomvulling per label"
					  },
					  {
					    "key": "@CfgLabP@",
					    "value": "Label Content"
					  },
					  {
					    "key": "@CfgLabQ@",
					    "value": "Configureer de items die aanwezig zijn in elk label"
					  },
					  {
					    "key": "@CfgLabR@",
					    "value": "Naam"
					  },
					  {
					    "key": "@CfgLabS@",
					    "value": "Linear Barcode opnemen?"
					  },
					  {
					    "key": "@CfgLabT@",
					    "value": "QR -code opnemen?"
					  },
					  {
					    "key": "@CfgLabU@",
					    "value": "Barcode als een nummer opnemen?"
					  },
					  {
					    "key": "@CfgLabUseAcc@",
					    "value": "Toegangsnummer gebruiken voor streepjescode?"
					  },
					  {
					    "key": "@CfgLabV@",
					    "value": "Velden om op te nemen"
					  },
					  {
					    "key": "@CfgLabW@",
					    "value": "omvatten een rand rond elk label?"
					  },
					  {
					    "key": "@CfgLabX@",
					    "value": "Aantal labels per rij"
					  },
					  {
					    "key": "@CfgLabY@",
					    "value": "Aantal labelrijen"
					  },
					  {
					    "key": "@CfgLabZ@",
					    "value": "EDIT LABEL"
					  },
					  {
					    "key": "@CodA@",
					    "value": "Een organismegroep moet worden ingevoerd"
					  },
					  {
					    "key": "@CodAA@",
					    "value": "Een aangepaste naam moet worden ingevoerd"
					  },
					  {
					    "key": "@CodAB@",
					    "value": "Een organisme -groep moet worden geselecteerd"
					  },
					  {
					    "key": "@CodAC@",
					    "value": "een code moet worden ingevoerd"
					  },
					  {
					    "key": "@CodAD@",
					    "value": "een brontoonsnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@CodAdd@",
					    "value": "Coderingslijst toevoegen"
					  },
					  {
					    "key": "@CodAddA@",
					    "value": "Aangepast organisme toevoegen"
					  },
					  {
					    "key": "@CodAddB@",
					    "value": "Voeg een aangepast organisme toe aan de huidige organisme -lijst. Hoewel taxonomische reikwijdte optioneel is, moet het, indien gespecificeerd, ten minste bestelling en familie bevatten"
					  },
					  {
					    "key": "@CodAddC@",
					    "value": "een nieuwe coderingslijst toevoegen"
					  },
					  {
					    "key": "@CodAddD@",
					    "value": "Groep toevoegen"
					  },
					  {
					    "key": "@CodAddE@",
					    "value": "Voeg Organism Group toe"
					  },
					  {
					    "key": "@CodAddF@",
					    "value": "Voeg een nieuwe organisme -groep toe"
					  },
					  {
					    "key": "@CodAddG@",
					    "value": "Bron toevoegen"
					  },
					  {
					    "key": "@CodAddH@",
					    "value": "Een nieuwe bron toevoegen"
					  },
					  {
					    "key": "@CodAddI@",
					    "value": "Bron toevoegen"
					  },
					  {
					    "key": "@CodAE@",
					    "value": "Een bron moet worden geselecteerd"
					  },
					  {
					    "key": "@CodAss@",
					    "value": "Code toewijzen"
					  },
					  {
					    "key": "@CodAssA@",
					    "value": "een code toewijzen aan het geselecteerde organisme"
					  },
					  {
					    "key": "@CodCOM@",
					    "value": "Comru"
					  },
					  {
					    "key": "@CodDel@",
					    "value": "Codeertlijst verwijderen"
					  },
					  {
					    "key": "@CodDelA@",
					    "value": "Verwijder een bestaande organisme -groep en al zijn inhoud"
					  },
					  {
					    "key": "@CodDelB@",
					    "value": "Delete Organisme"
					  },
					  {
					    "key": "@CodDelC@",
					    "value": "Verwijder een organisme of aangepaste vermelding van de Organism Group"
					  },
					  {
					    "key": "@CodDelD@",
					    "value": "Delete Group"
					  },
					  {
					    "key": "@CodDelE@",
					    "value": "Delete Organism Group"
					  },
					  {
					    "key": "@CodDelF@",
					    "value": "Bron verwijderen"
					  },
					  {
					    "key": "@CodDelG@",
					    "value": "Verwijder een bestaande bron en al zijn inhoud"
					  },
					  {
					    "key": "@CodDelH@",
					    "value": "Bron verwijderen"
					  },
					  {
					    "key": "@CodEdi@",
					    "value": "Organisme bewerken"
					  },
					  {
					    "key": "@CodEdiA@",
					    "value": "Custom Organism Code bewerken"
					  },
					  {
					    "key": "@CodGra@",
					    "value": "Gram"
					  },
					  {
					    "key": "@CodLis@",
					    "value": "Lijst met organismen die overeenkomen met de zoekcriteria"
					  },
					  {
					    "key": "@CodNew@",
					    "value": "Nieuwe entry"
					  },
					  {
					    "key": "@CodSco@",
					    "value": "Selecteer de toepasselijke organisme -reikwijdte door taxonomie of genaamd Organism Group"
					  },
					  {
					    "key": "@CodScoA@",
					    "value": "bewerk de toepasselijke organisme scope door taxonomie of genoemde organisme Group"
					  },
					  {
					    "key": "@CodSel@",
					    "value": "Selecteer het te gebruiken organisme"
					  },
					  {
					    "key": "@CodSelA@",
					    "value": "Selecteer Geslacht"
					  },
					  {
					    "key": "@CodSelB@",
					    "value": "SELECTEER SPECTEN"
					  },
					  {
					    "key": "@CodSelC@",
					    "value": "SELECT SEROTYPE"
					  },
					  {
					    "key": "@CodSelD@",
					    "value": "SELECTEERDE FIELDS"
					  },
					  {
					    "key": "@CodSelE@",
					    "value": "Selecteer velden om op rapport weer te geven"
					  },
					  {
					    "key": "@CodSou@",
					    "value": "Bronnaam"
					  },
					  {
					    "key": "@CodThe@",
					    "value": "Het organisme bestaat niet"
					  },
					  {
					    "key": "@CodTheA@",
					    "value": "het organisme is al aan deze coderingslijst toegevoegd"
					  },
					  {
					    "key": "@CodThi@",
					    "value": "Deze organisme -groep bestaat al"
					  },
					  {
					    "key": "@CodThiA@",
					    "value": "Deze coderingslijst kan niet worden verwijderd"
					  },
					  {
					    "key": "@CodThiB@",
					    "value": "dit item bestaat al in de lijst"
					  },
					  {
					    "key": "@CodThiC@",
					    "value": "Deze bron bestaat al"
					  },
					  {
					    "key": "@CodThiD@",
					    "value": "deze bron kan niet worden verwijderd"
					  },
					  {
					    "key": "@CodThiE@",
					    "value": "Deze bron bevat breekpunten en kan niet worden verwijderd"
					  },
					  {
					    "key": "@CodThiF@",
					    "value": "Deze bron bevat testpatronen en kan niet worden verwijderd"
					  },
					  {
					    "key": "@CodThiG@",
					    "value": "deze coderingslijst bevat vermeldingen en kan niet worden verwijderd"
					  },
					  {
					    "key": "@CodVal@",
					    "value": "als het opgeven van taxonomische reikwijdte, moet tenminste bestelling en familie worden opgenomen"
					  },
					  {
					    "key": "@ConA@",
					    "value": "Een cultuurtype moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAA@",
					    "value": "Een directe test moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAB@",
					    "value": "Een nieuwe staat moet worden ingevoerd"
					  },
					  {
					    "key": "@ConAC@",
					    "value": "Een staat moet worden ingevoerd"
					  },
					  {
					    "key": "@ConAD@",
					    "value": "Er bestaat al een workflowstap voor dit evenement"
					  },
					  {
					    "key": "@ConAdd@",
					    "value": "Formulier toevoegen"
					  },
					  {
					    "key": "@ConAddA@",
					    "value": "Cultuurtest toevoegen"
					  },
					  {
					    "key": "@ConAddAA@",
					    "value": "Een nieuwe cultuurtestinstelling toevoegen"
					  },
					  {
					    "key": "@ConAddAB@",
					    "value": "Workflow toevoegen"
					  },
					  {
					    "key": "@ConAddAC@",
					    "value": "Workflow toevoegen"
					  },
					  {
					    "key": "@ConAddAD@",
					    "value": "Een nieuwe workflow maken"
					  },
					  {
					    "key": "@ConAddB@",
					    "value": "Directe test toevoegen"
					  },
					  {
					    "key": "@ConAddC@",
					    "value": "Menu -optie toevoegen"
					  },
					  {
					    "key": "@ConAddD@",
					    "value": "Cultuurtype toevoegen standaard"
					  },
					  {
					    "key": "@ConAddE@",
					    "value": "Directe test standaard toevoegen"
					  },
					  {
					    "key": "@ConAddF@",
					    "value": "Een instelling van het type cultuur toevoegen"
					  },
					  {
					    "key": "@ConAddG@",
					    "value": "Testdefinitie toevoegen"
					  },
					  {
					    "key": "@ConAddH@",
					    "value": "NB: machtigingen moeten worden toegevoegd voor de nieuwe test !!"
					  },
					  {
					    "key": "@ConAddI@",
					    "value": "Field Definition toevoegen"
					  },
					  {
					    "key": "@ConAddJ@",
					    "value": "Field Definition toevoegen"
					  },
					  {
					    "key": "@ConAddK@",
					    "value": "Een nieuwe velddefinitie toevoegen"
					  },
					  {
					    "key": "@ConAddL@",
					    "value": "Nieuw rapport toevoegen"
					  },
					  {
					    "key": "@ConAddM@",
					    "value": "Een nieuwe rapportdefinitie toevoegen"
					  },
					  {
					    "key": "@ConAddN@",
					    "value": "Reportsectie toevoegen"
					  },
					  {
					    "key": "@ConAddO@",
					    "value": "Een nieuwe rapportsectie toevoegen"
					  },
					  {
					    "key": "@ConAddP@",
					    "value": "State toevoegen"
					  },
					  {
					    "key": "@ConAddQ@",
					    "value": "Workflow -invoer toevoegen"
					  },
					  {
					    "key": "@ConAddR@",
					    "value": "Voeg een nieuwe status toe"
					  },
					  {
					    "key": "@ConAddS@",
					    "value": "Staat toevoegen"
					  },
					  {
					    "key": "@ConAddT@",
					    "value": "Workflow Rule toevoegen"
					  },
					  {
					    "key": "@ConAddU@",
					    "value": "een nieuwe regel toevoegen aan de workflow"
					  },
					  {
					    "key": "@ConAddV@",
					    "value": "een directe testinstelling toevoegen"
					  },
					  {
					    "key": "@ConAddW@",
					    "value": "Pagina toevoegen"
					  },
					  {
					    "key": "@ConAddX@",
					    "value": "Pagina toevoegen"
					  },
					  {
					    "key": "@ConAddY@",
					    "value": "Een nieuwe paginadefinitie toevoegen"
					  },
					  {
					    "key": "@ConAddZ@",
					    "value": "Cultuurtest Standaard toevoegen"
					  },
					  {
					    "key": "@ConAde@",
					    "value": "Een beschrijving moet worden ingevoerd"
					  },
					  {
					    "key": "@ConAE@",
					    "value": "Een cultuurtest moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAfi@",
					    "value": "ontbreekt een veldbreedte in de roosterdefinitie"
					  },
					  {
					    "key": "@ConAla@",
					    "value": "Een label moet worden ingevoerd"
					  },
					  {
					    "key": "@ConAlaA@",
					    "value": "labelnaam"
					  },
					  {
					    "key": "@ConAli@",
					    "value": "een lijst moet worden opgegeven voor dit veldtype"
					  },
					  {
					    "key": "@ConAliA@",
					    "value": "Een lijst is niet opgegeven voor een van de vervolgkeuzelijsten in het raster"
					  },
					  {
					    "key": "@ConAna@",
					    "value": "Er ontbreekt een naam in de rasterdefinitie"
					  },
					  {
					    "key": "@ConAne@",
					    "value": "Een gebeurtenis moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAneA@",
					    "value": "Een instapstatus moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAni@",
					    "value": "Een identificatie moet worden ingevoerd"
					  },
					  {
					    "key": "@ConApa@",
					    "value": "Een paginanaam moet worden ingevoerd"
					  },
					  {
					    "key": "@ConAtl@",
					    "value": "ten minste één veld moet worden toegevoegd aan het rooster"
					  },
					  {
					    "key": "@ConAty@",
					    "value": "Een veldtype moet worden geselecteerd"
					  },
					  {
					    "key": "@ConAtyA@",
					    "value": "Een veldtype ontbreekt in de rasterdefinitie"
					  },
					  {
					    "key": "@ConAut@",
					    "value": "Automatische statusovergangen"
					  },
					  {
					    "key": "@ConBot@",
					    "value": "Onderste secties"
					  },
					  {
					    "key": "@ConCan@",
					    "value": "kan niet verwijderen omdat deze status in gebruik is"
					  },
					  {
					    "key": "@ConCha@",
					    "value": "Pagina -bestelling wijzigen"
					  },
					  {
					    "key": "@ConCon@",
					    "value": "Conditional Exit States"
					  },
					  {
					    "key": "@ConConA@",
					    "value": "Configuratiegeschiedenis"
					  },
					  {
					    "key": "@ConConB@",
					    "value": "Geschiedenis van configuratiewijzigingen beheren"
					  },
					  {
					    "key": "@ConConC@",
					    "value": "Configuratiebestand om te uploaden"
					  },
					  {
					    "key": "@ConCre@",
					    "value": "Nieuwe en bestaande vormdefinities maken en beheren"
					  },
					  {
					    "key": "@ConCreA@",
					    "value": "Nieuwe en bestaande workflows maken en beheren"
					  },
					  {
					    "key": "@ConCul@",
					    "value": "Cultuurtype lijst"
					  },
					  {
					    "key": "@ConCulA@",
					    "value": "Cultuurtype -instelling"
					  },
					  {
					    "key": "@ConCulB@",
					    "value": "Cultuurtestlijst"
					  },
					  {
					    "key": "@ConCulC@",
					    "value": "Cultuurtype Cultuur Test Standaardwaarden"
					  },
					  {
					    "key": "@ConCur@",
					    "value": "Huidige status"
					  },
					  {
					    "key": "@ConDat@",
					    "value": "Informatiesectie"
					  },
					  {
					    "key": "@ConDec@",
					    "value": "Decimal Points"
					  },
					  {
					    "key": "@ConDef@",
					    "value": "Pagina -inhoud definiëren"
					  },
					  {
					    "key": "@ConDefA@",
					    "value": "Standaard tot nu"
					  },
					  {
					    "key": "@ConDefB@",
					    "value": "Rapport definiëren"
					  },
					  {
					    "key": "@ConDefC@",
					    "value": "Standaard exitstatus"
					  },
					  {
					    "key": "@ConDefD@",
					    "value": "Standaardtaal"
					  },
					  {
					    "key": "@ConDefE@",
					    "value": "Workflow definiëren"
					  },
					  {
					    "key": "@ConDel@",
					    "value": "Formulier verwijderen"
					  },
					  {
					    "key": "@ConDelA@",
					    "value": "Isolaattest verwijderen"
					  },
					  {
					    "key": "@ConDelAA@",
					    "value": "Cultuurtype Cultuur Test Delete Culture Test Standaard"
					  },
					  {
					    "key": "@ConDelAB@",
					    "value": "Delete Culture Type Culture Test Standaard"
					  },
					  {
					    "key": "@ConDelAC@",
					    "value": "Workflow verwijderen"
					  },
					  {
					    "key": "@ConDelAD@",
					    "value": "Workflow verwijderen"
					  },
					  {
					    "key": "@ConDelAE@",
					    "value": "Een bestaande workflow verwijderen"
					  },
					  {
					    "key": "@ConDelB@",
					    "value": "Direct Test verwijderen"
					  },
					  {
					    "key": "@ConDelC@",
					    "value": "Menu -optie verwijderen"
					  },
					  {
					    "key": "@ConDelD@",
					    "value": "Instelling van het type cultuur verwijderen"
					  },
					  {
					    "key": "@ConDelE@",
					    "value": "Direct Test Standaard verwijderen"
					  },
					  {
					    "key": "@ConDelF@",
					    "value": "Een instelling van een cultuurtype verwijderen"
					  },
					  {
					    "key": "@ConDelG@",
					    "value": "een directe test verwijderen"
					  },
					  {
					    "key": "@ConDelH@",
					    "value": "Testdefinitie verwijderen"
					  },
					  {
					    "key": "@ConDelI@",
					    "value": "Een bestaande testdefinitie verwijderen"
					  },
					  {
					    "key": "@ConDelJ@",
					    "value": "Field Definitie verwijderen"
					  },
					  {
					    "key": "@ConDelK@",
					    "value": "Field verwijderen"
					  },
					  {
					    "key": "@ConDelL@",
					    "value": "dit veld verwijderen uit de pagina"
					  },
					  {
					    "key": "@ConDelM@",
					    "value": "Een bestaand rapport verwijderen"
					  },
					  {
					    "key": "@ConDelN@",
					    "value": "Rapport verwijderen"
					  },
					  {
					    "key": "@ConDelO@",
					    "value": "Reportsectie verwijderen"
					  },
					  {
					    "key": "@ConDelP@",
					    "value": "Een bestaand rapportgedeelte verwijderen"
					  },
					  {
					    "key": "@ConDelQ@",
					    "value": "State Delete"
					  },
					  {
					    "key": "@ConDelR@",
					    "value": "Workflow Entry Delete"
					  },
					  {
					    "key": "@ConDelS@",
					    "value": "State Delete State"
					  },
					  {
					    "key": "@ConDelT@",
					    "value": "Een bestaande staat verwijderen"
					  },
					  {
					    "key": "@ConDelU@",
					    "value": "State Delete State"
					  },
					  {
					    "key": "@ConDelW@",
					    "value": "Page verwijderen"
					  },
					  {
					    "key": "@ConDelX@",
					    "value": "Pagina verwijderen"
					  },
					  {
					    "key": "@ConDelY@",
					    "value": "Een bestaande paginadefinitie verwijderen"
					  },
					  {
					    "key": "@ConDelZ@",
					    "value": "DUNTE DIT DEZE SECTIE uit het rapport"
					  },
					  {
					    "key": "@ConDir@",
					    "value": "Directe testlijst"
					  },
					  {
					    "key": "@ConDirA@",
					    "value": "Directe tests"
					  },
					  {
					    "key": "@ConDirB@",
					    "value": "Directe testinstellingen"
					  },
					  {
					    "key": "@ConDirC@",
					    "value": "Voer de nieuwe testnaam in"
					  },
					  {
					    "key": "@ConDirD@",
					    "value": "Voer een beschrijving in om te verschijnen op de eerste pagina"
					  },
					  {
					    "key": "@ConDis@",
					    "value": "Culture Test uitschakelen"
					  },
					  {
					    "key": "@ConDisA@",
					    "value": "Directe test uitschakelen"
					  },
					  {
					    "key": "@ConDisB@",
					    "value": "Testdefinitie uitschakelen"
					  },
					  {
					    "key": "@ConDisC@",
					    "value": "Een bestaande testdefinitie uitschakelen"
					  },
					  {
					    "key": "@ConEdi@",
					    "value": "Formulier bewerken"
					  },
					  {
					    "key": "@ConEdiA@",
					    "value": "Isolaattest bewerken"
					  },
					  {
					    "key": "@ConEdiAA@",
					    "value": "Standaard isolaattest bewerken"
					  },
					  {
					    "key": "@ConEdiAB@",
					    "value": "Workflow bewerken"
					  },
					  {
					    "key": "@ConEdiAC@",
					    "value": "Workflow bewerken"
					  },
					  {
					    "key": "@ConEdiAD@",
					    "value": "Een workflowbeschrijving bewerken"
					  },
					  {
					    "key": "@ConEdiB@",
					    "value": "Directe test bewerken"
					  },
					  {
					    "key": "@ConEdiC@",
					    "value": "Menu -optie bewerken"
					  },
					  {
					    "key": "@ConEdiD@",
					    "value": "Standaard bewerken Cultuurtype"
					  },
					  {
					    "key": "@ConEdiE@",
					    "value": "Direct Test Standaard bewerken"
					  },
					  {
					    "key": "@ConEdiDT@",
					    "value": "Directe testdefinitie bewerken"
					  },
					  {
					    "key": "@ConEdiDTA@",
					    "value": "Een bestaande directe testdefinitie bewerken"
					  },
					  {
					    "key": "@ConEdiF@",
					    "value": "Definitie van isolaat Test bewerken"
					  },
					  {
					    "key": "@ConEdiG@",
					    "value": "Een bestaande cultuurtestdefinitie bewerken"
					  },
					  {
					    "key": "@ConEdiH@",
					    "value": "Definitie van het veld bewerken"
					  },
					  {
					    "key": "@ConEdiI@",
					    "value": "Field Definition bewerken"
					  },
					  {
					    "key": "@ConEdiJ@",
					    "value": "Een bestaande velddefinitie bewerken"
					  },
					  {
					    "key": "@ConEdiK@",
					    "value": "Field bewerken"
					  },
					  {
					    "key": "@ConEdiL@",
					    "value": "een bestaand rapport bewerken"
					  },
					  {
					    "key": "@ConEdiM@",
					    "value": "Rapport bewerken"
					  },
					  {
					    "key": "@ConEdiN@",
					    "value": "Reportsectie bewerken"
					  },
					  {
					    "key": "@ConEdiO@",
					    "value": "Een bestaand rapportgedeelte bewerken"
					  },
					  {
					    "key": "@ConEdiP@",
					    "value": "Staat bewerken"
					  },
					  {
					    "key": "@ConEdiQ@",
					    "value": "Workflow Entry bewerken"
					  },
					  {
					    "key": "@ConEdiR@",
					    "value": "een bestaande staat bewerken"
					  },
					  {
					    "key": "@ConEdiS@",
					    "value": "Status bewerken"
					  },
					  {
					    "key": "@ConEdiT@",
					    "value": "Werkstroomregel bewerken"
					  },
					  {
					    "key": "@ConEdiU@",
					    "value": "Bewerk een bestaande werkstroomregel"
					  },
					  {
					    "key": "@ConEdiV@",
					    "value": "Verwijder werkstroomregel"
					  },
					  {
					    "key": "@ConEdiW@",
					    "value": "Verwijder een bestaande werkstroomregel"
					  },
					  {
					    "key": "@ConEdiX@",
					    "value": "Pagina bewerken"
					  },
					  {
					    "key": "@ConEdiY@",
					    "value": "Pagina bewerken"
					  },
					  {
					    "key": "@ConEdiZ@",
					    "value": "Bestaande paginadefinitie bewerken"
					  },
					  {
					    "key": "@ConEnt@",
					    "value": "Invoerstatussen"
					  },
					  {
					    "key": "@ConEntA@",
					    "value": "ID invoeren"
					  },
					  {
					    "key": "@ConEntB@",
					    "value": "ENTER LABEL"
					  },
					  {
					    "key": "@ConEntC@",
					    "value": "Entry State"
					  },
					  {
					    "key": "@ConEntD@",
					    "value": "Validatiebericht invoeren"
					  },
					  {
					    "key": "@ConExi@",
					    "value": "Exit State"
					  },
					  {
					    "key": "@ConExp@",
					    "value": "Configuratie exporteren naar bestand"
					  },
					  {
					    "key": "@ConExpA@",
					    "value": "Configuratie exporteren"
					  },
					  {
					    "key": "@ConExpB@",
					    "value": "Configuratie -instellingen exporteren"
					  },
					  {
					    "key": "@ConExpC@",
					    "value": "Configuratie -instellingen exporteren naar een bestand"
					  },
					  {
					    "key": "@ConFie@",
					    "value": "Field Grid"
					  },
					  {
					    "key": "@ConFor@",
					    "value": "formuliernaam"
					  },
					  {
					    "key": "@ConGri@",
					    "value": "Rasterkolommen kunnen niet dezelfde naam hebben"
					  },
					  {
					    "key": "@ConHea@",
					    "value": "koptekst"
					  },
					  {
					    "key": "@ConIde@",
					    "value": "Identifier"
					  },
					  {
					    "key": "@ConImp@",
					    "value": "Configuratie importeren uit bestand"
					  },
					  {
					    "key": "@ConImpA@",
					    "value": "Configuratie importeren"
					  },
					  {
					    "key": "@ConImpB@",
					    "value": "Configuratie -instellingen importeren"
					  },
					  {
					    "key": "@ConImpC@",
					    "value": "Configuratie -instellingen importeren vanuit een bestand"
					  },
					  {
					    "key": "@ConMan@",
					    "value": "Formedefinities beheren"
					  },
					  {
					    "key": "@ConManA@",
					    "value": "Reportsectievelden beheren"
					  },
					  {
					    "key": "@ConManB@",
					    "value": "Sectie beheren"
					  },
					  {
					    "key": "@ConMen@",
					    "value": "Menu -opties"
					  },
					  {
					    "key": "@ConMul@",
					    "value": "Multiselect?"
					  },
					  {
					    "key": "@ConNew@",
					    "value": "Nieuwe Staat"
					  },
					  {
					    "key": "@ConOrg@",
					    "value": "Organisme Secties"
					  },
					  {
					    "key": "@ConPag@",
					    "value": "Page Order"
					  },
					  {
					    "key": "@ConPagA@",
					    "value": "Pagina bestelling"
					  },
					  {
					    "key": "@ConPar@",
					    "value": "Ouderknop"
					  },
					  {
					    "key": "@ConRep@",
					    "value": "Rapport aan kloon"
					  },
					  {
					    "key": "@ConReq@",
					    "value": "Vereist bericht"
					  },
					  {
					    "key": "@ConScr@",
					    "value": "Inactiviteitstime -out"
					  },
					  {
					    "key": "@ConSel@",
					    "value": "Selecteer Test"
					  },
					  {
					    "key": "@ConSelA@",
					    "value": "Selecteer Type"
					  },
					  {
					    "key": "@ConSelB@",
					    "value": "Lijst selecteren"
					  },
					  {
					    "key": "@ConSelC@",
					    "value": "Selecteer Standaard"
					  },
					  {
					    "key": "@ConSelD@",
					    "value": "Selecteer Rapport"
					  },
					  {
					    "key": "@ConSelE@",
					    "value": "Selecteer Entry States"
					  },
					  {
					    "key": "@ConSelF@",
					    "value": "Selecteer Workflow"
					  },
					  {
					    "key": "@ConSou@",
					    "value": "Brongegevens"
					  },
					  {
					    "key": "@ConSpe@",
					    "value": "Specimen Type Cultuur Type Standaards"
					  },
					  {
					    "key": "@ConSpeA@",
					    "value": "Specimen Type Directe test Standaardwaarden"
					  },
					  {
					    "key": "@ConSpeB@",
					    "value": "Cultuurtype Cultuurtest Standaardwaarden"
					  },
					  {
					    "key": "@ConSta@",
					    "value": "Staat verandert"
					  },
					  {
					    "key": "@ConTes@",
					    "value": "Test to Clone"
					  },
					  {
					    "key": "@ConThe@",
					    "value": "De staat die u wilt wijzigen, moet worden geselecteerd"
					  },
					  {
					    "key": "@ConThi@",
					    "value": "Deze staat bestaat al"
					  },
					  {
					    "key": "@ConTit@",
					    "value": "A titel moet worden ingevoerd"
					  },
					  {
					    "key": "@ConTop@",
					    "value": "Top Secties"
					  },
					  {
					    "key": "@ConWor@",
					    "value": "Workflow Steps"
					  },
					  {
					    "key": "@ConWorA@",
					    "value": "Workflow States"
					  },
					  {
					    "key": "@ConWorClo@",
					    "value": "Workflow to Clone"
					  },
					  {
					    "key": "@ConYou@",
					    "value": "U moet een test selecteren om te kloon"
					  },
					  {
					    "key": "@ConYouA@",
					    "value": "U kunt geen test verwijderen die in gebruik is"
					  },
					  {
					    "key": "@CulAdd@",
					    "value": "Voeg extra isolaatinformatie toe"
					  },
					  {
					    "key": "@CulBar@",
					    "value": "Culture Barcode"
					  },
					  {
					    "key": "@CulCul@",
					    "value": "Cultuurtype moet worden ingevoerd"
					  },
					  {
					    "key": "@CulEdi@",
					    "value": "Cultuuritems bewerken"
					  },
					  {
					    "key": "@CulIdeCer@",
					    "value": "Identification zekerheid"
					  },
					  {
					    "key": "@CulIdeCerB@",
					    "value": "Voer identificatie -zekerheidsmetriek in"
					  },
					  {
					    "key": "@CulIso@",
					    "value": "isoleren informatie"
					  },
					  {
					    "key": "@CulIsoA@",
					    "value": "isoleren details"
					  },
					  {
					    "key": "@CulSel@",
					    "value": "Selecteer Cultuurtype"
					  },
					  {
					    "key": "@CulSelA@",
					    "value": "Selecteer Directe test (s)"
					  },
					  {
					    "key": "@CulTyp@",
					    "value": "Cultuurtype"
					  },
					  {
					    "key": "@CulTypA@",
					    "value": "Cultuurtype"
					  },
					  {
					    "key": "@CusAdd@",
					    "value": "Aanvullende klinische informatie"
					  },
					  {
					    "key": "@CusAnt@",
					    "value": "Antibiotica in de afgelopen 24 uur"
					  },
					  {
					    "key": "@CusCli@",
					    "value": "Klinische informatie"
					  },
					  {
					    "key": "@CusEnt@",
					    "value": "Voer klinische informatie in"
					  },
					  {
					    "key": "@CusEntA@",
					    "value": "Voer aanvullende klinische informatie in"
					  },
					  {
					    "key": "@CusEntB@",
					    "value": "Voer verdere informatie in"
					  },
					  {
					    "key": "@CusEntC@",
					    "value": "Voer Specimen Details in"
					  },
					  {
					    "key": "@CusFur@",
					    "value": "Meer informatie"
					  },
					  {
					    "key": "@CusMel@",
					    "value": "Melioidosis Culture"
					  },
					  {
					    "key": "@CusReq@",
					    "value": "Request Isolate Tests"
					  },
					  {
					    "key": "@CusSpe@",
					    "value": "Specimen Information"
					  },
					  {
					    "key": "@CusSus@",
					    "value": "Verdachte klinische diagnose"
					  },
					  {
					    "key": "@CusTem@",
					    "value": "Temperatuur in de afgelopen 24 uur"
					  },
					  {
					    "key": "@EdiSet@",
					    "value": "Setting bewerken"
					  },
					  {
					    "key": "@EdiSetA@",
					    "value": "Setting bewerken"
					  },
					  {
					    "key": "@EdiThe@",
					    "value": "bewerk de waarde van een instelling"
					  },
					  {
					    "key": "@ExpAccIsoNum@",
					    "value": "Accessie en isolaatnummer"
					  },
					  {
					    "key": "@ExpAdd@",
					    "value": "Exportprofiel toevoegen"
					  },
					  {
					    "key": "@ExpAddA@",
					    "value": "Veld toevoegen aan profiel"
					  },
					  {
					    "key": "@ExpASTAnti@",
					    "value": "Antibioticum (AST)"
					  },
					  {
					    "key": "@ExpASTCat@",
					    "value": "Antibioticarecategorie (AST)"
					  },
					  {
					    "key": "@ExpASTDos@",
					    "value": "Dosering (AST)"
					  },
					  {
					    "key": "@ExpASTExc@",
					    "value": "Exclusieve AST -export"
					  },
					  {
					    "key": "@ExpASTGuid@",
					    "value": "Richtlijnen (AST)"
					  },
					  {
					    "key": "@ExpASTMeas@",
					    "value": "Measurement (AST)"
					  },
					  {
					    "key": "@ExpASTMeasExp@",
					    "value": "omvatten MIC -expressies"
					  },
					  {
					    "key": "@ExpASTMeth@",
					    "value": "Testmethode (AST)"
					  },
					  {
					    "key": "@ExpASTSpeCon@",
					    "value": "Special Accessation (AST)"
					  },
					  {
					    "key": "@ExpASTSpeConA@",
					    "value": "Special Accessation"
					  },
					  {
					    "key": "@ExpASTSusc@",
					    "value": "Susceptibility (AST)"
					  },
					  {
					    "key": "@ExpCon@",
					    "value": "Configureer en geef gegevensuitvoering uit"
					  },
					  {
					    "key": "@ExpConA@",
					    "value": "Configureer en geef een whonet export"
					  },
					  {
					    "key": "@ExpCulNum@",
					    "value": "cultuurnummer (cultuur)"
					  },
					  {
					    "key": "@ExpDel@",
					    "value": "Exportprofiel verwijderen"
					  },
					  {
					    "key": "@ExpDelA@",
					    "value": "Veld verwijderen uit profiel"
					  },
					  {
					    "key": "@ExpDhi@",
					    "value": "DHIS2 Export"
					  },
					  {
					    "key": "@ExpEdi@",
					    "value": "Exportprofiel bewerken"
					  },
					  {
					    "key": "@ExpEdiA@",
					    "value": "Veld bewerken in profiel"
					  },
					  {
					    "key": "@ExpFldAnti@",
					    "value": "Antibioticum gevoeligheid"
					  },
					  {
					    "key": "@ExpFldAntiA@",
					    "value": "Selecteer een antibioticum"
					  },
					  {
					    "key": "@ExpFldAntiB@",
					    "value": "antibioticameting"
					  },
					  {
					    "key": "@ExpGen@",
					    "value": "Een DHIS2 -exportbestand genereren en opslaan"
					  },
					  {
					    "key": "@ExpGenA@",
					    "value": "een whonet exportbestand genereren en opslaan"
					  },
					  {
					    "key": "@ExpGroOrg@",
					    "value": "Organisme Naam of groei"
					  },
					  {
					    "key": "@ExpLoc@",
					    "value": "Locatiehiërarchie"
					  },
					  {
					    "key": "@ExpLocA@",
					    "value": "Volledige locatienaam"
					  },
					  {
					    "key": "@ExpLocCode@",
					    "value": "Locatiecodehiërarchie"
					  },
					  {
					    "key": "@ExpMan@",
					    "value": "Exports beheren"
					  },
					  {
					    "key": "@ExpOrg@",
					    "value": "Organisation Hiërarchy"
					  },
					  {
					    "key": "@ExpOrgA@",
					    "value": "Full Organisation Name"
					  },
					  {
					    "key": "@ExpOrgCode@",
					    "value": "Organisation Code Hiërarchie"
					  },
					  {
					    "key": "@ExpPro@",
					    "value": "Exporteren beheren"
					  },
					  {
					    "key": "@ExpProA@",
					    "value": "Exportprofiel toevoegen"
					  },
					  {
					    "key": "@ExpProAlrExi@",
					    "value": "Er bestaat al een exportprofiel van deze naam"
					  },
					  {
					    "key": "@ExpProB@",
					    "value": "Creëren, beheren en uitvoeren Exportprofielen"
					  },
					  {
					    "key": "@ExpProC@",
					    "value": "Exportprofiel toevoegen"
					  },
					  {
					    "key": "@ExpProD@",
					    "value": "Beschrijving"
					  },
					  {
					    "key": "@ExpProDelA@",
					    "value": "Exportprofiel verwijderen"
					  },
					  {
					    "key": "@ExpProDelB@",
					    "value": "Een exportprofiel verwijderen"
					  },
					  {
					    "key": "@ExpProDErr@",
					    "value": "Beschrijving moet worden verstrekt"
					  },
					  {
					    "key": "@ExpProE@",
					    "value": "Exportprofiel bewerken"
					  },
					  {
					    "key": "@ExpProF@",
					    "value": "Profieldetails"
					  },
					  {
					    "key": "@ExpProFielAntiErr@",
					    "value": "Antibioticum vereist"
					  },
					  {
					    "key": "@ExpProFielBlank@",
					    "value": "Blanke kolom"
					  },
					  {
					    "key": "@ExpProFielComForErr@",
					    "value": "Reactieformaat vereist"
					  },
					  {
					    "key": "@ExpProFielComTypeErr@",
					    "value": "Reactietype vereist"
					  },
					  {
					    "key": "@ExpProField@",
					    "value": "Velden in profiel"
					  },
					  {
					    "key": "@ExpProFieldA@",
					    "value": "Voer teken (s) in om bestaande velden te zoeken"
					  },
					  {
					    "key": "@ExpProFieldB@",
					    "value": "Voer de vereiste kolomnaam in in uitvoerbestand"
					  },
					  {
					    "key": "@ExpProFielDef@",
					    "value": "statische kolomwaarde"
					  },
					  {
					    "key": "@ExpProFielHead@",
					    "value": "kopnaam"
					  },
					  {
					    "key": "@ExpProFielHeadErr@",
					    "value": "Kopnaam vereist"
					  },
					  {
					    "key": "@ExpProFielNameErr@",
					    "value": "Selecteer een veldnaam"
					  },
					  {
					    "key": "@ExpProFielTestMethErr@",
					    "value": "Testmethode Vereist"
					  },
					  {
					    "key": "@ExpProFielTestMethPreErr@",
					    "value": "Testmethode Voorrang vereist"
					  },
					  {
					    "key": "@ExpProG@",
					    "value": "Veld exportprofiel toevoegen"
					  },
					  {
					    "key": "@ExpProH@",
					    "value": "Selecteer het genoemde exportprofielveld dat u wilt opnemen en geef vervolgens de kopnaam op zoals u wilt dat deze in de CSV verschijnt"
					  },
					  {
					    "key": "@ExpProI@",
					    "value": "Veld Exporteren Profiel verwijderen"
					  },
					  {
					    "key": "@ExpProJ@",
					    "value": "Een exportprofielveld verwijderen"
					  },
					  {
					    "key": "@ExpProMap@",
					    "value": "Mapping beheren"
					  },
					  {
					    "key": "@ExpProMapDesc@",
					    "value": "Bouw een JSON- of XML-mapping voor de velden in dit exportprofiel."
					  },
					  {
					    "key": "@ExpProMapSav@",
					    "value": "De JSON- of XML-mapping voor een exportprofiel opslaan"
					  },
					  {
					    "key": "@ExpProMapFmt@",
					    "value": "Uitvoerformaat"
					  },
					  {
					    "key": "@ExpProMapJson@",
					    "value": "JSON"
					  },
					  {
					    "key": "@ExpProMapXml@",
					    "value": "XML"
					  },
					  {
					    "key": "@ExpProMapAddAttr@",
					    "value": "Attribuut toevoegen"
					  },
					  {
					    "key": "@ExpProMapAddArr@",
					    "value": "Array toevoegen"
					  },
					  {
					    "key": "@ExpProMapAttrName@",
					    "value": "Attribuutnaam"
					  },
					  {
					    "key": "@ExpProMapField@",
					    "value": "Veld"
					  },
					  {
					    "key": "@ExpProMapArrName@",
					    "value": "Arraynaam"
					  },
					  {
					    "key": "@ExpProMapArrType@",
					    "value": "Arraytype"
					  },
					  {
					    "key": "@ExpProMapArrSpecimen@",
					    "value": "Monsters"
					  },
					  {
					    "key": "@ExpProMapArrCulture@",
					    "value": "Kweken / isolaten"
					  },
					  {
					    "key": "@ExpProMapArrGrid@",
					    "value": "Grid (testresultaten)"
					  },
					  {
					    "key": "@ExpProMapArrAst@",
					    "value": "AST-resultaten"
					  },
					  {
					    "key": "@ExpProMapPreview@",
					    "value": "Voorbeeld"
					  },
					  {
					    "key": "@ExpProMapNoFields@",
					    "value": "Er zijn nog geen velden in het exportprofiel. Voeg eerst velden toe voordat u een mapping bouwt."
					  },
					  {
					    "key": "@ExpProMapNoSpecimen@",
					    "value": "Voor monster-arrays is ten minste één patiëntveld in het exportprofiel vereist."
					  },
					  {
					    "key": "@ExpProMapNoCulture@",
					    "value": "Voor kweek-arrays is ten minste één kweek- of isolaatveld in het exportprofiel vereist."
					  },
					  {
					    "key": "@ExpProMapGridReq@",
					    "value": "Grid-arrays kunnen alleen worden toegevoegd wanneer een gridveld is geselecteerd."
					  },
					  {
					    "key": "@ExpProMapNoAst@",
					    "value": "Voor AST-arrays is ten minste één AST-veld in het exportprofiel vereist."
					  },
					  {
					    "key": "@ExpProMapAttrNameErr@",
					    "value": "Attribuutnaam is verplicht."
					  },
					  {
					    "key": "@ExpProMapFieldErr@",
					    "value": "Een veldselectie is verplicht."
					  },
					  {
					    "key": "@ExpProMapSavedTitle@",
					    "value": "Mapping opgeslagen"
					  },
					  {
					    "key": "@ExpProMapSavedDesc@",
					    "value": "Uw wijzigingen aan de exportprofielmapping zijn opgeslagen."
					  },
					  {
					    "key": "@ExpProMapDelNode@",
					    "value": "Verwijderen"
					  },
					  {
					    "key": "@ExpProMapEditNode@",
					    "value": "Bewerken"
					  },
					  {
					    "key": "@ExpProMapRoot@",
					    "value": "root"
					  },
					  {
					    "key": "@ExpProMd@",
					    "value": "Modified Date"
					  },
					  {
					    "key": "@ExpProN@",
					    "value": "Naam"
					  },
					  {
					    "key": "@ExpProNErr@",
					    "value": "Naam moet worden verstrekt"
					  },
					  {
					    "key": "@ExpProOrderFie@",
					    "value": "Veldorde"
					  },
					  {
					    "key": "@ExpProOrderFieD@",
					    "value": "Bestelvelden zoals deze moeten verschijnen in exportkolommen"
					  },
					  {
					    "key": "@ExpProRec@",
					    "value": "Profielrecord exporteren"
					  },
					  {
					    "key": "@ExpProRecField@",
					    "value": "Field ID"
					  },
					  {
					    "key": "@ExpProRecFieldDel@",
					    "value": "Een veld verwijderen"
					  },
					  {
					    "key": "@ExpProRecFieldsTitle@",
					    "value": "Profielvelden exporteren"
					  },
					  {
					    "key": "@ExpProRecLbl@",
					    "value": "formuliernaam"
					  },
					  {
					    "key": "@ExpProRecTbl@",
					    "value": "tabelnaam"
					  },
					  {
					    "key": "@ExpProT@",
					    "value": "Voer een naam en beschrijving in voor het exportprofiel"
					  },
					  {
					    "key": "@ExpRet@",
					    "value": "Exporteren ophalen"
					  },
					  {
					    "key": "@ExpSta@",
					    "value": "Startdatum moet eerder zijn dan de einddatum"
					  },
					  {
					    "key": "@ExpTyp@",
					    "value": "Type export"
					  },
					  {
					    "key": "@ExpWho@",
					    "value": "Whonet Export"
					  },
					  {
					    "key": "@ExpWhoA@",
					    "value": "Whonet Antibiotical Columns"
					  },
					  {
					    "key": "@ExpYou@",
					    "value": "U moet een startdatum invoeren"
					  },
					  {
					    "key": "@GenA@",
					    "value": "Er moet een waarde worden ingevoerd"
					  },
					  {
					    "key": "@GenAct@",
					    "value": "Action"
					  },
					  {
					    "key": "@GenActA@",
					    "value": "Actions"
					  },
					  {
					    "key": "@GenAdd@",
					    "value": "Extra Notes"
					  },
					  {
					    "key": "@GenAddA@",
					    "value": "Toegevoegd"
					  },
					  {
					    "key": "@GenAddB@",
					    "value": "Een bericht toevoegen voor prominent display"
					  },
					  {
					    "key": "@GenAddC@",
					    "value": "Commentaar toevoegen"
					  },
					  {
					    "key": "@GenAddD@",
					    "value": "Lijst toevoegen"
					  },
					  {
					    "key": "@GenAddE@",
					    "value": "Entry toevoegen"
					  },
					  {
					    "key": "@GenAddF@",
					    "value": "Aangepaste invoer toevoegen"
					  },
					  {
					    "key": "@GenAddG@",
					    "value": "Toevoegen"
					  },
					  {
					    "key": "@GenAddH@",
					    "value": "Aanvullende informatie"
					  },
					  {
					    "key": "@GenAddI@",
					    "value": "Lijn toevoegen"
					  },
					  {
					    "key": "@GenAddJ@",
					    "value": "Toegevoegd door"
					  },
					  {
					    "key": "@GenAdm@",
					    "value": "Administration"
					  },
					  {
					    "key": "@GenAle@",
					    "value": "Alerts"
					  },
					  {
					    "key": "@GenAleA@",
					    "value": "Alerts"
					  },
					  {
					    "key": "@GenAleB@",
					    "value": "Information"
					  },
					  {
					    "key": "@GenAlp@",
					    "value": "Alpha"
					  },
					  {
					    "key": "@GenAlpA@",
					    "value": "Alpha"
					  },
					  {
					    "key": "@GenAlpB@",
					    "value": "Alpha Slider Type"
					  },
					  {
					    "key": "@GenAnd@",
					    "value": "en/of"
					  },
					  {
					    "key": "@GenAnt@",
					    "value": "Antibiotica"
					  },
					  {
					    "key": "@GenAntA@",
					    "value": "Antibiotics"
					  },
					  {
					    "key": "@GenAntB@",
					    "value": "Antibioticum dosering"
					  },
					  {
					    "key": "@GenApp@",
					    "value": "goedkeuren"
					  },
					  {
					    "key": "@GenAppA@",
					    "value": "goedgekeurd"
					  },
					  {
					    "key": "@GenAppB@",
					    "value": "beslissing door"
					  },
					  {
					    "key": "@GenAppC@",
					    "value": "goedkeuring"
					  },
					  {
					    "key": "@GenArc@",
					    "value": "Archive"
					  },
					  {
					    "key": "@GenAre@",
					    "value": "Area"
					  },
					  {
					    "key": "@GenBac@",
					    "value": "Back"
					  },
					  {
					    "key": "@GenBacA@",
					    "value": "Bacteria"
					  },
					  {
					    "key": "@GenBar@",
					    "value": "Patient Labels"
					  },
					  {
					    "key": "@GenBarA@",
					    "value": "Specimen Labels"
					  },
					  {
					    "key": "@GenBeg@",
					    "value": "Begin met cultureren"
					  },
					  {
					    "key": "@GenBegA@",
					    "value": "Begin met het testen"
					  },
					  {
					    "key": "@GenBlo@",
					    "value": "Blood"
					  },
					  {
					    "key": "@GenBlu@",
					    "value": "Blue"
					  },
					  {
					    "key": "@GenBre@",
					    "value": "Breakpoints"
					  },
					  {
					    "key": "@GenBreA@",
					    "value": "Breakpoint"
					  },
					  {
					    "key": "@GenCan@",
					    "value": "Annuleren"
					  },
					  {
					    "key": "@GenCas@",
					    "value": "Cast"
					  },
					  {
					    "key": "@GenCat@",
					    "value": "Categorie"
					  },
					  {
					    "key": "@GenCau@",
					    "value": "Let op!"
					  },
					  {
					    "key": "@GenCle@",
					    "value": "Wissen"
					  },
					  {
					    "key": "@GenClo@",
					    "value": "Sluiten"
					  },
					  {
					    "key": "@GenCloA@",
					    "value": "Klonen"
					  },
					  {
					    "key": "@GenCo1@",
					    "value": "Menu samenvouwen"
					  },
					  {
					    "key": "@GenCod@",
					    "value": "Codering"
					  },
					  {
					    "key": "@GenCodA@",
					    "value": "Code"
					  },
					  {
					    "key": "@GenCodB@",
					    "value": "Organism Group"
					  },
					  {
					    "key": "@GenColA@",
					    "value": "Color"
					  },
					  {
					    "key": "@GenCom@",
					    "value": "Comment 1"
					  },
					  {
					    "key": "@GenComA@",
					    "value": "Comment 2"
					  },
					  {
					    "key": "@GenComB@",
					    "value": "Comment"
					  },
					  {
					    "key": "@GenComC@",
					    "value": "voltooid"
					  },
					  {
					    "key": "@GenComD@",
					    "value": "voltooid op"
					  },
					  {
					    "key": "@GenComE@",
					    "value": "Reacties"
					  },
					  {
					    "key": "@GenComF@",
					    "value": "Commentertype"
					  },
					  {
					    "key": "@GenComG@",
					    "value": "bewerken commentaar"
					  },
					  {
					    "key": "@GenComH@",
					    "value": "bestelcommentaar"
					  },
					  {
					    "key": "@GenComI@",
					    "value": "bestelcommentaar"
					  },
					  {
					    "key": "@GenComJ@",
					    "value": "Specimen Reacties"
					  },
					  {
					    "key": "@GenComK@",
					    "value": "Culture Reacties"
					  },
					  {
					    "key": "@GenComL@",
					    "value": "Selecteer commentertype"
					  },
					  {
					    "key": "@GenComM@",
					    "value": "Standard Comment"
					  },
					  {
					    "key": "@GenComN@",
					    "value": "Comment verwijderen"
					  },
					  {
					    "key": "@GenComO@",
					    "value": "Een reactie verwijderen"
					  },
					  {
					    "key": "@GenComP@",
					    "value": "Comment bewerken"
					  },
					  {
					    "key": "@GenComQ@",
					    "value": "Een opmerking bewerken"
					  },
					  {
					    "key": "@GenComR@",
					    "value": "Comment Format"
					  },
					  {
					    "key": "@GenComS@",
					    "value": "Selecteer commentaarformaat"
					  },
					  {
					    "key": "@GenComT@",
					    "value": "Gratis tekstcommentaar"
					  },
					  {
					    "key": "@GenComU@",
					    "value": "Cultuurcommentaar toevoegen"
					  },
					  {
					    "key": "@GenCon@",
					    "value": "Configuratie"
					  },
					  {
					    "key": "@GenConA@",
					    "value": "Contactnummer"
					  },
					  {
					    "key": "@GenCry@",
					    "value": "Crystal"
					  },
					  {
					    "key": "@GenCul@",
					    "value": "Cultuurtype Categorie"
					  },
					  {
					    "key": "@GenCulA@",
					    "value": "Cultuurtype categorieën"
					  },
					  {
					    "key": "@GenCus@",
					    "value": "Custom"
					  },
					  {
					    "key": "@GenCusA@",
					    "value": "Custom Itties"
					  },
					  {
					    "key": "@GenDat@",
					    "value": "Datum toegevoegd"
					  },
					  {
					    "key": "@GenDatA@",
					    "value": "Datum voltooid"
					  },
					  {
					    "key": "@GenDatB@",
					    "value": "Datum gecreëerd"
					  },
					  {
					    "key": "@GenDatC@",
					    "value": "voltooid"
					  },
					  {
					    "key": "@GenDay@",
					    "value": "Day"
					  },
					  {
					    "key": "@GenDec@",
					    "value": "Decision"
					  },
					  {
					    "key": "@GenDef@",
					    "value": "Standaard"
					  },
					  {
					    "key": "@GenDefA@",
					    "value": "Default Workflow"
					  },
					  {
					    "key": "@GenDel@",
					    "value": "Test verwijderen"
					  },
					  {
					    "key": "@GenDelA@",
					    "value": "Lijst verwijderen"
					  },
					  {
					    "key": "@GenDelB@",
					    "value": "Entry verwijderen"
					  },
					  {
					    "key": "@GenDelC@",
					    "value": "Verwijderen"
					  },
					  {
					    "key": "@GenDep@",
					    "value": "Klinische vestiging of afdeling"
					  },
					  {
					    "key": "@GenDes@",
					    "value": "Beschrijving"
					  },
					  {
					    "key": "@GenDia@",
					    "value": "Diagnose"
					  },
					  {
					    "key": "@GenDiaA@",
					    "value": "Diary"
					  },
					  {
					    "key": "@GenDis@",
					    "value": "Display in Report"
					  },
					  {
					    "key": "@GenDisA@",
					    "value": "Display Culture in Report"
					  },
					  {
					    "key": "@GenDisB@",
					    "value": "Disable"
					  },
					  {
					    "key": "@GenDisC@",
					    "value": "Disk"
					  },
					  {
					    "key": "@GenDos@",
					    "value": "Dosering"
					  },
					  {
					    "key": "@GenEdi@",
					    "value": "EDIT TEST"
					  },
					  {
					    "key": "@GenEdiA@",
					    "value": "Invoer bewerken"
					  },
					  {
					    "key": "@GenEdiB@",
					    "value": "Bewerken"
					  },
					  {
					    "key": "@GenEna@",
					    "value": "Ingeschakeld"
					  },
					  {
					    "key": "@GenEnaA@",
					    "value": "Inschakelen"
					  },
					  {
					    "key": "@GenEnd@",
					    "value": "Einddatum"
					  },
					  {
					    "key": "@GenEndA@",
					    "value": "End"
					  },
					  {
					    "key": "@GenEnt@",
					    "value": "Voer de volledige schermmodus in"
					  },
					  {
					    "key": "@GenEntA@",
					    "value": "Enter Comment"
					  },
					  {
					    "key": "@GenEntB@",
					    "value": "Organisme Naam"
					  },
					  {
					    "key": "@GenEntC@",
					    "value": "Voer teken (s) in om te zoeken naar een waarde"
					  },
					  {
					    "key": "@GenErr@",
					    "value": "Foutbericht"
					  },
					  {
					    "key": "@GenEve@",
					    "value": "Event"
					  },
					  {
					    "key": "@GenEveA@",
					    "value": "Events"
					  },
					  {
					    "key": "@GenExi@",
					    "value": "Exit"
					  },
					  {
					    "key": "@GenExiA@",
					    "value": "Exit Fullscreen Mode"
					  },
					  {
					    "key": "@GenExp@",
					    "value": "Exporteren"
					  },
					  {
					    "key": "@GenExpA@",
					    "value": "Menu uitbreiden"
					  },
					  {
					    "key": "@GenExpB@",
					    "value": "Exportprofielen"
					  },
					  {
					    "key": "@GenExpVal@",
					    "value": "Verwachte waarde"
					  },
					  {
					    "key": "@GenFam@",
					    "value": "Family"
					  },
					  {
					    "key": "@GenFie@",
					    "value": "Velden"
					  },
					  {
					    "key": "@GenFieA@",
					    "value": "veld"
					  },
					  {
					    "key": "@GenFieB@",
					    "value": "veldnaam"
					  },
					  {
					    "key": "@GenFil@",
					    "value": "Filter"
					  },
					  {
					    "key": "@GenFilA@",
					    "value": "Filter Presets"
					  },
					  {
					    "key": "@GenFilB@",
					    "value": "Filter door trefwoord"
					  },
					  {
					    "key": "@GenFilC@",
					    "value": "bestandsnaam"
					  },
					  {
					    "key": "@GenFin@",
					    "value": "Finish"
					  },
					  {
					    "key": "@GenFir@",
					    "value": "Eerste goedkeuring"
					  },
					  {
					    "key": "@GenFirA@",
					    "value": "Tweede goedkeuring"
					  },
					  {
					    "key": "@GenFor@",
					    "value": "Vormen"
					  },
					  {
					    "key": "@GenForA@",
					    "value": "vorm"
					  },
					  {
					    "key": "@GenForB@",
					    "value": "formaat"
					  },
					  {
					    "key": "@GenFou@",
					    "value": "gevonden"
					  },
					  {
					    "key": "@GenFro@",
					    "value": "van"
					  },
					  {
					    "key": "@GenFul@",
					    "value": "Fullscreen"
					  },
					  {
					    "key": "@GenFun@",
					    "value": "Fungus"
					  },
					  {
					    "key": "@GenGen@",
					    "value": "Geslacht"
					  },
					  {
					    "key": "@GenGenA@",
					    "value": "General"
					  },
					  {
					    "key": "@GenGra@",
					    "value": "Analytics"
					  },
					  {
					    "key": "@GenGraA@",
					    "value": "Graphs"
					  },
					  {
					    "key": "@GenGre@",
					    "value": "Green"
					  },
					  {
					    "key": "@GenGri@",
					    "value": "Grid View"
					  },
					  {
					    "key": "@GenGro@",
					    "value": "Grouping"
					  },
					  {
					    "key": "@GenGroA@",
					    "value": "Group Name"
					  },
					  {
					    "key": "@GenGroId@",
					    "value": "Group ID"
					  },
					  {
					    "key": "@GenHel@",
					    "value": "Help"
					  },
					  {
					    "key": "@GenHex@",
					    "value": "Hex"
					  },
					  {
					    "key": "@GenHie@",
					    "value": "Hiërarchie"
					  },
					  {
					    "key": "@GenHis@",
					    "value": "Geschiedenis"
					  },
					  {
					    "key": "@GenHom@",
					    "value": "Home"
					  },
					  {
					    "key": "@GenHos@",
					    "value": "Host"
					  },
					  {
					    "key": "@GenHue@",
					    "value": "Hue"
					  },
					  {
					    "key": "@GenId@",
					    "value": "Id moet worden ingevoerd"
					  },
					  {
					    "key": "@GenIdA@",
					    "value": "Id"
					  },
					  {
					    "key": "@GenImp@",
					    "value": "Imports"
					  },
					  {
					    "key": "@GenInb@",
					    "value": "Inbound"
					  },
					  {
					    "key": "@GenInc@",
					    "value": "Inclusief in Report"
					  },
					  {
					    "key": "@GenIns@",
					    "value": "Interface"
					  },
					  {
					    "key": "@GenIss@",
					    "value": "uitgegeven datum"
					  },
					  {
					    "key": "@GenKey@",
					    "value": "Key"
					  },
					  {
					    "key": "@GenLab@",
					    "value": "Laboratories"
					  },
					  {
					    "key": "@GenLabA@",
					    "value": "Laboratory"
					  },
					  {
					    "key": "@GenLabB@",
					    "value": "Label"
					  },
					  {
					    "key": "@GenLabC@",
					    "value": "Laboratorium/organisatie"
					  },
					  {
					    "key": "@GenLan@",
					    "value": "Language"
					  },
					  {
					    "key": "@GenLat@",
					    "value": "Latitude"
					  },
					  {
					    "key": "@GenLev@",
					    "value": "Level"
					  },
					  {
					    "key": "@GenLis@",
					    "value": "Lists"
					  },
					  {
					    "key": "@GenLisA@",
					    "value": "List Name"
					  },
					  {
					    "key": "@GenLisB@",
					    "value": "Lijst"
					  },
					  {
					    "key": "@GenLoc@",
					    "value": "Locatie"
					  },
					  {
					    "key": "@GenLocA@",
					    "value": "Client Organisation"
					  },
					  {
					    "key": "@GenLocB@",
					    "value": "Locations"
					  },
					  {
					    "key": "@GenLog@",
					    "value": "Log Out"
					  },
					  {
					    "key": "@GenLogA@",
					    "value": "Uitlogbevestiging"
					  },
					  {
					    "key": "@GenLogB@",
					    "value": "Vanwege inactiviteit wordt u automatisch in één minuut uitgelogd. Zou u willen blijven ingelogd?"
					  },
					  {
					    "key": "@GenLogC@",
					    "value": "Login"
					  },
					  {
					    "key": "@GenLon@",
					    "value": "Longitude"
					  },
					  {
					    "key": "@GenMap@",
					    "value": "Mappings"
					  },
					  {
					    "key": "@GenMax@",
					    "value": "Maximum"
					  },
					  {
					    "key": "@GenMea@",
					    "value": "Measurement"
					  },
					  {
					    "key": "@GenMer@",
					    "value": "Merge"
					  },
					  {
					    "key": "@GenMes@",
					    "value": "Message"
					  },
					  {
					    "key": "@GenMet@",
					    "value": "Method"
					  },
					  {
					    "key": "@GenMic@",
					    "value": "Mic"
					  },
					  {
					    "key": "@GenMin@",
					    "value": "Minimum"
					  },
					  {
					    "key": "@GenMod@",
					    "value": "Last Modifier"
					  },
					  {
					    "key": "@GenMon@",
					    "value": "Monitoring"
					  },
					  {
					    "key": "@GenMonA@",
					    "value": "Maanden"
					  },
					  {
					    "key": "@GenMonB@",
					    "value": "Maand"
					  },
					  {
					    "key": "@GenMor@",
					    "value": "More"
					  },
					  {
					    "key": "@GenMov@",
					    "value": "Stap Up"
					  },
					  {
					    "key": "@GenMovA@",
					    "value": "Move Down"
					  },
					  {
					    "key": "@GenMulOff@",
					    "value": "Multiselect Off"
					  },
					  {
					    "key": "@GenMulOn@",
					    "value": "Multiselect aan"
					  },
					  {
					    "key": "@GenNam@",
					    "value": "Naam"
					  },
					  {
					    "key": "@GenNex@",
					    "value": "Volgende"
					  },
					  {
					    "key": "@GenNexA@",
					    "value": "Volgende pagina"
					  },
					  {
					    "key": "@GenNo@",
					    "value": "Nee"
					  },
					  {
					    "key": "@GenNoe@",
					    "value": "Geen vermeldingen gevonden"
					  },
					  {
					    "key": "@GenNon@",
					    "value": "geen"
					  },
					  {
					    "key": "@GenNot@",
					    "value": "Ast extra notities"
					  },
					  {
					    "key": "@GenNotA@",
					    "value": "niet goedgekeurd"
					  },
					  {
					    "key": "@GenNum@",
					    "value": "nee."
					  },
					  {
					    "key": "@GenOfA@",
					    "value": "van"
					  },
					  {
					    "key": "@GenOld@",
					    "value": "ouder"
					  },
					  {
					    "key": "@GenOpe@",
					    "value": "open vorm"
					  },
					  {
					    "key": "@GenOr@",
					    "value": "of"
					  },
					  {
					    "key": "@GenOrd@",
					    "value": "order"
					  },
					  {
					    "key": "@GenOrg@",
					    "value": "Klantorganisaties"
					  },
					  {
					    "key": "@GenOrgA@",
					    "value": "Organisme"
					  },
					  {
					    "key": "@GenOrgB@",
					    "value": "Organismen"
					  },
					  {
					    "key": "@GenOrgC@",
					    "value": "Client Organisation"
					  },
					  {
					    "key": "@GenOrgD@",
					    "value": "Organism List"
					  },
					  {
					    "key": "@GenOrgE@",
					    "value": "Organism Group"
					  },
					  {
					    "key": "@GenOrgF@",
					    "value": "Organism Groups"
					  },
					  {
					    "key": "@GenOut@",
					    "value": "Outbound"
					  },
					  {
					    "key": "@GenPag@",
					    "value": "Pages"
					  },
					  {
					    "key": "@GenPagA@",
					    "value": "Page"
					  },
					  {
					    "key": "@GenPar@",
					    "value": "Ouder",
					  },
					  {
					    "key": "@GenParA@",
					    "value": "Invoer ouder"
					  },
					  {
					    "key": "@GenPat@",
					    "value": "Patiënten"
					  },
					  {
					    "key": "@GenPos@",
					    "value": "Positie"
					  },
					  {
					    "key": "@GenPre@",
					    "value": "Vorige"
					  },
					  {
					    "key": "@GenPri@",
					    "value": "Barcode afdrukken (Type 1)"
					  },
					  {
					    "key": "@GenPriA@",
					    "value": "Barcode afdrukken (Type 2)"
					  },
					  {
					    "key": "@GenPriB@",
					    "value": "Printed"
					  },
					  {
					    "key": "@GenPriC@",
					    "value": "Print"
					  },
					  {
					    "key": "@GenPriD@",
					    "value": "Print Report"
					  },
					  {
					    "key": "@GenPriE@",
					    "value": "Print On Report"
					  },
					  {
					    "key": "@GenPriF@",
					    "value": "Afdrukvoorbeeld"
					  },
					  {
					    "key": "@GenPro@",
					    "value": "Profiel"
					  },
					  {
					    "key": "@GenPub@",
					    "value": "Gepubliceerd"
					  },
					  {
					    "key": "@GenPubA@",
					    "value": "Publiceren"
					  },
					  {
					    "key": "@GenQua@",
					    "value": "Hoeveelheid"
					  },
					  {
					    "key": "@GenQuaA@",
					    "value": "Kwaliteitscontrole"
					  },
					  {
					    "key": "@GenRea@",
					    "value": "Gratis tekst beoordeling commentaar"
					  },
					  {
					    "key": "@GenRed@",
					    "value": "Red"
					  },
					  {
					    "key": "@GenRef@",
					    "value": "Refresh"
					  },
					  {
					    "key": "@GenRej@",
					    "value": "Afwijzen"
					  },
					  {
					    "key": "@GenRem@",
					    "value": "Row Removal Bevestiging"
					  },
					  {
					    "key": "@GenRemA@",
					    "value": "Weet u zeker dat u de geselecteerde antibioticarij wilt verwijderen?"
					  },
					  {
					    "key": "@GenRep@",
					    "value": "Partijen"
					  },
					  {
					    "key": "@GenRepA@",
					    "value": "Rapport"
					  },
					  {
					    "key": "@GenRepB@",
					    "value": "Publiceer Specimen Reports"
					  },
					  {
					    "key": "@GenRepC@",
					    "value": "Herhaling"
					  },
					  {
					    "key": "@GenReq@",
					    "value": "Gevraagd"
					  },
					  {
					    "key": "@GenReqA@",
					    "value": "Gevraagd op"
					  },
					  {
					    "key": "@GenReqB@",
					    "value": "Vereist veld"
					  },
					  {
					    "key": "@GenRes@",
					    "value": "Resultaatdatum"
					  },
					  {
					    "key": "@GenResA@",
					    "value": "Resultaat moet worden ingevoerd"
					  },
					  {
					    "key": "@GenResB@",
					    "value": "Restart"
					  },
					  {
					    "key": "@GenResC@",
					    "value": "Sequentie resetten bij verandering"
					  },
					  {
					    "key": "@GenRol@",
					    "value": "Rollen"
					  },
					  {
					    "key": "@GenSav@",
					    "value": "Redden"
					  },
					  {
					    "key": "@GenSea@",
					    "value": "Zoekopdracht"
					  },
					  {
					    "key": "@GenSec@",
					    "value": "Secties"
					  },
					  {
					    "key": "@GenSecA@",
					    "value": "Sectie"
					  },
					  {
					    "key": "@GenSee@",
					    "value": "Gezien"
					  },
					  {
					    "key": "@GenSel@",
					    "value": "Selecteer Startdatum"
					  },
					  {
					    "key": "@GenSelA@",
					    "value": "Selecteer einddatum"
					  },
					  {
					    "key": "@GenSelB@",
					    "value": "Selecteer Locatie"
					  },
					  {
					    "key": "@GenSelC@",
					    "value": "Afdeling selecteren"
					  },
					  {
					    "key": "@GenSelD@",
					    "value": "Selecteer Diagnose"
					  },
					  {
					    "key": "@GenSelE@",
					    "value": "Selecteer een toepasselijke commentaar"
					  },
					  {
					    "key": "@GenSelF@",
					    "value": "Selecteer Organisme"
					  },
					  {
					    "key": "@GenSelG@",
					    "value": "Selecteer ouder"
					  },
					  {
					    "key": "@GenSelH@",
					    "value": "Set Organisme Scope"
					  },
					  {
					    "key": "@GenSelI@",
					    "value": "Bewerken Organisme Scope"
					  },
					  {
					    "key": "@GenSelJ@",
					    "value": "Type teken (s) om te zoeken en selecteren"
					  },
					  {
					    "key": "@GenSelK@",
					    "value": "Selecteer waarde"
					  },
					  {
					    "key": "@GenSelL@",
					    "value": "Geselecteerde records"
					  },
					  {
					    "key": "@GenSelM@",
					    "value": "Selecteer Area"
					  },
					  {
					    "key": "@GenSelN@",
					    "value": "Selecteer Format"
					  },
					  {
					    "key": "@GenSelO@",
					    "value": "Selecteer gebeurtenis"
					  },
					  {
					    "key": "@GenSelP@",
					    "value": "Typ karakter(s) om te zoeken"
					  },
					  {
					    "key": "@GenSelQ@",
					    "value": "Selecteer categorie"
					  },
					  {
					    "key": "@GenSer@",
					    "value": "Serotype"
					  },
					  {
					    "key": "@GenSet@",
					    "value": "Instellingen"
					  },
					  {
					    "key": "@GenSetA@",
					    "value": "Setting"
					  },
					  {
					    "key": "@GenSho@",
					    "value": "Preview box tonen"
					  },
					  {
					    "key": "@GenSin@",
					    "value": "single"
					  },
					  {
					    "key": "@GenSou@",
					    "value": "source"
					  },
					  {
					    "key": "@GenSpe@",
					    "value": "Exemplaren"
					  },
					  {
					    "key": "@GenSpeA@",
					    "value": "Specificeer"
					  },
					  {
					    "key": "@GenSpeB@",
					    "value": "soorten"
					  },
					  {
					    "key": "@GenSpeC@",
					    "value": "Specimen"
					  },
					  {
					    "key": "@GenSta@",
					    "value": "State"
					  },
					  {
					    "key": "@GenStaA@",
					    "value": "Status"
					  },
					  {
					    "key": "@GenStaB@",
					    "value": "Startdatum"
					  },
					  {
					    "key": "@GenStaC@",
					    "value": "Start"
					  },
					  {
					    "key": "@GenSub@",
					    "value": "verzenden"
					  },
					  {
					    "key": "@GenSubA@",
					    "value": "ondersoorten"
					  },
					  {
					    "key": "@GenSus@",
					    "value": "gevoeligheid"
					  },
					  {
					    "key": "@GenSys@",
					    "value": "Systeemconfiguratie"
					  },
					  {
					    "key": "@GenTab@",
					    "value": "Tabellen"
					  },
					  {
					    "key": "@GenTabA@",
					    "value": "Tabel"
					  },
					  {
					    "key": "@GenTag@",
					    "value": "Tags om in te stellen"
					  },
					  {
					    "key": "@GenTagA@",
					    "value": "Tag"
					  },
					  {
					    "key": "@GenTagB@",
					    "value": "Add Tag"
					  },
					  {
					    "key": "@GenTagC@",
					    "value": "Voer tagnaam in"
					  },
					  {
					    "key": "@GenTagD@",
					    "value": "Voer nieuwe tagnaam in"
					  },
					  {
					    "key": "@GenTagE@",
					    "value": "Selecteer tag"
					  },
					  {
					    "key": "@GenTagF@",
					    "value": "Tag toevoegen aan monster"
					  },
					  {
					    "key": "@GenTagG@",
					    "value": "Tag toevoegen aan patiënt"
					  },
					  {
					    "key": "@GenTagH@",
					    "value": "Oudertag"
					  },
					  {
					    "key": "@GenTagI@",
					    "value": "Selecteer oudertag"
					  },
					  {
					    "key": "@GenTagJ@",
					    "value": "Selecteer een bestaande tag of voer een nieuwe tagnaam in."
					  },
					  {
					    "key": "@GenTem@",
					    "value": "temperatuur"
					  },
					  {
					    "key": "@GenTes@",
					    "value": "Tests"
					  },
					  {
					    "key": "@GenTesA@",
					    "value": "Testtype"
					  },
					  {
					    "key": "@GenTesB@",
					    "value": "Testpatronen"
					  },
					  {
					    "key": "@GenTesC@",
					    "value": "Testnaam"
					  },
					  {
					    "key": "@GenTesD@",
					    "value": "Test"
					  },
					  {
					    "key": "@GenTesE@",
					    "value": "Testsamenvatting"
					  },
					  {
					    "key": "@GenTesF@",
					    "value": "Teststatus"
					  },
					  {
					    "key": "@GenTesG@",
					    "value": "Testcategorie"
					  },
					  {
					    "key": "@GenTesH@",
					    "value": "Testcategorieën"
					  },
					  {
					    "key": "@GenTex@",
					    "value": "Text"
					  },
					  {
					    "key": "@GenTit@",
					    "value": "Titel"
					  },
					  {
					    "key": "@GenTo@",
					    "value": "to"
					  },
					  {
					    "key": "@GenTod@",
					    "value": "Today"
					  },
					  {
					    "key": "@GenTog@",
					    "value": "Toggle Report Preview"
					  },
					  {
					    "key": "@GenTop@",
					    "value": "Topic"
					  },
					  {
					    "key": "@GenTopA@",
					    "value": "Selecteer onderwerp"
					  },
					  {
					    "key": "@GenTra@",
					    "value": "Tabelweergave"
					  },
					  {
					    "key": "@GenTraA@",
					    "value": "Transparantie"
					  },
					  {
					    "key": "@GenTyp@",
					    "value": "Type"
					  },
					  {
					    "key": "@GenUse@",
					    "value": "gebruikers"
					  },
					  {
					    "key": "@GenUseA@",
					    "value": "Gebruiker"
					  },
					  {
					    "key": "@GenUseB@",
					    "value": "gebruikersnaam"
					  },
					  {
					    "key": "@GenVal@",
					    "value": "waarde"
					  },
					  {
					    "key": "@GenVie@",
					    "value": "views"
					  },
					  {
					    "key": "@GenVieA@",
					    "value": "View/Update"
					  },
					  {
					    "key": "@GenVieB@",
					    "value": "Bekijk test"
					  },
					  {
					    "key": "@GenVieC@",
					    "value": "Bekijk"
					  },
					  {
					    "key": "@GenVieD@",
					    "value": "Rapporten"
					  },
					  {
					    "key": "@GenVieE@",
					    "value": "Bekijk naam"
					  },
					  {
					    "key": "@GenVieF@",
					    "value": "Bekijk rapport"
					  },
					  {
					    "key": "@GenVieG@",
					    "value": "Rapporten bekijken"
					  },
					  {
					    "key": "@GenWar@",
					    "value": "Afdeling"
					  },
					  {
					    "key": "@GenWid@",
					    "value": "Width"
					  },
					  {
					    "key": "@GenWor@",
					    "value": "Workflows"
					  },
					  {
					    "key": "@GenWorA@",
					    "value": "Workflow"
					  },
					  {
					    "key": "@GenYea@",
					    "value": "Gisten"
					  },
					  {
					    "key": "@GenYeaA@",
					    "value": "jaren"
					  },
					  {
					    "key": "@GenYeaB@",
					    "value": "jaar"
					  },
					  {
					    "key": "@GenYes@",
					    "value": "gisteren"
					  },
					  {
					    "key": "@GenYesA@",
					    "value": "ja"
					  },
					  {
					    "key": "@GraDat@",
					    "value": "Datuminterval"
					  },
					  {
					    "key": "@GraExp@",
					    "value": "Gegevens exporteren"
					  },
					  {
					    "key": "@GraExpA@",
					    "value": "Graph exporteren"
					  },
					  {
					    "key": "@GraGen@",
					    "value": "Gender Samenvatting"
					  },
					  {
					    "key": "@GraGra@",
					    "value": "Graph Type"
					  },
					  {
					    "key": "@GraInf@",
					    "value": "Visualiseer en exporteer relaties en trends in gegevens"
					  },
					  {
					    "key": "@GraLoc@",
					    "value": "Locatieniveau"
					  },
					  {
					    "key": "@GraNo@",
					    "value": "geen gegevens geretourneerd voor deze grafiek"
					  },
					  {
					    "key": "@GraOrg@",
					    "value": "Organisme gevoeligheid"
					  },
					  {
					    "key": "@GraOrgA@",
					    "value": "organisatieniveau"
					  },
					  {
					    "key": "@GraPle@",
					    "value": "Selecteer Graph -type en datuminterval voordat u het rapport uitvoert"
					  },
					  {
					    "key": "@GraPleA@",
					    "value": "Selecteer Graph Type, datuminterval en een of meer locaties voordat het rapport wordt uitgevoerd"
					  },
					  {
					    "key": "@GraSpe@",
					    "value": "Samenvatting van het specimentype"
					  },
					  {
					    "key": "@GraSpeB@",
					    "value": "Specimen Type"
					  },
					  {
					    "key": "@GraSpeC@",
					    "value": "Specimen Site"
					  },
					  {
					    "key": "@GraSpeD@",
					    "value": "Specimen State"
					  },
					  {
					    "key": "@GraSpeE@",
					    "value": "Specimens by tag"
					  },
					  {
					    "key": "@GraSpeF@",
					    "value": "Specimens per organisatie"
					  },
					  {
					    "key": "@GraSpeG@",
					    "value": "Specimens voor locatie"
					  },
					  {
					    "key": "@GraSpeH@",
					    "value": "Specimens door organisme"
					  },
					  {
					    "key": "@ImpAdd@",
					    "value": "Importprofiel toevoegen"
					  },
					  {
					    "key": "@ImpAddA@",
					    "value": "Importprofiel toevoegen"
					  },
					  {
					    "key": "@ImpAddB@",
					    "value": "Veld importprofiel toevoegen"
					  },
					  {
					    "key": "@ImpAddC@",
					    "value": "Veld importprofiel toevoegen"
					  },
					  {
					    "key": "@ImpAni@",
					    "value": "Er bestaat al een importprofiel van deze naam"
					  },
					  {
					    "key": "@ImpCol@",
					    "value": "Kolompositie"
					  },
					  {
					    "key": "@ImpColA@",
					    "value": "Een kolompositie moet worden ingevoerd"
					  },
					  {
					    "key": "@ImpColB@",
					    "value": "Kolompositie"
					  },
					  {
					    "key": "@ImpCre@",
					    "value": "Importprofielen maken en beheren"
					  },
					  {
					    "key": "@ImpDel@",
					    "value": "Importprofiel verwijderen"
					  },
					  {
					    "key": "@ImpDelA@",
					    "value": "Een importprofiel verwijderen"
					  },
					  {
					    "key": "@ImpDelB@",
					    "value": "Een importprofielveld verwijderen"
					  },
					  {
					    "key": "@ImpDelC@",
					    "value": "Importprofielveld verwijderen"
					  },
					  {
					    "key": "@ImpEdi@",
					    "value": "Importprofiel bewerken"
					  },
					  {
					    "key": "@ImpEdiA@",
					    "value": "Importprofiel bewerken"
					  },
					  {
					    "key": "@ImpEnt@",
					    "value": "Voer de profielgegevens van importeren in"
					  },
					  {
					    "key": "@ImpEntA@",
					    "value": "Voer een naam en beschrijving in voor het importprofiel"
					  },
					  {
					    "key": "@ImpFie@",
					    "value": "Een veld moet worden geselecteerd"
					  },
					  {
					    "key": "@ImpFil@",
					    "value": "bestand importeren"
					  },
					  {
					    "key": "@ImpImp@",
					    "value": "Profiel van het profiel importeren"
					  },
					  {
					    "key": "@ImpImpA@",
					    "value": "Profielvelden importeren"
					  },
					  {
					    "key": "@ImpImpB@",
					    "value": "Patiënt-, specimen- of cultuurinformatie importeren"
					  },
					  {
					    "key": "@ImpImpC@",
					    "value": "Bestand importeren"
					  },
					  {
					    "key": "@ImpImpD@",
					    "value": "Bestand importeren"
					  },
					  {
					    "key": "@ImpInc@",
					    "value": "Kopregel opnemen"
					  },
					  {
					    "key": "@ImpMan@",
					    "value": "Importprofielen beheren"
					  },
					  {
					    "key": "@ImpPro@",
					    "value": "Importeren beheren"
					  },
					  {
					    "key": "@ImpProA@",
					    "value": "Profiel importeren"
					  },
					  {
					    "key": "@ImpSel@",
					    "value": "Selecteer de kolompositie van de gegevens in de CSV en het veld om de gegevens te laden in"
					  },
					  {
					    "key": "@ImpSelA@",
					    "value": "Selecteer Bestand en laad inhoud"
					  },
					  {
					    "key": "@ImpTab@",
					    "value": "Tabel om te laden"
					  },
					  {
					    "key": "@ImpYou@",
					    "value": "u moet een tabel selecteren om de gegevens te laden in"
					  },
					  {
					    "key": "@InsAcc@",
					    "value": "Resultaten accepteren"
					  },
					  {
					    "key": "@InsAccA@",
					    "value": "Accepteren en opslaan van externe interface"
					  },
					  {
					    "key": "@InsAccB@",
					    "value": "Interface-resultaten accepteren"
					  },
					  {
					    "key": "@InsAccC@",
					    "value": "Accessienummer bevat geen cultuur met dit cultuurnummer"
					  },
					  {
					    "key": "@InsAdd@",
					    "value": "Interfaceprofiel toevoegen"
					  },
					  {
					    "key": "@InsAddA@",
					    "value": "Interfaceprofiel toevoegen"
					  },
					  {
					    "key": "@InsAddB@",
					    "value": "het profiel toevoegen voor een nieuwe interface"
					  },
					  {
					    "key": "@InsAll@",
					    "value": "ID overschrijven"
					  },
					  {
					    "key": "@InsAllA@",
					    "value": "AST AST -overschrift toestaan"
					  },
					  {
					    "key": "@InsAnt@",
					    "value": "Antibiotical Group"
					  },
					  {
					    "key": "@InsApp@",
					    "value": "goedkeuring vereist"
					  },
					  {
					    "key": "@InsAwa@",
					    "value": "In afwachting van acceptatie"
					  },
					  {
					    "key": "@InsBat@",
					    "value": "Batch Replay"
					  },
					  {
					    "key": "@InsBatA@",
					    "value": "Batch Replay interfacefouten"
					  },
					  {
					    "key": "@InsBatB@",
					    "value": "Batch Replay Interfacefouten"
					  },
					  {
					    "key": "@InsBatC@",
					    "value": "Replay alle geselecteerde interfacefouten"
					  },
					  {
					    "key": "@InsBatD@",
					    "value": "Batch Accept Resultaten"
					  },
					  {
					    "key": "@InsBatE@",
					    "value": "Batch wijst resultaten af"
					  },
					  {
					    "key": "@InsBatF@",
					    "value": "Batch Accept Resultaten"
					  },
					  {
					    "key": "@InsBatG@",
					    "value": "Batch weigeren resultaten"
					  },
					  {
					    "key": "@InsBatH@",
					    "value": "Batch accepteren en opslaan van externe interface"
					  },
					  {
					    "key": "@InsBatI@",
					    "value": "Batch wijst en verwijder de resultaten van externe interface"
					  },
					  {
					    "key": "@InsDef@",
					    "value": "standaardgroei"
					  },
					  {
					    "key": "@InsDel@",
					    "value": "Interfaceprofiel verwijderen"
					  },
					  {
					    "key": "@InsDelA@",
					    "value": "Interfaceprofiel verwijderen"
					  },
					  {
					    "key": "@InsDelB@",
					    "value": "Een bestaand interfaceprofiel verwijderen"
					  },
					  {
					    "key": "@InsLoa@",
					    "value": "Regels voor het laden van gegevens"
					  },
					  {
					    "key": "@InsLoaB@",
					    "value": "Regels om te volgen bij het laden van gegevens"
					  },
					  {
					    "key": "@InsDir@",
					    "value": "Richting"
					  },
					  {
					    "key": "@InsEdi@",
					    "value": "Interfaceprofiel bewerken"
					  },
					  {
					    "key": "@InsEdiB@",
					    "value": "Key -parameters bewerken die bepalen of en hoe het systeem interageert met een externe interface"
					  },
					  {
					    "key": "@InsProNam@",
					    "value": "Profielnaam"
					  },
					  {
					    "key": "@InsEnt@",
					    "value": "Voer interfacenaam in"
					  },
					  {
					    "key": "@InsErr@",
					    "value": "Interfacefouten"
					  },
					  {
					    "key": "@InsErrA@",
					    "value": "Interfacefouten"
					  },
					  {
					    "key": "@InsErrG@",
					    "value": "Een ID moet worden verstrekt"
					  },
					  {
					    "key": "@InsFor@",
					    "value": "Geformatteerde interfacefoutdetails"
					  },
					  {
					    "key": "@InsForA@",
					    "value": "Geformatteerde interfaceresultaten"
					  },
					  {
					    "key": "@InsIda@",
					    "value": "ID & AST-interface"
					  },
					  {
					    "key": "@InsIgn@",
					    "value": "negeren niet -erkende antibiotica"
					  },
					  {
					    "key": "@InsIns@",
					    "value": "Interface-resultaten"
					  },
					  {
					    "key": "@InsInsA@",
					    "value": "Interfaces"
					  },
					  {
					    "key": "@InsInsB@",
					    "value": "Interfaceprofiel"
					  },
					  {
					    "key": "@InsInsC@",
					    "value": "Interfaceberichtdetails"
					  },
					  {
					    "key": "@InsInsD@",
					    "value": "Interfacetype"
					  },
					  {
					    "key": "@InsInsE@",
					    "value": "Interfacenaam moet worden ingevoerd"
					  },
					  {
					    "key": "@InsInsF@",
					    "value": "Interfacetype moet worden geselecteerd"
					  },
					  {
					    "key": "@InsValGro@",
					    "value": "Standaard groei moet worden geselecteerd voor het interfacetype Id en AST"
					  },
					  {
					    "key": "@InsValOrg@",
					    "value": "Organismegroep moet worden geselecteerd voor het interfacetype Id en AST"
					  },
					  {
					    "key": "@InsValAnt@",
					    "value": "Antibioticagroep moet worden geselecteerd voor het interfacetype Id en AST"
					  },
					  {
					    "key": "@InsCusExp@",
					    "value": "Exportprofiel"
					  },
					  {
					    "key": "@InsCriG@",
					    "value": "Interfacecriteria (optioneel)"
					  },
					  {
					    "key": "@InsCriR@",
					    "value": "Interfacecombinatieregel"
					  },
					  {
					    "key": "@InsValExp@",
					    "value": "Exportprofiel moet worden geselecteerd voor het interfacetype Aangepast"
					  },
					  {
					    "key": "@InsCusPat@",
					    "value": "De patiënt bestaat niet en dit profiel maakt geen patiënten aan, dus het monster kon niet worden geladen"
					  },
					  {
					    "key": "@InsCusParse@",
					    "value": "Het inkomende bestand kon niet worden geïnterpreteerd met de gekoppelde exportprofielstructuur"
					  },
					  {
					    "key": "@InsCusNoExp@",
					    "value": "Het aangepaste interfaceprofiel heeft geen exportprofielmapping om het bestand mee te laden"
					  },
					  {
					    "key": "@InsCusVal@",
					    "value": "Een geïmporteerde waarde kon niet worden gekoppeld aan een lijstitem voor dit profiel"
					  },
					  {
					    "key": "@InsCusSave@",
					    "value": "Het aangepaste interfacerecord kon niet worden opgeslagen"
					  },
					  {
					    "key": "@InsLis@",
					    "value": "Lijst met interfacefouten"
					  },
					  {
					    "key": "@InsMan@",
					    "value": "Interfaceprofielen beheren"
					  },
					  {
					    "key": "@InsManA@",
					    "value": "Externe interfaces inschakelen, uitschakelen en configureren"
					  },
					  {
					    "key": "@InsManB@",
					    "value": "Interface-resultaten beheren"
					  },
					  {
					    "key": "@InsManC@",
					    "value": "Beoordeel en accepteer of wijs testresultaten van externe interfaces af"
					  },
					  {
					    "key": "@InsManD@",
					    "value": "Barcode van de fabrikant"
					  },
					  {
					    "key": "@InsNam@",
					    "value": "Interfacenaam"
					  },
					  {
					    "key": "@InsNee@",
					    "value": "heeft goedkeuring nodig"
					  },
					  {
					    "key": "@InsNew@",
					    "value": "Nieuwe interface-resultaten"
					  },
					  {
					    "key": "@InsNo@",
					    "value": "Geen exemplaar of cultuur gevonden waartegen het interface-resultaat kan worden toegepast"
					  },
					  {
					    "key": "@InsNoA@",
					    "value": "geen bijpassend toegangsnummer gevonden"
					  },
					  {
					    "key": "@InsRaw@",
					    "value": "onbewerkte gegevens"
					  },
					  {
					    "key": "@InsRawA@",
					    "value": "Ruwe interface-resultaatdetails"
					  },
					  {
					    "key": "@InsRej@",
					    "value": "Resultaten afwijzen"
					  },
					  {
					    "key": "@InsRejA@",
					    "value": "Resultaten afwijzen en verwijderen van externe interface"
					  },
					  {
					    "key": "@InsRejB@",
					    "value": "Interface-resultaten verwijderen"
					  },
					  {
					    "key": "@InsRep@",
					    "value": "Interfacefout opnieuw afspelen"
					  },
					  {
					    "key": "@InsRepA@",
					    "value": "Interfacefout opnieuw afspelen"
					  },
					  {
					    "key": "@InsRepB@",
					    "value": "De geselecteerde interfacefout opnieuw afspelen"
					  },
					  {
					    "key": "@InsReq@",
					    "value": "Verzoek gemaakt"
					  },
					  {
					    "key": "@InsRes@",
					    "value": "Resultaat ontvangen"
					  },
					  {
					    "key": "@InsSca@",
					    "value": "Scan de streepjescode van de fabrikant in"
					  },
					  {
					    "key": "@InsTes@",
					    "value": "Ondersteunde tests"
					  },
					  {
					    "key": "@InsTesA@",
					    "value": "Ingeschakelde tests"
					  },
					  {
					    "key": "@InsTypA@",
					    "value": "Testtype"
					  },
					  {
					    "key": "@InsUpd@",
					    "value": "Interfaceprofiel bijwerken"
					  },
					  {
					    "key": "@InsUpdA@",
					    "value": "Update -gebeurtenis mislukt"
					  },
					  {
					    "key": "@InsReqT@",
					    "value": "Interface-test aanvragen"
					  },
					  {
					    "key": "@InsReqTB@",
					    "value": "Een test aanvragen van de geselecteerde interface"
					  },
					  {
					    "key": "@InsReqEv@",
					    "value": "Interface-test aanvragen"
					  },
					  {
					    "key": "@InsAddC@",
					    "value": "Profiel toevoegen voor externe interface"
					  },
					  {
					    "key": "@InsEdiC@",
					    "value": "Profiel bewerken voor externe interface"
					  },
					  {
					    "key": "@InsDelC@",
					    "value": "Profiel verwijderen voor externe interface"
					  },
					  {
					    "key": "@InsAccD@",
					    "value": "Resultaten accepteren van externe interface"
					  },
					  {
					    "key": "@InsRejC@",
					    "value": "Resultaten afwijzen van externe interface"
					  },
					  {
					    "key": "@InsBatJ@",
					    "value": "Batch resultaten accepteren van externe interface"
					  },
					  {
					    "key": "@InsBatK@",
					    "value": "Batch resultaten afwijzen van externe interface"
					  },
					  {
					    "key": "@InsRepC@",
					    "value": "Speelt een interface-lading opnieuw af die een fout had"
					  },
					  {
					    "key": "@InsViewA@",
					    "value": "Interface-resultaatrecord bekijken"
					  },
					  {
					    "key": "@InsViewB@",
					    "value": "De details van een interfacefout bekijken"
					  },
					  {
					    "key": "@LabA@",
					    "value": "Een vertaling moet worden geselecteerd"
					  },
					  {
					    "key": "@LabAA@",
					    "value": "Een organisme -groep moet worden geselecteerd"
					  },
					  {
					    "key": "@LabAB@",
					    "value": "een testcategorie moet worden geselecteerd"
					  },
					  {
					    "key": "@LabAC@",
					    "value": "Een cultuurtype categorie moet worden geselecteerd"
					  },
					  {
					    "key": "@LabAD@",
					    "value": "een bestaande cultuurtestinstelling bewerken"
					  },
					  {
					    "key": "@LabAdd@",
					    "value": "Laboratorium toevoegen"
					  },
					  {
					    "key": "@LabAddA@",
					    "value": "Testcategorieregel toevoegen"
					  },
					  {
					    "key": "@LabAddB@",
					    "value": "Testcategorieregel toevoegen"
					  },
					  {
					    "key": "@LabAddC@",
					    "value": "Een nieuwe testcategorie -regel toevoegen"
					  },
					  {
					    "key": "@LabAddD@",
					    "value": "Cultuurtype Categorieregel toevoegen"
					  },
					  {
					    "key": "@LabAddE@",
					    "value": "Cultuurtype Categorieregel toevoegen"
					  },
					  {
					    "key": "@LabAddF@",
					    "value": "Een nieuwe categorie regel toevoegen"
					  },
					  {
					    "key": "@LabAddG@",
					    "value": "Directe testopties toevoegen"
					  },
					  {
					    "key": "@LabAddH@",
					    "value": "Opties van cultuurtype toevoegen"
					  },
					  {
					    "key": "@LabAddI@",
					    "value": "Cultuurtestopties toevoegen"
					  },
					  {
					    "key": "@LabAddJ@",
					    "value": "Workflowregel van het specimentype toevoegen"
					  },
					  {
					    "key": "@LabAddK@",
					    "value": "Workflowregel van het specimentype toevoegen"
					  },
					  {
					    "key": "@LabAddL@",
					    "value": "Toevoegen een nieuw specimen type workflowregel"
					  },
					  {
					    "key": "@LabAE@",
					    "value": "Een standaardworkflow moet worden geselecteerd"
					  },
					  {
					    "key": "@LabAF@",
					    "value": "Een workflow moet worden geselecteerd"
					  },
					  {
					    "key": "@LabBre@",
					    "value": "Breakpoint -lijsten om te gebruiken"
					  },
					  {
					    "key": "@LabBreA@",
					    "value": "Breakpoint Lists"
					  },
					  {
					    "key": "@LabCre@",
					    "value": "nieuwe en bestaande laboratoria maken en beheren"
					  },
					  {
					    "key": "@LabCul@",
					    "value": "Cultuurtype Categorisatie"
					  },
					  {
					    "key": "@LabCulA@",
					    "value": "Cultuurtype Cultuur Testopties"
					  },
					  {
					    "key": "@LabCulB@",
					    "value": "Cultuurtype lijst"
					  },
					  {
					    "key": "@LabCulC@",
					    "value": "Culture Types"
					  },
					  {
					    "key": "@LabCulD@",
					    "value": "Culture Test Setting"
					  },
					  {
					    "key": "@LabDel@",
					    "value": "Laboratorium verwijderen"
					  },
					  {
					    "key": "@LabDelA@",
					    "value": "Laboratorium verwijderen"
					  },
					  {
					    "key": "@LabDelB@",
					    "value": "Een bestaand laboratorium verwijderen"
					  },
					  {
					    "key": "@LabDelC@",
					    "value": "Testcategorieregel verwijderen"
					  },
					  {
					    "key": "@LabDelD@",
					    "value": "Test Category Rule verwijderen"
					  },
					  {
					    "key": "@LabDelE@",
					    "value": "Een bestaande testcategorie verwijderen"
					  },
					  {
					    "key": "@LabDelF@",
					    "value": "Cultuur Type categorieregel verwijderen"
					  },
					  {
					    "key": "@LabDelG@",
					    "value": "Cultuur Type Category Regel verwijderen"
					  },
					  {
					    "key": "@LabDelH@",
					    "value": "Een bestaande cultuurtype categorie verwijderen"
					  },
					  {
					    "key": "@LabDelI@",
					    "value": "Directe testopties verwijderen"
					  },
					  {
					    "key": "@LabDelJ@",
					    "value": "Opties voor cultuurtype verwijderen"
					  },
					  {
					    "key": "@LabDelK@",
					    "value": "Testopties verwijderen"
					  },
					  {
					    "key": "@LabDelL@",
					    "value": "Workflowregel voor het verwijderen van een specimentype"
					  },
					  {
					    "key": "@LabDelM@",
					    "value": "Workflowregel voor het verwijderen van een specimentype"
					  },
					  {
					    "key": "@LabDelN@",
					    "value": "Verwijder een bestaande exemplaar Type Workflow Regel"
					  },
					  {
					    "key": "@LabEdi@",
					    "value": "Laboratoriumdetails bewerken"
					  },
					  {
					    "key": "@LabEdiA@",
					    "value": "Laboratorium bewerken"
					  },
					  {
					    "key": "@LabEdiB@",
					    "value": "Label bewerken"
					  },
					  {
					    "key": "@LabEdiC@",
					    "value": "Testcategorie -regel bewerken"
					  },
					  {
					    "key": "@LabEdiD@",
					    "value": "Test Category Regel bewerken"
					  },
					  {
					    "key": "@LabEdiE@",
					    "value": "Een bestaande testcategorie -regel bewerken"
					  },
					  {
					    "key": "@LabEdiF@",
					    "value": "Cultuur Type categorieregel bewerken"
					  },
					  {
					    "key": "@LabEdiG@",
					    "value": "Cultuur Type Category Regel bewerken"
					  },
					  {
					    "key": "@LabEdiH@",
					    "value": "De inhoud van een cultuurtype categorie bewerken"
					  },
					  {
					    "key": "@LabEdiI@",
					    "value": "Directe testopties bewerken"
					  },
					  {
					    "key": "@LabEdiJ@",
					    "value": "Opties van cultuurtype bewerken"
					  },
					  {
					    "key": "@LabEdiK@",
					    "value": "Isolaattestopties bewerken"
					  },
					  {
					    "key": "@LabEdiL@",
					    "value": "Specimen Type Workflow Regel bewerken"
					  },
					  {
					    "key": "@LabEdiM@",
					    "value": "Bewerk workflowregel voor monstertype"
					  },
					  {
					    "key": "@LabEdiN@",
					    "value": "Bewerk een bestaande workflowregel voor monstertype"
					  },
					  {
					    "key": "@LabEnt@",
					    "value": "Voer laboratoriumnaam in"
					  },
					  {
					    "key": "@LabEntA@",
					    "value": "Voer nieuwe laboratoriumgegevens in"
					  },
					  {
					    "key": "@LabLab@",
					    "value": "Laboratorium"
					  },
					  {
					    "key": "@LabLabA@",
					    "value": "Laboratoriumnaam"
					  },
					  {
					    "key": "@LabLabB@",
					    "value": "Laboratoriumnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@LabLabC@",
					    "value": "Laboratoriumconfiguratie"
					  },
					  {
					    "key": "@LabMan@",
					    "value": "Laboratoria beheren"
					  },
					  {
					    "key": "@LabManA@",
					    "value": "Lijst beheren van tests die verschijnen voor een specifiek monstertype"
					  },
					  {
					    "key": "@LabManB@",
					    "value": "Beheer hoe tests zijn gecategoriseerd"
					  },
					  {
					    "key": "@LabManC@",
					    "value": "Beheer hoe cultuurtypen worden gecategoriseerd"
					  },
					  {
					    "key": "@LabManD@",
					    "value": "Lijst beheren van cultuurtypen die verschijnen voor een specifiek monstertype"
					  },
					  {
					    "key": "@LabOrg@",
					    "value": "Organismenlijsten om"
					  },
					  {
					    "key": "@LabOrgA@",
					    "value": "Lijsten met organismen"
					  },
					  {
					    "key": "@LabSpe@",
					    "value": "Specimen Type Directe testopties"
					  },
					  {
					    "key": "@LabSpeA@",
					    "value": "Typetypes van het specimentype"
					  },
					  {
					    "key": "@LabSpeB@",
					    "value": "Lijst met specimentype"
					  },
					  {
					    "key": "@LabTes@",
					    "value": "Testpatroonlijsten om te gebruiken"
					  },
					  {
					    "key": "@LabTesA@",
					    "value": "Testpatroonlijsten"
					  },
					  {
					    "key": "@LabTesB@",
					    "value": "Testcategorisatie"
					  },
					  {
					    "key": "@LabThi@",
					    "value": "Dit laboratorium bevat gebruikers en kan niet worden verwijderd"
					  },
					  {
					    "key": "@LabThiA@",
					    "value": "Dit laboratorium bevat specimens en kan niet worden verwijderd"
					  },
					  {
					    "key": "@LabThiB@",
					    "value": "dit is het laatste laboratorium en kan niet worden verwijderd"
					  },
					  {
					    "key": "@LabTxt@",
					    "value": "Patiëntlabelinhoud en dimensies bewerken. Opmerking: alle wijzigingen worden alleen van kracht uit de volgende login"
					  },
					  {
					    "key": "@LabTxtA@",
					    "value": "Bewerk specimenetiketinhoud en dimensies. Opmerking: eventuele wijzigingen worden alleen van kracht vanuit de volgende inlog"
					  },
					  {
					    "key": "@LabVie@",
					    "value": "Laboratoriumconfiguratie bekijken"
					  },
					  {
					    "key": "@LabWor@",
					    "value": "Workflows voor elk specimen type"
					  },
					  {
					    "key": "@LanA@",
					    "value": "Een te kopiëren vertaling moet worden ingevoerd"
					  },
					  {
					    "key": "@LanAA@",
					    "value": "Een vertaling om te verwijderen moet worden geselecteerd"
					  },
					  {
					    "key": "@LanAB@",
					    "value": "Een nieuwe vertaling moet worden ingevoerd"
					  },
					  {
					    "key": "@LanAdd@",
					    "value": "Vertaling toevoegen"
					  },
					  {
					    "key": "@LanAddA@",
					    "value": "Vertaling toevoegen"
					  },
					  {
					    "key": "@LanCre@",
					    "value": "Vertalingen maken en beheren"
					  },
					  {
					    "key": "@LanCreA@",
					    "value": "Een nieuwe vertaling maken"
					  },
					  {
					    "key": "@LanDel@",
					    "value": "Verwijderen vertaling"
					  },
					  {
					    "key": "@LanDelA@",
					    "value": "Verwijder een bestaande vertaling"
					  },
					  {
					    "key": "@LanDelB@",
					    "value": "Verwijderen vertaling"
					  },
					  {
					    "key": "@LanEdiA@",
					    "value": "Bewerk een vertaalinvoer"
					  },
					  {
					    "key": "@LanEdiB@",
					    "value": "Vertaalinvoer bewerken"
					  },
					  {
					    "key": "@LanEnt@",
					    "value": "Voer vertaalnaam in"
					  },
					  {
					    "key": "@LanMan@",
					    "value": "Vertalingen beheren"
					  },
					  {
					    "key": "@LanNew@",
					    "value": "Nieuwe vertaling"
					  },
					  {
					    "key": "@LanOld@",
					    "value": "Old Translation"
					  },
					  {
					    "key": "@LanSel@",
					    "value": "Selecteer vertaling"
					  },
					  {
					    "key": "@LanSelA@",
					    "value": "Selecteer vertaling"
					  },
					  {
					    "key": "@LanThi@",
					    "value": "Deze vertaling bestaat al"
					  },
					  {
					    "key": "@LanTra@",
					    "value": "vertalingen"
					  },
					  {
					    "key": "@LanTraA@",
					    "value": "vertaling"
					  },
					  {
					    "key": "@LanTraB@",
					    "value": "vertaling naar kopiëren"
					  },
					  {
					    "key": "@LanTraC@",
					    "value": "Vertaalnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@LanTraD@",
					    "value": "vertaling is in gebruik binnen een klantorganisatie"
					  },
					  {
					    "key": "@LanTraE@",
					    "value": "vertaling is in gebruik binnen een laboratorium"
					  },
					  {
					    "key": "@LocA@",
					    "value": "Er moet een locatie -ID worden verstrekt"
					  },
					  {
					    "key": "@LocAdd@",
					    "value": "Locatie toevoegen"
					  },
					  {
					    "key": "@LocCre@",
					    "value": "Nieuwe en bestaande locaties maken en beheren"
					  },
					  {
					    "key": "@LocDel@",
					    "value": "Locatie verwijderen"
					  },
					  {
					    "key": "@LocDelA@",
					    "value": "Een bestaande locatie verwijderen"
					  },
					  {
					    "key": "@LocEdi@",
					    "value": "Locatie bewerken"
					  },
					  {
					    "key": "@LocEdiA@",
					    "value": "Bestaande locatiegegevens bewerken"
					  },
					  {
					    "key": "@LocEnt@",
					    "value": "Voer nieuwe locatiegegevens in"
					  },
					  {
					    "key": "@LocEntA@",
					    "value": "Voer locatienaam in"
					  },
					  {
					    "key": "@LocEntB@",
					    "value": "Voer locatiecode in"
					  },
					  {
					    "key": "@LocEntC@",
					    "value": "Voer breedtegraad in"
					  },
					  {
					    "key": "@LocEntD@",
					    "value": "Voer lengtegraad in"
					  },
					  {
					    "key": "@LocLoc@",
					    "value": "Locatienaam"
					  },
					  {
					    "key": "@LocLocA@",
					    "value": "Locatienaam moet worden ingevoerd"
					  },
					  {
					    "key": "@LocLocB@",
					    "value": "Locatiecode"
					  },
					  {
					    "key": "@LocMan@",
					    "value": "Locaties beheren"
					  },
					  {
					    "key": "@LocPar@",
					    "value": "Ouderlocatie"
					  },
					  {
					    "key": "@LocSel@",
					    "value": "Zoeklocatie"
					  },
					  {
					    "key": "@LocSelA@",
					    "value": "Selecteer Ouderlocatie"
					  },
					  {
					    "key": "@LocThi@",
					    "value": "Deze locatie bevat onderliggende locaties en kan niet worden verwijderd"
					  },
					  {
					    "key": "@LocThiA@",
					    "value": "Deze locatie wordt door sommige patiënten gebruikt en kan niet worden verwijderd"
					  },
					  {
					    "key": "@LocThiB@",
					    "value": "Deze locatie wordt gebruikt door een leverancier en kan niet worden verwijderd"
					  },
					  {
					    "key": "@ManLab@",
					    "value": "Definities voor patiëntlabel beheren"
					  },
					  {
					    "key": "@ManLabA@",
					    "value": "Specimen Label Definities beheren"
					  },
					  {
					    "key": "@ManQcAntDis@",
					    "value": "Antibiotics (Disk)"
					  },
					  {
					    "key": "@ManQcAntMic@",
					    "value": "Antibiotics (MIC)"
					  },
					  {
					    "key": "@ManQcEna@",
					    "value": "Enabled"
					  },
					  {
					    "key": "@ManQcIncByDef@",
					    "value": "Standaard opgenomen"
					  },
					  {
					    "key": "@ManQcOrg@",
					    "value": "IQC -testprofielorganisme"
					  },
					  {
					    "key": "@ManQcOrgAbr@",
					    "value": "QC -organismen beheren"
					  },
					  {
					    "key": "@ManQcOrgAbrA@",
					    "value": "QC Organisme"
					  },
					  {
					    "key": "@ManQcOrgB@",
					    "value": "Beheer de lijst met organismen die beschikbaar zijn om te gebruiken in kwaliteitscontrole"
					  },
					  {
					    "key": "@ManQcOrgDet@",
					    "value": "Details"
					  },
					  {
					    "key": "@ManQcOrgNam@",
					    "value": "Naam"
					  },
					  {
					    "key": "@ManQcOthStr@",
					    "value": "andere stammen"
					  },
					  {
					    "key": "@ManQcPriStr@",
					    "value": "Primaire stam"
					  },
					  {
					    "key": "@ManQcStaBod@",
					    "value": "Standards Body"
					  },
					  {
					    "key": "@MapAdd@",
					    "value": "Toevoegen Mapping"
					  },
					  {
					    "key": "@MapAddA@",
					    "value": "Een nieuwe toewijzing maken"
					  },
					  {
					    "key": "@MapAddB@",
					    "value": "Toevoegen Mapping"
					  },
					  {
					    "key": "@MapAddErr@",
					    "value": "Er moet een toewijzingsnaam worden ingevoerd"
					  },
					  {
					    "key": "@MapAddErrA@",
					    "value": "Een toewijzing moet worden ingevoerd"
					  },
					  {
					    "key": "@MapAddErrB@",
					    "value": "Een toewijzing voordat de waarde moet worden ingevoerd"
					  },
					  {
					    "key": "@MapAddErrC@",
					    "value": "Een toewijzing na waarde moet worden ingevoerd"
					  },
					  {
					    "key": "@MapAft@",
					    "value": "After"
					  },
					  {
					    "key": "@MapBef@",
					    "value": "vóór"
					  },
					  {
					    "key": "@MapDel@",
					    "value": "Mapping Delete Mapping"
					  },
					  {
					    "key": "@MapDelA@",
					    "value": "Een bestaande toewijzing verwijderen"
					  },
					  {
					    "key": "@MapDelB@",
					    "value": "Mapping verwijderen"
					  },
					  {
					    "key": "@MapDelErr@",
					    "value": "Mapping kan niet worden verwijderd zoals het wordt gebruikt"
					  },
					  {
					    "key": "@MapEdi@",
					    "value": "Mapping Emapping"
					  },
					  {
					    "key": "@MapEdiA@",
					    "value": "bewerken een bestaande mapping"
					  },
					  {
					    "key": "@MapEdiB@",
					    "value": "Mapping bewerken"
					  },
					  {
					    "key": "@MapHea@",
					    "value": "Maken en beheren toewijzingen"
					  },
					  {
					    "key": "@MapSel@",
					    "value": "Selecteer een Mapping"
					  },
					  {
					    "key": "@MapTit@",
					    "value": "Mapping"
					  },
					  {
					    "key": "@MonMon@",
					    "value": "Monitor All Events"
					  },
					  {
					    "key": "@MonMonA@",
					    "value": "Monitor alle gebeurtenissen die gegevens over het systeem wijzigen"
					  },
					  {
					    "key": "@MonVie@",
					    "value": "Bekijk monitoringgebeurtenisgegevens"
					  },
					  {
					    "key": "@MonVieA@",
					    "value": "Details bekijken"
					  },
					  {
					    "key": "@MonVieB@",
					    "value": "Bekijk details (RAW)"
					  },
					  {
					    "key": "@MonVieC@",
					    "value": "Bekijk evenementdetails"
					  },
					  {
					    "key": "@MonVieD@",
					    "value": "Bekijk details van het evenement"
					  },
					  {
					    "key": "@MovBot@",
					    "value": "Ga naar de bodem"
					  },
					  {
					    "key": "@MovDow@",
					    "value": "Verplaats naar beneden"
					  },
					  {
					    "key": "@MovTop@",
					    "value": "Verplaats naar de bovenkant"
					  },
					  {
					    "key": "@MovUp@",
					    "value": "Beweeg omhoog"
					  },
					  {
					    "key": "@OrgA@",
					    "value": "Een naam van het organisme moet worden ingevoerd"
					  },
					  {
					    "key": "@OrgAdd@",
					    "value": "Klantorganisatie toevoegen"
					  },
					  {
					    "key": "@OrgAddA@",
					    "value": "Organisme toevoegen"
					  },
					  {
					    "key": "@OrgAddB@",
					    "value": "Klantorganisatie toevoegen"
					  },
					  {
					    "key": "@OrgAll@",
					    "value": "Master"
					  },
					  {
					    "key": "@OrgB@",
					    "value": "Een organisme -ID moet worden ingevoerd"
					  },
					  {
					    "key": "@OrgCha@",
					    "value": "Organisme Scope Change"
					  },
					  {
					    "key": "@OrgCod@",
					    "value": "Organisme Code"
					  },
					  {
					    "key": "@OrgCre@",
					    "value": "Creëer en beheren nieuwe en bestaande klantorganisaties"
					  },
					  {
					    "key": "@OrgDec@",
					    "value": "beslissen of het geselecteerde organisme -scope wijzigt"
					  },
					  {
					    "key": "@OrgDel@",
					    "value": "Verwijderen Client Organisation"
					  },
					  {
					    "key": "@OrgDelA@",
					    "value": "Organisme verwijderen"
					  },
					  {
					    "key": "@OrgDelB@",
					    "value": "Delete Organisation"
					  },
					  {
					    "key": "@OrgDelC@",
					    "value": "Delete Organisation"
					  },
					  {
					    "key": "@OrgDelD@",
					    "value": "Een bestaande organisatie verwijderen"
					  },
					  {
					    "key": "@OrgDon@",
					    "value": "Selecteer geen organisme -scope"
					  },
					  {
					    "key": "@OrgEdi@",
					    "value": "Client Organisation Bewerken"
					  },
					  {
					    "key": "@OrgEdiA@",
					    "value": "Bewerk de details van een bestaande klantorganisatie"
					  },
					  {
					    "key": "@OrgEdiB@",
					    "value": "Organisme bewerken"
					  },
					  {
					    "key": "@OrgEdiC@",
					    "value": "Clientorganisatie bewerken"
					  },
					  {
					    "key": "@OrgEnt@",
					    "value": "Voer nieuwe klantorganisatiegegevens in"
					  },
					  {
					    "key": "@OrgEntA@",
					    "value": "Voer de naam van de klantorganisatie in"
					  },
					  {
					    "key": "@OrgEntB@",
					    "value": "Voer de organisatiecode in"
					  },
					  {
					    "key": "@OrgEntC@",
					    "value": "Client Organisation Code MOET worden ingevoerd"
					  },
					  {
					    "key": "@OrgKee@",
					    "value": "Keep het huidige organisme Scope"
					  },
					  {
					    "key": "@OrgMan@",
					    "value": "Klantorganisaties beheren"
					  },
					  {
					    "key": "@OrgManA@",
					    "value": "Organismelijsten beheren"
					  },
					  {
					    "key": "@OrgManB@",
					    "value": "Lijsten beheren die worden gebruikt voor organismen"
					  },
					  {
					    "key": "@OrgManC@",
					    "value": "Synoniemen beheren"
					  },
					  {
					    "key": "@OrgManD@",
					    "value": "Synoniemen beheren"
					  },
					  {
					    "key": "@OrgManE@",
					    "value": "Preferente naam en synoniemen beheren voor organisme"
					  },
					  {
					    "key": "@OrgNam@",
					    "value": "Organisme Naam"
					  },
					  {
					    "key": "@OrgOrg@",
					    "value": "Naam klantorganisatie"
					  },
					  {
					    "key": "@OrgOrgA@",
					    "value": "Naam van de klantorganisatie moet worden ingevoerd"
					  },
					  {
					    "key": "@OrgOrgB@",
					    "value": "U mag geen dubbele klantorganisatie toevoegen in dezelfde hiërarchie"
					  },
					  {
					    "key": "@OrgPar@",
					    "value": "Maanorganisatie"
					  },
					  {
					    "key": "@OrgPre@",
					    "value": "Preferred Name"
					  },
					  {
					    "key": "@OrgPreA@",
					    "value": "Organisme (voorkeursnaam)"
					  },
					  {
					    "key": "@OrgSel@",
					    "value": "Selecteer Meerdagorganisatie"
					  },
					  {
					    "key": "@OrgSelA@",
					    "value": "Selecteer nieuw organisme Scope"
					  },
					  {
					    "key": "@OrgSys@",
					    "value": "Synoniemen"
					  },
					  {
					    "key": "@OrgThi@",
					    "value": "Deze organisatie bevat gebruikers en kan niet worden verwijderd"
					  },
					  {
					    "key": "@OrgThiA@",
					    "value": "deze organisatie bevat specimens en kan niet worden verwijderd"
					  },
					  {
					    "key": "@OrgThiB@",
					    "value": "Deze organisatie bevat kindorganisaties en kan niet worden verwijderd"
					  },
					  {
					    "key": "@OrgThiC@",
					    "value": "Dit organisme is geassocieerd met een cultuur en kan niet worden verwijderd"
					  },
					  {
					    "key": "@PatA@",
					    "value": "Een opmerking moet worden ingevoerd"
					  },
					  {
					    "key": "@PatAA@",
					    "value": "Een patiënt -ID is vereist"
					  },
					  {
					    "key": "@PatAdd@",
					    "value": "Een nieuwe patiënt toevoegen"
					  },
					  {
					    "key": "@PatAddA@",
					    "value": "Patiënt toevoegen"
					  },
					  {
					    "key": "@PatAddB@",
					    "value": "Patiëntadres"
					  },
					  {
					    "key": "@PatAddC@",
					    "value": "Voeg een opmerking toe aan een patiënt"
					  },
					  {
					    "key": "@PatAddD@",
					    "value": "adresregel 1"
					  },
					  {
					    "key": "@PatAddE@",
					    "value": "adresregel 2"
					  },
					  {
					    "key": "@PatAdm@",
					    "value": "toelatingsdatum"
					  },
					  {
					    "key": "@PatAge@",
					    "value": "leeftijd"
					  },
					  {
					    "key": "@PatAgeA@",
					    "value": "Leeftijd (jaren)"
					  },
					  {
					    "key": "@PatAgeB@",
					    "value": "leeftijd (maanden)"
					  },
					  {
					    "key": "@PatCli@",
					    "value": "klinisch contact telefoonnummer"
					  },
					  {
					    "key": "@PatCre@",
					    "value": "Bestaande patiëntendossiers beheren. Nieuwe patiënten kunnen alleen worden gemaakt tijdens het toevoegen van monsters"
					  },
					  {
					    "key": "@PatCreA@",
					    "value": "Een nieuw patiëntendossier maken"
					  },
					  {
					    "key": "@PatDat@",
					    "value": "Geboortedatum"
					  },
					  {
					    "key": "@PatDel@",
					    "value": "Patiënt verwijderen"
					  },
					  {
					    "key": "@PatDelA@",
					    "value": "Patiënt verwijderen"
					  },
					  {
					    "key": "@PatDelB@",
					    "value": "Een bestaande patiënt verwijderen"
					  },
					  {
					    "key": "@PatDet@",
					    "value": "Patiëntgegevens"
					  },
					  {
					    "key": "@PatDis@",
					    "value": "District"
					  },
					  {
					    "key": "@PatEdi@",
					    "value": "Patiëntdetails bewerken"
					  },
					  {
					    "key": "@PatEdiA@",
					    "value": "Patiënt bewerken"
					  },
					  {
					    "key": "@PatEdiB@",
					    "value": "Details bewerken"
					  },
					  {
					    "key": "@PatEnt@",
					    "value": "Voer een patiëntcommentaar in"
					  },
					  {
					    "key": "@PatEntA@",
					    "value": "Voer voornaam in"
					  },
					  {
					    "key": "@PatEntB@",
					    "value": "Enter achternaam"
					  },
					  {
					    "key": "@PatEntC@",
					    "value": "Enter Age (Years)"
					  },
					  {
					    "key": "@PatEntD@",
					    "value": "Voer telefoonnummer in"
					  },
					  {
					    "key": "@PatEntE@",
					    "value": "Voer nieuwe patiëntgegevens in"
					  },
					  {
					    "key": "@PatEntF@",
					    "value": "Voer patiëntreferentie in"
					  },
					  {
					    "key": "@PatEntG@",
					    "value": "Voer klinisch contactnummer in"
					  },
					  {
					    "key": "@PatEntH@",
					    "value": "Voer waarde in"
					  },
					  {
					    "key": "@PatEntI@",
					    "value": "Enter leeftijd (maanden)"
					  },
					  {
					    "key": "@PatErr@",
					    "value": "de opgegeven patiëntreferentie bestaat al"
					  },
					  {
					    "key": "@PatFin@",
					    "value": "Vind patiënt door trefwoord"
					  },
					  {
					    "key": "@PatFir@",
					    "value": "voornaam"
					  },
					  {
					    "key": "@PatGen@",
					    "value": "Geslacht moet worden ingevoerd"
					  },
					  {
					    "key": "@PatGenA@",
					    "value": "Gender"
					  },
					  {
					    "key": "@PatMan@",
					    "value": "Patiënten beheren"
					  },
					  {
					    "key": "@PatMer@",
					    "value": "Merge patiënt"
					  },
					  {
					    "key": "@PatMerA@",
					    "value": "Menig patiënt samenvoegen"
					  },
					  {
					    "key": "@PatMerB@",
					    "value": "Mergingsinhoud van de ene patiënt samenvoegen in de andere"
					  },
					  {
					    "key": "@PatMov@",
					    "value": "Specimen Verplaats"
					  },
					  {
					    "key": "@PatMovA@",
					    "value": "Monster verplaatsen naar een andere patiënt"
					  },
					  {
					    "key": "@PatPat@",
					    "value": "Patiëntreferentie moet worden ingevoerd"
					  },
					  {
					    "key": "@PatPatA@",
					    "value": "Naam van de patiënt"
					  },
					  {
					    "key": "@PatPatB@",
					    "value": "Patiënt Ref"
					  },
					  {
					    "key": "@PatPatC@",
					    "value": "Patiëntdossier"
					  },
					  {
					    "key": "@PatPatD@",
					    "value": "Patiëntgegevens"
					  },
					  {
					    "key": "@PatPatE@",
					    "value": "Patiëntcommentaar"
					  },
					  {
					    "key": "@PatPatF@",
					    "value": "Zoeken bestaande patiënten"
					  },
					  {
					    "key": "@PatPatG@",
					    "value": "Patiëntzoekresultaten"
					  },
					  {
					    "key": "@PatPatH@",
					    "value": "Details van de patiëntverzameling"
					  },
					  {
					    "key": "@PatPatI@",
					    "value": "Patiënt"
					  },
					  {
					    "key": "@PatPatJ@",
					    "value": "Patiëntlocatie"
					  },
					  {
					    "key": "@PatPatK@",
					    "value": "Patiënt bevat specimens en kan niet worden verwijderd"
					  },
					  {
					    "key": "@PatPatL@",
					    "value": "Patiënt om te fuseren van"
					  },
					  {
					    "key": "@PatPatM@",
					    "value": "Patiënt om te fuseren in"
					  },
					  {
					    "key": "@PatPro@",
					    "value": "Provincie"
					  },
					  {
					    "key": "@PatRef@",
					    "value": "Patiënt Ref"
					  },
					  {
					    "key": "@PatRefA@",
					    "value": "Een patiënt met dit referentienummer bestaat al"
					  },
					  {
					    "key": "@PatSea@",
					    "value": "Voer alle beschikbare informatie in. Als de patiënt nog niet in het systeem staat, kan een nieuw patiëntendossier worden toegevoegd op de pagina met zoekresultaten"
					  },
					  {
					    "key": "@PatSel@",
					    "value": "Selecteer datum van geboortedata"
					  },
					  {
					    "key": "@PatSelA@",
					    "value": "Selecteer Geslacht"
					  },
					  {
					    "key": "@PatSelB@",
					    "value": "Selecteer Provincie"
					  },
					  {
					    "key": "@PatSelC@",
					    "value": "Selecteer District"
					  },
					  {
					    "key": "@PatSelD@",
					    "value": "Selecteer Subdistrict"
					  },
					  {
					    "key": "@PatSelE@",
					    "value": "Selecteer een bestaande patiënt of definieer een nieuwe"
					  },
					  {
					    "key": "@PatSelF@",
					    "value": "Selecteer toelatingdatum"
					  },
					  {
					    "key": "@PatSub@",
					    "value": "Kanton"
					  },
					  {
					    "key": "@PatSur@",
					    "value": "Achternaam moet ingevuld worden"
					  },
					  {
					    "key": "@PatSurA@",
					    "value": "Achternaam"
					  },
					  {
					    "key": "@PatTel@",
					    "value": "Telefoonnummer"
					  },
					  {
					    "key": "@PatVie@",
					    "value": "Patiënt bekijken"
					  },
					  {
					    "key": "@PatZip@",
					    "value": "Zip/Postal Code"
					  },
					  {
					    "key": "@PoiCar@",
					    "value": "Details Point of Care"
					  },
					  {
					    "key": "@Qua@",
					    "value": "Quality"
					  },
					  {
					    "key": "@QuaAdd@",
					    "value": "Test Test"
					  },
					  {
					    "key": "@QuaAddA@",
					    "value": "IQC Test toevoegen"
					  },
					  {
					    "key": "@QuaAddB@",
					    "value": "IQC Test toevoegen"
					  },
					  {
					    "key": "@QuaAddC@",
					    "value": "Een nieuwe IQC -test toevoegen"
					  },
					  {
					    "key": "@QuaAddPro@",
					    "value": "Profiel toevoegen"
					  },
					  {
					    "key": "@QuaAddProA@",
					    "value": "IQC Test Profile toevoegen"
					  },
					  {
					    "key": "@QuaAddProB@",
					    "value": "Een nieuw IQC -testprofiel toevoegen"
					  },
					  {
					    "key": "@QuaAddProC@",
					    "value": "Een profielnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@QuaAddProD@",
					    "value": "Er bestaat al een profiel met deze naam"
					  },
					  {
					    "key": "@QuaAss@",
					    "value": "Quality Assurance"
					  },
					  {
					    "key": "@QuaComMes@",
					    "value": "klikken opslaan markeert deze test als voltooid en voorkomt dat resultaten worden ingevoerd of gewijzigd"
					  },
					  {
					    "key": "@QuaCon@",
					    "value": "inhoud"
					  },
					  {
					    "key": "@QuaDef@",
					    "value": "Standaardinstelling"
					  },
					  {
					    "key": "@QuaDelIqcResDes@",
					    "value": "Verwijder een interne kwaliteitscontroletestresultaat"
					  },
					  {
					    "key": "@QuaDelIqcTes@",
					    "value": "IQC Test verwijderen"
					  },
					  {
					    "key": "@QuaDelIqcTesRes@",
					    "value": "IQC -testresultaat verwijderen"
					  },
					  {
					    "key": "@QuaDelPro@",
					    "value": "Profiel verwijderen"
					  },
					  {
					    "key": "@QuaDelProA@",
					    "value": "IQC Test Profile verwijderen"
					  },
					  {
					    "key": "@QuaDelProB@",
					    "value": "Verwijder een bestaand IQC -testprofiel en al zijn instellingen"
					  },
					  {
					    "key": "@QuaDelTes@",
					    "value": "IQC Test verwijderen"
					  },
					  {
					    "key": "@QuaDelTesA@",
					    "value": "Verwijder een IQC -test en al zijn resultaten"
					  },
					  {
					    "key": "@QuaDo@",
					    "value": "Selecteer of deze organisme standaard wordt gebruikt bij het uitvoeren van IQC -tests"
					  },
					  {
					    "key": "@QuaEdi@",
					    "value": "IQC Test Profile Organisme bewerken"
					  },
					  {
					    "key": "@QuaEdiA@",
					    "value": "IQC Test Profile Organisme bewerken"
					  },
					  {
					    "key": "@QuaEdiIqcRes@",
					    "value": "IQC Test Resultaat bewerken"
					  },
					  {
					    "key": "@QuaEdiIqcTestProOrg@",
					    "value": "IQC Test Profile Organisme bewerken"
					  },
					  {
					    "key": "@QuaEdiQcOrg@",
					    "value": "QC -organismen bewerken"
					  },
					  {
					    "key": "@QuaEdiQcOrgDet@",
					    "value": "Selecteer welke kwaliteitscontrole -organismen te testen"
					  },
					  {
					    "key": "@QuaEdiQcOrgFor@",
					    "value": "QC -organismen bewerken voor IQC -test"
					  },
					  {
					    "key": "@QuaEdiTesRes@",
					    "value": "Testresultaat bewerken"
					  },
					  {
					    "key": "@QuaEnt@",
					    "value": "Testgegevens invoeren"
					  },
					  {
					    "key": "@QuaEntA@",
					    "value": "IQC Test uitvoeren"
					  },
					  {
					    "key": "@QuaEntB@",
					    "value": "Testresultaten invoeren"
					  },
					  {
					    "key": "@QuaEntC@",
					    "value": "Voer de resultaten in voor de IQC -test"
					  },
					  {
					    "key": "@QuaInh@",
					    "value": "Remming"
					  },
					  {
					    "key": "@QuaIqcProNot@",
					    "value": "Het geselecteerde IQC -testprofiel heeft geen QC -organismen ingeschakeld"
					  },
					  {
					    "key": "@QuaIqcTes@",
					    "value": "Interne kwaliteitscontroletest"
					  },
					  {
					    "key": "@QuaIqcTesA@",
					    "value": "IQC -tests"
					  },
					  {
					    "key": "@QuaIqcTesB@",
					    "value": "een of meer QC -organismen moeten worden geselecteerd"
					  },
					  {
					    "key": "@QuaIqcTesDes@",
					    "value": "nieuwe en bestaande interne kwaliteitscontroletests maken en uitvoeren"
					  },
					  {
					    "key": "@QuaIqcTesPro@",
					    "value": "IQC Test Profiles"
					  },
					  {
					    "key": "@QuaIqcTesProA@",
					    "value": "IQC Test Profile"
					  },
					  {
					    "key": "@QuaIqcTesProAnt@",
					    "value": "Selecteer antibiotica"
					  },
					  {
					    "key": "@QuaIqcTesProAntA@",
					    "value": "Selecteer welke antibiotica te gebruiken met dit IQC -testprofiel en organisme"
					  },
					  {
					    "key": "@QuaIqcTesProB@",
					    "value": "Een IQC -testprofiel moet worden ingevoerd"
					  },
					  {
					    "key": "@QuaIqcTesProDes@",
					    "value": "nieuwe en bestaande interne kwaliteitscontroletestprofielen maken en beheren"
					  },
					  {
					    "key": "@QuaIqcTesProNam@",
					    "value": "Profielnaam"
					  },
					  {
					    "key": "@QuaMarCom@",
					    "value": "Mark Complete"
					  },
					  {
					    "key": "@QuaMarIqcTesCom@",
					    "value": "Mark IQC Test Complete"
					  },
					  {
					    "key": "@QuaMarIqcTesComA@",
					    "value": "Wijzig de status van de IQC -test in voltooiing"
					  },
					  {
					    "key": "@QuaMasDis@",
					    "value": "Master (Disk)"
					  },
					  {
					    "key": "@QuaMasMic@",
					    "value": "Master (Mic)"
					  },
					  {
					    "key": "@QuaRanLow@",
					    "value": "Bereik lager"
					  },
					  {
					    "key": "@QuaRanUpp@",
					    "value": "Bereik Upper"
					  },
					  {
					    "key": "@QuaRemIdMis@",
					    "value": "De ID van het IQC -resultaat om te verwijderen ontbreekt"
					  },
					  {
					    "key": "@QuaRemIdMisA@",
					    "value": "de ID van de IQC -test om te verwijderen ontbreekt"
					  },
					  {
					    "key": "@QuaSelQcOrg@",
					    "value": "Selecteer QC -organismen"
					  },
					  {
					    "key": "@QuaSta@",
					    "value": "Status"
					  },
					  {
					    "key": "@QuaTarLow@",
					    "value": "Target Lower"
					  },
					  {
					    "key": "@QuaTarUpp@",
					    "value": "Target Upper"
					  },
					  {
					    "key": "@QuaUse@",
					    "value": "Standaard gebruik"
					  },
					  {
					    "key": "@RemCir@",
					    "value": "Circulaire referentie verwijderen"
					  },
					  {
					    "key": "@RepA@",
					    "value": "Er moet een rapportnaam worden ingevoerd"
					  },
					  {
					    "key": "@RepAnt@",
					    "value": "Antibioticum"
					  },
					  {
					    "key": "@RepAntA@",
					    "value": "Antibiogram"
					  },
					  {
					    "key": "@RepAntB@",
					    "value": "Voeg een antibioticum toe"
					  },
					  {
					    "key": "@RepApp@",
					    "value": "Uiterlijk"
					  },
					  {
					    "key": "@RepAppA@",
					    "value": "Rapport goedkeuren"
					  },
					  {
					    "key": "@RepAppB@",
					    "value": "Rapport goedkeuren zodat het kan worden bekeken door klantorganisaties"
					  },
					  {
					    "key": "@RepAppC@",
					    "value": "Rapport goedkeuren"
					  },
					  {
					    "key": "@RepAppD@",
					    "value": "Batch goedkeuren rapport"
					  },
					  {
					    "key": "@RepAppE@",
					    "value": "Keur alle geselecteerde rapporten goed, zodat ze door cliëntorganisaties kunnen worden bekeken"
					  },
					  {
					    "key": "@RepAppF@",
					    "value": "Goedgekeurde datum"
					  },
					  {
					    "key": "@RepAst@",
					    "value": "AST -informatie"
					  },
					  {
					    "key": "@RepAstA@",
					    "value": "AST -resultaten om af te drukken:"
					  },
					  {
					    "key": "@RepAur@",
					    "value": "Auramine"
					  },
					  {
					    "key": "@RepBat@",
					    "value": "Print Reports"
					  },
					  {
					    "key": "@RepBatA@",
					    "value": "Batch publiceren"
					  },
					  {
					    "key": "@RepBatB@",
					    "value": "Batch goedkeuren rapport"
					  },
					  {
					    "key": "@RepBatC@",
					    "value": "Batch weigeren rapport"
					  },
					  {
					    "key": "@RepBatD@",
					    "value": "Batch weigeren rapport"
					  },
					  {
					    "key": "@RepBatE@",
					    "value": "Weiger alle geselecteerde rapporten zodat ze niet kunnen worden bekeken door klantorganisaties"
					  },
					  {
					    "key": "@RepCel@",
					    "value": "Cell Count"
					  },
					  {
					    "key": "@RepCho@",
					    "value": "Kies Antibiotics"
					  },
					  {
					    "key": "@RepChoA@",
					    "value": "Kies organismen"
					  },
					  {
					    "key": "@RepCom@",
					    "value": "Specimen Reacties to Print:"
					  },
					  {
					    "key": "@RepComA@",
					    "value": "Culture Reacties to Print:"
					  },
					  {
					    "key": "@RepComB@",
					    "value": "AST -opmerkingen om af te drukken:"
					  },
					  {
					    "key": "@RepCul@",
					    "value": "Cultuurresultaten"
					  },
					  {
					    "key": "@RepCulA@",
					    "value": "Cultuurresultaten om af te drukken:"
					  },
					  {
					    "key": "@RepCulB@",
					    "value": "Cultuurtestresultaten om af te drukken:"
					  },
					  {
					    "key": "@RepCulC@",
					    "value": "Culture Print Selector"
					  },
					  {
					    "key": "@RepCulD@",
					    "value": "Selecteer welke isolatetests en AST-resultaten in het rapport verschijnen"
					  },
					  {
					    "key": "@RepDat@",
					    "value": "Rapportdatum"
					  },
					  {
					    "key": "@RepDef@",
					    "value": "Standaard koptekst monsterrapport"
					  },
					  {
					    "key": "@RepDefA@",
					    "value": "Standaard monsterrapport"
					  },
					  {
					    "key": "@RepDip@",
					    "value": "Pijlstok"
					  },
					  {
					    "key": "@RepDir@",
					    "value": "Directe testresultaten om af te drukken:"
					  },
					  {
					    "key": "@RepDor@",
					    "value": "Rapporten hebben goedkeuring nodig?"
					  },
					  {
					    "key": "@RepFin@",
					    "value": "Eindrapport"
					  },
					  {
					    "key": "@RepGra@",
					    "value": "Gram Stain"
					  },
					  {
					    "key": "@RepIfy@",
					    "value": "Als u het resultaat of de behandeling wilt bespreken, bel dan het laboratorium van de microbiologie"
					  },
					  {
					    "key": "@RepImg@",
					    "value": "Images"
					  },
					  {
					    "key": "@RepInd@",
					    "value": "India Ink"
					  },
					  {
					    "key": "@RepMic@",
					    "value": "Microbiology Laboratory Report"
					  },
					  {
					    "key": "@RepNam@",
					    "value": "Rapportnaam"
					  },
					  {
					    "key": "@RepOrg@",
					    "value": "Organisme met groei"
					  },
					  {
					    "key": "@RepPos@",
					    "value": "Rapportpositie"
					  },
					  {
					    "key": "@RepPre@",
					    "value": "Preculture Resultaten"
					  },
					  {
					    "key": "@RepPreA@",
					    "value": "Datum"
					  },
					  {
					    "key": "@RepPri@",
					    "value": "Print / publiceren Specimen Report"
					  },
					  {
					    "key": "@RepPriA@",
					    "value": "Print and Publish Report"
					  },
					  {
					    "key": "@RepPriB@",
					    "value": "Een selectie van rapporten afdrukken"
					  },
					  {
					    "key": "@RepPriC@",
					    "value": "Publiceer een aantal rapporten"
					  },
					  {
					    "key": "@RepPub@",
					    "value": "Publish Report"
					  },
					  {
					    "key": "@RepPubA@",
					    "value": "Gepubliceerde rapporten"
					  },
					  {
					    "key": "@RepPatLoc@",
					    "value": "Patiëntlocatie"
					  },
					  {
					    "key": "@RepRef@",
					    "value": "Patiënt Ref"
					  },
					  {
					    "key": "@RepRep@",
					    "value": "Report History"
					  },
					  {
					    "key": "@RepRepA@",
					    "value": "Rapport met succes gepubliceerd"
					  },
					  {
					    "key": "@RepRepB@",
					    "value": "Rapport met succes gedrukt"
					  },
					  {
					    "key": "@RepRepC@",
					    "value": "Report Viewer"
					  },
					  {
					    "key": "@RepRepD@",
					    "value": "Rapportgoedkeuring"
					  },
					  {
					    "key": "@RepRes@",
					    "value": "Resultaat"
					  },
					  {
					    "key": "@RepSen@",
					    "value": "Sensitivity"
					  },
					  {
					    "key": "@RepSho@",
					    "value": "Show Report"
					  },
					  {
					    "key": "@RepSin@",
					    "value": "Single Column Format (1)"
					  },
					  {
					    "key": "@RepSpe@",
					    "value": "Specimen Report"
					  },
					  {
					    "key": "@RepSpeA@",
					    "value": "Specimen Print Preview"
					  },
					  {
					    "key": "@RepSta@",
					    "value": "Rapportstatusweergave in kop"
					  },
					  {
					    "key": "@RepTab@",
					    "value": "Tabelindeling één (4 kolommen)"
					  },
					  {
					    "key": "@RepTwo@",
					    "value": "Twee kolomformaat (1)"
					  },
					  {
					    "key": "@RepTwoA@",
					    "value": "Twee kolomformaat (2)"
					  },
					  {
					    "key": "@RepTwoB@",
					    "value": "Twee kolom met roosterindeling (1)"
					  },
					  {
					    "key": "@RepTwoC@",
					    "value": "Twee kolomformaat (3)"
					  },
					  {
					    "key": "@RepTwoD@",
					    "value": "Twee kolommen met roosterindeling (2)"
					  },
					  {
					    "key": "@RepUna@",
					    "value": "Rapport afwijzen"
					  },
					  {
					    "key": "@RepUnaA@",
					    "value": "Rapport afwijzen zodat het niet langer kan worden bekeken door klantorganisaties"
					  },
					  {
					    "key": "@RepUnaB@",
					    "value": "Rapport afwijzen"
					  },
					  {
					    "key": "@RepVie@",
					    "value": "Bekijk alle gepubliceerde en goedgekeurde rapporten"
					  },
					  {
					    "key": "@RepVieA@",
					    "value": "Beheer het proces van het beoordelen van rapporten ter goedkeuring zodat ze kunnen worden gezien door klantorganisaties"
					  },
					  {
					    "key": "@RepWet@",
					    "value": "Wet Prep"
					  },
					  {
					    "key": "@RepZns@",
					    "value": "Zn Stain"
					  },
					  {
					    "key": "@RolAdd@",
					    "value": "Rol toevoegen"
					  },
					  {
					    "key": "@RolAddA@",
					    "value": "Een nieuwe rol toevoegen"
					  },
					  {
					    "key": "@RolCan@",
					    "value": "rolnaam moet uniek zijn"
					  },
					  {
					    "key": "@RolClo@",
					    "value": "kloon een rol"
					  },
					  {
					    "key": "@RolCloA@",
					    "value": "kloonrol"
					  },
					  {
					    "key": "@RolCloB@",
					    "value": "Kloon een bestaande rol op het systeem. Hiermee worden de toestemmingsgegevens van de bestaande rol naar de nieuwe rol gekopieerd"
					  },
					  {
					    "key": "@RolCon@",
					    "value": "Configureer algemene aspecten van de rol"
					  },
					  {
					    "key": "@RolConA@",
					    "value": "Configuratie moet worden ingevoerd"
					  },
					  {
					    "key": "@RolConB@",
					    "value": "Configureer gebeurtenisrechten voor deze rol"
					  },
					  {
					    "key": "@RolConC@",
					    "value": "Configureer menurechten voor deze rol"
					  },
					  {
					    "key": "@RolCre@",
					    "value": "Nieuwe en bestaande rollen aanmaken en beheren"
					  },
					  {
					    "key": "@RolDel@",
					    "value": "Een rol verwijderen"
					  },
					  {
					    "key": "@RolDelA@",
					    "value": "Rol verwijderen"
					  },
					  {
					    "key": "@RolDes@",
					    "value": "Rolbeschrijving moet worden ingevoerd"
					  },
					  {
					    "key": "@RolEdi@",
					    "value": "Bestaande rol bewerken"
					  },
					  {
					    "key": "@RolEdiA@",
					    "value": "Rol bewerken"
					  },
					  {
					    "key": "@RolEdiB@",
					    "value": "de algemene aspecten van een bestaande rol bewerken"
					  },
					  {
					    "key": "@RolEdiC@",
					    "value": "Role"
					  },
					  {
					    "key": "@RolEna@",
					    "value": "ingeschakeld moet worden ingevoerd"
					  },
					  {
					    "key": "@RolEve@",
					    "value": "Evenementmachtigingen"
					  },
					  {
					    "key": "@RolLab@",
					    "value": "Laboratoriumbeheerder"
					  },
					  {
					    "key": "@RolMan@",
					    "value": "Rollen beheren"
					  },
					  {
					    "key": "@RolManA@",
					    "value": "Evenementmachtigingen beheren"
					  },
					  {
					    "key": "@RolManB@",
					    "value": "Menu -machtigingen beheren"
					  },
					  {
					    "key": "@RolMen@",
					    "value": "Menu Machtigingen"
					  },
					  {
					    "key": "@RolNam@",
					    "value": "Rolaam moet worden ingevoerd"
					  },
					  {
					    "key": "@RolNew@",
					    "value": "Nieuwe roldetails"
					  },
					  {
					    "key": "@RolOrg@",
					    "value": "Organisatiebeheerder"
					  },
					  {
					    "key": "@RolRem@",
					    "value": "Een rol verwijderen uit het systeem"
					  },
					  {
					    "key": "@RolRol@",
					    "value": "Rolbeschrijving"
					  },
					  {
					    "key": "@RolRolA@",
					    "value": "Rol Name"
					  },
					  {
					    "key": "@RolRolB@",
					    "value": "Rol Record"
					  },
					  {
					    "key": "@RolRolC@",
					    "value": "Rollen"
					  },
					  {
					    "key": "@RolRolD@",
					    "value": "Role to Clone"
					  },
					  {
					    "key": "@RolSel@",
					    "value": "Select Roles"
					  },
					  {
					    "key": "@RolThi@",
					    "value": "Deze rol is in gebruik en kan niet worden verwijderd"
					  },
					  {
					    "key": "@RolUpd@",
					    "value": "Event Permissions Update"
					  },
					  {
					    "key": "@RolUpdA@",
					    "value": "Update Menu Permissies"
					  },
					  {
					    "key": "@RunExpD@",
					    "value": "Voer filterwaarden in. Startdatum en einddatum moeten worden verstrekt"
					  },
					  {
					    "key": "@RunExpE@",
					    "value": "Startdatum en einddatum moeten worden ingevoerd"
					  },
					  {
					    "key": "@RunExpT@",
					    "value": "Voer een export uit"
					  },
					  {
					    "key": "@SeaEnt@",
					    "value": "Voer zoek tekens in"
					  },
					  {
					    "key": "@SerA@",
					    "value": "Een serotype -naam moet worden ingevoerd"
					  },
					  {
					    "key": "@SerAdd@",
					    "value": "Serotype toevoegen"
					  },
					  {
					    "key": "@SerDel@",
					    "value": "Serotype verwijderen"
					  },
					  {
					    "key": "@SetAddAccTex@",
					    "value": "Tekst toevoegen"
					  },
					  {
					    "key": "@SetAddAccTexB@",
					    "value": "Tekst toevoegen aan het toetredingsnummer"
					  },
					  {
					    "key": "@SetAddAccTexC@",
					    "value": "Tekst om binnen toegangsnummer te plaatsen"
					  },
					  {
					    "key": "@SetAddPatTex@",
					    "value": "Patiëntverwijzing Tekst toevoegen"
					  },
					  {
					    "key": "@SetAddPatTexB@",
					    "value": "Tekst toevoegen aan de patiëntreferentie"
					  },
					  {
					    "key": "@SetAddPatTexC@",
					    "value": "Tekst om te plaatsen in de patiëntreferentie"
					  },
					  {
					    "key": "@SetAll@",
					    "value": "alle vermeldingen moeten een toewijzing bevatten"
					  },
					  {
					    "key": "@SetDel@",
					    "value": "instelling verwijderen"
					  },
					  {
					    "key": "@SetDelA@",
					    "value": "Een instelling verwijderen"
					  },
					  {
					    "key": "@SetEdi@",
					    "value": "Accessienummer bewerken"
					  },
					  {
					    "key": "@SetEdiAccTex@",
					    "value": "Tekst bewerken"
					  },
					  {
					    "key": "@SetEdiAccTexB@",
					    "value": "Tekst bewerken geplaatst in het toegangsnummer"
					  },
					  {
					    "key": "@SetEdiPat@",
					    "value": "Patiëntreferentie bewerken"
					  },
					  {
					    "key": "@SetEdiPatTex@",
					    "value": "Tekst bewerken"
					  },
					  {
					    "key": "@SetEdiPatTexB@",
					    "value": "Tekst bewerken in de patiëntreferentie"
					  },
					  {
					    "key": "@SetNum@",
					    "value": "Nummerreeks"
					  },
					  {
					    "key": "@SetNumA@",
					    "value": "De lengte van het nummer moet tussen 3 en 13 liggen"
					  },
					  {
					    "key": "@SetOrd@",
					    "value": "Volgorde van toegangsnummers"
					  },
					  {
					    "key": "@SetOrdPat@",
					    "value": "Volgorde van patiëntreferenties"
					  },
					  {
					    "key": "@SetScr@",
					    "value": "Waarde van de schermtime-out moet tussen 5 en 1000 liggen"
					  },
					  {
					    "key": "@SetSet@",
					    "value": "Stel de volgorde van de secties van de toegangsnummer in"
					  },
					  {
					    "key": "@SetSetPat@",
					    "value": "Stel de volgorde van de patiëntreferentiesecties in"
					  },
					  {
					    "key": "@SetTexNulErr@",
					    "value": "Tekst kan niet leeg zijn"
					  },
					  {
					    "key": "@SinCul@",
					    "value": "Cultuur"
					  },
					  {
					    "key": "@SinIso@",
					    "value": "Isolaat"
					  },
					  {
					    "key": "@SinPat@",
					    "value": "Patiënt"
					  },
					  {
					    "key": "@SinRol@",
					    "value": "Rol"
					  },
					  {
					    "key": "@SinSpe@",
					    "value": "Specimen"
					  },
					  {
					    "key": "@SinSpeA@",
					    "value": "Specimen"
					  },
					  {
					    "key": "@SpcA@",
					    "value": "Een soortnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@SpcAdd@",
					    "value": "Soorten toevoegen"
					  },
					  {
					    "key": "@SpcDel@",
					    "value": "Soorten verwijderen"
					  },
					  {
					    "key": "@SpeA@",
					    "value": "Er moet een opmerking worden ingevoerd"
					  },
					  {
					    "key": "@SpeAcc@",
					    "value": "toetredingsnummer"
					  },
					  {
					    "key": "@SpeAck@",
					    "value": "Bevestig ontvangst van het exemplaar"
					  },
					  {
					    "key": "@SpeAckA@",
					    "value": "bevestig ontvangst"
					  },
					  {
					    "key": "@SpeAckB@",
					    "value": "Initiële beoordeling"
					  },
					  {
					    "key": "@SpeAdd@",
					    "value": "een nieuwe cultuur toevoegen"
					  },
					  {
					    "key": "@SpeAddA@",
					    "value": "Record toevoegen voor ontvangen specimen"
					  },
					  {
					    "key": "@SpeAddB@",
					    "value": "Nieuw monster op afstand toevoegen"
					  },
					  {
					    "key": "@SpeAddC@",
					    "value": "Nieuw exemplaar toevoegen"
					  },
					  {
					    "key": "@SpeAddD@",
					    "value": "Cultuur toevoegen"
					  },
					  {
					    "key": "@SpeAddE@",
					    "value": "Toevoegen ontvangen specimen"
					  },
					  {
					    "key": "@SpeAddF@",
					    "value": "Aanvullende informatie"
					  },
					  {
					    "key": "@SpeAddG@",
					    "value": "ALLE OPTROKKENDE OPMERKINGEN VOEGEN"
					  },
					  {
					    "key": "@SpeAddH@",
					    "value": "een opmerking toevoegen aan een monster"
					  },
					  {
					    "key": "@SpeAddI@",
					    "value": "Selecteer een standaardcommentaar"
					  },
					  {
					    "key": "@SpeAddJ@",
					    "value": "Toevoegen Remote Specimen"
					  },
					  {
					    "key": "@SpeAddK@",
					    "value": "Specimen toevoegen"
					  },
					  {
					    "key": "@SpeAddL@",
					    "value": "Voeg een opmerking toe aan een cultuur"
					  },
					  {
					    "key": "@SpeAddM@",
					    "value": "Voeg een nieuw isolaat toe"
					  },
					  {
					    "key": "@SpeAddN@",
					    "value": "Isolaat toevoegen"
					  },
					  {
					    "key": "@SpeAdi@",
					    "value": "Een diagnose moet worden geselecteerd"
					  },
					  {
					    "key": "@SpeAdv@",
					    "value": "Monst nog niet verzonden"
					  },
					  {
					    "key": "@SpeAli@",
					    "value": "aliquot id"
					  },
					  {
					    "key": "@SpeAliA@",
					    "value": "aliquot bewerken"
					  },
					  {
					    "key": "@SpeAlr@",
					    "value": "Monster al ontvangen"
					  },
					  {
					    "key": "@SpeAn@",
					    "value": "Er moet een afdeling worden geselecteerd"
					  },
					  {
					    "key": "@SpeAnA@",
					    "value": "Er moet een laboratorium worden geselecteerd"
					  },
					  {
					    "key": "@SpeApi@",
					    "value": "API/ID Panel"
					  },
					  {
					    "key": "@SpeApp@",
					    "value": "De ingediende monsteranalyse goedkeuren of afwijzen"
					  },
					  {
					    "key": "@SpeAppA@",
					    "value": "Goedkeuringen"
					  },
					  {
					    "key": "@SpeAppB@",
					    "value": "ingediend door"
					  },
					  {
					    "key": "@SpeAppC@",
					    "value": "goedgekeurd door"
					  },
					  {
					    "key": "@SpeAppDat@",
					    "value": "Goedgekeurde datum"
					  },
					  {
					    "key": "@SpeAppD@",
					    "value": "Besluitdatum"
					  },
					  {
					    "key": "@SpeAsp@",
					    "value": "Een exemplaar moet worden geselecteerd"
					  },
					  {
					    "key": "@SpeAss@",
					    "value": "Beoordeel elk ontvangen exemplaar in de batch en ga dan naar de volgende"
					  },
					  {
					    "key": "@SpeAssA@",
					    "value": "Beoordeel elke cultuur in de batch op groei en ga dan naar de volgende"
					  },
					  {
					    "key": "@SpeAte@",
					    "value": "Er moet een test-ID worden opgegeven om te worden verwijderd"
					  },
					  {
					    "key": "@SpeB@",
					    "value": "Er moet een commentaartype worden ingevoerd"
					  },
					  {
					    "key": "@SpeBat@",
					    "value": "Batchproces dag 0 monsters"
					  },
					  {
					    "key": "@SpeBatA@",
					    "value": "Batch Process Day 1 Specimens"
					  },
					  {
					    "key": "@SpeBatB@",
					    "value": "Batch Specimen Goedkeuringsniveau 1"
					  },
					  {
					    "key": "@SpeBatC@",
					    "value": "Batch Specimen Goedkeuringsniveau 2"
					  },
					  {
					    "key": "@SpeBatD@",
					    "value": "Batch Subsubt Bevestiging"
					  },
					  {
					    "key": "@SpeBatE@",
					    "value": "Batch Processing"
					  },
					  {
					    "key": "@SpeBatF@",
					    "value": "Batch Submit"
					  },
					  {
					    "key": "@SpeBatG@",
					    "value": "Batch First Approveuring"
					  },
					  {
					    "key": "@SpeBatH@",
					    "value": "Batch Second Approveuring"
					  },
					  {
					    "key": "@SpeBatI@",
					    "value": "Batch Tag Toevoegen"
					  },
					  {
					    "key": "@SpeBlo@",
					    "value": "Blood Culture Blood and Bottle Weight (G)"
					  },
					  {
					    "key": "@SpeBot@",
					    "value": "Bloedcultuurfles alleen gewicht (G)"
					  },
					  {
					    "key": "@SpeCan@",
					    "value": "Een specimenverzoek annuleren"
					  },
					  {
					    "key": "@SpeCanA@",
					    "value": "Verzoek annuleren"
					  },
					  {
					    "key": "@SpeCanB@",
					    "value": "Annuleer het verzoek om een ​​monster"
					  },
					  {
					    "key": "@SpeCanC@",
					    "value": "annuleringsredenen"
					  },
					  {
					    "key": "@SpeCanD@",
					    "value": "kan de groeiwaarde niet veranderen van groei tot geen groei"
					  },
					  {
					    "key": "@SpeCha@",
					    "value": "Wijzig het monster in een vorige status"
					  },
					  {
					    "key": "@SpeCol@",
					    "value": "Verzamelingsdatum moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeColA@",
					    "value": "Verzamelingstijd moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeColB@",
					    "value": "Verzamelingsdatum/tijd"
					  },
					  {
					    "key": "@SpeColC@",
					    "value": "Verzamelingsdatum"
					  },
					  {
					    "key": "@SpeColD@",
					    "value": "Collection Time (24H)"
					  },
					  {
					    "key": "@SpeCre@",
					    "value": "Nieuwe en bestaande culturen maken en beheren"
					  },
					  {
					    "key": "@SpeCreA@",
					    "value": "Nieuwe en bestaande exemplaarrecords maken en beheren. Nieuwe patiënten kunnen worden toegevoegd tijdens het maken van specimens"
					  },
					  {
					    "key": "@SpeCul@",
					    "value": "Culture Details"
					  },
					  {
					    "key": "@SpeCulA@",
					    "value": "Isoleer record"
					  },
					  {
					    "key": "@SpeCulB@",
					    "value": "Cultures"
					  },
					  {
					    "key": "@SpeCulC@",
					    "value": "isoleren tests"
					  },
					  {
					    "key": "@SpeDay@",
					    "value": "Dag 1 bank Read"
					  },
					  {
					    "key": "@SpeDec@",
					    "value": "Beslissing is vereist"
					  },
					  {
					    "key": "@SpeDel@",
					    "value": "Delete Isolate"
					  },
					  {
					    "key": "@SpeDelA@",
					    "value": "Isolaat verwijderen"
					  },
					  {
					    "key": "@SpeDet@",
					    "value": "Specimen Details"
					  },
					  {
					    "key": "@SpeDir@",
					    "value": "Directe tests"
					  },
					  {
					    "key": "@SpeEdi@",
					    "value": "Een bestaande cultuur bewerken"
					  },
					  {
					    "key": "@SpeEdiA@",
					    "value": "isolaat bewerken"
					  },
					  {
					    "key": "@SpeEdiB@",
					    "value": "Bewerken Specimen"
					  },
					  {
					    "key": "@SpeEdiC@",
					    "value": "Specimen bewerken"
					  },
					  {
					    "key": "@SpeEnt@",
					    "value": "Voer een specimencommentaar in"
					  },
					  {
					    "key": "@SpeEntA@",
					    "value": "Enter ontvangen tijd"
					  },
					  {
					    "key": "@SpeEntB@",
					    "value": "Enter Weight"
					  },
					  {
					    "key": "@SpeEntC@",
					    "value": "Voer afwijzingsredenen in"
					  },
					  {
					    "key": "@SpeEntD@",
					    "value": "Voer aanvullende details in"
					  },
					  {
					    "key": "@SpeEntE@",
					    "value": "Voer bestaande bar-code in, indien aanwezig"
					  },
					  {
					    "key": "@SpeEntF@",
					    "value": "Voer ID Percentage in"
					  },
					  {
					    "key": "@SpeEntG@",
					    "value": "Voer de datum van het resultaat in (DD/MM/YYYY)"
					  },
					  {
					    "key": "@SpeEntH@",
					    "value": "Voer tijd van resultaat in"
					  },
					  {
					    "key": "@SpeEntI@",
					    "value": "Voer Collection Time in"
					  },
					  {
					    "key": "@SpeEntJ@",
					    "value": "Voer annuleringsredenen in"
					  },
					  {
					    "key": "@SpeEsb@",
					    "value": "ESBL"
					  },
					  {
					    "key": "@SpeExi@",
					    "value": "Bestaande barcode"
					  },
					  {
					    "key": "@SpeFir@",
					    "value": "Eerste niveau goedkeuring"
					  },
					  {
					    "key": "@SpeFul@",
					    "value": "Volledig organisme zoeken"
					  },
					  {
					    "key": "@SpeFulA@",
					    "value": "Volledig verwijdert isolaat uit het huidige monster"
					  },
					  {
					    "key": "@SpeGro@",
					    "value": "groei moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeGroA@",
					    "value": "groei?"
					  },
					  {
					    "key": "@SpeGroB@",
					    "value": "Groeiketails"
					  },
					  {
					    "key": "@SpeGroC@",
					    "value": "GROEI"
					  },
					  {
					    "key": "@SpeId@",
					    "value": "ID PROFIEL"
					  },
					  {
					    "key": "@SpeIdA@",
					    "value": "% id"
					  },
					  {
					    "key": "@SpeIde@",
					    "value": "Identificatiemethode"
					  },
					  {
					    "key": "@SpeImm@",
					    "value": "Onmiddellijke actie"
					  },
					  {
					    "key": "@SpeInd@",
					    "value": "geven de toestand van het monster aan en de reden voor afwijzing, zo niet impliciet"
					  },
					  {
					    "key": "@SpeIndA@",
					    "value": "geeft aan wat u gaat doen met het monster"
					  },
					  {
					    "key": "@SpeMan@",
					    "value": "Cultures beheren"
					  },
					  {
					    "key": "@SpeManA@",
					    "value": "Directe tests beheren"
					  },
					  {
					    "key": "@SpeManB@",
					    "value": "Directe tests beheren"
					  },
					  {
					    "key": "@SpeManC@",
					    "value": "Specimens beheren voor patiënt"
					  },
					  {
					    "key": "@SpeManD@",
					    "value": "Monsters beheren"
					  },
					  {
					    "key": "@SpeManE@",
					    "value": "Beheer de specimens die zijn afgerond"
					  },
					  {
					    "key": "@SpeNex@",
					    "value": "Next Specimen"
					  },
					  {
					    "key": "@SpeOrg@",
					    "value": "Organisme moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeOrgA@",
					    "value": "Organisme Identity"
					  },
					  {
					    "key": "@SpeOth@",
					    "value": "Archive Information"
					  },
					  {
					    "key": "@SpePat@",
					    "value": "Patiëntreferentie"
					  },
					  {
					    "key": "@SpePos@",
					    "value": "Positieve datum/tijd"
					  },
					  {
					    "key": "@SpePosA@",
					    "value": "Resultaatdatum"
					  },
					  {
					    "key": "@SpePosB@",
					    "value": "Resultaattijd (24 uur)"
					  },
					  {
					    "key": "@SpePro@",
					    "value": "Proces Today's specimens"
					  },
					  {
					    "key": "@SpeProA@",
					    "value": "verstrekken details met betrekking tot het monster als fysiek ontvangen"
					  },
					  {
					    "key": "@SpeProB@",
					    "value": "Patiëntspecifieke specimenverzamelingsgegevens verstrekken"
					  },
					  {
					    "key": "@SpeProC@",
					    "value": "Verstrek eventuele kwalificaties, begeleiding of aanvullende informatie van belang"
					  },
					  {
					    "key": "@SpeProD@",
					    "value": "Aangeboden exemplaarkenmerken"
					  },
					  {
					    "key": "@SpeProE@",
					    "value": "Details van identificatiemethode verstrekken"
					  },
					  {
					    "key": "@SpeProF@",
					    "value": "het organisme opgeven dat wordt geïdentificeerd of wordt gescreend door de naam of code in te voeren"
					  },
					  {
					    "key": "@SpeProG@",
					    "value": "Archiefgegevens bieden, waar van toepassing"
					  },
					  {
					    "key": "@SpeProH@",
					    "value": "Geef aliquot -identificatie indien nodig"
					  },
					  {
					    "key": "@SpeProI@",
					    "value": "geef belangrijke informatie over de huidige zorgaflevering van de patiënt"
					  },
					  {
					    "key": "@SpeProJ@",
					    "value": "Geef belangrijke datums en tijden op"
					  },
					  {
					    "key": "@SpeProK@",
					    "value": "specificeer het type cultuur en geef informatie over de omvang en timing van groei"
					  },
					  {
					    "key": "@SpeRea@",
					    "value": "Reden voor afwijzing"
					  },
					  {
					    "key": "@SpeReaA@",
					    "value": "Afwijzing Reden"
					  },
					  {
					    "key": "@SpeReaB@",
					    "value": "Niveau 1 Review Free Text Comment"
					  },
					  {
					    "key": "@SpeReaC@",
					    "value": "Level 2 Review Free Text Comment"
					  },
					  {
					    "key": "@SpeReaD@",
					    "value": "Level 1 Review Standard Comment"
					  },
					  {
					    "key": "@SpeReaE@",
					    "value": "Level 2 Review Standard Comment"
					  },
					  {
					    "key": "@SpeReaF@",
					    "value": "Standard Review Comment"
					  },
					  {
					    "key": "@SpeReaG@",
					    "value": "Standard Regjection Comment"
					  },
					  {
					    "key": "@SpeReaH@",
					    "value": "Gratis tekstafwijzingscommentaar"
					  },
					  {
					    "key": "@SpeRec@",
					    "value": "Ontvangen datum moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeRecA@",
					    "value": "Ontvangen tijd moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeRecB@",
					    "value": "Ontvangen voorwaarde"
					  },
					  {
					    "key": "@SpeRecC@",
					    "value": "Ontvangen datum/tijd"
					  },
					  {
					    "key": "@SpeRecD@",
					    "value": "Ontvangen datum"
					  },
					  {
					    "key": "@SpeRecE@",
					    "value": "ontvangen tijd (24 uur)"
					  },
					  {
					    "key": "@SpeRecF@",
					    "value": "Ontvangen voorwaarde moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeRecG@",
					    "value": "Ontmelding van de ontvangst"
					  },
					  {
					    "key": "@SpeRej@",
					    "value": "Specimen afwijzen"
					  },
					  {
					    "key": "@SpeRejA@",
					    "value": "Specimen afwijzen"
					  },
					  {
					    "key": "@SpeRes@",
					    "value": "Status wijzigen"
					  },
					  {
					    "key": "@SpeSec@",
					    "value": "Tweede niveau goedkeuring"
					  },
					  {
					    "key": "@SpeSel@",
					    "value": "Selecteer Ontvangen datum"
					  },
					  {
					    "key": "@SpeSelA@",
					    "value": "Selecteer Specimen Condition"
					  },
					  {
					    "key": "@SpeSelB@",
					    "value": "Selecteer Specimen -verschijning"
					  },
					  {
					    "key": "@SpeSelC@",
					    "value": "Selecteer Specimen Type"
					  },
					  {
					    "key": "@SpeSelD@",
					    "value": "Selecteer Specimen -site"
					  },
					  {
					    "key": "@SpeSelE@",
					    "value": "Selecteer Analytical Profile Index gebruikt"
					  },
					  {
					    "key": "@SpeSelF@",
					    "value": "Selecteer ID -profiel"
					  },
					  {
					    "key": "@SpeSelG@",
					    "value": "Selecteer Organisme Naam"
					  },
					  {
					    "key": "@SpeSelH@",
					    "value": "Selecteer Mate van groei"
					  },
					  {
					    "key": "@SpeSelI@",
					    "value": "Selecteer Collection Date"
					  },
					  {
					    "key": "@SpeSelJ@",
					    "value": "Selecteer cultuurorganisme"
					  },
					  {
					    "key": "@SpeSelK@",
					    "value": "Selecteer cultuurtypen"
					  },
					  {
					    "key": "@SpeSelM@",
					    "value": "Selecteer alle culturen die u waarschijnlijk zult uitvoeren. Culturen kunnen later worden toegevoegd of verwijderd"
					  },
					  {
					    "key": "@SpeSer@",
					    "value": "serotype"
					  },
					  {
					    "key": "@SpeSerA@",
					    "value": "serotype profiel"
					  },
					  {
					    "key": "@SpeSpe@",
					    "value": "Monstergoedkeuringsniveau 1"
					  },
					  {
					    "key": "@SpeSpeA@",
					    "value": "Monstergoedkeuringsniveau 2"
					  },
					  {
					    "key": "@SpeSpeB@",
					    "value": "Specimentype"
					  },
					  {
					    "key": "@SpeSpeC@",
					    "value": "Specimenlocatie"
					  },
					  {
					    "key": "@SpeSpeD@",
					    "value": "Specimengewicht"
					  },
					  {
					    "key": "@SpeSpeE@",
					    "value": "Specimen Condition"
					  },
					  {
					    "key": "@SpeSpeF@",
					    "value": "Specimen uiterlijk"
					  },
					  {
					    "key": "@SpeSpeG@",
					    "value": "Specimen Record"
					  },
					  {
					    "key": "@SpeSpeH@",
					    "value": "Specimen Action"
					  },
					  {
					    "key": "@SpeSpeI@",
					    "value": "Specimen Approcedure"
					  },
					  {
					    "key": "@SpeSpeJ@",
					    "value": "Specimen -attributen"
					  },
					  {
					    "key": "@SpeSpeK@",
					    "value": "Serotype opgeven indien nodig"
					  },
					  {
					    "key": "@SpeSpeL@",
					    "value": "Specificeer serotypeprofiel"
					  },
					  {
					    "key": "@SpeSpeM@",
					    "value": "Specimentimings"
					  },
					  {
					    "key": "@SpeSpeN@",
					    "value": "Het specimentype moet worden ingevoerd"
					  },
					  {
					    "key": "@SpeSpeO@",
					    "value": "De monsterplaats moet worden ingevoerd",
					  },
					  {
					    "key": "@SpeSpeP@",
					    "value": "Specimen Archief"
					  },
					  {
					    "key": "@SpeSpeQ@",
					    "value": "Specimenstatus"
					  },
					  {
					    "key": "@SpeSta@",
					    "value": "Samenvatting van de specimenstatus"
					  },
					  {
					    "key": "@SpeStaA@",
					    "value": "Specimenstaten"
					  },
					  {
					    "key": "@SpeStaB@",
					    "value": "Selecteer status of type categorie om de gefilterde specimenlijst te bekijken (afgerond exemplaren zijn beschikbaar onder de menu -optie` archive`)."
					  },
					  {
					    "key": "@SpeSub@",
					    "value": "Specimen indienen voor goedkeuring"
					  },
					  {
					    "key": "@SpeSubA@",
					    "value": "Bevestiging indienen"
					  },
					  {
					    "key": "@SpeSubDat@",
					    "value": "Ingediende datum"
					  },
					  {
					    "key": "@SpeTod@",
					    "value": "Exemplaren van vandaag"
					  },
					  {
					    "key": "@SpeTyp@",
					    "value": "Specimentypen"
					  },
					  {
					    "key": "@SpeVal@",
					    "value": "De ontvangen datum kan niet eerder zijn dan de incassodatum"
					  },
					  {
					    "key": "@SpeValA@",
					    "value": "Een laboratorium moet worden geselecteerd"
					  },
					  {
					    "key": "@SpeValB@",
					    "value": "Locatie van specimenverzameling moet worden opgegeven"
					  },
					  {
					    "key": "@SpeValC@",
					    "value": "Een monster moet worden geassocieerd met een klantorganisatie, maar er is geen gedefinieerd"
					  },
					  {
					    "key": "@SpeVie@",
					    "value": "Bekijk Isolate"
					  },
					  {
					    "key": "@SpeYou@",
					    "value": "U staat op het punt een exemplaarrecord voor goedkeuring in te dienen. Bevestig"
					  },
					  {
					    "key": "@StaAut@",
					    "value": "authenticeren"
					  },
					  {
					    "key": "@StaEst@",
					    "value": "vestigen"
					  },
					  {
					    "key": "@SupAdd@",
					    "value": "Leverancier toevoegen"
					  },
					  {
					    "key": "@SupAddA@",
					    "value": "Leverancier toevoegen"
					  },
					  {
					    "key": "@SupAddB@",
					    "value": "Opslag toevoegen"
					  },
					  {
					    "key": "@SupAddC@",
					    "value": "Opslag toevoegen"
					  },
					  {
					    "key": "@SupCre@",
					    "value": "nieuwe en bestaande leveranciers maken en beheren"
					  },
					  {
					    "key": "@SupCreA@",
					    "value": "nieuwe en bestaande opslaglocaties maken en beheren"
					  },
					  {
					    "key": "@SupDel@",
					    "value": "Leverancier verwijderen"
					  },
					  {
					    "key": "@SupDelA@",
					    "value": "Leverancier verwijderen"
					  },
					  {
					    "key": "@SupDelB@",
					    "value": "Verwijder een bestaande leverancier"
					  },
					  {
					    "key": "@SupDelC@",
					    "value": "Verwijderen opslag"
					  },
					  {
					    "key": "@SupDelD@",
					    "value": "Verwijderen opslag"
					  },
					  {
					    "key": "@SupDelE@",
					    "value": "Een bestaande opslaglocatie verwijderen"
					  },
					  {
					    "key": "@SupEdi@",
					    "value": "Leverancier bewerken"
					  },
					  {
					    "key": "@SupEdiA@",
					    "value": "Leverancier bewerken"
					  },
					  {
					    "key": "@SupEdiB@",
					    "value": "Bestaande leveranciersdetails bewerken"
					  },
					  {
					    "key": "@SupEdiC@",
					    "value": "Opslag bewerken"
					  },
					  {
					    "key": "@SupEdiD@",
					    "value": "Bewerken opslag"
					  },
					  {
					    "key": "@SupEdiE@",
					    "value": "Een bestaande opslaglocatie bewerken"
					  },
					  {
					    "key": "@SupEnt@",
					    "value": "Voer nieuwe leveranciersgegevens in"
					  },
					  {
					    "key": "@SupEntA@",
					    "value": "Voer een nieuwe opslaglocatie in"
					  },
					  {
					    "key": "@SupMan@",
					    "value": "Leveranciers beheren"
					  },
					  {
					    "key": "@SupManA@",
					    "value": "Opslaglocaties beheren"
					  },
					  {
					    "key": "@SupPar@",
					    "value": "Ouderopslag"
					  },
					  {
					    "key": "@SupSto@",
					    "value": "opslagnaam"
					  },
					  {
					    "key": "@SupStoA@",
					    "value": "opslagtype"
					  },
					  {
					    "key": "@SupSup@",
					    "value": "leveranciersnaam"
					  },
					  {
					    "key": "@SupThi@",
					    "value": "Deze opslaglocatie bevat locaties op het gebied van kinderopslag en kan niet worden verwijderd"
					  },
					  {
					    "key": "@SupYou@",
					    "value": "U moet een leveranciersnaam invoeren"
					  },
					  {
					    "key": "@SupYouA@",
					    "value": "U moet een leverancierstatus selecteren"
					  },
					  {
					    "key": "@TabA@",
					    "value": "Een lijst -ID moet worden verstrekt"
					  },
					  {
					    "key": "@TabAA@",
					    "value": "Een tabelnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@TabAB@",
					    "value": "Een tabelbeschrijving moet worden ingevoerd"
					  },
					  {
					    "key": "@TabAC@",
					    "value": "Er moet een tafel worden geselecteerd"
					  },
					  {
					    "key": "@TabAdd@",
					    "value": "Tabelinvoer toevoegen"
					  },
					  {
					    "key": "@TabAddA@",
					    "value": "Voeg een nieuwe tabelinvoer toe aan de momenteel geselecteerde tabel"
					  },
					  {
					    "key": "@TabAddB@",
					    "value": "Tabelinvoer toevoegen"
					  },
					  {
					    "key": "@TabAddC@",
					    "value": "Tabel toevoegen"
					  },
					  {
					    "key": "@TabAddD@",
					    "value": "Tabel toevoegen"
					  },
					  {
					    "key": "@TabAddE@",
					    "value": "Een nieuwe tabel toevoegen"
					  },
					  {
					    "key": "@TabCha@",
					    "value": "Wijzig de volgorde van de vermeldingen in een tabel"
					  },
					  {
					    "key": "@TabDel@",
					    "value": "Tabelinvoer verwijderen"
					  },
					  {
					    "key": "@TabDelA@",
					    "value": "Verwijder de geselecteerde tabelinvoer"
					  },
					  {
					    "key": "@TabDelB@",
					    "value": "Delete Tabel Entry"
					  },
					  {
					    "key": "@TabDelC@",
					    "value": "Tabel verwijderen"
					  },
					  {
					    "key": "@TabDelD@",
					    "value": "Tabel verwijderen"
					  },
					  {
					    "key": "@TabDelE@",
					    "value": "Een bestaande tabel verwijderen en alle inhoud"
					  },
					  {
					    "key": "@TabEdi@",
					    "value": "Tabelinvoer bewerken"
					  },
					  {
					    "key": "@TabEdiA@",
					    "value": "Tabelinvoer bewerken"
					  },
					  {
					    "key": "@TabEdiB@",
					    "value": "Bewerk de huidige tabelinvoer"
					  },
					  {
					    "key": "@TabEdiC@",
					    "value": "Bewerk tabel"
					  },
					  {
					    "key": "@TabEdiD@",
					    "value": "Bewerk tabel"
					  },
					  {
					    "key": "@TabEdiE@",
					    "value": "Bewerk een tabelbeschrijving"
					  },
					  {
					    "key": "@TabEnt@",
					    "value": "Voer de ouderlijst in"
					  },
					  {
					    "key": "@TabEntA@",
					    "value": "Voer de beschrijving in"
					  },
					  {
					    "key": "@TabEntB@",
					    "value": "Voer de naam in"
					  },
					  {
					    "key": "@TabMan@",
					    "value": "Tabellen onderhouden"
					  },
					  {
					    "key": "@TabManA@",
					    "value": "Houd de inhoud van elke referentietabel in"
					  },
					  {
					    "key": "@TabOrd@",
					    "value": "Bestellingstabelvermeldingen"
					  },
					  {
					    "key": "@TabOrdA@",
					    "value": "Besteltabel -vermeldingen"
					  },
					  {
					    "key": "@TabTab@",
					    "value": "Tabellen onderhouden"
					  },
					  {
					    "key": "@TabTabA@",
					    "value": "Tabelnaam"
					  },
					  {
					    "key": "@TabTabB@",
					    "value": "Tabelitems"
					  },
					  {
					    "key": "@TabThe@",
					    "value": "De tabel mag geen items bevatten"
					  },
					  {
					    "key": "@TabThi@",
					    "value": "Deze tabel bestaat al"
					  },
					  {
					    "key": "@TabThiA@",
					    "value": "deze tabelinvoer bestaat al"
					  },
					  {
					    "key": "@TesAdd@",
					    "value": "Testpatroon toevoegen"
					  },
					  {
					    "key": "@TesAddA@",
					    "value": "Een nieuw testpatroon toevoegen"
					  },
					  {
					    "key": "@TesAddB@",
					    "value": "Algemene instellingen"
					  },
					  {
					    "key": "@TesAddC@",
					    "value": "Geef het testpatroon een naam en stel de toepasbaarheid ervan in"
					  },
					  {
					    "key": "@TesAddD@",
					    "value": "Antibioticum toevoegen"
					  },
					  {
					    "key": "@TesAddE@",
					    "value": "Antibiotica toevoegen"
					  },
					  {
					    "key": "@TesAddF@",
					    "value": "specificeer de antibioticum-, doserings- en testmethode voor elke testpatrooncomponent"
					  },
					  {
					    "key": "@TesAddG@",
					    "value": "Nieuwe tests toevoegen voor specimen"
					  },
					  {
					    "key": "@TesAddH@",
					    "value": "Voeg nieuwe tests toe voor cultuur"
					  },
					  {
					    "key": "@TesAddI@",
					    "value": "geen testpatrooninhoud"
					  },
					  {
					    "key": "@TesAddJ@",
					    "value": "Richtlijnen moeten worden ingevoerd"
					  },
					  {
					    "key": "@TesAddK@",
					    "value": "Testpatroon toevoegen"
					  },
					  {
					    "key": "@TesAfb@",
					    "value": "AFB -hoeveelheid"
					  },
					  {
					    "key": "@TesApi@",
					    "value": "API-paneltest"
					  },
					  {
					    "key": "@TesApiA@",
					    "value": "API Id-paneel moet worden ingevoerd"
					  },
					  {
					    "key": "@TesApiB@",
					    "value": "API-paneel"
					  },
					  {
					    "key": "@TesAur@",
					    "value": "TB - Auramine"
					  },
					  {
					    "key": "@TesAurRes@",
					    "value": "TB - Auramine resultaat"
					  },
					  {
					    "key": "@TesBet@",
					    "value": "Betalactamase-test"
					  },
					  {
					    "key": "@TesBetA@",
					    "value": "Betalactamase"
					  },
					  {
					    "key": "@TesBetRes@",
					    "value": "Betalactamase resultaat"
					  },
					  {
					    "key": "@TesBio@",
					    "value": "Biochemistry Test"
					  },
					  {
					    "key": "@TesBioA@",
					    "value": "Biochemistry"
					  },
					  {
					    "key": "@TesCar@",
					    "value": "Directe tests uitvoeren"
					  },
					  {
					    "key": "@TesCarA@",
					    "value": "Voer isolaattests uit"
					  },
					  {
					    "key": "@TesCarB@",
					    "value": "Carbapenemase Test"
					  },
					  {
					    "key": "@TesCarC@",
					    "value": "Carbapenemase"
					  },
					  {
					    "key": "@TesCarRes@",
					    "value": "Carbapenemase resultaat"
					  },
					  {
					    "key": "@TesCat@",
					    "value": "Catalase Test"
					  },
					  {
					    "key": "@TesCatA@",
					    "value": "Catalase"
					  },
					  {
					    "key": "@TesCatRes@",
					    "value": "Catalase resultaat"
					  },
					  {
					    "key": "@TesCel@",
					    "value": "Cell Count Test"
					  },
					  {
					    "key": "@TesDel@",
					    "value": "Verwijderen Specimen -test"
					  },
					  {
					    "key": "@TesDelA@",
					    "value": "Testpatroon verwijderen"
					  },
					  {
					    "key": "@TesDelB@",
					    "value": "Een testpatroon verwijderen"
					  },
					  {
					    "key": "@TesDelC@",
					    "value": "Isolaattest verwijderen"
					  },
					  {
					    "key": "@TesDelD@",
					    "value": "Testpatroon verwijderen"
					  },
					  {
					    "key": "@TesDelE@",
					    "value": "Een bestaande test verwijderen"
					  },
					  {
					    "key": "@TesDip@",
					    "value": "Urine Dipstick"
					  },
					  {
					    "key": "@TesDir@",
					    "value": "Direct Test Management"
					  },
					  {
					    "key": "@TesEdi@",
					    "value": "Antibiotics bewerken"
					  },
					  {
					    "key": "@TesEdiA@",
					    "value": "Antibiotische componenten van het testpatroon bewerken"
					  },
					  {
					    "key": "@TesEdiB@",
					    "value": "Algemene instellingen bewerken"
					  },
					  {
					    "key": "@TesEdiC@",
					    "value": "Algemene kenmerken bewerken van het testpatroon"
					  },
					  {
					    "key": "@TesEdiD@",
					    "value": "Testpatroon bewerken"
					  },
					  {
					    "key": "@TesEdiE@",
					    "value": "Testpatroon bewerken"
					  },
					  {
					    "key": "@TesEnt@",
					    "value": "Voer de details in voor een celtest"
					  },
					  {
					    "key": "@TesEntA@",
					    "value": "Voer directe tests in"
					  },
					  {
					    "key": "@TesEntB@",
					    "value": "Voer directe tests in voor dit exemplaar"
					  },
					  {
					    "key": "@TesEntC@",
					    "value": "Voer de details in voor een gramvlektest"
					  },
					  {
					    "key": "@TesEntD@",
					    "value": "Voer de details in voor een India Ink-test"
					  },
					  {
					    "key": "@TesEntE@",
					    "value": "Voer de details in voor een natte voorbereidingstest"
					  },
					  {
					    "key": "@TesEntF@",
					    "value": "Voer de details in voor een Zn -vlektest"
					  },
					  {
					    "key": "@TesEntG@",
					    "value": "Voer de details in voor een zwangerschapstest"
					  },
					  {
					    "key": "@TesEntH@",
					    "value": "Voer TB -achtige organismen in gezien"
					  },
					  {
					    "key": "@TesEntI@",
					    "value": "Voer de details in voor een H. pylori antigeen -test"
					  },
					  {
					    "key": "@TesEntJ@",
					    "value": "Voer de details in voor een Jev -serologietest"
					  },
					  {
					    "key": "@TesEntK@",
					    "value": "Voer de details in voor een Wrights -vlektest"
					  },
					  {
					    "key": "@TesEntL@",
					    "value": "Voer de details in voor een schimmel natte prep -test"
					  },
					  {
					    "key": "@TesEntM@",
					    "value": "Voer de details in voor een biochemistratietest"
					  },
					  {
					    "key": "@TesEntN@",
					    "value": "Voer de details in voor een microscopietest"
					  },
					  {
					    "key": "@TesEntO@",
					    "value": "Voer de details in voor een duikstick -test"
					  },
					  {
					    "key": "@TesEntP@",
					    "value": "Voer isolaattests in"
					  },
					  {
					    "key": "@TesEntQ@",
					    "value": "Voer tests in voor een isolaat"
					  },
					  {
					    "key": "@TesEntR@",
					    "value": "Voer de details in voor een ESBL -test"
					  },
					  {
					    "key": "@TesEntS@",
					    "value": "Voer de details in voor een betalactamase -test"
					  },
					  {
					    "key": "@TesEntT@",
					    "value": "Voer de details in voor een carbapenemase -test"
					  },
					  {
					    "key": "@TesEntU@",
					    "value": "Voer de details in voor een API -paneeltest"
					  },
					  {
					    "key": "@TesEntV@",
					    "value": "Voer de details in voor een oxidasetest"
					  },
					  {
					    "key": "@TesEntW@",
					    "value": "Voer de details in voor een catalasetest"
					  },
					  {
					    "key": "@TesEpi@",
					    "value": "EPI Cellen"
					  },
					  {
					    "key": "@TesEpiA@",
					    "value": "Epithelium"
					  },
					  {
					    "key": "@TesEpiB@",
					    "value": "EPI -celtelling moet worden ingevoerd"
					  },
					  {
					    "key": "@TesEsb@",
					    "value": "ESBL TEST"
					  },
					  {
					    "key": "@TesEsbA@",
					    "value": "ESBL"
					  },
					  {
					    "key": "@TesEsbRes@",
					    "value": "ESBL resultaat"
					  },
					  {
					    "key": "@TesFun@",
					    "value": "Fungal Wet Prep Test"
					  },
					  {
					    "key": "@TesFunA@",
					    "value": "Fungal Wet P."
					  },
					  {
					    "key": "@TesFunB@",
					    "value": "Fungus moet worden ingevoerd"
					  },
					  {
					    "key": "@TesFunC@",
					    "value": "Type schimmelelement"
					  },
					  {
					    "key": "@TesFunPos@",
					    "value": "KOH schimmelresultaat"
					  },
					  {
					    "key": "@TesFunRes@",
					    "value": "KOH prep resultaat"
					  },
					  {
					    "key": "@TesGlu@",
					    "value": "Glucose (mm/l)"
					  },
					  {
					    "key": "@TesGluA@",
					    "value": "glucose"
					  },
					  {
					    "key": "@TesGluB@",
					    "value": "Glucose moet worden ingevoerd"
					  },
					  {
					    "key": "@TesGra@",
					    "value": "Gram Stain Test"
					  },
					  {
					    "key": "@TesGraA@",
					    "value": "Gram Stain Culture Test"
					  },
					  {
					    "key": "@TesGraWbc@",
					    "value": "Gram stain WBC"
					  },
					  {
					    "key": "@TesGui@",
					    "value": "Richtlijnen"
					  },
					  {
					    "key": "@TesHpy@",
					    "value": "H. Pylori Antigeen Test"
					  },
					  {
					    "key": "@TesHpyA@",
					    "value": "H. Pylori Ant."
					  },
					  {
					    "key": "@TesHpyB@",
					    "value": "H. Pylori Antigen"
					  },
					  {
					    "key": "@TesHpyRes@",
					    "value": "H. pylori antigeen resultaat"
					  },
					  {
					    "key": "@TesIdp@",
					    "value": "ID -profiel moet worden ingevoerd"
					  },
					  {
					    "key": "@TesInd@",
					    "value": "India Ink Test"
					  },
					  {
					    "key": "@TesIndA@",
					    "value": "India Ink Resultaat"
					  },
					  {
					    "key": "@TesIndPos@",
					    "value": "India Ink positief resultaat"
					  },
					  {
					    "key": "@TesJev@",
					    "value": "Jev Serology Test"
					  },
					  {
					    "key": "@TesJevA@",
					    "value": "Jev Serology"
					  },
					  {
					    "key": "@TesJevRes@",
					    "value": "JEV serologie resultaat"
					  },
					  {
					    "key": "@TesKet@",
					    "value": "Ketones"
					  },
					  {
					    "key": "@TesLeu@",
					    "value": "Leucocyten"
					  },
					  {
					    "key": "@TesMan@",
					    "value": "Selecteer directe tests"
					  },
					  {
					    "key": "@TesManA@",
					    "value": "Testpatronen beheren"
					  },
					  {
					    "key": "@TesManB@",
					    "value": "Lijst met testpatronen beheren"
					  },
					  {
					    "key": "@TesManC@",
					    "value": "Isoleertests beheren"
					  },
					  {
					    "key": "@TesManD@",
					    "value": "Isoleertests selecteren"
					  },
					  {
					    "key": "@TesMic@",
					    "value": "Directe microscopie"
					  },
					  {
					    "key": "@TesMicA@",
					    "value": "Microscopie"
					  },
					  {
					    "key": "@TesMon@",
					    "value": "Mononucleair (%)"
					  },
					  {
					    "key": "@TesNit@",
					    "value": "Nitrieten"
					  },
					  {
					    "key": "@TesOxi@",
					    "value": "Oxidase test"
					  },
					  {
					    "key": "@TesOxiA@",
					    "value": "Oxidase"
					  },
					  {
					    "key": "@TesOxiRes@",
					    "value": "Oxidase resultaat"
					  },
					  {
					    "key": "@TesPar@",
					    "value": "Parasieten"
					  },
					  {
					    "key": "@TesParA@",
					    "value": "Parasite"
					  },
					  {
					    "key": "@TesPat@",
					    "value": "Testpatroonnaam"
					  },
					  {
					    "key": "@TesPer@",
					    "value": "Percentage -ID moet worden ingevoerd"
					  },
					  {
					    "key": "@TesPh@",
					    "value": "pH"
					  },
					  {
					    "key": "@TesPol@",
					    "value": "polymorfonucleair (%)"
					  },
					  {
					    "key": "@TesPos@",
					    "value": "Positief resultaat"
					  },
					  {
					    "key": "@TesPre@",
					    "value": "Zwangerschapstest"
					  },
					  {
					    "key": "@TesPreA@",
					    "value": "Zwangerschap"
					  },
					  {
					    "key": "@TesPreRes@",
					    "value": "Zwangerschap resultaat"
					  },
					  {
					    "key": "@TesPro@",
					    "value": "Eiwit (G/L)"
					  },
					  {
					    "key": "@TesProA@",
					    "value": "Eiwit"
					  },
					  {
					    "key": "@TesProB@",
					    "value": "Eiwit moet worden ingevoerd"
					  },
					  {
					    "key": "@TesRbc@",
					    "value": "RBC (X10^6/L)"
					  },
					  {
					    "key": "@TesRbcA@",
					    "value": "RBC Qualitative"
					  },
					  {
					    "key": "@TesRbcB@",
					    "value": "RBC"
					  },
					  {
					    "key": "@TesSpe@",
					    "value": "Specifieke Gravity"
					  },
					  {
					    "key": "@TesTes@",
					    "value": "Deze kunnen later worden toegevoegd of verwijderd"
					  },
					  {
					    "key": "@TesTesA@",
					    "value": "Testresultaat"
					  },
					  {
					    "key": "@TesTesB@",
					    "value": "Selecteer isolaattests voor het huidige isolaat"
					  },
					  {
					    "key": "@TesTesC@",
					    "value": "Testselectie"
					  },
					  {
					    "key": "@TesUpd@",
					    "value": "Update testselectie voor specimen"
					  },
					  {
					    "key": "@TesUpdA@",
					    "value": "Update testselectie voor cultuur"
					  },
					  {
					    "key": "@TesWbc@",
					    "value": "WBC (X10^6/L)"
					  },
					  {
					    "key": "@TesWbcA@",
					    "value": "WBC Qualitative"
					  },
					  {
					    "key": "@TesWbcB@",
					    "value": "WBC"
					  },
					  {
					    "key": "@TesWet@",
					    "value": "Wet Prep Test"
					  },
					  {
					    "key": "@TesWetWbc@",
					    "value": "Wet prep WBC"
					  },
					  {
					    "key": "@TesWri@",
					    "value": "Wrights Stain Test"
					  },
					  {
					    "key": "@TesWriA@",
					    "value": "Wrights Stain"
					  },
					  {
					    "key": "@TesWriRes@",
					    "value": "Wrights stain resultaat"
					  },
					  {
					    "key": "@TesZns@",
					    "value": "Zn Stain"
					  },
					  {
					    "key": "@TesZnsA@",
					    "value": "Zn Stain Test"
					  },
					  {
					    "key": "@UseAdd@",
					    "value": "Voeg een nieuwe gebruiker toe"
					  },
					  {
					    "key": "@UseAddA@",
					    "value": "Gebruiker toevoegen"
					  },
					  {
					    "key": "@UseAddB@",
					    "value": "Selecteer een laboratorium of een clientorganisatie"
					  },
					  {
					    "key": "@UseCha@",
					    "value": "Wachtwoord wijzigen"
					  },
					  {
					    "key": "@UseChaA@",
					    "value": "Wijzig het wachtwoord voor de geselecteerde gebruiker"
					  },
					  {
					    "key": "@UseChaB@",
					    "value": "Wijzig mijn voorkeuren"
					  },
					  {
					    "key": "@UseChaC@",
					    "value": "Mijn wachtwoord wijzigen"
					  },
					  {
					    "key": "@UseClo@",
					    "value": "Clone User"
					  },
					  {
					    "key": "@UseCre@",
					    "value": "nieuwe en bestaande gebruikers maken en beheren"
					  },
					  {
					    "key": "@UseDel@",
					    "value": "Gebruiker verwijderen"
					  },
					  {
					    "key": "@UseDelA@",
					    "value": "Gebruiker verwijderen"
					  },
					  {
					    "key": "@UseDelB@",
					    "value": "Een bestaande gebruiker verwijderen"
					  },
					  {
					    "key": "@UseDis@",
					    "value": "Display Data Entry Full Screen"
					  },
					  {
					    "key": "@UseEdi@",
					    "value": "Een bestaande gebruiker bewerken"
					  },
					  {
					    "key": "@UseEdiA@",
					    "value": "Gebruiker bewerken"
					  },
					  {
					    "key": "@UseEit@",
					    "value": "Een laboratorium- of clientorganisatie moet worden ingevoerd"
					  },
					  {
					    "key": "@UseEma@",
					    "value": "E -mail"
					  },
					  {
					    "key": "@UseFir@",
					    "value": "voornaam"
					  },
					  {
					    "key": "@UseLas@",
					    "value": "Achternaam"
					  },
					  {
					    "key": "@UseMan@",
					    "value": "Gebruikers beheren"
					  },
					  {
					    "key": "@UseMy@",
					    "value": "Mijn wachtwoord"
					  },
					  {
					    "key": "@UseMyA@",
					    "value": "Mijn voorkeuren"
					  },
					  {
					    "key": "@UseNew@",
					    "value": "Nieuw wachtwoord"
					  },
					  {
					    "key": "@UsePas@",
					    "value": "Wachtwoord moet worden ingevoerd"
					  },
					  {
					    "key": "@UsePasA@",
					    "value": "wachtwoord"
					  },
					  {
					    "key": "@UsePasB@",
					    "value": "wachtwoord moet minimaal 10 tekens zijn"
					  },
					  {
					    "key": "@UsePre@",
					    "value": "Voorkeuren"
					  },
					  {
					    "key": "@UseRol@",
					    "value": "Rollen moeten worden ingevoerd"
					  },
					  {
					    "key": "@UseSel@",
					    "value": "Select Laboratory"
					  },
					  {
					    "key": "@UseSelA@",
					    "value": "Select Client Organisation"
					  },
					  {
					    "key": "@UseThi@",
					    "value": "Deze gebruiker heeft het systeem bijgewerkt en kan niet worden verwijderd"
					  },
					  {
					    "key": "@UseUse@",
					    "value": "Gebruikersnaam moet worden ingevoerd"
					  },
					  {
					    "key": "@UseUseA@",
					    "value": "Gebruiker kan niet behoren tot zowel een clientorganisatie als een laboratorium"
					  },
					  {
					    "key": "@UseUseB@",
					    "value": "Gebruikersnaam"
					  },
					  {
					    "key": "@UseUseC@",
					    "value": "Gebruikerseigenschappen"
					  },
					  {
					    "key": "@UseUseD@",
					    "value": "moet gebruikersidentificatie worden verstrekt"
					  },
					  {
					    "key": "@ValAtl@",
					    "value": "At Line"
					  },
					  {
					    "key": "@ValMes@",
					    "value": "moet worden ingevoerd"
					  },
					  {
					    "key": "@ValOr@",
					    "value": "ten minste één veld"
					  },
					  {
					    "key": "@ValOrA@",
					    "value": "U moet een standaard of gratis tekstcommentaar invoeren"
					  },
					  {
					    "key": "@Ver@",
					    "value": "Versie"
					  },
					  {
					    "key": "@VieMan@",
					    "value": "Bekijkdefinities beheren"
					  },
					  {
					    "key": "@VieTxt@",
					    "value": "nieuwe en bestaande weergavefinities maken en beheren"
					  },
					  {
					    "key": "@WhoQualVal@",
					    "value": "Gebruik kwalitatieve waarden"
					  },
					  {
					    "key": "@ZonDia@",
					    "value": "Zone Diameter"
					  }
					]
					""";
    }
}
