import { NormalLines, NormalLinesNoPositionSave } from './NormalLines';

const sectionHeading = (lines, pdf) => {
    NormalLines(lines, pdf);
};

const inlineHeading = (lines, pdf) => {
    NormalLinesNoPositionSave(lines, pdf);
};

export { sectionHeading as SectionHeading, inlineHeading as InlineHeading };