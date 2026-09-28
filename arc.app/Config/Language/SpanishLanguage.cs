using arc.app.Common;

namespace arc.app.Config.Language
{
    internal class SpanishLanguage : IDefinition
    {
        public string Get()
        {
            return """
				[
					{
						"key": "@AleAdd@",
						"value": "Agregar alerta"
					},
					{
						"key": "@AleAddChildTag@",
						"value": "Agregar etiqueta secundaria"
					},
					{
						"key": "@AleTagHasChildren@",
						"value": "No se puede eliminar una etiqueta que tiene etiquetas secundarias"
					},
					{
						"key": "@AleAle@",
						"value": "Nombre de alerta"
					},
					{
						"key": "@AleCre@",
						"value": "Cree y administre alertas nuevas y existentes"
					},
					{
						"key": "@AleDel@",
						"value": "Eliminar alerta"
					},
					{
						"key": "@AleEdi@",
						"value": "Editar alerta"
					},
					{
						"key": "@AleMan@",
						"value": "Administrar alertas"
					},
					{
						"key": "@GenTagH@",
						"value": "Etiqueta principal"
					},
					{
						"key": "@GenTagI@",
						"value": "Seleccionar etiqueta principal"
					},
					{
						"key": "@AstAdd@",
						"value": "Agregar o actualizar datos de AST para una cultura"
					},
					{
						"key": "@AstAnt@",
						"value": "Pruebas de susceptibilidad a los antimicrobianos"
					},
					{
						"key": "@AstCre@",
						"value": "Crear y administrar resultados de AST"
					},
					{
						"key": "@AstDis@",
						"value": "Pruebas de disco"
					},
					{
						"key": "@AstDos@",
						"value": "La dosis debe ser distinta de cero"
					},
					{
						"key": "@AstDup@",
						"value": "Antibiótico duplicado"
					},
					{
						"key": "@AstEnt@",
						"value": "Ingrese los antibióticos con los que se analizarán, por tipo de método AST, y los resultados de la prueba, si están disponibles"
					},
					{
						"key": "@AstGui@",
						"value": "Deben especificarse las pautas"
					},
					{
						"key": "@AstMan@",
						"value": "Administrar AST"
					},
					{
						"key": "@AstMea@",
						"value": "Medida fuera de rango"
					},
					{
						"key": "@AstMicMea@",
						"value": "La medición MIC debe incluir un número válido"
					},
					{
						"key": "@AstMic@",
						"value": "Pruebas MIC"
					},
					{
						"key": "@AstPen@",
						"value": "AST pendiente"
					},
					{
						"key": "@AstPre@",
						"value": "Seleccionar presencia"
					},
					{
						"key": "@AstRes@",
						"value": "Resultados AST"
					},
					{
						"key": "@AstTes@",
						"value": "Pruebas AST"
					},
					{
						"key": "@AstTesA@",
						"value": "Patrón de prueba AST"
					},
					{
						"key": "@AstTesB@",
						"value": "Patrón de prueba"
					},
					{
						"key": "@AstTesC@",
						"value": "Seleccionar patrón de prueba"
					},
					{
						"key": "@AstUse@",
						"value": "Usar patrón para pruebas de disco"
					},
					{
						"key": "@AstUseA@",
						"value": "Usar patrón para pruebas de tira"
					},
					{
						"key": "@BatPro@",
						"value": "Procesamiento de muchas muestras como un lote"
					},
					{
						"key": "@BatRec@",
						"value": "Registros publicados con éxito"
					},
					{
						"key": "@BreA@",
						"value": "Se debe ingresar un tipo de muestra"
					},
					{
						"key": "@BreAA@",
						"value": "Se debe ingresar un host"
					},
					{
						"key": "@BreAB@",
						"value": "Se debe ingresar un método de prueba"
					},
					{
						"key": "@BreAC@",
						"value": "Debe introducirse una susceptibilidad"
					},
					{
						"key": "@BreAD@",
						"value": "Se debe ingresar un nombre de patrón de prueba"
					},
					{
						"key": "@BreAE@",
						"value": "Debe introducirse una dosis"
					},
					{
						"key": "@BreAF@",
						"value": "Se debe ingresar una fuente de punto de interrupción"
					},
					{
						"key": "@BreAG@",
						"value": "Deben introducirse consideraciones especiales"
					},
					{
						"key": "@BreAdd@",
						"value": "Agregar punto de interrupción"
					},
					{
						"key": "@BreAddA@",
						"value": "Agregar un nuevo punto de interrupción"
					},
					{
						"key": "@BreAddB@",
						"value": "Agregar método de prueba"
					},
					{
						"key": "@BreAddC@",
						"value": "Agregar anfitrión"
					},
					{
						"key": "@BreAddD@",
						"value": "Agregar un nuevo método de prueba"
					},
					{
						"key": "@BreAddE@",
						"value": "Agregar un nuevo anfitrión"
					},
					{
						"key": "@BreAddF@",
						"value": "Agregar susceptibilidad"
					},
					{
						"key": "@BreAddG@",
						"value": "Agrega una nueva susceptibilidad"
					},
					{
						"key": "@BreAddH@",
						"value": "Establecer criterios"
					},
					{
						"key": "@BreAddI@",
						"value": "Definir las otras características para las que se aplicará el punto de interrupción."
					},
					{
						"key": "@BreAddJ@",
						"value": "Definir punto de interrupción"
					},
					{
						"key": "@BreAddK@",
						"value": "Establecer el rango de medición para cada determinación de susceptibilidad"
					},
					{
						"key": "@BreAddL@",
						"value": "Editar punto de interrupción"
					},
					{
						"key": "@BreAddM@",
						"value": "Editar criterios"
					},
					{
						"key": "@BreAddN@",
						"value": "Edite las otras características para las que se aplicará el punto de interrupción"
					},
					{
						"key": "@BreAn@",
						"value": "Se debe ingresar un antibiótico"
					},
					{
						"key": "@BreAnA@",
						"value": "Se debe ingresar un pedido"
					},
					{
						"key": "@BreBre@",
						"value": "Puntos de interrupción"
					},
					{
						"key": "@BreCan@",
						"value": "No se puede eliminar porque este host está en uso"
					},
					{
						"key": "@BreCanA@",
						"value": "No se puede eliminar porque este método de prueba está en uso"
					},
					{
						"key": "@BreCanB@",
						"value": "No se puede eliminar porque esta susceptibilidad está en uso"
					},
					{
						"key": "@BreDel@",
						"value": "Eliminar punto de interrupción"
					},
					{
						"key": "@BreDelA@",
						"value": "Eliminar un punto de interrupción"
					},
					{
						"key": "@BreDelB@",
						"value": "Eliminar método de prueba"
					},
					{
						"key": "@BreDelC@",
						"value": "Eliminar host"
					},
					{
						"key": "@BreDelD@",
						"value": "Eliminar un método de prueba existente"
					},
					{
						"key": "@BreDelE@",
						"value": "Eliminar susceptibilidad"
					},
					{
						"key": "@BreDelF@",
						"value": "Eliminar una susceptibilidad existente"
					},
					{
						"key": "@BreEdi@",
						"value": "Editar punto de interrupción"
					},
					{
						"key": "@BreEdiA@",
						"value": "Editar un punto de interrupción existente"
					},
					{
						"key": "@BreEdiB@",
						"value": "Editar método de prueba"
					},
					{
						"key": "@BreEdiC@",
						"value": "Editar anfitrión"
					},
					{
						"key": "@BreEdiD@",
						"value": "Editar un método de prueba existente"
					},
					{
						"key": "@BreEdiE@",
						"value": "Editar un host existente"
					},
					{
						"key": "@BreEdiF@",
						"value": "Editar susceptibilidad"
					},
					{
						"key": "@BreEdiG@",
						"value": "Editar una susceptibilidad existente"
					},
					{
						"key": "@BreHos@",
						"value": "Lista de anfitriones"
					},
					{
						"key": "@BreMan@",
						"value": "Gestionar puntos de interrupción"
					},
					{
						"key": "@BreManA@",
						"value": "Administrar lista de puntos de interrupción"
					},
					{
						"key": "@BreNod@",
						"value": "Sin datos de punto de interrupción"
					},
					{
						"key": "@BreOve@",
						"value": "Zonas superpuestas"
					},
					{
						"key": "@BreSou@",
						"value": "Fuente"
					},
					{
						"key": "@BreSouA@",
						"value": "Sin fuente de punto de interrupción"
					},
					{
						"key": "@BreSpe@",
						"value": "Consideraciones Especiales"
					},
					{
						"key": "@BreSpeA@",
						"value": "Especial"
					},
					{
						"key": "@BreTes@",
						"value": "Método de prueba"
					},
					{
						"key": "@BreTesA@",
						"value": "Métodos de prueba"
					},
					{
						"key": "@BreThe@",
						"value": "El anfitrión que desea cambiar debe estar seleccionado"
					},
					{
						"key": "@BreTheA@",
						"value": "Debe seleccionar el método de prueba que desea cambiar"
					},
					{
						"key": "@BreTheB@",
						"value": "Debe seleccionar la susceptibilidad que desea cambiar"
					},
					{
						"key": "@BreThi@",
						"value": "Este método de prueba ya existe"
					},
					{
						"key": "@BreThiA@",
						"value": "Este anfitrión ya existe"
					},
					{
						"key": "@BreThiB@",
						"value": "Esta susceptibilidad ya existe"
					},
					{
						"key": "@BreLin@",
						"value": "Líneas de punto de corte"
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
						"value": "Historial de aprobación"
					},
					{
						"key": "@BreAddApp@",
						"value": "Añadir aprobación/rechazo"
					},
					{
						"key": "@BreAddAppB@",
						"value": "Añadir aprobación/rechazo"
					},
					{
						"key": "@BreDatRec@",
						"value": "Fecha registrada"
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
						"value": "Se debe ingresar un nombre de lista"
					},
					{
						"key": "@CodAA@",
						"value": "Se debe ingresar una entrada personalizada"
					},
					{
						"key": "@CodAB@",
						"value": "Debe seleccionarse una lista de codificación"
					},
					{
						"key": "@CodAC@",
						"value": "Se debe ingresar un código"
					},
					{
						"key": "@CodAdd@",
						"value": "Agregar lista de codificación"
					},
					{
						"key": "@CodAddA@",
						"value": "Agregar entrada personalizada de codificación"
					},
					{
						"key": "@CodAddB@",
						"value": "Agregar una entrada personalizada a la lista de organismos"
					},
					{
						"key": "@CodAddC@",
						"value": "Agregar una nueva lista de codificación"
					},
					{
						"key": "@CodAss@",
						"value": "Asignar código"
					},
					{
						"key": "@CodAssA@",
						"value": "Asignar un código al organismo seleccionado"
					},
					{
						"key": "@CodDel@",
						"value": "Eliminar lista de codificación"
					},
					{
						"key": "@CodDelA@",
						"value": "Eliminar una lista de coing existente y todo su contenido"
					},
					{
						"key": "@CodDelB@",
						"value": "Eliminar organismo"
					},
					{
						"key": "@CodDelC@",
						"value": "Eliminar un organismo o una entrada personalizada de la lista de codificación"
					},
					{
						"key": "@CodEdi@",
						"value": "Editar entrada personalizada de codificación"
					},
					{
						"key": "@CodGra@",
						"value": "Gramo"
					},
					{
						"key": "@CodLis@",
						"value": "Lista de organismos que coinciden con los criterios de búsqueda"
					},
					{
						"key": "@CodNew@",
						"value": "Nueva entrada"
					},
					{
						"key": "@CodSco@",
						"value": "Seleccione el ámbito del organismo en el que se aplicará la configuración"
					},
					{
						"key": "@CodScoA@",
						"value": "Edite el alcance del organismo en el que se aplicará la configuración"
					},
					{
						"key": "@CodSel@",
						"value": "Seleccione el organismo a utilizar"
					},
					{
						"key": "@CodSelA@",
						"value": "Seleccionar género"
					},
					{
						"key": "@CodSelB@",
						"value": "Seleccionar especies"
					},
					{
						"key": "@CodSelC@",
						"value": "Seleccionar serotipo"
					},
					{
						"key": "@CodThe@",
						"value": "El organismo no existe"
					},
					{
						"key": "@CodTheA@",
						"value": "El organismo ya ha sido agregado a esta lista de codificación."
					},
					{
						"key": "@CodThi@",
						"value": "Esta lista de codificación ya existe"
					},
					{
						"key": "@CodThiA@",
						"value": "Esta lista de codificación no se puede eliminar"
					},
					{
						"key": "@CodThiB@",
						"value": "Esta entrada ya existe en la lista"
					},
					{
						"key": "@ConAdd@",
						"value": "Agregar formulario"
					},
					{
						"key": "@ConCre@",
						"value": "Cree y administre definiciones de formularios nuevas y existentes"
					},
					{
						"key": "@ConDel@",
						"value": "Eliminar formulario"
					},
					{
						"key": "@ConEdi@",
						"value": "Editar formulario"
					},
					{
						"key": "@ConFor@",
						"value": "Nombre del formulario"
					},
					{
						"key": "@ConMan@",
						"value": "Administrar definiciones de formularios"
					},
					{
						"key": "@CusAdd@",
						"value": "Información clínica adicional"
					},
					{
						"key": "@CusAnt@",
						"value": "Antibióticos en las últimas 24 horas"
					},
					{
						"key": "@CusCli@",
						"value": "Información clínica"
					},
					{
						"key": "@CusEnt@",
						"value": "Ingrese información clínica"
					},
					{
						"key": "@CusEntA@",
						"value": "Ingrese información clínica adicional"
					},
					{
						"key": "@CusEntB@",
						"value": "Ingrese más información"
					},
					{
						"key": "@CusEntC@",
						"value": "Ingrese los detalles de la muestra"
					},
					{
						"key": "@CusFur@",
						"value": "Más información"
					},
					{
						"key": "@CusMel@",
						"value": "Cultivo de melioidosis"
					},
					{
						"key": "@CusReq@",
						"value": "Solicitar pruebas de cultivo"
					},
					{
						"key": "@CusSpe@",
						"value": "Información de la muestra"
					},
					{
						"key": "@CusSus@",
						"value": "Sospecha de diagnóstico clínico"
					},
					{
						"key": "@CusTem@",
						"value": "Temperatura. En las últimas 24 horas"
					},
					{
						"key": "@ExpCon@",
						"value": "Configurar y emitir una exportación DHIS2"
					},
					{
						"key": "@ExpCon@",
						"value": "Configurar y emitir exportaciones de datos"
					},
					{
						"key": "@ExpConA@",
						"value": "Configurar y emitir una exportación WHONET"
					},
					{
						"key": "@ExpDhi@",
						"value": "Exportación DHIS2"
					},
					{
						"key": "@ExpGen@",
						"value": "Genere y guarde un archivo de exportación DHIS2"
					},
					{
						"key": "@ExpGenA@",
						"value": "Genere y guarde un archivo de exportación WHONET"
					},
					{
						"key": "@ExpMan@",
						"value": "Gestionar exportaciones"
					},
					{
						"key": "@ExpPro@",
						"value": "Lista de perfiles"
					},
					{
						"key": "@ExpProD@",
						"value": "Descripción"
					},
					{
						"key": "@ExpProE@",
						"value": "Activada"
					},
					{
						"key": "@ExpProMap@",
						"value": "Gestionar asignación"
					},
					{
						"key": "@ExpProMapDesc@",
						"value": "Cree una asignación JSON o XML para los campos incluidos en este perfil de exportación."
					},
					{
						"key": "@ExpProMapSav@",
						"value": "Guardar la asignación JSON o XML de un perfil de exportación"
					},
					{
						"key": "@ExpProMapFmt@",
						"value": "Formato de salida"
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
						"value": "Añadir atributo"
					},
					{
						"key": "@ExpProMapAddArr@",
						"value": "Añadir arreglo"
					},
					{
						"key": "@ExpProMapAttrName@",
						"value": "Nombre del atributo"
					},
					{
						"key": "@ExpProMapField@",
						"value": "Campo"
					},
					{
						"key": "@ExpProMapArrName@",
						"value": "Nombre del arreglo"
					},
					{
						"key": "@ExpProMapArrType@",
						"value": "Tipo de arreglo"
					},
					{
						"key": "@ExpProMapArrSpecimen@",
						"value": "Muestras"
					},
					{
						"key": "@ExpProMapArrCulture@",
						"value": "Cultivos / aislados"
					},
					{
						"key": "@ExpProMapArrGrid@",
						"value": "Cuadrícula (resultados de pruebas)"
					},
					{
						"key": "@ExpProMapArrAst@",
						"value": "Resultados AST"
					},
					{
						"key": "@ExpProMapPreview@",
						"value": "Vista previa"
					},
					{
						"key": "@ExpProMapNoFields@",
						"value": "Aún no hay campos en el perfil de exportación. Añada campos al perfil antes de crear una asignación."
					},
					{
						"key": "@ExpProMapNoSpecimen@",
						"value": "Los arreglos de muestras requieren al menos un campo de paciente en el perfil de exportación."
					},
					{
						"key": "@ExpProMapNoCulture@",
						"value": "Los arreglos de cultivos requieren al menos un campo de cultivo o aislado en el perfil de exportación."
					},
					{
						"key": "@ExpProMapGridReq@",
						"value": "Los arreglos de cuadrícula sólo pueden añadirse cuando hay un campo de cuadrícula seleccionado."
					},
					{
						"key": "@ExpProMapNoAst@",
						"value": "Los arreglos AST requieren al menos un campo de la tabla AST en el perfil de exportación."
					},
					{
						"key": "@ExpProMapAttrNameErr@",
						"value": "El nombre del atributo es obligatorio."
					},
					{
						"key": "@ExpProMapFieldErr@",
						"value": "Es obligatorio seleccionar un campo."
					},
					{
						"key": "@ExpProMapUniqueRef@",
						"value": "Referencia única"
					},
					{
						"key": "@ExpProMapUniqueRefErr@",
						"value": "Solo un atributo puede marcarse como referencia única para campos de {table}."
					},
					{
						"key": "@ExpProMapUniqueRefNoField@",
						"value": "Seleccione un campo antes de marcar Referencia única."
					},
					{
						"key": "@ExpProMapSavedTitle@",
						"value": "Asignación guardada"
					},
					{
						"key": "@ExpProMapSavedDesc@",
						"value": "Se han guardado los cambios en la asignación del perfil de exportación."
					},
					{
						"key": "@ExpProMapDelNode@",
						"value": "Quitar"
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
						"value": "Fecha de modificación"
					},
					{
						"key": "@ExpProN@",
						"value": "Nombre"
					},
					{
						"key": "@ExpRet@",
						"value": "Recuperar exportación"
					},
					{
						"key": "@ExpTyp@",
						"value": "Tipo de exportación"
					},
					{
						"key": "@ExpWho@",
						"value": "Exportación WHONET"
					},
					{
						"key": "@ExpYou@",
						"value": "Debes ingresar una fecha de inicio"
					},
					{
						"key": "@GenA@",
						"value": "Debe introducirse un valor"
					},
					{
						"key": "@GenAct@",
						"value": "Acción"
					},
					{
						"key": "@GenAdd@",
						"value": "Notas adicionales"
					},
					{
						"key": "@GenAddA@",
						"value": "Adicional"
					},
					{
						"key": "@GenAddB@",
						"value": "Agregar un mensaje para mostrarlo en un lugar destacado"
					},
					{
						"key": "@GenAddC@",
						"value": "Agregar comentario"
					},
					{
						"key": "@GenAddD@",
						"value": "Añadir lista"
					},
					{
						"key": "@GenAddE@",
						"value": "Añadir entrada"
					},
					{
						"key": "@GenAdm@",
						"value": "Administración"
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
						"value": "Dosis de antibiótico"
					},
					{
						"key": "@GenBac@",
						"value": "atrás"
					},
					{
						"key": "@GenBacA@",
						"value": "Bacterias"
					},
					{
						"key": "@GenBlo@",
						"value": "Sangre"
					},
					{
						"key": "@GenBre@",
						"value": "Puntos de interrupción"
					},
					{
						"key": "@GenCan@",
						"value": "Cancelar"
					},
					{
						"key": "@GenCas@",
						"value": "Emitir"
					},
					{
						"key": "@GenClo@",
						"value": "Cerrar"
					},
					{
						"key": "@GenCo1@",
						"value": "Contraer menú"
					},
					{
						"key": "@GenCod@",
						"value": "Codificación"
					},
					{
						"key": "@GenCodA@",
						"value": "Código"
					},
					{
						"key": "@GenCodB@",
						"value": "Lista de codificación"
					},
					{
						"key": "@GenCom@",
						"value": "Comentario 1"
					},
					{
						"key": "@GenComA@",
						"value": "Comentario 2"
					},
					{
						"key": "@GenComB@",
						"value": "Comentario"
					},
					{
						"key": "@GenCon@",
						"value": "Configuración"
					},
					{
						"key": "@GenConA@",
						"value": "Número de contacto"
					},
					{
						"key": "@GenCry@",
						"value": "Cristal"
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
						"value": "Fecha Agregada"
					},
					{
						"key": "@GenDatA@",
						"value": "Fecha de finalización"
					},
					{
						"key": "@GenDec@",
						"value": "Decisión"
					},
					{
						"key": "@GenDef@",
						"value": "Defecto"
					},
					{
						"key": "@GenDel@",
						"value": "Eliminar prueba"
					},
					{
						"key": "@GenDelA@",
						"value": "Eliminar lista"
					},
					{
						"key": "@GenDelB@",
						"value": "Eliminar la entrada"
					},
					{
						"key": "@GenDelC@",
						"value": "Borrar"
					},
					{
						"key": "@GenDep@",
						"value": "Departamento"
					},
					{
						"key": "@GenDes@",
						"value": "Descripción"
					},
					{
						"key": "@GenDia@",
						"value": "Diagnóstico"
					},
					{
						"key": "@GenDiaA@",
						"value": "Diario"
					},
					{
						"key": "@GenDis@",
						"value": "Mostrar en informe"
					},
					{
						"key": "@GenDisA@",
						"value": "Mostrar cultura en el informe"
					},
					{
						"key": "@GenDos@",
						"value": "Dosis"
					},
					{
						"key": "@GenEdi@",
						"value": "Editar prueba"
					},
					{
						"key": "@GenEdiA@",
						"value": "Editar entrada"
					},
					{
						"key": "@GenEna@",
						"value": "Activado"
					},
					{
						"key": "@GenEnaA@",
						"value": "Habilitar"
					},
					{
						"key": "@GenEnd@",
						"value": "Fecha final"
					},
					{
						"key": "@GenEnt@",
						"value": "Ingrese al modo de pantalla completa"
					},
					{
						"key": "@GenEntA@",
						"value": "Ingresar comentario"
					},
					{
						"key": "@GenEntB@",
						"value": "Entrada"
					},
					{
						"key": "@GenEntC@",
						"value": "Ingrese al menos un carácter"
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
						"value": "Salida"
					},
					{
						"key": "@GenExiA@",
						"value": "Salir del modo de pantalla completa"
					},
					{
						"key": "@GenExp@",
						"value": "Exportaciones"
					},
					{
						"key": "@GenFam@",
						"value": "Familia"
					},
					{
						"key": "@GenFil@",
						"value": "Filtrar"
					},
					{
						"key": "@GenFilA@",
						"value": "Filtrar ajustes preestablecidos"
					},
					{
						"key": "@GenFilB@",
						"value": "Filtrar por palabra clave"
					},
					{
						"key": "@GenFin@",
						"value": "Terminar"
					},
					{
						"key": "@GenFir@",
						"value": "Primera aprobación"
					},
					{
						"key": "@GenFirA@",
						"value": "Segunda aprobación"
					},
					{
						"key": "@GenFor@",
						"value": "Formularios"
					},
					{
						"key": "@GenFou@",
						"value": "Fundar"
					},
					{
						"key": "@GenFul@",
						"value": "Pantalla completa"
					},
					{
						"key": "@GenFun@",
						"value": "Hongo"
					},
					{
						"key": "@GenGen@",
						"value": "Género"
					},
					{
						"key": "@GenGri@",
						"value": "Vista en cuadrícula"
					},
					{
						"key": "@GenGro@",
						"value": "Agrupamiento"
					},
					{
						"key": "@GenHel@",
						"value": "Ayudar"
					},
					{
						"key": "@GenHom@",
						"value": "Hogar"
					},
					{
						"key": "@GenHos@",
						"value": "Anfitrión"
					},
					{
						"key": "@GenId@",
						"value": "Se debe ingresar el ID"
					},
					{
						"key": "@GenInc@",
						"value": "Incluir en informe"
					},
					{
						"key": "@GenIss@",
						"value": "Fecha de emisión"
					},
					{
						"key": "@GenKey@",
						"value": "Llave"
					},
					{
						"key": "@GenLab@",
						"value": "Laboratorios"
					},
					{
						"key": "@GenLabA@",
						"value": "Laboratorio"
					},
					{
						"key": "@GenLan@",
						"value": "Idioma"
					},
					{
						"key": "@GenLis@",
						"value": "Liza"
					},
					{
						"key": "@GenLisA@",
						"value": "Lista de nombres"
					},
					{
						"key": "@GenLoc@",
						"value": "Localización"
					},
					{
						"key": "@GenMea@",
						"value": "Medición"
					},
					{
						"key": "@GenMes@",
						"value": "Mensaje"
					},
					{
						"key": "@GenMet@",
						"value": "Método"
					},
					{
						"key": "@GenMon@",
						"value": "Vigilancia"
					},
					{
						"key": "@GenNam@",
						"value": "Nombre"
					},
					{
						"key": "@GenNex@",
						"value": "próximo"
					},
					{
						"key": "@GenNexA@",
						"value": "Siguiente página"
					},
					{
						"key": "@GenOrd@",
						"value": "Pedido"
					},
					{
						"key": "@GenOrg@",
						"value": "Organizaciones de clientes"
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
						"value": "Organización del cliente"
					},
					{
						"key": "@GenOrgD@",
						"value": "Lista de organismos"
					},
					{
						"key": "@GenPar@",
						"value": "Padre"
					},
					{
						"key": "@GenParA@",
						"value": "Entrada de los padres"
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
						"key": "@GenPriB@",
						"value": "Impreso"
					},
					{
						"key": "@GenPriC@",
						"value": "Impresión"
					},
					{
						"key": "@GenPub@",
						"value": "Publicado"
					},
					{
						"key": "@GenPubA@",
						"value": "Publicar"
					},
					{
						"key": "@GenQua@",
						"value": "Cantidad"
					},
					{
						"key": "@GenRea@",
						"value": "Razón"
					},
					{
						"key": "@GenRep@",
						"value": "Informes por lotes"
					},
					{
						"key": "@GenRepA@",
						"value": "Reporte"
					},
					{
						"key": "@GenRes@",
						"value": "Fecha de resultado"
					},
					{
						"key": "@GenResA@",
						"value": "Se debe ingresar el resultado"
					},
					{
						"key": "@GenRol@",
						"value": "Roles"
					},
					{
						"key": "@GenSav@",
						"value": "Ahorrar"
					},
					{
						"key": "@GenSea@",
						"value": "Buscar"
					},
					{
						"key": "@GenSee@",
						"value": "Visto"
					},
					{
						"key": "@GenSel@",
						"value": "Seleccionar fecha de inicio"
					},
					{
						"key": "@GenSelA@",
						"value": "Seleccionar fecha de finalización"
					},
					{
						"key": "@GenSelB@",
						"value": "Seleccionar ubicación"
					},
					{
						"key": "@GenSelC@",
						"value": "Seleccionar departamento"
					},
					{
						"key": "@GenSelD@",
						"value": "Seleccionar diagnóstico"
					},
					{
						"key": "@GenSelE@",
						"value": "Seleccione un comentario aplicable"
					},
					{
						"key": "@GenSelF@",
						"value": "Seleccionar organismo"
					},
					{
						"key": "@GenSelG@",
						"value": "Seleccionar padre"
					},
					{
						"key": "@GenSelH@",
						"value": "Establecer el alcance del organismo"
					},
					{
						"key": "@GenSelI@",
						"value": "Editar el alcance del organismo"
					},
					{
						"key": "@GenSelJ@",
						"value": "Seleccionar departamento"
					},
					{
						"key": "@GenSelK@",
						"value": "Selecciona valor"
					},
					{
						"key": "@GenSelL@",
						"value": "registros seleccionados"
					},
					{
						"key": "@GenSer@",
						"value": "Serotipo"
					},
					{
						"key": "@GenSet@",
						"value": "Ajustes"
					},
					{
						"key": "@GenSpe@",
						"value": "Especímenes"
					},
					{
						"key": "@GenSpeA@",
						"value": "Especificar"
					},
					{
						"key": "@GenSpeB@",
						"value": "Especies"
					},
					{
						"key": "@GenSta@",
						"value": "Estado"
					},
					{
						"key": "@GenStaA@",
						"value": "Estado"
					},
					{
						"key": "@GenStaB@",
						"value": "Fecha de inicio"
					},
					{
						"key": "@GenSub@",
						"value": "Enviar"
					},
					{
						"key": "@GenSubA@",
						"value": "Subespecies"
					},
					{
						"key": "@GenSus@",
						"value": "Susceptibilidad"
					},
					{
						"key": "@GenSys@",
						"value": "Configuración del sistema"
					},
					{
						"key": "@GenTab@",
						"value": "Mesas"
					},
					{
						"key": "@GenTes@",
						"value": "Pruebas"
					},
					{
						"key": "@GenTesA@",
						"value": "Tipo de prueba"
					},
					{
						"key": "@GenTesB@",
						"value": "Patrones de prueba"
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
						"value": "Escribe"
					},
					{
						"key": "@GenUse@",
						"value": "Usuarios"
					},
					{
						"key": "@GenUseA@",
						"value": "Usuario"
					},
					{
						"key": "@GenVal@",
						"value": "Valor"
					},
					{
						"key": "@GenVie@",
						"value": "Puntos de vista"
					},
					{
						"key": "@GenVieA@",
						"value": "Ver / Actualizar"
					},
					{
						"key": "@GenVieB@",
						"value": "Ver prueba"
					},
					{
						"key": "@GenVieC@",
						"value": "Vista"
					},
					{
						"key": "@GenVieD@",
						"value": "Ver los informes"
					},
					{
						"key": "@GenWar@",
						"value": "Departamento"
					},
					{
						"key": "@GenWor@",
						"value": "Flujos de trabajo"
					},
					{
						"key": "@GenYea@",
						"value": "Levaduras"
					},
					{
						"key": "@LabA@",
						"value": "Se debe seleccionar una traducción"
					},
					{
						"key": "@LabAA@",
						"value": "Debe seleccionarse una lista de codificación"
					},
					{
						"key": "@LabAdd@",
						"value": "Agregar laboratorio"
					},
					{
						"key": "@LabBre@",
						"value": "Listas de puntos de interrupción para usar"
					},
					{
						"key": "@LabBreA@",
						"value": "Listas de puntos de interrupción"
					},
					{
						"key": "@LabCre@",
						"value": "Crear y gestionar laboratorios nuevos y existentes."
					},
					{
						"key": "@LabDel@",
						"value": "Eliminar laboratorio"
					},
					{
						"key": "@LabEdi@",
						"value": "Editar los detalles del laboratorio"
					},
					{
						"key": "@LabEdiA@",
						"value": "Editar laboratorio"
					},
					{
						"key": "@LabEnt@",
						"value": "Ingrese el nombre del laboratorio"
					},
					{
						"key": "@LabEntA@",
						"value": "Ingrese los detalles del nuevo laboratorio"
					},
					{
						"key": "@LabLab@",
						"value": "Laboratorio"
					},
					{
						"key": "@LabLabA@",
						"value": "Nombre del laboratorio"
					},
					{
						"key": "@LabLabB@",
						"value": "Debe ingresar el nombre del laboratorio"
					},
					{
						"key": "@LabMan@",
						"value": "Gestionar laboratorios"
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
						"value": "Listas de patrones de prueba para usar"
					},
					{
						"key": "@LabTesA@",
						"value": "Listas de patrones de prueba"
					},
					{
						"key": "@LanA@",
						"value": "Se debe ingresar una traducción de la que se va a copiar"
					},
					{
						"key": "@LanAA@",
						"value": "Se debe seleccionar una traducción para eliminar"
					},
					{
						"key": "@LanAdd@",
						"value": "Agregar traducción"
					},
					{
						"key": "@LanCre@",
						"value": "Crea y gestiona traducciones"
					},
					{
						"key": "@LanCreA@",
						"value": "Crear una nueva traducción"
					},
					{
						"key": "@LanDel@",
						"value": "Eliminar traducción"
					},
					{
						"key": "@LanDelA@",
						"value": "Eliminar una traducción existente"
					},
					{
						"key": "@LanEdiA@",
						"value": "Editar una entrada de traducción"
					},
					{
						"key": "@LanEnt@",
						"value": "Ingrese el nombre de la traducción"
					},
					{
						"key": "@LanMan@",
						"value": "Gestionar traducciones"
					},
					{
						"key": "@LanSel@",
						"value": "Seleccionar traducción"
					},
					{
						"key": "@LanThi@",
						"value": "Esta traducción ya existe"
					},
					{
						"key": "@LanTra@",
						"value": "Traducciones"
					},
					{
						"key": "@LanTraA@",
						"value": "Traducción"
					},
					{
						"key": "@LanTraB@",
						"value": "Traducción para copiar"
					},
					{
						"key": "@LanTraC@",
						"value": "Se debe ingresar el nombre de la traducción"
					},
					{
						"key": "@LanTraD@",
						"value": "La traducción está en uso dentro de una organización cliente."
					},
					{
						"key": "@LanTraE@",
						"value": "La traducción se utiliza en un laboratorio."
					},
					{
						"key": "@MonMon@",
						"value": "Supervisar todos los eventos"
					},
					{
						"key": "@MonMonA@",
						"value": "Monitorear todos los eventos que cambian los datos en el sistema"
					},
					{
						"key": "@MonVie@",
						"value": "Ver detalles de eventos de monitoreo"
					},
					{
						"key": "@MonVieA@",
						"value": "Ver detalles"
					},
					{
						"key": "@MonVieB@",
						"value": "Ver detalles (sin procesar)"
					},
					{
						"key": "@MonVieC@",
						"value": "Ver detalles del evento"
					},
					{
						"key": "@MonVieD@",
						"value": "Ver detalles del evento"
					},
					{
						"key": "@OrgA@",
						"value": "Se debe ingresar un nombre de organismo"
					},
					{
						"key": "@OrgAdd@",
						"value": "Agregar organización cliente"
					},
					{
						"key": "@OrgAddA@",
						"value": "Agregar organismo"
					},
					{
						"key": "@OrgAll@",
						"value": "Todos los organismos"
					},
					{
						"key": "@OrgB@",
						"value": "Se debe ingresar una identificación de organismo"
					},
					{
						"key": "@OrgCre@",
						"value": "Crear y administrar organizaciones de clientes nuevas y existentes."
					},
					{
						"key": "@OrgDel@",
						"value": "Eliminar organización cliente"
					},
					{
						"key": "@OrgDelA@",
						"value": "Eliminar organismo"
					},
					{
						"key": "@OrgEdi@",
						"value": "Editar la organización del cliente"
					},
					{
						"key": "@OrgEdiA@",
						"value": "Editar los detalles de una organización cliente existente"
					},
					{
						"key": "@OrgEdiB@",
						"value": "Editar organismo"
					},
					{
						"key": "@OrgEnt@",
						"value": "Ingrese los detalles de la organización del nuevo cliente"
					},
					{
						"key": "@OrgEntA@",
						"value": "Ingrese el nombre de la organización del cliente"
					},
					{
						"key": "@OrgMan@",
						"value": "Gestionar organizaciones de clientes"
					},
					{
						"key": "@OrgManA@",
						"value": "Administrar listas de organismos"
					},
					{
						"key": "@OrgManB@",
						"value": "Gestionar listas utilizadas para organismos."
					},
					{
						"key": "@OrgOrg@",
						"value": "Nombre de la organización del cliente"
					},
					{
						"key": "@OrgOrgA@",
						"value": "Se debe ingresar el nombre de la organización del cliente"
					},
					{
						"key": "@OrgPar@",
						"value": "Organización matriz"
					},
					{
						"key": "@OrgSel@",
						"value": "Seleccionar organización principal"
					},
					{
						"key": "@PatA@",
						"value": "Debe ingresar un comentario"
					},
					{
						"key": "@PatAdd@",
						"value": "Agregar un nuevo paciente"
					},
					{
						"key": "@PatAddA@",
						"value": "Agregar paciente"
					},
					{
						"key": "@PatAddB@",
						"value": "Dirección del paciente"
					},
					{
						"key": "@PatAddC@",
						"value": "Agregar un comentario a un paciente"
					},
					{
						"key": "@PatAdm@",
						"value": "Fecha de admisión"
					},
					{
						"key": "@PatAge@",
						"value": "La edad"
					},
					{
						"key": "@PatCli@",
						"value": "Número de contacto clínico"
					},
					{
						"key": "@PatCre@",
						"value": "Cree y administre registros de pacientes nuevos y existentes."
					},
					{
						"key": "@PatDat@",
						"value": "Fecha de cumpleaños"
					},
					{
						"key": "@PatDel@",
						"value": "Eliminar paciente"
					},
					{
						"key": "@PatDet@",
						"value": "Detalles del paciente"
					},
					{
						"key": "@PatDis@",
						"value": "Distrito"
					},
					{
						"key": "@PatEdi@",
						"value": "Editar los detalles del paciente"
					},
					{
						"key": "@PatEdiA@",
						"value": "Editar paciente"
					},
					{
						"key": "@PatEdiB@",
						"value": "Editar los detalles de la dirección del paciente"
					},
					{
						"key": "@PatEnt@",
						"value": "Ingrese un comentario de paciente"
					},
					{
						"key": "@PatEntA@",
						"value": "Ingrese el nombre"
					},
					{
						"key": "@PatEntB@",
						"value": "Ingrese el apellido"
					},
					{
						"key": "@PatEntC@",
						"value": "Ingrese la edad"
					},
					{
						"key": "@PatEntD@",
						"value": "Ingrese el número de teléfono"
					},
					{
						"key": "@PatEntE@",
						"value": "Ingrese los detalles del nuevo paciente"
					},
					{
						"key": "@PatEntF@",
						"value": "Ingrese la referencia del paciente"
					},
					{
						"key": "@PatEntG@",
						"value": "Ingrese el número de contacto clínico"
					},
					{
						"key": "@PatEntH@",
						"value": "Ingrese valor"
					},
					{
						"key": "@PatFin@",
						"value": "Encontrar paciente por palabra clave"
					},
					{
						"key": "@PatFir@",
						"value": "Primer nombre"
					},
					{
						"key": "@PatGen@",
						"value": "Se debe ingresar el género"
					},
					{
						"key": "@PatGenA@",
						"value": "Género"
					},
					{
						"key": "@PatMan@",
						"value": "Gestionar pacientes"
					},
					{
						"key": "@PatPat@",
						"value": "Se debe ingresar la referencia del paciente"
					},
					{
						"key": "@PatPatA@",
						"value": "Nombre del paciente"
					},
					{
						"key": "@PatPatB@",
						"value": "Ref paciente"
					},
					{
						"key": "@PatPatC@",
						"value": "Historia clínica del paciente"
					},
					{
						"key": "@PatPatD@",
						"value": "Detalles del paciente"
					},
					{
						"key": "@PatPatE@",
						"value": "Comentarios del paciente"
					},
					{
						"key": "@PatPatF@",
						"value": "Búsqueda de pacientes"
					},
					{
						"key": "@PatPatG@",
						"value": "Resultados de la búsqueda de pacientes"
					},
					{
						"key": "@PatPatH@",
						"value": "Detalles de la colección de pacientes"
					},
					{
						"key": "@PatPatI@",
						"value": "Paciente"
					},
					{
						"key": "@PatPatJ@",
						"value": "Ubicación del paciente"
					},
					{
						"key": "@PatPro@",
						"value": "Provincia"
					},
					{
						"key": "@PatSea@",
						"value": "Buscar un paciente"
					},
					{
						"key": "@PatSel@",
						"value": "Seleccionar fecha de nacimiento"
					},
					{
						"key": "@PatSelA@",
						"value": "Seleccione género"
					},
					{
						"key": "@PatSelB@",
						"value": "Seleccionar provincia"
					},
					{
						"key": "@PatSelC@",
						"value": "Seleccionar distrito"
					},
					{
						"key": "@PatSelD@",
						"value": "Seleccionar subdistrito"
					},
					{
						"key": "@PatSelE@",
						"value": "Seleccione un paciente existente o defina uno nuevo"
					},
					{
						"key": "@PatSelF@",
						"value": "Seleccionar fecha de admisión"
					},
					{
						"key": "@PatSub@",
						"value": "Subdistrito"
					},
					{
						"key": "@PatSur@",
						"value": "Se debe ingresar el apellido"
					},
					{
						"key": "@PatSurA@",
						"value": "Apellido"
					},
					{
						"key": "@PatTel@",
						"value": "Número de teléfono"
					},
					{
						"key": "@PatVie@",
						"value": "Ver paciente"
					},
					{
						"key": "@RepA@",
						"value": "Se debe ingresar un nombre de informe"
					},
					{
						"key": "@RepAnt@",
						"value": "Antibiótico"
					},
					{
						"key": "@RepApp@",
						"value": "Apariencia"
					},
					{
						"key": "@RepAur@",
						"value": "Auramina"
					},
					{
						"key": "@RepBat@",
						"value": "Impresión por lotes"
					},
					{
						"key": "@RepBatA@",
						"value": "Publicación por lotes"
					},
					{
						"key": "@RepCel@",
						"value": "Conteo de células"
					},
					{
						"key": "@RepCul@",
						"value": "Resultado de la cultura"
					},
					{
						"key": "@RepDip@",
						"value": "Varilla graduada"
					},
					{
						"key": "@RepFin@",
						"value": "Reporte final"
					},
					{
						"key": "@RepGra@",
						"value": "Tinción de Gram"
					},
					{
						"key": "@RepIfy@",
						"value": "Si desea hablar sobre el resultado o el tratamiento, llame al Laboratorio de Microbiología."
					},
					{
						"key": "@RepInd@",
						"value": "Tinta china"
					},
					{
						"key": "@RepMic@",
						"value": "Informe del laboratorio de microbiología"
					},
					{
						"key": "@RepPre@",
						"value": "Resultados de precultivo"
					},
					{
						"key": "@RepPreA@",
						"value": "Fecha de precultivo"
					},
					{
						"key": "@RepPri@",
						"value": "Imprimir / publicar informe de muestra"
					},
					{
						"key": "@RepPriA@",
						"value": "Imprimir y publicar informe"
					},
					{
						"key": "@RepPriB@",
						"value": "Imprima un lote de informes"
					},
					{
						"key": "@RepPriC@",
						"value": "Publica un lote de informes"
					},
					{
						"key": "@RepPub@",
						"value": "Publicar informe"
					},
					{
						"key": "@RepPatLoc@",
						"value": "Ubicación del paciente"
					},
					{
						"key": "@RepRef@",
						"value": "Paciente"
					},
					{
						"key": "@RepRep@",
						"value": "Historial de informes"
					},
					{
						"key": "@RepRes@",
						"value": "Resultado"
					},
					{
						"key": "@RepSen@",
						"value": "Sensibilidad"
					},
					{
						"key": "@RepSpe@",
						"value": "Informe de muestra"
					},
					{
						"key": "@RepWet@",
						"value": "Preparación húmeda"
					},
					{
						"key": "@RepZns@",
						"value": "Tinción ZN"
					},
					{
						"key": "@RolAdd@",
						"value": "Agregar rol"
					},
					{
						"key": "@RolAddA@",
						"value": "Añadiendo un nuevo rol"
					},
					{
						"key": "@RolCan@",
						"value": "El nombre del rol debe ser único"
					},
					{
						"key": "@RolClo@",
						"value": "Clonar un rol"
					},
					{
						"key": "@RolCloA@",
						"value": "Rol de clonación"
					},
					{
						"key": "@RolCloB@",
						"value": "Clonar un rol existente en el sistema. Esto copiará los detalles del permiso del rol existente al nuevo rol"
					},
					{
						"key": "@RolCon@",
						"value": "Configurar aspectos generales del rol"
					},
					{
						"key": "@RolConA@",
						"value": "Se debe ingresar la configuración"
					},
					{
						"key": "@RolConB@",
						"value": "Configurar permisos de eventos para este rol"
					},
					{
						"key": "@RolConC@",
						"value": "Configurar permisos de menú para este rol"
					},
					{
						"key": "@RolCre@",
						"value": "Crear y administrar roles nuevos y existentes"
					},
					{
						"key": "@RolDel@",
						"value": "Eliminar un rol"
					},
					{
						"key": "@RolDelA@",
						"value": "Eliminar rol"
					},
					{
						"key": "@RolDes@",
						"value": "Se debe ingresar la descripción del rol"
					},
					{
						"key": "@RolEdi@",
						"value": "Editar rol existente"
					},
					{
						"key": "@RolEdiA@",
						"value": "Editar rol"
					},
					{
						"key": "@RolEdiB@",
						"value": "Editar los aspectos generales de un rol existente"
					},
					{
						"key": "@RolEna@",
						"value": "Habilitado debe ingresarse"
					},
					{
						"key": "@RolEve@",
						"value": "Permisos de eventos"
					},
					{
						"key": "@RolLab@",
						"value": "Administrador de laboratorio"
					},
					{
						"key": "@RolMan@",
						"value": "Administrar roles"
					},
					{
						"key": "@RolManA@",
						"value": "Administrar permisos de eventos"
					},
					{
						"key": "@RolManB@",
						"value": "Administrar permisos de menú"
					},
					{
						"key": "@RolMen@",
						"value": "Permisos de menú"
					},
					{
						"key": "@RolNam@",
						"value": "Se debe ingresar el nombre del rol"
					},
					{
						"key": "@RolNew@",
						"value": "Detalles del nuevo rol"
					},
					{
						"key": "@RolOrg@",
						"value": "Administrador de la organización"
					},
					{
						"key": "@RolRem@",
						"value": "Eliminar una función del sistema"
					},
					{
						"key": "@RolRol@",
						"value": "Descripción del rol"
					},
					{
						"key": "@RolRolA@",
						"value": "Nombre de rol"
					},
					{
						"key": "@RolRolB@",
						"value": "Registro de roles"
					},
					{
						"key": "@RolRolC@",
						"value": "Roles"
					},
					{
						"key": "@RolRolD@",
						"value": "Papel para clonar"
					},
					{
						"key": "@RolSel@",
						"value": "Seleccionar roles"
					},
					{
						"key": "@RolUpd@",
						"value": "Actualizar permisos de eventos"
					},
					{
						"key": "@RolUpdA@",
						"value": "Actualizar permisos del menú"
					},
					{
						"key": "@SerA@",
						"value": "Debe introducirse un nombre de serotipo"
					},
					{
						"key": "@SerAdd@",
						"value": "Agregar serotipo"
					},
					{
						"key": "@SerDel@",
						"value": "Eliminar serotipo"
					},
					{
						"key": "@SpcA@",
						"value": "Se debe ingresar un nombre de especie"
					},
					{
						"key": "@SpcAdd@",
						"value": "Agregar especies"
					},
					{
						"key": "@SpcDel@",
						"value": "Eliminar especies"
					},
					{
						"key": "@SpeA@",
						"value": "Debe ingresar un comentario"
					},
					{
						"key": "@SpeAcc@",
						"value": "Número de acceso"
					},
					{
						"key": "@SpeAck@",
						"value": "Acuse recibo de la muestra"
					},
					{
						"key": "@SpeAckA@",
						"value": "Acuse de recibo"
					},
					{
						"key": "@SpeAckB@",
						"value": "Acuse de recibo de la muestra"
					},
					{
						"key": "@SpeAdd@",
						"value": "Agregar una nueva cultura"
					},
					{
						"key": "@SpeAddA@",
						"value": "Agregar registro para la muestra recibida"
					},
					{
						"key": "@SpeAddB@",
						"value": "Agregar nueva muestra de forma remota"
					},
					{
						"key": "@SpeAddC@",
						"value": "Agregar nueva muestra"
					},
					{
						"key": "@SpeAddD@",
						"value": "Agregar cultura"
					},
					{
						"key": "@SpeAddE@",
						"value": "Agregar muestra"
					},
					{
						"key": "@SpeAddF@",
						"value": "Orientación adicional"
					},
					{
						"key": "@SpeAddG@",
						"value": "Agregue el motivo de la aprobación o el rechazo"
					},
					{
						"key": "@SpeAddH@",
						"value": "Agregar un comentario a una muestra"
					},
					{
						"key": "@SpeAdi@",
						"value": "Debe seleccionarse un diagnóstico"
					},
					{
						"key": "@SpeAdv@",
						"value": "Solicitud de microbiología"
					},
					{
						"key": "@SpeAli@",
						"value": "ID de alícuota"
					},
					{
						"key": "@SpeAlr@",
						"value": "Muestra ya recibida"
					},
					{
						"key": "@SpeAn@",
						"value": "Debe seleccionarse un departamento"
					},
					{
						"key": "@SpeAnA@",
						"value": "Debe seleccionarse un laboratorio"
					},
					{
						"key": "@SpeApi@",
						"value": "Panel API / ID"
					},
					{
						"key": "@SpeApp@",
						"value": "Aprobar o rechazar el análisis de muestras enviado"
					},
					{
						"key": "@SpeAss@",
						"value": "Evalúe cada muestra recibida en el lote, luego pase al siguiente"
					},
					{
						"key": "@SpeAssA@",
						"value": "Evalúe el crecimiento de cada cultivo del lote y luego pase al siguiente"
					},
					{
						"key": "@SpeBat@",
						"value": "Muestras del día 0 del proceso por lotes"
					},
					{
						"key": "@SpeBatA@",
						"value": "Procesar por lotes las muestras del día 1"
					},
					{
						"key": "@SpeBlo@",
						"value": "Hemocultivo Sangre y peso de la botella"
					},
					{
						"key": "@SpeBot@",
						"value": "Peso de la botella de hemocultivo solamente"
					},
					{
						"key": "@SpeCan@",
						"value": "Cancelar una solicitud de muestra"
					},
					{
						"key": "@SpeCanA@",
						"value": "Cancelar petición"
					},
					{
						"key": "@SpeCanB@",
						"value": "Cancelar la solicitud de una muestra"
					},
					{
						"key": "@SpeCol@",
						"value": "Se debe ingresar la fecha de recolección"
					},
					{
						"key": "@SpeColA@",
						"value": "Se debe ingresar la hora de recolección"
					},
					{
						"key": "@SpeColB@",
						"value": "Fecha / hora de recogida"
					},
					{
						"key": "@SpeColC@",
						"value": "Fecha de colección"
					},
					{
						"key": "@SpeColD@",
						"value": "Tiempo de recogida"
					},
					{
						"key": "@SpeCre@",
						"value": "Crear y gestionar culturas nuevas y existentes."
					},
					{
						"key": "@SpeCreA@",
						"value": "Cree y administre registros de muestras nuevos y existentes."
					},
					{
						"key": "@SpeCul@",
						"value": "Detalles de la cultura"
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
						"key": "@SpeCulC@",
						"value": "Pruebas de cultivo"
					},
					{
						"key": "@SpeDay@",
						"value": "Lectura de banco del día 1"
					},
					{
						"key": "@SpeDec@",
						"value": "Se requiere decisión"
					},
					{
						"key": "@SpeDel@",
						"value": "Eliminar cultura"
					},
					{
						"key": "@SpeDet@",
						"value": "Detalles de la muestra"
					},
					{
						"key": "@SpeDir@",
						"value": "Pruebas directas"
					},
					{
						"key": "@SpeEdi@",
						"value": "Editar una cultura existente"
					},
					{
						"key": "@SpeEdiA@",
						"value": "Editar cultura"
					},
					{
						"key": "@SpeEdiB@",
						"value": "Editar muestra"
					},
					{
						"key": "@SpeEnt@",
						"value": "Ingrese un comentario de muestra"
					},
					{
						"key": "@SpeEntA@",
						"value": "Ingrese la hora recibida"
					},
					{
						"key": "@SpeEntB@",
						"value": "Ingrese el peso"
					},
					{
						"key": "@SpeEntC@",
						"value": "Ingrese el motivo del rechazo"
					},
					{
						"key": "@SpeEntD@",
						"value": "Ingrese detalles adicionales"
					},
					{
						"key": "@SpeEntE@",
						"value": "Ingrese el código de barras existente, si está presente"
					},
					{
						"key": "@SpeEntF@",
						"value": "Ingrese el porcentaje de identificación"
					},
					{
						"key": "@SpeEntG@",
						"value": "Ingrese la fecha de un resultado positivo"
					},
					{
						"key": "@SpeEntH@",
						"value": "Ingrese el tiempo de un resultado positivo"
					},
					{
						"key": "@SpeEntI@",
						"value": "Ingrese la hora de recolección"
					},
					{
						"key": "@SpeEsb@",
						"value": "BLEE"
					},
					{
						"key": "@SpeExi@",
						"value": "Código de barras existente"
					},
					{
						"key": "@SpeFul@",
						"value": "Búsqueda completa de organismos"
					},
					{
						"key": "@SpeGro@",
						"value": "Se debe ingresar el crecimiento"
					},
					{
						"key": "@SpeGroA@",
						"value": "¿Crecimiento?"
					},
					{
						"key": "@SpeId@",
						"value": "Perfil de identificación"
					},
					{
						"key": "@SpeIdA@",
						"value": "% IDENTIFICACIÓN"
					},
					{
						"key": "@SpeIde@",
						"value": "Método de identificación"
					},
					{
						"key": "@SpeImm@",
						"value": "Acción inmediata"
					},
					{
						"key": "@SpeInd@",
						"value": "Indique el estado de la muestra y el motivo del rechazo si no está implícito"
					},
					{
						"key": "@SpeIndA@",
						"value": "Indique lo que va a hacer con la muestra."
					},
					{
						"key": "@SpeMan@",
						"value": "Gestionar culturas"
					},
					{
						"key": "@SpeManA@",
						"value": "Gestionar pruebas directas"
					},
					{
						"key": "@SpeManB@",
						"value": "Gestionar pruebas directas"
					},
					{
						"key": "@SpeManC@",
						"value": "Gestionar muestras para el paciente"
					},
					{
						"key": "@SpeManD@",
						"value": "Manejar muestras"
					},
					{
						"key": "@SpeNex@",
						"value": "Siguiente espécimen"
					},
					{
						"key": "@SpeOrg@",
						"value": "Se debe ingresar el organismo"
					},
					{
						"key": "@SpeOrgA@",
						"value": "Identidad del organismo"
					},
					{
						"key": "@SpeOth@",
						"value": "Otra información"
					},
					{
						"key": "@SpePos@",
						"value": "Fecha / hora positivas"
					},
					{
						"key": "@SpePosA@",
						"value": "Fecha positiva"
					},
					{
						"key": "@SpePosB@",
						"value": "Tiempo positivo"
					},
					{
						"key": "@SpePro@",
						"value": "Procesar muestras de hoy"
					},
					{
						"key": "@SpeProA@",
						"value": "Proporcione detalles relacionados con la muestra tal como se recibió físicamente."
					},
					{
						"key": "@SpeProB@",
						"value": "Proporcione los detalles de la colección de muestras específicas del paciente."
					},
					{
						"key": "@SpeProC@",
						"value": "Proporcionar cualquier calificación, orientación o información adicional de importancia."
					},
					{
						"key": "@SpeProD@",
						"value": "Proporcionar características de la muestra"
					},
					{
						"key": "@SpeProE@",
						"value": "Proporcionar detalles del método de identificación"
					},
					{
						"key": "@SpeProF@",
						"value": "Proporcione detalles del organismo identificado o que se está examinando para"
					},
					{
						"key": "@SpeProG@",
						"value": "Proporcione estos detalles restantes, según corresponda"
					},
					{
						"key": "@SpeProH@",
						"value": "Proporcione un identificador de alícuota si es necesario"
					},
					{
						"key": "@SpeProI@",
						"value": "Proporcionar información clínica precisa en el momento de la recopilación."
					},
					{
						"key": "@SpeProJ@",
						"value": "Proporcione fechas y horas importantes"
					},
					{
						"key": "@SpeRea@",
						"value": "Motivo del rechazo"
					},
					{
						"key": "@SpeRec@",
						"value": "Se debe ingresar la fecha de recepción"
					},
					{
						"key": "@SpeRecA@",
						"value": "Se debe ingresar la hora recibida"
					},
					{
						"key": "@SpeRecB@",
						"value": "Condición recibida"
					},
					{
						"key": "@SpeRecB@",
						"value": "Se debe ingresar la condición recibida"
					},
					{
						"key": "@SpeRecC@",
						"value": "Fecha / hora de recepción"
					},
					{
						"key": "@SpeRecD@",
						"value": "Fecha recibida"
					},
					{
						"key": "@SpeRecE@",
						"value": "Hora recibida"
					},
					{
						"key": "@SpeRej@",
						"value": "Rechazar muestra"
					},
					{
						"key": "@SpeSel@",
						"value": "Seleccione la fecha de recepción"
					},
					{
						"key": "@SpeSelA@",
						"value": "Seleccione la condición de la muestra"
					},
					{
						"key": "@SpeSelB@",
						"value": "Seleccione la apariencia de la muestra"
					},
					{
						"key": "@SpeSelC@",
						"value": "Seleccione el tipo de muestra"
					},
					{
						"key": "@SpeSelD@",
						"value": "Seleccione el sitio de la muestra"
					},
					{
						"key": "@SpeSelE@",
						"value": "Seleccione el índice de perfil analítico utilizado"
					},
					{
						"key": "@SpeSelF@",
						"value": "Seleccionar perfil de identificación"
					},
					{
						"key": "@SpeSelG@",
						"value": "Seleccione el nombre del organismo"
					},
					{
						"key": "@SpeSelH@",
						"value": "Seleccione el grado de crecimiento"
					},
					{
						"key": "@SpeSelI@",
						"value": "Seleccionar fecha de recolección"
					},
					{
						"key": "@SpeSelJ@",
						"value": "Seleccionar organismo de cultivo"
					},
					{
						"key": "@SpeSer@",
						"value": "Serotipo"
					},
					{
						"key": "@SpeSerA@",
						"value": "Perfil de serotipo"
					},
					{
						"key": "@SpeSpe@",
						"value": "Nivel de aprobación de la muestra 1"
					},
					{
						"key": "@SpeSpeA@",
						"value": "Nivel de aprobación de la muestra 2"
					},
					{
						"key": "@SpeSpeB@",
						"value": "Tipo de muestra"
					},
					{
						"key": "@SpeSpeC@",
						"value": "Sitio de la muestra"
					},
					{
						"key": "@SpeSpeD@",
						"value": "Peso de la muestra"
					},
					{
						"key": "@SpeSpeE@",
						"value": "Condición de la muestra"
					},
					{
						"key": "@SpeSpeF@",
						"value": "Apariencia de la muestra"
					},
					{
						"key": "@SpeSpeG@",
						"value": "Registro de muestras"
					},
					{
						"key": "@SpeSpeH@",
						"value": "Acción de la muestra"
					},
					{
						"key": "@SpeSpeI@",
						"value": "Aprobación de la muestra"
					},
					{
						"key": "@SpeSpeJ@",
						"value": "Atributos de la muestra"
					},
					{
						"key": "@SpeSpeK@",
						"value": "Especifique el serotipo si es apropiado"
					},
					{
						"key": "@SpeSpeL@",
						"value": "Especificar perfil de serotipo"
					},
					{
						"key": "@SpeSpeM@",
						"value": "Tiempos de muestra"
					},
					{
						"key": "@SpeSpeN@",
						"value": "Se debe ingresar el tipo de muestra"
					},
					{
						"key": "@SpeSpeO@",
						"value": "Se debe ingresar al sitio de la muestra"
					},
					{
						"key": "@SpeSub@",
						"value": "Envíe la muestra para su aprobación"
					},
					{
						"key": "@SpeSubA@",
						"value": "Enviar confirmación"
					},
					{
						"key": "@SpeSubDat@",
						"value": "Fecha de envío"
					},
					{
						"key": "@SpeAppDat@",
						"value": "Fecha de aprobación"
					},
					{
						"key": "@SpeTod@",
						"value": "Muestras de hoy"
					},
					{
						"key": "@SpeVie@",
						"value": "Ver cultura"
					},
					{
						"key": "@SpeYou@",
						"value": "Está a punto de enviar un registro de muestra para su aprobación. Por favor confirmar."
					},
					{
						"key": "@TabA@",
						"value": "Se debe proporcionar una identificación de lista"
					},
					{
						"key": "@TabAdd@",
						"value": "Agregar entrada de tabla"
					},
					{
						"key": "@TabAddA@",
						"value": "Agregar una nueva entrada de tabla a la tabla seleccionada actualmente"
					},
					{
						"key": "@TabDel@",
						"value": "Eliminar entrada de tabla"
					},
					{
						"key": "@TabDelA@",
						"value": "Eliminar la entrada de la tabla seleccionada"
					},
					{
						"key": "@TabEdi@",
						"value": "Editar entrada de tabla"
					},
					{
						"key": "@TabMan@",
						"value": "Mantener tablas"
					},
					{
						"key": "@TabManA@",
						"value": "Mantener el contenido de cada tabla de referencia."
					},
					{
						"key": "@TabTab@",
						"value": "Mantener tablas"
					},
					{
						"key": "@TesAdd@",
						"value": "Agregar patrón de prueba"
					},
					{
						"key": "@TesAddA@",
						"value": "Agregar un nuevo patrón de prueba"
					},
					{
						"key": "@TesAddB@",
						"value": "Configuración general"
					},
					{
						"key": "@TesAddC@",
						"value": "Asigne un nombre al patrón de prueba y establezca su aplicabilidad"
					},
					{
						"key": "@TesAddD@",
						"value": "Agregar antibiótico"
					},
					{
						"key": "@TesAddE@",
						"value": "Agregar antibióticos"
					},
					{
						"key": "@TesAddF@",
						"value": "Especifique el antibiótico, la dosis y el método de prueba para cada componente del patrón de prueba."
					},
					{
						"key": "@TesAddG@",
						"value": "Agregar nuevas pruebas para la muestra"
					},
					{
						"key": "@TesAddH@",
						"value": "Agregar nuevas pruebas de cultivo"
					},
					{
						"key": "@TesAddI@",
						"value": "Sin contenido de patrón de prueba"
					},
					{
						"key": "@TesAddJ@",
						"value": "Deben ingresarse las pautas"
					},
					{
						"key": "@TesAfb@",
						"value": "Cantidad AFB"
					},
					{
						"key": "@TesApi@",
						"value": "Prueba del panel API"
					},
					{
						"key": "@TesApiA@",
						"value": "Se debe ingresar al Panel de ID de API"
					},
					{
						"key": "@TesApiB@",
						"value": "Panel de API"
					},
					{
						"key": "@TesAur@",
						"value": "Tb - Auramina"
					},
					{
						"key": "@TesAurRes@",
						"value": "Resultado TB - Auramina"
					},
					{
						"key": "@TesBet@",
						"value": "Prueba de betalactamasa"
					},
					{
						"key": "@TesBetA@",
						"value": "Betalactamasa"
					},
					{
						"key": "@TesBetRes@",
						"value": "Resultado de betalactamasa"
					},
					{
						"key": "@TesBio@",
						"value": "Prueba de bioquímica"
					},
					{
						"key": "@TesBioA@",
						"value": "Bioquímica"
					},
					{
						"key": "@TesCar@",
						"value": "Realizar pruebas directas"
					},
					{
						"key": "@TesCarA@",
						"value": "Realizar pruebas de cultivo"
					},
					{
						"key": "@TesCarB@",
						"value": "Prueba de carbapenemasa"
					},
					{
						"key": "@TesCarC@",
						"value": "Carbapenemasa"
					},
					{
						"key": "@TesCel@",
						"value": "Prueba de recuento de células"
					},
					{
						"key": "@TesDel@",
						"value": "Eliminar prueba de muestra"
					},
					{
						"key": "@TesDelA@",
						"value": "Eliminar patrón de prueba"
					},
					{
						"key": "@TesDelB@",
						"value": "Eliminar un patrón de prueba"
					},
					{
						"key": "@TesDelC@",
						"value": "Eliminar prueba de cultivo"
					},
					{
						"key": "@TesDip@",
						"value": "Varilla de nivel de orina"
					},
					{
						"key": "@TesEdi@",
						"value": "Editar antibióticos"
					},
					{
						"key": "@TesEdiA@",
						"value": "Editar componentes antibióticos del patrón de prueba"
					},
					{
						"key": "@TesEdiB@",
						"value": "Editar configuración general"
					},
					{
						"key": "@TesEdiC@",
						"value": "Editar las características generales del patrón de prueba"
					},
					{
						"key": "@TesEdiD@",
						"value": "Editar patrón de prueba"
					},
					{
						"key": "@TesEnt@",
						"value": "Ingrese los detalles para una prueba de recuento de células"
					},
					{
						"key": "@TesEntA@",
						"value": "Entrar en pruebas directas"
					},
					{
						"key": "@TesEntB@",
						"value": "Ingrese pruebas directas para esta muestra"
					},
					{
						"key": "@TesEntC@",
						"value": "Ingrese los detalles para una prueba de tinción de Gram"
					},
					{
						"key": "@TesEntD@",
						"value": "Ingrese los detalles para una prueba de tinta china"
					},
					{
						"key": "@TesEntE@",
						"value": "Ingrese los detalles para una prueba de preparación húmeda"
					},
					{
						"key": "@TesEntF@",
						"value": "Ingrese los detalles para una prueba de tinción de ZN"
					},
					{
						"key": "@TesEntG@",
						"value": "Ingrese los detalles para una prueba de embarazo"
					},
					{
						"key": "@TesEntH@",
						"value": "Ingrese Tb como organismos vistos"
					},
					{
						"key": "@TesEntI@",
						"value": "Ingrese los detalles para una prueba de antígeno de H. pylori"
					},
					{
						"key": "@TesEntJ@",
						"value": "Ingrese los detalles para una prueba de serología JEV"
					},
					{
						"key": "@TesEntK@",
						"value": "Ingrese los detalles para una prueba de tinción de Wrights"
					},
					{
						"key": "@TesEntL@",
						"value": "Ingrese los detalles para una prueba de preparación húmeda para hongos"
					},
					{
						"key": "@TesEntM@",
						"value": "Ingrese los detalles para una prueba de bioquímica"
					},
					{
						"key": "@TesEntN@",
						"value": "Ingrese los detalles para una prueba de microscopía"
					},
					{
						"key": "@TesEntO@",
						"value": "Ingrese los detalles para una prueba de varilla medidora"
					},
					{
						"key": "@TesEntP@",
						"value": "Entrar en pruebas de cultivo"
					},
					{
						"key": "@TesEntQ@",
						"value": "Ingrese pruebas para una cultura"
					},
					{
						"key": "@TesEntR@",
						"value": "Ingrese los detalles para una prueba Esbl"
					},
					{
						"key": "@TesEntS@",
						"value": "Ingrese los detalles para una prueba de betalactamasa"
					},
					{
						"key": "@TesEntT@",
						"value": "Ingrese los detalles para una prueba de carbapenemasa"
					},
					{
						"key": "@TesEntU@",
						"value": "Ingrese los detalles para una prueba del Panel Api"
					},
					{
						"key": "@TesEpi@",
						"value": "Células Epi"
					},
					{
						"key": "@TesEpiA@",
						"value": "Epitelio"
					},
					{
						"key": "@TesEsb@",
						"value": "Prueba Esbl"
					},
					{
						"key": "@TesEsbA@",
						"value": "Esbl"
					},
					{
						"key": "@TesEsbRes@",
						"value": "Resultado ESBL"
					},
					{
						"key": "@TesFun@",
						"value": "Preparación húmeda para hongos"
					},
					{
						"key": "@TesFunA@",
						"value": "Preparación húmeda para hongos"
					},
					{
						"key": "@TesFunB@",
						"value": "Se debe ingresar el hongo"
					},
					{
						"key": "@TesFunC@",
						"value": "Tipo de elemento fúngico"
					},
					{
						"key": "@TesGlu@",
						"value": "Glucosa (mM / L)"
					},
					{
						"key": "@TesGluA@",
						"value": "Glucosa"
					},
					{
						"key": "@TesGluB@",
						"value": "Debe introducirse glucosa"
					},
					{
						"key": "@TesGra@",
						"value": "Prueba de tinción de Gram"
					},
					{
						"key": "@TesGraWbc@",
						"value": "WBC tinción de Gram"
					},
					{
						"key": "@TesGui@",
						"value": "Pautas"
					},
					{
						"key": "@TesHpy@",
						"value": "Prueba de antígeno de H. pylori"
					},
					{
						"key": "@TesHpyA@",
						"value": "H. pylori Ant."
					},
					{
						"key": "@TesIdp@",
						"value": "Se debe ingresar el perfil de identificación"
					},
					{
						"key": "@TesInd@",
						"value": "Prueba de tinta china"
					},
					{
						"key": "@TesIndA@",
						"value": "Resultado de tinta china"
					},
					{
						"key": "@TesIndPos@",
						"value": "Resultado positivo tinta china"
					},
					{
						"key": "@TesJev@",
						"value": "Prueba de serología JEV"
					},
					{
						"key": "@TesJevA@",
						"value": "Serología JEV"
					},
					{
						"key": "@TesKet@",
						"value": "Cetonas"
					},
					{
						"key": "@TesLeu@",
						"value": "Leucocitos"
					},
					{
						"key": "@TesMan@",
						"value": "Seleccionar pruebas"
					},
					{
						"key": "@TesManA@",
						"value": "Administrar patrones de prueba"
					},
					{
						"key": "@TesManB@",
						"value": "Administrar la lista de patrones de prueba"
					},
					{
						"key": "@TesMic@",
						"value": "Microscopía directa"
					},
					{
						"key": "@TesMicA@",
						"value": "Microscopía"
					},
					{
						"key": "@TesMon@",
						"value": "Mononuclear (%)"
					},
					{
						"key": "@TesNit@",
						"value": "Nitritos"
					},
					{
						"key": "@TesOxiRes@",
						"value": "Resultado de oxidasa"
					},
					{
						"key": "@TesPar@",
						"value": "Parásitos"
					},
					{
						"key": "@TesParA@",
						"value": "Parásito"
					},
					{
						"key": "@TesPat@",
						"value": "Nombre del patrón de prueba"
					},
					{
						"key": "@TesPer@",
						"value": "Se debe ingresar el ID de porcentaje"
					},
					{
						"key": "@TesPh@",
						"value": "pH"
					},
					{
						"key": "@TesPol@",
						"value": "Polimorfonuclear (%)"
					},
					{
						"key": "@TesPos@",
						"value": "Resultado positivo"
					},
					{
						"key": "@TesPre@",
						"value": "Prueba de embarazo"
					},
					{
						"key": "@TesPreA@",
						"value": "El embarazo"
					},
					{
						"key": "@TesPreRes@",
						"value": "Resultado de embarazo"
					},
					{
						"key": "@TesPro@",
						"value": "Proteína (g / L)"
					},
					{
						"key": "@TesProA@",
						"value": "Proteína"
					},
					{
						"key": "@TesProB@",
						"value": "Se debe ingresar proteína"
					},
					{
						"key": "@TesRbc@",
						"value": "RBC (x10 ^ 6 / L)"
					},
					{
						"key": "@TesRbcA@",
						"value": "RBC cualitativo"
					},
					{
						"key": "@TesRbcB@",
						"value": "RBC"
					},
					{
						"key": "@TesSpe@",
						"value": "Gravedad específica"
					},
					{
						"key": "@TesTes@",
						"value": "Seleccionar pruebas para realizar"
					},
					{
						"key": "@TesTesA@",
						"value": "Resultado de la prueba"
					},
					{
						"key": "@TesTesB@",
						"value": "Probar selecciones para la cultura seleccionada"
					},
					{
						"key": "@TesUpd@",
						"value": "Actualizar la selección de prueba para la muestra"
					},
					{
						"key": "@TesUpdA@",
						"value": "Actualizar la selección de prueba para cultivo"
					},
					{
						"key": "@TesWbc@",
						"value": "WBC (x10 ^ 6 / L)"
					},
					{
						"key": "@TesWbcA@",
						"value": "WBC cualitativo"
					},
					{
						"key": "@TesWbcB@",
						"value": "WBC"
					},
					{
						"key": "@TesWet@",
						"value": "Preparación húmeda de heces"
					},
					{
						"key": "@TesWetWbc@",
						"value": "WBC preparación húmeda"
					},
					{
						"key": "@TesWri@",
						"value": "Prueba de tinción de Wright"
					},
					{
						"key": "@TesWriA@",
						"value": "Mancha de Wright"
					},
					{
						"key": "@TesWriRes@",
						"value": "Resultado tinción Wright"
					},
					{
						"key": "@TesZns@",
						"value": "Tinción ZN"
					},
					{
						"key": "@UseAdd@",
						"value": "Agregar un nuevo usuario"
					},
					{
						"key": "@UseAddA@",
						"value": "Agregar usuario"
					},
					{
						"key": "@UseAddB@",
						"value": "Agregar propiedades de usuario"
					},
					{
						"key": "@UseClo@",
						"value": "Clonar usuario"
					},
					{
						"key": "@UseCre@",
						"value": "Crear y administrar usuarios nuevos y existentes."
					},
					{
						"key": "@UseDel@",
						"value": "Borrar usuario"
					},
					{
						"key": "@UseEdi@",
						"value": "Editar un usuario existente"
					},
					{
						"key": "@UseEdiA@",
						"value": "editar usuario"
					},
					{
						"key": "@UseEit@",
						"value": "Se debe ingresar un laboratorio o una organización cliente"
					},
					{
						"key": "@UseEma@",
						"value": "Correo electrónico"
					},
					{
						"key": "@UseFir@",
						"value": "Primer nombre"
					},
					{
						"key": "@UseLas@",
						"value": "Apellido"
					},
					{
						"key": "@UseMan@",
						"value": "Administrar usuarios"
					},
					{
						"key": "@UsePas@",
						"value": "Debe ingresar la contraseña"
					},
					{
						"key": "@UsePasA@",
						"value": "Contraseña"
					},
					{
						"key": "@UseRol@",
						"value": "Deben introducirse roles"
					},
					{
						"key": "@UseSel@",
						"value": "Seleccionar laboratorio"
					},
					{
						"key": "@UseSelA@",
						"value": "Seleccione la organización del cliente"
					},
					{
						"key": "@UseUse@",
						"value": "Se debe ingresar el nombre de usuario"
					},
					{
						"key": "@UseUseA@",
						"value": "El usuario no puede pertenecer tanto a una organización cliente como a un laboratorio"
					},
					{
						"key": "@UseUseB@",
						"value": "Nombre de usuario"
					},
					{
						"key": "@UseUseC@",
						"value": "Propiedades de usuario"
					},
					{
						"key": "@ValAtl@",
						"value": "en línea"
					},
					{
						"key": "@ValMes@",
						"value": "debe ser ingresado"
					},
					{
						"key": "@ValOr@",
						"value": "Al menos un campo"
					},
					{
						"key": "@ZonDia@",
						"value": "Diámetro de la zona"
					}
				]
				""";
        }
    }
}
