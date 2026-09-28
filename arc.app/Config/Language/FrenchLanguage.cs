using arc.app.Common;

namespace arc.app.Config.Language
{
    internal class FrenchLanguage : IDefinition
    {
        public string Get()
        {
            return """
				[
					{
						"key": "@AleAddChildTag@",
						"value": "Ajouter une étiquette enfant"
					},
					{
						"key": "@AleTagHasChildren@",
						"value": "Impossible de supprimer une étiquette qui a des étiquettes enfants"
					},
					{
						"key": "@GenTagH@",
						"value": "Étiquette parente"
					},
					{
						"key": "@GenTagI@",
						"value": "Sélectionner l'étiquette parente"
					},
					{
						"key": "@AstAdd@",
						"value": "Ajouter ou mettre à jour des données AST pour une culture"
					},
					{
						"key": "@AstAnt@",
						"value": "Tests de sensibilité aux antimicrobiens"
					},
					{
						"key": "@AstCre@",
						"value": "Créer et gérer les résultats AST"
					},
					{
						"key": "@AstDis@",
						"value": "Tests de disque"
					},
					{
						"key": "@AstEnt@",
						"value": "Entrez les antibiotiques à tester, par type de méthode AST, et les résultats des tests si disponibles"
					},
					{
						"key": "@AstMan@",
						"value": "Gérer l'AST"
					},
					{
						"key": "@AstMic@",
						"value": "Tests MIC"
					},
					{
						"key": "@AstPen@",
						"value": "AST en attente"
					},
					{
						"key": "@AstPre@",
						"value": "Sélectionner la présence"
					},
					{
						"key": "@AstRes@",
						"value": "Résultats de l'AST"
					},
					{
						"key": "@AstTes@",
						"value": "Tests AST"
					},
					{
						"key": "@AstTesA@",
						"value": "Modèle de test AST"
					},
					{
						"key": "@AstTesB@",
						"value": "Modèle de test"
					},
					{
						"key": "@AstTesC@",
						"value": "Sélectionnez le motif de test"
					},
					{
						"key": "@AstUse@",
						"value": "Utiliser le modèle pour les tests de disque"
					},
					{
						"key": "@AstUseA@",
						"value": "Utiliser le modèle pour les tests sur bandelettes"
					},
					{
						"key": "@BreA@",
						"value": "Un type d'échantillon doit être saisi"
					},
					{
						"key": "@BreAA@",
						"value": "Un hôte doit être saisi"
					},
					{
						"key": "@BreAB@",
						"value": "Une méthode de test doit être saisie"
					},
					{
						"key": "@BreAC@",
						"value": "Une susceptibilité doit être saisie"
					},
					{
						"key": "@BreAD@",
						"value": "Un nom de motif de test doit être saisi"
					},
					{
						"key": "@BreAE@",
						"value": "Une posologie doit être saisie"
					},
					{
						"key": "@BreAdd@",
						"value": "Ajouter un point d'arrêt"
					},
					{
						"key": "@BreAddA@",
						"value": "Ajouter un nouveau point d'arrêt"
					},
					{
						"key": "@BreAddB@",
						"value": "Ajouter une méthode de test"
					},
					{
						"key": "@BreAddC@",
						"value": "Ajouter un hôte"
					},
					{
						"key": "@BreAddD@",
						"value": "Ajouter une nouvelle méthode de test"
					},
					{
						"key": "@BreAddE@",
						"value": "Ajouter un nouvel hôte"
					},
					{
						"key": "@BreAddF@",
						"value": "Ajouter de la susceptibilité"
					},
					{
						"key": "@BreAddG@",
						"value": "Ajouter une nouvelle susceptibilité"
					},
					{
						"key": "@BreAddH@",
						"value": "Définir des critères"
					},
					{
						"key": "@BreAddI@",
						"value": "Définir les autres caractéristiques pour lesquelles le point d'arrêt s'appliquera"
					},
					{
						"key": "@BreAddJ@",
						"value": "Définir le point d'arrêt"
					},
					{
						"key": "@BreAddK@",
						"value": "Définir la plage de mesure pour chaque détermination de susceptibilité"
					},
					{
						"key": "@BreAddL@",
						"value": "Modifier le point d'arrêt"
					},
					{
						"key": "@BreAddM@",
						"value": "Modifier les critères"
					},
					{
						"key": "@BreAddN@",
						"value": "Modifier les autres caractéristiques pour lesquelles le point d'arrêt s'appliquera"
					},
					{
						"key": "@BreAn@",
						"value": "Un antibiotique doit être saisi"
					},
					{
						"key": "@BreAnA@",
						"value": "Une commande doit être saisie"
					},
					{
						"key": "@BreBre@",
						"value": "Points d'arrêt"
					},
					{
						"key": "@BreCan@",
						"value": "Impossible de supprimer car cet hôte est en cours d'utilisation"
					},
					{
						"key": "@BreCanA@",
						"value": "Impossible de supprimer car cette méthode de test est en cours d'utilisation"
					},
					{
						"key": "@BreCanB@",
						"value": "Impossible de supprimer car cette sensibilité est en cours d'utilisation"
					},
					{
						"key": "@BreDel@",
						"value": "Supprimer le point d'arrêt"
					},
					{
						"key": "@BreDelA@",
						"value": "Supprimer un point d'arrêt"
					},
					{
						"key": "@BreDelB@",
						"value": "Supprimer la méthode de test"
					},
					{
						"key": "@BreDelC@",
						"value": "Supprimer l'hôte"
					},
					{
						"key": "@BreDelD@",
						"value": "Supprimer une méthode de test existante"
					},
					{
						"key": "@BreDelE@",
						"value": "Supprimer la susceptibilité"
					},
					{
						"key": "@BreDelF@",
						"value": "Supprimer une susceptibilité existante"
					},
					{
						"key": "@BreEdi@",
						"value": "Modifier le point d'arrêt"
					},
					{
						"key": "@BreEdiA@",
						"value": "Modifier un point d'arrêt existant"
					},
					{
						"key": "@BreEdiB@",
						"value": "Modifier la méthode de test"
					},
					{
						"key": "@BreEdiC@",
						"value": "Modifier l'hôte"
					},
					{
						"key": "@BreEdiD@",
						"value": "Modifier une méthode de test existante"
					},
					{
						"key": "@BreEdiE@",
						"value": "Modifier un hôte existant"
					},
					{
						"key": "@BreEdiF@",
						"value": "Modifier la susceptibilité"
					},
					{
						"key": "@BreEdiG@",
						"value": "Modifier une susceptibilité existante"
					},
					{
						"key": "@BreHos@",
						"value": "Liste des hôtes"
					},
					{
						"key": "@BreMan@",
						"value": "Gérer les points d'arrêt"
					},
					{
						"key": "@BreManA@",
						"value": "Gérer la liste des points d'arrêt"
					},
					{
						"key": "@BreTes@",
						"value": "Méthode d'essai"
					},
					{
						"key": "@BreTesA@",
						"value": "Méthodes d'essai"
					},
					{
						"key": "@BreThe@",
						"value": "L'hôte que vous souhaitez changer doit être sélectionné"
					},
					{
						"key": "@BreTheA@",
						"value": "La méthode de test que vous souhaitez modifier doit être sélectionnée"
					},
					{
						"key": "@BreTheB@",
						"value": "La susceptibilité que vous souhaitez modifier doit être sélectionnée"
					},
					{
						"key": "@BreThi@",
						"value": "Cette méthode de test existe déjà"
					},
					{
						"key": "@BreThiA@",
						"value": "Cet hôte existe déjà"
					},
					{
						"key": "@BreThiB@",
						"value": "Cette susceptibilité existe déjà"
					},
					{
						"key": "@BreLin@",
						"value": "Lignes de point de rupture"
					},
					{
						"key": "@BreSta@",
						"value": "Valeur de début"
					},
					{
						"key": "@BreEnd@",
						"value": "Valeur de fin"
					},
					{
						"key": "@BreAppHis@",
						"value": "Historique d'approbation"
					},
					{
						"key": "@BreAddApp@",
						"value": "Ajouter approbation/rejet"
					},
					{
						"key": "@BreAddAppB@",
						"value": "Ajouter approbation/rejet"
					},
					{
						"key": "@BreDatRec@",
						"value": "Date enregistrée"
					},
					{
						"key": "@BreRecBy@",
						"value": "Enregistré par"
					},
					{
						"key": "@BreId@",
						"value": "Id"
					},
					{
						"key": "@CodA@",
						"value": "Un nom de liste doit être saisi"
					},
					{
						"key": "@CodAA@",
						"value": "Une entrée personnalisée doit être saisie"
					},
					{
						"key": "@CodAB@",
						"value": "Une liste de codage doit être sélectionnée"
					},
					{
						"key": "@CodAC@",
						"value": "Un code doit être saisi"
					},
					{
						"key": "@CodAdd@",
						"value": "Ajouter une liste de codage"
					},
					{
						"key": "@CodAddA@",
						"value": "Ajouter une entrée personnalisée de codage"
					},
					{
						"key": "@CodAddB@",
						"value": "Ajouter une entrée personnalisée à la liste des organismes"
					},
					{
						"key": "@CodAddC@",
						"value": "Ajouter une nouvelle liste de codage"
					},
					{
						"key": "@CodAss@",
						"value": "Attribuer un code"
					},
					{
						"key": "@CodAssA@",
						"value": "Attribuer un code à l'organisme sélectionné"
					},
					{
						"key": "@CodDel@",
						"value": "Supprimer la liste de codage"
					},
					{
						"key": "@CodDelA@",
						"value": "Supprimer une liste de coing existante et tout son contenu"
					},
					{
						"key": "@CodDelB@",
						"value": "Supprimer l'organisme"
					},
					{
						"key": "@CodDelC@",
						"value": "Supprimer un organisme ou une entrée personnalisée de la liste de codage"
					},
					{
						"key": "@CodEdi@",
						"value": "Modifier l'entrée personnalisée de codage"
					},
					{
						"key": "@CodGra@",
						"value": "Gramme"
					},
					{
						"key": "@CodLis@",
						"value": "Liste des organismes correspondant aux critères de recherche"
					},
					{
						"key": "@CodNew@",
						"value": "Nouvelle entrée"
					},
					{
						"key": "@CodSco@",
						"value": "Sélectionnez la portée de l'organisme à laquelle la configuration s'appliquera"
					},
					{
						"key": "@CodScoA@",
						"value": "Modifier la portée de l'organisme à laquelle la configuration s'appliquera"
					},
					{
						"key": "@CodSel@",
						"value": "Sélectionnez l'organisme à utiliser"
					},
					{
						"key": "@CodSelA@",
						"value": "Sélectionnez le genre"
					},
					{
						"key": "@CodSelB@",
						"value": "Sélectionnez les espèces"
					},
					{
						"key": "@CodSelC@",
						"value": "Sélectionnez le sérotype"
					},
					{
						"key": "@CodThe@",
						"value": "L'organisme n'existe pas"
					},
					{
						"key": "@CodTheA@",
						"value": "L'organisme a déjà été ajouté à cette liste de codage"
					},
					{
						"key": "@CodThi@",
						"value": "Cette liste de codage existe déjà"
					},
					{
						"key": "@CodThiA@",
						"value": "Cette liste de codage ne peut pas être supprimée"
					},
					{
						"key": "@CodThiB@",
						"value": "Cette entrée existe déjà dans la liste"
					},
					{
						"key": "@ConAdd@",
						"value": "Ajouter un formulaire"
					},
					{
						"key": "@ConCre@",
						"value": "Créer et gérer des définitions de formulaire nouvelles et existantes"
					},
					{
						"key": "@ConDel@",
						"value": "Supprimer le formulaire"
					},
					{
						"key": "@ConEdi@",
						"value": "Modifier le formulaire"
					},
					{
						"key": "@ConFor@",
						"value": "Nom de forme"
					},
					{
						"key": "@ConMan@",
						"value": "Gérer les définitions de formulaire"
					},
					{
						"key": "@ExpCon@",
						"value": "Configurer et émettre une exportation DHIS2"
					},
					{
						"key": "@ExpCon@",
						"value": "Configurer et émettre des exportations de données"
					},
					{
						"key": "@ExpConA@",
						"value": "Configurer et émettre une exportation WHONET"
					},
					{
						"key": "@ExpDhi@",
						"value": "Exportation DHIS2"
					},
					{
						"key": "@ExpGen@",
						"value": "Générer et enregistrer un fichier d'export DHIS2"
					},
					{
						"key": "@ExpGenA@",
						"value": "Générer et enregistrer un fichier d'exportation WHONET"
					},
					{
						"key": "@ExpMan@",
						"value": "Gérer les exportations"
					},
					{
						"key": "@ExpMan@",
						"value": "Gérer les exportations"
					},
					{
						"key": "@ExpPro@",
						"value": "Liste des profils"
					},
					{
						"key": "@ExpProD@",
						"value": "Description"
					},
					{
						"key": "@ExpProE@",
						"value": "Activé"
					},
					{
						"key": "@ExpProMap@",
						"value": "Gérer le mappage"
					},
					{
						"key": "@ExpProMapDesc@",
						"value": "Construisez un mappage JSON ou XML pour les champs inclus dans ce profil d'exportation."
					},
					{
						"key": "@ExpProMapSav@",
						"value": "Enregistrer le mappage JSON ou XML d'un profil d'exportation"
					},
					{
						"key": "@ExpProMapFmt@",
						"value": "Format de sortie"
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
						"value": "Ajouter un attribut"
					},
					{
						"key": "@ExpProMapAddArr@",
						"value": "Ajouter un tableau"
					},
					{
						"key": "@ExpProMapAttrName@",
						"value": "Nom de l'attribut"
					},
					{
						"key": "@ExpProMapField@",
						"value": "Champ"
					},
					{
						"key": "@ExpProMapArrName@",
						"value": "Nom du tableau"
					},
					{
						"key": "@ExpProMapArrType@",
						"value": "Type de tableau"
					},
					{
						"key": "@ExpProMapArrSpecimen@",
						"value": "Échantillons"
					},
					{
						"key": "@ExpProMapArrCulture@",
						"value": "Cultures / isolats"
					},
					{
						"key": "@ExpProMapArrGrid@",
						"value": "Grille (résultats des tests)"
					},
					{
						"key": "@ExpProMapArrAst@",
						"value": "Résultats AST"
					},
					{
						"key": "@ExpProMapPreview@",
						"value": "Aperçu"
					},
					{
						"key": "@ExpProMapNoFields@",
						"value": "Le profil d'exportation ne contient encore aucun champ. Ajoutez des champs au profil avant de construire un mappage."
					},
					{
						"key": "@ExpProMapNoSpecimen@",
						"value": "Les tableaux d'échantillons nécessitent au moins un champ de patient dans le profil d'exportation."
					},
					{
						"key": "@ExpProMapNoCulture@",
						"value": "Les tableaux de cultures nécessitent au moins un champ de culture ou d'isolat dans le profil d'exportation."
					},
					{
						"key": "@ExpProMapGridReq@",
						"value": "Les tableaux de grille ne peuvent être ajoutés que lorsqu'un champ de grille est sélectionné."
					},
					{
						"key": "@ExpProMapNoAst@",
						"value": "Les tableaux AST nécessitent au moins un champ de la table AST dans le profil d'exportation."
					},
					{
						"key": "@ExpProMapAttrNameErr@",
						"value": "Le nom de l'attribut est requis."
					},
					{
						"key": "@ExpProMapFieldErr@",
						"value": "Une sélection de champ est requise."
					},
					{
						"key": "@ExpProMapUniqueRef@",
						"value": "Référence unique"
					},
					{
						"key": "@ExpProMapUniqueRefErr@",
						"value": "Un seul attribut peut être marqué comme référence unique pour les champs {table}."
					},
					{
						"key": "@ExpProMapUniqueRefNoField@",
						"value": "Sélectionnez un champ avant de marquer Référence unique."
					},
					{
						"key": "@ExpProMapSavedTitle@",
						"value": "Mappage enregistré"
					},
					{
						"key": "@ExpProMapSavedDesc@",
						"value": "Vos modifications du mappage du profil d'exportation ont été enregistrées."
					},
					{
						"key": "@ExpProMapDelNode@",
						"value": "Supprimer"
					},
					{
						"key": "@ExpProMapEditNode@",
						"value": "Modifier"
					},
					{
						"key": "@ExpProMapRoot@",
						"value": "root"
					},
					{
						"key": "@ExpProMd@",
						"value": "Date modifiée"
					},
					{
						"key": "@ExpProN@",
						"value": "Nom"
					},
					{
						"key": "@ExpWho@",
						"value": "Exportation WHONET"
					},
					{
						"key": "@ExpYou@",
						"value": "Vous devez entrer une date de début"
					},
					{
						"key": "@GenA@",
						"value": "Une valeur doit être saisie"
					},
					{
						"key": "@GenAct@",
						"value": "action"
					},
					{
						"key": "@GenAdd@",
						"value": "Notes complémentaires"
					},
					{
						"key": "@GenAddA@",
						"value": "Ajoutée"
					},
					{
						"key": "@GenAddB@",
						"value": "Ajouter un message pour un affichage bien visible"
					},
					{
						"key": "@GenAddC@",
						"value": "Ajouter un commentaire"
					},
					{
						"key": "@GenAddD@",
						"value": "Ajouter la liste"
					},
					{
						"key": "@GenAddE@",
						"value": "Ajouter une entrée"
					},
					{
						"key": "@GenAdm@",
						"value": "Administration"
					},
					{
						"key": "@GenAle@",
						"value": "Alertes"
					},
					{
						"key": "@GenAleA@",
						"value": "Alertes"
					},
					{
						"key": "@GenAnt@",
						"value": "Antibiotique"
					},
					{
						"key": "@GenAntA@",
						"value": "Antibiotiques"
					},
					{
						"key": "@GenAntB@",
						"value": "Posologie des antibiotiques"
					},
					{
						"key": "@GenBac@",
						"value": "Arrière"
					},
					{
						"key": "@GenBre@",
						"value": "Points d'arrêt"
					},
					{
						"key": "@GenCan@",
						"value": "Annuler"
					},
					{
						"key": "@GenClo@",
						"value": "proche"
					},
					{
						"key": "@GenCo1@",
						"value": "Réduire le menu"
					},
					{
						"key": "@GenCod@",
						"value": "Codage"
					},
					{
						"key": "@GenCodA@",
						"value": "Code"
					},
					{
						"key": "@GenCodB@",
						"value": "Liste de codage"
					},
					{
						"key": "@GenCom@",
						"value": "Commentaire 1"
					},
					{
						"key": "@GenComA@",
						"value": "Commentaire 2"
					},
					{
						"key": "@GenComB@",
						"value": "Commenter"
					},
					{
						"key": "@GenCon@",
						"value": "Configuration"
					},
					{
						"key": "@GenConA@",
						"value": "Numéro de contact"
					},
					{
						"key": "@GenCus@",
						"value": "Personnalisé"
					},
					{
						"key": "@GenCusA@",
						"value": "Entrées personnalisées"
					},
					{
						"key": "@GenDat@",
						"value": "date ajoutée"
					},
					{
						"key": "@GenDatA@",
						"value": "Rendez-vous complet"
					},
					{
						"key": "@GenDec@",
						"value": "Décision"
					},
					{
						"key": "@GenDef@",
						"value": "Défaut"
					},
					{
						"key": "@GenDel@",
						"value": "Supprimer le test"
					},
					{
						"key": "@GenDelA@",
						"value": "Supprimer la liste"
					},
					{
						"key": "@GenDelB@",
						"value": "Supprimer l'entrée"
					},
					{
						"key": "@GenDelC@",
						"value": "Effacer"
					},
					{
						"key": "@GenDes@",
						"value": "La description"
					},
					{
						"key": "@GenDia@",
						"value": "Diagnostic"
					},
					{
						"key": "@GenDiaA@",
						"value": "Journal intime"
					},
					{
						"key": "@GenDis@",
						"value": "Afficher dans le rapport"
					},
					{
						"key": "@GenDos@",
						"value": "Dosage"
					},
					{
						"key": "@GenEdi@",
						"value": "Modifier le test"
					},
					{
						"key": "@GenEdiA@",
						"value": "Modifier l'entrée"
					},
					{
						"key": "@GenEna@",
						"value": "Activée"
					},
					{
						"key": "@GenEnaA@",
						"value": "Permettre"
					},
					{
						"key": "@GenEnd@",
						"value": "Date de fin"
					},
					{
						"key": "@GenEnt@",
						"value": "Entrer en mode plein écran"
					},
					{
						"key": "@GenEntA@",
						"value": "Saisir un commentaire"
					},
					{
						"key": "@GenEntB@",
						"value": "Entrée"
					},
					{
						"key": "@GenEve@",
						"value": "Événement"
					},
					{
						"key": "@GenEveA@",
						"value": "Événements"
					},
					{
						"key": "@GenExi@",
						"value": "Sortir"
					},
					{
						"key": "@GenExiA@",
						"value": "Quitter le mode plein écran"
					},
					{
						"key": "@GenExp@",
						"value": "Exportations"
					},
					{
						"key": "@GenFam@",
						"value": "Famille"
					},
					{
						"key": "@GenFil@",
						"value": "Filtre"
					},
					{
						"key": "@GenFilA@",
						"value": "Filtrer les préréglages"
					},
					{
						"key": "@GenFilB@",
						"value": "Filtrer par mot-clé"
					},
					{
						"key": "@GenFin@",
						"value": "Finir"
					},
					{
						"key": "@GenFir@",
						"value": "Première approbation"
					},
					{
						"key": "@GenFirA@",
						"value": "Deuxième approbation"
					},
					{
						"key": "@GenFor@",
						"value": "Formes"
					},
					{
						"key": "@GenFou@",
						"value": "Trouvé"
					},
					{
						"key": "@GenFul@",
						"value": "Plein écran"
					},
					{
						"key": "@GenGen@",
						"value": "Genre"
					},
					{
						"key": "@GenGri@",
						"value": "Vue grille"
					},
					{
						"key": "@GenGro@",
						"value": "Regroupement"
					},
					{
						"key": "@GenHel@",
						"value": "Aider"
					},
					{
						"key": "@GenHom@",
						"value": "Accueil"
					},
					{
						"key": "@GenHos@",
						"value": "Hôte"
					},
					{
						"key": "@GenId@",
						"value": "L'identifiant doit être saisi"
					},
					{
						"key": "@GenInc@",
						"value": "Inclure dans le rapport"
					},
					{
						"key": "@GenIss@",
						"value": "Date de publication"
					},
					{
						"key": "@GenKey@",
						"value": "Clé"
					},
					{
						"key": "@GenLab@",
						"value": "Laboratoires"
					},
					{
						"key": "@GenLabA@",
						"value": "Laboratoire"
					},
					{
						"key": "@GenLan@",
						"value": "Langue"
					},
					{
						"key": "@GenLis@",
						"value": "Listes"
					},
					{
						"key": "@GenLisA@",
						"value": "Liste de noms"
					},
					{
						"key": "@GenLoc@",
						"value": "Emplacement"
					},
					{
						"key": "@GenMea@",
						"value": "La mesure"
					},
					{
						"key": "@GenMes@",
						"value": "Un message"
					},
					{
						"key": "@GenMet@",
						"value": "Méthode"
					},
					{
						"key": "@GenMon@",
						"value": "Surveillance"
					},
					{
						"key": "@GenNam@",
						"value": "Nom"
					},
					{
						"key": "@GenNex@",
						"value": "Prochain"
					},
					{
						"key": "@GenNexA@",
						"value": "Page suivante"
					},
					{
						"key": "@GenOrd@",
						"value": "Commander"
					},
					{
						"key": "@GenOrg@",
						"value": "Organisations"
					},
					{
						"key": "@GenOrgA@",
						"value": "Organisme"
					},
					{
						"key": "@GenOrgB@",
						"value": "Organismes"
					},
					{
						"key": "@GenOrgC@",
						"value": "Organisation"
					},
					{
						"key": "@GenOrgD@",
						"value": "Liste des organismes"
					},
					{
						"key": "@GenPar@",
						"value": "Parent"
					},
					{
						"key": "@GenParA@",
						"value": "Entrée des parents"
					},
					{
						"key": "@GenPat@",
						"value": "Les patients"
					},
					{
						"key": "@GenPre@",
						"value": "Précédent"
					},
					{
						"key": "@GenPri@",
						"value": "Imprimer le code-barres (type 1)"
					},
					{
						"key": "@GenPriA@",
						"value": "Imprimer le code-barres (type 2)"
					},
					{
						"key": "@GenQua@",
						"value": "Quantité"
					},
					{
						"key": "@GenRea@",
						"value": "Raison"
					},
					{
						"key": "@GenRep@",
						"value": "Rapports"
					},
					{
						"key": "@GenRes@",
						"value": "Date de résultat"
					},
					{
						"key": "@GenRol@",
						"value": "Les rôles"
					},
					{
						"key": "@GenSav@",
						"value": "sauvegarder"
					},
					{
						"key": "@GenSea@",
						"value": "Chercher"
					},
					{
						"key": "@GenSee@",
						"value": "Vu"
					},
					{
						"key": "@GenSel@",
						"value": "Sélectionnez la date de début"
					},
					{
						"key": "@GenSelA@",
						"value": "Sélectionnez la date de fin"
					},
					{
						"key": "@GenSelB@",
						"value": "Sélectionnez l'emplacement"
					},
					{
						"key": "@GenSelC@",
						"value": "Sélectionnez le quartier"
					},
					{
						"key": "@GenSelD@",
						"value": "Sélectionnez le diagnostic"
					},
					{
						"key": "@GenSelE@",
						"value": "Sélectionnez un commentaire applicable"
					},
					{
						"key": "@GenSelF@",
						"value": "Sélectionner un organisme"
					},
					{
						"key": "@GenSelG@",
						"value": "Sélectionnez le parent"
					},
					{
						"key": "@GenSelH@",
						"value": "Définir la portée de l'organisme"
					},
					{
						"key": "@GenSelI@",
						"value": "Modifier la portée de l'organisme"
					},
					{
						"key": "@GenSer@",
						"value": "Sérotype"
					},
					{
						"key": "@GenSet@",
						"value": "Paramètres"
					},
					{
						"key": "@GenSpe@",
						"value": "Spécimens"
					},
					{
						"key": "@GenSpeA@",
						"value": "Spécifier"
					},
					{
						"key": "@GenSpeB@",
						"value": "Espèce"
					},
					{
						"key": "@GenSta@",
						"value": "État"
					},
					{
						"key": "@GenStaA@",
						"value": "Statut"
					},
					{
						"key": "@GenStaB@",
						"value": "Date de début"
					},
					{
						"key": "@GenSub@",
						"value": "Soumettre"
					},
					{
						"key": "@GenSubA@",
						"value": "Sous-espèce"
					},
					{
						"key": "@GenSus@",
						"value": "Susceptibilité"
					},
					{
						"key": "@GenTab@",
						"value": "les tables"
					},
					{
						"key": "@GenTes@",
						"value": "Essais"
					},
					{
						"key": "@GenTesA@",
						"value": "Type de test"
					},
					{
						"key": "@GenTesB@",
						"value": "Modèles de test"
					},
					{
						"key": "@GenTit@",
						"value": "Titre"
					},
					{
						"key": "@GenTop@",
						"value": "Sujet"
					},
					{
						"key": "@GenTyp@",
						"value": "Taper"
					},
					{
						"key": "@GenUse@",
						"value": "Utilisateurs"
					},
					{
						"key": "@GenUseA@",
						"value": "Utilisateur"
					},
					{
						"key": "@GenVal@",
						"value": "Valeur"
					},
					{
						"key": "@GenVie@",
						"value": "Vues"
					},
					{
						"key": "@GenVieA@",
						"value": "Afficher/Mettre à jour"
					},
					{
						"key": "@GenVieB@",
						"value": "Voir l'essai"
					},
					{
						"key": "@GenVieC@",
						"value": "Vue"
					},
					{
						"key": "@GenVieD@",
						"value": "Afficher les rapports"
					},
					{
						"key": "@GenWar@",
						"value": "salle"
					},
					{
						"key": "@GenWor@",
						"value": "Flux de travail"
					},
					{
						"key": "@LabA@",
						"value": "Une traduction doit être sélectionnée"
					},
					{
						"key": "@LabAA@",
						"value": "Un codage doit être sélectionné"
					},
					{
						"key": "@LabAdd@",
						"value": "Ajouter un laboratoire"
					},
					{
						"key": "@LabBre@",
						"value": "Listes de points d'arrêt à utiliser"
					},
					{
						"key": "@LabBreA@",
						"value": "Listes de points d'arrêt"
					},
					{
						"key": "@LabCre@",
						"value": "Créer et gérer des laboratoires nouveaux et existants"
					},
					{
						"key": "@LabDel@",
						"value": "Supprimer le laboratoire"
					},
					{
						"key": "@LabEdi@",
						"value": "Modifier les détails du laboratoire"
					},
					{
						"key": "@LabEdiA@",
						"value": "Modifier le laboratoire"
					},
					{
						"key": "@LabEnt@",
						"value": "Entrez le nom du laboratoire"
					},
					{
						"key": "@LabEntA@",
						"value": "Saisir les nouveaux détails du laboratoire"
					},
					{
						"key": "@LabLab@",
						"value": "Laboratoire"
					},
					{
						"key": "@LabLabA@",
						"value": "Nom du laboratoire"
					},
					{
						"key": "@LabLabB@",
						"value": "Le nom du laboratoire doit être saisi"
					},
					{
						"key": "@LabMan@",
						"value": "Gérer les laboratoires"
					},
					{
						"key": "@LabOrg@",
						"value": "Listes d'organismes à utiliser"
					},
					{
						"key": "@LabOrgA@",
						"value": "Listes d'organismes"
					},
					{
						"key": "@LabTes@",
						"value": "Listes de motifs de test à utiliser"
					},
					{
						"key": "@LabTesA@",
						"value": "Listes de motifs de test"
					},
					{
						"key": "@LanA@",
						"value": "Une traduction à copier doit être saisie"
					},
					{
						"key": "@LanAA@",
						"value": "Une traduction à supprimer doit être sélectionnée"
					},
					{
						"key": "@LanAdd@",
						"value": "Ajouter une traduction"
					},
					{
						"key": "@LanCre@",
						"value": "Créer et gérer des traductions"
					},
					{
						"key": "@LanCreA@",
						"value": "Créer une nouvelle traduction"
					},
					{
						"key": "@LanDel@",
						"value": "Supprimer la traduction"
					},
					{
						"key": "@LanDelA@",
						"value": "Supprimer une traduction existante"
					},
					{
						"key": "@LanEdiA@",
						"value": "Modifier une entrée de traduction"
					},
					{
						"key": "@LanEnt@",
						"value": "Entrez le nom de la traduction"
					},
					{
						"key": "@LanMan@",
						"value": "Gérer les traductions"
					},
					{
						"key": "@LanSel@",
						"value": "Sélectionnez la traduction"
					},
					{
						"key": "@LanThi@",
						"value": "Cette traduction existe déjà"
					},
					{
						"key": "@LanTra@",
						"value": "Traductions"
					},
					{
						"key": "@LanTraA@",
						"value": "Traduction"
					},
					{
						"key": "@LanTraB@",
						"value": "Traduction à copier"
					},
					{
						"key": "@LanTraC@",
						"value": "Le nom de la traduction doit être saisi"
					},
					{
						"key": "@LanTraD@",
						"value": "La traduction est utilisée au sein d'une organisation"
					},
					{
						"key": "@LanTraE@",
						"value": "La traduction est utilisée dans un laboratoire"
					},
					{
						"key": "@MonMon@",
						"value": "Surveiller tous les événements"
					},
					{
						"key": "@MonMonA@",
						"value": "Surveiller tous les événements qui modifient les données sur le système"
					},
					{
						"key": "@MonVie@",
						"value": "Afficher les détails de l'événement de surveillance"
					},
					{
						"key": "@MonVieA@",
						"value": "Voir les détails"
					},
					{
						"key": "@MonVieB@",
						"value": "Afficher les détails (bruts)"
					},
					{
						"key": "@MonVieC@",
						"value": "Afficher les détails de l'événement"
					},
					{
						"key": "@MonVieD@",
						"value": "Voir les détails de l'événement"
					},
					{
						"key": "@OrgA@",
						"value": "Un nom d'organisme doit être saisi"
					},
					{
						"key": "@OrgAdd@",
						"value": "Ajouter une organisation"
					},
					{
						"key": "@OrgAddA@",
						"value": "Ajouter un organisme"
					},
					{
						"key": "@OrgAll@",
						"value": "Tous les organismes"
					},
					{
						"key": "@OrgB@",
						"value": "Un identifiant d'organisme doit être saisi"
					},
					{
						"key": "@OrgCre@",
						"value": "Créer et gérer des organisations nouvelles et existantes"
					},
					{
						"key": "@OrgDel@",
						"value": "Supprimer l'organisation"
					},
					{
						"key": "@OrgDelA@",
						"value": "Supprimer l'organisme"
					},
					{
						"key": "@OrgEdi@",
						"value": "Modifier l'organisation"
					},
					{
						"key": "@OrgEdiA@",
						"value": "Modifier les détails d'une organisation existante"
					},
					{
						"key": "@OrgEdiB@",
						"value": "Modifier l'organisme"
					},
					{
						"key": "@OrgEnt@",
						"value": "Saisir les nouveaux détails de l'organisation"
					},
					{
						"key": "@OrgEntA@",
						"value": "Entrez le nom de l'organisation"
					},
					{
						"key": "@OrgMan@",
						"value": "Gérer les organisations"
					},
					{
						"key": "@OrgManA@",
						"value": "Gérer les listes d'organismes"
					},
					{
						"key": "@OrgManB@",
						"value": "Gérer les listes utilisées pour les organismes"
					},
					{
						"key": "@OrgOrg@",
						"value": "Nom de l'organisme"
					},
					{
						"key": "@OrgOrgA@",
						"value": "Le nom de l'organisation doit être saisi"
					},
					{
						"key": "@OrgPar@",
						"value": "Organisation mère"
					},
					{
						"key": "@OrgSel@",
						"value": "Sélectionnez l'organisation parente"
					},
					{
						"key": "@PatA@",
						"value": "Un commentaire doit être saisi"
					},
					{
						"key": "@PatAdd@",
						"value": "Ajouter un nouveau patient"
					},
					{
						"key": "@PatAddA@",
						"value": "Ajouter un patient"
					},
					{
						"key": "@PatAddB@",
						"value": "Adresse du patient"
					},
					{
						"key": "@PatAddC@",
						"value": "Ajouter un commentaire à un patient"
					},
					{
						"key": "@PatAdm@",
						"value": "Date d'admission"
					},
					{
						"key": "@PatAge@",
						"value": "Âge"
					},
					{
						"key": "@PatCli@",
						"value": "Numéro de contact clinique"
					},
					{
						"key": "@PatCre@",
						"value": "Créer et gérer des dossiers patients nouveaux et existants"
					},
					{
						"key": "@PatDat@",
						"value": "Date de naissance"
					},
					{
						"key": "@PatDel@",
						"value": "Supprimer le patient"
					},
					{
						"key": "@PatDet@",
						"value": "Détails du patient"
					},
					{
						"key": "@PatDis@",
						"value": "Quartier"
					},
					{
						"key": "@PatEdi@",
						"value": "Modifier les détails du patient"
					},
					{
						"key": "@PatEdiA@",
						"value": "Modifier le patient"
					},
					{
						"key": "@PatEdiB@",
						"value": "Modifier les détails de l'adresse du patient"
					},
					{
						"key": "@PatEnt@",
						"value": "Entrez un commentaire de patient"
					},
					{
						"key": "@PatEntA@",
						"value": "entrez votre prénom"
					},
					{
						"key": "@PatEntB@",
						"value": "Entrez le nom de famille"
					},
					{
						"key": "@PatEntC@",
						"value": "Entrez l'âge"
					},
					{
						"key": "@PatEntD@",
						"value": "Entrez le numéro de téléphone"
					},
					{
						"key": "@PatEntE@",
						"value": "Saisir les nouveaux détails du patient"
					},
					{
						"key": "@PatEntF@",
						"value": "Entrer la référence du patient"
					},
					{
						"key": "@PatEntG@",
						"value": "Entrez le numéro de contact clinique"
					},
					{
						"key": "@PatEntH@",
						"value": "Entrer la valeur"
					},
					{
						"key": "@PatFin@",
						"value": "Rechercher un patient par mot-clé"
					},
					{
						"key": "@PatFir@",
						"value": "Prénom"
					},
					{
						"key": "@PatGen@",
						"value": "Le sexe doit être saisi"
					},
					{
						"key": "@PatGenA@",
						"value": "Genre"
					},
					{
						"key": "@PatMan@",
						"value": "Gérer les patients"
					},
					{
						"key": "@PatPat@",
						"value": "La référence du patient doit être saisie"
					},
					{
						"key": "@PatPatA@",
						"value": "Nom du patient"
					},
					{
						"key": "@PatPatB@",
						"value": "Réf patient"
					},
					{
						"key": "@PatPatC@",
						"value": "Dossier patient"
					},
					{
						"key": "@PatPatD@",
						"value": "Détails du patient"
					},
					{
						"key": "@PatPatE@",
						"value": "Commentaires des patients"
					},
					{
						"key": "@PatPatF@",
						"value": "Recherche de patients"
					},
					{
						"key": "@PatPatG@",
						"value": "Résultats de la recherche de patients"
					},
					{
						"key": "@PatPatH@",
						"value": "Détails de la collecte des patients"
					},
					{
						"key": "@PatPatI@",
						"value": "Patient"
					},
					{
						"key": "@PatPatJ@",
						"value": "Emplacement du patient"
					},
					{
						"key": "@PatPro@",
						"value": "Province"
					},
					{
						"key": "@PatSea@",
						"value": "Rechercher un patient"
					},
					{
						"key": "@PatSel@",
						"value": "Sélectionnez la date de naissance"
					},
					{
						"key": "@PatSelA@",
						"value": "Sélectionnez le sexe"
					},
					{
						"key": "@PatSelB@",
						"value": "Sélectionnez la province"
					},
					{
						"key": "@PatSelC@",
						"value": "Sélectionnez le quartier"
					},
					{
						"key": "@PatSelD@",
						"value": "Sélectionnez le sous-district"
					},
					{
						"key": "@PatSelE@",
						"value": "Sélectionnez un patient existant ou définissez-en un nouveau"
					},
					{
						"key": "@PatSelF@",
						"value": "Sélectionnez la date d'admission"
					},
					{
						"key": "@PatSub@",
						"value": "Sous-district"
					},
					{
						"key": "@PatSur@",
						"value": "Le nom de famille doit être saisi"
					},
					{
						"key": "@PatSurA@",
						"value": "Nom de famille"
					},
					{
						"key": "@PatTel@",
						"value": "Numéro de téléphone"
					},
					{
						"key": "@PatVie@",
						"value": "Afficher le patient"
					},
					{
						"key": "@RepA@",
						"value": "Un nom de rapport doit être saisi"
					},
					{
						"key": "@RepAnt@",
						"value": "Antibiotique"
					},
					{
						"key": "@RepApp@",
						"value": "Apparence"
					},
					{
						"key": "@RepCel@",
						"value": "Nombre de cellules"
					},
					{
						"key": "@RepCul@",
						"value": "Culture Résultat"
					},
					{
						"key": "@RepGra@",
						"value": "Coloration de Gram"
					},
					{
						"key": "@RepIfy@",
						"value": "Si vous souhaitez discuter du résultat ou du traitement, veuillez appeler le Laboratoire de microbiologie"
					},
					{
						"key": "@RepInd@",
						"value": "Encre indienne"
					},
					{
						"key": "@RepMic@",
						"value": "Rapport du laboratoire de microbiologie"
					},
					{
						"key": "@RepPre@",
						"value": "Résultats de la préculture"
					},
					{
						"key": "@RepPreA@",
						"value": "Date de préculture"
					},
					{
						"key": "@RepPri@",
						"value": "Imprimer/publier le rapport d'échantillon"
					},
					{
						"key": "@RepPriA@",
						"value": "Imprimer et publier le rapport"
					},
					{
						"key": "@RepPub@",
						"value": "Publier le rapport"
					},
					{
						"key": "@RepPatLoc@",
						"value": "Emplacement du patient"
					},
					{
						"key": "@RepRef@",
						"value": "Patient"
					},
					{
						"key": "@RepRep@",
						"value": "Historique des rapports"
					},
					{
						"key": "@RepRes@",
						"value": "Résultat"
					},
					{
						"key": "@RepSen@",
						"value": "Sensibilité"
					},
					{
						"key": "@RepSpe@",
						"value": "Rapport d'échantillon"
					},
					{
						"key": "@RepWet@",
						"value": "Préparation humide"
					},
					{
						"key": "@RepZns@",
						"value": "Tache ZN"
					},
					{
						"key": "@RolAdd@",
						"value": "Ajouter un rôle"
					},
					{
						"key": "@RolAddA@",
						"value": "Ajout d'un nouveau rôle"
					},
					{
						"key": "@RolCan@",
						"value": "Le nom du rôle doit être unique"
					},
					{
						"key": "@RolClo@",
						"value": "Cloner un rôle"
					},
					{
						"key": "@RolCloA@",
						"value": "Rôle de clone"
					},
					{
						"key": "@RolCloB@",
						"value": "Clonez un rôle existant sur le système. Cela copiera les détails de l'autorisation du rôle existant vers le nouveau rôle"
					},
					{
						"key": "@RolCon@",
						"value": "Configurer les aspects généraux du rôle"
					},
					{
						"key": "@RolConA@",
						"value": "La configuration doit être saisie"
					},
					{
						"key": "@RolConB@",
						"value": "Configurer les autorisations d'événement pour ce rôle"
					},
					{
						"key": "@RolConC@",
						"value": "Configurer les autorisations de menu pour ce rôle"
					},
					{
						"key": "@RolCre@",
						"value": "Créer et gérer des rôles nouveaux et existants"
					},
					{
						"key": "@RolDel@",
						"value": "Supprimer un rôle"
					},
					{
						"key": "@RolDelA@",
						"value": "Supprimer le rôle"
					},
					{
						"key": "@RolDes@",
						"value": "La description du rôle doit être saisie"
					},
					{
						"key": "@RolEdi@",
						"value": "Modifier le rôle existant"
					},
					{
						"key": "@RolEdiA@",
						"value": "Modifier le rôle"
					},
					{
						"key": "@RolEdiB@",
						"value": "Modifier les aspects généraux d'un rôle existant"
					},
					{
						"key": "@RolEna@",
						"value": "Activé doit être saisi"
					},
					{
						"key": "@RolEve@",
						"value": "Autorisations d'événement"
					},
					{
						"key": "@RolLab@",
						"value": "Administrateur de laboratoire"
					},
					{
						"key": "@RolMan@",
						"value": "Gérer les rôles"
					},
					{
						"key": "@RolManA@",
						"value": "Gérer les autorisations d'événement"
					},
					{
						"key": "@RolManB@",
						"value": "Gérer les autorisations de menu"
					},
					{
						"key": "@RolMen@",
						"value": "Autorisations de menu"
					},
					{
						"key": "@RolNam@",
						"value": "Le nom du rôle doit être saisi"
					},
					{
						"key": "@RolNew@",
						"value": "Détails du nouveau rôle"
					},
					{
						"key": "@RolOrg@",
						"value": "Administrateur de l'organisation"
					},
					{
						"key": "@RolRem@",
						"value": "Supprimer un rôle du système"
					},
					{
						"key": "@RolRol@",
						"value": "Description du rôle"
					},
					{
						"key": "@RolRolA@",
						"value": "Nom de rôle"
					},
					{
						"key": "@RolRolB@",
						"value": "Enregistrement de rôle"
					},
					{
						"key": "@RolRolC@",
						"value": "Les rôles"
					},
					{
						"key": "@RolRolD@",
						"value": "Rôle à cloner"
					},
					{
						"key": "@RolSel@",
						"value": "Sélectionnez les rôles"
					},
					{
						"key": "@RolUpd@",
						"value": "Mettre à jour les autorisations d'événement"
					},
					{
						"key": "@RolUpdA@",
						"value": "Mettre à jour les autorisations du menu"
					},
					{
						"key": "@SerA@",
						"value": "Un nom de sérotype doit être saisi"
					},
					{
						"key": "@SerAdd@",
						"value": "Ajouter un sérotype"
					},
					{
						"key": "@SerDel@",
						"value": "Supprimer le sérotype"
					},
					{
						"key": "@SpcA@",
						"value": "Un nom d'espèce doit être saisi"
					},
					{
						"key": "@SpcAdd@",
						"value": "Ajouter des espèces"
					},
					{
						"key": "@SpcDel@",
						"value": "Supprimer des espèces"
					},
					{
						"key": "@SpeA@",
						"value": "Un commentaire doit être saisi"
					},
					{
						"key": "@SpeAcc@",
						"value": "Numéro d'accès"
					},
					{
						"key": "@SpeAck@",
						"value": "Accuser réception du spécimen"
					},
					{
						"key": "@SpeAckA@",
						"value": "Accuser réception"
					},
					{
						"key": "@SpeAckB@",
						"value": "Accuser réception de l'échantillon"
					},
					{
						"key": "@SpeAdd@",
						"value": "Ajouter une nouvelle culture"
					},
					{
						"key": "@SpeAddA@",
						"value": "Ajouter un enregistrement pour le spécimen reçu"
					},
					{
						"key": "@SpeAddB@",
						"value": "Ajouter un nouveau spécimen à distance"
					},
					{
						"key": "@SpeAddC@",
						"value": "Ajouter un nouveau spécimen"
					},
					{
						"key": "@SpeAddD@",
						"value": "Ajouter de la culture"
					},
					{
						"key": "@SpeAddE@",
						"value": "Ajouter un spécimen"
					},
					{
						"key": "@SpeAddF@",
						"value": "Conseils supplémentaires"
					},
					{
						"key": "@SpeAddG@",
						"value": "Ajouter la raison de l'approbation ou du rejet"
					},
					{
						"key": "@SpeAddH@",
						"value": "Ajouter un commentaire à un spécimen"
					},
					{
						"key": "@SpeAdv@",
						"value": "Demande de spécimen à l'avance"
					},
					{
						"key": "@SpeAli@",
						"value": "ID d'aliquote"
					},
					{
						"key": "@SpeAlr@",
						"value": "Spécimen déjà reçu"
					},
					{
						"key": "@SpeApi@",
						"value": "Panneau API/ID"
					},
					{
						"key": "@SpeApp@",
						"value": "Approuver ou rejeter l'analyse d'échantillon soumise"
					},
					{
						"key": "@SpeAss@",
						"value": "Évaluez chaque échantillon reçu dans le lot, puis passez au suivant"
					},
					{
						"key": "@SpeAssA@",
						"value": "Évaluez la croissance de chaque culture du lot, puis passez à la suivante"
					},
					{
						"key": "@SpeBat@",
						"value": "Traitement par lots jour 0 échantillons"
					},
					{
						"key": "@SpeBatA@",
						"value": "Traitement par lots des échantillons du jour 1"
					},
					{
						"key": "@SpeCan@",
						"value": "Annuler une demande de spécimen"
					},
					{
						"key": "@SpeCanA@",
						"value": "Demande d'annulation"
					},
					{
						"key": "@SpeCanB@",
						"value": "Annuler la demande de spécimen"
					},
					{
						"key": "@SpeCol@",
						"value": "La date de collecte doit être saisie"
					},
					{
						"key": "@SpeColA@",
						"value": "L'heure de collecte doit être saisie"
					},
					{
						"key": "@SpeColB@",
						"value": "Date/Heure de collecte"
					},
					{
						"key": "@SpeColC@",
						"value": "Date de collecte"
					},
					{
						"key": "@SpeColD@",
						"value": "Heure de collecte"
					},
					{
						"key": "@SpeCre@",
						"value": "Créer et gérer des cultures nouvelles et existantes"
					},
					{
						"key": "@SpeCreA@",
						"value": "Créer et gérer des enregistrements de spécimens nouveaux et existants"
					},
					{
						"key": "@SpeCul@",
						"value": "Détails de la culture"
					},
					{
						"key": "@SpeCulA@",
						"value": "Dossier culturel"
					},
					{
						"key": "@SpeCulB@",
						"value": "Des cultures"
					},
					{
						"key": "@SpeDay@",
						"value": "Jour 1 Lecture sur banc"
					},
					{
						"key": "@SpeDec@",
						"value": "Une décision est requise"
					},
					{
						"key": "@SpeDel@",
						"value": "Supprimer la culture"
					},
					{
						"key": "@SpeDet@",
						"value": "Détails de l'échantillon"
					},
					{
						"key": "@SpeDir@",
						"value": "Essais directs"
					},
					{
						"key": "@SpeEdi@",
						"value": "Modifier une culture existante"
					},
					{
						"key": "@SpeEdiA@",
						"value": "Modifier la culture"
					},
					{
						"key": "@SpeEnt@",
						"value": "Entrez un commentaire de spécimen"
					},
					{
						"key": "@SpeEntA@",
						"value": "Entrez L'heure de réception"
					},
					{
						"key": "@SpeEntB@",
						"value": "Entrez le poids de l'échantillon"
					},
					{
						"key": "@SpeEntC@",
						"value": "Saisissez le motif du rejet"
					},
					{
						"key": "@SpeEntD@",
						"value": "Entrez des détails supplémentaires"
					},
					{
						"key": "@SpeEntE@",
						"value": "Entrez le code-barres existant, le cas échéant"
					},
					{
						"key": "@SpeEntF@",
						"value": "Entrez le pourcentage d'identification"
					},
					{
						"key": "@SpeEntG@",
						"value": "Entrez la date d'un résultat positif"
					},
					{
						"key": "@SpeEntH@",
						"value": "Entrez L'heure d'un résultat positif"
					},
					{
						"key": "@SpeEntI@",
						"value": "Entrez L'heure de collecte"
					},
					{
						"key": "@SpeEsb@",
						"value": "BLSE"
					},
					{
						"key": "@SpeExi@",
						"value": "Code à barres existant"
					},
					{
						"key": "@SpeFul@",
						"value": "Recherche complète d'organismes"
					},
					{
						"key": "@SpeGro@",
						"value": "La croissance doit être saisie"
					},
					{
						"key": "@SpeGroA@",
						"value": "Croissance?"
					},
					{
						"key": "@SpeId@",
						"value": "Profil d'identification"
					},
					{
						"key": "@SpeIdA@",
						"value": "% IDENTIFIANT"
					},
					{
						"key": "@SpeIde@",
						"value": "Méthode d'identification"
					},
					{
						"key": "@SpeImm@",
						"value": "Action immédiate"
					},
					{
						"key": "@SpeInd@",
						"value": "Indiquer l'état du spécimen et le motif du rejet si non implicite"
					},
					{
						"key": "@SpeIndA@",
						"value": "Indiquez ce que vous vous apprêtez à faire avec le spécimen"
					},
					{
						"key": "@SpeMan@",
						"value": "Gérer les cultures"
					},
					{
						"key": "@SpeManA@",
						"value": "Gérer les tests directs"
					},
					{
						"key": "@SpeManB@",
						"value": "Gérer les tests directs"
					},
					{
						"key": "@SpeManC@",
						"value": "Gérer les échantillons pour le patient"
					},
					{
						"key": "@SpeManD@",
						"value": "Gérer les échantillons"
					},
					{
						"key": "@SpeNex@",
						"value": "Spécimen suivant"
					},
					{
						"key": "@SpeOrg@",
						"value": "l'organisme doit être saisi"
					},
					{
						"key": "@SpeOrgA@",
						"value": "Identité de l'organisme"
					},
					{
						"key": "@SpeOth@",
						"value": "les autres informations"
					},
					{
						"key": "@SpePos@",
						"value": "Date/heure positive"
					},
					{
						"key": "@SpePosA@",
						"value": "Date positive"
					},
					{
						"key": "@SpePosB@",
						"value": "Temps positif"
					},
					{
						"key": "@SpePro@",
						"value": "Traiter les échantillons d'aujourd'hui"
					},
					{
						"key": "@SpeProA@",
						"value": "Fournir des détails concernant l'échantillon tel qu'il a été physiquement reçu"
					},
					{
						"key": "@SpeProB@",
						"value": "Fournir des détails sur la collecte d'échantillons spécifiques au patient"
					},
					{
						"key": "@SpeProC@",
						"value": "Fournir des qualifications, des conseils ou des informations supplémentaires d'importance"
					},
					{
						"key": "@SpeProD@",
						"value": "Fournir les caractéristiques de l'échantillon"
					},
					{
						"key": "@SpeProE@",
						"value": "Fournir les détails de la méthode d'identification"
					},
					{
						"key": "@SpeProF@",
						"value": "Fournissez des détails sur l'organisme identifié ou faisant l'objet d'un dépistage"
					},
					{
						"key": "@SpeProG@",
						"value": "Fournissez ces détails restants, le cas échéant"
					},
					{
						"key": "@SpeProH@",
						"value": "Fournir l'identifiant d'aliquote si nécessaire"
					},
					{
						"key": "@SpeProI@",
						"value": "Fournir des informations cliniques exactes au moment de la collecte"
					},
					{
						"key": "@SpeProJ@",
						"value": "Indiquez les dates et heures importantes"
					},
					{
						"key": "@SpeRea@",
						"value": "Raison du rejet"
					},
					{
						"key": "@SpeRec@",
						"value": "La date de réception doit être saisie"
					},
					{
						"key": "@SpeRecA@",
						"value": "L'heure de réception doit être saisie"
					},
					{
						"key": "@SpeRecB@",
						"value": "État reçu"
					},
					{
						"key": "@SpeRecB@",
						"value": "La condition reçue doit être saisie"
					},
					{
						"key": "@SpeRecC@",
						"value": "Date/heure de réception"
					},
					{
						"key": "@SpeRecD@",
						"value": "Date de réception"
					},
					{
						"key": "@SpeRecE@",
						"value": "Heure de réception"
					},
					{
						"key": "@SpeRej@",
						"value": "Rejeter l'échantillon"
					},
					{
						"key": "@SpeSel@",
						"value": "Sélectionnez la date de réception"
					},
					{
						"key": "@SpeSelA@",
						"value": "Sélectionnez l'état de l'échantillon"
					},
					{
						"key": "@SpeSelB@",
						"value": "Sélectionnez l'apparence de l'échantillon"
					},
					{
						"key": "@SpeSelC@",
						"value": "Sélectionnez le type d'échantillon"
					},
					{
						"key": "@SpeSelD@",
						"value": "Sélectionnez le site du spécimen"
					},
					{
						"key": "@SpeSelE@",
						"value": "Sélectionnez l'index de profil analytique utilisé"
					},
					{
						"key": "@SpeSelF@",
						"value": "Sélectionnez le profil d'identification"
					},
					{
						"key": "@SpeSelG@",
						"value": "Sélectionnez le nom de l'organisme"
					},
					{
						"key": "@SpeSelH@",
						"value": "Sélectionnez l'étendue de la croissance"
					},
					{
						"key": "@SpeSelI@",
						"value": "Sélectionnez la date de collecte"
					},
					{
						"key": "@SpeSelJ@",
						"value": "Sélectionner l'organisme de culture"
					},
					{
						"key": "@SpeSer@",
						"value": "Sérotype"
					},
					{
						"key": "@SpeSerA@",
						"value": "Profil de sérotype"
					},
					{
						"key": "@SpeSpe@",
						"value": "Approbation des spécimens niveau 1"
					},
					{
						"key": "@SpeSpeA@",
						"value": "Approbation des spécimens niveau 2"
					},
					{
						"key": "@SpeSpeB@",
						"value": "Type d'échantillon"
					},
					{
						"key": "@SpeSpeC@",
						"value": "Site du spécimen"
					},
					{
						"key": "@SpeSpeD@",
						"value": "Poids de l'échantillon"
					},
					{
						"key": "@SpeSpeE@",
						"value": "État de l'échantillon"
					},
					{
						"key": "@SpeSpeF@",
						"value": "Apparence de l'échantillon"
					},
					{
						"key": "@SpeSpeG@",
						"value": "Enregistrement du spécimen"
					},
					{
						"key": "@SpeSpeH@",
						"value": "Action de l'échantillon"
					},
					{
						"key": "@SpeSpeI@",
						"value": "Approbation du spécimen"
					},
					{
						"key": "@SpeSpeJ@",
						"value": "Attributs de l'échantillon"
					},
					{
						"key": "@SpeSpeK@",
						"value": "Précisez le sérotype le cas échéant"
					},
					{
						"key": "@SpeSpeL@",
						"value": "Spécifier le profil de sérotype"
					},
					{
						"key": "@SpeSpeM@",
						"value": "Horaires des échantillons"
					},
					{
						"key": "@SpeSpeN@",
						"value": "Le type d'échantillon doit être saisi"
					},
					{
						"key": "@SpeSpeO@",
						"value": "Le site du spécimen doit être saisi"
					},
					{
						"key": "@SpeSub@",
						"value": "Soumettre un spécimen pour approbation"
					},
					{
						"key": "@SpeSubA@",
						"value": "Soumettre la confirmation"
					},
					{
						"key": "@SpeSubDat@",
						"value": "Date de soumission"
					},
					{
						"key": "@SpeAppDat@",
						"value": "Date d'approbation"
					},
					{
						"key": "@SpeTod@",
						"value": "Les spécimens d'aujourd'hui"
					},
					{
						"key": "@SpeVie@",
						"value": "Voir Culture"
					},
					{
						"key": "@SpeYou@",
						"value": "Vous êtes sur le point de soumettre un spécimen de dossier pour approbation. Veuillez confirmer."
					},
					{
						"key": "@TabA@",
						"value": "Un identifiant de liste doit être fourni"
					},
					{
						"key": "@TabAdd@",
						"value": "Ajouter une entrée au tableau"
					},
					{
						"key": "@TabAddA@",
						"value": "Ajouter une nouvelle entrée de table à la table actuellement sélectionnée"
					},
					{
						"key": "@TabDel@",
						"value": "Supprimer l'entrée du tableau"
					},
					{
						"key": "@TabDelA@",
						"value": "Supprimer l'entrée de table sélectionnée"
					},
					{
						"key": "@TabEdi@",
						"value": "Modifier l'entrée du tableau"
					},
					{
						"key": "@TabMan@",
						"value": "Maintenir les tableaux"
					},
					{
						"key": "@TabManA@",
						"value": "Maintenir le contenu de chaque table de référence"
					},
					{
						"key": "@TabTab@",
						"value": "Maintenir les tableaux"
					},
					{
						"key": "@TesAdd@",
						"value": "Ajouter un motif de test"
					},
					{
						"key": "@TesAddA@",
						"value": "Ajouter un nouveau motif de test"
					},
					{
						"key": "@TesAddB@",
						"value": "réglages généraux"
					},
					{
						"key": "@TesAddC@",
						"value": "Donnez un nom au motif de test et définissez son applicabilité"
					},
					{
						"key": "@TesAddD@",
						"value": "Ajouter un antibiotique"
					},
					{
						"key": "@TesAddE@",
						"value": "Ajouter des antibiotiques"
					},
					{
						"key": "@TesAddF@",
						"value": "Spécifiez l'antibiotique, le dosage et la méthode de test pour chaque composant du motif de test"
					},
					{
						"key": "@TesAfb@",
						"value": "Quantité AFB"
					},
					{
						"key": "@TesCar@",
						"value": "Effectuer des tests directs"
					},
					{
						"key": "@TesCel@",
						"value": "Test de numération cellulaire"
					},
					{
						"key": "@TesDel@",
						"value": "Supprimer le test d'échantillon"
					},
					{
						"key": "@TesDelA@",
						"value": "Supprimer le motif de test"
					},
					{
						"key": "@TesDelB@",
						"value": "Supprimer un motif de test"
					},
					{
						"key": "@TesEdi@",
						"value": "Modifier les antibiotiques"
					},
					{
						"key": "@TesEdiA@",
						"value": "Modifier les composants antibiotiques du motif de test"
					},
					{
						"key": "@TesEdiB@",
						"value": "Modifier les paramètres généraux"
					},
					{
						"key": "@TesEdiC@",
						"value": "Modifier les caractéristiques générales du motif de test"
					},
					{
						"key": "@TesEdiD@",
						"value": "Modifier le motif de test"
					},
					{
						"key": "@TesEnt@",
						"value": "Entrez les détails d'un test de comptage de cellules"
					},
					{
						"key": "@TesEntA@",
						"value": "Entrez les tests directs"
					},
					{
						"key": "@TesEntB@",
						"value": "Entrez les tests directs pour cet échantillon"
					},
					{
						"key": "@TesEntC@",
						"value": "Entrez les détails d'un test de coloration de Gram"
					},
					{
						"key": "@TesEntD@",
						"value": "Entrez les détails d'un test d'encre de Chine"
					},
					{
						"key": "@TesEntE@",
						"value": "Entrez les détails d'un test de préparation humide"
					},
					{
						"key": "@TesEntF@",
						"value": "Entrez les détails d'un test de coloration ZN"
					},
					{
						"key": "@TesEpi@",
						"value": "Cellules épi"
					},
					{
						"key": "@TesGra@",
						"value": "Test de coloration de Gram"
					},
					{
						"key": "@TesInd@",
						"value": "Test d'encre de Chine"
					},
					{
						"key": "@TesIndA@",
						"value": "Résultat d'encre de Chine"
					},
					{
						"key": "@TesIndPos@",
						"value": "Résultat positif encre de Chine"
					},
					{
						"key": "@TesAurRes@",
						"value": "Résultat TB - Auramine"
					},
					{
						"key": "@TesBetRes@",
						"value": "Résultat bétalactamase"
					},
					{
						"key": "@TesCarRes@",
						"value": "Résultat carbapénémase"
					},
					{
						"key": "@TesCatRes@",
						"value": "Résultat catalase"
					},
					{
						"key": "@TesEsbRes@",
						"value": "Résultat ESBL"
					},
					{
						"key": "@TesFunPos@",
						"value": "Résultat fongique KOH"
					},
					{
						"key": "@TesFunRes@",
						"value": "Résultat KOH prep"
					},
					{
						"key": "@TesGraWbc@",
						"value": "GB coloration de Gram"
					},
					{
						"key": "@TesHpyRes@",
						"value": "Résultat antigène H. pylori"
					},
					{
						"key": "@TesJevRes@",
						"value": "Résultat sérologie JEV"
					},
					{
						"key": "@TesOxiRes@",
						"value": "Résultat oxydase"
					},
					{
						"key": "@TesPreRes@",
						"value": "Résultat grossesse"
					},
					{
						"key": "@TesWetWbc@",
						"value": "GB préparation humide"
					},
					{
						"key": "@TesWriRes@",
						"value": "Résultat coloration Wright"
					},
					{
						"key": "@TesMan@",
						"value": "Gérer les sélections de tests"
					},
					{
						"key": "@TesManA@",
						"value": "Gérer les modèles de test"
					},
					{
						"key": "@TesManB@",
						"value": "Gérer la liste des modèles de test"
					},
					{
						"key": "@TesMon@",
						"value": "Mononucléaire"
					},
					{
						"key": "@TesPar@",
						"value": "Parasites"
					},
					{
						"key": "@TesParA@",
						"value": "Parasite"
					},
					{
						"key": "@TesPat@",
						"value": "Nom du motif de test"
					},
					{
						"key": "@TesPol@",
						"value": "Polymorphonucléaire"
					},
					{
						"key": "@TesPos@",
						"value": "Résultat positif"
					},
					{
						"key": "@TesRbc@",
						"value": "GR (mm^3)"
					},
					{
						"key": "@TesRbcA@",
						"value": "Qualitatif RBC"
					},
					{
						"key": "@TesRbcB@",
						"value": "RBC"
					},
					{
						"key": "@TesTes@",
						"value": "Sélections de test pour l'échantillon sélectionné"
					},
					{
						"key": "@TesUpd@",
						"value": "Mettre à jour la sélection de test pour l'échantillon"
					},
					{
						"key": "@TesWbc@",
						"value": "GB (mm^3)"
					},
					{
						"key": "@TesWbcA@",
						"value": "WBC qualitatif"
					},
					{
						"key": "@TesWbcB@",
						"value": "GB"
					},
					{
						"key": "@TesWet@",
						"value": "Test de préparation humide"
					},
					{
						"key": "@TesZns@",
						"value": "Test de coloration ZN"
					},
					{
						"key": "@UseAdd@",
						"value": "Ajouter un nouvel utilisateur"
					},
					{
						"key": "@UseAddA@",
						"value": "Ajouter un utilisateur"
					},
					{
						"key": "@UseAddB@",
						"value": "Ajouter des propriétés utilisateur"
					},
					{
						"key": "@UseClo@",
						"value": "Cloner l'utilisateur"
					},
					{
						"key": "@UseCre@",
						"value": "Créer et gérer des utilisateurs nouveaux et existants"
					},
					{
						"key": "@UseDel@",
						"value": "Supprimer l'utilisateur"
					},
					{
						"key": "@UseEdi@",
						"value": "Modifier un utilisateur existant"
					},
					{
						"key": "@UseEdiA@",
						"value": "Modifier l'utilisateur"
					},
					{
						"key": "@UseEit@",
						"value": "Un laboratoire ou une organisation doit être saisi"
					},
					{
						"key": "@UseEma@",
						"value": "E-mail"
					},
					{
						"key": "@UseFir@",
						"value": "Prénom"
					},
					{
						"key": "@UseLas@",
						"value": "Nom de famille"
					},
					{
						"key": "@UseMan@",
						"value": "gérer les utilisateurs"
					},
					{
						"key": "@UsePas@",
						"value": "Le mot de passe doit être saisi"
					},
					{
						"key": "@UsePasA@",
						"value": "Mot de passe"
					},
					{
						"key": "@UseRol@",
						"value": "Les rôles doivent être saisis"
					},
					{
						"key": "@UseSel@",
						"value": "Sélectionner le laboratoire"
					},
					{
						"key": "@UseSelA@",
						"value": "Sélectionnez l'organisation"
					},
					{
						"key": "@UseUse@",
						"value": "Le nom d'utilisateur doit être saisi"
					},
					{
						"key": "@UseUseA@",
						"value": "l'utilisateur ne peut pas appartenir à la fois à une organisation et à un laboratoire"
					},
					{
						"key": "@UseUseB@",
						"value": "Nom d'utilisateur"
					},
					{
						"key": "@UseUseC@",
						"value": "Propriétés de l'utilisateur"
					},
					{
						"key": "@ZonDia@",
						"value": "Diamètre de la zone"
					}
				]
				""";
        }
    }
}
