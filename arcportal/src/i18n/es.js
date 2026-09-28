/**
 * Spanish (es) locale strings for the Fluent UI DatePicker.
 *
 * Values match Unicode CLDR data for the `es` locale, i.e. what
 * Intl.DateTimeFormat('es', ...) and .NET's CultureInfo("es") produce.
 * Month and weekday names are lowercase, which is correct Spanish
 * orthography and how CLDR ships them.
 */

const DayPickerStrings = {
    months: [
        'enero',
        'febrero',
        'marzo',
        'abril',
        'mayo',
        'junio',
        'julio',
        'agosto',
        'septiembre',
        'octubre',
        'noviembre',
        'diciembre',
    ],

    // CLDR abbreviated months — note 'sept', not 'sep'
    shortMonths: [
        'ene',
        'feb',
        'mar',
        'abr',
        'may',
        'jun',
        'jul',
        'ago',
        'sept',
        'oct',
        'nov',
        'dic',
    ],

    // Index 0 = Sunday, aligned to Date.getDay()
    days: [
        'domingo',
        'lunes',
        'martes',
        'miércoles',
        'jueves',
        'viernes',
        'sábado',
    ],

    shortDays: ['dom', 'lun', 'mar', 'mié', 'jue', 'vie', 'sáb'],

    goToToday: 'Ir a hoy',
    prevMonthAriaLabel: 'Ir al mes anterior',
    nextMonthAriaLabel: 'Ir al mes siguiente',
    prevYearAriaLabel: 'Ir al año anterior',
    nextYearAriaLabel: 'Ir al año siguiente',
};

const loginStrings = {
    username: 'Nombre de usuario',
    password: 'Contraseña',
    version: 'Versión',
};

export default DayPickerStrings;
export { loginStrings };
