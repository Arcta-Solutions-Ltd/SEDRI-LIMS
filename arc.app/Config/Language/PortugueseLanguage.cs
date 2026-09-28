using arc.app.Common;

namespace arc.app.Config.Language
{
    internal class PortugueseLanguage : IDefinition
    {
        public string Get()
        {
            return """
				[
					{
						"key": "@AleAddChildTag@",
						"value": "Adicionar etiqueta secundária"
					},
					{
						"key": "@AleTagHasChildren@",
						"value": "Não é possível excluir uma etiqueta que tem etiquetas secundárias"
					},
					{
						"key": "@GenTagH@",
						"value": "Etiqueta principal"
					},
					{
						"key": "@GenTagI@",
						"value": "Selecionar etiqueta principal"
					},
					{
						"key": "@AstAdd@",
						"value": "Adicionar ou atualizar dados AST para uma cultura"
					},
					{
						"key": "@AstAnt@",
						"value": "Testes de Suscetibilidade Antimicrobiana"
					},
					{
						"key": "@AstCre@",
						"value": "Criar e gerenciar resultados de AST"
					},
					{
						"key": "@AstDis@",
						"value": "Testes de disco"
					},
					{
						"key": "@AstEnt@",
						"value": "Insira os antibióticos a serem testados, por tipo de método AST e os resultados do teste, se disponíveis"
					},
					{
						"key": "@AstMan@",
						"value": "Gerenciar AST"
					},
					{
						"key": "@AstMic@",
						"value": "Testes MIC"
					},
					{
						"key": "@AstPen@",
						"value": "AST pendente"
					},
					{
						"key": "@AstPre@",
						"value": "Selecione a presença"
					},
					{
						"key": "@AstRes@",
						"value": "Resultados AST"
					},
					{
						"key": "@AstTes@",
						"value": "Testes AST"
					},
					{
						"key": "@AstTesA@",
						"value": "Padrão de Teste AST"
					},
					{
						"key": "@AstTesB@",
						"value": "Padrão de teste"
					},
					{
						"key": "@AstTesC@",
						"value": "Selecione o padrão de teste"
					},
					{
						"key": "@AstUse@",
						"value": "Usar padrão para testes de disco"
					},
					{
						"key": "@AstUseA@",
						"value": "Use o padrão para testes de tira"
					},
					{
						"key": "@BreA@",
						"value": "Um tipo de amostra deve ser inserido"
					},
					{
						"key": "@BreAA@",
						"value": "Um host deve ser inserido"
					},
					{
						"key": "@BreAB@",
						"value": "Um método de teste deve ser inserido"
					},
					{
						"key": "@BreAC@",
						"value": "Uma suscetibilidade deve ser inserida"
					},
					{
						"key": "@BreAD@",
						"value": "Um nome de padrão de teste deve ser inserido"
					},
					{
						"key": "@BreAE@",
						"value": "Uma dosagem deve ser inserida"
					},
					{
						"key": "@BreAdd@",
						"value": "Adicionar Ponto de Interrupção"
					},
					{
						"key": "@BreAddA@",
						"value": "Adicionar um novo ponto de interrupção"
					},
					{
						"key": "@BreAddB@",
						"value": "Adicionar método de teste"
					},
					{
						"key": "@BreAddC@",
						"value": "Adicionar hospedeiro"
					},
					{
						"key": "@BreAddD@",
						"value": "Adicione um novo método de teste"
					},
					{
						"key": "@BreAddE@",
						"value": "Adicionar um novo host"
					},
					{
						"key": "@BreAddF@",
						"value": "Adicionar suscetibilidade"
					},
					{
						"key": "@BreAddG@",
						"value": "Adicionar uma nova suscetibilidade"
					},
					{
						"key": "@BreAddH@",
						"value": "Definir critérios"
					},
					{
						"key": "@BreAddI@",
						"value": "Defina as outras características para as quais o ponto de interrupção se aplicará"
					},
					{
						"key": "@BreAddJ@",
						"value": "Definir Ponto de Interrupção"
					},
					{
						"key": "@BreAddK@",
						"value": "Defina a faixa de medição para cada determinação de suscetibilidade"
					},
					{
						"key": "@BreAddL@",
						"value": "Editar Ponto de Interrupção"
					},
					{
						"key": "@BreAddM@",
						"value": "Critérios de edição"
					},
					{
						"key": "@BreAddN@",
						"value": "Edite as outras características para as quais o ponto de interrupção se aplicará"
					},
					{
						"key": "@BreAn@",
						"value": "Um antibiótico deve ser inserido"
					},
					{
						"key": "@BreAnA@",
						"value": "Um pedido deve ser inserido"
					},
					{
						"key": "@BreBre@",
						"value": "Breakpoints"
					},
					{
						"key": "@BreCan@",
						"value": "Não é possível excluir porque este host está em uso"
					},
					{
						"key": "@BreCanA@",
						"value": "Não é possível excluir porque este método de teste está em uso"
					},
					{
						"key": "@BreCanB@",
						"value": "Não é possível excluir porque esta suscetibilidade está em uso"
					},
					{
						"key": "@BreDel@",
						"value": "Apagar Ponto de Interrupção"
					},
					{
						"key": "@BreDelA@",
						"value": "Excluir um ponto de interrupção"
					},
					{
						"key": "@BreDelB@",
						"value": "Excluir método de teste"
					},
					{
						"key": "@BreDelC@",
						"value": "Excluir host"
					},
					{
						"key": "@BreDelD@",
						"value": "Exclua um método de teste existente"
					},
					{
						"key": "@BreDelE@",
						"value": "Excluir suscetibilidade"
					},
					{
						"key": "@BreDelF@",
						"value": "Exclua uma suscetibilidade existente"
					},
					{
						"key": "@BreEdi@",
						"value": "Editar Ponto de Interrupção"
					},
					{
						"key": "@BreEdiA@",
						"value": "Editar um ponto de interrupção existente"
					},
					{
						"key": "@BreEdiB@",
						"value": "Editar método de teste"
					},
					{
						"key": "@BreEdiC@",
						"value": "Editar host"
					},
					{
						"key": "@BreEdiD@",
						"value": "Editar um método de teste existente"
					},
					{
						"key": "@BreEdiE@",
						"value": "Editar um host existente"
					},
					{
						"key": "@BreEdiF@",
						"value": "Editar suscetibilidade"
					},
					{
						"key": "@BreEdiG@",
						"value": "Editar uma suscetibilidade existente"
					},
					{
						"key": "@BreHos@",
						"value": "Lista de Host"
					},
					{
						"key": "@BreMan@",
						"value": "Gerenciar pontos de interrupção"
					},
					{
						"key": "@BreManA@",
						"value": "Gerenciar lista de pontos de interrupção"
					},
					{
						"key": "@BreTes@",
						"value": "Método de teste"
					},
					{
						"key": "@BreTesA@",
						"value": "Métodos de teste"
					},
					{
						"key": "@BreThe@",
						"value": "O host que você deseja alterar deve ser selecionado"
					},
					{
						"key": "@BreTheA@",
						"value": "O método de teste que você deseja alterar deve ser selecionado"
					},
					{
						"key": "@BreTheB@",
						"value": "A suscetibilidade que você deseja alterar deve ser selecionada"
					},
					{
						"key": "@BreThi@",
						"value": "Este método de teste já existe"
					},
					{
						"key": "@BreThiA@",
						"value": "Este host já existe"
					},
					{
						"key": "@BreThiB@",
						"value": "Esta suscetibilidade já existe"
					},
					{
						"key": "@BreLin@",
						"value": "Linhas de ponto de corte"
					},
					{
						"key": "@BreSta@",
						"value": "Valor inicial"
					},
					{
						"key": "@BreEnd@",
						"value": "Valor final"
					},
					{
						"key": "@BreAppHis@",
						"value": "Histórico de aprovação"
					},
					{
						"key": "@BreAddApp@",
						"value": "Adicionar aprovação/rejeição"
					},
					{
						"key": "@BreAddAppB@",
						"value": "Adicionar aprovação/rejeição"
					},
					{
						"key": "@BreDatRec@",
						"value": "Data registrada"
					},
					{
						"key": "@BreRecBy@",
						"value": "Registrado por"
					},
					{
						"key": "@BreId@",
						"value": "Id"
					},
					{
						"key": "@CodA@",
						"value": "Um nome de lista deve ser inserido"
					},
					{
						"key": "@CodAA@",
						"value": "Uma entrada personalizada deve ser inserida"
					},
					{
						"key": "@CodAB@",
						"value": "Uma lista de codificação deve ser selecionada"
					},
					{
						"key": "@CodAC@",
						"value": "Um código deve ser inserido"
					},
					{
						"key": "@CodAdd@",
						"value": "Adicionar lista de codificação"
					},
					{
						"key": "@CodAddA@",
						"value": "Adicionar entrada de codificação personalizada"
					},
					{
						"key": "@CodAddB@",
						"value": "Adicionar entrada personalizada à lista de organismos"
					},
					{
						"key": "@CodAddC@",
						"value": "Adicionar uma nova lista de codificação"
					},
					{
						"key": "@CodAss@",
						"value": "Atribuir Código"
					},
					{
						"key": "@CodAssA@",
						"value": "Atribuir um código ao organismo selecionado"
					},
					{
						"key": "@CodDel@",
						"value": "Excluir lista de codificação"
					},
					{
						"key": "@CodDelA@",
						"value": "Exclua uma lista de coing existente e todo o seu conteúdo"
					},
					{
						"key": "@CodDelB@",
						"value": "Excluir organismo"
					},
					{
						"key": "@CodDelC@",
						"value": "Exclua um organismo ou entrada personalizada da lista de codificação"
					},
					{
						"key": "@CodEdi@",
						"value": "Editar entrada personalizada de codificação"
					},
					{
						"key": "@CodGra@",
						"value": "Grama"
					},
					{
						"key": "@CodLis@",
						"value": "Lista de organismos que correspondem aos critérios de pesquisa"
					},
					{
						"key": "@CodNew@",
						"value": "Nova entrada"
					},
					{
						"key": "@CodSco@",
						"value": "Selecione o escopo do organismo no qual a configuração será aplicada"
					},
					{
						"key": "@CodScoA@",
						"value": "Edite o escopo do organismo no qual a configuração será aplicada"
					},
					{
						"key": "@CodSel@",
						"value": "Selecione o organismo a usar"
					},
					{
						"key": "@CodSelA@",
						"value": "Selecione o gênero"
					},
					{
						"key": "@CodSelB@",
						"value": "Selecione as espécies"
					},
					{
						"key": "@CodSelC@",
						"value": "Selecione o sorotipo"
					},
					{
						"key": "@CodThe@",
						"value": "O organismo não existe"
					},
					{
						"key": "@CodTheA@",
						"value": "O organismo já foi adicionado a esta lista de codificação"
					},
					{
						"key": "@CodThi@",
						"value": "Esta lista de codificação já existe"
					},
					{
						"key": "@CodThiA@",
						"value": "Esta lista de codificação não pode ser excluída"
					},
					{
						"key": "@CodThiB@",
						"value": "Esta entrada já existe na lista"
					},
					{
						"key": "@ConAdd@",
						"value": "Adicionar Formulário"
					},
					{
						"key": "@ConCre@",
						"value": "Crie e gerencie definições de formulário novas e existentes"
					},
					{
						"key": "@ConDel@",
						"value": "Apagar Formulário"
					},
					{
						"key": "@ConEdi@",
						"value": "Editar Formulário"
					},
					{
						"key": "@ConFor@",
						"value": "Nome do formulario"
					},
					{
						"key": "@ConMan@",
						"value": "Gerenciar definições de formulário"
					},
					{
						"key": "@ExpCon@",
						"value": "Configure e emita uma exportação DHIS2"
					},
					{
						"key": "@ExpCon@",
						"value": "Configurar e emitir exportações de dados"
					},
					{
						"key": "@ExpConA@",
						"value": "Configurar e emitir uma exportação WHONET"
					},
					{
						"key": "@ExpDhi@",
						"value": "Exportação DHIS2"
					},
					{
						"key": "@ExpGen@",
						"value": "Gere e salve um arquivo de exportação DHIS2"
					},
					{
						"key": "@ExpGenA@",
						"value": "Gerar e salvar um arquivo de exportação WHONET"
					},
					{
						"key": "@ExpMan@",
						"value": "Gerenciar Exportações"
					},
					{
						"key": "@ExpPro@",
						"value": "Listar perfis"
					},
					{
						"key": "@ExpProD@",
						"value": "Descrição"
					},
					{
						"key": "@ExpProE@",
						"value": "Habilitada"
					},
					{
						"key": "@ExpProMap@",
						"value": "Gerir mapeamento"
					},
					{
						"key": "@ExpProMapDesc@",
						"value": "Construa um mapeamento JSON ou XML para os campos incluídos neste perfil de exportação."
					},
					{
						"key": "@ExpProMapSav@",
						"value": "Guardar o mapeamento JSON ou XML para um perfil de exportação"
					},
					{
						"key": "@ExpProMapFmt@",
						"value": "Formato de saída"
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
						"value": "Adicionar atributo"
					},
					{
						"key": "@ExpProMapAddArr@",
						"value": "Adicionar matriz"
					},
					{
						"key": "@ExpProMapAttrName@",
						"value": "Nome do atributo"
					},
					{
						"key": "@ExpProMapField@",
						"value": "Campo"
					},
					{
						"key": "@ExpProMapArrName@",
						"value": "Nome da matriz"
					},
					{
						"key": "@ExpProMapArrType@",
						"value": "Tipo de matriz"
					},
					{
						"key": "@ExpProMapArrSpecimen@",
						"value": "Amostras"
					},
					{
						"key": "@ExpProMapArrCulture@",
						"value": "Culturas / isolados"
					},
					{
						"key": "@ExpProMapArrGrid@",
						"value": "Grelha (resultados de testes)"
					},
					{
						"key": "@ExpProMapArrAst@",
						"value": "Resultados AST"
					},
					{
						"key": "@ExpProMapPreview@",
						"value": "Pré-visualização"
					},
					{
						"key": "@ExpProMapNoFields@",
						"value": "Ainda não existem campos no perfil de exportação. Adicione campos ao perfil antes de construir um mapeamento."
					},
					{
						"key": "@ExpProMapNoSpecimen@",
						"value": "As matrizes de amostras requerem pelo menos um campo de paciente no perfil de exportação."
					},
					{
						"key": "@ExpProMapNoCulture@",
						"value": "As matrizes de culturas requerem pelo menos um campo de cultura ou isolado no perfil de exportação."
					},
					{
						"key": "@ExpProMapGridReq@",
						"value": "As matrizes de grelha só podem ser adicionadas quando existe um campo de grelha selecionado."
					},
					{
						"key": "@ExpProMapNoAst@",
						"value": "As matrizes AST requerem pelo menos um campo da tabela AST no perfil de exportação."
					},
					{
						"key": "@ExpProMapAttrNameErr@",
						"value": "O nome do atributo é obrigatório."
					},
					{
						"key": "@ExpProMapFieldErr@",
						"value": "É obrigatório seleccionar um campo."
					},
					{
						"key": "@ExpProMapSavedTitle@",
						"value": "Mapeamento guardado"
					},
					{
						"key": "@ExpProMapSavedDesc@",
						"value": "As suas alterações ao mapeamento do perfil de exportação foram guardadas."
					},
					{
						"key": "@ExpProMapDelNode@",
						"value": "Remover"
					},
					{
						"key": "@ExpProMapEditNode@",
						"value": "Editar"
					},
					{
						"key": "@ExpProMapRoot@",
						"value": "root"
					},
					{
						"key": "@ExpProMd@",
						"value": "Data modificada"
					},
					{
						"key": "@ExpProN@",
						"value": "Nome"
					},
					{
						"key": "@ExpTyp@",
						"value": "Tipo de Exportação"
					},
					{
						"key": "@ExpWho@",
						"value": "WHONET Exportar"
					},
					{
						"key": "@ExpYou@",
						"value": "Você deve inserir uma data de início"
					},
					{
						"key": "@GenA@",
						"value": "Um valor deve ser inserido"
					},
					{
						"key": "@GenAct@",
						"value": "Açao"
					},
					{
						"key": "@GenAdd@",
						"value": "Notas Adicionais"
					},
					{
						"key": "@GenAddA@",
						"value": "Adicionado"
					},
					{
						"key": "@GenAddB@",
						"value": "Adicione uma mensagem para exibição em destaque"
					},
					{
						"key": "@GenAddC@",
						"value": "Adicionar comentário"
					},
					{
						"key": "@GenAddD@",
						"value": "Adicionar lista"
					},
					{
						"key": "@GenAddE@",
						"value": "Adicionar entrada"
					},
					{
						"key": "@GenAdm@",
						"value": "Administração"
					},
					{
						"key": "@GenAle@",
						"value": "Alertas"
					},
					{
						"key": "@GenAleA@",
						"value": "Alertas"
					},
					{
						"key": "@GenAnt@",
						"value": "Antibiótico"
					},
					{
						"key": "@GenAntA@",
						"value": "Antibióticos"
					},
					{
						"key": "@GenAntB@",
						"value": "Dosagem Antibiótica"
					},
					{
						"key": "@GenBac@",
						"value": "Voltar"
					},
					{
						"key": "@GenBre@",
						"value": "Breakpoints"
					},
					{
						"key": "@GenCan@",
						"value": "Cancelar"
					},
					{
						"key": "@GenClo@",
						"value": "Fechar"
					},
					{
						"key": "@GenCo1@",
						"value": "Recolher Menu"
					},
					{
						"key": "@GenCod@",
						"value": "Codificação"
					},
					{
						"key": "@GenCodA@",
						"value": "Código"
					},
					{
						"key": "@GenCodB@",
						"value": "Lista de Codificação"
					},
					{
						"key": "@GenCom@",
						"value": "Comentário 1"
					},
					{
						"key": "@GenComA@",
						"value": "Comentário 2"
					},
					{
						"key": "@GenComB@",
						"value": "Comente"
					},
					{
						"key": "@GenCon@",
						"value": "Configuração"
					},
					{
						"key": "@GenConA@",
						"value": "Número de contato"
					},
					{
						"key": "@GenCus@",
						"value": "Personalizado"
					},
					{
						"key": "@GenCusA@",
						"value": "Entradas personalizadas"
					},
					{
						"key": "@GenDat@",
						"value": "data adicionada"
					},
					{
						"key": "@GenDatA@",
						"value": "Data de Conclusão"
					},
					{
						"key": "@GenDec@",
						"value": "Decisão"
					},
					{
						"key": "@GenDef@",
						"value": "Predefinição"
					},
					{
						"key": "@GenDel@",
						"value": "Excluir teste"
					},
					{
						"key": "@GenDelA@",
						"value": "Apagar a Lista"
					},
					{
						"key": "@GenDelB@",
						"value": "Excluir entrada"
					},
					{
						"key": "@GenDelC@",
						"value": "Excluir"
					},
					{
						"key": "@GenDes@",
						"value": "Descrição"
					},
					{
						"key": "@GenDia@",
						"value": "Diagnóstico"
					},
					{
						"key": "@GenDiaA@",
						"value": "Diário"
					},
					{
						"key": "@GenDis@",
						"value": "Exibir no relatório"
					},
					{
						"key": "@GenDos@",
						"value": "Dosagem"
					},
					{
						"key": "@GenEdi@",
						"value": "Editar teste"
					},
					{
						"key": "@GenEdiA@",
						"value": "Editar entrada"
					},
					{
						"key": "@GenEna@",
						"value": "Habilitado"
					},
					{
						"key": "@GenEnaA@",
						"value": "Permitir"
					},
					{
						"key": "@GenEnd@",
						"value": "Data final"
					},
					{
						"key": "@GenEnt@",
						"value": "Entrar no modo de tela inteira"
					},
					{
						"key": "@GenEntA@",
						"value": "Digite o comentário"
					},
					{
						"key": "@GenEntB@",
						"value": "Entrada"
					},
					{
						"key": "@GenEve@",
						"value": "Evento"
					},
					{
						"key": "@GenEveA@",
						"value": "Eventos"
					},
					{
						"key": "@GenExi@",
						"value": "Saída"
					},
					{
						"key": "@GenExiA@",
						"value": "Sair do modo de tela inteira"
					},
					{
						"key": "@GenExp@",
						"value": "Exportações"
					},
					{
						"key": "@GenFam@",
						"value": "Família"
					},
					{
						"key": "@GenFil@",
						"value": "Filtro"
					},
					{
						"key": "@GenFilA@",
						"value": "Pré-ajustes de filtro"
					},
					{
						"key": "@GenFilB@",
						"value": "Filtrar por palavra-chave"
					},
					{
						"key": "@GenFin@",
						"value": "Terminar"
					},
					{
						"key": "@GenFir@",
						"value": "Primeira Aprovação"
					},
					{
						"key": "@GenFirA@",
						"value": "Segunda Aprovação"
					},
					{
						"key": "@GenFor@",
						"value": "Formulários"
					},
					{
						"key": "@GenFou@",
						"value": "Encontrado"
					},
					{
						"key": "@GenFul@",
						"value": "Tela cheia"
					},
					{
						"key": "@GenGen@",
						"value": "Gênero"
					},
					{
						"key": "@GenGri@",
						"value": "Visualização em grade"
					},
					{
						"key": "@GenGro@",
						"value": "Agrupamento"
					},
					{
						"key": "@GenHel@",
						"value": "Ajuda"
					},
					{
						"key": "@GenHom@",
						"value": "Casa"
					},
					{
						"key": "@GenHos@",
						"value": "Hospedeiro"
					},
					{
						"key": "@GenId@",
						"value": "Id deve ser inserido"
					},
					{
						"key": "@GenInc@",
						"value": "Incluir no Relatório"
					},
					{
						"key": "@GenIss@",
						"value": "Data de Emissão"
					},
					{
						"key": "@GenKey@",
						"value": "Chave"
					},
					{
						"key": "@GenLab@",
						"value": "Laboratories"
					},
					{
						"key": "@GenLabA@",
						"value": "Laboratório"
					},
					{
						"key": "@GenLan@",
						"value": "Língua"
					},
					{
						"key": "@GenLis@",
						"value": "Listas"
					},
					{
						"key": "@GenLisA@",
						"value": "Lista de nomes"
					},
					{
						"key": "@GenLoc@",
						"value": "Localização"
					},
					{
						"key": "@GenMea@",
						"value": "Medição"
					},
					{
						"key": "@GenMes@",
						"value": "Mensagem"
					},
					{
						"key": "@GenMet@",
						"value": "Método"
					},
					{
						"key": "@GenMon@",
						"value": "Monitoramento"
					},
					{
						"key": "@GenNam@",
						"value": "Nome"
					},
					{
						"key": "@GenNex@",
						"value": "Próximo"
					},
					{
						"key": "@GenNexA@",
						"value": "Próxima página"
					},
					{
						"key": "@GenOrd@",
						"value": "Pedido"
					},
					{
						"key": "@GenOrg@",
						"value": "Organizações"
					},
					{
						"key": "@GenOrgA@",
						"value": "Organismo"
					},
					{
						"key": "@GenOrgB@",
						"value": "Organismos"
					},
					{
						"key": "@GenOrgC@",
						"value": "Organização"
					},
					{
						"key": "@GenOrgD@",
						"value": "Lista de Organismos"
					},
					{
						"key": "@GenPar@",
						"value": "Pai"
					},
					{
						"key": "@GenParA@",
						"value": "Entrada pai"
					},
					{
						"key": "@GenPat@",
						"value": "Pacientes"
					},
					{
						"key": "@GenPre@",
						"value": "Anterior"
					},
					{
						"key": "@GenPri@",
						"value": "Imprimir código de barras (tipo 1)"
					},
					{
						"key": "@GenPriA@",
						"value": "Imprimir código de barras (tipo 2)"
					},
					{
						"key": "@GenQua@",
						"value": "Quantidade"
					},
					{
						"key": "@GenRea@",
						"value": "Razão"
					},
					{
						"key": "@GenRep@",
						"value": "Relatórios"
					},
					{
						"key": "@GenRes@",
						"value": "Data do Resultado"
					},
					{
						"key": "@GenRol@",
						"value": "Funções"
					},
					{
						"key": "@GenSav@",
						"value": "Salve "
					},
					{
						"key": "@GenSea@",
						"value": "Procurar"
					},
					{
						"key": "@GenSee@",
						"value": "Visto"
					},
					{
						"key": "@GenSel@",
						"value": "Selecione a data de início"
					},
					{
						"key": "@GenSelA@",
						"value": "Selecione a data de término"
					},
					{
						"key": "@GenSelB@",
						"value": "Selecione a localização"
					},
					{
						"key": "@GenSelC@",
						"value": "Selecione a ala"
					},
					{
						"key": "@GenSelD@",
						"value": "Selecione o diagnóstico"
					},
					{
						"key": "@GenSelE@",
						"value": "Selecione um comentário aplicável"
					},
					{
						"key": "@GenSelF@",
						"value": "Selecione o organismo"
					},
					{
						"key": "@GenSelG@",
						"value": "Selecione um dos pais"
					},
					{
						"key": "@GenSelH@",
						"value": "Definir o escopo do organismo"
					},
					{
						"key": "@GenSelI@",
						"value": "Editar Escopo do Organismo"
					},
					{
						"key": "@GenSer@",
						"value": "Serótipo"
					},
					{
						"key": "@GenSet@",
						"value": "Definições"
					},
					{
						"key": "@GenSpe@",
						"value": "Espécimes"
					},
					{
						"key": "@GenSpeA@",
						"value": "Especificamos"
					},
					{
						"key": "@GenSpeB@",
						"value": "Espécies"
					},
					{
						"key": "@GenSta@",
						"value": "Estado"
					},
					{
						"key": "@GenStaA@",
						"value": "Status"
					},
					{
						"key": "@GenStaB@",
						"value": "Data de início"
					},
					{
						"key": "@GenSub@",
						"value": "Enviar"
					},
					{
						"key": "@GenSubA@",
						"value": "Subespécies"
					},
					{
						"key": "@GenSus@",
						"value": "Suscetibilidade"
					},
					{
						"key": "@GenTab@",
						"value": "Mesas"
					},
					{
						"key": "@GenTes@",
						"value": "Testes"
					},
					{
						"key": "@GenTesA@",
						"value": "Tipo de Teste"
					},
					{
						"key": "@GenTesB@",
						"value": "Padrões de Teste"
					},
					{
						"key": "@GenTit@",
						"value": "Título"
					},
					{
						"key": "@GenTop@",
						"value": "Tema"
					},
					{
						"key": "@GenTyp@",
						"value": "Modelo"
					},
					{
						"key": "@GenUse@",
						"value": "Comercial"
					},
					{
						"key": "@GenUseA@",
						"value": "Do utilizador"
					},
					{
						"key": "@GenVal@",
						"value": "Valor"
					},
					{
						"key": "@GenVie@",
						"value": "Visualizações"
					},
					{
						"key": "@GenVieA@",
						"value": "Ver / Atualizar"
					},
					{
						"key": "@GenVieB@",
						"value": "Ver teste"
					},
					{
						"key": "@GenVieC@",
						"value": "Visualizar"
					},
					{
						"key": "@GenVieD@",
						"value": "Ver relatórios"
					},
					{
						"key": "@GenWar@",
						"value": "ala"
					},
					{
						"key": "@GenWor@",
						"value": "Fluxos de Trabalho"
					},
					{
						"key": "@LabA@",
						"value": "Uma tradução deve ser selecionada"
					},
					{
						"key": "@LabAA@",
						"value": "Uma codificação deve ser selecionada"
					},
					{
						"key": "@LabAdd@",
						"value": "Adicionar Laboratório"
					},
					{
						"key": "@LabBre@",
						"value": "Listas de pontos de interrupção para usar"
					},
					{
						"key": "@LabBreA@",
						"value": "Listas de pontos de interrupção"
					},
					{
						"key": "@LabCre@",
						"value": "Criar e gerenciar laboratórios novos e existentes"
					},
					{
						"key": "@LabDel@",
						"value": "Excluir Laboratório"
					},
					{
						"key": "@LabEdi@",
						"value": "Editar detalhes do laboratório"
					},
					{
						"key": "@LabEdiA@",
						"value": "Editar Laboratório"
					},
					{
						"key": "@LabEnt@",
						"value": "Digite o nome do laboratório"
					},
					{
						"key": "@LabEntA@",
						"value": "Insira os novos detalhes do laboratório"
					},
					{
						"key": "@LabLab@",
						"value": "Laboratório"
					},
					{
						"key": "@LabLabA@",
						"value": "Nome do Laboratório"
					},
					{
						"key": "@LabLabB@",
						"value": "O nome do laboratório deve ser inserido"
					},
					{
						"key": "@LabMan@",
						"value": "Gerenciar Laboratórios"
					},
					{
						"key": "@LabOrg@",
						"value": "Listas de organismos para usar"
					},
					{
						"key": "@LabOrgA@",
						"value": "Listas de organismos"
					},
					{
						"key": "@LabTes@",
						"value": "Listas de padrões de teste para usar"
					},
					{
						"key": "@LabTesA@",
						"value": "Listas de padrões de teste"
					},
					{
						"key": "@LanA@",
						"value": "Uma tradução a ser copiada deve ser inserida"
					},
					{
						"key": "@LanAA@",
						"value": "Uma tradução para deletar deve ser selecionada"
					},
					{
						"key": "@LanAdd@",
						"value": "Adicionar tradução"
					},
					{
						"key": "@LanCre@",
						"value": "Criar e gerenciar traduções"
					},
					{
						"key": "@LanCreA@",
						"value": "Crie uma nova tradução"
					},
					{
						"key": "@LanDel@",
						"value": "Excluir tradução"
					},
					{
						"key": "@LanDelA@",
						"value": "Apagar uma tradução existente"
					},
					{
						"key": "@LanEdiA@",
						"value": "Editar uma entrada de tradução"
					},
					{
						"key": "@LanEnt@",
						"value": "Insira o nome da tradução"
					},
					{
						"key": "@LanMan@",
						"value": "Gerenciar traduções"
					},
					{
						"key": "@LanSel@",
						"value": "Selecione a tradução"
					},
					{
						"key": "@LanThi@",
						"value": "Esta tradução já existe"
					},
					{
						"key": "@LanTra@",
						"value": "Traduções"
					},
					{
						"key": "@LanTraA@",
						"value": "Tradução"
					},
					{
						"key": "@LanTraB@",
						"value": "Tradução para copiar"
					},
					{
						"key": "@LanTraC@",
						"value": "O nome da tradução deve ser inserido"
					},
					{
						"key": "@LanTraD@",
						"value": "A tradução está em uso dentro de uma organização"
					},
					{
						"key": "@LanTraE@",
						"value": "A tradução está em uso em um laboratório"
					},
					{
						"key": "@MonMon@",
						"value": "Monitore todos os eventos"
					},
					{
						"key": "@MonMonA@",
						"value": "Monitore todos os eventos que alteram os dados no sistema"
					},
					{
						"key": "@MonVie@",
						"value": "Ver detalhes do evento de monitoramento"
					},
					{
						"key": "@MonVieA@",
						"value": "Ver detalhes"
					},
					{
						"key": "@MonVieB@",
						"value": "Ver Detalhes (Raw)"
					},
					{
						"key": "@MonVieC@",
						"value": "Ver detalhes do evento"
					},
					{
						"key": "@MonVieD@",
						"value": "Ver detalhes do evento"
					},
					{
						"key": "@OrgA@",
						"value": "Um nome de organismo deve ser inserido"
					},
					{
						"key": "@OrgAdd@",
						"value": "Adicionar Organização"
					},
					{
						"key": "@OrgAddA@",
						"value": "Adicionar Organismo"
					},
					{
						"key": "@OrgAll@",
						"value": "Todos os Organismos"
					},
					{
						"key": "@OrgB@",
						"value": "Um ID de organismo deve ser inserido"
					},
					{
						"key": "@OrgCre@",
						"value": "Crie e gerencie organizações novas e existentes"
					},
					{
						"key": "@OrgDel@",
						"value": "Excluir Organização"
					},
					{
						"key": "@OrgDelA@",
						"value": "Excluir Organismo"
					},
					{
						"key": "@OrgEdi@",
						"value": "Editar Organização"
					},
					{
						"key": "@OrgEdiA@",
						"value": "Editar os detalhes de uma organização existente"
					},
					{
						"key": "@OrgEdiB@",
						"value": "Editar Organismo"
					},
					{
						"key": "@OrgEnt@",
						"value": "Insira os novos detalhes da organização"
					},
					{
						"key": "@OrgEntA@",
						"value": "Digite o nome da organização"
					},
					{
						"key": "@OrgMan@",
						"value": "Gerenciar Organizações"
					},
					{
						"key": "@OrgManA@",
						"value": "Gerenciar listas de organismos"
					},
					{
						"key": "@OrgManB@",
						"value": "Gerenciar listas usadas para organismos"
					},
					{
						"key": "@OrgOrg@",
						"value": "Nome da Organização"
					},
					{
						"key": "@OrgOrgA@",
						"value": "O nome da organização deve ser inserido"
					},
					{
						"key": "@OrgPar@",
						"value": "Organização Matriz"
					},
					{
						"key": "@OrgSel@",
						"value": "Selecione a organização mãe"
					},
					{
						"key": "@PatA@",
						"value": "Um comentário deve ser inserido"
					},
					{
						"key": "@PatAdd@",
						"value": "Adicionar um novo paciente"
					},
					{
						"key": "@PatAddA@",
						"value": "Adicionar Paciente"
					},
					{
						"key": "@PatAddB@",
						"value": "Endereço do paciente"
					},
					{
						"key": "@PatAddC@",
						"value": "Adicione um comentário a um paciente"
					},
					{
						"key": "@PatAdm@",
						"value": "Data de admissão"
					},
					{
						"key": "@PatAge@",
						"value": "Era"
					},
					{
						"key": "@PatCli@",
						"value": "Número de contato clínico"
					},
					{
						"key": "@PatCre@",
						"value": "Crie e gerencie registros de pacientes novos e existentes"
					},
					{
						"key": "@PatDat@",
						"value": "Data de nascimento"
					},
					{
						"key": "@PatDel@",
						"value": "Apagar Paciente"
					},
					{
						"key": "@PatDet@",
						"value": "Dados do paciente"
					},
					{
						"key": "@PatDis@",
						"value": "Distrito"
					},
					{
						"key": "@PatEdi@",
						"value": "Editar detalhes do paciente"
					},
					{
						"key": "@PatEdiA@",
						"value": "Editar Paciente"
					},
					{
						"key": "@PatEdiB@",
						"value": "Editar os detalhes do endereço do paciente"
					},
					{
						"key": "@PatEnt@",
						"value": "Insira um comentário de paciente"
					},
					{
						"key": "@PatEntA@",
						"value": "Introduza o primeiro nome"
					},
					{
						"key": "@PatEntB@",
						"value": "Digite o sobrenome"
					},
					{
						"key": "@PatEntC@",
						"value": "Insira a idade"
					},
					{
						"key": "@PatEntD@",
						"value": "Digite o número do telefone"
					},
					{
						"key": "@PatEntE@",
						"value": "Insira os novos detalhes do paciente"
					},
					{
						"key": "@PatEntF@",
						"value": "Insira a referência do paciente"
					},
					{
						"key": "@PatEntG@",
						"value": "Insira o número de contato clínico"
					},
					{
						"key": "@PatEntH@",
						"value": "Insira o valor"
					},
					{
						"key": "@PatFin@",
						"value": "Encontre paciente por palavra-chave"
					},
					{
						"key": "@PatFir@",
						"value": "Primeiro nome"
					},
					{
						"key": "@PatGen@",
						"value": "Sexo deve ser inserido"
					},
					{
						"key": "@PatGenA@",
						"value": "Gênero"
					},
					{
						"key": "@PatMan@",
						"value": "Gerenciar Pacientes"
					},
					{
						"key": "@PatPat@",
						"value": "Referência do paciente deve ser inserida"
					},
					{
						"key": "@PatPatA@",
						"value": "Nome do paciente"
					},
					{
						"key": "@PatPatB@",
						"value": "Ref. Paciente"
					},
					{
						"key": "@PatPatC@",
						"value": "Ficha do Paciente"
					},
					{
						"key": "@PatPatD@",
						"value": "Dados do paciente"
					},
					{
						"key": "@PatPatE@",
						"value": "Comentários do paciente"
					},
					{
						"key": "@PatPatF@",
						"value": "Pesquisa de Paciente"
					},
					{
						"key": "@PatPatG@",
						"value": "Resultados da pesquisa de paciente"
					},
					{
						"key": "@PatPatH@",
						"value": "Detalhes de coleta do paciente"
					},
					{
						"key": "@PatPatI@",
						"value": "Paciente"
					},
					{
						"key": "@PatPatJ@",
						"value": "Localização do paciente"
					},
					{
						"key": "@PatPro@",
						"value": "Província"
					},
					{
						"key": "@PatSea@",
						"value": "Procure um paciente"
					},
					{
						"key": "@PatSel@",
						"value": "Selecione a data de nascimento"
					},
					{
						"key": "@PatSelA@",
						"value": "Selecione o gênero"
					},
					{
						"key": "@PatSelB@",
						"value": "Selecione a província"
					},
					{
						"key": "@PatSelC@",
						"value": "Selecione o distrito"
					},
					{
						"key": "@PatSelD@",
						"value": "Selecione o subdistrito"
					},
					{
						"key": "@PatSelE@",
						"value": "Selecione um paciente existente ou defina um novo"
					},
					{
						"key": "@PatSelF@",
						"value": "Selecione a data de admissão"
					},
					{
						"key": "@PatSub@",
						"value": "Sub distrito"
					},
					{
						"key": "@PatSur@",
						"value": "O sobrenome deve ser inserido"
					},
					{
						"key": "@PatSurA@",
						"value": "Sobrenome"
					},
					{
						"key": "@PatTel@",
						"value": "Número de telefone"
					},
					{
						"key": "@PatVie@",
						"value": "Ver Paciente"
					},
					{
						"key": "@RepA@",
						"value": "Um nome de relatório deve ser inserido"
					},
					{
						"key": "@RepAnt@",
						"value": "Antibiótico"
					},
					{
						"key": "@RepApp@",
						"value": "Aparência"
					},
					{
						"key": "@RepCel@",
						"value": "Contagem de células"
					},
					{
						"key": "@RepCul@",
						"value": "Resultado da Cultura"
					},
					{
						"key": "@RepGra@",
						"value": "Mancha de Gram"
					},
					{
						"key": "@RepIfy@",
						"value": "Se você gostaria de discutir o resultado ou tratamento, ligue para o Laboratório de Microbiologia"
					},
					{
						"key": "@RepInd@",
						"value": "Tinta da Índia"
					},
					{
						"key": "@RepMic@",
						"value": "Relatório de laboratório de microbiologia"
					},
					{
						"key": "@RepPre@",
						"value": "Resultados da pré-cultura"
					},
					{
						"key": "@RepPreA@",
						"value": "Data de Pré-cultura"
					},
					{
						"key": "@RepPri@",
						"value": "Imprimir / publicar relatório de amostra"
					},
					{
						"key": "@RepPriA@",
						"value": "Imprimir e publicar relatório"
					},
					{
						"key": "@RepPub@",
						"value": "Publicar Relatório"
					},
					{
						"key": "@RepRef@",
						"value": "Paciente"
					},
					{
						"key": "@RepRep@",
						"value": "Histórico do Relatório"
					},
					{
						"key": "@RepRes@",
						"value": "Resultado"
					},
					{
						"key": "@RepSen@",
						"value": "Sensibilidade"
					},
					{
						"key": "@RepSpe@",
						"value": "Relatório de Amostra"
					},
					{
						"key": "@RepWet@",
						"value": "Wet Prep"
					},
					{
						"key": "@RepZns@",
						"value": "ZN Stain"
					},
					{
						"key": "@RolAdd@",
						"value": "Adicionar papel"
					},
					{
						"key": "@RolAddA@",
						"value": "Adicionando uma nova função"
					},
					{
						"key": "@RolCan@",
						"value": "O nome da função deve ser exclusivo"
					},
					{
						"key": "@RolClo@",
						"value": "Clonar uma função"
					},
					{
						"key": "@RolCloA@",
						"value": "Função do clone"
					},
					{
						"key": "@RolCloB@",
						"value": "Clone uma função existente no sistema. Isso irá copiar os detalhes de permissão da função existente para a nova função"
					},
					{
						"key": "@RolCon@",
						"value": "Configure os aspectos gerais da função"
					},
					{
						"key": "@RolConA@",
						"value": "A configuração deve ser inserida"
					},
					{
						"key": "@RolConB@",
						"value": "Configure as permissões de eventos para esta função"
					},
					{
						"key": "@RolConC@",
						"value": "Configure as permissões do menu para esta função"
					},
					{
						"key": "@RolCre@",
						"value": "Criar e gerenciar funções novas e existentes"
					},
					{
						"key": "@RolDel@",
						"value": "Excluir uma função"
					},
					{
						"key": "@RolDelA@",
						"value": "Excluir função"
					},
					{
						"key": "@RolDes@",
						"value": "A descrição da função deve ser inserida"
					},
					{
						"key": "@RolEdi@",
						"value": "Editar função existente"
					},
					{
						"key": "@RolEdiA@",
						"value": "Editar papel"
					},
					{
						"key": "@RolEdiB@",
						"value": "Edite os aspectos gerais de uma função existente"
					},
					{
						"key": "@RolEna@",
						"value": "Ativado deve ser inserido"
					},
					{
						"key": "@RolEve@",
						"value": "Permissões de eventos"
					},
					{
						"key": "@RolLab@",
						"value": "Admin de Laboratório"
					},
					{
						"key": "@RolMan@",
						"value": "Gerenciar funções"
					},
					{
						"key": "@RolManA@",
						"value": "Gerenciar permissões de eventos"
					},
					{
						"key": "@RolManB@",
						"value": "Gerenciar permissões do menu"
					},
					{
						"key": "@RolMen@",
						"value": "Permissões do menu"
					},
					{
						"key": "@RolNam@",
						"value": "O nome da função deve ser inserido"
					},
					{
						"key": "@RolNew@",
						"value": "Detalhes da nova função"
					},
					{
						"key": "@RolOrg@",
						"value": "Administrador da Organização"
					},
					{
						"key": "@RolRem@",
						"value": "Remover uma função do sistema"
					},
					{
						"key": "@RolRol@",
						"value": "Descrição da Função"
					},
					{
						"key": "@RolRolA@",
						"value": "Nome do papel"
					},
					{
						"key": "@RolRolB@",
						"value": "Registro de Função"
					},
					{
						"key": "@RolRolC@",
						"value": "Funções"
					},
					{
						"key": "@RolRolD@",
						"value": "Função para clonar"
					},
					{
						"key": "@RolSel@",
						"value": "Selecione as funções"
					},
					{
						"key": "@RolUpd@",
						"value": "Atualizar permissões de eventos"
					},
					{
						"key": "@RolUpdA@",
						"value": "Atualizar permissões do menu"
					},
					{
						"key": "@SerA@",
						"value": "Um nome de serótipo deve ser inserido"
					},
					{
						"key": "@SerAdd@",
						"value": "Adicionar serótipo"
					},
					{
						"key": "@SerDel@",
						"value": "Excluir serótipo"
					},
					{
						"key": "@SpcA@",
						"value": "Um nome de espécie deve ser inserido"
					},
					{
						"key": "@SpcAdd@",
						"value": "Adicionar Espécies"
					},
					{
						"key": "@SpcDel@",
						"value": "Apagar Espécies"
					},
					{
						"key": "@SpeA@",
						"value": "Um comentário deve ser inserido"
					},
					{
						"key": "@SpeAcc@",
						"value": "Número de acesso"
					},
					{
						"key": "@SpeAck@",
						"value": "Confirmar o recebimento da amostra"
					},
					{
						"key": "@SpeAckA@",
						"value": "Confirmar recebimento"
					},
					{
						"key": "@SpeAckB@",
						"value": "Confirmar recebimento de amostra"
					},
					{
						"key": "@SpeAdd@",
						"value": "Adicione uma nova cultura"
					},
					{
						"key": "@SpeAddA@",
						"value": "Adicionar registro para espécime recebido"
					},
					{
						"key": "@SpeAddB@",
						"value": "Adicionar novo espécime remotamente"
					},
					{
						"key": "@SpeAddC@",
						"value": "Adicionar novo espécime"
					},
					{
						"key": "@SpeAddD@",
						"value": "Adicionar Cultura"
					},
					{
						"key": "@SpeAddE@",
						"value": "Adicionar espécime"
					},
					{
						"key": "@SpeAddF@",
						"value": "Orientação Adicional"
					},
					{
						"key": "@SpeAddG@",
						"value": "Adicionar motivo para aprovação ou rejeição"
					},
					{
						"key": "@SpeAddH@",
						"value": "Adicione um comentário a um espécime"
					},
					{
						"key": "@SpeAdv@",
						"value": "Solicitação de amostra antecipada"
					},
					{
						"key": "@SpeAli@",
						"value": "ID da Alíquota"
					},
					{
						"key": "@SpeAlr@",
						"value": "Amostra já recebida"
					},
					{
						"key": "@SpeApi@",
						"value": "Painel API / ID"
					},
					{
						"key": "@SpeApp@",
						"value": "Aprovar ou rejeitar a análise do espécime submetido"
					},
					{
						"key": "@SpeAss@",
						"value": "Avalie cada amostra recebida no lote e, em seguida, passe para a próxima"
					},
					{
						"key": "@SpeAssA@",
						"value": "Avalie o crescimento de cada cultura no lote e, em seguida, passe para a próxima"
					},
					{
						"key": "@SpeBat@",
						"value": "Amostras do dia 0 do processo em lote"
					},
					{
						"key": "@SpeBatA@",
						"value": "Amostras do dia 1 do processo em lote"
					},
					{
						"key": "@SpeCan@",
						"value": "Cancelar um pedido de amostra"
					},
					{
						"key": "@SpeCanA@",
						"value": "Cancelar pedido"
					},
					{
						"key": "@SpeCanB@",
						"value": "Cancelar o pedido de um espécime"
					},
					{
						"key": "@SpeCol@",
						"value": "A data de coleta deve ser inserida"
					},
					{
						"key": "@SpeColA@",
						"value": "A hora da coleta deve ser inserida"
					},
					{
						"key": "@SpeColB@",
						"value": "Data / hora da coleta"
					},
					{
						"key": "@SpeColC@",
						"value": "Data de Coleta"
					},
					{
						"key": "@SpeColD@",
						"value": "Hora de coleta"
					},
					{
						"key": "@SpeCre@",
						"value": "Crie e gerencie culturas novas e existentes"
					},
					{
						"key": "@SpeCreA@",
						"value": "Criar e gerenciar registros de espécimes novos e existentes"
					},
					{
						"key": "@SpeCul@",
						"value": "Detalhes da cultura"
					},
					{
						"key": "@SpeCulA@",
						"value": "Registro de cultura"
					},
					{
						"key": "@SpeCulB@",
						"value": "Culturas"
					},
					{
						"key": "@SpeDay@",
						"value": "Leitura de banco do dia 1"
					},
					{
						"key": "@SpeDec@",
						"value": "A decisão é necessária"
					},
					{
						"key": "@SpeDel@",
						"value": "Apagar Cultura"
					},
					{
						"key": "@SpeDet@",
						"value": "Detalhes da amostra"
					},
					{
						"key": "@SpeDir@",
						"value": "Testes Diretos"
					},
					{
						"key": "@SpeEdi@",
						"value": "Editar uma cultura existente"
					},
					{
						"key": "@SpeEdiA@",
						"value": "Editar Cultura"
					},
					{
						"key": "@SpeEnt@",
						"value": "Insira um comentário de amostra"
					},
					{
						"key": "@SpeEntA@",
						"value": "Insira a hora de recebimento"
					},
					{
						"key": "@SpeEntB@",
						"value": "Insira o peso da amostra"
					},
					{
						"key": "@SpeEntC@",
						"value": "Insira o motivo da rejeição"
					},
					{
						"key": "@SpeEntD@",
						"value": "Insira detalhes adicionais"
					},
					{
						"key": "@SpeEntE@",
						"value": "Insira o código de barras existente, se houver"
					},
					{
						"key": "@SpeEntF@",
						"value": "Insira a porcentagem de ID"
					},
					{
						"key": "@SpeEntG@",
						"value": "Insira a data de um resultado positivo"
					},
					{
						"key": "@SpeEntH@",
						"value": "Insira a hora de um resultado positivo"
					},
					{
						"key": "@SpeEntI@",
						"value": "Informe o horário de coleta"
					},
					{
						"key": "@SpeEsb@",
						"value": "ESBL"
					},
					{
						"key": "@SpeExi@",
						"value": "Código de Barras Existente"
					},
					{
						"key": "@SpeFul@",
						"value": "Pesquisa Completa de Organismo"
					},
					{
						"key": "@SpeGro@",
						"value": "O crescimento deve ser inserido"
					},
					{
						"key": "@SpeGroA@",
						"value": "Crescimento?"
					},
					{
						"key": "@SpeId@",
						"value": "Perfil de ID"
					},
					{
						"key": "@SpeIdA@",
						"value": "% EU IRIA"
					},
					{
						"key": "@SpeIde@",
						"value": "Método de Identificação"
					},
					{
						"key": "@SpeImm@",
						"value": "Ação imediata"
					},
					{
						"key": "@SpeInd@",
						"value": "Indique a condição da amostra e o motivo da rejeição, se não estiver implícito"
					},
					{
						"key": "@SpeIndA@",
						"value": "Indique o que você está prestes a fazer com o espécime"
					},
					{
						"key": "@SpeMan@",
						"value": "Gerenciar Culturas"
					},
					{
						"key": "@SpeManA@",
						"value": "Gerenciar testes diretos"
					},
					{
						"key": "@SpeManB@",
						"value": "Gerenciar testes diretos"
					},
					{
						"key": "@SpeManC@",
						"value": "Gerenciar espécimes para paciente"
					},
					{
						"key": "@SpeManD@",
						"value": "Gerenciar espécimes"
					},
					{
						"key": "@SpeNex@",
						"value": "Próximo espécime"
					},
					{
						"key": "@SpeOrg@",
						"value": "O organismo deve ser inserido"
					},
					{
						"key": "@SpeOrgA@",
						"value": "Identidade do Organismo"
					},
					{
						"key": "@SpeOth@",
						"value": "Outra informação"
					},
					{
						"key": "@SpePos@",
						"value": "Data / Hora Positiva"
					},
					{
						"key": "@SpePosA@",
						"value": "Data Positiva"
					},
					{
						"key": "@SpePosB@",
						"value": "Tempo Positivo"
					},
					{
						"key": "@SpePro@",
						"value": "Amostras do processo de hoje"
					},
					{
						"key": "@SpeProA@",
						"value": "Forneça detalhes relativos ao espécime recebido fisicamente"
					},
					{
						"key": "@SpeProB@",
						"value": "Forneça detalhes de coleta de espécimes específicos do paciente"
					},
					{
						"key": "@SpeProC@",
						"value": "Forneça quaisquer qualificações, orientações ou informações adicionais de importância"
					},
					{
						"key": "@SpeProD@",
						"value": "Fornece características do espécime"
					},
					{
						"key": "@SpeProE@",
						"value": "Forneça detalhes do método de identificação"
					},
					{
						"key": "@SpeProF@",
						"value": "Forneça detalhes do organismo identificado ou sendo examinado para"
					},
					{
						"key": "@SpeProG@",
						"value": "Forneça esses detalhes restantes, conforme aplicável"
					},
					{
						"key": "@SpeProH@",
						"value": "Forneça o identificador da Alíquota, se necessário"
					},
					{
						"key": "@SpeProI@",
						"value": "Fornece informações clínicas precisas no momento da coleta"
					},
					{
						"key": "@SpeProJ@",
						"value": "Forneça datas e horários importantes"
					},
					{
						"key": "@SpeRea@",
						"value": "Razão para rejeição"
					},
					{
						"key": "@SpeRec@",
						"value": "A data de recebimento deve ser inserida"
					},
					{
						"key": "@SpeRecA@",
						"value": "A hora de recebimento deve ser inserida"
					},
					{
						"key": "@SpeRecB@",
						"value": "Condição recebida"
					},
					{
						"key": "@SpeRecB@",
						"value": "A condição recebida deve ser inserida"
					},
					{
						"key": "@SpeRecC@",
						"value": "Data / hora de recebimento"
					},
					{
						"key": "@SpeRecD@",
						"value": "Data de Recebimento"
					},
					{
						"key": "@SpeRecE@",
						"value": "Tempo Recebido"
					},
					{
						"key": "@SpeRej@",
						"value": "Rejeitar espécime"
					},
					{
						"key": "@SpeSel@",
						"value": "Selecione a data de recebimento"
					},
					{
						"key": "@SpeSelA@",
						"value": "Selecione a condição da amostra"
					},
					{
						"key": "@SpeSelB@",
						"value": "Selecione a aparência do espécime"
					},
					{
						"key": "@SpeSelC@",
						"value": "Selecione o tipo de amostra"
					},
					{
						"key": "@SpeSelD@",
						"value": "Selecione o local da amostra"
					},
					{
						"key": "@SpeSelE@",
						"value": "Selecione Índice de Perfil Analítico usado"
					},
					{
						"key": "@SpeSelF@",
						"value": "Selecione o perfil de ID"
					},
					{
						"key": "@SpeSelG@",
						"value": "Selecione o nome do organismo"
					},
					{
						"key": "@SpeSelH@",
						"value": "Selecione a extensão do crescimento"
					},
					{
						"key": "@SpeSelI@",
						"value": "Selecione a data de coleta"
					},
					{
						"key": "@SpeSelJ@",
						"value": "Selecione o organismo de cultura"
					},
					{
						"key": "@SpeSer@",
						"value": "Serótipo"
					},
					{
						"key": "@SpeSerA@",
						"value": "Perfil de sorotipo"
					},
					{
						"key": "@SpeSpe@",
						"value": "Nível 1 de aprovação de espécime"
					},
					{
						"key": "@SpeSpeA@",
						"value": "Nível 2 de aprovação de espécime"
					},
					{
						"key": "@SpeSpeB@",
						"value": "Tipo de amostra"
					},
					{
						"key": "@SpeSpeC@",
						"value": "Local da amostra"
					},
					{
						"key": "@SpeSpeD@",
						"value": "Peso da amostra"
					},
					{
						"key": "@SpeSpeE@",
						"value": "Condição do espécime"
					},
					{
						"key": "@SpeSpeF@",
						"value": "Aparência do espécime"
					},
					{
						"key": "@SpeSpeG@",
						"value": "Registro de espécime"
					},
					{
						"key": "@SpeSpeH@",
						"value": "Ação do espécime"
					},
					{
						"key": "@SpeSpeI@",
						"value": "Aprovação de espécime"
					},
					{
						"key": "@SpeSpeJ@",
						"value": "Atributos de espécime"
					},
					{
						"key": "@SpeSpeK@",
						"value": "Especifique o sorotipo se apropriado"
					},
					{
						"key": "@SpeSpeL@",
						"value": "Especifique o perfil do serótipo"
					},
					{
						"key": "@SpeSpeM@",
						"value": "Tempo da amostra"
					},
					{
						"key": "@SpeSpeN@",
						"value": "Tipo de amostra deve ser inserido"
					},
					{
						"key": "@SpeSpeO@",
						"value": "O local da amostra deve ser inserido"
					},
					{
						"key": "@SpeSub@",
						"value": "Enviar espécime para aprovação"
					},
					{
						"key": "@SpeSubA@",
						"value": "Enviar confirmação"
					},
					{
						"key": "@SpeTod@",
						"value": "Amostras de hoje"
					},
					{
						"key": "@SpeVie@",
						"value": "Ver cultura"
					},
					{
						"key": "@SpeYou@",
						"value": "Você está prestes a enviar um registro de amostra para aprovação. Por favor confirme."
					},
					{
						"key": "@TabA@",
						"value": "Um id de lista deve ser fornecido"
					},
					{
						"key": "@TabAdd@",
						"value": "Adicionar entrada de tabela"
					},
					{
						"key": "@TabAddA@",
						"value": "Adicionar uma nova entrada de tabela para a tabela atualmente selecionada"
					},
					{
						"key": "@TabDel@",
						"value": "Excluir entrada de tabela"
					},
					{
						"key": "@TabDelA@",
						"value": "Apaga a entrada da tabela seleccionada"
					},
					{
						"key": "@TabEdi@",
						"value": "Editar entrada de tabela"
					},
					{
						"key": "@TabMan@",
						"value": "Manter tabelas"
					},
					{
						"key": "@TabManA@",
						"value": "Manter o conteúdo de cada tabela de referência"
					},
					{
						"key": "@TabTab@",
						"value": "Manter tabelas"
					},
					{
						"key": "@TesAdd@",
						"value": "Adicionar Padrão de Teste"
					},
					{
						"key": "@TesAddA@",
						"value": "Adicionar um novo padrão de teste"
					},
					{
						"key": "@TesAddB@",
						"value": "Configurações Gerais"
					},
					{
						"key": "@TesAddC@",
						"value": "Dê um nome ao padrão de teste e defina sua aplicabilidade"
					},
					{
						"key": "@TesAddD@",
						"value": "Adicionar Antibiótico"
					},
					{
						"key": "@TesAddE@",
						"value": "Adicionar Antibióticos"
					},
					{
						"key": "@TesAddF@",
						"value": "Especifique o antibiótico, a dosagem e o método de teste para cada componente do padrão de teste"
					},
					{
						"key": "@TesAfb@",
						"value": "Quantidade AFB"
					},
					{
						"key": "@TesCar@",
						"value": "Realizar testes diretos"
					},
					{
						"key": "@TesCel@",
						"value": "Teste de contagem de células"
					},
					{
						"key": "@TesDel@",
						"value": "Excluir teste de amostra"
					},
					{
						"key": "@TesDelA@",
						"value": "Excluir padrão de teste"
					},
					{
						"key": "@TesDelB@",
						"value": "Exclua um padrão de teste"
					},
					{
						"key": "@TesEdi@",
						"value": "Editar antibióticos"
					},
					{
						"key": "@TesEdiA@",
						"value": "Editar os componentes do antibiótico do padrão de teste"
					},
					{
						"key": "@TesEdiB@",
						"value": "Editar configurações gerais"
					},
					{
						"key": "@TesEdiC@",
						"value": "Edite as características gerais do padrão de teste"
					},
					{
						"key": "@TesEdiD@",
						"value": "Editar Padrão de Teste"
					},
					{
						"key": "@TesEnt@",
						"value": "Insira os detalhes para um teste de contagem de células"
					},
					{
						"key": "@TesEntA@",
						"value": "Insira testes diretos"
					},
					{
						"key": "@TesEntB@",
						"value": "Insira testes diretos para este espécime"
					},
					{
						"key": "@TesEntC@",
						"value": "Insira os detalhes para um teste de coloração de Gram"
					},
					{
						"key": "@TesEntD@",
						"value": "Insira os detalhes para um teste de tinta da Índia"
					},
					{
						"key": "@TesEntE@",
						"value": "Insira os detalhes para um teste de preparação úmida"
					},
					{
						"key": "@TesEntF@",
						"value": "Insira os detalhes para um teste de coloração ZN"
					},
					{
						"key": "@TesEpi@",
						"value": "Epi Cells"
					},
					{
						"key": "@TesGra@",
						"value": "Teste de Coloração de Gram"
					},
					{
						"key": "@TesInd@",
						"value": "Teste de tinta da Índia"
					},
					{
						"key": "@TesIndA@",
						"value": "Resultado da tinta da Índia"
					},
					{
						"key": "@TesMan@",
						"value": "Gerenciar seleções de teste"
					},
					{
						"key": "@TesManA@",
						"value": "Gerenciar padrões de teste"
					},
					{
						"key": "@TesManB@",
						"value": "Gerenciar lista de padrões de teste"
					},
					{
						"key": "@TesMon@",
						"value": "Mononuclear"
					},
					{
						"key": "@TesPar@",
						"value": "Parasitas"
					},
					{
						"key": "@TesParA@",
						"value": "Parasita"
					},
					{
						"key": "@TesPat@",
						"value": "Nome do padrão de teste"
					},
					{
						"key": "@TesPol@",
						"value": "Polimorfonuclear"
					},
					{
						"key": "@TesPos@",
						"value": "Resultado positivo"
					},
					{
						"key": "@TesRbc@",
						"value": "RBC (mm ^ 3)"
					},
					{
						"key": "@TesRbcA@",
						"value": "RBC Qualitativo"
					},
					{
						"key": "@TesRbcB@",
						"value": "RBC"
					},
					{
						"key": "@TesTes@",
						"value": "Seleções de teste para o espécime selecionado"
					},
					{
						"key": "@TesUpd@",
						"value": "Atualizar seleção de teste para espécime"
					},
					{
						"key": "@TesWbc@",
						"value": "WBC (mm ^ 3)"
					},
					{
						"key": "@TesWbcA@",
						"value": "WBC qualitativo"
					},
					{
						"key": "@TesWbcB@",
						"value": "WBC"
					},
					{
						"key": "@TesWet@",
						"value": "Teste de preparação úmida"
					},
					{
						"key": "@TesZns@",
						"value": "Teste de Mancha ZN"
					},
					{
						"key": "@UseAdd@",
						"value": "Adicionar um novo usuário"
					},
					{
						"key": "@UseAddA@",
						"value": "Adicionar usuário"
					},
					{
						"key": "@UseAddB@",
						"value": "Adicionar propriedades do usuário"
					},
					{
						"key": "@UseClo@",
						"value": "Clone User"
					},
					{
						"key": "@UseCre@",
						"value": "Criar e gerenciar usuários novos e existentes"
					},
					{
						"key": "@UseDel@",
						"value": "Deletar usuário"
					},
					{
						"key": "@UseEdi@",
						"value": "Editar um usuário existente"
					},
					{
						"key": "@UseEdiA@",
						"value": "Editar usuário"
					},
					{
						"key": "@UseEit@",
						"value": "Um Laboratório ou Organização deve ser inserido"
					},
					{
						"key": "@UseEma@",
						"value": "O email"
					},
					{
						"key": "@UseFir@",
						"value": "Primeiro nome"
					},
					{
						"key": "@UseLas@",
						"value": "Último nome"
					},
					{
						"key": "@UseMan@",
						"value": "Gerenciar usuários"
					},
					{
						"key": "@UsePas@",
						"value": "A senha deve ser inserida"
					},
					{
						"key": "@UsePasA@",
						"value": "Senha"
					},
					{
						"key": "@UseRol@",
						"value": "As funções devem ser inseridas"
					},
					{
						"key": "@UseSel@",
						"value": "Selecione Laboratório"
					},
					{
						"key": "@UseSelA@",
						"value": "Selecione a Organização"
					},
					{
						"key": "@UseUse@",
						"value": "O nome de usuário deve ser inserido"
					},
					{
						"key": "@UseUseA@",
						"value": "O usuário não pode pertencer a uma organização e a um laboratório"
					},
					{
						"key": "@UseUseB@",
						"value": "Nome do usuário"
					},
					{
						"key": "@UseUseC@",
						"value": "Propriedades do usuário"
					},
					{
						"key": "@ZonDia@",
						"value": "Diâmetro da Zona"
					}
				]
				""";
        }
    }
}
