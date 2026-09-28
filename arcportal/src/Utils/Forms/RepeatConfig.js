/**
 * Reads repeat configuration with PascalCase or camelCase property names from form config JSON.
 */

export const getRepeatFromPage = (repeat) => repeat?.FromPage ?? repeat?.fromPage ?? '';

export const getRepeatTitle = (repeat) => repeat?.Title ?? repeat?.title ?? '';

export const getRepeatPrompt = (repeat) => repeat?.Prompt ?? repeat?.prompt ?? '';
